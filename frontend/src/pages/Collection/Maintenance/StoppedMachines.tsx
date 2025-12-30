import React, { useState, useEffect } from 'react';
import styles from '../Restrictions/PlantRestriction.module.css';
import { StoppedMachine } from '../../../types/machineStatus';

interface Usina {
  id: number;
  nome: string;
}

interface UnidadeGeradora {
  id: number;
  nome: string;
  usinaId: number;
}

interface StoppedMachinesProps {
  onLoadUsinas: () => Promise<Usina[]>;
  onLoadUnidades: (usinaId: number) => Promise<UnidadeGeradora[]>;
  onLoadMachines: (filters: { dataPdp?: string; usinaId?: number }) => Promise<StoppedMachine[]>;
  onUpdateStatus: (machine: StoppedMachine) => Promise<void>;
}

const TIPOS_PARADA = ['PROGRAMADA', 'FORCADA', 'EMERGENCIAL'] as const;
const MOTIVOS_PARADA = [
  'Manutenção Preventiva',
  'Manutenção Corretiva',
  'Falha Mecânica',
  'Falha Elétrica',
  'Falta de Combustível',
  'Restrição Hidráulica',
  'Restrição Ambiental',
  'Teste de Equipamento',
  'Outros',
] as const;

const StoppedMachines: React.FC<StoppedMachinesProps> = ({
  onLoadUsinas,
  onLoadUnidades,
  onLoadMachines,
  onUpdateStatus,
}) => {
  const [machines, setMachines] = useState<StoppedMachine[]>([]);
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [unidades, setUnidades] = useState<UnidadeGeradora[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<StoppedMachine>>({
    dataPdp: '',
    usinaId: 0,
    unidadeGeradoraId: 0,
    potenciaGerada: 0,
    horaInicio: '',
    horaFim: '',
    observacao: '',
    motivoParada: '',
    tipoParada: 'PROGRAMADA',
  });
  const [filters, setFilters] = useState({
    dataPdp: '',
    usinaId: 0,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (filters.dataPdp || filters.usinaId) {
      loadMachinesList();
    }
  }, [filters]);

  useEffect(() => {
    if (formData.usinaId && formData.usinaId > 0) {
      loadUnidadesByUsina(formData.usinaId);
    }
  }, [formData.usinaId]);

  const loadData = async () => {
    try {
      setLoading(true);
      setError(null);
      const usinasData = await onLoadUsinas();
      setUsinas(usinasData);
    } catch (err) {
      setError('Erro ao carregar dados das usinas');
      console.error('Erro ao carregar usinas:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadMachinesList = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await onLoadMachines(filters);
      setMachines(data);
    } catch (err) {
      setError('Erro ao carregar máquinas paradas');
      console.error('Erro ao carregar máquinas:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadUnidadesByUsina = async (usinaId: number) => {
    try {
      const data = await onLoadUnidades(usinaId);
      setUnidades(data);
    } catch (err) {
      console.error('Erro ao carregar unidades:', err);
    }
  };

  const handleOpenDialog = (machine?: StoppedMachine) => {
    if (machine) {
      setEditingId(machine.id || null);
      setFormData(machine);
    } else {
      setEditingId(null);
      setFormData({
        dataPdp: '',
        usinaId: 0,
        unidadeGeradoraId: 0,
        potenciaGerada: 0,
        horaInicio: '',
        horaFim: '',
        observacao: '',
        motivoParada: '',
        tipoParada: 'PROGRAMADA',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingId(null);
    setFormData({
      dataPdp: '',
      usinaId: 0,
      unidadeGeradoraId: 0,
      potenciaGerada: 0,
      horaInicio: '',
      horaFim: '',
      observacao: '',
      motivoParada: '',
      tipoParada: 'PROGRAMADA',
    });
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!formData.dataPdp || !formData.usinaId || !formData.unidadeGeradoraId || !formData.motivoParada) {
        setError('Preencha todos os campos obrigatórios');
        return;
      }

      const usina = usinas.find(u => u.id === formData.usinaId);
      const unidade = unidades.find(u => u.id === formData.unidadeGeradoraId);

      const machineToSave: StoppedMachine = {
        ...formData,
        id: editingId || 0,
        usinaNome: usina?.nome || '',
        unidadeNome: unidade?.nome || '',
      } as StoppedMachine;

      await onUpdateStatus(machineToSave);
      setSuccessMessage('Parada registrada com sucesso');
      handleCloseDialog();
      await loadMachinesList();
    } catch (err) {
      setError('Erro ao registrar parada');
      console.error('Erro ao salvar:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleFilterChange = (field: string, value: string | number) => {
    setFilters(prev => ({ ...prev, [field]: value }));
  };

  const handleFormChange = (field: string, value: string | number) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  return (
    <div className={styles.container} data-testid="stopped-machines-container">
      <div className={styles.header}>
        <h2>Coleta - Máquinas Paradas</h2>
      </div>

      {error && (
        <div className={styles.alert} data-testid="error-message">
          {error}
        </div>
      )}

      {successMessage && (
        <div className={styles.success} data-testid="success-message">
          {successMessage}
        </div>
      )}

      <div className={styles.filters}>
        <div className={styles.formGroup}>
          <label htmlFor="filter-data-pdp">Data PDP:</label>
          <input
            type="date"
            id="filter-data-pdp"
            data-testid="filter-data-pdp"
            value={filters.dataPdp}
            onChange={(e) => handleFilterChange('dataPdp', e.target.value)}
          />
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="filter-usina">Usina:</label>
          <select
            id="filter-usina"
            data-testid="filter-usina"
            value={filters.usinaId}
            onChange={(e) => handleFilterChange('usinaId', Number(e.target.value))}
          >
            <option value={0}>Todas</option>
            {usinas.map(usina => (
              <option key={usina.id} value={usina.id}>
                {usina.nome}
              </option>
            ))}
          </select>
        </div>

        <button
          className={styles.btnPrimary}
          data-testid="btn-registrar-parada"
          onClick={() => handleOpenDialog()}
        >
          + Registrar Parada
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="machines-table">
          <thead>
            <tr>
              <th>Usina</th>
              <th>Unidade</th>
              <th>Tipo Parada</th>
              <th>Motivo</th>
              <th>Hora Início</th>
              <th>Hora Fim</th>
              <th>Observação</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {machines.length === 0 && !loading && (
              <tr>
                <td colSpan={8} className={styles.noData}>
                  Nenhuma máquina parada encontrada
                </td>
              </tr>
            )}
            {loading && (
              <tr>
                <td colSpan={8} className={styles.loading}>
                  Carregando...
                </td>
              </tr>
            )}
            {machines.map(machine => (
              <tr key={machine.id} data-testid={`machine-row-${machine.id}`}>
                <td>{machine.usinaNome}</td>
                <td>{machine.unidadeNome}</td>
                <td>
                  <span className={`${styles.badge} ${styles[`badge${machine.tipoParada}`]}`}>
                    {machine.tipoParada}
                  </span>
                </td>
                <td>{machine.motivoParada}</td>
                <td>{machine.horaInicio}</td>
                <td>{machine.horaFim || '-'}</td>
                <td>{machine.observacao}</td>
                <td>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-edit-${machine.id}`}
                    onClick={() => handleOpenDialog(machine)}
                    title="Editar"
                  >
                    ✏️
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {openDialog && (
        <div className={styles.modal} data-testid="dialog-form">
          <div className={styles.modalContent}>
            <div className={styles.modalHeader}>
              <h3>{editingId ? 'Editar Parada' : 'Registrar Parada'}</h3>
              <button
                className={styles.btnClose}
                data-testid="btn-close-dialog"
                onClick={handleCloseDialog}
              >
                ✕
              </button>
            </div>

            <div className={styles.modalBody}>
              <div className={styles.formGroup}>
                <label htmlFor="form-data-pdp">Data PDP:*</label>
                <input
                  type="date"
                  id="form-data-pdp"
                  data-testid="form-data-pdp"
                  value={formData.dataPdp}
                  onChange={(e) => handleFormChange('dataPdp', e.target.value)}
                  required
                />
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="form-usina">Usina:*</label>
                <select
                  id="form-usina"
                  data-testid="form-usina"
                  value={formData.usinaId}
                  onChange={(e) => handleFormChange('usinaId', Number(e.target.value))}
                  required
                >
                  <option value={0}>Selecione uma usina</option>
                  {usinas.map(usina => (
                    <option key={usina.id} value={usina.id}>
                      {usina.nome}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="form-unidade">Unidade:*</label>
                <select
                  id="form-unidade"
                  data-testid="form-unidade"
                  value={formData.unidadeGeradoraId}
                  onChange={(e) => handleFormChange('unidadeGeradoraId', Number(e.target.value))}
                  required
                  disabled={!formData.usinaId}
                >
                  <option value={0}>Selecione uma unidade</option>
                  {unidades.map(unidade => (
                    <option key={unidade.id} value={unidade.id}>
                      {unidade.nome}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label htmlFor="form-tipo-parada">Tipo de Parada:*</label>
                  <select
                    id="form-tipo-parada"
                    data-testid="form-tipo-parada"
                    value={formData.tipoParada}
                    onChange={(e) => handleFormChange('tipoParada', e.target.value)}
                    required
                  >
                    {TIPOS_PARADA.map(tipo => (
                      <option key={tipo} value={tipo}>
                        {tipo}
                      </option>
                    ))}
                  </select>
                </div>

                <div className={styles.formGroup}>
                  <label htmlFor="form-motivo-parada">Motivo da Parada:*</label>
                  <select
                    id="form-motivo-parada"
                    data-testid="form-motivo-parada"
                    value={formData.motivoParada}
                    onChange={(e) => handleFormChange('motivoParada', e.target.value)}
                    required
                  >
                    <option value="">Selecione o motivo</option>
                    {MOTIVOS_PARADA.map(motivo => (
                      <option key={motivo} value={motivo}>
                        {motivo}
                      </option>
                    ))}
                  </select>
                </div>
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label htmlFor="form-hora-inicio">Hora Início:*</label>
                  <input
                    type="time"
                    id="form-hora-inicio"
                    data-testid="form-hora-inicio"
                    value={formData.horaInicio}
                    onChange={(e) => handleFormChange('horaInicio', e.target.value)}
                    required
                  />
                </div>

                <div className={styles.formGroup}>
                  <label htmlFor="form-hora-fim">Hora Fim:</label>
                  <input
                    type="time"
                    id="form-hora-fim"
                    data-testid="form-hora-fim"
                    value={formData.horaFim}
                    onChange={(e) => handleFormChange('horaFim', e.target.value)}
                  />
                </div>
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="form-observacao">Observação:</label>
                <textarea
                  id="form-observacao"
                  data-testid="form-observacao"
                  value={formData.observacao}
                  onChange={(e) => handleFormChange('observacao', e.target.value)}
                  rows={3}
                />
              </div>
            </div>

            <div className={styles.modalFooter}>
              <button
                className={styles.btnSecondary}
                data-testid="btn-cancel"
                onClick={handleCloseDialog}
                disabled={loading}
              >
                Cancelar
              </button>
              <button
                className={styles.btnPrimary}
                data-testid="btn-save"
                onClick={handleSave}
                disabled={loading}
              >
                {loading ? 'Salvando...' : 'Salvar'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default StoppedMachines;

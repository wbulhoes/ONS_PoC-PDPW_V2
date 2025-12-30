import React, { useState, useEffect } from 'react';
import styles from '../Restrictions/PlantRestriction.module.css';

interface UnitMaintenance {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  unidadeGeradoraId: number;
  unidadeNome: string;
  tipoManutencao: string;
  dataInicio: string;
  dataFim: string;
  observacao: string;
  status: string;
}

interface Usina {
  id: number;
  nome: string;
}

interface UnidadeGeradora {
  id: number;
  nome: string;
  usinaId: number;
}

interface UnitMaintenanceProps {
  onLoadUsinas: () => Promise<Usina[]>;
  onLoadUnidades: (usinaId: number) => Promise<UnidadeGeradora[]>;
  onLoadMaintenances: (filters: any) => Promise<UnitMaintenance[]>;
  onSave: (maintenance: UnitMaintenance) => Promise<void>;
  onDelete: (id: number) => Promise<void>;
}

const UnitMaintenance: React.FC<UnitMaintenanceProps> = ({
  onLoadUsinas,
  onLoadUnidades,
  onLoadMaintenances,
  onSave,
  onDelete,
}) => {
  const [maintenances, setMaintenances] = useState<UnitMaintenance[]>([]);
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [unidades, setUnidades] = useState<UnidadeGeradora[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<UnitMaintenance>>({
    dataPdp: '',
    usinaId: 0,
    unidadeGeradoraId: 0,
    tipoManutencao: '',
    dataInicio: '',
    dataFim: '',
    observacao: '',
    status: 'PROGRAMADA',
  });
  const [filters, setFilters] = useState({
    dataPdp: '',
    usinaId: 0,
    status: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const tiposManutencao = [
    'PREVENTIVA',
    'CORRETIVA',
    'PREDITIVA',
    'EMERGENCIAL',
  ];

  const statusOptions = ['PROGRAMADA', 'EM_ANDAMENTO', 'CONCLUIDA', 'CANCELADA'];

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (filters.dataPdp || filters.usinaId || filters.status) {
      loadMaintenancesList();
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
      setError('Erro ao carregar dados');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const loadMaintenancesList = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await onLoadMaintenances(filters);
      setMaintenances(data);
    } catch (err) {
      setError('Erro ao carregar manutenções');
      console.error(err);
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

  const handleOpenDialog = (maintenance?: UnitMaintenance) => {
    if (maintenance) {
      setEditingId(maintenance.id || null);
      setFormData(maintenance);
    } else {
      setEditingId(null);
      setFormData({
        dataPdp: '',
        usinaId: 0,
        unidadeGeradoraId: 0,
        tipoManutencao: '',
        dataInicio: '',
        dataFim: '',
        observacao: '',
        status: 'PROGRAMADA',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingId(null);
    setUnidades([]);
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);
      
      if (!formData.dataPdp || !formData.usinaId || !formData.unidadeGeradoraId || !formData.tipoManutencao) {
        setError('Preencha todos os campos obrigatórios');
        return;
      }

      const usina = usinas.find(u => u.id === formData.usinaId);
      const unidade = unidades.find(u => u.id === formData.unidadeGeradoraId);
      const maintenanceToSave: UnitMaintenance = {
        ...formData,
        id: editingId || undefined,
        usinaNome: usina?.nome || '',
        unidadeNome: unidade?.nome || '',
      } as UnitMaintenance;

      await onSave(maintenanceToSave);
      setSuccessMessage(editingId ? 'Manutenção atualizada com sucesso' : 'Manutenção criada com sucesso');
      handleCloseDialog();
      await loadMaintenancesList();
    } catch (err) {
      setError('Erro ao salvar manutenção');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja realmente excluir esta manutenção?')) {
      return;
    }

    try {
      setLoading(true);
      setError(null);
      await onDelete(id);
      setSuccessMessage('Manutenção excluída com sucesso');
      await loadMaintenancesList();
    } catch (err) {
      setError('Erro ao excluir manutenção');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="unit-maintenance-container">
      <div className={styles.header}>
        <h2>Coleta - Manutenção de Unidades Geradoras</h2>
      </div>

      {error && <div className={styles.alert} data-testid="error-message">{error}</div>}
      {successMessage && <div className={styles.success} data-testid="success-message">{successMessage}</div>}

      <div className={styles.filters}>
        <div className={styles.formGroup}>
          <label htmlFor="filter-data-pdp">Data PDP:</label>
          <input
            type="date"
            id="filter-data-pdp"
            data-testid="filter-data-pdp"
            value={filters.dataPdp}
            onChange={(e) => setFilters(prev => ({ ...prev, dataPdp: e.target.value }))}
          />
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="filter-usina">Usina:</label>
          <select
            id="filter-usina"
            data-testid="filter-usina"
            value={filters.usinaId}
            onChange={(e) => setFilters(prev => ({ ...prev, usinaId: Number(e.target.value) }))}
          >
            <option value={0}>Todas</option>
            {usinas.map(usina => (
              <option key={usina.id} value={usina.id}>{usina.nome}</option>
            ))}
          </select>
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="filter-status">Status:</label>
          <select
            id="filter-status"
            data-testid="filter-status"
            value={filters.status}
            onChange={(e) => setFilters(prev => ({ ...prev, status: e.target.value }))}
          >
            <option value="">Todos</option>
            {statusOptions.map(status => (
              <option key={status} value={status}>{status}</option>
            ))}
          </select>
        </div>

        <button
          className={styles.btnPrimary}
          data-testid="btn-nova-manutencao"
          onClick={() => handleOpenDialog()}
        >
          + Nova Manutenção
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="maintenances-table">
          <thead>
            <tr>
              <th>Data PDP</th>
              <th>Usina</th>
              <th>Unidade</th>
              <th>Tipo</th>
              <th>Período</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {maintenances.length === 0 && !loading && (
              <tr>
                <td colSpan={7} className={styles.noData}>
                  Nenhuma manutenção encontrada
                </td>
              </tr>
            )}
            {loading && (
              <tr>
                <td colSpan={7} className={styles.loading}>
                  Carregando...
                </td>
              </tr>
            )}
            {maintenances.map(maintenance => (
              <tr key={maintenance.id} data-testid={`maintenance-row-${maintenance.id}`}>
                <td>{maintenance.dataPdp}</td>
                <td>{maintenance.usinaNome}</td>
                <td>{maintenance.unidadeNome}</td>
                <td>{maintenance.tipoManutencao}</td>
                <td>{maintenance.dataInicio} até {maintenance.dataFim}</td>
                <td>
                  <span className={`${styles.badge} ${styles[`badge${maintenance.status}`]}`}>
                    {maintenance.status}
                  </span>
                </td>
                <td>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-edit-${maintenance.id}`}
                    onClick={() => handleOpenDialog(maintenance)}
                    title="Editar"
                  >
                    ✏️
                  </button>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-delete-${maintenance.id}`}
                    onClick={() => handleDelete(maintenance.id!)}
                    title="Excluir"
                  >
                    🗑️
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
              <h3>{editingId ? 'Editar Manutenção' : 'Nova Manutenção'}</h3>
              <button className={styles.btnClose} data-testid="btn-close-dialog" onClick={handleCloseDialog}>
                ✕
              </button>
            </div>

            <div className={styles.modalBody}>
              <div className={styles.formGroup}>
                <label>Data PDP:*</label>
                <input
                  type="date"
                  data-testid="form-data-pdp"
                  value={formData.dataPdp}
                  onChange={(e) => setFormData(prev => ({ ...prev, dataPdp: e.target.value }))}
                />
              </div>

              <div className={styles.formGroup}>
                <label>Usina:*</label>
                <select
                  data-testid="form-usina"
                  value={formData.usinaId}
                  onChange={(e) => setFormData(prev => ({ ...prev, usinaId: Number(e.target.value) }))}
                >
                  <option value={0}>Selecione uma usina</option>
                  {usinas.map(usina => (
                    <option key={usina.id} value={usina.id}>{usina.nome}</option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Unidade Geradora:*</label>
                <select
                  data-testid="form-unidade"
                  value={formData.unidadeGeradoraId}
                  onChange={(e) => setFormData(prev => ({ ...prev, unidadeGeradoraId: Number(e.target.value) }))}
                  disabled={!formData.usinaId}
                >
                  <option value={0}>Selecione uma unidade</option>
                  {unidades.map(unidade => (
                    <option key={unidade.id} value={unidade.id}>{unidade.nome}</option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Tipo de Manutenção:*</label>
                <select
                  data-testid="form-tipo-manutencao"
                  value={formData.tipoManutencao}
                  onChange={(e) => setFormData(prev => ({ ...prev, tipoManutencao: e.target.value }))}
                >
                  <option value="">Selecione o tipo</option>
                  {tiposManutencao.map(tipo => (
                    <option key={tipo} value={tipo}>{tipo}</option>
                  ))}
                </select>
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label>Data Início:*</label>
                  <input
                    type="date"
                    data-testid="form-data-inicio"
                    value={formData.dataInicio}
                    onChange={(e) => setFormData(prev => ({ ...prev, dataInicio: e.target.value }))}
                  />
                </div>

                <div className={styles.formGroup}>
                  <label>Data Fim:*</label>
                  <input
                    type="date"
                    data-testid="form-data-fim"
                    value={formData.dataFim}
                    onChange={(e) => setFormData(prev => ({ ...prev, dataFim: e.target.value }))}
                  />
                </div>
              </div>

              <div className={styles.formGroup}>
                <label>Status:</label>
                <select
                  data-testid="form-status"
                  value={formData.status}
                  onChange={(e) => setFormData(prev => ({ ...prev, status: e.target.value }))}
                >
                  {statusOptions.map(status => (
                    <option key={status} value={status}>{status}</option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Observação:</label>
                <textarea
                  data-testid="form-observacao"
                  value={formData.observacao}
                  onChange={(e) => setFormData(prev => ({ ...prev, observacao: e.target.value }))}
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

export default UnitMaintenance;

import React, { useState, useEffect } from 'react';
import styles from './PlantRestriction.module.css';

interface PlantRestriction {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  tipoRestricao: string;
  dataInicio: string;
  dataFim: string;
  potenciaMaxima: number;
  potenciaMinima: number;
  observacao: string;
  status: string;
}

interface Usina {
  id: number;
  nome: string;
  sigla: string;
  tipo: string;
}

interface PlantRestrictionProps {
  onLoadUsinas: () => Promise<Usina[]>;
  onLoadRestrictions: (filters: { dataPdp?: string; usinaId?: number; status?: string }) => Promise<PlantRestriction[]>;
  onSave: (restriction: PlantRestriction) => Promise<void>;
  onDelete: (id: number) => Promise<void>;
}

const PlantRestriction: React.FC<PlantRestrictionProps> = ({
  onLoadUsinas,
  onLoadRestrictions,
  onSave,
  onDelete,
}) => {
  const [restrictions, setRestrictions] = useState<PlantRestriction[]>([]);
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<PlantRestriction>>({
    dataPdp: '',
    usinaId: 0,
    tipoRestricao: '',
    dataInicio: '',
    dataFim: '',
    potenciaMaxima: 0,
    potenciaMinima: 0,
    observacao: '',
    status: 'ATIVA',
  });
  const [filters, setFilters] = useState({
    dataPdp: '',
    usinaId: 0,
    status: '',
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const tiposRestricao = [
    'MANUTENCAO_PROGRAMADA',
    'FALHA_EQUIPAMENTO',
    'RESTRICAO_HIDROLOGICA',
    'RESTRICAO_AMBIENTAL',
    'RESTRICAO_OPERATIVA',
    'OUTROS',
  ];

  const statusOptions = ['ATIVA', 'INATIVA', 'CANCELADA'];

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (filters.dataPdp || filters.usinaId || filters.status) {
      loadRestrictionsList();
    }
  }, [filters]);

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

  const loadRestrictionsList = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await onLoadRestrictions(filters);
      setRestrictions(data);
    } catch (err) {
      setError('Erro ao carregar restrições');
      console.error('Erro ao carregar restrições:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (restriction?: PlantRestriction) => {
    if (restriction) {
      setEditingId(restriction.id || null);
      setFormData(restriction);
    } else {
      setEditingId(null);
      setFormData({
        dataPdp: '',
        usinaId: 0,
        tipoRestricao: '',
        dataInicio: '',
        dataFim: '',
        potenciaMaxima: 0,
        potenciaMinima: 0,
        observacao: '',
        status: 'ATIVA',
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
      tipoRestricao: '',
      dataInicio: '',
      dataFim: '',
      potenciaMaxima: 0,
      potenciaMinima: 0,
      observacao: '',
      status: 'ATIVA',
    });
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);
      
      if (!formData.dataPdp || !formData.usinaId || !formData.tipoRestricao) {
        setError('Preencha todos os campos obrigatórios');
        return;
      }

      const usina = usinas.find(u => u.id === formData.usinaId);
      const restrictionToSave: PlantRestriction = {
        ...formData,
        id: editingId || undefined,
        usinaNome: usina?.nome || '',
      } as PlantRestriction;

      await onSave(restrictionToSave);
      setSuccessMessage(editingId ? 'Restrição atualizada com sucesso' : 'Restrição criada com sucesso');
      handleCloseDialog();
      await loadRestrictionsList();
    } catch (err) {
      setError('Erro ao salvar restrição');
      console.error('Erro ao salvar:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja realmente excluir esta restrição?')) {
      return;
    }

    try {
      setLoading(true);
      setError(null);
      await onDelete(id);
      setSuccessMessage('Restrição excluída com sucesso');
      await loadRestrictionsList();
    } catch (err) {
      setError('Erro ao excluir restrição');
      console.error('Erro ao excluir:', err);
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
    <div className={styles.container} data-testid="plant-restriction-container">
      <div className={styles.header}>
        <h2>Coleta - Restrições de Usinas</h2>
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

        <div className={styles.formGroup}>
          <label htmlFor="filter-status">Status:</label>
          <select
            id="filter-status"
            data-testid="filter-status"
            value={filters.status}
            onChange={(e) => handleFilterChange('status', e.target.value)}
          >
            <option value="">Todos</option>
            {statusOptions.map(status => (
              <option key={status} value={status}>
                {status}
              </option>
            ))}
          </select>
        </div>

        <button
          className={styles.btnPrimary}
          data-testid="btn-nova-restricao"
          onClick={() => handleOpenDialog()}
        >
          + Nova Restrição
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="restrictions-table">
          <thead>
            <tr>
              <th>Data PDP</th>
              <th>Usina</th>
              <th>Tipo Restrição</th>
              <th>Período</th>
              <th>Potência (MW)</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {restrictions.length === 0 && !loading && (
              <tr>
                <td colSpan={7} className={styles.noData}>
                  Nenhuma restrição encontrada
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
            {restrictions.map(restriction => (
              <tr key={restriction.id} data-testid={`restriction-row-${restriction.id}`}>
                <td>{restriction.dataPdp}</td>
                <td>{restriction.usinaNome}</td>
                <td>{restriction.tipoRestricao}</td>
                <td>
                  {restriction.dataInicio} até {restriction.dataFim}
                </td>
                <td>
                  {restriction.potenciaMinima} - {restriction.potenciaMaxima}
                </td>
                <td>
                  <span className={`${styles.badge} ${styles[`badge${restriction.status}`]}`}>
                    {restriction.status}
                  </span>
                </td>
                <td>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-edit-${restriction.id}`}
                    onClick={() => handleOpenDialog(restriction)}
                    title="Editar"
                  >
                    ✏️
                  </button>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-delete-${restriction.id}`}
                    onClick={() => handleDelete(restriction.id!)}
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
              <h3>{editingId ? 'Editar Restrição' : 'Nova Restrição'}</h3>
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
                <label htmlFor="form-tipo-restricao">Tipo de Restrição:*</label>
                <select
                  id="form-tipo-restricao"
                  data-testid="form-tipo-restricao"
                  value={formData.tipoRestricao}
                  onChange={(e) => handleFormChange('tipoRestricao', e.target.value)}
                  required
                >
                  <option value="">Selecione o tipo</option>
                  {tiposRestricao.map(tipo => (
                    <option key={tipo} value={tipo}>
                      {tipo}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label htmlFor="form-data-inicio">Data Início:*</label>
                  <input
                    type="date"
                    id="form-data-inicio"
                    data-testid="form-data-inicio"
                    value={formData.dataInicio}
                    onChange={(e) => handleFormChange('dataInicio', e.target.value)}
                    required
                  />
                </div>

                <div className={styles.formGroup}>
                  <label htmlFor="form-data-fim">Data Fim:*</label>
                  <input
                    type="date"
                    id="form-data-fim"
                    data-testid="form-data-fim"
                    value={formData.dataFim}
                    onChange={(e) => handleFormChange('dataFim', e.target.value)}
                    required
                  />
                </div>
              </div>

              <div className={styles.formRow}>
                <div className={styles.formGroup}>
                  <label htmlFor="form-potencia-minima">Potência Mínima (MW):</label>
                  <input
                    type="number"
                    id="form-potencia-minima"
                    data-testid="form-potencia-minima"
                    value={formData.potenciaMinima}
                    onChange={(e) => handleFormChange('potenciaMinima', Number(e.target.value))}
                    min={0}
                    step={0.1}
                  />
                </div>

                <div className={styles.formGroup}>
                  <label htmlFor="form-potencia-maxima">Potência Máxima (MW):</label>
                  <input
                    type="number"
                    id="form-potencia-maxima"
                    data-testid="form-potencia-maxima"
                    value={formData.potenciaMaxima}
                    onChange={(e) => handleFormChange('potenciaMaxima', Number(e.target.value))}
                    min={0}
                    step={0.1}
                  />
                </div>
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="form-status">Status:</label>
                <select
                  id="form-status"
                  data-testid="form-status"
                  value={formData.status}
                  onChange={(e) => handleFormChange('status', e.target.value)}
                >
                  {statusOptions.map(status => (
                    <option key={status} value={status}>
                      {status}
                    </option>
                  ))}
                </select>
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

export default PlantRestriction;

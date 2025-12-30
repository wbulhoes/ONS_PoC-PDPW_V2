import React, { useState, useEffect } from 'react';
import styles from '../Restrictions/PlantRestriction.module.css';
import { UnitOutage as UnitOutageType, TIPOS_PARADA, STATUS_PARADA } from '../../../types/unitOutage';

interface Usina {
  id: number;
  nome: string;
}

interface UnidadeGeradora {
  id: number;
  nome: string;
  usinaId: number;
}

interface UnitOutageProps {
  onLoadUsinas: () => Promise<Usina[]>;
  onLoadUnidades: (usinaId: number) => Promise<UnidadeGeradora[]>;
  onLoadOutages: (filters: { dataPdp?: string; usinaId?: number; status?: string }) => Promise<UnitOutageType[]>;
  onSave: (outage: UnitOutageType) => Promise<void>;
  onDelete: (id: number) => Promise<void>;
}

const UnitOutage: React.FC<UnitOutageProps> = ({
  onLoadUsinas,
  onLoadUnidades,
  onLoadOutages,
  onSave,
  onDelete,
}) => {
  const [outages, setOutages] = useState<UnitOutageType[]>([]);
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [unidades, setUnidades] = useState<UnidadeGeradora[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<UnitOutageType>>({
    dataPdp: '',
    usinaId: 0,
    unidadeGeradoraId: 0,
    tipoParada: '',
    dataInicio: '',
    dataFim: '',
    motivoParada: '',
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

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    if (filters.dataPdp || filters.usinaId || filters.status) {
      loadOutagesList();
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

  const loadOutagesList = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await onLoadOutages(filters);
      setOutages(data);
    } catch (err) {
      setError('Erro ao carregar paradas');
      console.error('Erro ao carregar paradas:', err);
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

  const handleOpenDialog = (outage?: UnitOutageType) => {
    if (outage) {
      setEditingId(outage.id || null);
      setFormData(outage);
    } else {
      setEditingId(null);
      setFormData({
        dataPdp: '',
        usinaId: 0,
        unidadeGeradoraId: 0,
        tipoParada: '',
        dataInicio: '',
        dataFim: '',
        motivoParada: '',
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
      unidadeGeradoraId: 0,
      tipoParada: '',
      dataInicio: '',
      dataFim: '',
      motivoParada: '',
      observacao: '',
      status: 'ATIVA',
    });
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);

      if (!formData.dataPdp || !formData.usinaId || !formData.unidadeGeradoraId || !formData.tipoParada) {
        setError('Preencha todos os campos obrigatórios');
        return;
      }

      const usina = usinas.find(u => u.id === formData.usinaId);
      const unidade = unidades.find(u => u.id === formData.unidadeGeradoraId);

      const outageToSave: UnitOutageType = {
        ...formData,
        id: editingId || undefined,
        usinaNome: usina?.nome || '',
        unidadeNome: unidade?.nome || '',
      } as UnitOutageType;

      await onSave(outageToSave);
      setSuccessMessage(editingId ? 'Parada atualizada com sucesso' : 'Parada criada com sucesso');
      handleCloseDialog();
      await loadOutagesList();
    } catch (err) {
      setError('Erro ao salvar parada');
      console.error('Erro ao salvar:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja realmente excluir esta parada?')) {
      return;
    }

    try {
      setLoading(true);
      setError(null);
      await onDelete(id);
      setSuccessMessage('Parada excluída com sucesso');
      await loadOutagesList();
    } catch (err) {
      setError('Erro ao excluir parada');
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
    <div className={styles.container} data-testid="unit-outage-container">
      <div className={styles.header}>
        <h2>Coleta - Paradas de Unidades</h2>
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
            {STATUS_PARADA.map(status => (
              <option key={status} value={status}>
                {status}
              </option>
            ))}
          </select>
        </div>

        <button
          className={styles.btnPrimary}
          data-testid="btn-nova-parada"
          onClick={() => handleOpenDialog()}
        >
          + Nova Parada
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="outages-table">
          <thead>
            <tr>
              <th>Data PDP</th>
              <th>Usina</th>
              <th>Unidade</th>
              <th>Tipo</th>
              <th>Motivo</th>
              <th>Período</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {outages.length === 0 && !loading && (
              <tr>
                <td colSpan={8} className={styles.noData}>
                  Nenhuma parada encontrada
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
            {outages.map(outage => (
              <tr key={outage.id} data-testid={`outage-row-${outage.id}`}>
                <td>{outage.dataPdp}</td>
                <td>{outage.usinaNome}</td>
                <td>{outage.unidadeNome}</td>
                <td>{outage.tipoParada}</td>
                <td>{outage.motivoParada}</td>
                <td>
                  {outage.dataInicio} até {outage.dataFim}
                </td>
                <td>
                  <span className={`${styles.badge} ${styles[`badge${outage.status}`]}`}>
                    {outage.status}
                  </span>
                </td>
                <td>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-edit-${outage.id}`}
                    onClick={() => handleOpenDialog(outage)}
                    title="Editar"
                  >
                    ✏️
                  </button>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-delete-${outage.id}`}
                    onClick={() => handleDelete(outage.id!)}
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
              <h3>{editingId ? 'Editar Parada' : 'Nova Parada'}</h3>
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

              <div className={styles.formGroup}>
                <label htmlFor="form-tipo-parada">Tipo de Parada:*</label>
                <select
                  id="form-tipo-parada"
                  data-testid="form-tipo-parada"
                  value={formData.tipoParada}
                  onChange={(e) => handleFormChange('tipoParada', e.target.value)}
                  required
                >
                  <option value="">Selecione o tipo</option>
                  {TIPOS_PARADA.map(tipo => (
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

              <div className={styles.formGroup}>
                <label htmlFor="form-motivo-parada">Motivo da Parada:*</label>
                <input
                  type="text"
                  id="form-motivo-parada"
                  data-testid="form-motivo-parada"
                  value={formData.motivoParada}
                  onChange={(e) => handleFormChange('motivoParada', e.target.value)}
                  required
                />
              </div>

              <div className={styles.formGroup}>
                <label htmlFor="form-status">Status:</label>
                <select
                  id="form-status"
                  data-testid="form-status"
                  value={formData.status}
                  onChange={(e) => handleFormChange('status', e.target.value)}
                >
                  {STATUS_PARADA.map(status => (
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

export default UnitOutage;

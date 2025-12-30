import React, { useState, useEffect } from 'react';
import styles from '../Collection/Restrictions/PlantRestriction.module.css';

interface ElectricalDispatchReason {
  id?: number;
  codigo: string;
  descricao: string;
  ativo: boolean;
  dataInclusao?: string;
  dataAtualizacao?: string;
}

interface ElectricalDispatchReasonProps {
  onLoadReasons: () => Promise<ElectricalDispatchReason[]>;
  onSave: (reason: ElectricalDispatchReason) => Promise<void>;
  onDelete: (id: number) => Promise<void>;
}

const ElectricalDispatchReason: React.FC<ElectricalDispatchReasonProps> = ({
  onLoadReasons,
  onSave,
  onDelete,
}) => {
  const [reasons, setReasons] = useState<ElectricalDispatchReason[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [formData, setFormData] = useState<Partial<ElectricalDispatchReason>>({
    codigo: '',
    descricao: '',
    ativo: true,
  });
  const [filterAtivo, setFilterAtivo] = useState<string>('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    loadReasonsList();
  }, []);

  const loadReasonsList = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await onLoadReasons();
      setReasons(data);
    } catch (err) {
      setError('Erro ao carregar motivos de despacho');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleOpenDialog = (reason?: ElectricalDispatchReason) => {
    if (reason) {
      setEditingId(reason.id || null);
      setFormData(reason);
    } else {
      setEditingId(null);
      setFormData({
        codigo: '',
        descricao: '',
        ativo: true,
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingId(null);
    setFormData({
      codigo: '',
      descricao: '',
      ativo: true,
    });
    setError(null);
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);
      
      if (!formData.codigo || !formData.descricao) {
        setError('Preencha todos os campos obrigatórios');
        return;
      }

      const reasonToSave: ElectricalDispatchReason = {
        ...formData,
        id: editingId || undefined,
      } as ElectricalDispatchReason;

      await onSave(reasonToSave);
      setSuccessMessage(editingId ? 'Motivo atualizado com sucesso' : 'Motivo criado com sucesso');
      handleCloseDialog();
      await loadReasonsList();
    } catch (err) {
      setError('Erro ao salvar motivo de despacho');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja realmente excluir este motivo?')) {
      return;
    }

    try {
      setLoading(true);
      setError(null);
      await onDelete(id);
      setSuccessMessage('Motivo excluído com sucesso');
      await loadReasonsList();
    } catch (err) {
      setError('Erro ao excluir motivo de despacho');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const filteredReasons = reasons.filter(reason => {
    if (filterAtivo === '') return true;
    return filterAtivo === 'true' ? reason.ativo : !reason.ativo;
  });

  return (
    <div className={styles.container} data-testid="electrical-dispatch-reason-container">
      <div className={styles.header}>
        <h2>Cadastro - Motivos de Despacho Razão Elétrica</h2>
      </div>

      {error && <div className={styles.alert} data-testid="error-message">{error}</div>}
      {successMessage && <div className={styles.success} data-testid="success-message">{successMessage}</div>}

      <div className={styles.filters}>
        <div className={styles.formGroup}>
          <label htmlFor="filter-ativo">Status:</label>
          <select
            id="filter-ativo"
            data-testid="filter-ativo"
            value={filterAtivo}
            onChange={(e) => setFilterAtivo(e.target.value)}
          >
            <option value="">Todos</option>
            <option value="true">Ativo</option>
            <option value="false">Inativo</option>
          </select>
        </div>

        <button
          className={styles.btnPrimary}
          data-testid="btn-novo-motivo"
          onClick={() => handleOpenDialog()}
        >
          + Novo Motivo
        </button>
      </div>

      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="reasons-table">
          <thead>
            <tr>
              <th>Código</th>
              <th>Descrição</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {filteredReasons.length === 0 && !loading && (
              <tr>
                <td colSpan={4} className={styles.noData}>
                  Nenhum motivo encontrado
                </td>
              </tr>
            )}
            {loading && (
              <tr>
                <td colSpan={4} className={styles.loading}>
                  Carregando...
                </td>
              </tr>
            )}
            {filteredReasons.map(reason => (
              <tr key={reason.id} data-testid={`reason-row-${reason.id}`}>
                <td>{reason.codigo}</td>
                <td>{reason.descricao}</td>
                <td>
                  <span className={`${styles.badge} ${reason.ativo ? styles.badgeATIVA : styles.badgeINATIVA}`}>
                    {reason.ativo ? 'Ativo' : 'Inativo'}
                  </span>
                </td>
                <td>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-edit-${reason.id}`}
                    onClick={() => handleOpenDialog(reason)}
                    title="Editar"
                  >
                    ✏️
                  </button>
                  <button
                    className={styles.btnIcon}
                    data-testid={`btn-delete-${reason.id}`}
                    onClick={() => handleDelete(reason.id!)}
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
              <h3>{editingId ? 'Editar Motivo' : 'Novo Motivo'}</h3>
              <button className={styles.btnClose} data-testid="btn-close-dialog" onClick={handleCloseDialog}>
                ✕
              </button>
            </div>

            <div className={styles.modalBody}>
              <div className={styles.formGroup}>
                <label>Código:*</label>
                <input
                  type="text"
                  data-testid="form-codigo"
                  value={formData.codigo}
                  onChange={(e) => setFormData(prev => ({ ...prev, codigo: e.target.value }))}
                  maxLength={10}
                  placeholder="Ex: RE001"
                />
              </div>

              <div className={styles.formGroup}>
                <label>Descrição:*</label>
                <textarea
                  data-testid="form-descricao"
                  value={formData.descricao}
                  onChange={(e) => setFormData(prev => ({ ...prev, descricao: e.target.value }))}
                  rows={3}
                  placeholder="Descreva o motivo de despacho"
                />
              </div>

              <div className={styles.formGroup}>
                <label>
                  <input
                    type="checkbox"
                    data-testid="form-ativo"
                    checked={formData.ativo}
                    onChange={(e) => setFormData(prev => ({ ...prev, ativo: e.target.checked }))}
                    style={{ marginRight: '8px' }}
                  />
                  Ativo
                </label>
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

export default ElectricalDispatchReason;

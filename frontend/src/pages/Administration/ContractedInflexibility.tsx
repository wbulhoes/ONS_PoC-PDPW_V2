import React, { useState, useEffect } from 'react';
import styles from './ContractedInflexibility.module.css';
import { ContractedInflexibility } from '../../types/contractedInflexibility';
import { contractedInflexibilityService } from '../../services/contractedInflexibilityService';

const ContractedInflexibilityPage: React.FC = () => {
  const [data, setData] = useState<ContractedInflexibility[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedIds, setSelectedIds] = useState<string[]>([]);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalMode, setModalMode] = useState<'create' | 'edit'>('create');
  const [currentItem, setCurrentItem] = useState<Partial<ContractedInflexibility>>({});

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const result = await contractedInflexibilityService.getAll();
      setData(result);
    } catch (err) {
      setError('Erro ao carregar dados.');
    } finally {
      setLoading(false);
    }
  };

  const handleSelect = (id: string) => {
    setSelectedIds(prev => {
      if (prev.includes(id)) {
        return prev.filter(item => item !== id);
      } else {
        return [...prev, id];
      }
    });
  };

  const handleOpenModal = (mode: 'create' | 'edit') => {
    if (mode === 'edit') {
      if (selectedIds.length !== 1) {
        alert('Selecione apenas um item para alterar.');
        return;
      }
      const itemToEdit = data.find(item => item.id === selectedIds[0]);
      if (itemToEdit) {
        setCurrentItem({ ...itemToEdit });
      }
    } else {
      setCurrentItem({
        habilitado: true,
        contrato: 'Posterior a 2011'
      });
    }
    setModalMode(mode);
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
    setCurrentItem({});
  };

  const handleSave = async () => {
    try {
      if (modalMode === 'create') {
        await contractedInflexibilityService.create(currentItem as Omit<ContractedInflexibility, 'id'>);
      } else {
        if (currentItem.id) {
          await contractedInflexibilityService.update(currentItem.id, currentItem);
        }
      }
      handleCloseModal();
      loadData();
      setSelectedIds([]);
    } catch (err) {
      alert('Erro ao salvar.');
    }
  };

  const handleDelete = async () => {
    if (selectedIds.length === 0) {
      alert('Selecione itens para excluir.');
      return;
    }
    if (window.confirm('Tem certeza que deseja excluir os itens selecionados?')) {
      try {
        for (const id of selectedIds) {
          await contractedInflexibilityService.delete(id);
        }
        loadData();
        setSelectedIds([]);
      } catch (err) {
        alert('Erro ao excluir.');
      }
    }
  };

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Inflexibilidade Contratada</h1>

      <div className={styles.actions}>
        <button className={`${styles.button} ${styles.buttonPrimary}`} onClick={() => handleOpenModal('create')} data-testid="btn-incluir">
          Incluir
        </button>
        <button className={`${styles.button} ${styles.buttonSecondary}`} onClick={() => handleOpenModal('edit')} data-testid="btn-alterar">
          Alterar
        </button>
        <button className={`${styles.button} ${styles.buttonDanger}`} onClick={handleDelete} data-testid="btn-excluir">
          Excluir
        </button>
      </div>

      {loading && <p>Carregando...</p>}
      {error && <p className={styles.errorMessage}>{error}</p>}

      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th></th>
              <th>Usina</th>
              <th>Início Vigência</th>
              <th>Fim Vigência</th>
              <th>Valor</th>
              <th>Habilitado</th>
              <th>Contrato</th>
            </tr>
          </thead>
          <tbody>
            {data.map(item => (
              <tr key={item.id}>
                <td>
                  <input
                    type="checkbox"
                    checked={selectedIds.includes(item.id)}
                    onChange={() => handleSelect(item.id)}
                    data-testid={`checkbox-${item.id}`}
                  />
                </td>
                <td>{item.nomeUsina}</td>
                <td>{new Date(item.dataInicio).toLocaleDateString('pt-BR')}</td>
                <td>{new Date(item.dataFim).toLocaleDateString('pt-BR')}</td>
                <td>{item.valor}</td>
                <td>{item.habilitado ? 'Sim' : 'Não'}</td>
                <td>{item.contrato}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      {isModalOpen && (
        <div className={styles.modalOverlay}>
          <div className={styles.modalContent}>
            <div className={styles.modalHeader}>
              <span className={styles.modalTitle}>{modalMode === 'create' ? 'Incluir' : 'Alterar'} Inflexibilidade</span>
              <button className={styles.closeButton} onClick={handleCloseModal}>&times;</button>
            </div>
            <div className={styles.modalBody}>
              <div className={styles.formGroup}>
                <label>Usina:</label>
                <input
                  type="text"
                  value={currentItem.nomeUsina || ''}
                  onChange={e => setCurrentItem({ ...currentItem, nomeUsina: e.target.value })}
                  data-testid="input-usina"
                />
              </div>
              <div className={styles.formGroup}>
                <label>Início Vigência:</label>
                <input
                  type="date"
                  value={currentItem.dataInicio || ''}
                  onChange={e => setCurrentItem({ ...currentItem, dataInicio: e.target.value })}
                  data-testid="input-data-inicio"
                />
              </div>
              <div className={styles.formGroup}>
                <label>Fim Vigência:</label>
                <input
                  type="date"
                  value={currentItem.dataFim || ''}
                  onChange={e => setCurrentItem({ ...currentItem, dataFim: e.target.value })}
                  data-testid="input-data-fim"
                />
              </div>
              <div className={styles.formGroup}>
                <label>Valor:</label>
                <input
                  type="number"
                  value={currentItem.valor || ''}
                  onChange={e => setCurrentItem({ ...currentItem, valor: Number(e.target.value) })}
                  data-testid="input-valor"
                />
              </div>
              <div className={styles.formGroup}>
                <label>
                  <input
                    type="checkbox"
                    checked={currentItem.habilitado || false}
                    onChange={e => setCurrentItem({ ...currentItem, habilitado: e.target.checked })}
                    data-testid="input-habilitado"
                  />
                  Habilitado
                </label>
              </div>
              <div className={styles.formGroup}>
                <label>Contrato:</label>
                <select
                  value={currentItem.contrato || 'Posterior a 2011'}
                  onChange={e => setCurrentItem({ ...currentItem, contrato: e.target.value as any })}
                  data-testid="input-contrato"
                >
                  <option value="Posterior a 2011">Posterior a 2011</option>
                  <option value="Anterior a 2011">Anterior a 2011</option>
                </select>
              </div>
            </div>
            <div className={styles.modalActions}>
              <button className={`${styles.button} ${styles.buttonSecondary}`} onClick={handleCloseModal}>Cancelar</button>
              <button className={`${styles.button} ${styles.buttonPrimary}`} onClick={handleSave} data-testid="btn-salvar">Salvar</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ContractedInflexibilityPage;

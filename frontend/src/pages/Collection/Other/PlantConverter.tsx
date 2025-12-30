import React, { useState, useEffect } from 'react';
import styles from './PlantConverter.module.css';
import { PlantConverter } from '../../../types/plantConverter';
import { plantConverterService } from '../../../services/plantConverterService';

const PlantConverterPage: React.FC = () => {
  const [selectedAgent, setSelectedAgent] = useState<string>('1');
  const [selectedPlant, setSelectedPlant] = useState<string>('');
  const [selectedConverter, setSelectedConverter] = useState<string>('');
  const [lossPercent, setLossPercent] = useState<string>('');
  const [priority, setPriority] = useState<string>('');
  
  const [list, setList] = useState<PlantConverter[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [saving, setSaving] = useState<boolean>(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error', text: string } | null>(null);

  // Mock lists
  const agents = [
    { id: '1', name: 'Agente A' },
    { id: '2', name: 'Agente B' }
  ];

  const plants = [
    { id: '1', name: 'Usina A' },
    { id: '2', name: 'Usina B' }
  ];

  const converters = [
    { id: '101', name: 'Conversora X' },
    { id: '102', name: 'Conversora Y' }
  ];

  useEffect(() => {
    loadData();
  }, [selectedAgent]);

  const loadData = async () => {
    setLoading(true);
    try {
      const data = await plantConverterService.getByAgent(selectedAgent);
      setList(data);
    } catch (error) {
      console.error('Error loading data:', error);
      setMessage({ type: 'error', text: 'Erro ao carregar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    if (!selectedPlant || !selectedConverter || !lossPercent || !priority) {
      setMessage({ type: 'error', text: 'Preencha todos os campos.' });
      return;
    }

    setSaving(true);
    setMessage(null);
    try {
      const newItem = await plantConverterService.save({
        usinaId: selectedPlant,
        conversoraId: selectedConverter,
        perda: parseFloat(lossPercent),
        prioridade: parseInt(priority)
      });
      
      setList([...list, newItem]);
      setMessage({ type: 'success', text: 'Associação salva com sucesso!' });
      
      // Clear form
      setSelectedPlant('');
      setSelectedConverter('');
      setLossPercent('');
      setPriority('');
    } catch (error) {
      console.error('Error saving data:', error);
      setMessage({ type: 'error', text: 'Erro ao salvar dados.' });
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Tem certeza que deseja excluir?')) return;

    try {
      await plantConverterService.delete(id);
      setList(list.filter(item => item.id !== id));
      setMessage({ type: 'success', text: 'Item excluído com sucesso!' });
    } catch (error) {
      console.error('Error deleting data:', error);
      setMessage({ type: 'error', text: 'Erro ao excluir item.' });
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <div className={styles.cardHeader}>
          <h2 className={styles.cardTitle}>Usina x Conversora</h2>
        </div>

        {message && (
          <div className={`${styles.message} ${message.type === 'success' ? styles.success : styles.error}`}>
            {message.text}
          </div>
        )}

        <div className={styles.form}>
          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label htmlFor="agent">Empresa:</label>
              <select
                id="agent"
                className={styles.select}
                value={selectedAgent}
                onChange={(e) => setSelectedAgent(e.target.value)}
              >
                {agents.map(agent => (
                  <option key={agent.id} value={agent.id}>{agent.name}</option>
                ))}
              </select>
            </div>
          </div>

          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label htmlFor="plant">Usinas:</label>
              <select
                id="plant"
                className={styles.select}
                value={selectedPlant}
                onChange={(e) => setSelectedPlant(e.target.value)}
              >
                <option value="">Selecione...</option>
                {plants.map(plant => (
                  <option key={plant.id} value={plant.id}>{plant.name}</option>
                ))}
              </select>
            </div>
            <div className={styles.formGroup}>
              <label htmlFor="converter">Usina Conversora:</label>
              <select
                id="converter"
                className={styles.select}
                value={selectedConverter}
                onChange={(e) => setSelectedConverter(e.target.value)}
              >
                <option value="">Selecione...</option>
                {converters.map(conv => (
                  <option key={conv.id} value={conv.id}>{conv.name}</option>
                ))}
              </select>
            </div>
          </div>

          <div className={styles.formRow}>
            <div className={styles.formGroup}>
              <label htmlFor="loss">Percentual de Perda (%):</label>
              <input
                type="number"
                id="loss"
                className={styles.input}
                value={lossPercent}
                onChange={(e) => setLossPercent(e.target.value)}
                step="0.1"
              />
            </div>
            <div className={styles.formGroup}>
              <label htmlFor="priority">Prioridade:</label>
              <input
                type="number"
                id="priority"
                className={styles.input}
                value={priority}
                onChange={(e) => setPriority(e.target.value)}
              />
            </div>
            <button 
              className={`${styles.button} ${styles.saveButton}`}
              onClick={handleSave}
              disabled={saving}
            >
              {saving ? 'Salvando...' : 'Salvar'}
            </button>
          </div>
        </div>

        <div className={styles.tableContainer}>
          <table className={styles.table} data-testid="data-table">
            <thead>
              <tr>
                <th>Usina</th>
                <th>Conversora</th>
                <th>Perda %</th>
                <th>Prioridade</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr><td colSpan={5} style={{textAlign: 'center'}}>Carregando...</td></tr>
              ) : list.length === 0 ? (
                <tr><td colSpan={5} style={{textAlign: 'center'}}>Nenhum registro encontrado.</td></tr>
              ) : (
                list.map(item => (
                  <tr key={item.id}>
                    <td>{item.usinaNome}</td>
                    <td>{item.conversoraNome}</td>
                    <td>{item.perda}</td>
                    <td>{item.prioridade}</td>
                    <td>
                      <button 
                        className={styles.deleteButton}
                        onClick={() => handleDelete(item.id)}
                      >
                        Excluir
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default PlantConverterPage;

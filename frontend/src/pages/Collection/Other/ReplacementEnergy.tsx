import React, { useState, useEffect } from 'react';
import styles from './ReplacementEnergy.module.css';
import { ReplacementEnergy } from '../../../types/replacementEnergy';
import { replacementEnergyService } from '../../../services/replacementEnergyService';

const ReplacementEnergyPage: React.FC = () => {
  const [date, setDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedAgent, setSelectedAgent] = useState<string>('1');
  const [selectedPlant, setSelectedPlant] = useState<string>('');
  const [data, setData] = useState<ReplacementEnergy | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [saving, setSaving] = useState<boolean>(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error', text: string } | null>(null);

  // Mock lists
  const agents = [
    { id: '1', name: 'Agente A' },
    { id: '2', name: 'Agente B' }
  ];

  const plants = [
    { id: '1', name: 'Usina 1' },
    { id: '2', name: 'Usina 2' }
  ];

  useEffect(() => {
    if (selectedAgent && selectedPlant && date) {
      loadData();
    }
  }, [selectedAgent, selectedPlant, date]);

  const loadData = async () => {
    setLoading(true);
    setMessage(null);
    try {
      const result = await replacementEnergyService.getByDateAndAgent(date, selectedAgent);
      const plantData = result.find(d => d.usinaId === selectedPlant);
      
      if (plantData) {
        setData(plantData);
      } else {
        // Initialize empty data
        setData({
          id: '',
          data: date,
          agenteId: selectedAgent,
          usinaId: selectedPlant,
          valores: Array(48).fill(0)
        });
      }
    } catch (error) {
      console.error('Error loading data:', error);
      setMessage({ type: 'error', text: 'Erro ao carregar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleValueChange = (index: number, value: string) => {
    if (!data) return;
    
    const numValue = parseFloat(value) || 0;
    const newValues = [...data.valores];
    newValues[index] = numValue;
    
    setData({
      ...data,
      valores: newValues
    });
  };

  const handleSave = async () => {
    if (!data) return;
    
    setSaving(true);
    setMessage(null);
    try {
      await replacementEnergyService.save(data);
      setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
    } catch (error) {
      console.error('Error saving data:', error);
      setMessage({ type: 'error', text: 'Erro ao salvar dados.' });
    } finally {
      setSaving(false);
    }
  };

  const renderTable = () => {
    if (!data) return null;

    return (
      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Intervalo</th>
              <th>Valor (MWmed)</th>
            </tr>
          </thead>
          <tbody>
            {data.valores.map((value, index) => (
              <tr key={index}>
                <td>{index + 1}</td>
                <td>
                  <input
                    type="number"
                    className={styles.input}
                    value={value}
                    onChange={(e) => handleValueChange(index, e.target.value)}
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    );
  };

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <div className={styles.cardHeader}>
          <h2 className={styles.cardTitle}>Energia de Reposição por Período</h2>
        </div>

        {message && (
          <div className={`${styles.message} ${message.type === 'success' ? styles.success : styles.error}`} style={{ padding: '10px', marginBottom: '20px', borderRadius: '4px', backgroundColor: message.type === 'success' ? '#d4edda' : '#f8d7da', color: message.type === 'success' ? '#155724' : '#721c24' }}>
            {message.text}
          </div>
        )}

        <div className={styles.filterGroup}>
          <div className={styles.filterItem}>
            <label htmlFor="date">Data PDP:</label>
            <input
              type="date"
              id="date"
              className={styles.select}
              value={date}
              onChange={(e) => setDate(e.target.value)}
            />
          </div>

          <div className={styles.filterItem}>
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

          <div className={styles.filterItem}>
            <label htmlFor="plant">Usina:</label>
            <select
              id="plant"
              className={styles.select}
              value={selectedPlant}
              onChange={(e) => setSelectedPlant(e.target.value)}
            >
              <option value="">Selecione uma Usina</option>
              {plants.map(plant => (
                <option key={plant.id} value={plant.id}>{plant.name}</option>
              ))}
            </select>
          </div>
        </div>

        {loading ? (
          <div>Carregando...</div>
        ) : (
          <>
            {selectedPlant ? renderTable() : <div style={{ padding: '20px', textAlign: 'center', color: '#666' }}>Selecione uma usina para visualizar os dados.</div>}
            
            {selectedPlant && (
              <div className={styles.actions}>
                <button 
                  className={`${styles.button} ${styles.saveButton}`}
                  onClick={handleSave}
                  disabled={saving}
                >
                  {saving ? 'Salvando...' : 'Salvar'}
                </button>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  );
};

export default ReplacementEnergyPage;

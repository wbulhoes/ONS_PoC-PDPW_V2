import React, { useState, useEffect } from 'react';
import styles from './ObservationQuery.module.css';

interface ObservationData {
  date: string;
  observation: string;
}

const ObservationQuery: React.FC = () => {
  const [availableDates, setAvailableDates] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [observation, setObservation] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [loadingDates, setLoadingDates] = useState<boolean>(true);

  // Mock fetching available dates
  useEffect(() => {
    const fetchDates = async () => {
      setLoadingDates(true);
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 500));
        
        // Mock dates (YYYY-MM-DD format for value, but display will be formatted)
        const dates = [
          '2023-10-25',
          '2023-10-24',
          '2023-10-23',
          '2023-10-22',
          '2023-10-21'
        ];
        setAvailableDates(dates);
        if (dates.length > 0) {
          setSelectedDate(dates[0]);
        }
      } catch (error) {
        console.error('Error fetching dates:', error);
      } finally {
        setLoadingDates(false);
      }
    };

    fetchDates();
  }, []);

  const handleSearch = async () => {
    if (!selectedDate) return;

    setLoading(true);
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 800));
      
      // Mock response based on date
      const mockObservation = `Observações do dia ${formatDate(selectedDate)}:

1. Operação normal do sistema.
2. Restrição na usina X devido a manutenção programada.
3. Fluxo de potência dentro dos limites operacionais.
4. Previsão de carga atendida conforme programação.

-- Fim das observações --`;

      setObservation(mockObservation);
    } catch (error) {
      console.error('Error fetching observation:', error);
      setObservation('Erro ao carregar observações.');
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    if (!dateString) return '';
    const [year, month, day] = dateString.split('-');
    return `${day}/${month}/${year}`;
  };

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <div className={styles.header}>
          <h1 className={styles.title}>Consulta de Observação Diária</h1>
        </div>

        <div className={styles.filterSection}>
          <div className={styles.formGroup}>
            <label htmlFor="dateSelect" className={styles.label}>Data PDP:</label>
            <select
              id="dateSelect"
              className={styles.select}
              value={selectedDate}
              onChange={(e) => setSelectedDate(e.target.value)}
              disabled={loadingDates || loading}
            >
              {loadingDates ? (
                <option>Carregando datas...</option>
              ) : (
                availableDates.map(date => (
                  <option key={date} value={date}>
                    {formatDate(date)}
                  </option>
                ))
              )}
            </select>
          </div>
          
          <button 
            className={styles.button} 
            onClick={handleSearch}
            disabled={loading || loadingDates || !selectedDate}
          >
            {loading ? 'Pesquisando...' : 'Pesquisar'}
          </button>
        </div>

        <div className={styles.resultSection}>
          <label className={styles.label}>Observação:</label>
          <textarea
            className={styles.observationText}
            value={observation}
            readOnly
            placeholder="Selecione uma data e clique em Pesquisar para ver as observações."
          />
        </div>
      </div>
    </div>
  );
};

export default ObservationQuery;

import React, { useState, useEffect } from 'react';
import styles from './ProgrammingMilestoneQuery.module.css';

interface Milestone {
  id: number;
  time: string;
  description: string;
  isPublic: boolean;
  isMandatory: boolean;
  requester: string;
  responsible: string;
  status: 'Executed' | 'Pending';
  executionTime?: string;
}

const ProgrammingMilestoneQuery: React.FC = () => {
  const [availableDates, setAvailableDates] = useState<string[]>([]);
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [milestones, setMilestones] = useState<Milestone[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [loadingDates, setLoadingDates] = useState<boolean>(true);

  // Mock fetching available dates
  useEffect(() => {
    const fetchDates = async () => {
      setLoadingDates(true);
      try {
        // Simulate API call
        await new Promise(resolve => setTimeout(resolve, 500));
        
        const dates = [
          '2023-10-25',
          '2023-10-24',
          '2023-10-23'
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
      
      // Mock data based on date
      const mockMilestones: Milestone[] = [
        {
          id: 1,
          time: '08:00',
          description: 'Abertura do Dia',
          isPublic: true,
          isMandatory: true,
          requester: 'Sistema',
          responsible: 'Operador A',
          status: 'Executed',
          executionTime: '08:01'
        },
        {
          id: 2,
          time: '09:00',
          description: 'Coleta de Dados Hidráulicos',
          isPublic: true,
          isMandatory: true,
          requester: 'Sistema',
          responsible: 'Operador B',
          status: 'Executed',
          executionTime: '09:15'
        },
        {
          id: 3,
          time: '10:00',
          description: 'Validação de Dados',
          isPublic: false,
          isMandatory: true,
          requester: 'Gerente',
          responsible: 'Operador A',
          status: 'Pending'
        },
        {
          id: 4,
          time: '11:00',
          description: 'Rodada Preliminar DESSEM',
          isPublic: true,
          isMandatory: false,
          requester: 'Sistema',
          responsible: '-',
          status: 'Pending'
        }
      ];

      setMilestones(mockMilestones);
    } catch (error) {
      console.error('Error fetching milestones:', error);
      setMilestones([]);
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
          <h1 className={styles.title}>Consulta de Marcos de Programação</h1>
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
          {loading ? (
            <div className={styles.loading}>Carregando marcos...</div>
          ) : milestones.length > 0 ? (
            <div className={styles.timeline}>
              {milestones.map(milestone => (
                <div key={milestone.id} className={styles.milestoneItem}>
                  <div className={styles.milestoneTime}>{milestone.time}</div>
                  <div className={styles.milestoneContent}>
                    <div className={styles.milestoneHeader}>
                      <span className={styles.milestoneTitle}>{milestone.description}</span>
                      <span className={`${styles.milestoneStatus} ${milestone.status === 'Executed' ? styles.statusExecuted : styles.statusPending}`}>
                        {milestone.status === 'Executed' ? 'Executado' : 'Pendente'}
                      </span>
                    </div>
                    <div className={styles.milestoneDetails}>
                      <div>
                        <span className={styles.detailLabel}>Solicitante:</span>
                        {milestone.requester}
                      </div>
                      <div>
                        <span className={styles.detailLabel}>Responsável:</span>
                        {milestone.responsible}
                      </div>
                      <div>
                        <span className={styles.detailLabel}>Obrigatório:</span>
                        {milestone.isMandatory ? 'Sim' : 'Não'}
                      </div>
                      {milestone.executionTime && (
                        <div>
                          <span className={styles.detailLabel}>Executado às:</span>
                          {milestone.executionTime}
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className={styles.empty}>
              Selecione uma data e clique em Pesquisar para ver os marcos.
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ProgrammingMilestoneQuery;

import React, { useState, useEffect } from 'react';
import styles from './Balance.module.css';

interface BalanceData {
  interval: number;
  generation: number;
  load: number;
  interchange: number;
  closing: number;
}

const Balance: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [data, setData] = useState<BalanceData[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  // Mock data for companies
  const companies = [
    { id: '1', name: 'Empresa A' },
    { id: '2', name: 'Empresa B' },
    { id: '3', name: 'Empresa C' },
  ];

  // Mock fetch data
  useEffect(() => {
    if (selectedCompany && selectedDate) {
      setLoading(true);
      // Simulate API call
      setTimeout(() => {
        const mockData: BalanceData[] = Array.from({ length: 48 }, (_, i) => ({
          interval: i + 1,
          generation: Math.floor(Math.random() * 1000),
          load: Math.floor(Math.random() * 800),
          interchange: Math.floor(Math.random() * 200),
          closing: 0, // Calculated later or from API
        })).map((item) => ({
          ...item,
          closing: item.generation - item.load + item.interchange,
        }));
        setData(mockData);
        setLoading(false);
      }, 500);
    } else {
      setData([]);
    }
  }, [selectedCompany, selectedDate]);

  const getIntervalLabel = (interval: number) => {
    const startHour = Math.floor((interval - 1) / 2);
    const startMin = (interval - 1) % 2 === 0 ? '00' : '30';
    const endHour = Math.floor(interval / 2);
    const endMin = interval % 2 === 0 ? '00' : '30';

    const format = (h: number) => h.toString().padStart(2, '0');
    return `${format(startHour)}:${startMin} - ${format(endHour)}:${endMin}`;
  };

  const calculateAverage = (field: keyof BalanceData) => {
    if (data.length === 0) return 0;
    const sum = data.reduce((acc, item) => acc + item[field], 0);
    return (sum / data.length).toFixed(2);
  };

  const calculateTotal = (field: keyof BalanceData) => {
    if (data.length === 0) return 0;
    return data.reduce((acc, item) => acc + item[field], 0).toFixed(2);
  };

  return (
    <div className={styles.container} data-testid="balance-container">
      <div className={styles.header}>
        <h1 className={styles.title}>Coleta de Balanço</h1>
      </div>

      <div className={styles.filterSection}>
        <div className={styles.formGroup}>
          <label htmlFor="date-select" className={styles.label}>
            Data PDP:
          </label>
          <input
            type="date"
            id="date-select"
            className={styles.select}
            value={selectedDate}
            onChange={(e) => setSelectedDate(e.target.value)}
          />
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="company-select" className={styles.label}>
            Empresa:
          </label>
          <select
            id="company-select"
            className={styles.select}
            value={selectedCompany}
            onChange={(e) => setSelectedCompany(e.target.value)}
          >
            <option value="">Selecione uma empresa</option>
            {companies.map((company) => (
              <option key={company.id} value={company.id}>
                {company.name}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.buttonGroup}>
          <button className={styles.viewButton} disabled={!selectedCompany || loading}>
            Visualizar
          </button>
        </div>
      </div>

      {loading ? (
        <div>Carregando...</div>
      ) : (
        data.length > 0 && (
          <div className={styles.tableContainer}>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Intervalo</th>
                  <th>Geração</th>
                  <th>Carga</th>
                  <th>Intercâmbio</th>
                  <th>Fechamento</th>
                </tr>
              </thead>
              <tbody>
                {data.map((row) => (
                  <tr key={row.interval}>
                    <td className={styles.intervalCell}>{getIntervalLabel(row.interval)}</td>
                    <td>{row.generation}</td>
                    <td>{row.load}</td>
                    <td>{row.interchange}</td>
                    <td>{row.closing}</td>
                  </tr>
                ))}
                <tr className={styles.totalRow}>
                  <td className={styles.intervalCell}>Média</td>
                  <td>{calculateAverage('generation')}</td>
                  <td>{calculateAverage('load')}</td>
                  <td>{calculateAverage('interchange')}</td>
                  <td>{calculateAverage('closing')}</td>
                </tr>
                <tr className={styles.totalRow}>
                  <td className={styles.intervalCell}>Total</td>
                  <td>{calculateTotal('generation')}</td>
                  <td>{calculateTotal('load')}</td>
                  <td>{calculateTotal('interchange')}</td>
                  <td>{calculateTotal('closing')}</td>
                </tr>
              </tbody>
            </table>
          </div>
        )
      )}
    </div>
  );
};

export default Balance;

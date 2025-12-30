import React, { useState, useEffect } from 'react';
import styles from './GenerationQuery.module.css';

interface GenerationData {
  interval: number;
  hour: string;
  values: { [plantCode: string]: number };
  total: number;
}

interface Plant {
  code: string;
  name: string;
  order: number;
}

type DataSource = 'transferArea' | 'sent' | 'consolidated' | 'receivedDESSEM' | 'consistedDESSEM';

const GenerationQuery: React.FC = () => {
  const [pdpDate, setPdpDate] = useState<string>('');
  const [company, setCompany] = useState<string>('');
  const [dataSource, setDataSource] = useState<DataSource>('transferArea');
  const [availableDates, setAvailableDates] = useState<string[]>([]);
  const [companies, setCompanies] = useState<Array<{ code: string; name: string }>>([]);
  const [generationData, setGenerationData] = useState<GenerationData[]>([]);
  const [plants, setPlants] = useState<Plant[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadInitialData();
  }, []);

  const loadInitialData = async () => {
    try {
      // Carregar empresas
      const companiesResponse = await fetch('/api/companies');
      const companiesData = await companiesResponse.json();
      setCompanies(companiesData);

      // Carregar datas disponíveis
      const datesResponse = await fetch('/api/pdp/dates');
      const datesData = await datesResponse.json();
      setAvailableDates(datesData);
    } catch (err) {
      setError('Erro ao carregar dados iniciais');
      console.error(err);
    }
  };

  const loadGenerationData = async () => {
    if (!pdpDate || !company) {
      setError('Selecione data e empresa');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const endpoint = getEndpointByDataSource(dataSource);
      const response = await fetch(
        `/api/thermal/generation/${endpoint}?date=${pdpDate}&company=${company}`
      );

      if (!response.ok) {
        throw new Error('Erro ao carregar dados de geração');
      }

      const data = await response.json();
      setPlants(data.plants);
      setGenerationData(processGenerationData(data.intervals, data.plants));
    } catch (err) {
      setError('Erro ao carregar dados de geração térmica');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const getEndpointByDataSource = (source: DataSource): string => {
    const endpoints: Record<DataSource, string> = {
      transferArea: 'transfer-area',
      sent: 'sent',
      consolidated: 'consolidated',
      receivedDESSEM: 'received-dessem',
      consistedDESSEM: 'consisted-dessem'
    };
    return endpoints[source];
  };

  const processGenerationData = (intervals: any[], plantsData: Plant[]): GenerationData[] => {
    const data: GenerationData[] = [];
    
    for (let i = 1; i <= 48; i++) {
      const hour = formatInterval(i);
      const intervalData = intervals.find(int => int.interval === i) || {};
      
      const values: { [plantCode: string]: number } = {};
      let total = 0;

      plantsData.forEach(plant => {
        const value = intervalData[plant.code] || 0;
        values[plant.code] = value;
        total += value;
      });

      data.push({
        interval: i,
        hour,
        values,
        total
      });
    }

    return data;
  };

  const formatInterval = (interval: number): string => {
    const halfHour = interval - 1;
    const startHour = Math.floor(halfHour / 2);
    const startMinute = (halfHour % 2) * 30;
    const endHour = Math.floor((halfHour + 1) / 2);
    const endMinute = ((halfHour + 1) % 2) * 30;

    return `${String(startHour).padStart(2, '0')}:${String(startMinute).padStart(2, '0')}-${String(endHour).padStart(2, '0')}:${String(endMinute).padStart(2, '0')}`;
  };

  const calculateTotals = (): { [plantCode: string]: number } => {
    const totals: { [plantCode: string]: number } = {};
    
    plants.forEach(plant => {
      totals[plant.code] = generationData.reduce(
        (sum, interval) => sum + (interval.values[plant.code] || 0),
        0
      );
    });

    return totals;
  };

  const calculateAverages = (): { [plantCode: string]: number } => {
    const totals = calculateTotals();
    const averages: { [plantCode: string]: number } = {};
    
    plants.forEach(plant => {
      averages[plant.code] = totals[plant.code] / 48;
    });

    return averages;
  };

  const exportToExcel = () => {
    // Implementar exportação para Excel
    console.log('Exportar para Excel');
  };

  const totals = calculateTotals();
  const averages = calculateAverages();
  const grandTotal = Object.values(totals).reduce((sum, val) => sum + val, 0);
  const grandAverage = grandTotal / 48;

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>Consulta de Geração Térmica</h1>
      </div>

      <div className={styles.filters}>
        <div className={styles.filterGroup}>
          <label>Dados:</label>
          <div className={styles.radioGroup}>
            <label>
              <input
                type="radio"
                value="transferArea"
                checked={dataSource === 'transferArea'}
                onChange={(e) => setDataSource(e.target.value as DataSource)}
              />
              Área Transferência
            </label>
            <label>
              <input
                type="radio"
                value="sent"
                checked={dataSource === 'sent'}
                onChange={(e) => setDataSource(e.target.value as DataSource)}
              />
              Enviados
            </label>
            <label>
              <input
                type="radio"
                value="consolidated"
                checked={dataSource === 'consolidated'}
                disabled
                onChange={(e) => setDataSource(e.target.value as DataSource)}
              />
              Consolidados
            </label>
            <label>
              <input
                type="radio"
                value="receivedDESSEM"
                checked={dataSource === 'receivedDESSEM'}
                onChange={(e) => setDataSource(e.target.value as DataSource)}
              />
              Recebidos DESSEM
            </label>
            <label>
              <input
                type="radio"
                value="consistedDESSEM"
                checked={dataSource === 'consistedDESSEM'}
                onChange={(e) => setDataSource(e.target.value as DataSource)}
              />
              Consistidos DESSEM
            </label>
          </div>
        </div>

        <div className={styles.selectGroup}>
          <div className={styles.selectRow}>
            <label>Data do PDP:</label>
            <select 
              value={pdpDate} 
              onChange={(e) => setPdpDate(e.target.value)}
              className={styles.select}
            >
              <option value="">Selecione...</option>
              {availableDates.map(date => (
                <option key={date} value={date}>
                  {new Date(date).toLocaleDateString('pt-BR')}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.selectRow}>
            <label>Empresa:</label>
            <select 
              value={company} 
              onChange={(e) => setCompany(e.target.value)}
              className={styles.select}
            >
              <option value="">Selecione...</option>
              {companies.map(comp => (
                <option key={comp.code} value={comp.code}>
                  {comp.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className={styles.buttonGroup}>
          <button 
            onClick={loadGenerationData}
            disabled={loading || !pdpDate || !company}
            className={styles.btnVisualizar}
          >
            Visualizar
          </button>
          <button 
            onClick={exportToExcel}
            disabled={generationData.length === 0}
            className={styles.btnExport}
          >
            Exportar para Excel
          </button>
        </div>
      </div>

      {error && (
        <div className={styles.error}>
          {error}
        </div>
      )}

      {loading && (
        <div className={styles.loading}>
          Carregando dados...
        </div>
      )}

      {generationData.length > 0 && !loading && (
        <div className={styles.tableContainer}>
          <table className={styles.dataTable}>
            <thead>
              <tr className={styles.headerRow}>
                <th>Intervalo</th>
                <th>Total</th>
                {plants.map(plant => (
                  <th key={plant.code}>{plant.code}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {generationData.map((row, index) => (
                <tr 
                  key={row.interval}
                  className={index % 2 === 1 ? styles.evenRow : ''}
                >
                  <td>{row.hour}</td>
                  <td className={styles.totalCell}>{row.total.toFixed(2)}</td>
                  {plants.map(plant => (
                    <td key={plant.code}>
                      {row.values[plant.code]?.toFixed(2) || '0.00'}
                    </td>
                  ))}
                </tr>
              ))}
              <tr className={styles.totalRow}>
                <td><strong>Total</strong></td>
                <td className={styles.totalCell}>
                  <strong>{grandTotal.toFixed(2)}</strong>
                </td>
                {plants.map(plant => (
                  <td key={plant.code}>
                    <strong>{totals[plant.code]?.toFixed(2) || '0.00'}</strong>
                  </td>
                ))}
              </tr>
              <tr className={styles.averageRow}>
                <td><strong>Média</strong></td>
                <td className={styles.totalCell}>
                  <strong>{grandAverage.toFixed(2)}</strong>
                </td>
                {plants.map(plant => (
                  <td key={plant.code}>
                    <strong>{averages[plant.code]?.toFixed(2) || '0.00'}</strong>
                  </td>
                ))}
              </tr>
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default GenerationQuery;

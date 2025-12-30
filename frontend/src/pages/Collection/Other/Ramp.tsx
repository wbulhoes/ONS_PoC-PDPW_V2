import React, { useState, useEffect } from 'react';
import styles from './Ramp.module.css';
import { RampData, TableRowData, CompanyDTO, PlantDTO } from '../../../types/ramp';

/**
 * Componente para Coleta de Rampas de Geração
 * Registra a taxa de mudança de potência (MW/min) de usinas por intervalo de tempo
 * Rampas são restrições operacionais que limitam a velocidade de mudança de geração
 */
const Ramp: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(
    new Date().toISOString().split('T')[0]
  );
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [selectedPlant, setSelectedPlant] = useState<string>('');
  const [companies, setCompanies] = useState<CompanyDTO[]>([]);
  const [plants, setPlants] = useState<PlantDTO[]>([]);
  const [rampData, setRampData] = useState<RampData[]>([]);
  const [tableData, setTableData] = useState<TableRowData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [showTable, setShowTable] = useState(false);
  const [dataModified, setDataModified] = useState(false);

  // Carregar empresas ao montar componente
  useEffect(() => {
    const loadCompanies = async () => {
      try {
        setLoading(true);
        // TODO: Implementar chamada à API
        const mockCompanies: CompanyDTO[] = [
          { codEmpresa: '001', nomeEmpresa: 'Empresa Geradora A' },
          { codEmpresa: '002', nomeEmpresa: 'Empresa Geradora B' },
          { codEmpresa: '003', nomeEmpresa: 'Empresa Geradora C' },
        ];
        setCompanies(mockCompanies);
      } catch (err) {
        setError('Erro ao carregar empresas');
      } finally {
        setLoading(false);
      }
    };

    loadCompanies();
  }, []);

  // Carregar usinas quando empresa é selecionada
  useEffect(() => {
    const loadPlants = async () => {
      if (!selectedCompany) {
        setPlants([]);
        setSelectedPlant('');
        return;
      }

      try {
        setLoading(true);
        // TODO: Implementar chamada à API
        const mockPlants: PlantDTO[] = [
          { codUsina: 'UHE001', nomeUsina: 'Usina Hidroelétrica 1', ordem: 1 },
          { codUsina: 'UHE002', nomeUsina: 'Usina Hidroelétrica 2', ordem: 2 },
          { codUsina: 'UTE001', nomeUsina: 'Usina Termelétrica 1', ordem: 3 },
        ];
        setPlants(mockPlants);
        setSelectedPlant('');
      } catch (err) {
        setError('Erro ao carregar usinas');
      } finally {
        setLoading(false);
      }
    };

    loadPlants();
  }, [selectedCompany]);

  // Carregar dados de rampas
  const handleSearch = async () => {
    if (!selectedCompany || !selectedPlant || !selectedDate) {
      setError('Por favor, selecione empresa, usina e data');
      return;
    }

    try {
      setLoading(true);
      setError('');

      // TODO: Implementar chamada à API
      // Simular dados de rampas de geração (48 intervalos de 30 min)
      const mockData: RampData[] = Array.from({ length: 48 }, (_, i) => ({
        codUsina: selectedPlant,
        intRampa: i + 1,
        valRampaTran: Math.random() * 100 + 10, // MW/min
      }));

      setRampData(mockData);
      buildTableData(mockData);
      setShowTable(true);
      setDataModified(false);
    } catch (err) {
      setError('Erro ao carregar dados de rampas');
    } finally {
      setLoading(false);
    }
  };

  // Construir estrutura de tabela (48 intervalos de 30 minutos)
  const buildTableData = (data: RampData[]) => {
    const intervals = new Map<number, TableRowData>();

    data.forEach((item) => {
      const interval = item.intRampa;
      if (!intervals.has(interval)) {
        intervals.set(interval, {
          intervalo: interval,
          valores: {},
          total: 0,
          media: 0,
        });
      }

      const row = intervals.get(interval)!;
      row.valores[item.codUsina] = item.valRampaTran;
      row.total += item.valRampaTran;
    });

    // Calcular médias
    const rowsArray = Array.from(intervals.values()).map((row) => {
      const plantCount = Object.keys(row.valores).length;
      row.media = plantCount > 0 ? row.total / plantCount : 0;
      return row;
    });

    setTableData(rowsArray.sort((a, b) => a.intervalo - b.intervalo));
  };

  // Atualizar valor na tabela
  const handleValueChange = (intervalo: number, codUsina: string, value: number) => {
    if (value < 0) {
      setError('Valores de rampa devem ser não-negativos');
      return;
    }

    const updatedData = tableData.map((row) => {
      if (row.intervalo === intervalo) {
        const oldValue = row.valores[codUsina] || 0;
        const newTotal = row.total - oldValue + value;
        const plantCount = Object.keys(row.valores).length;
        const newMedia = plantCount > 0 ? newTotal / plantCount : 0;

        return {
          ...row,
          valores: { ...row.valores, [codUsina]: value },
          total: Math.round(newTotal * 100) / 100,
          media: Math.round(newMedia * 100) / 100,
        };
      }
      return row;
    });

    setTableData(updatedData);
    setDataModified(true);
  };

  // Salvar dados
  const handleSave = async () => {
    if (!dataModified) {
      setSuccessMessage('Nenhuma alteração para salvar');
      setTimeout(() => setSuccessMessage(''), 3000);
      return;
    }

    try {
      setLoading(true);
      setError('');

      // TODO: Implementar chamada à API para salvar
      console.log('Salvando dados de rampas:', tableData);

      setSuccessMessage('Dados de rampas salvos com sucesso!');
      setDataModified(false);
      setTimeout(() => setSuccessMessage(''), 3000);
    } catch (err) {
      setError('Erro ao salvar dados de rampas');
    } finally {
      setLoading(false);
    }
  };

  // Limpar dados
  const handleClear = () => {
    setTableData([]);
    setShowTable(false);
    setDataModified(false);
    setError('');
  };

  const plantCodes = tableData.length > 0 ? Object.keys(tableData[0].valores) : [];

  return (
    <div className={styles.container} data-testid="ramp-container">
      <div className={styles.card}>
        <h1 className={styles.title} data-testid="ramp-title">
          Coleta de Rampas de Geração
        </h1>
        <p className={styles.subtitle} data-testid="ramp-subtitle">
          Registro da taxa de mudança de potência (MW/min) de usinas por intervalo
        </p>

        {error && (
          <div className={styles.alert} data-testid="error-message">
            {error}
          </div>
        )}

        {successMessage && (
          <div className={styles.alertSuccess} data-testid="success-message">
            {successMessage}
          </div>
        )}

        <div className={styles.filterSection} data-testid="filter-section">
          <div className={styles.formGroup}>
            <label htmlFor="date" className={styles.label}>
              Data PDP:
            </label>
            <input
              type="date"
              id="date"
              value={selectedDate}
              onChange={(e) => {
                setSelectedDate(e.target.value);
                setShowTable(false);
              }}
              className={styles.select}
              data-testid="date-filter"
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="company" className={styles.label}>
              Empresa:
            </label>
            <select
              id="company"
              value={selectedCompany}
              onChange={(e) => {
                setSelectedCompany(e.target.value);
                setShowTable(false);
              }}
              className={styles.select}
              data-testid="company-filter"
              disabled={loading}
            >
              <option value="">Selecione uma empresa...</option>
              {companies.map((company) => (
                <option key={company.codEmpresa} value={company.codEmpresa}>
                  {company.nomeEmpresa}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="plant" className={styles.label}>
              Usina:
            </label>
            <select
              id="plant"
              value={selectedPlant}
              onChange={(e) => {
                setSelectedPlant(e.target.value);
                setShowTable(false);
              }}
              className={styles.select}
              data-testid="plant-filter"
              disabled={!selectedCompany || loading}
            >
              <option value="">Selecione uma usina...</option>
              {plants.map((plant) => (
                <option key={plant.codUsina} value={plant.codUsina}>
                  {plant.nomeUsina}
                </option>
              ))}
              {plants.length > 0 && <option value="all">Todas as Usinas</option>}
            </select>
          </div>

          <button
            className={styles.button}
            onClick={handleSearch}
            disabled={loading || !selectedCompany || !selectedPlant}
            data-testid="btn-search"
          >
            {loading ? 'Carregando...' : 'Pesquisar'}
          </button>
        </div>

        {showTable && tableData.length > 0 && (
          <div className={styles.tableWrapper} data-testid="table-wrapper">
            <div className={styles.info} data-testid="info-section">
              <p>
                <strong>Nota:</strong> Valores em MW/min. Rampas limitam a velocidade de mudança de potência.
              </p>
            </div>

            <div className={styles.tableScroll}>
              <table className={styles.table} data-testid="ramp-table">
                <thead>
                  <tr>
                    <th className={styles.headerInterval}>Intervalo (30min)</th>
                    {plantCodes.map((plant) => (
                      <th key={plant} className={styles.headerPlant}>
                        {plant}
                      </th>
                    ))}
                    <th className={styles.headerTotal}>Total</th>
                    <th className={styles.headerMedia}>Média</th>
                  </tr>
                </thead>
                <tbody>
                  {tableData.map((row) => (
                    <tr key={`row-${row.intervalo}`} data-testid={`table-row-${row.intervalo}`}>
                      <td className={styles.cellInterval}>
                        {String(Math.floor((row.intervalo - 1) / 2)).padStart(2, '0')}:
                        {((row.intervalo - 1) % 2) * 30 === 0 ? '00' : '30'}
                      </td>
                      {plantCodes.map((plant) => (
                        <td key={`cell-${row.intervalo}-${plant}`} className={styles.cellValue}>
                          <input
                            type="number"
                            step="0.01"
                            min="0"
                            value={row.valores[plant] || 0}
                            onChange={(e) =>
                              handleValueChange(row.intervalo, plant, parseFloat(e.target.value) || 0)
                            }
                            className={styles.input}
                            disabled={loading}
                            data-testid={`input-${row.intervalo}-${plant}`}
                          />
                        </td>
                      ))}
                      <td className={styles.cellTotal} data-testid={`total-${row.intervalo}`}>{row.total.toFixed(2)}</td>
                      <td className={styles.cellMedia} data-testid={`media-${row.intervalo}`}>{row.media.toFixed(2)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className={styles.actionButtons} data-testid="action-buttons">
              <button
                className={styles.buttonPrimary}
                onClick={handleSave}
                disabled={loading || !dataModified}
                data-testid="btn-save"
              >
                {loading ? 'Salvando...' : 'Salvar'}
              </button>
              <button
                className={styles.buttonSecondary}
                onClick={handleClear}
                disabled={loading}
                data-testid="btn-clear"
              >
                Limpar
              </button>
            </div>
          </div>
        )}

        {showTable && tableData.length === 0 && !loading && (
          <div className={styles.noData} data-testid="no-data-message">
            Nenhum dado disponível para os filtros selecionados
          </div>
        )}
      </div>
    </div>
  );
};

export default Ramp;

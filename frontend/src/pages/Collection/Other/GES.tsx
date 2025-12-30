import React, { useEffect, useMemo, useState } from 'react';
import styles from './GES.module.css';
import { CompanyDTO, PlantDTO, TableRowData, GESData } from '../../../types/ges';

// Mock estático para manter determinismo dos testes enquanto a API não está integrada
const mockCompanies: CompanyDTO[] = [
  { codEmpresa: 'E001', nomeEmpresa: 'Empresa Geradora Norte' },
  { codEmpresa: 'E002', nomeEmpresa: 'Empresa Geradora Sul' },
];

const mockPlants: PlantDTO[] = [
  { codUsina: 'UGE1', nomeUsina: 'Usina Geradora 1', ordem: 1 },
  { codUsina: 'UGE2', nomeUsina: 'Usina Geradora 2', ordem: 2 },
  { codUsina: 'UTE1', nomeUsina: 'Usina Termica 1', ordem: 3 },
];

const buildMockData = (plants: PlantDTO[], isAll: boolean): GESData[] => {
  const targets = isAll ? plants : plants.slice(0, 1);
  return targets.flatMap((plant, plantIndex) =>
    Array.from({ length: 48 }, (_, i) => ({
      datPdp: '',
      codEmpresa: '',
      codUsina: plant.codUsina,
      intGes: i + 1,
      valGesTran: Number(((i + 1) * (plantIndex + 1)).toFixed(2)),
    }))
  );
};

const formatIntervalLabel = (intervalo: number): string => {
  const hour = Math.floor((intervalo - 1) / 2)
    .toString()
    .padStart(2, '0');
  const minute = ((intervalo - 1) % 2) * 30 === 0 ? '00' : '30';
  return `${hour}:${minute}`;
};

const GES: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [selectedPlant, setSelectedPlant] = useState<string>('');
  const [companies, setCompanies] = useState<CompanyDTO[]>([]);
  const [plants, setPlants] = useState<PlantDTO[]>([]);
  const [tableData, setTableData] = useState<TableRowData[]>([]);
  const [columnTotals, setColumnTotals] = useState<Record<string, { total: number; media: number }>>({});
  const [grandTotal, setGrandTotal] = useState<number>(0);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');
  const [showTable, setShowTable] = useState<boolean>(false);
  const [dataModified, setDataModified] = useState<boolean>(false);

  useEffect(() => {
    setCompanies(mockCompanies);
  }, []);

  useEffect(() => {
    if (!selectedCompany) {
      setPlants([]);
      setSelectedPlant('');
      setShowTable(false);
      setTableData([]);
      setDataModified(false);
      return;
    }

    setPlants(mockPlants);
    setSelectedPlant('');
    setShowTable(false);
    setTableData([]);
    setDataModified(false);
  }, [selectedCompany]);

  const plantCodes = useMemo(() => {
    if (tableData.length === 0) return [] as string[];
    const firstRowPlants = Object.keys(tableData[0].valores);
    return firstRowPlants.sort();
  }, [tableData]);

  const toSafeNumber = (value: number | string): number => {
    const numeric = typeof value === 'number' ? value : Number(value);
    if (Number.isNaN(numeric)) return 0;
    return Math.max(0, numeric);
  };

  const recalcTotals = (rows: TableRowData[]) => {
    const totals: Record<string, { total: number; media: number }> = {};
    let overall = 0;

    rows.forEach((row) => {
      overall += row.total;
      Object.entries(row.valores).forEach(([plant, value]) => {
        if (!totals[plant]) {
          totals[plant] = { total: 0, media: 0 };
        }
        totals[plant].total += toSafeNumber(value);
      });
    });

    const intervalCount = rows.length || 1;
    Object.keys(totals).forEach((plant) => {
      totals[plant].media = totals[plant].total / intervalCount;
    });

    setGrandTotal(overall);
    setColumnTotals(totals);
  };

  const buildTableData = (data: GESData[]) => {
    const intervals = new Map<number, TableRowData>();

    data.forEach((item) => {
      if (!intervals.has(item.intGes)) {
        intervals.set(item.intGes, {
          intervalo: item.intGes,
          valores: {},
          total: 0,
          media: 0,
        });
      }

      const row = intervals.get(item.intGes)!;
      row.valores[item.codUsina] = item.valGesTran;
      row.total += item.valGesTran;
    });

    const rowsArray = Array.from(intervals.values())
      .sort((a, b) => a.intervalo - b.intervalo)
      .map((row) => {
        const count = Object.keys(row.valores).length || 1;
        return {
          ...row,
          total: Number(row.total.toFixed(2)),
          media: Number((row.total / count).toFixed(2)),
        };
      });

    setTableData(rowsArray);
    recalcTotals(rowsArray);
  };

  const handleSearch = async () => {
    if (!selectedCompany || !selectedPlant || !selectedDate) {
      setError('Selecione data, empresa e usina para continuar');
      return;
    }

    try {
      setLoading(true);
      setError('');
      setSuccessMessage('');

      const isAll = selectedPlant === 'all';
      const plantsForSearch = isAll
        ? plants
        : plants.filter((plant) => plant.codUsina === selectedPlant);

      const mockData = buildMockData(plantsForSearch, isAll);
      buildTableData(mockData);
      setShowTable(true);
      setDataModified(false);
    } catch (err) {
      setError('Erro ao carregar dados de GES');
    } finally {
      setLoading(false);
    }
  };

  const handleValueChange = (intervalo: number, codUsina: string, rawValue: string) => {
    const normalized = rawValue.replace(',', '.');
    const trimmed = normalized.trim();
    const hasNegativeSign = trimmed.startsWith('-');
    const parsed = Number(trimmed);

    let nextError = '';
    if (trimmed === '') {
      nextError = 'Valor de GES é obrigatório';
    } else if (hasNegativeSign || parsed < 0) {
      nextError = 'Valores de GES não podem ser negativos';
    } else if (Number.isNaN(parsed)) {
      nextError = 'Valor inválido de GES';
    }

    if (nextError) {
      setError(nextError);
    } else {
      setError('');
    }

    const updatedRows = tableData.map((row) => {
      if (row.intervalo !== intervalo) return row;

      const updatedValores = { ...row.valores, [codUsina]: trimmed };
      const total = Object.values(updatedValores).reduce((acc, curr) => acc + toSafeNumber(curr), 0);
      const media = total / Object.keys(updatedValores).length;

      return {
        ...row,
        valores: updatedValores,
        total: Number(total.toFixed(2)),
        media: Number(media.toFixed(2)),
      };
    });

    setTableData(updatedRows);
    recalcTotals(updatedRows);
    setDataModified(true);
  };

  const handleSave = async () => {
    if (!dataModified) {
      setSuccessMessage('Nenhuma alteração para salvar');
      setTimeout(() => setSuccessMessage(''), 2500);
      return;
    }

    try {
      setLoading(true);
      setError('');

      // TODO: substituir por chamada real à API quando disponível
      console.log('Salvando GES', tableData);

      setSuccessMessage('Dados de GES salvos com sucesso');
      setDataModified(false);
      setTimeout(() => setSuccessMessage(''), 2500);
    } catch (err) {
      setError('Erro ao salvar dados de GES');
    } finally {
      setLoading(false);
    }
  };

  const handleClear = () => {
    setTableData([]);
    setShowTable(false);
    setDataModified(false);
    setGrandTotal(0);
    setColumnTotals({});
    setError('');
    setSuccessMessage('');
  };

  const overallAverage = useMemo(() => {
    if (tableData.length === 0) return 0;
    return Number((grandTotal / tableData.length).toFixed(2));
  }, [grandTotal, tableData.length]);

  return (
    <div className={styles.container} data-testid="ges-container">
      <div className={styles.card}>
        <h1 className={styles.title} data-testid="ges-title">
          Coleta de Geração de Energia Substituição (GES)
        </h1>
        <p className={styles.subtitle} data-testid="ges-subtitle">
          Registro dos valores de GES por usina e intervalo de 30 minutos (48 intervalos)
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
              Data PDP
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
              Empresa
            </label>
            <select
              id="company"
              value={selectedCompany}
              onChange={(e) => setSelectedCompany(e.target.value)}
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
              Usina
            </label>
            <select
              id="plant"
              value={selectedPlant}
              onChange={(e) => setSelectedPlant(e.target.value)}
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
              {plants.length > 0 && <option value="all">Todas as usinas</option>}
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
                <strong>Notas:</strong> Valores em MW. Totais e médias são recalculados automaticamente ao
                editar qualquer célula.
              </p>
            </div>

            <div className={styles.tableScroll}>
              <table className={styles.table} data-testid="ges-table">
                <thead>
                  <tr>
                    <th className={styles.headerInterval}>Intervalo</th>
                    {plantCodes.map((plant) => (
                      <th key={`head-${plant}`} className={styles.headerPlant} data-testid={`header-${plant}`}>
                        {plant}
                      </th>
                    ))}
                    <th className={styles.headerTotal}>Total</th>
                    <th className={styles.headerMedia}>Média</th>
                  </tr>
                </thead>
                <tbody>
                  {tableData.map((row) => (
                    <tr key={`row-${row.intervalo}`} data-testid={`row-${row.intervalo}`}>
                      <td className={styles.cellInterval}>{formatIntervalLabel(row.intervalo)}</td>
                      {plantCodes.map((plant) => (
                        <td key={`cell-${row.intervalo}-${plant}`} className={styles.cellValue}>
                          <input
                            type="number"
                            step="0.01"
                            min="0"
                            value={`${row.valores[plant] ?? ''}`}
                            onChange={(e) => handleValueChange(row.intervalo, plant, e.target.value)}
                            className={styles.input}
                            data-testid={`input-${row.intervalo}-${plant}`}
                            disabled={loading}
                          />
                        </td>
                      ))}
                      <td className={styles.cellTotal} data-testid={`total-${row.intervalo}`}>
                        {row.total.toFixed(2)}
                      </td>
                      <td className={styles.cellMedia} data-testid={`media-${row.intervalo}`}>
                        {row.media.toFixed(2)}
                      </td>
                    </tr>
                  ))}
                </tbody>
                <tfoot className={styles.tableFooter}>
                  <tr data-testid="row-total">
                    <td>Total (MW)</td>
                    {plantCodes.map((plant) => (
                      <td key={`total-${plant}`} className={styles.cellTotal}>
                        {(columnTotals[plant]?.total ?? 0).toFixed(2)}
                      </td>
                    ))}
                    <td className={styles.cellTotal}>{grandTotal.toFixed(2)}</td>
                    <td className={styles.cellMedia}>--</td>
                  </tr>
                  <tr data-testid="row-average">
                    <td>Média (MW)</td>
                    {plantCodes.map((plant) => (
                      <td key={`avg-${plant}`} className={styles.cellMedia}>
                        {(columnTotals[plant]?.media ?? 0).toFixed(2)}
                      </td>
                    ))}
                    <td className={styles.cellMedia}>{overallAverage.toFixed(2)}</td>
                    <td className={styles.cellMedia}>--</td>
                  </tr>
                </tfoot>
              </table>
            </div>

            <div className={styles.summaryGrid}>
              <div className={styles.summaryCard} data-testid="summary-total">
                <p className={styles.summaryTitle}>Total Geral (MW)</p>
                <p className={styles.summaryValue}>{grandTotal.toFixed(2)}</p>
              </div>
              <div className={styles.summaryCard} data-testid="summary-average">
                <p className={styles.summaryTitle}>Média Geral (MW)</p>
                <p className={styles.summaryValue}>{overallAverage.toFixed(2)}</p>
              </div>
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
            Nenhum dado encontrado para os filtros selecionados
          </div>
        )}
      </div>
    </div>
  );
};

export default GES;

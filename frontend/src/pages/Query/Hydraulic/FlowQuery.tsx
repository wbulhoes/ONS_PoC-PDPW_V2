import React, { useState, useEffect } from 'react';
import styles from './FlowQuery.module.css';
import {
  FlowData,
  FlowQueryFilters,
  FlowQueryResponse,
  FlowAggregatedData,
  TipoVazao,
  FLOW_QUERY_CONSTANTS,
} from '../../../types/flowQuery';

/**
 * Componente para Consulta de Vazão Hidráulica
 * Permite consultar dados históricos de vazão com filtros avançados,
 * paginação e exportação para Excel
 */
const FlowQuery: React.FC = () => {
  const [filters, setFilters] = useState<FlowQueryFilters>({
    tipoVazao: TipoVazao.TOTAL,
  });
  
  const [queryResult, setQueryResult] = useState<FlowQueryResponse>({
    dados: [],
    total: 0,
    pagina: 1,
    itensPorPagina: FLOW_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE,
    totalPaginas: 0,
  });
  
  const [aggregatedData, setAggregatedData] = useState<FlowAggregatedData[]>([]);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [usinaOptions, setUsinaOptions] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [showFilters, setShowFilters] = useState(true);

  useEffect(() => {
    // Carregar opções de empresas
    setEmpresaOptions(['Empresa A', 'Empresa B', 'Empresa C', 'Todas']);
  }, []);

  const handleFilterChange = (field: keyof FlowQueryFilters, value: any) => {
    setFilters(prev => ({ ...prev, [field]: value }));
    
    // Se mudou a empresa, recarregar usinas
    if (field === 'codEmpresa') {
      loadUsinasForEmpresa(value);
    }
  };

  const loadUsinasForEmpresa = (codEmpresa: string) => {
    if (!codEmpresa) {
      setUsinaOptions([]);
      return;
    }
    
    // Mock de usinas por empresa
    const mockUsinas = [
      'Itaipu',
      'Belo Monte',
      'Tucuruí',
      'Jirau',
      'Santo Antônio',
    ];
    setUsinaOptions(mockUsinas);
  };

  const handleQuery = async () => {
    setIsLoading(true);
    
    try {
      // Simular chamada API
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Mock de dados
      const mockData: FlowData[] = Array.from({ length: 20 }, (_, index) => ({
        id: index + 1,
        dataPDP: filters.dataPDPInicio || '2024-01-15',
        codEmpresa: 'EMP001',
        nomeEmpresa: 'Empresa A',
        codUsina: 'USN001',
        nomeUsina: 'Itaipu',
        intervalo: (index % 48) + 1,
        horario: `${Math.floor((index % 48) * 0.5).toString().padStart(2, '0')}:${((index % 48) * 30) % 60 === 0 ? '00' : '30'}-${Math.floor(((index % 48) + 1) * 0.5).toString().padStart(2, '0')}:${(((index % 48) + 1) * 30) % 60 === 0 ? '00' : '30'}`,
        vazao: 10000 + Math.random() * 5000,
        vazaoAfluente: 8000 + Math.random() * 3000,
        vazaoDefluente: 9500 + Math.random() * 4000,
        vazaoTurbinada: 9000 + Math.random() * 3500,
        vazaoVertida: 500 + Math.random() * 1000,
      }));
      
      setQueryResult({
        dados: mockData,
        total: mockData.length,
        pagina: 1,
        itensPorPagina: FLOW_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE,
        totalPaginas: Math.ceil(mockData.length / FLOW_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE),
      });
      
      // Calcular dados agregados
      calculateAggregatedData(mockData);
    } catch (error) {
      console.error('Erro ao consultar vazão:', error);
      alert('Erro ao realizar consulta');
    } finally {
      setIsLoading(false);
    }
  };

  const calculateAggregatedData = (dados: FlowData[]) => {
    const groupedByUsina = dados.reduce((acc, item) => {
      const key = item.nomeUsina;
      if (!acc[key]) {
        acc[key] = [];
      }
      acc[key].push(item.vazao);
      return acc;
    }, {} as Record<string, number[]>);
    
    const aggregated: FlowAggregatedData[] = Object.entries(groupedByUsina).map(([usina, vazoes]) => ({
      nomeUsina: usina,
      vazaoMedia: vazoes.reduce((sum, v) => sum + v, 0) / vazoes.length,
      vazaoMinima: Math.min(...vazoes),
      vazaoMaxima: Math.max(...vazoes),
      vazaoTotal: vazoes.reduce((sum, v) => sum + v, 0),
      numeroIntervalos: vazoes.length,
    }));
    
    setAggregatedData(aggregated);
  };

  const handleExport = (formato: 'excel' | 'csv' | 'pdf') => {
    console.log('Exportando para', formato, queryResult.dados);
    alert(`Exportação ${formato.toUpperCase()} será implementada`);
  };

  const handlePageChange = (novaPagina: number) => {
    setQueryResult(prev => ({ ...prev, pagina: novaPagina }));
  };

  const getVazaoValue = (item: FlowData): number => {
    switch (filters.tipoVazao) {
      case TipoVazao.AFLUENTE:
        return item.vazaoAfluente || 0;
      case TipoVazao.DEFLUENTE:
        return item.vazaoDefluente || 0;
      case TipoVazao.TURBINADA:
        return item.vazaoTurbinada || 0;
      case TipoVazao.VERTIDA:
        return item.vazaoVertida || 0;
      default:
        return item.vazao;
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <img src="/images/tit_CnsVazao.gif" alt="Consulta de Vazão" />
        </div>
      </div>

      {/* Seção de Filtros */}
      <div className={styles.filtersSection}>
        <div className={styles.filterHeader}>
          <h3>Filtros de Consulta</h3>
          <button
            onClick={() => setShowFilters(!showFilters)}
            className={styles.toggleButton}
          >
            {showFilters ? '▼ Ocultar' : '▶ Mostrar'}
          </button>
        </div>

        {showFilters && (
          <div className={styles.filtersContent}>
            <div className={styles.filterRow}>
              <div className={styles.filterGroup}>
                <label><strong>Data Início:</strong></label>
                <input
                  type="date"
                  value={filters.dataPDPInicio || ''}
                  onChange={(e) => handleFilterChange('dataPDPInicio', e.target.value)}
                  className={styles.dateInput}
                />
              </div>

              <div className={styles.filterGroup}>
                <label><strong>Data Fim:</strong></label>
                <input
                  type="date"
                  value={filters.dataPDPFim || ''}
                  onChange={(e) => handleFilterChange('dataPDPFim', e.target.value)}
                  className={styles.dateInput}
                />
              </div>
            </div>

            <div className={styles.filterRow}>
              <div className={styles.filterGroup}>
                <label><strong>Empresa:</strong></label>
                <select
                  value={filters.codEmpresa || ''}
                  onChange={(e) => handleFilterChange('codEmpresa', e.target.value)}
                  className={styles.select}
                >
                  <option value="">Selecione</option>
                  {empresaOptions.map(emp => (
                    <option key={emp} value={emp}>{emp}</option>
                  ))}
                </select>
              </div>

              <div className={styles.filterGroup}>
                <label><strong>Usina:</strong></label>
                <select
                  value={filters.codUsina || ''}
                  onChange={(e) => handleFilterChange('codUsina', e.target.value)}
                  className={styles.select}
                  disabled={!filters.codEmpresa}
                >
                  <option value="">Selecione</option>
                  {usinaOptions.map(usina => (
                    <option key={usina} value={usina}>{usina}</option>
                  ))}
                </select>
              </div>
            </div>

            <div className={styles.filterRow}>
              <div className={styles.filterGroup}>
                <label><strong>Tipo de Vazão:</strong></label>
                <select
                  value={filters.tipoVazao}
                  onChange={(e) => handleFilterChange('tipoVazao', e.target.value as TipoVazao)}
                  className={styles.select}
                >
                  <option value={TipoVazao.TOTAL}>Total</option>
                  <option value={TipoVazao.AFLUENTE}>Afluente</option>
                  <option value={TipoVazao.DEFLUENTE}>Defluente</option>
                  <option value={TipoVazao.TURBINADA}>Turbinada</option>
                  <option value={TipoVazao.VERTIDA}>Vertida</option>
                </select>
              </div>

              <div className={styles.filterGroup}>
                <label><strong>Intervalo:</strong></label>
                <div className={styles.intervalInputs}>
                  <input
                    type="number"
                    min="1"
                    max="48"
                    placeholder="Início"
                    value={filters.intervaloInicio || ''}
                    onChange={(e) => handleFilterChange('intervaloInicio', parseInt(e.target.value))}
                    className={styles.numberInput}
                  />
                  <span>até</span>
                  <input
                    type="number"
                    min="1"
                    max="48"
                    placeholder="Fim"
                    value={filters.intervaloFim || ''}
                    onChange={(e) => handleFilterChange('intervaloFim', parseInt(e.target.value))}
                    className={styles.numberInput}
                  />
                </div>
              </div>
            </div>

            <div className={styles.filterActions}>
              <button
                onClick={handleQuery}
                className={styles.queryButton}
                disabled={isLoading}
              >
                {isLoading ? 'Consultando...' : 'Consultar'}
              </button>
              <button
                onClick={() => setFilters({ tipoVazao: TipoVazao.TOTAL })}
                className={styles.clearButton}
              >
                Limpar Filtros
              </button>
            </div>
          </div>
        )}
      </div>

      {/* Seção de Resultados */}
      {queryResult.dados.length > 0 && (
        <>
          <div className={styles.resultsHeader}>
            <h3>Resultados da Consulta ({queryResult.total} registros)</h3>
            <div className={styles.exportButtons}>
              <button onClick={() => handleExport('excel')} className={styles.exportButton}>
                📊 Excel
              </button>
              <button onClick={() => handleExport('csv')} className={styles.exportButton}>
                📄 CSV
              </button>
              <button onClick={() => handleExport('pdf')} className={styles.exportButton}>
                📕 PDF
              </button>
            </div>
          </div>

          {/* Dados Agregados */}
          {aggregatedData.length > 0 && (
            <div className={styles.aggregatedSection}>
              <h4>Resumo por Usina</h4>
              <table className={styles.aggregatedTable}>
                <thead>
                  <tr>
                    <th>Usina</th>
                    <th>Média (m³/s)</th>
                    <th>Mínima (m³/s)</th>
                    <th>Máxima (m³/s)</th>
                    <th>Total (m³/s)</th>
                    <th>Intervalos</th>
                  </tr>
                </thead>
                <tbody>
                  {aggregatedData.map((agg, index) => (
                    <tr key={index}>
                      <td>{agg.nomeUsina}</td>
                      <td>{agg.vazaoMedia.toFixed(2)}</td>
                      <td>{agg.vazaoMinima.toFixed(2)}</td>
                      <td>{agg.vazaoMaxima.toFixed(2)}</td>
                      <td>{agg.vazaoTotal.toFixed(2)}</td>
                      <td>{agg.numeroIntervalos}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}

          {/* Tabela de Dados Detalhados */}
          <div className={styles.dataTable}>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Data PDP</th>
                  <th>Empresa</th>
                  <th>Usina</th>
                  <th>Intervalo</th>
                  <th>Horário</th>
                  <th>Vazão (m³/s)</th>
                </tr>
              </thead>
              <tbody>
                {queryResult.dados.map((item) => (
                  <tr key={item.id}>
                    <td>{item.dataPDP}</td>
                    <td>{item.nomeEmpresa}</td>
                    <td>{item.nomeUsina}</td>
                    <td>{item.intervalo}</td>
                    <td>{item.horario}</td>
                    <td className={styles.numericCell}>{getVazaoValue(item).toFixed(2)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {/* Paginação */}
          {queryResult.totalPaginas > 1 && (
            <div className={styles.pagination}>
              <button
                onClick={() => handlePageChange(queryResult.pagina - 1)}
                disabled={queryResult.pagina === 1}
                className={styles.pageButton}
              >
                « Anterior
              </button>
              
              <span className={styles.pageInfo}>
                Página {queryResult.pagina} de {queryResult.totalPaginas}
              </span>
              
              <button
                onClick={() => handlePageChange(queryResult.pagina + 1)}
                disabled={queryResult.pagina === queryResult.totalPaginas}
                className={styles.pageButton}
              >
                Próxima »
              </button>
            </div>
          )}
        </>
      )}

      {/* Mensagem quando não há resultados */}
      {queryResult.dados.length === 0 && !isLoading && (
        <div className={styles.noResults}>
          <p>Nenhum resultado encontrado. Use os filtros acima para realizar uma consulta.</p>
        </div>
      )}
    </div>
  );
};

export default FlowQuery;

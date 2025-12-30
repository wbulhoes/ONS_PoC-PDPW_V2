import React, { useState, useEffect, useCallback } from 'react';
import styles from './AvailabilityQuery.module.css';
import {
  AvailabilityQueryFilters,
  AvailabilityQueryResponse,
  DisponibilidadeData,
  DisponibilidadeAggregated,
  TipoUsina,
  StatusDisponibilidade,
  EmpresaInfo,
  UsinaInfo,
  AVAILABILITY_QUERY_CONSTANTS,
  generateIntervalLabel,
  TIPO_USINA_LABELS,
  STATUS_LABELS,
} from '../../../types/availabilityQuery';
import { AvailabilityQueryService } from '../../../services/availabilityQueryService';

/**
 * Componente para Consulta de Disponibilidade de Usinas
 * 
 * Funcionalidades:
 * - Filtros avançados (data, empresa, usina, tipo, status)
 * - Visualização em grid/tabela com 48 intervalos
 * - Agregação de dados por usina
 * - Paginação
 * - Exportação em Excel/CSV/PDF
 */
const AvailabilityQuery: React.FC = () => {
  // ========== ESTADO DE FILTROS ==========
  const [filters, setFilters] = useState<AvailabilityQueryFilters>({
    tipoUsina: TipoUsina.TODAS,
  });

  // ========== ESTADO DE DADOS ==========
  const [queryResult, setQueryResult] = useState<AvailabilityQueryResponse>({
    dados: [],
    total: 0,
    pagina: 1,
    itensPorPagina: AVAILABILITY_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE,
    totalPaginas: 0,
  });

  const [aggregatedData, setAggregatedData] = useState<DisponibilidadeAggregated[]>([]);
  const [empresaOptions, setEmpresaOptions] = useState<EmpresaInfo[]>([]);
  const [usinaOptions, setUsinaOptions] = useState<UsinaInfo[]>([]);

  // ========== ESTADO DE UI ==========
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string>('');
  const [showFilters, setShowFilters] = useState(true);
  const [successMessage, setSuccessMessage] = useState<string>('');

  // ========== EFEITO: Carregar empresas ao montar ==========
  useEffect(() => {
    loadEmpresas();
  }, []);

  // ========== EFEITO: Carregar usinas quando empresa muda ==========
  useEffect(() => {
    if (filters.codEmpresa) {
      loadUsinasForEmpresa(filters.codEmpresa);
    } else {
      setUsinaOptions([]);
    }
  }, [filters.codEmpresa]);

  // ========== FUNÇÕES DE CARREGAMENTO ==========
  const loadEmpresas = async () => {
    try {
      const empresas = await AvailabilityQueryService.getEmpresas();
      setEmpresaOptions(empresas);
    } catch (err) {
      console.error('Erro ao carregar empresas:', err);
      setError('Erro ao carregar lista de empresas');
    }
  };

  const loadUsinasForEmpresa = async (codigoEmpresa: string) => {
    try {
      // Parse do ID da empresa (assumindo que o código contém o ID)
      const empresaId = parseInt(codigoEmpresa.split('-')[0]) || 1;
      const usinas = await AvailabilityQueryService.getUsinasByEmpresa(empresaId);
      setUsinaOptions(usinas);
      // Limpar seleção de usina quando trocar empresa
      setFilters(prev => ({ ...prev, codUsina: undefined }));
    } catch (err) {
      console.error('Erro ao carregar usinas:', err);
      setError('Erro ao carregar usinas da empresa');
    }
  };

  // ========== FUNÇÕES DE MUDANÇA DE FILTROS ==========
  const handleFilterChange = (field: keyof AvailabilityQueryFilters, value: any) => {
    setFilters(prev => ({
      ...prev,
      [field]: value === '' ? undefined : value,
    }));
    setError('');
  };

  // ========== FUNÇÃO PRINCIPAL: EXECUTAR CONSULTA ==========
  const handleQuery = useCallback(async () => {
    if (!filters.dataPDPInicio && !filters.codEmpresa) {
      setError('Preencha pelo menos Data PDP ou Empresa para fazer a consulta');
      return;
    }

    setIsLoading(true);
    setError('');
    setSuccessMessage('');

    try {
      // Executar consulta com filtros
      const result = await AvailabilityQueryService.query(
        filters,
        1, // Iniciar na página 1
        AVAILABILITY_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE
      );

      setQueryResult(result);
      setSuccessMessage(`${result.total} registros encontrados`);

      // Calcular dados agregados
      if (result.dados.length > 0) {
        const aggregated = await AvailabilityQueryService.getAggregatedData(filters);
        setAggregatedData(aggregated);
      } else {
        setAggregatedData([]);
      }
    } catch (err) {
      const errorMsg = err instanceof Error ? err.message : 'Erro ao executar consulta';
      console.error('Erro na consulta:', err);
      setError(errorMsg);
      setQueryResult({
        dados: [],
        total: 0,
        pagina: 1,
        itensPorPagina: AVAILABILITY_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE,
        totalPaginas: 0,
      });
    } finally {
      setIsLoading(false);
    }
  }, [filters]);

  // ========== FUNÇÃO: LIMPAR FILTROS ==========
  const handleClearFilters = () => {
    setFilters({ tipoUsina: TipoUsina.TODAS });
    setQueryResult({
      dados: [],
      total: 0,
      pagina: 1,
      itensPorPagina: AVAILABILITY_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE,
      totalPaginas: 0,
    });
    setAggregatedData([]);
    setError('');
    setSuccessMessage('');
  };

  // ========== FUNÇÃO: MUDAR PÁGINA ==========
  const handlePageChange = async (novaPagina: number) => {
    if (novaPagina < 1 || novaPagina > queryResult.totalPaginas) return;

    setIsLoading(true);
    try {
      const result = await AvailabilityQueryService.query(
        filters,
        novaPagina,
        queryResult.itensPorPagina
      );
      setQueryResult(result);
    } catch (err) {
      setError('Erro ao carregar página');
    } finally {
      setIsLoading(false);
    }
  };

  // ========== FUNÇÃO: EXPORTAR DADOS ==========
  const handleExport = async (formato: 'excel' | 'csv' | 'pdf') => {
    if (queryResult.dados.length === 0) {
      setError('Nenhum dado para exportar');
      return;
    }

    setIsLoading(true);
    try {
      const blob = await AvailabilityQueryService.exportData(filters, formato);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `disponibilidade_${new Date().toISOString().split('T')[0]}.${formato === 'excel' ? 'xlsx' : formato}`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      setSuccessMessage(`Arquivo exportado em ${formato.toUpperCase()}`);
    } catch (err) {
      setError('Erro ao exportar dados');
    } finally {
      setIsLoading(false);
    }
  };

  // ========== RENDERIZAÇÃO: FILTROS ==========
  const renderFilters = () => (
    <div className={styles.filtersSection}>
      <div
        className={styles.filtersHeader}
        onClick={() => setShowFilters(!showFilters)}
        role="button"
        tabIndex={0}
        onKeyDown={(e) => {
          if (e.key === 'Enter' || e.key === ' ') setShowFilters(!showFilters);
        }}
      >
        <h2>
          <span className={`${styles.toggleIcon} ${!showFilters ? styles.collapsed : ''}`}>
            ▼
          </span>
          Filtros de Busca
        </h2>
      </div>

      <div className={`${styles.filtersContent} ${!showFilters ? styles.hidden : ''}`}>
        <div className={styles.filterGroup}>
          <label htmlFor="dataPDPInicio">Data PDP - Início</label>
          <input
            id="dataPDPInicio"
            type="date"
            value={filters.dataPDPInicio || ''}
            onChange={(e) => handleFilterChange('dataPDPInicio', e.target.value)}
          />
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="dataPDPFim">Data PDP - Fim</label>
          <input
            id="dataPDPFim"
            type="date"
            value={filters.dataPDPFim || ''}
            onChange={(e) => handleFilterChange('dataPDPFim', e.target.value)}
          />
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="codEmpresa">Empresa</label>
          <select
            id="codEmpresa"
            value={filters.codEmpresa || ''}
            onChange={(e) => handleFilterChange('codEmpresa', e.target.value)}
          >
            <option value="">Todas</option>
            {empresaOptions.map((empresa) => (
              <option key={empresa.id} value={empresa.codigo}>
                {empresa.nome}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="codUsina">Usina</label>
          <select
            id="codUsina"
            value={filters.codUsina || ''}
            onChange={(e) => handleFilterChange('codUsina', e.target.value)}
            disabled={!filters.codEmpresa}
          >
            <option value="">Todas</option>
            {usinaOptions.map((usina) => (
              <option key={usina.id} value={usina.codigo}>
                {usina.nome}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="tipoUsina">Tipo de Usina</label>
          <select
            id="tipoUsina"
            value={filters.tipoUsina || ''}
            onChange={(e) => handleFilterChange('tipoUsina', e.target.value)}
          >
            {Object.entries(TIPO_USINA_LABELS).map(([key, label]) => (
              <option key={key} value={key}>
                {label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="statusDisponibilidade">Status</label>
          <select
            id="statusDisponibilidade"
            value={filters.statusDisponibilidade || ''}
            onChange={(e) => handleFilterChange('statusDisponibilidade', e.target.value)}
          >
            <option value="">Todos</option>
            {Object.entries(STATUS_LABELS).map(([key, label]) => (
              <option key={key} value={key}>
                {label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="intervaloInicio">Intervalo - Início</label>
          <input
            id="intervaloInicio"
            type="number"
            min="1"
            max="48"
            value={filters.intervaloInicio || ''}
            onChange={(e) => handleFilterChange('intervaloInicio', e.target.value ? parseInt(e.target.value) : undefined)}
          />
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="intervaloFim">Intervalo - Fim</label>
          <input
            id="intervaloFim"
            type="number"
            min="1"
            max="48"
            value={filters.intervaloFim || ''}
            onChange={(e) => handleFilterChange('intervaloFim', e.target.value ? parseInt(e.target.value) : undefined)}
          />
        </div>
      </div>

      <div className={styles.filtersActions}>
        <button
          className={`${styles.btn} ${styles.btnPrimary}`}
          onClick={handleQuery}
          disabled={isLoading}
        >
          {isLoading ? 'Buscando...' : 'Buscar'}
        </button>
        <button
          className={`${styles.btn} ${styles.btnSecondary}`}
          onClick={handleClearFilters}
          disabled={isLoading}
        >
          Limpar
        </button>
      </div>
    </div>
  );

  // ========== RENDERIZAÇÃO: MENSAGENS ==========
  const renderMessages = () => (
    <>
      {error && (
        <div className={styles.error}>
          <span className={styles.errorIcon}>⚠️</span>
          {error}
        </div>
      )}
      {successMessage && (
        <div className={styles.success}>
          <span>✓</span> {successMessage}
        </div>
      )}
    </>
  );

  // ========== RENDERIZAÇÃO: GRID DE DADOS ==========
  const renderDataGrid = () => {
    if (isLoading) {
      return (
        <div className={styles.loading}>
          <span className={styles.loadingSpinner}></span>
          Carregando dados...
        </div>
      );
    }

    if (queryResult.dados.length === 0) {
      return (
        <div className={styles.loading}>
          Nenhum registro encontrado. Ajuste os filtros e tente novamente.
        </div>
      );
    }

    return (
      <>
        <div className={styles.resultsSummary}>
          <div>
            <strong>{queryResult.total}</strong> registros encontrados
            {' '}| Página <strong>{queryResult.pagina}</strong> de{' '}
            <strong>{queryResult.totalPaginas}</strong>
          </div>
        </div>

        <div className={styles.gridContainer}>
          <table className={styles.grid}>
            <thead>
              <tr>
                <th>Data PDP</th>
                <th>Empresa</th>
                <th>Usina</th>
                <th>Tipo</th>
                <th>Intervalo</th>
                <th className={styles.columnNumeric}>Cap. Máx. (MW)</th>
                <th className={styles.columnNumeric}>Cap. Mín. (MW)</th>
                <th className={styles.columnNumeric}>Disponibilidade %</th>
                <th>Status</th>
                <th>Motivo</th>
              </tr>
            </thead>
            <tbody>
              {queryResult.dados.map((item, idx) => (
                <tr key={`${item.id}-${idx}`}>
                  <td>{item.dataPDP}</td>
                  <td>{item.nomeEmpresa}</td>
                  <td>{item.nomeUsina}</td>
                  <td>{TIPO_USINA_LABELS[item.tipoUsina]}</td>
                  <td className={styles.columnCenter}>{item.horario}</td>
                  <td className={styles.columnNumeric}>
                    {item.capacidadeMaximaDisponivel.toFixed(2)}
                  </td>
                  <td className={styles.columnNumeric}>
                    {item.capacidadeMinimDisponivel.toFixed(2)}
                  </td>
                  <td className={styles.columnNumeric}>
                    {item.percentualDisponibilidade.toFixed(1)}%
                  </td>
                  <td>
                    <span
                      className={`${styles.badge} ${
                        item.statusDisponibilidade === StatusDisponibilidade.ATIVA
                          ? styles.badgeActive
                          : item.statusDisponibilidade === StatusDisponibilidade.INATIVA
                          ? styles.badgeInactive
                          : styles.badgeMaintenance
                      }`}
                    >
                      {STATUS_LABELS[item.statusDisponibilidade]}
                    </span>
                  </td>
                  <td>{item.motivoIndisponibilidade || '-'}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        {/* Paginação */}
        <div className={styles.pagination}>
          <button
            className={`${styles.paginationItem} ${queryResult.pagina === 1 ? styles.disabled : ''}`}
            onClick={() => handlePageChange(1)}
            disabled={queryResult.pagina === 1 || isLoading}
          >
            « Primeira
          </button>
          <button
            className={`${styles.paginationItem} ${queryResult.pagina === 1 ? styles.disabled : ''}`}
            onClick={() => handlePageChange(queryResult.pagina - 1)}
            disabled={queryResult.pagina === 1 || isLoading}
          >
            ‹ Anterior
          </button>

          {Array.from({ length: Math.min(5, queryResult.totalPaginas) }, (_, i) => {
            let pageNum = queryResult.pagina - 2 + i;
            if (pageNum < 1) pageNum += 5;
            if (pageNum > queryResult.totalPaginas) pageNum -= 5;
            return pageNum >= 1 && pageNum <= queryResult.totalPaginas ? (
              <button
                key={pageNum}
                className={`${styles.paginationItem} ${pageNum === queryResult.pagina ? styles.active : ''}`}
                onClick={() => handlePageChange(pageNum)}
                disabled={isLoading}
              >
                {pageNum}
              </button>
            ) : null;
          })}

          <button
            className={`${styles.paginationItem} ${
              queryResult.pagina === queryResult.totalPaginas ? styles.disabled : ''
            }`}
            onClick={() => handlePageChange(queryResult.pagina + 1)}
            disabled={queryResult.pagina === queryResult.totalPaginas || isLoading}
          >
            Próxima ›
          </button>
          <button
            className={`${styles.paginationItem} ${
              queryResult.pagina === queryResult.totalPaginas ? styles.disabled : ''
            }`}
            onClick={() => handlePageChange(queryResult.totalPaginas)}
            disabled={queryResult.pagina === queryResult.totalPaginas || isLoading}
          >
            Última »
          </button>

          <span className={styles.paginationInfo}>
            Página {queryResult.pagina} de {queryResult.totalPaginas}
          </span>
        </div>

        {/* Exportação */}
        <div className={styles.exportSection}>
          <button
            className={styles.exportBtn}
            onClick={() => handleExport('excel')}
            disabled={isLoading}
            title="Exportar para Excel"
          >
            <span className={styles.exportIcon}>📊</span> Excel
          </button>
          <button
            className={styles.exportBtn}
            onClick={() => handleExport('csv')}
            disabled={isLoading}
            title="Exportar para CSV"
          >
            <span className={styles.exportIcon}>📄</span> CSV
          </button>
          <button
            className={styles.exportBtn}
            onClick={() => handleExport('pdf')}
            disabled={isLoading}
            title="Exportar para PDF"
          >
            <span className={styles.exportIcon}>📋</span> PDF
          </button>
        </div>
      </>
    );
  };

  // ========== RENDERIZAÇÃO: AGREGAÇÃO ==========
  const renderAggregation = () => {
    if (aggregatedData.length === 0) return null;

    return (
      <div className={styles.aggregationSection}>
        <h2>Resumo por Usina</h2>
        <div className={styles.aggregationGrid}>
          {aggregatedData.map((item, idx) => (
            <div key={`${item.codUsina}-${idx}`} className={styles.aggregationCard}>
              <h3>{item.nomeUsina}</h3>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Tipo:</span>
                <span className={styles.aggregationValue}>{TIPO_USINA_LABELS[item.tipoUsina]}</span>
              </div>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Cap. Máx. Média:</span>
                <span className={styles.aggregationValue}>{item.capacidadeMaximaMedia.toFixed(2)} MW</span>
              </div>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Cap. Mín. Média:</span>
                <span className={styles.aggregationValue}>{item.capacidadeMinimaMedia.toFixed(2)} MW</span>
              </div>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Disponibilidade Média:</span>
                <span className={styles.aggregationValue}>{item.percentualDisponibilidadeMedia.toFixed(1)}%</span>
              </div>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Status Predominante:</span>
                <span className={styles.aggregationValue}>{STATUS_LABELS[item.statusPredominante]}</span>
              </div>
              <div className={styles.aggregationStat}>
                <span className={styles.aggregationLabel}>Registros:</span>
                <span className={styles.aggregationValue}>{item.quantidadeRegistros}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    );
  };

  // ========== RENDERIZAÇÃO PRINCIPAL ==========
  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1>📊 Consulta de Disponibilidade</h1>
        <p>Consulte dados históricos de disponibilidade de usinas hidráulicas e térmicas</p>
      </div>

      {renderMessages()}
      {renderFilters()}

      {queryResult.dados.length > 0 && (
        <div className={styles.resultsSection}>
          {renderDataGrid()}
          {renderAggregation()}
        </div>
      )}

      {queryResult.dados.length === 0 && !isLoading && queryResult.total === 0 && (
        <div className={styles.loading}>
          Selecione filtros e clique em "Buscar" para consultar dados
        </div>
      )}
    </div>
  );
};

export default AvailabilityQuery;

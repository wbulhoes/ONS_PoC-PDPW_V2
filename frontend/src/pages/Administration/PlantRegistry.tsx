import React, { useState, useEffect } from 'react';
import styles from './PlantRegistry.module.css';
import {
  Usina,
  PlantQueryParams,
  PlantQueryResponse,
  EmpresaOption,
  getTipoUsinaLabel,
  formatarCodigoUsina,
  isCodigoEmpresaValido,
} from '../../types/plant';

interface PlantRegistryProps {
  onLoadEmpresas: () => Promise<EmpresaOption[]>;
  onSearchUsinas: (params: PlantQueryParams) => Promise<PlantQueryResponse>;
  onViewDetails: (codUsina: string, codEmpresa: string) => void;
}

const PlantRegistry: React.FC<PlantRegistryProps> = ({
  onLoadEmpresas,
  onSearchUsinas,
  onViewDetails,
}) => {
  const [empresas, setEmpresas] = useState<EmpresaOption[]>([]);
  const [empresaSelecionada, setEmpresaSelecionada] = useState<string>('');
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  
  // Paginação
  const [currentPage, setCurrentPage] = useState<number>(0);
  const [totalPages, setTotalPages] = useState<number>(0);
  const [totalRecords, setTotalRecords] = useState<number>(0);
  const [pageSize] = useState<number>(10);

  // Carrega empresas ao montar componente
  useEffect(() => {
    carregarEmpresas();
  }, []);

  const carregarEmpresas = async () => {
    try {
      setLoading(true);
      setError(null);
      const empresasData = await onLoadEmpresas();
      setEmpresas(empresasData);
    } catch (err) {
      console.error('Erro ao carregar empresas:', err);
      setError('Não foi possível acessar a Base de Dados.');
    } finally {
      setLoading(false);
    }
  };

  const handlePesquisar = async () => {
    if (!isCodigoEmpresaValido(empresaSelecionada)) {
      setError('Selecione uma empresa para pesquisar');
      return;
    }

    await carregarUsinas(0);
  };

  const carregarUsinas = async (page: number) => {
    try {
      setLoading(true);
      setError(null);

      const params: PlantQueryParams = {
        codEmpresa: empresaSelecionada,
        page,
        pageSize,
      };

      const response = await onSearchUsinas(params);
      setUsinas(response.usinas);
      setCurrentPage(response.page);
      setTotalPages(response.totalPages);
      setTotalRecords(response.total);
    } catch (err) {
      console.error('Erro ao carregar usinas:', err);
      setError('Não foi possível acessar a Base de Dados.');
      setUsinas([]);
    } finally {
      setLoading(false);
    }
  };

  const handlePageChange = async (newPage: number) => {
    if (newPage >= 0 && newPage < totalPages) {
      await carregarUsinas(newPage);
    }
  };

  const handleViewDetails = (usina: Usina) => {
    onViewDetails(usina.codUsina, empresaSelecionada);
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2>Consulta de Usinas</h2>
      </div>

      {error && (
        <div className={styles.error} data-testid="error-message">
          {error}
        </div>
      )}

      <div className={styles.filterContainer}>
        <div className={styles.formGroup}>
          <label htmlFor="empresaSelect">Empresa:</label>
          <div className={styles.searchRow}>
            <select
              id="empresaSelect"
              value={empresaSelecionada}
              onChange={(e) => setEmpresaSelecionada(e.target.value)}
              disabled={loading}
              data-testid="empresa-select"
              className={styles.empresaSelect}
            >
              <option value="">Selecione uma empresa</option>
              {empresas.map((empresa) => (
                <option key={empresa.codEmpre} value={empresa.codEmpre}>
                  {empresa.sigEmpre}
                </option>
              ))}
            </select>
            <button
              onClick={handlePesquisar}
              disabled={loading || !isCodigoEmpresaValido(empresaSelecionada)}
              data-testid="pesquisar-btn"
              className={styles.btnPesquisar}
            >
              {loading ? 'Pesquisando...' : 'Pesquisar'}
            </button>
          </div>
        </div>
      </div>

      {usinas.length > 0 && (
        <div className={styles.gridContainer} data-testid="grid-container">
          <div className={styles.gridHeader}>
            <span className={styles.recordCount}>
              {totalRecords} usina{totalRecords !== 1 ? 's' : ''} encontrada{totalRecords !== 1 ? 's' : ''}
            </span>
          </div>

          <div className={styles.tableWrapper}>
            <table className={styles.table} data-testid="usinas-table">
              <thead>
                <tr>
                  <th>Código</th>
                  <th>Sigla</th>
                  <th>Nome</th>
                  <th>Tipo</th>
                </tr>
              </thead>
              <tbody>
                {usinas.map((usina, index) => (
                  <tr
                    key={usina.codUsina}
                    className={index % 2 === 1 ? styles.alternateRow : ''}
                    data-testid={`usina-row-${index}`}
                  >
                    <td>
                      <button
                        onClick={() => handleViewDetails(usina)}
                        className={styles.linkButton}
                        data-testid={`view-details-${usina.codUsina}`}
                      >
                        {formatarCodigoUsina(usina.codUsina)}
                      </button>
                    </td>
                    <td className={styles.centerAlign}>{usina.sigUsina}</td>
                    <td>{usina.nomUsina}</td>
                    <td className={styles.centerAlign}>{getTipoUsinaLabel(usina.tipUsina)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {totalPages > 1 && (
            <div className={styles.pagination} data-testid="pagination">
              <button
                onClick={() => handlePageChange(currentPage - 1)}
                disabled={currentPage === 0}
                className={styles.paginationBtn}
                data-testid="prev-page-btn"
              >
                &lt; Anterior
              </button>
              <span className={styles.pageInfo} data-testid="page-info">
                Página {currentPage + 1} de {totalPages}
              </span>
              <button
                onClick={() => handlePageChange(currentPage + 1)}
                disabled={currentPage >= totalPages - 1}
                className={styles.paginationBtn}
                data-testid="next-page-btn"
              >
                Próxima &gt;
              </button>
            </div>
          )}
        </div>
      )}

      {!loading && usinas.length === 0 && empresaSelecionada && (
        <div className={styles.noResults} data-testid="no-results">
          Nenhuma usina encontrada para a empresa selecionada.
        </div>
      )}

      {loading && (
        <div className={styles.loading} data-testid="loading">
          Carregando dados...
        </div>
      )}
    </div>
  );
};

export default PlantRegistry;

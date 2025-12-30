import React, { useState, useEffect } from 'react';
import {
  EquipePDP,
  TeamQueryResponse,
  FormMode,
  validarNomeEquipe,
  formatarNomeEquipe,
  validarSelecaoExclusao,
  validarSelecaoAlteracao,
  isConstraintError,
} from '../../types/team';
import styles from './TeamRegistry.module.css';

interface TeamRegistryProps {
  onLoadEquipes: () => Promise<EquipePDP[]>;
  onCreate: (nomEquipe: string) => Promise<void>;
  onUpdate: (idEquipe: number, nomEquipe: string) => Promise<void>;
  onDelete: (ids: number[]) => Promise<void>;
}

const TeamRegistry: React.FC<TeamRegistryProps> = ({
  onLoadEquipes,
  onCreate,
  onUpdate,
  onDelete,
}) => {
  // Estados principais
  const [equipes, setEquipes] = useState<EquipePDP[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');

  // Estados do formulário
  const [mode, setMode] = useState<FormMode>('view');
  const [idEquipe, setIdEquipe] = useState<string>('');
  const [nomeEquipe, setNomeEquipe] = useState<string>('');
  const [formError, setFormError] = useState<string>('');

  // Estados de seleção e paginação
  const [selectedIds, setSelectedIds] = useState<Set<number>>(new Set());
  const [currentPage, setCurrentPage] = useState<number>(1);
  const itemsPerPage = 4;

  // Carregar equipes ao montar
  useEffect(() => {
    loadEquipes();
  }, []);

  // Limpar mensagens após 5 segundos
  useEffect(() => {
    if (error || successMessage) {
      const timer = setTimeout(() => {
        setError('');
        setSuccessMessage('');
      }, 5000);
      return () => clearTimeout(timer);
    }
  }, [error, successMessage]);

  /**
   * Carrega todas as equipes
   */
  const loadEquipes = async () => {
    try {
      setLoading(true);
      setError('');
      const data = await onLoadEquipes();
      setEquipes(data);
      setSelectedIds(new Set());
    } catch (err) {
      setError('Não foi possível recuperar os registros.');
      console.error('Erro ao carregar equipes:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Alterna seleção de checkbox individual
   */
  const toggleSelection = (id: number) => {
    const newSelection = new Set(selectedIds);
    if (newSelection.has(id)) {
      newSelection.delete(id);
    } else {
      newSelection.add(id);
    }
    setSelectedIds(newSelection);
  };

  /**
   * Seleciona ou desmarca todos os itens da página atual
   */
  const toggleSelectAll = () => {
    const paginatedEquipes = getPaginatedEquipes();
    const allSelected = paginatedEquipes.every((eq) => selectedIds.has(eq.idEquipePdp));

    const newSelection = new Set(selectedIds);
    if (allSelected) {
      paginatedEquipes.forEach((eq) => newSelection.delete(eq.idEquipePdp));
    } else {
      paginatedEquipes.forEach((eq) => newSelection.add(eq.idEquipePdp));
    }
    setSelectedIds(newSelection);
  };

  /**
   * Retorna equipes da página atual
   */
  const getPaginatedEquipes = (): EquipePDP[] => {
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    return equipes.slice(startIndex, endIndex);
  };

  /**
   * Calcula total de páginas
   */
  const getTotalPages = (): number => {
    return Math.ceil(equipes.length / itemsPerPage);
  };

  /**
   * Inicia modo de inclusão
   */
  const handleIncluir = () => {
    setMode('create');
    setIdEquipe('');
    setNomeEquipe('');
    setFormError('');
    setError('');
    setSuccessMessage('');
  };

  /**
   * Inicia modo de alteração
   */
  const handleAlterar = () => {
    const ids = Array.from(selectedIds);
    const validacao = validarSelecaoAlteracao(ids);

    if (!validacao.valido) {
      setError(validacao.mensagem);
      return;
    }

    const equipeSelecionada = equipes.find((eq) => eq.idEquipePdp === ids[0]);
    if (!equipeSelecionada) {
      setError('Equipe não encontrada.');
      return;
    }

    setMode('edit');
    setIdEquipe(equipeSelecionada.idEquipePdp.toString());
    setNomeEquipe(equipeSelecionada.nomEquipePdp);
    setFormError('');
    setError('');
    setSuccessMessage('');
  };

  /**
   * Exclui equipes selecionadas
   */
  const handleExcluir = async () => {
    const ids = Array.from(selectedIds);
    const validacao = validarSelecaoExclusao(ids);

    if (!validacao.valido) {
      setError(validacao.mensagem);
      return;
    }

    if (!window.confirm(`Confirma a exclusão de ${ids.length} equipe(s)?`)) {
      return;
    }

    try {
      setLoading(true);
      setError('');
      await onDelete(ids);
      setSuccessMessage(`${ids.length} equipe(s) excluída(s) com sucesso!`);
      setCurrentPage(1);
      await loadEquipes();
    } catch (err: any) {
      if (isConstraintError(err.message)) {
        setError('Não é possível excluir, a equipe está associada a um usuário.');
      } else {
        setError('Erro ao excluir equipe(s).');
      }
      console.error('Erro ao excluir:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Salva equipe (criar ou atualizar)
   */
  const handleSalvar = async () => {
    const validacaoNome = validarNomeEquipe(nomeEquipe);
    if (validacaoNome !== true) {
      setFormError(validacaoNome);
      return;
    }

    try {
      setLoading(true);
      setError('');
      setFormError('');

      if (mode === 'create') {
        await onCreate(nomeEquipe.trim());
        setSuccessMessage('Equipe incluída com sucesso!');
      } else if (mode === 'edit') {
        await onUpdate(parseInt(idEquipe), nomeEquipe.trim());
        setSuccessMessage('Equipe atualizada com sucesso!');
      }

      handleCancelar();
      await loadEquipes();
    } catch (err: any) {
      setFormError('Erro ao salvar equipe.');
      console.error('Erro ao salvar:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Cancela operação atual
   */
  const handleCancelar = () => {
    setMode('view');
    setIdEquipe('');
    setNomeEquipe('');
    setFormError('');
  };

  /**
   * Navega para página anterior
   */
  const goToPreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  /**
   * Navega para próxima página
   */
  const goToNextPage = () => {
    if (currentPage < getTotalPages()) {
      setCurrentPage(currentPage + 1);
    }
  };

  const paginatedEquipes = getPaginatedEquipes();
  const totalPages = getTotalPages();
  const allPageItemsSelected =
    paginatedEquipes.length > 0 &&
    paginatedEquipes.every((eq) => selectedIds.has(eq.idEquipePdp));

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Cadastro de Equipe PDP</h1>

      {/* Mensagens de erro e sucesso */}
      {error && (
        <div className={styles.errorMessage} data-testid="error-message">
          {error}
        </div>
      )}
      {successMessage && (
        <div className={styles.successMessage} data-testid="success-message">
          {successMessage}
        </div>
      )}

      {/* Grid de Equipes */}
      <div className={styles.gridContainer} data-testid="grid-container">
        <table className={styles.dataGrid} data-testid="equipes-table">
          <thead>
            <tr>
              <th style={{ width: '20px' }}>
                <input
                  type="checkbox"
                  checked={allPageItemsSelected}
                  onChange={toggleSelectAll}
                  disabled={loading || paginatedEquipes.length === 0}
                  data-testid="select-all-checkbox"
                />
              </th>
              <th style={{ width: '100px' }}>Código</th>
              <th style={{ width: '500px' }}>Nome Equipe</th>
            </tr>
          </thead>
          <tbody>
            {loading && (
              <tr>
                <td colSpan={3} className={styles.loading}>
                  Carregando...
                </td>
              </tr>
            )}
            {!loading && paginatedEquipes.length === 0 && (
              <tr>
                <td colSpan={3} className={styles.noData}>
                  Nenhuma equipe encontrada.
                </td>
              </tr>
            )}
            {!loading &&
              paginatedEquipes.map((equipe, index) => (
                <tr
                  key={equipe.idEquipePdp}
                  className={index % 2 === 1 ? styles.alternateRow : ''}
                  data-testid={`equipe-row-${index}`}
                >
                  <td>
                    <input
                      type="checkbox"
                      checked={selectedIds.has(equipe.idEquipePdp)}
                      onChange={() => toggleSelection(equipe.idEquipePdp)}
                      data-testid={`checkbox-${equipe.idEquipePdp}`}
                    />
                  </td>
                  <td>{equipe.idEquipePdp}</td>
                  <td>{formatarNomeEquipe(equipe.nomEquipePdp)}</td>
                </tr>
              ))}
          </tbody>
        </table>

        {/* Paginação */}
        {totalPages > 1 && (
          <div className={styles.pagination} data-testid="pagination">
            <button
              onClick={goToPreviousPage}
              disabled={currentPage === 1 || loading}
              className={styles.pageButton}
              data-testid="prev-page-btn"
            >
              &lt;Anterior
            </button>
            <span className={styles.pageInfo} data-testid="page-info">
              Página {currentPage} de {totalPages}
            </span>
            <button
              onClick={goToNextPage}
              disabled={currentPage >= totalPages || loading}
              className={styles.pageButton}
              data-testid="next-page-btn"
            >
              Próxima&gt;
            </button>
          </div>
        )}
      </div>

      {/* Botões de Ação - Grid */}
      {mode === 'view' && (
        <div className={styles.actionButtons} data-testid="action-buttons">
          <button
            onClick={handleIncluir}
            disabled={loading}
            className={styles.btnIncluir}
            data-testid="incluir-btn"
          >
            Incluir
          </button>
          <button
            onClick={handleAlterar}
            disabled={loading || selectedIds.size === 0}
            className={styles.btnAlterar}
            data-testid="alterar-btn"
          >
            Alterar
          </button>
          <button
            onClick={handleExcluir}
            disabled={loading || selectedIds.size === 0}
            className={styles.btnExcluir}
            data-testid="excluir-btn"
          >
            Excluir
          </button>
        </div>
      )}

      {/* Formulário de Edição */}
      {(mode === 'create' || mode === 'edit') && (
        <div className={styles.formContainer} data-testid="form-container">
          {formError && (
            <div className={styles.formError} data-testid="form-error">
              {formError}
            </div>
          )}

          <table className={styles.formTable}>
            <tbody>
              <tr>
                <td className={styles.labelCell}>Código:&nbsp;</td>
                <td>
                  <input
                    type="text"
                    value={idEquipe}
                    disabled
                    className={styles.inputCodigo}
                    data-testid="codigo-input"
                  />
                </td>
              </tr>
              <tr>
                <td className={styles.labelCell}>Nome Equipe:&nbsp;</td>
                <td>
                  <input
                    type="text"
                    value={nomeEquipe}
                    onChange={(e) => setNomeEquipe(e.target.value)}
                    maxLength={40}
                    className={styles.inputNome}
                    data-testid="nome-input"
                    autoFocus
                  />
                </td>
              </tr>
            </tbody>
          </table>

          <div className={styles.formButtons}>
            <button
              onClick={handleSalvar}
              disabled={loading}
              className={styles.btnSalvar}
              data-testid="salvar-btn"
            >
              Salvar
            </button>
            <button
              onClick={handleCancelar}
              disabled={loading}
              className={styles.btnCancelar}
              data-testid="cancelar-btn"
            >
              Cancelar
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default TeamRegistry;

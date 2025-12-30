/**
 * Componente: UserAssociation
 * Tela: Associação Usuário X Empresa (frmAssocUsuar.aspx)
 *
 * Funcionalidades:
 * - Dropdowns para selecionar empresa e usuário (filtros)
 * - Listagem paginada de associações existentes (5 por página)
 * - Inclusão de novas associações (empresa + usuário)
 * - Exclusão com seleção múltipla
 */

import React, { useState, useEffect } from 'react';
import {
  UserCompanyAssociation,
  UserCompanyAssociationListResponse,
  CompanyOption,
  UserOption,
  AssociationPaginationParams,
  AssociationOperationResponse,
  NewAssociation,
} from '../../types/userAssociation';
import styles from './UserAssociation.module.css';

interface UserAssociationProps {
  onLoadAssociations?: (
    params: AssociationPaginationParams
  ) => Promise<UserCompanyAssociationListResponse>;
  onLoadCompanies?: () => Promise<CompanyOption[]>;
  onLoadUsers?: () => Promise<UserOption[]>;
  onAddAssociation?: (data: NewAssociation) => Promise<AssociationOperationResponse>;
  onDeleteAssociations?: (
    associations: Array<{ codempre: string; usuar_id: string }>
  ) => Promise<AssociationOperationResponse>;
}

const UserAssociation: React.FC<UserAssociationProps> = ({
  onLoadAssociations,
  onLoadCompanies,
  onLoadUsers,
  onAddAssociation,
  onDeleteAssociations,
}) => {
  const [companies, setCompanies] = useState<CompanyOption[]>([]);
  const [users, setUsers] = useState<UserOption[]>([]);
  const [selectedCompany, setSelectedCompany] = useState<string>('0');
  const [selectedUser, setSelectedUser] = useState<string>('0');

  const [associations, setAssociations] = useState<UserCompanyAssociation[]>([]);
  const [selectedAssociations, setSelectedAssociations] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');

  // Paginação
  const [currentPage, setCurrentPage] = useState(0);
  const [pageSize] = useState(5); // 5 registros por página (conforme legado)
  const [totalItems, setTotalItems] = useState(0);

  useEffect(() => {
    loadCompanies();
    loadUsers();
  }, []);

  useEffect(() => {
    if (selectedCompany !== '0' || selectedUser !== '0') {
      setCurrentPage(0);
      loadAssociations();
    }
  }, [selectedCompany, selectedUser]);

  useEffect(() => {
    if (currentPage >= 0 && (selectedCompany !== '0' || selectedUser !== '0')) {
      loadAssociations();
    }
  }, [currentPage]);

  const loadCompanies = async () => {
    try {
      let companyList: CompanyOption[];

      if (onLoadCompanies) {
        companyList = await onLoadCompanies();
      } else {
        // Mock data para desenvolvimento
        companyList = generateMockCompanies();
      }

      setCompanies(companyList);
    } catch (error) {
      console.error('Erro ao carregar empresas:', error);
    }
  };

  const loadUsers = async () => {
    try {
      let userList: UserOption[];

      if (onLoadUsers) {
        userList = await onLoadUsers();
      } else {
        // Mock data para desenvolvimento
        userList = generateMockUsers();
      }

      setUsers(userList);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    }
  };

  const loadAssociations = async () => {
    setLoading(true);
    setMessage('');

    try {
      let response: UserCompanyAssociationListResponse;

      if (onLoadAssociations) {
        response = await onLoadAssociations({
          page: currentPage,
          pageSize,
          filters: {
            codempre: selectedCompany !== '0' ? selectedCompany : undefined,
            usuar_id: selectedUser !== '0' ? selectedUser : undefined,
          },
        });
      } else {
        // Mock data para desenvolvimento
        response = generateMockAssociations(currentPage, pageSize, selectedCompany, selectedUser);
      }

      if (response.sucesso) {
        setAssociations(response.associacoes);
        setTotalItems(response.total);
      } else {
        setMessage(response.mensagem || 'Erro ao carregar associações');
        setAssociations([]);
      }
    } catch (error) {
      setMessage('Não foi possível recuperar os registros!');
      setAssociations([]);
    } finally {
      setLoading(false);
    }
  };

  const handleAddAssociation = async () => {
    // Validações
    if (selectedCompany === '0' || selectedUser === '0') {
      alert('Não foi possível incluir a associação! Selecione empresa e usuário.');
      return;
    }

    // Verifica se já existe associação
    if (associations.length > 0) {
      alert('Não foi possível incluir a associação! Já existe associação para esta combinação.');
      return;
    }

    setLoading(true);
    try {
      let result: AssociationOperationResponse;

      if (onAddAssociation) {
        result = await onAddAssociation({
          codempre: selectedCompany,
          usuar_id: selectedUser,
        });
      } else {
        // Mock para desenvolvimento
        result = {
          sucesso: true,
          mensagem: 'Associação incluída com sucesso!',
        };
      }

      if (result.sucesso) {
        setMessage(result.mensagem);
        loadAssociations();
      } else {
        alert(result.mensagem || 'Não foi possível incluir a associação!');
      }
    } catch (error) {
      alert('Não foi possível incluir a associação!');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteAssociations = async () => {
    if (selectedAssociations.size === 0) {
      alert('Selecione pelo menos um item para exclusão.');
      return;
    }

    if (!window.confirm(`Confirma a exclusão de ${selectedAssociations.size} associação(ões)?`)) {
      return;
    }

    setLoading(true);
    try {
      const associationsToDelete = Array.from(selectedAssociations).map((key) => {
        const [codempre, usuar_id] = key.split('|');
        return { codempre, usuar_id };
      });

      let result: AssociationOperationResponse;

      if (onDeleteAssociations) {
        result = await onDeleteAssociations(associationsToDelete);
      } else {
        // Mock para desenvolvimento
        result = {
          sucesso: true,
          mensagem: 'Associação(ões) excluída(s) com sucesso!',
        };
      }

      if (result.sucesso) {
        setMessage(result.mensagem);
        setSelectedAssociations(new Set());
        setCurrentPage(0);
        loadAssociations();
      } else {
        alert(result.mensagem || 'Não foi possível excluir o(s) registro(s)!');
      }
    } catch (error) {
      alert('Não foi possível excluir o(s) registro(s)!');
    } finally {
      setLoading(false);
    }
  };

  const handleCheckboxChange = (codempre: string, usuar_id: string) => {
    const key = `${codempre}|${usuar_id}`;
    setSelectedAssociations((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(key)) {
        newSet.delete(key);
      } else {
        newSet.add(key);
      }
      return newSet;
    });
  };

  const handlePageChange = (newPage: number) => {
    setCurrentPage(newPage);
    setSelectedAssociations(new Set());
  };

  const totalPages = Math.ceil(totalItems / pageSize);

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Associação Usuário X Empresa</h1>
      </div>

      <div className={styles.content}>
        <div className={styles.filterSection}>
          <div className={styles.filterRow}>
            <label className={styles.label}>Empresa:</label>
            <select
              className={styles.select}
              value={selectedCompany}
              onChange={(e) => setSelectedCompany(e.target.value)}
              disabled={loading}
            >
              <option value="0"></option>
              {companies.map((company) => (
                <option key={company.codempre} value={company.codempre}>
                  {company.sigempre}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.filterRow}>
            <label className={styles.label}>Usuário:</label>
            <select
              className={styles.select}
              value={selectedUser}
              onChange={(e) => setSelectedUser(e.target.value)}
              disabled={loading}
            >
              <option value="0"></option>
              {users.map((user) => (
                <option key={user.usuar_id} value={user.usuar_id}>
                  {user.usuar_nome}
                </option>
              ))}
            </select>
          </div>
        </div>

        {message && <div className={styles.message}>{message}</div>}

        {loading ? (
          <div className={styles.loading}>Carregando...</div>
        ) : (
          <>
            {(selectedCompany !== '0' || selectedUser !== '0') && (
              <div className={styles.tableSection}>
                {associations.length > 0 ? (
                  <>
                    <table className={styles.table}>
                      <thead>
                        <tr>
                          <th style={{ width: '20px' }}></th>
                          <th style={{ width: '150px' }}>Empresa</th>
                          <th style={{ width: '330px' }}>Usuário</th>
                        </tr>
                      </thead>
                      <tbody>
                        {associations.map((assoc, index) => {
                          const key = `${assoc.codempre}|${assoc.usuar_id}`;
                          return (
                            <tr
                              key={key}
                              className={index % 2 === 0 ? styles.evenRow : styles.oddRow}
                            >
                              <td className={styles.checkboxCell}>
                                <input
                                  type="checkbox"
                                  checked={selectedAssociations.has(key)}
                                  onChange={() =>
                                    handleCheckboxChange(assoc.codempre, assoc.usuar_id)
                                  }
                                />
                              </td>
                              <td>{assoc.sigempre}</td>
                              <td>{assoc.usuar_nome}</td>
                            </tr>
                          );
                        })}
                      </tbody>
                    </table>

                    {totalPages > 1 && (
                      <div className={styles.pagination}>
                        {currentPage > 0 && (
                          <button onClick={() => handlePageChange(currentPage - 1)}>
                            &lt;Anterior
                          </button>
                        )}
                        <span className={styles.pageInfo}>
                          Página {currentPage + 1} de {totalPages}
                        </span>
                        {currentPage < totalPages - 1 && (
                          <button onClick={() => handlePageChange(currentPage + 1)}>
                            Próxima&gt;
                          </button>
                        )}
                      </div>
                    )}
                  </>
                ) : (
                  <div className={styles.emptyMessage}>Nenhuma associação encontrada</div>
                )}
              </div>
            )}
          </>
        )}

        <div className={styles.buttonSection}>
          <button
            className={styles.button}
            onClick={handleAddAssociation}
            disabled={loading || selectedCompany === '0' || selectedUser === '0'}
          >
            Incluir
          </button>
          <button
            className={styles.button}
            onClick={handleDeleteAssociations}
            disabled={loading || selectedAssociations.size === 0}
          >
            Excluir
          </button>
        </div>
      </div>
    </div>
  );
};

/**
 * Funções para gerar dados mock durante desenvolvimento
 */
function generateMockCompanies(): CompanyOption[] {
  return [
    { codempre: '1', sigempre: 'FURNAS' },
    { codempre: '2', sigempre: 'CHESF' },
    { codempre: '3', sigempre: 'ELETRONORTE' },
    { codempre: '4', sigempre: 'COPEL' },
    { codempre: '5', sigempre: 'CEMIG' },
    { codempre: '6', sigempre: 'ELETROSUL' },
  ];
}

function generateMockUsers(): UserOption[] {
  return [
    { usuar_id: 'admin', usuar_nome: 'ADMINISTRADOR DO SISTEMA' },
    { usuar_id: 'jsilva', usuar_nome: 'JOÃO DA SILVA' },
    { usuar_id: 'mferreira', usuar_nome: 'MARIA FERREIRA' },
    { usuar_id: 'psantos', usuar_nome: 'PEDRO SANTOS' },
    { usuar_id: 'acosta', usuar_nome: 'ANA COSTA' },
  ];
}

function generateMockAssociations(
  page: number,
  pageSize: number,
  selectedCompany: string,
  selectedUser: string
): UserCompanyAssociationListResponse {
  const allAssociations: UserCompanyAssociation[] = [
    {
      codempre: '1',
      sigempre: 'FURNAS',
      usuar_id: 'admin',
      usuar_nome: 'ADMINISTRADOR DO SISTEMA',
    },
    { codempre: '1', sigempre: 'FURNAS', usuar_id: 'jsilva', usuar_nome: 'JOÃO DA SILVA' },
    { codempre: '2', sigempre: 'CHESF', usuar_id: 'mferreira', usuar_nome: 'MARIA FERREIRA' },
    { codempre: '2', sigempre: 'CHESF', usuar_id: 'psantos', usuar_nome: 'PEDRO SANTOS' },
    { codempre: '3', sigempre: 'ELETRONORTE', usuar_id: 'acosta', usuar_nome: 'ANA COSTA' },
    { codempre: '4', sigempre: 'COPEL', usuar_id: 'jsilva', usuar_nome: 'JOÃO DA SILVA' },
    { codempre: '4', sigempre: 'COPEL', usuar_id: 'admin', usuar_nome: 'ADMINISTRADOR DO SISTEMA' },
    { codempre: '5', sigempre: 'CEMIG', usuar_id: 'mferreira', usuar_nome: 'MARIA FERREIRA' },
    { codempre: '6', sigempre: 'ELETROSUL', usuar_id: 'psantos', usuar_nome: 'PEDRO SANTOS' },
  ];

  // Aplicar filtros
  let filteredAssociations = allAssociations;

  if (selectedCompany !== '0') {
    filteredAssociations = filteredAssociations.filter((a) => a.codempre === selectedCompany);
  }

  if (selectedUser !== '0') {
    filteredAssociations = filteredAssociations.filter((a) => a.usuar_id === selectedUser);
  }

  const start = page * pageSize;
  const end = start + pageSize;
  const paginatedAssociations = filteredAssociations.slice(start, end);

  return {
    sucesso: true,
    associacoes: paginatedAssociations,
    total: filteredAssociations.length,
  };
}

export default UserAssociation;

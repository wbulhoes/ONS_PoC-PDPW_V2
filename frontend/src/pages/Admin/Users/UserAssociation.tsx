import React, { useState, useEffect } from 'react';
import styles from './UserAssociation.module.css';

interface Company {
  id: string;
  sigla: string;
  nome: string;
}

interface User {
  id: string;
  nome: string;
}

interface UserAssociation {
  id: string;
  empresaId: string;
  empresaSigla: string;
  usuarioId: string;
  usuarioNome: string;
}

export default function UserAssociation() {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [associations, setAssociations] = useState<UserAssociation[]>([]);
  const [selectedCompany, setSelectedCompany] = useState('');
  const [selectedUser, setSelectedUser] = useState('');
  const [selectedAssociations, setSelectedAssociations] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const itemsPerPage = 5;

  useEffect(() => {
    loadCompanies();
    loadUsers();
    loadAssociations();
  }, [currentPage]);

  const loadCompanies = async () => {
    try {
      // TODO: Implementar chamada para API
      // const response = await api.getCompanies();
      // setCompanies(response.data);

      // Mock data
      const mockCompanies: Company[] = [
        { id: '1', sigla: 'ONS', nome: 'Operador Nacional do Sistema Elétrico' },
        { id: '2', sigla: 'ANEEL', nome: 'Agência Nacional de Energia Elétrica' },
        { id: '3', sigla: 'MME', nome: 'Ministério de Minas e Energia' },
      ];
      setCompanies(mockCompanies);
    } catch (error) {
      console.error('Erro ao carregar empresas:', error);
    }
  };

  const loadUsers = async () => {
    try {
      // TODO: Implementar chamada para API
      // const response = await api.getUsers();
      // setUsers(response.data);

      // Mock data
      const mockUsers: User[] = [
        { id: '1', nome: 'João Silva' },
        { id: '2', nome: 'Maria Santos' },
        { id: '3', nome: 'Pedro Oliveira' },
        { id: '4', nome: 'Ana Costa' },
      ];
      setUsers(mockUsers);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    }
  };

  const loadAssociations = async () => {
    setLoading(true);
    try {
      // TODO: Implementar chamada para API
      // const response = await api.getUserAssociations({ page: currentPage, limit: itemsPerPage });
      // setAssociations(response.data);
      // setTotalPages(response.totalPages);

      // Mock data
      const mockAssociations: UserAssociation[] = [
        { id: '1', empresaId: '1', empresaSigla: 'ONS', usuarioId: '1', usuarioNome: 'João Silva' },
        {
          id: '2',
          empresaId: '1',
          empresaSigla: 'ONS',
          usuarioId: '2',
          usuarioNome: 'Maria Santos',
        },
        {
          id: '3',
          empresaId: '2',
          empresaSigla: 'ANEEL',
          usuarioId: '3',
          usuarioNome: 'Pedro Oliveira',
        },
        { id: '4', empresaId: '3', empresaSigla: 'MME', usuarioId: '4', usuarioNome: 'Ana Costa' },
        {
          id: '5',
          empresaId: '2',
          empresaSigla: 'ANEEL',
          usuarioId: '1',
          usuarioNome: 'João Silva',
        },
      ];

      const startIndex = (currentPage - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const paginatedAssociations = mockAssociations.slice(startIndex, endIndex);

      setAssociations(paginatedAssociations);
      setTotalPages(Math.ceil(mockAssociations.length / itemsPerPage));
    } catch (error) {
      console.error('Erro ao carregar associações:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleAssociationSelection = (associationId: string) => {
    setSelectedAssociations((prev) =>
      prev.includes(associationId)
        ? prev.filter((id) => id !== associationId)
        : [...prev, associationId]
    );
  };

  const handleInclude = async () => {
    if (!selectedCompany || !selectedUser) {
      alert('Selecione uma empresa e um usuário para incluir.');
      return;
    }

    try {
      // TODO: Implementar criação de associação
      console.log('Incluindo associação:', { empresaId: selectedCompany, usuarioId: selectedUser });

      // Limpar seleções e recarregar
      setSelectedCompany('');
      setSelectedUser('');
      loadAssociations();
    } catch (error) {
      console.error('Erro ao incluir associação:', error);
    }
  };

  const handleExclude = async () => {
    if (selectedAssociations.length === 0) {
      alert('Selecione pelo menos uma associação para excluir.');
      return;
    }

    if (
      window.confirm(`Deseja realmente excluir ${selectedAssociations.length} associação(ões)?`)
    ) {
      try {
        // TODO: Implementar exclusão
        console.log('Excluindo associações:', selectedAssociations);

        setSelectedAssociations([]);
        loadAssociations();
      } catch (error) {
        console.error('Erro ao excluir associações:', error);
      }
    }
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  return (
    <div className={styles.container} data-testid="user-association-container">
      <div className={styles.header} data-testid="user-association-header">
        <h1 className={styles.title} data-testid="user-association-title">
          Associação de Usuários
        </h1>
      </div>

      <div className={styles.content}>
        <div className={styles.selectionForm} data-testid="user-association-selection-form">
          <div className={styles.formGroup}>
            <label htmlFor="company" className={styles.label}>
              Empresa:
            </label>
            <select
              id="company"
              value={selectedCompany}
              onChange={(e) => setSelectedCompany(e.target.value)}
              className={styles.select}
              data-testid="user-association-select-company"
            >
              <option value="">Selecione uma empresa...</option>
              {companies.map((company) => (
                <option key={company.id} value={company.id}>
                  {company.sigla} - {company.nome}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="user" className={styles.label}>
              Usuário:
            </label>
            <select
              id="user"
              value={selectedUser}
              onChange={(e) => setSelectedUser(e.target.value)}
              className={styles.select}
              data-testid="user-association-select-user"
            >
              <option value="">Selecione um usuário...</option>
              {users.map((user) => (
                <option key={user.id} value={user.id}>
                  {user.nome}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className={styles.tableContainer} data-testid="user-association-table-container">
          {loading ? (
            <div className={styles.loading} data-testid="user-association-loading">
              Carregando...
            </div>
          ) : (
            <table className={styles.table} data-testid="user-association-table">
              <thead>
                <tr>
                  <th className={styles.checkboxColumn}>
                    <input
                      type="checkbox"
                      checked={
                        selectedAssociations.length === associations.length &&
                        associations.length > 0
                      }
                      onChange={(e) => {
                        if (e.target.checked) {
                          setSelectedAssociations(associations.map((assoc) => assoc.id));
                        } else {
                          setSelectedAssociations([]);
                        }
                      }}
                      data-testid="user-association-select-all"
                    />
                  </th>
                  <th>Empresa</th>
                  <th>Usuário</th>
                </tr>
              </thead>
              <tbody>
                {associations.map((association) => (
                  <tr
                    key={association.id}
                    className={
                      selectedAssociations.includes(association.id) ? styles.selectedRow : ''
                    }
                  >
                    <td>
                      <input
                        type="checkbox"
                        checked={selectedAssociations.includes(association.id)}
                        onChange={() => handleAssociationSelection(association.id)}
                        data-testid={`user-association-checkbox-${association.id}`}
                      />
                    </td>
                    <td data-testid={`user-association-empresa-${association.id}`}>
                      {association.empresaSigla}
                    </td>
                    <td data-testid={`user-association-usuario-${association.id}`}>
                      {association.usuarioNome}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        <div className={styles.pagination} data-testid="user-association-pagination">
          <button
            type="button"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className={styles.pageButton}
            data-testid="user-association-prev-page"
          >
            &lt; Anterior
          </button>
          <span data-testid="user-association-page-info">
            Página {currentPage} de {totalPages}
          </span>
          <button
            type="button"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className={styles.pageButton}
            data-testid="user-association-next-page"
          >
            Próxima &gt;
          </button>
        </div>

        <div className={styles.actions} data-testid="user-association-actions">
          <button
            type="button"
            onClick={handleInclude}
            disabled={!selectedCompany || !selectedUser}
            className={styles.button}
            data-testid="user-association-btn-include"
          >
            Incluir
          </button>
          <button
            type="button"
            onClick={handleExclude}
            disabled={selectedAssociations.length === 0}
            className={styles.button}
            data-testid="user-association-btn-exclude"
          >
            Excluir
          </button>
        </div>
      </div>
    </div>
  );
}

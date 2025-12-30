import React, { useState, useEffect } from 'react';
import styles from './UserTeamAssociation.module.css';

interface Team {
  id: string;
  nome: string;
}

interface User {
  id: string;
  nome: string;
}

interface UserTeamAssociation {
  id: string;
  equipeId: string;
  equipeNome: string;
  usuarioId: string;
  usuarioNome: string;
}

export default function UserTeamAssociation() {
  const [teams, setTeams] = useState<Team[]>([]);
  const [users, setUsers] = useState<User[]>([]);
  const [associations, setAssociations] = useState<UserTeamAssociation[]>([]);
  const [selectedTeam, setSelectedTeam] = useState('');
  const [selectedUser, setSelectedUser] = useState('');
  const [selectedAssociations, setSelectedAssociations] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const itemsPerPage = 5;

  useEffect(() => {
    loadTeams();
    loadUsers();
    loadAssociations();
  }, [currentPage]);

  const loadTeams = async () => {
    try {
      // TODO: Implementar chamada para API
      // const response = await api.getTeams();
      // setTeams(response.data);

      // Mock data
      const mockTeams: Team[] = [
        { id: '1', nome: 'Equipe de Desenvolvimento' },
        { id: '2', nome: 'Equipe de Operação' },
        { id: '3', nome: 'Equipe de Suporte' },
        { id: '4', nome: 'Equipe de Qualidade' },
      ];
      setTeams(mockTeams);
    } catch (error) {
      console.error('Erro ao carregar equipes:', error);
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
        { id: '5', nome: 'Carlos Mendes' },
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
      // const response = await api.getUserTeamAssociations({ page: currentPage, limit: itemsPerPage });
      // setAssociations(response.data);
      // setTotalPages(response.totalPages);

      // Mock data
      const mockAssociations: UserTeamAssociation[] = [
        {
          id: '1',
          equipeId: '1',
          equipeNome: 'Equipe de Desenvolvimento',
          usuarioId: '1',
          usuarioNome: 'João Silva',
        },
        {
          id: '2',
          equipeId: '1',
          equipeNome: 'Equipe de Desenvolvimento',
          usuarioId: '2',
          usuarioNome: 'Maria Santos',
        },
        {
          id: '3',
          equipeId: '2',
          equipeNome: 'Equipe de Operação',
          usuarioId: '3',
          usuarioNome: 'Pedro Oliveira',
        },
        {
          id: '4',
          equipeId: '3',
          equipeNome: 'Equipe de Suporte',
          usuarioId: '4',
          usuarioNome: 'Ana Costa',
        },
        {
          id: '5',
          equipeId: '2',
          equipeNome: 'Equipe de Operação',
          usuarioId: '5',
          usuarioNome: 'Carlos Mendes',
        },
        {
          id: '6',
          equipeId: '4',
          equipeNome: 'Equipe de Qualidade',
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
    if (!selectedTeam || !selectedUser) {
      alert('Selecione uma equipe e um usuário para incluir.');
      return;
    }

    try {
      // TODO: Implementar criação de associação
      console.log('Incluindo associação:', { equipeId: selectedTeam, usuarioId: selectedUser });

      // Limpar seleções e recarregar
      setSelectedTeam('');
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
    <div className={styles.container} data-testid="user-team-association-container">
      <div className={styles.header} data-testid="user-team-association-header">
        <h1 className={styles.title} data-testid="user-team-association-title">
          Associação de Usuários a Equipes PDP
        </h1>
      </div>

      <div className={styles.content}>
        <div className={styles.selectionForm} data-testid="user-team-association-selection-form">
          <div className={styles.formGroup}>
            <label htmlFor="team" className={styles.label}>
              Equipe:
            </label>
            <select
              id="team"
              value={selectedTeam}
              onChange={(e) => setSelectedTeam(e.target.value)}
              className={styles.select}
              data-testid="user-team-association-select-team"
            >
              <option value="">Selecione uma equipe...</option>
              {teams.map((team) => (
                <option key={team.id} value={team.id}>
                  {team.nome}
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
              data-testid="user-team-association-select-user"
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

        <div className={styles.tableContainer} data-testid="user-team-association-table-container">
          {loading ? (
            <div className={styles.loading} data-testid="user-team-association-loading">
              Carregando...
            </div>
          ) : (
            <table className={styles.table} data-testid="user-team-association-table">
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
                      data-testid="user-team-association-select-all"
                    />
                  </th>
                  <th>Equipe</th>
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
                        data-testid={`user-team-association-checkbox-${association.id}`}
                      />
                    </td>
                    <td data-testid={`user-team-association-equipe-${association.id}`}>
                      {association.equipeNome}
                    </td>
                    <td data-testid={`user-team-association-usuario-${association.id}`}>
                      {association.usuarioNome}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        <div className={styles.pagination} data-testid="user-team-association-pagination">
          <button
            type="button"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className={styles.pageButton}
            data-testid="user-team-association-prev-page"
          >
            &lt; Anterior
          </button>
          <span data-testid="user-team-association-page-info">
            Página {currentPage} de {totalPages}
          </span>
          <button
            type="button"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className={styles.pageButton}
            data-testid="user-team-association-next-page"
          >
            Próxima &gt;
          </button>
        </div>

        <div className={styles.actions} data-testid="user-team-association-actions">
          <button
            type="button"
            onClick={handleInclude}
            disabled={!selectedTeam || !selectedUser}
            className={styles.button}
            data-testid="user-team-association-btn-include"
          >
            Incluir
          </button>
          <button
            type="button"
            onClick={handleExclude}
            disabled={selectedAssociations.length === 0}
            className={styles.button}
            data-testid="user-team-association-btn-exclude"
          >
            Excluir
          </button>
        </div>
      </div>
    </div>
  );
}

import React, { useState, useEffect } from 'react';
import styles from './TeamRegistry.module.css';

interface Team {
  id: number;
  nome: string;
  descricao: string;
  ativa: boolean;
  dataCriacao: string;
}

const TeamRegistry: React.FC = () => {
  const [teams, setTeams] = useState<Team[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [selectedTeams, setSelectedTeams] = useState<number[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [editingTeam, setEditingTeam] = useState<Team | null>(null);
  const [formData, setFormData] = useState({
    nome: '',
    descricao: '',
    ativa: true,
  });
  const [isLoading, setIsLoading] = useState(false);

  const itemsPerPage = 5;

  // Mock data - será substituído pela API
  useEffect(() => {
    const mockTeams: Team[] = [
      {
        id: 1,
        nome: 'Equipe Desenvolvimento',
        descricao: 'Equipe responsável pelo desenvolvimento de sistemas',
        ativa: true,
        dataCriacao: '2023-01-15',
      },
      {
        id: 2,
        nome: 'Equipe Testes',
        descricao: 'Equipe responsável pelos testes de qualidade',
        ativa: true,
        dataCriacao: '2023-02-20',
      },
      {
        id: 3,
        nome: 'Equipe Produção',
        descricao: 'Equipe responsável pela operação em produção',
        ativa: false,
        dataCriacao: '2023-03-10',
      },
      {
        id: 4,
        nome: 'Equipe Suporte',
        descricao: 'Equipe de suporte técnico aos usuários',
        ativa: true,
        dataCriacao: '2023-04-05',
      },
      {
        id: 5,
        nome: 'Equipe Análise',
        descricao: 'Equipe de análise de requisitos',
        ativa: true,
        dataCriacao: '2023-05-12',
      },
      {
        id: 6,
        nome: 'Equipe Segurança',
        descricao: 'Equipe responsável pela segurança da informação',
        ativa: true,
        dataCriacao: '2023-06-18',
      },
    ];
    setTeams(mockTeams);
  }, []);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? (e.target as HTMLInputElement).checked : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);

    try {
      if (isEditing && editingTeam) {
        // Atualizar equipe existente
        const updatedTeams = teams.map((team) =>
          team.id === editingTeam.id
            ? { ...team, ...formData, dataCriacao: team.dataCriacao }
            : team
        );
        setTeams(updatedTeams);
        setIsEditing(false);
        setEditingTeam(null);
      } else {
        // Criar nova equipe
        const newTeam: Team = {
          id: Math.max(...teams.map((t) => t.id)) + 1,
          ...formData,
          dataCriacao: new Date().toISOString().split('T')[0],
        };
        setTeams([...teams, newTeam]);
      }

      // Limpar formulário
      setFormData({
        nome: '',
        descricao: '',
        ativa: true,
      });
    } catch (error) {
      console.error('Erro ao salvar equipe:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleEdit = (team: Team) => {
    setIsEditing(true);
    setEditingTeam(team);
    setFormData({
      nome: team.nome,
      descricao: team.descricao,
      ativa: team.ativa,
    });
  };

  const handleDelete = async () => {
    if (selectedTeams.length === 0) return;

    setIsLoading(true);
    try {
      const updatedTeams = teams.filter((team) => !selectedTeams.includes(team.id));
      setTeams(updatedTeams);
      setSelectedTeams([]);
    } catch (error) {
      console.error('Erro ao excluir equipes:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleCancel = () => {
    setIsEditing(false);
    setEditingTeam(null);
    setFormData({
      nome: '',
      descricao: '',
      ativa: true,
    });
  };

  const handleSelectTeam = (teamId: number) => {
    setSelectedTeams((prev) =>
      prev.includes(teamId) ? prev.filter((id) => id !== teamId) : [...prev, teamId]
    );
  };

  const handleSelectAll = () => {
    const currentTeams = paginatedTeams;
    if (selectedTeams.length === currentTeams.length) {
      setSelectedTeams([]);
    } else {
      setSelectedTeams(currentTeams.map((team) => team.id));
    }
  };

  const paginatedTeams = teams.slice((currentPage - 1) * itemsPerPage, currentPage * itemsPerPage);

  const totalPages = Math.ceil(teams.length / itemsPerPage);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    setSelectedTeams([]); // Limpar seleção ao mudar página
  };

  return (
    <div className={styles.container} data-testid="team-registry-container">
      <div className={styles.header}>
        <h1 className={styles.title}>Cadastro de Equipes</h1>
      </div>

      <div className={styles.content}>
        <form className={styles.form} onSubmit={handleSubmit}>
          <div className={styles.formGroup}>
            <label className={styles.label} htmlFor="nome">
              Nome:
            </label>
            <input
              type="text"
              id="nome"
              name="nome"
              value={formData.nome}
              onChange={handleInputChange}
              className={styles.input}
              required
              maxLength={100}
            />
          </div>

          <div className={styles.formGroup}>
            <label className={styles.label} htmlFor="descricao">
              Descrição:
            </label>
            <textarea
              id="descricao"
              name="descricao"
              value={formData.descricao}
              onChange={handleInputChange}
              className={styles.textarea}
              rows={3}
              maxLength={500}
            />
          </div>

          <div className={styles.formGroup}>
            <label className={styles.checkboxLabel}>
              <input
                type="checkbox"
                name="ativa"
                checked={formData.ativa}
                onChange={handleInputChange}
                className={styles.checkbox}
              />
              Ativa
            </label>
          </div>

          <div className={styles.formActions}>
            <button type="submit" className={styles.button} disabled={isLoading}>
              {isLoading ? 'Salvando...' : isEditing ? 'Atualizar' : 'Salvar'}
            </button>
            {isEditing && (
              <button type="button" onClick={handleCancel} className={styles.buttonSecondary}>
                Cancelar
              </button>
            )}
          </div>
        </form>

        <div className={styles.tableContainer}>
          {isLoading && teams.length === 0 ? (
            <div className={styles.loading}>Carregando equipes...</div>
          ) : (
            <>
              <table className={styles.table}>
                <thead>
                  <tr>
                    <th className={styles.checkboxColumn}>
                      <input
                        type="checkbox"
                        checked={
                          selectedTeams.length === paginatedTeams.length &&
                          paginatedTeams.length > 0
                        }
                        onChange={handleSelectAll}
                        data-testid="select-all-checkbox"
                      />
                    </th>
                    <th>Nome</th>
                    <th>Descrição</th>
                    <th>Status</th>
                    <th>Data Criação</th>
                    <th>Ações</th>
                  </tr>
                </thead>
                <tbody>
                  {paginatedTeams.map((team) => (
                    <tr
                      key={team.id}
                      className={selectedTeams.includes(team.id) ? styles.selectedRow : ''}
                    >
                      <td className={styles.checkboxColumn}>
                        <input
                          type="checkbox"
                          checked={selectedTeams.includes(team.id)}
                          onChange={() => handleSelectTeam(team.id)}
                        />
                      </td>
                      <td>{team.nome}</td>
                      <td>{team.descricao}</td>
                      <td>{team.ativa ? 'Ativa' : 'Inativa'}</td>
                      <td>{new Date(team.dataCriacao).toLocaleDateString('pt-BR')}</td>
                      <td>
                        <button
                          onClick={() => handleEdit(team)}
                          className={styles.actionButton}
                          disabled={isLoading}
                        >
                          Editar
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>

              {teams.length === 0 && !isLoading && (
                <div className={styles.emptyState}>Nenhuma equipe cadastrada.</div>
              )}

              {totalPages > 1 && (
                <div className={styles.pagination}>
                  <button
                    onClick={() => handlePageChange(currentPage - 1)}
                    disabled={currentPage === 1}
                    className={styles.pageButton}
                  >
                    Anterior
                  </button>
                  <span>
                    Página {currentPage} de {totalPages}
                  </span>
                  <button
                    onClick={() => handlePageChange(currentPage + 1)}
                    disabled={currentPage === totalPages}
                    className={styles.pageButton}
                  >
                    Próxima
                  </button>
                </div>
              )}

              <div className={styles.actions}>
                <button
                  onClick={handleDelete}
                  disabled={selectedTeams.length === 0 || isLoading}
                  className={styles.deleteButton}
                >
                  Excluir Selecionados ({selectedTeams.length})
                </button>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default TeamRegistry;

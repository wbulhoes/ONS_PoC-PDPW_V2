import React, { useState, useEffect } from 'react';
import styles from './UserRegistry.module.css';

interface User {
  id: string;
  login: string;
  nome: string;
  email: string;
  telefone: string;
}

export default function UserRegistry() {
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUsers, setSelectedUsers] = useState<string[]>([]);
  const [formData, setFormData] = useState({
    login: '',
    nome: '',
    email: '',
    telefone: '',
  });
  const [isEditing, setIsEditing] = useState(false);
  const [loading, setLoading] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const itemsPerPage = 4;

  useEffect(() => {
    loadUsers();
  }, [currentPage]);

  const loadUsers = async () => {
    setLoading(true);
    try {
      // TODO: Implementar chamada para API
      // const response = await api.getUsers({ page: currentPage, limit: itemsPerPage });
      // setUsers(response.data);
      // setTotalPages(response.totalPages);

      // Mock data para desenvolvimento
      const mockUsers: User[] = [
        {
          id: '1',
          login: 'user1',
          nome: 'Usuário Um',
          email: 'user1@exemplo.com',
          telefone: '11999999999',
        },
        {
          id: '2',
          login: 'user2',
          nome: 'Usuário Dois',
          email: 'user2@exemplo.com',
          telefone: '11888888888',
        },
        {
          id: '3',
          login: 'user3',
          nome: 'Usuário Três',
          email: 'user3@exemplo.com',
          telefone: '11777777777',
        },
        {
          id: '4',
          login: 'user4',
          nome: 'Usuário Quatro',
          email: 'user4@exemplo.com',
          telefone: '11666666666',
        },
        {
          id: '5',
          login: 'user5',
          nome: 'Usuário Cinco',
          email: 'user5@exemplo.com',
          telefone: '11555555555',
        },
      ];

      const startIndex = (currentPage - 1) * itemsPerPage;
      const endIndex = startIndex + itemsPerPage;
      const paginatedUsers = mockUsers.slice(startIndex, endIndex);

      setUsers(paginatedUsers);
      setTotalPages(Math.ceil(mockUsers.length / itemsPerPage));
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleUserSelection = (userId: string) => {
    setSelectedUsers((prev) =>
      prev.includes(userId) ? prev.filter((id) => id !== userId) : [...prev, userId]
    );
  };

  const handleSearch = () => {
    loadUsers();
  };

  const handleEdit = () => {
    if (selectedUsers.length === 1) {
      const userToEdit = users.find((user) => user.id === selectedUsers[0]);
      if (userToEdit) {
        setFormData({
          login: userToEdit.login,
          nome: userToEdit.nome,
          email: userToEdit.email,
          telefone: userToEdit.telefone,
        });
        setIsEditing(true);
      }
    }
  };

  const handleSave = async () => {
    try {
      if (isEditing) {
        // TODO: Implementar atualização
        console.log('Atualizando usuário:', formData);
      } else {
        // TODO: Implementar criação
        console.log('Criando usuário:', formData);
      }

      // Limpar formulário e recarregar lista
      handleCancel();
      loadUsers();
    } catch (error) {
      console.error('Erro ao salvar usuário:', error);
    }
  };

  const handleDelete = async () => {
    if (selectedUsers.length === 0) return;

    if (window.confirm(`Deseja realmente excluir ${selectedUsers.length} usuário(s)?`)) {
      try {
        // TODO: Implementar exclusão
        console.log('Excluindo usuários:', selectedUsers);

        setSelectedUsers([]);
        loadUsers();
      } catch (error) {
        console.error('Erro ao excluir usuários:', error);
      }
    }
  };

  const handleCancel = () => {
    setFormData({
      login: '',
      nome: '',
      email: '',
      telefone: '',
    });
    setIsEditing(false);
    setSelectedUsers([]);
  };

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  return (
    <div className={styles.container} data-testid="user-registry-container">
      <div className={styles.header} data-testid="user-registry-header">
        <h1 className={styles.title} data-testid="user-registry-title">
          Cadastro de Usuários
        </h1>
      </div>

      <div className={styles.content}>
        <form className={styles.form} data-testid="user-registry-form">
          <div className={styles.formGroup}>
            <label htmlFor="login" className={styles.label}>
              Login:
            </label>
            <input
              type="text"
              id="login"
              name="login"
              value={formData.login}
              onChange={handleInputChange}
              maxLength={8}
              className={styles.input}
              data-testid="user-registry-input-login"
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="nome" className={styles.label}>
              Nome:
            </label>
            <input
              type="text"
              id="nome"
              name="nome"
              value={formData.nome}
              onChange={handleInputChange}
              maxLength={40}
              className={styles.input}
              data-testid="user-registry-input-nome"
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="email" className={styles.label}>
              E-mail:
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              maxLength={40}
              className={styles.input}
              data-testid="user-registry-input-email"
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="telefone" className={styles.label}>
              Telefone:
            </label>
            <input
              type="text"
              id="telefone"
              name="telefone"
              value={formData.telefone}
              onChange={handleInputChange}
              maxLength={20}
              className={styles.input}
              data-testid="user-registry-input-telefone"
            />
          </div>
        </form>

        <div className={styles.tableContainer} data-testid="user-registry-table-container">
          {loading ? (
            <div className={styles.loading} data-testid="user-registry-loading">
              Carregando...
            </div>
          ) : (
            <table className={styles.table} data-testid="user-registry-table">
              <thead>
                <tr>
                  <th className={styles.checkboxColumn}>
                    <input
                      type="checkbox"
                      checked={selectedUsers.length === users.length && users.length > 0}
                      onChange={(e) => {
                        if (e.target.checked) {
                          setSelectedUsers(users.map((user) => user.id));
                        } else {
                          setSelectedUsers([]);
                        }
                      }}
                      data-testid="user-registry-select-all"
                    />
                  </th>
                  <th>Login</th>
                  <th>Nome</th>
                  <th>E-mail</th>
                  <th>Telefone</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr
                    key={user.id}
                    className={selectedUsers.includes(user.id) ? styles.selectedRow : ''}
                  >
                    <td>
                      <input
                        type="checkbox"
                        checked={selectedUsers.includes(user.id)}
                        onChange={() => handleUserSelection(user.id)}
                        data-testid={`user-registry-checkbox-${user.id}`}
                      />
                    </td>
                    <td data-testid={`user-registry-login-${user.id}`}>{user.login}</td>
                    <td data-testid={`user-registry-nome-${user.id}`}>{user.nome}</td>
                    <td data-testid={`user-registry-email-${user.id}`}>{user.email}</td>
                    <td data-testid={`user-registry-telefone-${user.id}`}>{user.telefone}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        <div className={styles.pagination} data-testid="user-registry-pagination">
          <button
            type="button"
            onClick={() => handlePageChange(currentPage - 1)}
            disabled={currentPage === 1}
            className={styles.pageButton}
            data-testid="user-registry-prev-page"
          >
            &lt; Anterior
          </button>
          <span data-testid="user-registry-page-info">
            Página {currentPage} de {totalPages}
          </span>
          <button
            type="button"
            onClick={() => handlePageChange(currentPage + 1)}
            disabled={currentPage === totalPages}
            className={styles.pageButton}
            data-testid="user-registry-next-page"
          >
            Próxima &gt;
          </button>
        </div>

        <div className={styles.actions} data-testid="user-registry-actions">
          <button
            type="button"
            onClick={handleSearch}
            className={styles.button}
            data-testid="user-registry-btn-search"
          >
            Pesquisar
          </button>
          <button
            type="button"
            onClick={handleEdit}
            disabled={selectedUsers.length !== 1}
            className={styles.button}
            data-testid="user-registry-btn-edit"
          >
            Alterar
          </button>
          <button
            type="button"
            onClick={handleSave}
            disabled={!formData.login.trim() || !formData.nome.trim()}
            className={styles.button}
            data-testid="user-registry-btn-save"
          >
            Salvar
          </button>
          <button
            type="button"
            onClick={handleDelete}
            disabled={selectedUsers.length === 0}
            className={styles.button}
            data-testid="user-registry-btn-delete"
          >
            Excluir
          </button>
          <button
            type="button"
            onClick={handleCancel}
            className={styles.button}
            data-testid="user-registry-btn-cancel"
          >
            Cancelar
          </button>
        </div>
      </div>
    </div>
  );
}

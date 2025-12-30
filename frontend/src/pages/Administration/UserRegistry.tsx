/**
 * Componente: UserRegistry
 * Tela: Cadastro de Usuários (frmCadUsuario.aspx)
 *
 * Funcionalidades:
 * - Formulário de cadastro com Login, Nome, E-mail e Telefone
 * - Listagem paginada de usuários (4 por página)
 * - Pesquisa com filtros (login, nome, email, telefone)
 * - Inclusão, alteração e exclusão de usuários
 * - Seleção múltipla para exclusão
 */

import React, { useState, useEffect } from 'react';
import {
  User,
  UserFormData,
  UserListResponse,
  UserFilters,
  UserPaginationParams,
  UserFormMode,
} from '../../types/user';
import styles from './UserRegistry.module.css';

interface UserRegistryProps {
  onLoadUsers?: (params: UserPaginationParams) => Promise<UserListResponse>;
  onSaveUser?: (
    user: UserFormData,
    mode: UserFormMode
  ) => Promise<{ sucesso: boolean; mensagem: string }>;
  onDeleteUsers?: (userIds: number[]) => Promise<{ sucesso: boolean; mensagem: string }>;
}

const UserRegistry: React.FC<UserRegistryProps> = ({ onLoadUsers, onSaveUser, onDeleteUsers }) => {
  const [formData, setFormData] = useState<UserFormData>({
    nome: '',
    email: '',
    telefone: '',
    equipePDPId: 0,
    perfil: '',
  });

  const [filters, setFilters] = useState<UserFilters>({});
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUsers, setSelectedUsers] = useState<Set<number>>(new Set());
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState<'success' | 'error' | null>(null);
  const [formMode, setFormMode] = useState<UserFormMode>(UserFormMode.CREATE);

  // Paginação
  const [currentPage, setCurrentPage] = useState(0);
  const [pageSize] = useState(4); // 4 registros por página (conforme legado)
  const [totalItems, setTotalItems] = useState(0);

  // Estado dos botões
  const [buttonsState, setButtonsState] = useState({
    pesquisar: true,
    alterar: false,
    salvar: true,
    excluir: false,
    cancelar: true,
  });

  useEffect(() => {
    // Não carrega automaticamente, só após pesquisa
  }, []);

  const loadUsers = async () => {
    setLoading(true);
    setMessage('');
    setMessageType(null);

    try {
      let response: UserListResponse;

      if (onLoadUsers) {
        response = await onLoadUsers({
          page: currentPage,
          pageSize,
          filters,
        });
      } else {
        response = { sucesso: true, usuarios: [], total: 0 };
      }

      if (response.sucesso) {
        setUsers(response.usuarios);
        setTotalItems(response.total);

        // Habilita botões de alteração e exclusão se houver registros
        if (response.usuarios.length > 0) {
          setButtonsState((prev) => ({
            ...prev,
            alterar: true,
            excluir: true,
          }));
        }
      } else {
        setMessage(response.mensagem || 'Erro ao carregar usuários');
        setMessageType('error');
        setUsers([]);
      }
    } catch (error) {
      setMessage('Erro ao carregar usuários');
      setMessageType('error');
      setUsers([]);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (field: keyof UserFormData, value: string | number) => {
    setFormData((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handleFilterChange = (field: keyof UserFilters, value: string) => {
    setFilters((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const handlePesquisar = () => {
    setCurrentPage(0);
    setSelectedUsers(new Set());
    loadUsers();
  };

  const handleAlterar = () => {
    if (selectedUsers.size === 0) {
      alert('Selecione pelo menos um item para alteração.');
      return;
    }

    if (selectedUsers.size > 1) {
      alert('Marque somente um item para alteração!');
      return;
    }

    const userId = Array.from(selectedUsers)[0];
    const user = users.find((u) => u.id === userId);

    if (user) {
      setFormData({
        nome: user.nome,
        email: user.email,
        telefone: user.telefone,
        equipePDPId: user.equipePDPId || 0,
        perfil: user.perfil,
      });
      setFormMode(UserFormMode.EDIT);
      setButtonsState({
        pesquisar: false,
        alterar: false,
        salvar: true,
        excluir: false,
        cancelar: true,
      });
    }
  };

  const handleSalvar = async () => {
    // Validação
    if (!formData.nome || !formData.email || !formData.telefone || !formData.perfil) {
      alert('Preencha todos os campos obrigatórios.');
      return;
    }

    setLoading(true);
    try {
      let result: { sucesso: boolean; mensagem: string };

      if (onSaveUser) {
        result = await onSaveUser(formData, formMode);
      } else {
        result = { sucesso: true, mensagem: 'Usuário salvo com sucesso!' };
      }

      if (result.sucesso) {
        handleCancelar();
        await loadUsers();
        setMessage(result.mensagem);
        setMessageType('success');
      } else {
        setMessage(result.mensagem);
        setMessageType('error');
      }
    } catch (error) {
      setMessage('Não foi possível salvar o usuário!');
      setMessageType('error');
    } finally {
      setLoading(false);
    }
  };

  const handleExcluir = async () => {
    if (selectedUsers.size === 0) {
      alert('Selecione pelo menos um item para exclusão.');
      return;
    }

    if (!window.confirm(`Confirma a exclusão de ${selectedUsers.size} usuário(s)?`)) {
      return;
    }

    setLoading(true);
    try {
      let result: { sucesso: boolean; mensagem: string };

      if (onDeleteUsers) {
        result = await onDeleteUsers(Array.from(selectedUsers));
      } else {
        result = { sucesso: true, mensagem: 'Usuário(s) excluído(s) com sucesso!' };
      }

      if (result.sucesso) {
        setSelectedUsers(new Set());
        await loadUsers();
        setMessage(result.mensagem);
        setMessageType('success');
      } else {
        setMessage(result.mensagem || 'Não foi possível excluir o(s) registro(s)!');
        setMessageType('error');
      }
    } catch (error) {
      setMessage('Não foi possível excluir o(s) registro(s)!');
      setMessageType('error');
    } finally {
      setLoading(false);
    }
  };

  const handleCancelar = () => {
    setFormData({ nome: '', email: '', telefone: '', equipePDPId: 0, perfil: '' });
    setFormMode(UserFormMode.CREATE);
    setSelectedUsers(new Set());
    setButtonsState({
      pesquisar: true,
      alterar: false,
      salvar: true,
      excluir: false,
      cancelar: true,
    });
  };

  const handleCheckboxChange = (userId: number) => {
    setSelectedUsers((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(userId)) {
        newSet.delete(userId);
      } else {
        newSet.add(userId);
      }
      return newSet;
    });
  };

  const handlePageChange = (newPage: number) => {
    setCurrentPage(newPage);
    setSelectedUsers(new Set());
  };

  useEffect(() => {
    if (currentPage >= 0 && users.length > 0) {
      loadUsers();
    }
  }, [currentPage]);

  const totalPages = Math.ceil(totalItems / pageSize);

  // Auto-dismiss toast messages
  useEffect(() => {
    if (!message || !messageType) return;
    const timeout = messageType === 'success' ? 3000 : 5000;
    const id = setTimeout(() => {
      setMessage('');
      setMessageType(null);
    }, timeout);
    return () => clearTimeout(id);
  }, [message, messageType]);

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Cadastro de Usuários</h1>
      </div>

      <div className={styles.content}>
        <div className={styles.formSection}>
          <div className={styles.formRow}>
            <label className={styles.label}>Nome:&nbsp;</label>
            <input
              type="text"
              className={styles.inputLong}
              value={formData.nome}
              onChange={(e) => handleInputChange('nome', e.target.value)}
              maxLength={100}
            />
          </div>

          <div className={styles.formRow}>
            <label className={styles.label}>E-mail:&nbsp;</label>
            <input
              type="email"
              className={styles.inputLong}
              value={formData.email}
              onChange={(e) => handleInputChange('email', e.target.value)}
              maxLength={100}
            />
          </div>

          <div className={styles.formRow}>
            <label className={styles.label}>Telefone:&nbsp;</label>
            <input
              type="text"
              className={styles.inputMedium}
              value={formData.telefone}
              onChange={(e) => handleInputChange('telefone', e.target.value)}
              maxLength={20}
            />
          </div>

          <div className={styles.formRow}>
            <label className={styles.label}>Equipe PDP:&nbsp;</label>
            <input
              type="number"
              className={styles.inputMedium}
              value={formData.equipePDPId}
              onChange={(e) => handleInputChange('equipePDPId', Number(e.target.value))}
              min={0}
            />
          </div>

          <div className={styles.formRow}>
            <label className={styles.label}>Perfil:&nbsp;</label>
            <select
              className={styles.inputMedium}
              value={formData.perfil}
              onChange={(e) => handleInputChange('perfil', e.target.value)}
              required
            >
              <option value="">Selecione...</option>
              <option value="Operador">Operador</option>
              <option value="Analista">Analista</option>
              <option value="Administrador">Administrador</option>
              <option value="Coordenador">Coordenador</option>
              <option value="Consultor">Consultor</option>
            </select>
          </div>
        </div>

        {message && (
          <div
            className={
              messageType === 'error' ? styles.errorBanner : styles.successBanner
            }
          >
            <span>{message}</span>
            {messageType === 'error' && (
              <button className={styles.retryButton} onClick={loadUsers} disabled={loading}>
                Tentar novamente
              </button>
            )}
          </div>
        )}

        {loading ? (
          <div className={styles.tableSection}>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th style={{ width: '20px' }}></th>
                  <th style={{ width: '200px' }}>Nome</th>
                  <th style={{ width: '200px' }}>E-mail</th>
                  <th style={{ width: '100px' }}>Telefone</th>
                  <th style={{ width: '100px' }}>Equipe</th>
                  <th style={{ width: '100px' }}>Perfil</th>
                </tr>
              </thead>
              <tbody>
                {Array.from({ length: pageSize }).map((_, idx) => (
                  <tr key={idx} className={idx % 2 === 0 ? styles.evenRow : styles.oddRow}>
                    <td className={styles.checkboxCell}>
                      <div className={styles.skeletonBox} />
                    </td>
                    <td><div className={styles.skeletonText} /></td>
                    <td><div className={styles.skeletonText} /></td>
                    <td><div className={styles.skeletonText} /></td>
                    <td><div className={styles.skeletonText} /></td>
                    <td><div className={styles.skeletonText} /></td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        ) : (
          <>
            {users.length > 0 && (
              <div className={styles.tableSection}>
                <table className={styles.table}>
                  <thead>
                    <tr>
                      <th style={{ width: '20px' }}></th>
                      <th style={{ width: '200px' }}>Nome</th>
                      <th style={{ width: '200px' }}>E-mail</th>
                      <th style={{ width: '100px' }}>Telefone</th>
                      <th style={{ width: '100px' }}>Equipe</th>
                      <th style={{ width: '100px' }}>Perfil</th>
                    </tr>
                  </thead>
                  <tbody>
                    {users.map((user, index) => (
                      <tr
                        key={user.id}
                        className={index % 2 === 0 ? styles.evenRow : styles.oddRow}
                      >
                        <td className={styles.checkboxCell}>
                          <input
                            type="checkbox"
                            checked={selectedUsers.has(user.id)}
                            onChange={() => handleCheckboxChange(user.id)}
                          />
                        </td>
                        <td>{user.nome}</td>
                        <td>{user.email}</td>
                        <td>{user.telefone}</td>
                        <td>{user.equipePDPId}</td>
                        <td>{user.perfil}</td>
                      </tr>
                    ))}
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
                      <button onClick={() => handlePageChange(currentPage + 1)}>Próxima&gt;</button>
                    )}
                  </div>
                )}
              </div>
            )}
          </>
        )}

        <div className={styles.buttonSection}>
          <button
            className={styles.button}
            onClick={handlePesquisar}
            disabled={!buttonsState.pesquisar || loading}
          >
            Pesquisar
          </button>
          <button
            className={styles.button}
            onClick={handleAlterar}
            disabled={!buttonsState.alterar || loading}
          >
            Alterar
          </button>
          <button
            className={styles.button}
            onClick={handleSalvar}
            disabled={!buttonsState.salvar || loading}
          >
            Salvar
          </button>
          <button
            className={styles.button}
            onClick={handleExcluir}
            disabled={!buttonsState.excluir || loading}
          >
            Excluir
          </button>
          <button
            className={styles.button}
            onClick={handleCancelar}
            disabled={!buttonsState.cancelar || loading}
          >
            Cancelar
          </button>
        </div>
      </div>
    </div>
  );
};

export default UserRegistry;

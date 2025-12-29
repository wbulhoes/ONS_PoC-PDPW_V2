import React, { useState, useEffect } from 'react';
import styles from './UserManagement.module.css';
import { User } from '../../../types/user';
import { userService } from '../../../services/userService';

const initialForm: Omit<User, 'id'> = {
  nome: '',
  email: '',
  telefone: '',
  equipePDPId: 0,
  perfil: '',
};

const UserManagementPage: React.FC = () => {
  console.log('UserManagementPage montado');
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [selectedIds, setSelectedIds] = useState<number[]>([]);
  const [formData, setFormData] = useState<Omit<User, 'id'>>(initialForm);
  const [editId, setEditId] = useState<number | null>(null);

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const result = await userService.getAll();
      setUsers(result);
    } catch (err) {
      setError('Erro ao carregar usuários.');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: name === 'equipePDPId' ? Number(value) : value }));
  };

  const handleSave = async () => {
    if (!formData.nome || !formData.email || !formData.telefone || !formData.perfil) {
      alert('Preencha todos os campos obrigatórios.');
      return;
    }
    try {
      if (editId) {
        await userService.update(editId, formData);
        alert('Usuário atualizado com sucesso!');
      } else {
        await userService.create(formData);
        alert('Usuário criado com sucesso!');
      }
      handleCancel();
      loadUsers();
    } catch (err) {
      alert('Erro ao salvar usuário.');
    }
  };

  const handleEdit = () => {
    if (selectedIds.length !== 1) {
      alert('Selecione apenas um usuário para alterar.');
      return;
    }
    const userToEdit = users.find(u => u.id === selectedIds[0]);
    if (userToEdit) {
      const { id, ...rest } = userToEdit;
      setFormData(rest);
      setEditId(userToEdit.id);
    }
  };

  const handleDelete = async () => {
    if (selectedIds.length === 0) {
      alert('Selecione usuários para excluir.');
      return;
    }
    if (window.confirm('Tem certeza que deseja excluir os usuários selecionados?')) {
      try {
        await userService.delete(selectedIds);
        loadUsers();
        setSelectedIds([]);
      } catch (err) {
        alert('Erro ao excluir usuários.');
      }
    }
  };

  const handleCancel = () => {
    setFormData(initialForm);
    setEditId(null);
    setSelectedIds([]);
  };

  const handleSelect = (id: number) => {
    setSelectedIds(prev => {
      if (prev.includes(id)) {
        return prev.filter(item => item !== id);
      } else {
        return [...prev, id];
      }
    });
  };

  return (
    <div className={styles.container}>
      <h1 className={styles.title}>Cadastro de Usuários</h1>
      <div className={styles.formContainer}>
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="nome">Nome:</label>
            <input
              type="text"
              id="nome"
              name="nome"
              value={formData.nome}
              onChange={handleInputChange}
              maxLength={100}
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="email">E-mail:</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              maxLength={100}
            />
          </div>
        </div>
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="telefone">Telefone:</label>
            <input
              type="text"
              id="telefone"
              name="telefone"
              value={formData.telefone}
              onChange={handleInputChange}
              maxLength={20}
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="equipePDPId">Equipe PDP ID:</label>
            <input
              type="number"
              id="equipePDPId"
              name="equipePDPId"
              value={formData.equipePDPId}
              onChange={handleInputChange}
              min={0}
            />
          </div>
          <div className={styles.formGroup}>
            <label htmlFor="perfil">Perfil:</label>
            <select
              id="perfil"
              name="perfil"
              value={formData.perfil}
              onChange={handleInputChange}
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
        <div className={styles.actions}>
          <button className={`${styles.button} ${styles.buttonPrimary}`} onClick={handleSave}>
            {editId ? 'Salvar Alterações' : 'Incluir'}
          </button>
          <button className={`${styles.button} ${styles.buttonSecondary}`} onClick={handleEdit} disabled={selectedIds.length !== 1}>
            Alterar
          </button>
          <button className={`${styles.button} ${styles.buttonDanger}`} onClick={handleDelete} disabled={selectedIds.length === 0}>
            Excluir
          </button>
          <button className={`${styles.button} ${styles.buttonSecondary}`} onClick={handleCancel}>
            Cancelar
          </button>
        </div>
      </div>
      {loading && <p>Carregando...</p>}
      {error && <p className={styles.errorMessage}>{error}</p>}
      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th></th>
              <th>Nome</th>
              <th>E-mail</th>
              <th>Telefone</th>
              <th>Equipe</th>
              <th>Perfil</th>
            </tr>
          </thead>
          <tbody>
            {users.map(user => (
              <tr key={user.id}>
                <td>
                  <input
                    type="checkbox"
                    checked={selectedIds.includes(user.id)}
                    onChange={() => handleSelect(user.id)}
                  />
                </td>
                <td>{user.nome}</td>
                <td>{user.email}</td>
                <td>{user.telefone}</td>
                <td>{user.equipePDPId ?? '-'}</td>
                <td>{user.perfil}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default UserManagementPage;

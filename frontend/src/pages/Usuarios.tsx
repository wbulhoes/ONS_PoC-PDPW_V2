import React, { useEffect, useState } from 'react';
import styles from './Usuarios.module.css';

interface Usuario {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  equipePDPId: number | null;
  perfil: string;
}

interface EquipePDP {
  id: number;
  nome: string;
}

const Usuarios: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [equipes, setEquipes] = useState<EquipePDP[]>([]);
  const [form, setForm] = useState<Omit<Usuario, 'id'>>({ nome: '', email: '', telefone: '', equipePDPId: 0, perfil: '' });
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    carregarUsuarios();
    carregarEquipes();
  }, []);

  const carregarUsuarios = async () => {
    setLoading(true);
    setError(null);
    try {
      const resp = await fetch('/api/usuarios');
      const data = await resp.json();
      setUsuarios(Array.isArray(data) ? data : (data.usuarios || []));
    } catch (err) {
      setError('Erro ao carregar usuários');
    } finally {
      setLoading(false);
    }
  };

  const carregarEquipes = async () => {
    try {
      const resp = await fetch('/api/equipespdp');
      const data = await resp.json();
      setEquipes(data);
    } catch {}
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: name === 'equipePDPId' ? Number(value) : value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      if (editandoId) {
        await fetch(`/api/usuarios/${editandoId}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(form)
        });
      } else {
        await fetch('/api/usuarios', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(form)
        });
      }
      setForm({ nome: '', email: '', telefone: '', equipePDPId: 0, perfil: '' });
      setEditandoId(null);
      await carregarUsuarios();
    } catch (err) {
      setError('Erro ao salvar usuário');
    } finally {
      setLoading(false);
    }
  };

  const handleEditar = (usuario: Usuario) => {
    const { id, ...rest } = usuario;
    setForm(rest);
    setEditandoId(usuario.id);
  };

  const handleRemover = async (id: number) => {
    if (!window.confirm('Deseja remover este usuário?')) return;
    setLoading(true);
    try {
      await fetch(`/api/usuarios/${id}`, { method: 'DELETE' });
      await carregarUsuarios();
    } catch {
      setError('Erro ao remover usuário');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Cadastro de Usuários</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label>Nome:</label>
          <input name="nome" value={form.nome} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label>E-mail:</label>
          <input name="email" type="email" value={form.email} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label>Telefone:</label>
          <input name="telefone" value={form.telefone} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label>Equipe PDP:</label>
          <select name="equipePDPId" value={form.equipePDPId} onChange={handleChange} required>
            <option value="">Selecione...</option>
            {equipes.map(eq => <option key={eq.id} value={eq.id}>{eq.nome}</option>)}
          </select>
        </div>
        <div>
          <label>Perfil:</label>
          <select name="perfil" value={form.perfil} onChange={handleChange} required>
            <option value="">Selecione...</option>
            <option value="Operador">Operador</option>
            <option value="Analista">Analista</option>
            <option value="Administrador">Administrador</option>
            <option value="Coordenador">Coordenador</option>
            <option value="Consultor">Consultor</option>
          </select>
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editandoId ? 'Atualizar' : 'Salvar'}</button>
        {editandoId && <button type="button" className={styles.btnSec} onClick={() => { setForm({ nome: '', email: '', telefone: '', equipePDPId: 0, perfil: '' }); setEditandoId(null); }}>Cancelar</button>}
      </form>

      <h2>Usuários Cadastrados</h2>
      {error && <div className={styles.error}>{error}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Nome</th>
              <th>E-mail</th>
              <th>Telefone</th>
              <th>Equipe</th>
              <th>Perfil</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {usuarios.map(u => (
              <tr key={u.id}>
                <td>{u.nome}</td>
                <td>{u.email}</td>
                <td>{u.telefone}</td>
                <td>{u.equipePDPId ? (equipes.find(eq => eq.id === u.equipePDPId)?.nome || '-') : '-'}</td>
                <td>{u.perfil}</td>
                <td>
                  <button className={styles.btnSec} onClick={() => handleEditar(u)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleRemover(u.id)}>Remover</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {(!loading && usuarios.length === 0) && <div className={styles.noData}>Nenhum usuário cadastrado</div>}
    </div>
  );
};

export default Usuarios;

import React, { useState, useEffect } from 'react';
import styles from './CompanyManagement.module.css';
import { companyService, Empresa, CreateEmpresaDto, UpdateEmpresaDto } from '../../../services/companyService';

const initialForm: CreateEmpresaDto = {
  nome: '',
  cnpj: '',
  telefone: '',
  email: '',
  endereco: '',
};

const CompanyManagement: React.FC = () => {
  const [empresas, setEmpresas] = useState<Empresa[]>([]);
  const [form, setForm] = useState<CreateEmpresaDto>(initialForm);
  const [editId, setEditId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadEmpresas();
  }, []);

  const loadEmpresas = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await companyService.getAll();
      setEmpresas(data);
    } catch (err) {
      setError('Erro ao carregar empresas');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      if (editId) {
        await companyService.update(editId, form as UpdateEmpresaDto);
        setSuccess('Empresa atualizada com sucesso!');
      } else {
        await companyService.create(form);
        setSuccess('Empresa cadastrada com sucesso!');
      }
      setForm(initialForm);
      setEditId(null);
      await loadEmpresas();
    } catch (err: any) {
      setError('Erro ao salvar empresa');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (empresa: Empresa) => {
    setForm({
      nome: empresa.nome,
      cnpj: empresa.cnpj,
      telefone: empresa.telefone,
      email: empresa.email,
      endereco: empresa.endereco,
    });
    setEditId(empresa.id);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja remover esta empresa?')) return;
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      await companyService.delete(id);
      setSuccess('Empresa removida com sucesso!');
      await loadEmpresas();
    } catch {
      setError('Erro ao remover empresa');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Cadastro de Empresas</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label htmlFor="nome">Nome:</label>
          <input id="nome" name="nome" value={form.nome} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label htmlFor="cnpj">CNPJ:</label>
          <input id="cnpj" name="cnpj" value={form.cnpj} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label htmlFor="telefone">Telefone:</label>
          <input id="telefone" name="telefone" value={form.telefone} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label htmlFor="email">E-mail:</label>
          <input id="email" name="email" type="email" value={form.email} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label htmlFor="endereco">Endereço:</label>
          <input id="endereco" name="endereco" value={form.endereco} onChange={handleChange} required maxLength={100} />
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editId ? 'Atualizar' : 'Salvar'}</button>
        {editId && <button type="button" className={styles.btnSec} onClick={() => { setForm(initialForm); setEditId(null); }}>Cancelar</button>}
      </form>

      <h2>Empresas Cadastradas</h2>
      {error && <div className={styles.error}>{error}</div>}
      {success && <div className={styles.success}>{success}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Nome</th>
              <th>CNPJ</th>
              <th>Telefone</th>
              <th>E-mail</th>
              <th>Endereço</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {empresas.map(e => (
              <tr key={e.id}>
                <td>{e.nome}</td>
                <td>{e.cnpj}</td>
                <td>{e.telefone}</td>
                <td>{e.email}</td>
                <td>{e.endereco}</td>
                <td>
                  <button className={styles.btnSec} onClick={() => handleEdit(e)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleDelete(e.id)}>Remover</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {(!loading && empresas.length === 0) && <div className={styles.noData}>Nenhuma empresa cadastrada</div>}
    </div>
  );
};

export default CompanyManagement;

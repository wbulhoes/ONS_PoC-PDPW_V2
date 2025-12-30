/**
 * Componente: Cadastro/Consulta de Empresas
 * Migração de: legado/pdpw/frmCnsEmpresa.aspx
 *
 * Funcionalidades:
 * - Listagem paginada de empresas
 * - Exibição de dados: código, nome, sigla, GTPO, controladora, região, sistema
 * - Paginação customizada
 */

import React, { useState, useEffect } from 'react';
import styles from './Company.module.css';
import { companyService, CreateEmpresaDto, UpdateEmpresaDto, Empresa } from '../../services/companyService';

const initialForm: CreateEmpresaDto = {
  nome: '',
  cnpj: '',
  telefone: '',
  email: '',
  endereco: '',
};

const Company: React.FC = () => {
  const [empresas, setEmpresas] = useState<Empresa[]>([]);
  const [form, setForm] = useState<CreateEmpresaDto>(initialForm);
  const [editandoId, setEditandoId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    carregarEmpresas();
  }, []);

  const carregarEmpresas = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await companyService.getAll();
      setEmpresas(data);
    } catch (err: any) {
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
      if (editandoId) {
        await companyService.update(editandoId, form as UpdateEmpresaDto);
        setSuccess('Empresa atualizada com sucesso!');
      } else {
        await companyService.create(form);
        setSuccess('Empresa cadastrada com sucesso!');
      }
      setForm(initialForm);
      setEditandoId(null);
      await carregarEmpresas();
    } catch (err: any) {
      setError('Erro ao salvar empresa');
    } finally {
      setLoading(false);
    }
  };

  const handleEditar = (empresa: Empresa) => {
    setForm({
      nome: empresa.nome,
      cnpj: empresa.cnpj,
      telefone: empresa.telefone,
      email: empresa.email,
      endereco: empresa.endereco,
    });
    setEditandoId(empresa.id);
  };

  const handleRemover = async (id: number) => {
    if (!window.confirm('Deseja remover esta empresa?')) return;
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      await companyService.delete(id);
      setSuccess('Empresa removida com sucesso!');
      await carregarEmpresas();
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
          <label>Nome:</label>
          <input name="nome" value={form.nome} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label>CNPJ:</label>
          <input name="cnpj" value={form.cnpj} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label>Telefone:</label>
          <input name="telefone" value={form.telefone} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label>E-mail:</label>
          <input name="email" type="email" value={form.email} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label>Endereço:</label>
          <input name="endereco" value={form.endereco} onChange={handleChange} required maxLength={100} />
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editandoId ? 'Atualizar' : 'Salvar'}</button>
        {editandoId && <button type="button" className={styles.btnSec} onClick={() => { setForm(initialForm); setEditandoId(null); }}>Cancelar</button>}
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
                  <button className={styles.btnSec} onClick={() => handleEditar(e)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleRemover(e.id)}>Remover</button>
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

export default Company;

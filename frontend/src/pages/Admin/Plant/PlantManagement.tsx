import React, { useState, useEffect } from 'react';
import styles from './PlantManagement.module.css';
import { plantService, Usina, CreateUsinaDto, UpdateUsinaDto } from '../../../services/plantService';

const initialForm: CreateUsinaDto = {
  codigo: '',
  nome: '',
  tipoUsinaId: 0,
  empresaId: 0,
  capacidadeInstalada: 0,
  localizacao: '',
  dataOperacao: '',
  ativo: true,
};

const PlantManagement: React.FC = () => {
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [form, setForm] = useState<CreateUsinaDto>(initialForm);
  const [editId, setEditId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadUsinas();
  }, []);

  const loadUsinas = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await plantService.getAll();
      setUsinas(data);
    } catch (err) {
      setError('Erro ao carregar usinas');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setForm(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : name === 'tipoUsinaId' || name === 'empresaId' || name === 'capacidadeInstalada' ? Number(value) : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      if (editId) {
        await plantService.update(editId, form as UpdateUsinaDto);
        setSuccess('Usina atualizada com sucesso!');
      } else {
        await plantService.create(form);
        setSuccess('Usina cadastrada com sucesso!');
      }
      setForm(initialForm);
      setEditId(null);
      await loadUsinas();
    } catch (err: any) {
      setError('Erro ao salvar usina');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (usina: Usina) => {
    setForm({
      codigo: usina.codigo,
      nome: usina.nome,
      tipoUsinaId: usina.tipoUsinaId,
      empresaId: usina.empresaId,
      capacidadeInstalada: usina.capacidadeInstalada,
      localizacao: usina.localizacao || '',
      dataOperacao: usina.dataOperacao || '',
      ativo: usina.ativo,
    });
    setEditId(usina.id);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja remover esta usina?')) return;
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      await plantService.delete(id);
      setSuccess('Usina removida com sucesso!');
      await loadUsinas();
    } catch {
      setError('Erro ao remover usina');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Cadastro de Usinas Geradoras</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label htmlFor="codigo">Código:</label>
          <input id="codigo" name="codigo" value={form.codigo} onChange={handleChange} required maxLength={20} />
        </div>
        <div>
          <label htmlFor="nome">Nome:</label>
          <input id="nome" name="nome" value={form.nome} onChange={handleChange} required maxLength={100} />
        </div>
        <div>
          <label htmlFor="tipoUsinaId">Tipo Usina ID:</label>
          <input id="tipoUsinaId" name="tipoUsinaId" type="number" value={form.tipoUsinaId} onChange={handleChange} min={0} />
        </div>
        <div>
          <label htmlFor="empresaId">Empresa ID:</label>
          <input id="empresaId" name="empresaId" type="number" value={form.empresaId} onChange={handleChange} min={0} />
        </div>
        <div>
          <label htmlFor="capacidadeInstalada">Capacidade Instalada (MW):</label>
          <input id="capacidadeInstalada" name="capacidadeInstalada" type="number" value={form.capacidadeInstalada} onChange={handleChange} min={0} />
        </div>
        <div>
          <label htmlFor="localizacao">Localização:</label>
          <input id="localizacao" name="localizacao" value={form.localizacao} onChange={handleChange} maxLength={100} />
        </div>
        <div>
          <label htmlFor="dataOperacao">Data de Operação:</label>
          <input id="dataOperacao" name="dataOperacao" type="date" value={form.dataOperacao || ''} onChange={handleChange} />
        </div>
        <div>
          <label htmlFor="ativo">Ativo:</label>
          <input id="ativo" name="ativo" type="checkbox" checked={form.ativo} onChange={handleChange} />
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editId ? 'Atualizar' : 'Salvar'}</button>
        {editId && <button type="button" className={styles.btnSec} onClick={() => { setForm(initialForm); setEditId(null); }}>Cancelar</button>}
      </form>

      <h2>Usinas Cadastradas</h2>
      {error && <div className={styles.error}>{error}</div>}
      {success && <div className={styles.success}>{success}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Código</th>
              <th>Nome</th>
              <th>Tipo</th>
              <th>Empresa</th>
              <th>Capacidade</th>
              <th>Localização</th>
              <th>Data Operação</th>
              <th>Ativo</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {usinas.map(u => (
              <tr key={u.id}>
                <td>{u.codigo}</td>
                <td>{u.nome}</td>
                <td>{u.tipoUsinaId}</td>
                <td>{u.empresaId}</td>
                <td>{u.capacidadeInstalada}</td>
                <td>{u.localizacao}</td>
                <td>{u.dataOperacao ? new Date(u.dataOperacao).toLocaleDateString() : '-'}</td>
                <td>{u.ativo ? 'Sim' : 'Não'}</td>
                <td>
                  <button className={styles.btnSec} onClick={() => handleEdit(u)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleDelete(u.id)}>Remover</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {(!loading && usinas.length === 0) && <div className={styles.noData}>Nenhuma usina cadastrada</div>}
    </div>
  );
};

export default PlantManagement;

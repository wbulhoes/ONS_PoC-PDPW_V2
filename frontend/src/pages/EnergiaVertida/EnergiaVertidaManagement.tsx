import React, { useState, useEffect } from 'react';
import styles from './EnergiaVertidaManagement.module.css';
import { energiaVertidaService, EnergiaVertida, CreateEnergiaVertidaDto, UpdateEnergiaVertidaDto } from '../../services/energiaVertidaService';

const initialForm: CreateEnergiaVertidaDto = {
  dataReferencia: '',
  codigoUsina: '',
  energiaVertida: 0,
  motivoVertimento: '',
  observacoes: '',
};

const EnergiaVertidaManagement: React.FC = () => {
  const [registros, setRegistros] = useState<EnergiaVertida[]>([]);
  const [form, setForm] = useState<CreateEnergiaVertidaDto>(initialForm);
  const [editId, setEditId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadRegistros();
  }, []);

  const loadRegistros = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await energiaVertidaService.getAll();
      setRegistros(data);
    } catch (err) {
      setError('Erro ao carregar registros');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: name === 'energiaVertida' ? Number(value) : value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      if (editId) {
        await energiaVertidaService.update(editId, form as UpdateEnergiaVertidaDto);
        setSuccess('Registro atualizado com sucesso!');
      } else {
        await energiaVertidaService.create(form);
        setSuccess('Registro cadastrado com sucesso!');
      }
      setForm(initialForm);
      setEditId(null);
      await loadRegistros();
    } catch (err: any) {
      setError('Erro ao salvar registro');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (registro: EnergiaVertida) => {
    setForm({
      dataReferencia: registro.dataReferencia.split('T')[0],
      codigoUsina: registro.codigoUsina,
      energiaVertida: registro.energiaVertida,
      motivoVertimento: registro.motivoVertimento || '',
      observacoes: registro.observacoes || '',
    });
    setEditId(registro.id);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja remover este registro?')) return;
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      await energiaVertidaService.delete(id);
      setSuccess('Registro removido com sucesso!');
      await loadRegistros();
    } catch {
      setError('Erro ao remover registro');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Energia Vertida</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label htmlFor="dataReferencia">Data de Referência:</label>
          <input id="dataReferencia" name="dataReferencia" type="date" value={form.dataReferencia} onChange={handleChange} required />
        </div>
        <div>
          <label htmlFor="codigoUsina">Código da Usina:</label>
          <input id="codigoUsina" name="codigoUsina" value={form.codigoUsina} onChange={handleChange} required maxLength={50} />
        </div>
        <div>
          <label htmlFor="energiaVertida">Energia Vertida (MWh):</label>
          <input id="energiaVertida" name="energiaVertida" type="number" value={form.energiaVertida} onChange={handleChange} min={0} required />
        </div>
        <div>
          <label htmlFor="motivoVertimento">Motivo Vertimento:</label>
          <input id="motivoVertimento" name="motivoVertimento" value={form.motivoVertimento} onChange={handleChange} maxLength={500} />
        </div>
        <div>
          <label htmlFor="observacoes">Observações:</label>
          <textarea id="observacoes" name="observacoes" value={form.observacoes} onChange={handleChange} rows={2} />
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editId ? 'Atualizar' : 'Salvar'}</button>
        {editId && <button type="button" className={styles.btnSec} onClick={() => { setForm(initialForm); setEditId(null); }}>Cancelar</button>}
      </form>

      <h2>Registros de Energia Vertida</h2>
      {error && <div className={styles.error}>{error}</div>}
      {success && <div className={styles.success}>{success}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Data</th>
              <th>Usina</th>
              <th>Energia Vertida</th>
              <th>Motivo</th>
              <th>Observações</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {registros.map(r => (
              <tr key={r.id}>
                <td>{r.dataReferencia.split('T')[0]}</td>
                <td>{r.codigoUsina}</td>
                <td>{r.energiaVertida}</td>
                <td>{r.motivoVertimento}</td>
                <td>{r.observacoes}</td>
                <td>
                  <button className={styles.btnSec} onClick={() => handleEdit(r)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleDelete(r.id)}>Remover</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {(!loading && registros.length === 0) && <div className={styles.noData}>Nenhum registro cadastrado</div>}
    </div>
  );
};

export default EnergiaVertidaManagement;

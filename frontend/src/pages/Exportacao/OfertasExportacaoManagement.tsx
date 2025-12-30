import React, { useState, useEffect } from 'react';
import styles from './OfertasExportacaoManagement.module.css';
import { ofertaExportacaoService, OfertaExportacao, CreateOfertaExportacaoDto, UpdateOfertaExportacaoDto } from '../../services/ofertaExportacaoService';

const initialForm: CreateOfertaExportacaoDto = {
  usinaId: 0,
  dataOferta: '',
  dataPDP: '',
  valorMW: 0,
  precoMWh: 0,
  horaInicial: '00:00:00',
  horaFinal: '23:59:59',
  observacoes: '',
  semanaPMOId: undefined,
};

const OfertasExportacaoManagement: React.FC = () => {
  const [ofertas, setOfertas] = useState<OfertaExportacao[]>([]);
  const [form, setForm] = useState<CreateOfertaExportacaoDto>(initialForm);
  const [editId, setEditId] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<string | null>(null);

  useEffect(() => {
    loadOfertas();
  }, []);

  const loadOfertas = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await ofertaExportacaoService.getAll();
      setOfertas(data);
    } catch (err) {
      setError('Erro ao carregar ofertas');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
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
        await ofertaExportacaoService.update(editId, form as UpdateOfertaExportacaoDto);
        setSuccess('Oferta atualizada com sucesso!');
      } else {
        await ofertaExportacaoService.create(form);
        setSuccess('Oferta cadastrada com sucesso!');
      }
      setForm(initialForm);
      setEditId(null);
      await loadOfertas();
    } catch (err: any) {
      setError('Erro ao salvar oferta');
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (oferta: OfertaExportacao) => {
    setForm({
      usinaId: oferta.usinaId,
      dataOferta: oferta.dataOferta.split('T')[0],
      dataPDP: oferta.dataPDP.split('T')[0],
      valorMW: oferta.valorMW,
      precoMWh: oferta.precoMWh,
      horaInicial: oferta.horaInicial,
      horaFinal: oferta.horaFinal,
      observacoes: oferta.observacoes || '',
      semanaPMOId: oferta.semanaPMOId,
    });
    setEditId(oferta.id);
  };

  const handleDelete = async (id: number) => {
    if (!window.confirm('Deseja remover esta oferta?')) return;
    setLoading(true);
    setError(null);
    setSuccess(null);
    try {
      await ofertaExportacaoService.delete(id);
      setSuccess('Oferta removida com sucesso!');
      await loadOfertas();
    } catch {
      setError('Erro ao remover oferta');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Ofertas de Exportação</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label htmlFor="usinaId">Usina ID:</label>
          <input id="usinaId" name="usinaId" type="number" value={form.usinaId} onChange={handleChange} min={0} required />
        </div>
        <div>
          <label htmlFor="dataOferta">Data da Oferta:</label>
          <input id="dataOferta" name="dataOferta" type="date" value={form.dataOferta} onChange={handleChange} required />
        </div>
        <div>
          <label htmlFor="dataPDP">Data PDP:</label>
          <input id="dataPDP" name="dataPDP" type="date" value={form.dataPDP} onChange={handleChange} required />
        </div>
        <div>
          <label htmlFor="valorMW">Valor MW:</label>
          <input id="valorMW" name="valorMW" type="number" value={form.valorMW} onChange={handleChange} min={0} required />
        </div>
        <div>
          <label htmlFor="precoMWh">Preço MWh:</label>
          <input id="precoMWh" name="precoMWh" type="number" value={form.precoMWh} onChange={handleChange} min={0} required />
        </div>
        <div>
          <label htmlFor="horaInicial">Hora Inicial:</label>
          <input id="horaInicial" name="horaInicial" type="time" value={form.horaInicial} onChange={handleChange} required />
        </div>
        <div>
          <label htmlFor="horaFinal">Hora Final:</label>
          <input id="horaFinal" name="horaFinal" type="time" value={form.horaFinal} onChange={handleChange} required />
        </div>
        <div>
          <label htmlFor="observacoes">Observações:</label>
          <textarea id="observacoes" name="observacoes" value={form.observacoes} onChange={handleChange} rows={2} />
        </div>
        <button type="submit" className={styles.btnPrimary} disabled={loading}>{editId ? 'Atualizar' : 'Salvar'}</button>
        {editId && <button type="button" className={styles.btnSec} onClick={() => { setForm(initialForm); setEditId(null); }}>Cancelar</button>}
      </form>

      <h2>Ofertas Cadastradas</h2>
      {error && <div className={styles.error}>{error}</div>}
      {success && <div className={styles.success}>{success}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Usina</th>
              <th>Data Oferta</th>
              <th>Data PDP</th>
              <th>Valor MW</th>
              <th>Preço MWh</th>
              <th>Hora Inicial</th>
              <th>Hora Final</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {ofertas.map(o => (
              <tr key={o.id}>
                <td>{o.usinaNome}</td>
                <td>{o.dataOferta.split('T')[0]}</td>
                <td>{o.dataPDP.split('T')[0]}</td>
                <td>{o.valorMW}</td>
                <td>{o.precoMWh}</td>
                <td>{o.horaInicial}</td>
                <td>{o.horaFinal}</td>
                <td>{o.statusAnalise}</td>
                <td>
                  <button className={styles.btnSec} onClick={() => handleEdit(o)}>Editar</button>
                  <button className={styles.btnDanger} onClick={() => handleDelete(o.id)}>Remover</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
      {(!loading && ofertas.length === 0) && <div className={styles.noData}>Nenhuma oferta cadastrada</div>}
    </div>
  );
};

export default OfertasExportacaoManagement;

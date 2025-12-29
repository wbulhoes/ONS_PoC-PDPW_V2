import React, { useState, useEffect } from 'react';
import { dadosEnergeticosService } from '../services';
import { DadoEnergeticoDto, CriarDadoEnergeticoDto } from '../types';
import styles from './DadosHidraulicos.module.css';

const DadosHidraulicos: React.FC = () => {
  const [dados, setDados] = useState<DadoEnergeticoDto[]>([]);
  const [form, setForm] = useState<CriarDadoEnergeticoDto>({
    dataReferencia: '',
    codigoUsina: '',
    producaoMWh: 0,
    capacidadeDisponivel: 0,
    status: 'Ativo',
    observacoes: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    setLoading(true);
    setError(null);
    try {
      const resultado = await dadosEnergeticosService.obterTodos();
      setDados(resultado.filter(d => d.status === 'Hidraulico'));
    } catch (err) {
      setError('Erro ao carregar dados hidráulicos');
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: name === 'producaoMWh' || name === 'capacidadeDisponivel' ? parseFloat(value) || 0 : value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      await dadosEnergeticosService.criar({ ...form, status: 'Hidraulico' });
      setForm({ dataReferencia: '', codigoUsina: '', producaoMWh: 0, capacidadeDisponivel: 0, status: 'Hidraulico', observacoes: '' });
      await carregarDados();
    } catch (err) {
      setError('Erro ao salvar dado hidráulico');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>Dados Hidráulicos - Coleta</h2>
      <form className={styles.form} onSubmit={handleSubmit}>
        <div>
          <label>Data de Referência:</label>
          <input type="date" name="dataReferencia" value={form.dataReferencia} onChange={handleChange} required />
        </div>
        <div>
          <label>Código da Usina:</label>
          <input type="text" name="codigoUsina" value={form.codigoUsina} onChange={handleChange} required />
        </div>
        <div>
          <label>Vazão (MWh):</label>
          <input type="number" name="producaoMWh" value={form.producaoMWh} onChange={handleChange} required min={0} />
        </div>
        <div>
          <label>Disponibilidade (MW):</label>
          <input type="number" name="capacidadeDisponivel" value={form.capacidadeDisponivel} onChange={handleChange} required min={0} />
        </div>
        <div>
          <label>Observações:</label>
          <textarea name="observacoes" value={form.observacoes} onChange={handleChange} rows={2} />
        </div>
        <button type="submit" className={styles.btnPrimary}>Salvar</button>
      </form>

      <h2>Consulta de Dados Hidráulicos</h2>
      {error && <div className={styles.error}>{error}</div>}
      {loading ? <div>Carregando...</div> : (
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Data</th>
              <th>Código Usina</th>
              <th>Vazão (MWh)</th>
              <th>Disponibilidade (MW)</th>
              <th>Observações</th>
            </tr>
          </thead>
          <tbody>
            {dados.map((d) => (
              <tr key={d.id}>
                <td>{new Date(d.dataReferencia).toLocaleDateString('pt-BR')}</td>
                <td>{d.codigoUsina}</td>
                <td>{d.producaoMWh}</td>
                <td>{d.capacidadeDisponivel}</td>
                <td>{d.observacoes}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
}

export default DadosHidraulicos;

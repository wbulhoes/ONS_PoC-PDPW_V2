import React, { useState, useEffect } from 'react';
import { dadosEnergeticosService } from '../services';
import { DadoEnergeticoDto } from '../types';
import styles from './DadosEnergeticos.module.css';

const DadosEnergeticos: React.FC = () => {
  const [dados, setDados] = useState<DadoEnergeticoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState({
    dataReferencia: '',
    codigoUsina: '',
    producaoMWh: 0,
    capacidadeDisponivel: 0,
    status: 'PLANEJADO',
    observacoes: '',
  });
  const [editandoId, setEditandoId] = useState<number | null>(null);

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const resultado = await dadosEnergeticosService.obterTodos();
      setDados(resultado);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar dados energéticos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editandoId) {
        await dadosEnergeticosService.atualizar(editandoId, formData);
      } else {
        await dadosEnergeticosService.criar(formData);
      }
      resetForm();
      carregarDados();
    } catch (err) {
      setError('Erro ao salvar dados');
      console.error(err);
    }
  };

  const handleEditar = (dado: DadoEnergeticoDto) => {
    setFormData({
      dataReferencia: dado.dataReferencia,
      codigoUsina: dado.codigoUsina,
      producaoMWh: dado.producaoMWh,
      capacidadeDisponivel: dado.capacidadeDisponivel,
      status: dado.status,
      observacoes: dado.observacoes || '',
    });
    setEditandoId(dado.id);
  };

  const handleRemover = async (id: number) => {
    if (window.confirm('Deseja realmente remover este registro?')) {
      try {
        await dadosEnergeticosService.remover(id);
        carregarDados();
      } catch (err) {
        setError('Erro ao remover dados');
        console.error(err);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      dataReferencia: '',
      codigoUsina: '',
      producaoMWh: 0,
      capacidadeDisponivel: 0,
      status: 'PLANEJADO',
      observacoes: '',
    });
    setEditandoId(null);
  };

  if (loading) return <div className={styles.loading}>Carregando...</div>;

  return (
    <div className={styles.container}>
      <h2>1. Dados Energéticos</h2>

      {error && <div className={styles.error}>{error}</div>}

      <form onSubmit={handleSubmit} className={styles.form}>
        <div className={styles.formGroup}>
          <label>Data Referência:</label>
          <input
            type="date"
            value={formData.dataReferencia}
            onChange={(e) => setFormData({ ...formData, dataReferencia: e.target.value })}
            required
          />
        </div>

        <div className={styles.formGroup}>
          <label>Código Usina:</label>
          <input
            type="text"
            value={formData.codigoUsina}
            onChange={(e) => setFormData({ ...formData, codigoUsina: e.target.value })}
            required
          />
        </div>

        <div className={styles.formGroup}>
          <label>Produção (MWh):</label>
          <input
            type="number"
            value={formData.producaoMWh}
            onChange={(e) => setFormData({ ...formData, producaoMWh: parseFloat(e.target.value) })}
            required
          />
        </div>

        <div className={styles.formGroup}>
          <label>Capacidade Disponível:</label>
          <input
            type="number"
            value={formData.capacidadeDisponivel}
            onChange={(e) => setFormData({ ...formData, capacidadeDisponivel: parseFloat(e.target.value) })}
            required
          />
        </div>

        <div className={styles.formGroup}>
          <label>Status:</label>
          <select value={formData.status} onChange={(e) => setFormData({ ...formData, status: e.target.value })}>
            <option value="PLANEJADO">Planejado</option>
            <option value="CONFIRMADO">Confirmado</option>
            <option value="REALIZADO">Realizado</option>
          </select>
        </div>

        <div className={styles.formGroup}>
          <label>Observações:</label>
          <textarea
            value={formData.observacoes}
            onChange={(e) => setFormData({ ...formData, observacoes: e.target.value })}
          />
        </div>

        <div className={styles.formActions}>
          <button type="submit" className={styles.btnSalvar}>
            {editandoId ? 'Atualizar' : 'Salvar'}
          </button>
          {editandoId && (
            <button type="button" onClick={resetForm} className={styles.btnCancelar}>
              Cancelar
            </button>
          )}
        </div>
      </form>

      <div className={styles.tableContainer}>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Data</th>
              <th>Usina</th>
              <th>Produção (MWh)</th>
              <th>Capacidade</th>
              <th>Status</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {dados.map((dado) => (
              <tr key={dado.id}>
                <td>{new Date(dado.dataReferencia).toLocaleDateString()}</td>
                <td>{dado.codigoUsina}</td>
                <td>{dado.producaoMWh.toLocaleString()}</td>
                <td>{dado.capacidadeDisponivel.toLocaleString()}</td>
                <td>
                  <span className={`${styles.badge} ${styles[`badge${dado.status}`]}`}>{dado.status}</span>
                </td>
                <td>
                  <button onClick={() => handleEditar(dado)} className={styles.btnEditar}>
                    Editar
                  </button>
                  <button onClick={() => handleRemover(dado.id)} className={styles.btnRemover}>
                    Remover
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default DadosEnergeticos;

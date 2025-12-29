import React, { useState, useEffect } from 'react';
import { previsoesEolicasService, usinasService, semanasPmoService } from '../services';
import { PrevisaoEolicaDto, UsinaDto, SemanaPmoDto } from '../types';
import styles from './PrevisaoEolica.module.css';

const PrevisaoEolica: React.FC = () => {
  const [previsoes, setPrevisoes] = useState<PrevisaoEolicaDto[]>([]);
  const [usinas, setUsinas] = useState<UsinaDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [editandoId, setEditandoId] = useState<number | null>(null);

  const [formData, setFormData] = useState({
    parqueEolicoId: 0,
    semanaPmoId: 0,
    dataPrevisao: '',
    potenciaPrevistoMW: 0,
    fatorCapacidade: 0,
    velocidadeVentoMS: 0,
    observacoes: '',
  });

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const [previsoesData, usinasData, semanasData] = await Promise.all([
        previsoesEolicasService.obterTodas(),
        usinasService.obterPorTipo(3), // Tipo 3 = Eólica
        semanasPmoService.obterProximas(8),
      ]);

      setPrevisoes(previsoesData);
      setUsinas(usinasData);
      setSemanas(semanasData);
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editandoId) {
        await previsoesEolicasService.atualizar(editandoId, formData);
      } else {
        await previsoesEolicasService.criar(formData);
      }
      resetForm();
      carregarDados();
    } catch (err) {
      console.error('Erro ao salvar previsão:', err);
    }
  };

  const handleAtualizarPrevisao = async (id: number, novaPotencia: number) => {
    try {
      await previsoesEolicasService.atualizarPrevisao(id, novaPotencia);
      carregarDados();
    } catch (err) {
      console.error('Erro ao atualizar previsão:', err);
    }
  };

  const handleEditar = (previsao: PrevisaoEolicaDto) => {
    setFormData({
      parqueEolicoId: previsao.parqueEolicoId,
      semanaPmoId: previsao.semanaPmoId,
      dataPrevisao: previsao.dataPrevisao,
      potenciaPrevistoMW: previsao.potenciaPrevistoMW,
      fatorCapacidade: previsao.fatorCapacidade,
      velocidadeVentoMS: previsao.velocidadeVentoMS || 0,
      observacoes: previsao.observacoes || '',
    });
    setEditandoId(previsao.id);
  };

  const handleRemover = async (id: number) => {
    if (window.confirm('Deseja realmente remover esta previsão?')) {
      try {
        await previsoesEolicasService.remover(id);
        carregarDados();
      } catch (err) {
        console.error('Erro ao remover previsão:', err);
      }
    }
  };

  const resetForm = () => {
    setFormData({
      parqueEolicoId: 0,
      semanaPmoId: 0,
      dataPrevisao: '',
      potenciaPrevistoMW: 0,
      fatorCapacidade: 0,
      velocidadeVentoMS: 0,
      observacoes: '',
    });
    setEditandoId(null);
  };

  const calcularFatorCapacidade = (potenciaPrevisto: number, parqueId: number) => {
    const usina = usinas.find((u) => u.id === parqueId);
    if (!usina || usina.capacidadeInstalada === 0) return 0;
    return (potenciaPrevisto / usina.capacidadeInstalada) * 100;
  };

  if (loading) return <div className={styles.loading}>Carregando...</div>;

  return (
    <div className={styles.container}>
      <h2>3. Previsão Eólica</h2>

      <form onSubmit={handleSubmit} className={styles.form}>
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Parque Eólico:</label>
            <select
              value={formData.parqueEolicoId}
              onChange={(e) => {
                const parqueId = Number(e.target.value);
                setFormData({
                  ...formData,
                  parqueEolicoId: parqueId,
                  fatorCapacidade: calcularFatorCapacidade(formData.potenciaPrevistoMW, parqueId),
                });
              }}
              required
            >
              <option value="">Selecione...</option>
              {usinas.map((u) => (
                <option key={u.id} value={u.id}>
                  {u.nome} ({u.capacidadeInstalada.toLocaleString()} MW)
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label>Semana PMO:</label>
            <select
              value={formData.semanaPmoId}
              onChange={(e) => setFormData({ ...formData, semanaPmoId: Number(e.target.value) })}
              required
            >
              <option value="">Selecione...</option>
              {semanas.map((s) => (
                <option key={s.id} value={s.id}>
                  Semana {s.numero}/{s.ano}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label>Data Previsão:</label>
            <input
              type="date"
              value={formData.dataPrevisao}
              onChange={(e) => setFormData({ ...formData, dataPrevisao: e.target.value })}
              required
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Potência Prevista (MW):</label>
            <input
              type="number"
              step="0.01"
              value={formData.potenciaPrevistoMW}
              onChange={(e) => {
                const potencia = parseFloat(e.target.value);
                setFormData({
                  ...formData,
                  potenciaPrevistoMW: potencia,
                  fatorCapacidade: calcularFatorCapacidade(potencia, formData.parqueEolicoId),
                });
              }}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label>Fator de Capacidade (%):</label>
            <input type="number" step="0.01" value={formData.fatorCapacidade.toFixed(2)} disabled />
          </div>

          <div className={styles.formGroup}>
            <label>Velocidade do Vento (m/s):</label>
            <input
              type="number"
              step="0.1"
              value={formData.velocidadeVentoMS}
              onChange={(e) => setFormData({ ...formData, velocidadeVentoMS: parseFloat(e.target.value) })}
            />
          </div>
        </div>

        <div className={styles.formGroup}>
          <label>Observações:</label>
          <textarea
            value={formData.observacoes}
            onChange={(e) => setFormData({ ...formData, observacoes: e.target.value })}
            rows={3}
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
        <h3>Previsões Cadastradas</h3>
        <table className={styles.table}>
          <thead>
            <tr>
              <th>Parque Eólico</th>
              <th>Semana PMO</th>
              <th>Data</th>
              <th>Potência (MW)</th>
              <th>Fator Cap. (%)</th>
              <th>Vento (m/s)</th>
              <th>Ações</th>
            </tr>
          </thead>
          <tbody>
            {previsoes.map((prev) => {
              const usina = usinas.find((u) => u.id === prev.parqueEolicoId);
              const semana = semanas.find((s) => s.id === prev.semanaPmoId);
              return (
                <tr key={prev.id}>
                  <td>{usina?.nome || 'N/A'}</td>
                  <td>
                    {semana ? `${semana.numero}/${semana.ano}` : 'N/A'}
                  </td>
                  <td>{new Date(prev.dataPrevisao).toLocaleDateString()}</td>
                  <td>{prev.potenciaPrevistoMW.toLocaleString()}</td>
                  <td>{prev.fatorCapacidade.toFixed(2)}%</td>
                  <td>{prev.velocidadeVentoMS?.toFixed(1) || '-'}</td>
                  <td>
                    <button onClick={() => handleEditar(prev)} className={styles.btnEditar}>
                      Editar
                    </button>
                    <button onClick={() => handleRemover(prev.id)} className={styles.btnRemover}>
                      Remover
                    </button>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default PrevisaoEolica;

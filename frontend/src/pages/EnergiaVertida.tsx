import React, { useState, useEffect } from 'react';
import { energiaVertidaService, usinasService, semanasPmoService } from '../services';
import { EnergiaVertidaDto, UsinaDto, SemanaPmoDto } from '../types';
import styles from './OfertasExportacao.module.css';

const EnergiaVertida: React.FC = () => {
  const [vertimentos, setVertimentos] = useState<EnergiaVertidaDto[]>([]);
  const [usinas, setUsinas] = useState<UsinaDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [editandoId, setEditandoId] = useState<number | null>(null);

  const [formData, setFormData] = useState({
    usinaId: 0,
    semanaPmoId: 0,
    dataReferencia: '',
    energiaVertidaMWh: 0,
    motivoVertimento: '',
    observacoes: '',
  });

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const [vertimentosData, usinasData, semanasData] = await Promise.all([
        energiaVertidaService.obterTodas(),
        usinasService.obterPorTipo(1), // Tipo 1 = Hidráulica
        semanasPmoService.obterProximas(8),
      ]);

      setVertimentos(vertimentosData);
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
        await energiaVertidaService.atualizar(editandoId, formData);
      } else {
        await energiaVertidaService.criar(formData);
      }
      resetForm();
      carregarDados();
    } catch (err) {
      console.error('Erro ao salvar vertimento:', err);
      alert('Erro ao salvar dados de energia vertida');
    }
  };

  const handleEditar = (vertimento: EnergiaVertidaDto) => {
    setFormData({
      usinaId: vertimento.usinaId,
      semanaPmoId: vertimento.semanaPmoId,
      dataReferencia: vertimento.dataReferencia,
      energiaVertidaMWh: vertimento.energiaVertidaMWh,
      motivoVertimento: vertimento.motivoVertimento,
      observacoes: vertimento.observacoes || '',
    });
    setEditandoId(vertimento.id);
  };

  const handleRemover = async (id: number) => {
    if (!window.confirm('Deseja realmente remover este registro?')) return;

    try {
      await energiaVertidaService.remover(id);
      carregarDados();
    } catch (err) {
      console.error('Erro ao remover vertimento:', err);
      alert('Erro ao remover registro');
    }
  };

  const resetForm = () => {
    setFormData({
      usinaId: 0,
      semanaPmoId: 0,
      dataReferencia: '',
      energiaVertidaMWh: 0,
      motivoVertimento: '',
      observacoes: '',
    });
    setEditandoId(null);
  };

  if (loading) return <div className={styles.loading}>Carregando...</div>;

  return (
    <div className={styles.container}>
      <h2>9. Dados de Energia Vertida Turbinável</h2>

      <div className={styles.infoBox}>
        <h3>ℹ️ Sobre Energia Vertida Turbinável</h3>
        <p>
          Energia vertida turbinável refere-se ao volume de água que passa pelas turbinas de usinas hidrelétricas sem
          ser aproveitado para geração de energia, devido a restrições operativas, excesso de afluência ou limitações
          do sistema.
        </p>
      </div>

      <form onSubmit={handleSubmit} className={styles.form}>
        <h3>{editandoId ? 'Editar Vertimento' : 'Registrar Vertimento'}</h3>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Usina Hidrelétrica:</label>
            <select
              value={formData.usinaId}
              onChange={(e) => setFormData({ ...formData, usinaId: Number(e.target.value) })}
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
            <label>Data de Referência:</label>
            <input
              type="date"
              value={formData.dataReferencia}
              onChange={(e) => setFormData({ ...formData, dataReferencia: e.target.value })}
              required
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Energia Vertida (MWh):</label>
            <input
              type="number"
              step="0.01"
              value={formData.energiaVertidaMWh}
              onChange={(e) => setFormData({ ...formData, energiaVertidaMWh: parseFloat(e.target.value) })}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label>Motivo do Vertimento:</label>
            <select
              value={formData.motivoVertimento}
              onChange={(e) => setFormData({ ...formData, motivoVertimento: e.target.value })}
              required
            >
              <option value="">Selecione...</option>
              <option value="EXCESSO_AFLUENCIA">Excesso de Afluência</option>
              <option value="RESTRICAO_OPERATIVA">Restrição Operativa</option>
              <option value="LIMITACAO_TRANSMISSAO">Limitação de Transmissão</option>
              <option value="MANUTENCAO">Manutenção Programada</option>
              <option value="CONTROLE_CHEIAS">Controle de Cheias</option>
              <option value="OUTROS">Outros</option>
            </select>
          </div>
        </div>

        <div className={styles.formGroup}>
          <label>Observações:</label>
          <textarea
            value={formData.observacoes}
            onChange={(e) => setFormData({ ...formData, observacoes: e.target.value })}
            rows={3}
            placeholder="Descreva detalhes adicionais sobre o vertimento..."
          />
        </div>

        <div className={styles.formActions}>
          <button type="submit" className={styles.btnSalvar}>
            {editandoId ? 'Atualizar' : 'Registrar'}
          </button>
          {editandoId && (
            <button type="button" onClick={resetForm} className={styles.btnCancelar}>
              Cancelar
            </button>
          )}
        </div>
      </form>

      <div className={styles.ofertasContainer}>
        <h3>Vertimentos Registrados ({vertimentos.length})</h3>

        {vertimentos.length === 0 ? (
          <p className={styles.noData}>Nenhum vertimento registrado.</p>
        ) : (
          <div className={styles.ofertasList}>
            {vertimentos.map((vertimento) => {
              const usina = usinas.find((u) => u.id === vertimento.usinaId);
              const semana = semanas.find((s) => s.id === vertimento.semanaPmoId);

              return (
                <div key={vertimento.id} className={styles.ofertaCard}>
                  <div className={styles.ofertaHeader}>
                    <div>
                      <h4>{usina?.nome || 'N/A'}</h4>
                      <p className={styles.ofertaSubtitulo}>
                        Semana {semana?.numero}/{semana?.ano} - {new Date(vertimento.dataReferencia).toLocaleDateString()}
                      </p>
                    </div>
                  </div>

                  <div className={styles.ofertaDetalhes}>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Energia Vertida:</span>
                      <span className={styles.detalheValor}>{vertimento.energiaVertidaMWh.toLocaleString()} MWh</span>
                    </div>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Motivo:</span>
                      <span className={styles.detalheValor}>{vertimento.motivoVertimento.replace('_', ' ')}</span>
                    </div>
                    {vertimento.observacoes && (
                      <div className={styles.detalheItem}>
                        <span className={styles.detalheLabel}>Observações:</span>
                        <span className={styles.detalheValor}>{vertimento.observacoes}</span>
                      </div>
                    )}
                  </div>

                  <div className={styles.ofertaAcoes}>
                    <button onClick={() => handleEditar(vertimento)} className={styles.btnEditar}>
                      ✏️ Editar
                    </button>
                    <button onClick={() => handleRemover(vertimento.id)} className={styles.btnRemover}>
                      🗑️ Remover
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>

      <div className={styles.infoBox} style={{ marginTop: '2rem' }}>
        <h3>📊 Motivos Comuns de Vertimento</h3>
        <ul style={{ paddingLeft: '1.5rem', color: '#374151' }}>
          <li><strong>Excesso de Afluência:</strong> Volume de água superior à capacidade de turbinamento</li>
          <li><strong>Restrição Operativa:</strong> Limitações técnicas ou operacionais da usina</li>
          <li><strong>Limitação de Transmissão:</strong> Restrições na capacidade de escoamento da energia</li>
          <li><strong>Manutenção:</strong> Paradas programadas para manutenção de equipamentos</li>
          <li><strong>Controle de Cheias:</strong> Vertimento preventivo para controle de níveis</li>
        </ul>
      </div>
    </div>
  );
};

export default EnergiaVertida;

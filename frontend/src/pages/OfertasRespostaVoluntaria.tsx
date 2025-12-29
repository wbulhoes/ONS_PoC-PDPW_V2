import React, { useState, useEffect } from 'react';
import { ofertasRespostaVoluntariaService, usuariosService, semanasPmoService } from '../services';
import { OfertaRespostaVoluntariaDto, SemanaPmoDto } from '../types';
import styles from './OfertasExportacao.module.css'; // Reutilizando estilos

const OfertasRespostaVoluntaria: React.FC = () => {
  const [ofertas, setOfertas] = useState<OfertaRespostaVoluntariaDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [filtroStatus, setFiltroStatus] = useState<string>('TODAS');
  const [editandoId, setEditandoId] = useState<number | null>(null);

  const [formData, setFormData] = useState({
    consumidorId: 0,
    semanaPmoId: 0,
    dataOferta: '',
    reducaoDemandaMW: 0,
    precoOfertaRS: 0,
    periodoInicio: '',
    periodoFim: '',
    observacoes: '',
  });

  useEffect(() => {
    carregarDados();
  }, []);

  useEffect(() => {
    filtrarOfertas();
  }, [filtroStatus]);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const [ofertasData, semanasData] = await Promise.all([
        ofertasRespostaVoluntariaService.obterTodas(),
        semanasPmoService.obterProximas(8),
      ]);

      setOfertas(ofertasData);
      setSemanas(semanasData);
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
    } finally {
      setLoading(false);
    }
  };

  const filtrarOfertas = async () => {
    try {
      setLoading(true);
      let resultado;

      switch (filtroStatus) {
        case 'PENDENTE':
          resultado = await ofertasRespostaVoluntariaService.obterPorStatus('PENDENTE');
          break;
        case 'APROVADO':
          resultado = await ofertasRespostaVoluntariaService.obterPorStatus('APROVADO');
          break;
        case 'REJEITADO':
          resultado = await ofertasRespostaVoluntariaService.obterPorStatus('REJEITADO');
          break;
        default:
          resultado = await ofertasRespostaVoluntariaService.obterTodas();
      }

      setOfertas(resultado);
    } catch (err) {
      console.error('Erro ao filtrar ofertas:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editandoId) {
        await ofertasRespostaVoluntariaService.atualizar(editandoId, formData);
      } else {
        await ofertasRespostaVoluntariaService.criar(formData);
      }
      resetForm();
      carregarDados();
    } catch (err) {
      console.error('Erro ao salvar oferta:', err);
      alert('Erro ao salvar oferta RV');
    }
  };

  const handleAprovar = async (id: number) => {
    if (!window.confirm('Deseja aprovar esta oferta de resposta voluntária?')) return;

    try {
      await ofertasRespostaVoluntariaService.aprovar(id);
      alert('Oferta aprovada com sucesso!');
      carregarDados();
    } catch (err) {
      console.error('Erro ao aprovar oferta:', err);
      alert('Erro ao aprovar oferta');
    }
  };

  const handleRejeitar = async (id: number) => {
    const motivo = window.prompt('Informe o motivo da rejeição:');
    if (!motivo) return;

    try {
      await ofertasRespostaVoluntariaService.rejeitar(id);
      alert('Oferta rejeitada com sucesso!');
      carregarDados();
    } catch (err) {
      console.error('Erro ao rejeitar oferta:', err);
      alert('Erro ao rejeitar oferta');
    }
  };

  const handleEditar = (oferta: OfertaRespostaVoluntariaDto) => {
    setFormData({
      consumidorId: oferta.consumidorId,
      semanaPmoId: oferta.semanaPmoId,
      dataOferta: oferta.dataOferta,
      reducaoDemandaMW: oferta.reducaoDemandaMW,
      precoOfertaRS: oferta.precoOfertaRS,
      periodoInicio: oferta.periodoInicio,
      periodoFim: oferta.periodoFim,
      observacoes: oferta.observacoes || '',
    });
    setEditandoId(oferta.id);
  };

  const handleRemover = async (id: number) => {
    if (!window.confirm('Deseja realmente remover esta oferta?')) return;

    try {
      await ofertasRespostaVoluntariaService.remover(id);
      carregarDados();
    } catch (err) {
      console.error('Erro ao remover oferta:', err);
      alert('Erro ao remover oferta');
    }
  };

  const resetForm = () => {
    setFormData({
      consumidorId: 0,
      semanaPmoId: 0,
      dataOferta: '',
      reducaoDemandaMW: 0,
      precoOfertaRS: 0,
      periodoInicio: '',
      periodoFim: '',
      observacoes: '',
    });
    setEditandoId(null);
  };

  const getStatusBadgeClass = (status: string) => {
    switch (status.toUpperCase()) {
      case 'APROVADO':
        return styles.badgeAprovado;
      case 'REJEITADO':
        return styles.badgeRejeitado;
      case 'PENDENTE':
        return styles.badgePendente;
      default:
        return '';
    }
  };

  if (loading) return <div className={styles.loading}>Carregando...</div>;

  return (
    <div className={styles.container}>
      <h2>8. Ofertas de Resposta Voluntária da Demanda</h2>

      <div className={styles.infoBox}>
        <h3>ℹ️ Sobre Ofertas de Resposta Voluntária (RV)</h3>
        <p>
          As ofertas de Resposta Voluntária são propostas de redução temporária de demanda apresentadas por consumidores,
          contribuindo para o equilíbrio do sistema elétrico em períodos críticos.
        </p>
      </div>

      <div className={styles.filtros}>
        <label>Filtrar por Status:</label>
        <div className={styles.filtroButtons}>
          <button
            className={filtroStatus === 'TODAS' ? styles.filtroActive : ''}
            onClick={() => setFiltroStatus('TODAS')}
          >
            Todas
          </button>
          <button
            className={filtroStatus === 'PENDENTE' ? styles.filtroActive : ''}
            onClick={() => setFiltroStatus('PENDENTE')}
          >
            Pendentes
          </button>
          <button
            className={filtroStatus === 'APROVADO' ? styles.filtroActive : ''}
            onClick={() => setFiltroStatus('APROVADO')}
          >
            Aprovadas
          </button>
          <button
            className={filtroStatus === 'REJEITADO' ? styles.filtroActive : ''}
            onClick={() => setFiltroStatus('REJEITADO')}
          >
            Rejeitadas
          </button>
        </div>
      </div>

      <form onSubmit={handleSubmit} className={styles.form}>
        <h3>{editandoId ? 'Editar Oferta RV' : 'Nova Oferta RV'}</h3>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>ID Consumidor:</label>
            <input
              type="number"
              value={formData.consumidorId}
              onChange={(e) => setFormData({ ...formData, consumidorId: Number(e.target.value) })}
              required
            />
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
            <label>Data da Oferta:</label>
            <input
              type="date"
              value={formData.dataOferta}
              onChange={(e) => setFormData({ ...formData, dataOferta: e.target.value })}
              required
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Redução de Demanda (MW):</label>
            <input
              type="number"
              step="0.01"
              value={formData.reducaoDemandaMW}
              onChange={(e) => setFormData({ ...formData, reducaoDemandaMW: parseFloat(e.target.value) })}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label>Preço (R$/MWh):</label>
            <input
              type="number"
              step="0.01"
              value={formData.precoOfertaRS}
              onChange={(e) => setFormData({ ...formData, precoOfertaRS: parseFloat(e.target.value) })}
              required
            />
          </div>
        </div>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Período Início:</label>
            <input
              type="datetime-local"
              value={formData.periodoInicio}
              onChange={(e) => setFormData({ ...formData, periodoInicio: e.target.value })}
              required
            />
          </div>

          <div className={styles.formGroup}>
            <label>Período Fim:</label>
            <input
              type="datetime-local"
              value={formData.periodoFim}
              onChange={(e) => setFormData({ ...formData, periodoFim: e.target.value })}
              required
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
            {editandoId ? 'Atualizar' : 'Cadastrar'}
          </button>
          {editandoId && (
            <button type="button" onClick={resetForm} className={styles.btnCancelar}>
              Cancelar
            </button>
          )}
        </div>
      </form>

      <div className={styles.ofertasContainer}>
        <h3>Ofertas RV Cadastradas ({ofertas.length})</h3>

        {ofertas.length === 0 ? (
          <p className={styles.noData}>Nenhuma oferta RV encontrada.</p>
        ) : (
          <div className={styles.ofertasList}>
            {ofertas.map((oferta) => {
              const semana = semanas.find((s) => s.id === oferta.semanaPmoId);

              return (
                <div key={oferta.id} className={styles.ofertaCard}>
                  <div className={styles.ofertaHeader}>
                    <div>
                      <h4>Consumidor ID: {oferta.consumidorId}</h4>
                      <p className={styles.ofertaSubtitulo}>
                        Semana {semana?.numero}/{semana?.ano}
                      </p>
                    </div>
                    <span className={`${styles.badge} ${getStatusBadgeClass(oferta.status)}`}>{oferta.status}</span>
                  </div>

                  <div className={styles.ofertaDetalhes}>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Redução:</span>
                      <span className={styles.detalheValor}>{oferta.reducaoDemandaMW.toLocaleString()} MW</span>
                    </div>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Preço:</span>
                      <span className={styles.detalheValor}>R$ {oferta.precoOfertaRS.toLocaleString()}/MWh</span>
                    </div>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Período:</span>
                      <span className={styles.detalheValor}>
                        {new Date(oferta.periodoInicio).toLocaleString()} até{' '}
                        {new Date(oferta.periodoFim).toLocaleString()}
                      </span>
                    </div>
                    {oferta.observacoes && (
                      <div className={styles.detalheItem}>
                        <span className={styles.detalheLabel}>Observações:</span>
                        <span className={styles.detalheValor}>{oferta.observacoes}</span>
                      </div>
                    )}
                  </div>

                  <div className={styles.ofertaAcoes}>
                    {oferta.status === 'PENDENTE' && (
                      <>
                        <button onClick={() => handleAprovar(oferta.id)} className={styles.btnAprovar}>
                          ✅ Aprovar
                        </button>
                        <button onClick={() => handleRejeitar(oferta.id)} className={styles.btnRejeitar}>
                          ❌ Rejeitar
                        </button>
                        <button onClick={() => handleEditar(oferta)} className={styles.btnEditar}>
                          ✏️ Editar
                        </button>
                      </>
                    )}
                    <button onClick={() => handleRemover(oferta.id)} className={styles.btnRemover}>
                      🗑️ Remover
                    </button>
                  </div>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default OfertasRespostaVoluntaria;

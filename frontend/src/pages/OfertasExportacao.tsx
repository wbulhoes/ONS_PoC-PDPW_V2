import React, { useState, useEffect } from 'react';
import { ofertasExportacaoService, usinasService, semanasPmoService } from '../services';
import { OfertaExportacaoDto, UsinaDto, SemanaPmoDto } from '../types';
import styles from './OfertasExportacao.module.css';

const OfertasExportacao: React.FC = () => {
  const [ofertas, setOfertas] = useState<OfertaExportacaoDto[]>([]);
  const [usinas, setUsinas] = useState<UsinaDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [filtroStatus, setFiltroStatus] = useState<string>('TODAS');
  const [editandoId, setEditandoId] = useState<number | null>(null);

  const [formData, setFormData] = useState({
    usinaId: 0,
    semanaPmoId: 0,
    dataOferta: '',
    potenciaOfertadaMW: 0,
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
      const [ofertasData, usinasData, semanasData] = await Promise.all([
        ofertasExportacaoService.obterTodas(),
        usinasService.obterPorTipo(2), // Tipo 2 = Térmica
        semanasPmoService.obterProximas(8),
      ]);

      setOfertas(ofertasData);
      setUsinas(usinasData);
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
          resultado = await ofertasExportacaoService.obterPorStatus('PENDENTE');
          break;
        case 'APROVADO':
          resultado = await ofertasExportacaoService.obterPorStatus('APROVADO');
          break;
        case 'REJEITADO':
          resultado = await ofertasExportacaoService.obterPorStatus('REJEITADO');
          break;
        default:
          resultado = await ofertasExportacaoService.obterTodas();
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
        await ofertasExportacaoService.atualizar(editandoId, formData);
      } else {
        await ofertasExportacaoService.criar(formData);
      }
      resetForm();
      carregarDados();
    } catch (err) {
      console.error('Erro ao salvar oferta:', err);
      alert('Erro ao salvar oferta');
    }
  };

  const handleAprovar = async (id: number) => {
    if (!window.confirm('Deseja aprovar esta oferta de exportação?')) return;

    try {
      await ofertasExportacaoService.aprovar(id);
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
      await ofertasExportacaoService.rejeitar(id);
      alert('Oferta rejeitada com sucesso!');
      carregarDados();
    } catch (err) {
      console.error('Erro ao rejeitar oferta:', err);
      alert('Erro ao rejeitar oferta');
    }
  };

  const handleEditar = (oferta: OfertaExportacaoDto) => {
    setFormData({
      usinaId: oferta.usinaId,
      semanaPmoId: oferta.semanaPmoId,
      dataOferta: oferta.dataOferta,
      potenciaOfertadaMW: oferta.potenciaOfertadaMW,
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
      await ofertasExportacaoService.remover(id);
      carregarDados();
    } catch (err) {
      console.error('Erro ao remover oferta:', err);
      alert('Erro ao remover oferta');
    }
  };

  const resetForm = () => {
    setFormData({
      usinaId: 0,
      semanaPmoId: 0,
      dataOferta: '',
      potenciaOfertadaMW: 0,
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
      <h2>7. Ofertas de Exportação de Térmicas</h2>

      <div className={styles.infoBox}>
        <h3>ℹ️ Sobre Ofertas de Exportação</h3>
        <p>
          As ofertas de exportação são propostas de geração térmica para atendimento à demanda de energia elétrica,
          submetidas pelos agentes e analisadas pelo ONS.
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
        <h3>{editandoId ? 'Editar Oferta' : 'Nova Oferta'}</h3>

        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label>Usina Térmica:</label>
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
            <label>Potência Ofertada (MW):</label>
            <input
              type="number"
              step="0.01"
              value={formData.potenciaOfertadaMW}
              onChange={(e) => setFormData({ ...formData, potenciaOfertadaMW: parseFloat(e.target.value) })}
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
        <h3>Ofertas Cadastradas ({ofertas.length})</h3>

        {ofertas.length === 0 ? (
          <p className={styles.noData}>Nenhuma oferta encontrada.</p>
        ) : (
          <div className={styles.ofertasList}>
            {ofertas.map((oferta) => {
              const usina = usinas.find((u) => u.id === oferta.usinaId);
              const semana = semanas.find((s) => s.id === oferta.semanaPmoId);

              return (
                <div key={oferta.id} className={styles.ofertaCard}>
                  <div className={styles.ofertaHeader}>
                    <div>
                      <h4>{usina?.nome || 'N/A'}</h4>
                      <p className={styles.ofertaSubtitulo}>
                        Semana {semana?.numero}/{semana?.ano}
                      </p>
                    </div>
                    <span className={`${styles.badge} ${getStatusBadgeClass(oferta.status)}`}>{oferta.status}</span>
                  </div>

                  <div className={styles.ofertaDetalhes}>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Potência:</span>
                      <span className={styles.detalheValor}>{oferta.potenciaOfertadaMW.toLocaleString()} MW</span>
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

export default OfertasExportacao;

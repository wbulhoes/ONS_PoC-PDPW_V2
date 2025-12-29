import React, { useState, useEffect } from 'react';
import { arquivosDadgerService, semanasPmoService } from '../services';
import { ArquivoDadgerDto, SemanaPmoDto } from '../types';
import styles from './FinalizacaoProgramacao.module.css';

const FinalizacaoProgramacao: React.FC = () => {
  const [arquivos, setArquivos] = useState<ArquivoDadgerDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [semanaSelecionada, setSemanaSelecionada] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [processando, setProcessando] = useState(false);

  useEffect(() => {
    carregarSemanas();
  }, []);

  useEffect(() => {
    if (semanaSelecionada) {
      carregarArquivos();
    }
  }, [semanaSelecionada]);

  const carregarSemanas = async () => {
    try {
      const resultado = await semanasPmoService.obterProximas(8);
      setSemanas(resultado);
      if (resultado.length > 0) {
        setSemanaSelecionada(resultado[0].id);
      }
    } catch (err) {
      console.error('Erro ao carregar semanas:', err);
    }
  };

  const carregarArquivos = async () => {
    if (!semanaSelecionada) return;

    try {
      setLoading(true);
      const resultado = await arquivosDadgerService.obterPorSemana(semanaSelecionada);
      setArquivos(resultado);
    } catch (err) {
      console.error('Erro ao carregar arquivos:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleFinalizarProgramacao = async (arquivoId: number) => {
    if (!window.confirm('Deseja finalizar a programação? Esta ação irá publicar o arquivo DADGER aprovado.')) {
      return;
    }

    try {
      setProcessando(true);
      await arquivosDadgerService.aprovar(arquivoId);
      alert('Programação finalizada e publicada com sucesso!');
      carregarArquivos();
    } catch (err) {
      console.error('Erro ao finalizar programação:', err);
      alert('Erro ao finalizar programação');
    } finally {
      setProcessando(false);
    }
  };

  const getStatusClass = (status: string) => {
    switch (status.toUpperCase()) {
      case 'APROVADO':
        return styles.statusAprovado;
      case 'REJEITADO':
        return styles.statusRejeitado;
      case 'GERADO':
        return styles.statusGerado;
      case 'PUBLICADO':
        return styles.statusPublicado;
      default:
        return '';
    }
  };

  const semanaAtual = semanas.find((s) => s.id === semanaSelecionada);
  const arquivosAprovados = arquivos.filter((a) => a.status === 'APROVADO');
  const podePublicar = arquivosAprovados.length > 0;

  return (
    <div className={styles.container}>
      <h2>5. Finalização da Programação</h2>

      <div className={styles.infoBox}>
        <h3>ℹ️ Sobre a Finalização</h3>
        <p>
          A finalização da programação diária consiste em:
        </p>
        <ul>
          <li>Validação dos arquivos DADGER aprovados</li>
          <li>Verificação de consistência dos dados</li>
          <li>Publicação oficial para os modelos DESSEM/NEWAVE</li>
          <li>Notificação aos agentes do setor elétrico</li>
          <li>Geração de relatórios consolidados</li>
        </ul>
      </div>

      <div className={styles.selecaoSemana}>
        <label>Semana PMO:</label>
        <select value={semanaSelecionada || ''} onChange={(e) => setSemanaSelecionada(Number(e.target.value))}>
          {semanas.map((s) => (
            <option key={s.id} value={s.id}>
              Semana {s.numero}/{s.ano} ({new Date(s.dataInicio).toLocaleDateString()} -{' '}
              {new Date(s.dataFim).toLocaleDateString()})
            </option>
          ))}
        </select>
      </div>

      {semanaAtual && (
        <div className={styles.resumoSemana}>
          <h3>📅 Semana Selecionada</h3>
          <div className={styles.resumoGrid}>
            <div className={styles.resumoItem}>
              <span className={styles.resumoLabel}>Semana PMO:</span>
              <span className={styles.resumoValor}>
                {semanaAtual.numero}/{semanaAtual.ano}
              </span>
            </div>
            <div className={styles.resumoItem}>
              <span className={styles.resumoLabel}>Período:</span>
              <span className={styles.resumoValor}>
                {new Date(semanaAtual.dataInicio).toLocaleDateString()} a{' '}
                {new Date(semanaAtual.dataFim).toLocaleDateString()}
              </span>
            </div>
            <div className={styles.resumoItem}>
              <span className={styles.resumoLabel}>Status:</span>
              <span className={`${styles.resumoValor} ${styles[`status${semanaAtual.status}`]}`}>
                {semanaAtual.status}
              </span>
            </div>
            <div className={styles.resumoItem}>
              <span className={styles.resumoLabel}>Arquivos Aprovados:</span>
              <span className={styles.resumoValor}>{arquivosAprovados.length}</span>
            </div>
          </div>
        </div>
      )}

      {loading ? (
        <div className={styles.loading}>Carregando arquivos...</div>
      ) : (
        <div className={styles.arquivosContainer}>
          <h3>📁 Arquivos DADGER da Semana</h3>

          {arquivos.length === 0 ? (
            <p className={styles.noData}>Nenhum arquivo gerado para esta semana.</p>
          ) : (
            <div className={styles.arquivosList}>
              {arquivos.map((arquivo) => (
                <div key={arquivo.id} className={`${styles.arquivoCard} ${getStatusClass(arquivo.status)}`}>
                  <div className={styles.arquivoHeader}>
                    <div className={styles.arquivoInfo}>
                      <h4>{arquivo.nomeArquivo}</h4>
                      <p className={styles.arquivoPath}>{arquivo.caminhoArquivo}</p>
                    </div>
                    <div className={styles.arquivoStatus}>
                      <span className={`${styles.badge} ${getStatusClass(arquivo.status)}`}>{arquivo.status}</span>
                      <span className={styles.versao}>v{arquivo.versao}</span>
                    </div>
                  </div>

                  <div className={styles.arquivoDetalhes}>
                    <div className={styles.detalheItem}>
                      <span className={styles.detalheLabel}>Data de Criação:</span>
                      <span>{new Date(arquivo.dataCriacao).toLocaleString()}</span>
                    </div>
                    {arquivo.observacoes && (
                      <div className={styles.detalheItem}>
                        <span className={styles.detalheLabel}>Observações:</span>
                        <span>{arquivo.observacoes}</span>
                      </div>
                    )}
                  </div>

                  <div className={styles.arquivoAcoes}>
                    {arquivo.status === 'APROVADO' && (
                      <button
                        onClick={() => handleFinalizarProgramacao(arquivo.id)}
                        className={styles.btnFinalizar}
                        disabled={processando}
                      >
                        {processando ? 'Publicando...' : '✅ Publicar Programação'}
                      </button>
                    )}

                    {arquivo.status === 'PUBLICADO' && (
                      <div className={styles.publicadoInfo}>
                        <span className={styles.checkIcon}>✓</span>
                        Programação publicada e disponível para os modelos
                      </div>
                    )}

                    {arquivo.status === 'REJEITADO' && (
                      <div className={styles.rejeitadoInfo}>
                        <span className={styles.warningIcon}>⚠</span>
                        Arquivo rejeitado - gerar nova versão
                      </div>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {!podePublicar && arquivos.length > 0 && (
        <div className={styles.alertBox}>
          <h4>⚠️ Atenção</h4>
          <p>Não há arquivos DADGER aprovados para publicação nesta semana.</p>
          <p>É necessário gerar e aprovar um arquivo antes de finalizar a programação.</p>
        </div>
      )}

      <div className={styles.workflowBox}>
        <h3>🔄 Workflow de Finalização</h3>
        <div className={styles.workflowSteps}>
          <div className={styles.workflowStep}>
            <div className={styles.stepNumber}>1</div>
            <div className={styles.stepText}>
              <strong>Gerar DADGER</strong>
              <p>Criar arquivo com dados da semana</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.workflowStep}>
            <div className={styles.stepNumber}>2</div>
            <div className={styles.stepText}>
              <strong>Revisar</strong>
              <p>Validar dados do arquivo</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.workflowStep}>
            <div className={styles.stepNumber}>3</div>
            <div className={styles.stepText}>
              <strong>Aprovar</strong>
              <p>Aprovar arquivo DADGER</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.workflowStep}>
            <div className={styles.stepNumber}>4</div>
            <div className={styles.stepText}>
              <strong>Publicar</strong>
              <p>Finalizar programação</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default FinalizacaoProgramacao;

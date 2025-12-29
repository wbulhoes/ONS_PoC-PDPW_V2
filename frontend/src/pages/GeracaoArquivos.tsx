import React, { useState, useEffect } from 'react';
import { arquivosDadgerService, semanasPmoService } from '../services';
import { ArquivoDadgerDto, SemanaPmoDto } from '../types';
import styles from './GeracaoArquivos.module.css';

const GeracaoArquivos: React.FC = () => {
  const [arquivos, setArquivos] = useState<ArquivoDadgerDto[]>([]);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [semanaSelecionada, setSemanaSelecionada] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [gerando, setGerando] = useState(false);

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

  const handleGerarArquivo = async () => {
    if (!semanaSelecionada) return;

    if (!window.confirm('Deseja gerar novo arquivo DADGER para esta semana?')) {
      return;
    }

    try {
      setGerando(true);
      await arquivosDadgerService.gerar(semanaSelecionada);
      alert('Arquivo DADGER gerado com sucesso!');
      carregarArquivos();
    } catch (err) {
      console.error('Erro ao gerar arquivo:', err);
      alert('Erro ao gerar arquivo DADGER');
    } finally {
      setGerando(false);
    }
  };

  const handleAprovar = async (id: number) => {
    if (!window.confirm('Deseja aprovar este arquivo?')) {
      return;
    }

    try {
      await arquivosDadgerService.aprovar(id);
      alert('Arquivo aprovado com sucesso!');
      carregarArquivos();
    } catch (err) {
      console.error('Erro ao aprovar arquivo:', err);
      alert('Erro ao aprovar arquivo');
    }
  };

  const handleRejeitar = async (id: number) => {
    if (!window.confirm('Deseja rejeitar este arquivo?')) {
      return;
    }

    try {
      await arquivosDadgerService.rejeitar(id);
      alert('Arquivo rejeitado com sucesso!');
      carregarArquivos();
    } catch (err) {
      console.error('Erro ao rejeitar arquivo:', err);
      alert('Erro ao rejeitar arquivo');
    }
  };

  const handleDownload = async (id: number, nomeArquivo: string) => {
    try {
      const blob = await arquivosDadgerService.download(id);
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = nomeArquivo;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (err) {
      console.error('Erro ao baixar arquivo:', err);
      alert('Erro ao baixar arquivo');
    }
  };

  const getStatusBadgeClass = (status: string) => {
    switch (status.toUpperCase()) {
      case 'GERADO':
        return styles.badgeGerado;
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

  return (
    <div className={styles.container}>
      <h2>4. Geração de Arquivos DADGER</h2>

      <div className={styles.toolbar}>
        <div className={styles.semanaSelector}>
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

        <button onClick={handleGerarArquivo} className={styles.btnGerar} disabled={gerando || !semanaSelecionada}>
          {gerando ? 'Gerando...' : 'Gerar Novo Arquivo DADGER'}
        </button>
      </div>

      {loading ? (
        <div className={styles.loading}>Carregando...</div>
      ) : (
        <div className={styles.tableContainer}>
          <h3>Arquivos da Semana Selecionada</h3>
          {arquivos.length === 0 ? (
            <p className={styles.noData}>Nenhum arquivo gerado para esta semana.</p>
          ) : (
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Nome do Arquivo</th>
                  <th>Versão</th>
                  <th>Status</th>
                  <th>Data Criação</th>
                  <th>Observações</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody>
                {arquivos.map((arquivo) => {
                  const semana = semanas.find((s) => s.id === arquivo.semanaPmoId);
                  return (
                    <tr key={arquivo.id}>
                      <td>
                        <strong>{arquivo.nomeArquivo}</strong>
                        {semana && (
                          <div className={styles.semanaInfo}>
                            Semana {semana.numero}/{semana.ano}
                          </div>
                        )}
                      </td>
                      <td>
                        <span className={styles.versao}>v{arquivo.versao}</span>
                      </td>
                      <td>
                        <span className={`${styles.badge} ${getStatusBadgeClass(arquivo.status)}`}>
                          {arquivo.status}
                        </span>
                      </td>
                      <td>{new Date(arquivo.dataCriacao).toLocaleString()}</td>
                      <td>{arquivo.observacoes || '-'}</td>
                      <td>
                        <div className={styles.actionButtons}>
                          <button
                            onClick={() => handleDownload(arquivo.id, arquivo.nomeArquivo)}
                            className={styles.btnDownload}
                            title="Download"
                          >
                            ⬇️ Download
                          </button>

                          {arquivo.status === 'GERADO' && (
                            <>
                              <button
                                onClick={() => handleAprovar(arquivo.id)}
                                className={styles.btnAprovar}
                                title="Aprovar"
                              >
                                ✅ Aprovar
                              </button>
                              <button
                                onClick={() => handleRejeitar(arquivo.id)}
                                className={styles.btnRejeitar}
                                title="Rejeitar"
                              >
                                ❌ Rejeitar
                              </button>
                            </>
                          )}

                          {arquivo.status === 'APROVADO' && (
                            <span className={styles.aprovadoTag}>Arquivo aprovado e pronto para uso</span>
                          )}

                          {arquivo.status === 'REJEITADO' && (
                            <span className={styles.rejeitadoTag}>Arquivo rejeitado - gere nova versão</span>
                          )}
                        </div>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}
        </div>
      )}

      <div className={styles.infoBox}>
        <h4>ℹ️ Informações sobre Arquivos DADGER</h4>
        <ul>
          <li>
            <strong>DADGER:</strong> Dados Gerais para o modelo de otimização energética DESSEM
          </li>
          <li>Cada semana PMO pode ter múltiplas versões do arquivo</li>
          <li>Apenas arquivos com status "APROVADO" podem ser utilizados nos modelos</li>
          <li>Arquivos "REJEITADOS" não devem ser utilizados</li>
          <li>Ao gerar novo arquivo, a versão é incrementada automaticamente</li>
        </ul>
      </div>
    </div>
  );
};

export default GeracaoArquivos;

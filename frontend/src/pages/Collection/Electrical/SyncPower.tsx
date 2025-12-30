import React, { useState, useEffect } from 'react';
import styles from './SyncPower.module.css';
import {
  DadosPotenciaSincronizada,
  SyncPowerFormData,
  gerarIntervalos,
  validarValorPotencia,
} from '../../../types/syncPower';

interface SyncPowerProps {
  onLoadData: (formData: SyncPowerFormData) => Promise<DadosPotenciaSincronizada | null>;
  onSave: (data: DadosPotenciaSincronizada) => Promise<void>;
}

/**
 * Componente para Coleta de Potência Sincronizada
 * 
 * Migrado de: legado/pdpw/frmColPotSinc.aspx
 * 
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Visualização de dados em tabela com 24 intervalos (compactados dos 48 originais)
 * - Edição via textarea (formato line-separated)
 * - Área fixa: NE (Nordeste)
 */
const SyncPower: React.FC<SyncPowerProps> = ({ onLoadData, onSave }) => {
  const [formData, setFormData] = useState<SyncPowerFormData>({
    dataPdp: '',
  });

  const [data, setData] = useState<DadosPotenciaSincronizada | null>(null);
  const [textareaValue, setTextareaValue] = useState<string>('');
  const [showTextarea, setShowTextarea] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');

  // Dados mockados para demonstração
  const datasPdp = ['', '15/01/2025', '16/01/2025', '17/01/2025'];

  /**
   * Carrega dados quando data é selecionada
   */
  useEffect(() => {
    if (formData.dataPdp) {
      handleLoadData();
    } else {
      setData(null);
      setShowTextarea(false);
    }
  }, [formData.dataPdp]);

  /**
   * Carrega dados do servidor
   */
  const handleLoadData = async () => {
    if (!formData.dataPdp) return;

    setIsLoading(true);
    setError('');

    try {
      const result = await onLoadData(formData);
      setData(result);
      
      if (result) {
        prepareTextareaData(result);
        setShowTextarea(true);
      }
    } catch (err) {
      setError('Não foi possível acessar a Base de Dados.');
      console.error('Erro ao carregar dados:', err);
    } finally {
      setIsLoading(false);
    }
  };

  /**
   * Prepara dados para o textarea
   * Formato: uma linha por intervalo (24 linhas)
   */
  const prepareTextareaData = (dadosCarregados: DadosPotenciaSincronizada) => {
    const valores = dadosCarregados.intervalos.map(int => 
      int.valPotSincronizadaSup.toString()
    );
    setTextareaValue(valores.join('\n'));
  };

  /**
   * Remove ENTERs duplicados ao digitar no textarea
   */
  const handleTextareaKeyUp = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === 'Enter') {
      let value = textareaValue;
      // Remove \n\n seguidos
      value = value.replace(/\n\n/g, '\n');
      setTextareaValue(value);
    }
  };

  /**
   * Salva dados editados no textarea
   */
  const handleSave = async () => {
    if (!data) return;

    setIsLoading(true);
    setError('');

    try {
      const updatedData = parseTextareaData();
      await onSave(updatedData);
      
      // Recarrega dados após salvar
      await handleLoadData();
    } catch (err) {
      setError('Não foi possível gravar os dados.');
      console.error('Erro ao salvar:', err);
    } finally {
      setIsLoading(false);
    }
  };

  /**
   * Converte conteúdo do textarea de volta para estrutura de dados
   */
  const parseTextareaData = (): DadosPotenciaSincronizada => {
    if (!data) throw new Error('Dados não carregados');

    const lines = textareaValue.split('\n');
    const newIntervalos = data.intervalos.map((intervalo, idx) => ({
      ...intervalo,
      valPotSincronizadaSup: idx < lines.length ? (parseFloat(lines[idx]) || 0) : 0,
    }));

    return {
      ...data,
      intervalos: newIntervalos,
    };
  };

  /**
   * Processa mudança da data PDP
   */
  const handleDateChange = (value: string) => {
    setFormData({ dataPdp: value });
    setShowTextarea(false);
    setTextareaValue('');
  };

  /**
   * Fecha a janela (simula comportamento legado)
   */
  const handleClose = () => {
    if (window.opener) {
      window.close();
    } else {
      // Em desenvolvimento, apenas limpa o formulário
      setFormData({ dataPdp: '' });
      setData(null);
      setShowTextarea(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.systemTitle}>
          <h2>Sistema de Coleta</h2>
        </div>
        <div className={styles.pageTitle}>
          <h3>Potência Sincronizada</h3>
        </div>
      </div>

      {error && <div className={styles.errorMessage}>{error}</div>}

      <div className={styles.form}>
        <div className={styles.formGroup}>
          <label htmlFor="dataPdp">
            <strong>Data:</strong>
          </label>
          <select
            id="dataPdp"
            value={formData.dataPdp}
            onChange={e => handleDateChange(e.target.value)}
            className={styles.select}
            data-testid="select-data-pdp"
          >
            {datasPdp.map((data, idx) => (
              <option key={idx} value={data} data-testid={`option-data-${idx}`}>
                {data}
              </option>
            ))}
          </select>

          {showTextarea && (
            <button
              onClick={handleSave}
              disabled={isLoading}
              className={styles.saveButton}
              title="Salvar"
              data-testid="btn-save"
            >
              {isLoading ? 'Salvando...' : 'Salvar'}
            </button>
          )}

          <button
            onClick={handleClose}
            className={styles.closeButton}
            title="Fechar"
            data-testid="btn-close"
          >
            Fechar
          </button>
        </div>
      </div>

      {data && (
        <div className={styles.tableWrapper}>
          <div className={styles.tableContainer}>
            <table className={styles.table} data-testid="table-potencia">
              <thead>
                <tr>
                  <th className={styles.intervalColumn}>Instante</th>
                  <th className={styles.valueColumn}>Pot Sinc</th>
                </tr>
              </thead>
              <tbody>
                {data.intervalos.map((intervalo, idx) => (
                  <tr
                    key={intervalo.intervalo}
                    className={idx % 2 === 1 ? styles.evenRow : ''}
                    data-testid={`row-intervalo-${intervalo.intervalo}`}
                  >
                    <td className={styles.intervalCell} data-testid={`cell-horario-${intervalo.intervalo}`}>
                      {intervalo.horario}
                    </td>
                    <td className={styles.valueCell} data-testid={`cell-valor-${intervalo.intervalo}`}>
                      {intervalo.valPotSincronizadaSup}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          {showTextarea && (
            <div className={styles.textareaOverlay} data-testid="textarea-overlay">
              <textarea
                value={textareaValue}
                onChange={e => setTextareaValue(e.target.value)}
                onKeyUp={handleTextareaKeyUp}
                rows={27}
                className={styles.textarea}
                placeholder="Digite os valores (Enter separa os intervalos)"
                data-testid="textarea-valores"
              />
            </div>
          )}
        </div>
      )}

      {!data && !isLoading && formData.dataPdp && (
        <div className={styles.emptyState} data-testid="empty-state">
          <p>Nenhum dado disponível para a data selecionada.</p>
        </div>
      )}

      {isLoading && (
        <div className={styles.loading} data-testid="loading">
          <p>Carregando dados...</p>
        </div>
      )}
    </div>
  );
};

export default SyncPower;

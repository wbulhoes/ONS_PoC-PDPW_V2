import React, { useState, useEffect } from 'react';
import styles from './Interchange.module.css';
import {
  GridIntercambio,
  InterchangeFormData,
  ModoVisualizacao,
  OpcaoIntercambio,
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  formatarLabelColuna,
  parseIntercambioValue,
  ColunaIntercambio,
} from '../../../types/interchange';

interface InterchangeProps {
  onLoadData: (formData: InterchangeFormData) => Promise<GridIntercambio | null>;
  onSave: (formData: InterchangeFormData, valores: string) => Promise<void>;
}

const Interchange: React.FC<InterchangeProps> = ({ onLoadData, onSave }) => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [codEmpresa, setCodEmpresa] = useState<string>('');
  const [modoVisualizacao, setModoVisualizacao] = useState<ModoVisualizacao>('modalidade');
  const [intercambioSelecionado, setIntercambioSelecionado] = useState<string>('');
  const [opcoesIntercambio, setOpcoesIntercambio] = useState<OpcaoIntercambio[]>([]);
  const [gridData, setGridData] = useState<GridIntercambio | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [valoresTextarea, setValoresTextarea] = useState<string>('');
  const [mostrarTextarea, setMostrarTextarea] = useState(false);

  // Carrega dados quando empresa é selecionada
  useEffect(() => {
    if (dataPdp && codEmpresa) {
      carregarDados();
    }
  }, [dataPdp, codEmpresa, modoVisualizacao]);

  const carregarDados = async () => {
    try {
      setLoading(true);
      setError(null);
      setIntercambioSelecionado('');
      setMostrarTextarea(false);

      const formData: InterchangeFormData = {
        dataPdp,
        codEmpresa,
        modoVisualizacao,
      };

      const dados = await onLoadData(formData);
      setGridData(dados);

      // Carrega opções de intercâmbio baseado nos dados
      if (dados && dados.colunas.length > 0) {
        const opcoes: OpcaoIntercambio[] = dados.colunas.map((col) => ({
          label: col.definicao.tipoIntercambio || col.label,
          value: `${col.definicao.codEmpresDe}|${col.definicao.codEmpresPara}|${col.definicao.codContaDe}|${col.definicao.codContaPara}|${col.definicao.codContaModal}`,
        }));

        setOpcoesIntercambio(opcoes);
      } else {
        setOpcoesIntercambio([]);
      }
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
      setError('Não foi possível acessar a Base de Dados.');
    } finally {
      setLoading(false);
    }
  };

  const handleIntercambioChange = (value: string) => {
    setIntercambioSelecionado(value);

    if (value === '0' || !value) {
      setMostrarTextarea(false);
      return;
    }

    // Prepara textarea com valores do intercâmbio selecionado
    if (value === 'Todos') {
      // Editar todos os intercâmbios
      prepararTextareaTotal();
    } else {
      // Editar um intercâmbio específico
      prepararTextareaIndividual(value);
    }

    setMostrarTextarea(true);
  };

  const prepararTextareaIndividual = (value: string) => {
    if (!gridData) return;

    const coluna = gridData.colunas.find((col) => {
      const valorColuna = `${col.definicao.codEmpresDe}|${col.definicao.codEmpresPara}|${col.definicao.codContaDe}|${col.definicao.codContaPara}|${col.definicao.codContaModal}`;
      return valorColuna === value;
    });

    if (coluna) {
      const valores = coluna.valores.join('\n');
      setValoresTextarea(valores);
    }
  };

  const prepararTextareaTotal = () => {
    if (!gridData) return;

    // Formato: valores separados por TAB (colunas) e ENTER (linhas)
    const linhas: string[] = [];
    for (let i = 0; i < 48; i++) {
      const valoresLinha = gridData.colunas.map((col) => col.valores[i]);
      linhas.push(valoresLinha.join('\t'));
    }

    setValoresTextarea(linhas.join('\n'));
  };

  const parseTextareaData = (texto: string): number[][] => {
    const linhas = texto.split('\n');
    const matriz: number[][] = [];

    linhas.forEach((linha) => {
      const valores = linha.split('\t').map((val) => {
        const num = parseFloat(val.trim());
        return isNaN(num) ? 0 : num;
      });
      matriz.push(valores);
    });

    return matriz;
  };

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);

      const formData: InterchangeFormData = {
        dataPdp,
        codEmpresa,
        modoVisualizacao,
        intercambioSelecionado,
      };

      await onSave(formData, valoresTextarea);

      // Recarrega dados após salvar
      setMostrarTextarea(false);
      setIntercambioSelecionado('');
      await carregarDados();
    } catch (err) {
      console.error('Erro ao salvar:', err);
      setError('Não foi possível gravar os dados.');
    } finally {
      setLoading(false);
    }
  };

  const renderGrid = () => {
    if (!gridData || gridData.colunas.length === 0) {
      return (
        <div className={styles.emptyState} data-testid="empty-state">
          <p>Nenhum intercâmbio encontrado para os filtros selecionados.</p>
        </div>
      );
    }

    return (
      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="table-intercambio">
          <thead>
            <tr>
              <th className={styles.headerInterval}>Intervalo</th>
              <th className={styles.headerTotal}>Total</th>
              {gridData.colunas.map((coluna, idx) => (
                <th
                  key={idx}
                  className={`${styles.headerColumn} ${
                    intercambioSelecionado !== '' &&
                    intercambioSelecionado !== 'Todos' &&
                    intercambioSelecionado === `${coluna.definicao.codEmpresDe}|${coluna.definicao.codEmpresPara}|${coluna.definicao.codContaDe}|${coluna.definicao.codContaPara}|${coluna.definicao.codContaModal}`
                      ? styles.selectedColumn
                      : ''
                  }`}
                  data-testid={`header-coluna-${idx}`}
                >
                  {coluna.label}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {Array.from({ length: 48 }, (_, i) => (
              <tr key={i} data-testid={`row-intervalo-${i + 1}`}>
                <td className={styles.cellInterval}>{intervaloParaHorario(i + 1)}</td>
                <td className={styles.cellTotal}>
                  {gridData.totaisLinha[i] !== undefined ? gridData.totaisLinha[i] : 0}
                </td>
                {gridData.colunas.map((coluna, idx) => (
                  <td key={idx} className={styles.cellValue} data-testid={`cell-${i + 1}-${idx}`}>
                    {coluna.valores[i] !== undefined ? coluna.valores[i] : 0}
                  </td>
                ))}
              </tr>
            ))}
            <tr className={styles.rowFooter}>
              <td className={styles.cellInterval}>
                <strong>Total</strong>
              </td>
              <td className={styles.cellTotal}>
                <strong>{gridData.totalGeral}</strong>
              </td>
              {gridData.colunas.map((coluna, idx) => (
                <td key={idx} className={styles.cellValue}>
                  <strong>{coluna.total}</strong>
                </td>
              ))}
            </tr>
            <tr className={styles.rowFooter}>
              <td className={styles.cellInterval}>
                <strong>Média</strong>
              </td>
              <td className={styles.cellTotal}>
                <strong>{gridData.mediaGeral}</strong>
              </td>
              {gridData.colunas.map((coluna, idx) => (
                <td key={idx} className={styles.cellValue}>
                  <strong>{coluna.media}</strong>
                </td>
              ))}
            </tr>
          </tbody>
        </table>

        {mostrarTextarea && (
          <textarea
            className={styles.textarea}
            data-testid="textarea-valores"
            value={valoresTextarea}
            onChange={(e) => setValoresTextarea(e.target.value)}
            onKeyUp={(e) => {
              // Remove ENTERs duplos (chr(13) duplicado)
              if (e.key === 'Enter') {
                const valor = valoresTextarea.replace(/\n\n/g, '\n');
                setValoresTextarea(valor);
              }
            }}
            rows={48}
            style={{
              left: intercambioSelecionado === 'Todos' ? '166px' : `${102 + (opcoesIntercambio.findIndex((opt) => opt.value === intercambioSelecionado) + 1) * 64}px`,
            }}
          />
        )}
      </div>
    );
  };

  return (
    <div className={styles.container}>
      <header className={styles.header}>
        <h1 className={styles.title}>Intercâmbio</h1>
      </header>

      {error && (
        <div className={styles.error} data-testid="error-message">
          <p>{error}</p>
        </div>
      )}

      {loading && (
        <div className={styles.loading} data-testid="loading">
          <p>Carregando...</p>
        </div>
      )}

      <form className={styles.form} onSubmit={(e) => e.preventDefault()}>
        <div className={styles.formRow}>
          <label htmlFor="data-pdp">
            <strong>Data PDP:</strong>
          </label>
          <select
            id="data-pdp"
            data-testid="select-data-pdp"
            value={dataPdp}
            onChange={(e) => setDataPdp(e.target.value)}
            className={styles.select}
          >
            <option value="">Selecione uma data</option>
            <option value="15/01/2025">15/01/2025</option>
            <option value="16/01/2025">16/01/2025</option>
            <option value="17/01/2025">17/01/2025</option>
          </select>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="empresa">
            <strong>Empresa:</strong>
          </label>
          <select
            id="empresa"
            data-testid="select-empresa"
            value={codEmpresa}
            onChange={(e) => setCodEmpresa(e.target.value)}
            className={styles.select}
          >
            <option value="">Selecione uma empresa</option>
            <option value="SE">SE - Sudeste</option>
            <option value="NE">NE - Nordeste</option>
            <option value="NO">NO - Norte</option>
            <option value="SU">SU - Sul</option>
          </select>

          <label className={styles.radioLabel}>
            <input
              type="radio"
              name="modo"
              data-testid="radio-modalidade"
              checked={modoVisualizacao === 'modalidade'}
              onChange={() => setModoVisualizacao('modalidade')}
            />
            Por Modalidade
          </label>

          <label className={styles.radioLabel}>
            <input
              type="radio"
              name="modo"
              data-testid="radio-empresa"
              checked={modoVisualizacao === 'empresa'}
              onChange={() => setModoVisualizacao('empresa')}
            />
            Por Empresa
          </label>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="intercambio">
            <strong>Intercâmbios:</strong>
          </label>
          <select
            id="intercambio"
            data-testid="select-intercambio"
            value={intercambioSelecionado}
            onChange={(e) => handleIntercambioChange(e.target.value)}
            className={styles.select}
            disabled={!gridData || opcoesIntercambio.length === 0}
          >
            <option value="">Selecione um Intercâmbio</option>
            {opcoesIntercambio.map((opcao, idx) => (
              <option key={idx} value={opcao.value}>
                {opcao.label}
              </option>
            ))}
            {opcoesIntercambio.length > 0 && <option value="Todos">Todos os Intercâmbios</option>}
          </select>

          {mostrarTextarea && (
            <button
              type="button"
              data-testid="btn-save"
              onClick={handleSave}
              className={styles.btnSave}
              disabled={loading}
            >
              Salvar
            </button>
          )}
        </div>
      </form>

      {renderGrid()}
    </div>
  );
};

export default Interchange;

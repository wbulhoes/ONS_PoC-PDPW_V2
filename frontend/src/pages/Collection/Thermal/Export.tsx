import React, { useState, useEffect } from 'react';
import styles from './Export.module.css';
import {
  GridExportacao,
  ExportFormData,
  OpcaoUsina,
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  parseUsinaValue,
} from '../../../types/export';

interface ExportProps {
  onLoadData: (formData: ExportFormData) => Promise<GridExportacao | null>;
  onSave: (formData: ExportFormData, valores: string) => Promise<void>;
}

const Export: React.FC<ExportProps> = ({ onLoadData, onSave }) => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [codEmpresa, setCodEmpresa] = useState<string>('');
  const [usinaSelecionada, setUsinaSelecionada] = useState<string>('');
  const [opcoesUsina, setOpcoesUsina] = useState<OpcaoUsina[]>([]);
  const [gridData, setGridData] = useState<GridExportacao | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [valoresTextarea, setValoresTextarea] = useState<string>('');
  const [mostrarTextarea, setMostrarTextarea] = useState(false);

  // Carrega dados quando empresa é selecionada
  useEffect(() => {
    if (dataPdp && codEmpresa) {
      carregarDados();
    }
  }, [dataPdp, codEmpresa]);

  const carregarDados = async () => {
    try {
      setLoading(true);
      setError(null);
      setUsinaSelecionada('');
      setMostrarTextarea(false);

      const formData: ExportFormData = {
        dataPdp,
        codEmpresa,
      };

      const dados = await onLoadData(formData);
      setGridData(dados);

      // Carrega opções de usinas baseado nos dados
      if (dados && dados.colunas.length > 0) {
        const opcoes: OpcaoUsina[] = dados.colunas.map((col) => ({
          label: col.label,
          value: col.codUsina,
        }));

        setOpcoesUsina(opcoes);
      } else {
        setOpcoesUsina([]);
      }
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
      setError('Não foi possível acessar a Base de Dados.');
    } finally {
      setLoading(false);
    }
  };

  const handleUsinaChange = (value: string) => {
    setUsinaSelecionada(value);

    if (value === '' || !value) {
      setMostrarTextarea(false);
      return;
    }

    // Prepara textarea com valores da usina selecionada
    if (value === 'Todas as Usinas') {
      // Editar todas as usinas
      prepararTextareaTotal();
    } else {
      // Editar uma usina específica
      prepararTextareaIndividual(value);
    }

    setMostrarTextarea(true);
  };

  const prepararTextareaIndividual = (codUsina: string) => {
    if (!gridData) return;

    const coluna = gridData.colunas.find((col) => col.codUsina === codUsina);

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

  const handleSave = async () => {
    try {
      setLoading(true);
      setError(null);

      const formData: ExportFormData = {
        dataPdp,
        codEmpresa,
      };

      await onSave(formData, valoresTextarea);

      // Recarrega dados após salvar
      setMostrarTextarea(false);
      setUsinaSelecionada('');
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
          <p>Nenhuma usina encontrada para os filtros selecionados.</p>
        </div>
      );
    }

    return (
      <div className={styles.tableContainer}>
        <table className={styles.table} data-testid="table-exportacao">
          <thead>
            <tr>
              <th className={styles.headerInterval}>Intervalo</th>
              <th className={styles.headerTotal}>Total</th>
              {gridData.colunas.map((coluna, idx) => (
                <th
                  key={idx}
                  className={`${styles.headerColumn} ${
                    usinaSelecionada === coluna.codUsina ? styles.selectedColumn : ''
                  }`}
                  data-testid={`header-usina-${idx}`}
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
              left: usinaSelecionada === 'Todas as Usinas' ? '154px' : `${90 + (opcoesUsina.findIndex((opt) => opt.value === usinaSelecionada) + 1) * 64}px`,
            }}
          />
        )}
      </div>
    );
  };

  return (
    <div className={styles.container}>
      <header className={styles.header}>
        <h1 className={styles.title}>Exportação</h1>
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
        </div>

        <div className={styles.formRow}>
          <label htmlFor="usina">
            <strong>Usinas:</strong>
          </label>
          <select
            id="usina"
            data-testid="select-usina"
            value={usinaSelecionada}
            onChange={(e) => handleUsinaChange(e.target.value)}
            className={styles.select}
            disabled={!gridData || opcoesUsina.length === 0}
          >
            <option value="">Selecione uma Usina</option>
            {opcoesUsina.map((opcao, idx) => (
              <option key={idx} value={opcao.value}>
                {opcao.label}
              </option>
            ))}
            {opcoesUsina.length > 0 && <option value="Todas as Usinas">Todas as Usinas</option>}
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

export default Export;

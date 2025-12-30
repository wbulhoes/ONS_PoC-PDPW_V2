import React, { useState, useEffect } from 'react';
import styles from './Import.module.css';
import {
  GridImportacao,
  ImportFormData,
  OpcaoUsina,
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  formatarValoresParaTextarea,
  parseValoresDoTextarea,
} from '../../../types/import';

interface ImportProps {
  onLoadData: (formData: ImportFormData) => Promise<GridImportacao | null>;
  onSave: (formData: ImportFormData, valores: string) => Promise<void>;
}

const Import: React.FC<ImportProps> = ({ onLoadData, onSave }) => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [codEmpresa, setCodEmpresa] = useState<string>('');
  const [usinaSelecionada, setUsinaSelecionada] = useState<string>('');
  const [opcoesUsina, setOpcoesUsina] = useState<OpcaoUsina[]>([]);
  const [gridData, setGridData] = useState<GridImportacao | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [valoresTextarea, setValoresTextarea] = useState<string>('');
  const [mostrarTextarea, setMostrarTextarea] = useState(false);
  const [salvando, setSalvando] = useState(false);

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

      const formData: ImportFormData = {
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

    // Prepara valores em formato de matriz (intervalo x usina)
    const valoresMatriz: number[][] = [];

    for (let i = 0; i < 48; i++) {
      const linha: number[] = [];
      for (const coluna of gridData.colunas) {
        linha.push(coluna.valores[i] || 0);
      }
      valoresMatriz.push(linha);
    }

    const valores = formatarValoresParaTextarea(valoresMatriz);
    setValoresTextarea(valores);
  };

  const handleSalvar = async () => {
    if (!dataPdp || !codEmpresa || !usinaSelecionada) {
      setError('Selecione Data PDP, Empresa e Usina(s)');
      return;
    }

    try {
      setSalvando(true);
      setError(null);

      const formData: ImportFormData = {
        dataPdp,
        codEmpresa,
      };

      await onSave(formData, valoresTextarea);

      // Limpa formulário após salvar
      setUsinaSelecionada('');
      setMostrarTextarea(false);
      setValoresTextarea('');

      // Recarrega dados
      await carregarDados();
    } catch (err) {
      console.error('Erro ao salvar dados:', err);
      setError('Não foi possível gravar os dados.');
    } finally {
      setSalvando(false);
    }
  };

  const handleTextareaChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setValoresTextarea(e.target.value);
  };

  const limparFormulario = () => {
    setValoresTextarea('');
    setUsinaSelecionada('');
    setMostrarTextarea(false);
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2>Coleta - Importação de Energia</h2>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      <div className={styles.formGroup}>
        <label htmlFor="dataPdp">Data PDP:</label>
        <select
          id="dataPdp"
          value={dataPdp}
          onChange={(e) => setDataPdp(e.target.value)}
          disabled={loading}
          data-testid="data-pdp-select"
        >
          <option value="">Selecione uma data</option>
          <option value="2024-01-15">15/01/2024</option>
          <option value="2024-01-16">16/01/2024</option>
          <option value="2024-01-17">17/01/2024</option>
        </select>
      </div>

      <div className={styles.formGroup}>
        <label htmlFor="codEmpresa">Empresa:</label>
        <select
          id="codEmpresa"
          value={codEmpresa}
          onChange={(e) => setCodEmpresa(e.target.value)}
          disabled={loading}
          data-testid="empresa-select"
        >
          <option value="">Selecione uma empresa</option>
          <option value="ELETROBRAS">Eletrobras</option>
          <option value="CEMIG">CEMIG</option>
          <option value="COPEL">COPEL</option>
        </select>
      </div>

      <div className={styles.formGroup}>
        <label htmlFor="usinaSelecionada">Usinas:</label>
        <div className={styles.usinaContainer}>
          <select
            id="usinaSelecionada"
            value={usinaSelecionada}
            onChange={(e) => handleUsinaChange(e.target.value)}
            disabled={loading || opcoesUsina.length === 0}
            data-testid="usina-select"
          >
            <option value="">Selecione uma usina</option>
            {opcoesUsina.map((opcao) => (
              <option key={opcao.value} value={opcao.value}>
                {opcao.label}
              </option>
            ))}
            {opcoesUsina.length > 0 && <option value="Todas as Usinas">Todas as Usinas</option>}
          </select>
          <button onClick={handleSalvar} disabled={!mostrarTextarea || salvando} data-testid="salvar-btn">
            {salvando ? 'Salvando...' : 'Salvar'}
          </button>
        </div>
      </div>

      {mostrarTextarea && (
        <div className={styles.textareaContainer}>
          <label htmlFor="valoresTextarea">Valores:</label>
          <textarea
            id="valoresTextarea"
            value={valoresTextarea}
            onChange={handleTextareaChange}
            rows={12}
            placeholder="Insira os valores..."
            data-testid="valores-textarea"
          />
          <button onClick={limparFormulario} className={styles.btnLimpar} data-testid="limpar-btn">
            Limpar
          </button>
        </div>
      )}

      {gridData && (
        <div className={styles.gridContainer} data-testid="grid-container">
          <h3>Dados de Importação - 48 Intervalos</h3>
          <div className={styles.gridWrapper}>
            <table className={styles.grid}>
              <thead>
                <tr>
                  <th>Intervalo</th>
                  <th>Total</th>
                  {gridData.colunas.map((col) => (
                    <th
                      key={col.codUsina}
                      className={usinaSelecionada === col.codUsina ? styles.selecionada : ''}
                    >
                      {col.label}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {gridData.intervalos.map((intervalo, idx) => (
                  <tr key={idx}>
                    <td className={styles.intervalo}>{intervalo}</td>
                    <td className={styles.total}>
                      {gridData.colunas.reduce((acc, col) => acc + (col.valores[idx] || 0), 0)}
                    </td>
                    {gridData.colunas.map((col) => (
                      <td key={`${col.codUsina}-${idx}`} className={styles.valor}>
                        {col.valores[idx] || 0}
                      </td>
                    ))}
                  </tr>
                ))}
                <tr className={styles.totalRow}>
                  <td>Total</td>
                  <td>
                    {gridData.totais[0] !== undefined ? gridData.totais[0] : calcularTotalColuna(gridData.colunas.flatMap((c) => c.valores))}
                  </td>
                  {gridData.colunas.map((col) => (
                    <td key={`total-${col.codUsina}`}>{calcularTotalColuna(col.valores)}</td>
                  ))}
                </tr>
                <tr className={styles.mediaRow}>
                  <td>Média</td>
                  <td>
                    {gridData.medias[0] !== undefined
                      ? gridData.medias[0].toFixed(2)
                      : calcularMediaColuna(gridData.colunas.flatMap((c) => c.valores)).toFixed(2)}
                  </td>
                  {gridData.colunas.map((col) => (
                    <td key={`media-${col.codUsina}`}>{calcularMediaColuna(col.valores).toFixed(2)}</td>
                  ))}
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      )}

      {loading && <div className={styles.loading}>Carregando dados...</div>}
    </div>
  );
};

export default Import;

import React, { useState, useEffect } from 'react';
import {
  CargaDado,
  LoadQueryResponse,
  Empresa,
  DataPDP,
  SalvarCargaParams,
  gerarIntervalos48,
  formatarValoresParaTextarea,
  parseTextareaParaValores,
  calcularTotalEMedia,
  validarValoresCarga,
  validarSelecao,
  formatarDataPDP
} from '../../../types/load';
import styles from './index.module.css';

/**
 * Props do componente Load
 */
interface LoadProps {
  /** Callback para carregar datas PDP disponíveis */
  onLoadDatas: () => Promise<DataPDP[]>;
  
  /** Callback para carregar empresas do usuário */
  onLoadEmpresas: () => Promise<Empresa[]>;
  
  /** Callback para carregar dados de carga */
  onLoadCarga: (datPdp: string, codEmpre: string) => Promise<LoadQueryResponse>;
  
  /** Callback para salvar dados de carga */
  onSave: (params: SalvarCargaParams) => Promise<void>;
}

/**
 * Componente Load - Coleta de Carga
 * 
 * Permite a coleta e visualização de dados de previsão de carga elétrica
 * em intervalos de 30 minutos (48 intervalos por dia).
 * 
 * Funcionalidades:
 * - Seleção de data PDP
 * - Seleção de empresa/agente
 * - Visualização de dados em grid
 * - Edição via textarea overlay
 * - Cálculo automático de total e média
 * - Validação de dados
 * - Persistência no banco de dados
 */
const Load: React.FC<LoadProps> = ({
  onLoadDatas,
  onLoadEmpresas,
  onLoadCarga,
  onSave
}) => {
  // Estados principais
  const [datas, setDatas] = useState<DataPDP[]>([]);
  const [empresas, setEmpresas] = useState<Empresa[]>([]);
  const [selectedData, setSelectedData] = useState<string>('');
  const [selectedEmpresa, setSelectedEmpresa] = useState<string>('');
  
  // Estados dos dados de carga
  const [valores, setValores] = useState<number[]>(Array(48).fill(0));
  const [total, setTotal] = useState<number>(0);
  const [media, setMedia] = useState<number>(0);
  
  // Estados de interface
  const [textareaMode, setTextareaMode] = useState<boolean>(false);
  const [textareaValue, setTextareaValue] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [showSaveButton, setShowSaveButton] = useState<boolean>(false);
  const [showAlterButton, setShowAlterButton] = useState<boolean>(false);
  
  // Array com os 48 intervalos
  const intervalos = gerarIntervalos48();
  
  /**
   * Carrega datas PDP disponíveis ao montar o componente
   */
  useEffect(() => {
    loadDatas();
  }, []);
  
  /**
   * Carrega datas PDP
   */
  const loadDatas = async () => {
    try {
      setLoading(true);
      const datasCarregadas = await onLoadDatas();
      setDatas(datasCarregadas);
      setError('');
    } catch (err) {
      setError('Não foi possível acessar a Base de Dados');
    } finally {
      setLoading(false);
    }
  };
  
  /**
   * Carrega empresas quando uma data é selecionada
   */
  useEffect(() => {
    if (selectedData) {
      loadEmpresas();
    }
  }, [selectedData]);
  
  /**
   * Carrega lista de empresas
   */
  const loadEmpresas = async () => {
    try {
      setLoading(true);
      const empresasCarregadas = await onLoadEmpresas();
      setEmpresas(empresasCarregadas);
      setError('');
    } catch (err) {
      setError('Não foi possível carregar as empresas');
    } finally {
      setLoading(false);
    }
  };
  
  /**
   * Handler para mudança de data
   */
  const handleDataChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    const newData = event.target.value;
    setSelectedData(newData);
    
    // Limpa seleção de empresa e dados
    setSelectedEmpresa('');
    setValores(Array(48).fill(0));
    setTotal(0);
    setMedia(0);
    setShowAlterButton(false);
    setShowSaveButton(false);
  };
  
  /**
   * Handler para mudança de empresa
   * Carrega dados de carga da empresa selecionada
   */
  const handleEmpresaChange = async (event: React.ChangeEvent<HTMLSelectElement>) => {
    const newEmpresa = event.target.value;
    setSelectedEmpresa(newEmpresa);
    
    if (newEmpresa && selectedData) {
      await loadCargaData(selectedData, newEmpresa);
    }
  };
  
  /**
   * Carrega dados de carga da empresa/data selecionada
   */
  const loadCargaData = async (datPdp: string, codEmpre: string) => {
    try {
      setLoading(true);
      const response = await onLoadCarga(datPdp, codEmpre);
      
      // Preenche array de valores (48 intervalos)
      const novosValores = Array(48).fill(0);
      response.cargas.forEach(carga => {
        if (carga.intCarga >= 1 && carga.intCarga <= 48) {
          novosValores[carga.intCarga - 1] = carga.valCargaTran;
        }
      });
      
      setValores(novosValores);
      setTotal(response.total);
      setMedia(response.media);
      
      // Mostra botão Alterar
      setShowAlterButton(true);
      setShowSaveButton(false);
      
      setError('');
    } catch (err) {
      setError('Não foi possível carregar os dados de carga');
    } finally {
      setLoading(false);
    }
  };
  
  /**
   * Ativa modo de edição via textarea
   */
  const handleAlterarClick = () => {
    const textoValores = formatarValoresParaTextarea(valores);
    setTextareaValue(textoValores);
    setTextareaMode(true);
    setShowAlterButton(false);
    setShowSaveButton(true);
  };
  
  /**
   * Handler para mudança no textarea
   * Remove duplas quebras de linha (comportamento do legado)
   */
  const handleTextareaChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    let valor = event.target.value;
    
    // Remove duplas quebras de linha (chr(10) + chr(10))
    valor = valor.replace(/\n\n/g, '\n');
    
    setTextareaValue(valor);
  };
  
  /**
   * Handler para tecla pressionada no textarea
   * Previne duplos enters
   */
  const handleTextareaKeyUp = (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (event.key === 'Enter') {
      // Verifica se há dupla quebra de linha e remove
      const valor = textareaValue.replace(/\n\n/g, '\n');
      if (valor !== textareaValue) {
        setTextareaValue(valor);
      }
    }
  };
  
  /**
   * Salva dados de carga
   */
  const handleSalvarClick = async () => {
    // Valida seleção
    const validacaoSelecao = validarSelecao(selectedData, selectedEmpresa);
    if (!validacaoSelecao.isValid) {
      setError(validacaoSelecao.errors.join('. '));
      return;
    }
    
    // Parse valores do textarea (se em modo edição)
    let valoresParaSalvar = valores;
    if (textareaMode) {
      valoresParaSalvar = parseTextareaParaValores(textareaValue);
      
      // Atualiza grid com novos valores
      setValores(valoresParaSalvar);
      
      // Recalcula total e média
      const { total: novoTotal, media: novaMedia } = calcularTotalEMedia(valoresParaSalvar);
      setTotal(novoTotal);
      setMedia(novaMedia);
      
      // Sai do modo textarea
      setTextareaMode(false);
    }
    
    // Valida valores
    const validacaoValores = validarValoresCarga(valoresParaSalvar);
    if (!validacaoValores.isValid) {
      setError(validacaoValores.errors.join('. '));
      return;
    }
    
    // Salva no banco
    try {
      setLoading(true);
      
      await onSave({
        datPdp: selectedData,
        codEmpre: selectedEmpresa,
        valores: valoresParaSalvar
      });
      
      // Atualiza interface
      setShowSaveButton(false);
      setShowAlterButton(true);
      setError('');
      
    } catch (err) {
      setError('Não foi possível gravar os dados');
    } finally {
      setLoading(false);
    }
  };
  
  return (
    <div className={styles.loadContainer}>
      <h2 className={styles.title}>Coleta de Carga</h2>
      
      {/* Mensagem de erro */}
      {error && (
        <div className={styles.errorMessage}>
          {error}
        </div>
      )}
      
      {/* Filtros */}
      <div className={styles.filterContainer}>
        <div className={styles.filterGroup}>
          <label htmlFor="cboData" className={styles.filterLabel}>
            Data PDP:
          </label>
          <select
            id="cboData"
            className={styles.filterSelect}
            value={selectedData}
            onChange={handleDataChange}
            disabled={loading}
          >
            <option value="">Selecione uma data</option>
            {datas.map(data => (
              <option key={data.datPdp} value={data.datPdp}>
                {data.datPdpFormatada}
              </option>
            ))}
          </select>
        </div>
        
        <div className={styles.filterGroup}>
          <label htmlFor="cboEmpresa" className={styles.filterLabel}>
            Empresa:
          </label>
          <select
            id="cboEmpresa"
            className={styles.filterSelect}
            value={selectedEmpresa}
            onChange={handleEmpresaChange}
            disabled={loading || !selectedData}
          >
            <option value="">Selecione uma empresa</option>
            {empresas.map(empresa => (
              <option key={empresa.codEmpre} value={empresa.codEmpre}>
                {empresa.nomeEmpre}
              </option>
            ))}
          </select>
        </div>
      </div>
      
      {/* Grid de dados */}
      <div className={styles.gridContainer}>
        <div className={styles.gridWrapper}>
          <table className={styles.dataGrid}>
            <thead>
              <tr>
                <th className={styles.headerCell}>Hora</th>
                <th className={styles.headerCell}>Valor</th>
              </tr>
            </thead>
            <tbody>
              {intervalos.map((intervalo, index) => (
                <tr key={index} className={styles.dataRow}>
                  <td className={styles.timeCell}>{intervalo}</td>
                  <td className={styles.valueCell}>
                    {valores[index].toFixed(2)}
                  </td>
                </tr>
              ))}
              
              {/* Linha de total */}
              <tr className={styles.summaryRow}>
                <td className={styles.summaryLabelCell}>Total</td>
                <td className={styles.summaryValueCell}>
                  {total.toFixed(2)}
                </td>
              </tr>
              
              {/* Linha de média */}
              <tr className={styles.summaryRow}>
                <td className={styles.summaryLabelCell}>Média</td>
                <td className={styles.summaryValueCell}>
                  {media}
                </td>
              </tr>
            </tbody>
          </table>
          
          {/* Textarea overlay para edição */}
          {textareaMode && (
            <div className={styles.textareaOverlay}>
              <textarea
                className={styles.textareaEdit}
                value={textareaValue}
                onChange={handleTextareaChange}
                onKeyUp={handleTextareaKeyUp}
                rows={48}
                disabled={loading}
              />
            </div>
          )}
        </div>
      </div>
      
      {/* Botões de ação */}
      <div className={styles.buttonContainer}>
        {showSaveButton && (
          <button
            className={styles.saveButton}
            onClick={handleSalvarClick}
            disabled={loading}
          >
            {loading ? 'Salvando...' : 'Salvar'}
          </button>
        )}
        
        {showAlterButton && (
          <button
            className={styles.alterButton}
            onClick={handleAlterarClick}
            disabled={loading}
          >
            Alterar
          </button>
        )}
      </div>
    </div>
  );
};

export default Load;

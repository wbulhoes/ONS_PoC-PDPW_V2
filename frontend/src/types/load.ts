/**
 * Tipos e interfaces para Coleta de Carga (Load Collection)
 * 
 * Este módulo gerencia dados de previsão de carga elétrica em intervalos de 30 minutos.
 * Sistema: PDPw - Programação Diária de Produção
 * Funcionalidade: Coleta e visualização de dados de carga por empresa
 */

/**
 * Representa um dado de carga para um intervalo específico
 */
export interface CargaDado {
  /** Número do intervalo (1-48) - intervalos de 30 minutos */
  intCarga: number;
  
  /** Valor de carga transacional em MW */
  valCargaTran: number;
}

/**
 * Resposta da API ao consultar dados de carga
 */
export interface LoadQueryResponse {
  /** Lista de dados de carga (48 intervalos) */
  cargas: CargaDado[];
  
  /** Energia total (MW médios) */
  total: number;
  
  /** Média de carga (MW) */
  media: number;
}

/**
 * Dados de uma empresa/agente para seleção
 */
export interface Empresa {
  /** Código da empresa */
  codEmpre: string;
  
  /** Nome da empresa */
  nomeEmpre: string;
}

/**
 * Dados de uma data PDP disponível
 */
export interface DataPDP {
  /** Data no formato YYYYMMDD */
  datPdp: string;
  
  /** Data formatada para exibição (DD/MM/YYYY) */
  datPdpFormatada: string;
}

/**
 * Parâmetros para salvar dados de carga
 */
export interface SalvarCargaParams {
  /** Data PDP no formato YYYYMMDD */
  datPdp: string;
  
  /** Código da empresa */
  codEmpre: string;
  
  /** Lista de valores de carga (48 intervalos) */
  valores: number[];
}

/**
 * Estado de validação dos dados de carga
 */
export interface LoadValidationState {
  /** Indica se os dados são válidos */
  isValid: boolean;
  
  /** Lista de mensagens de erro */
  errors: string[];
}

/**
 * Gera array com os 48 intervalos de 30 minutos do dia
 * 
 * @returns Array com 48 strings no formato "HH:mm - HH:mm"
 * 
 * @example
 * const intervalos = gerarIntervalos48();
 * // ["00:00 - 00:30", "00:30 - 01:00", ..., "23:30 - 23:59"]
 */
export function gerarIntervalos48(): string[] {
  const intervalos: string[] = [];
  
  for (let i = 0; i < 48; i++) {
    const horaInicio = Math.floor(i / 2);
    const minutoInicio = (i % 2) * 30;
    const horaFim = Math.floor((i + 1) / 2);
    const minutoFim = ((i + 1) % 2) * 30;
    
    const inicio = `${String(horaInicio).padStart(2, '0')}:${String(minutoInicio).padStart(2, '0')}`;
    
    // Último intervalo termina em 23:59
    const fim = i === 47 
      ? '23:59' 
      : `${String(horaFim).padStart(2, '0')}:${String(minutoFim).padStart(2, '0')}`;
    
    intervalos.push(`${inicio} - ${fim}`);
  }
  
  return intervalos;
}

/**
 * Formata array de valores para exibição em textarea
 * Cada valor em uma linha
 * 
 * @param valores Array com 48 valores numéricos
 * @returns String com valores separados por quebra de linha
 * 
 * @example
 * const texto = formatarValoresParaTextarea([100, 105, 110, ...]);
 * // "100\n105\n110\n..."
 */
export function formatarValoresParaTextarea(valores: number[]): string {
  return valores.map(v => v.toString()).join('\n');
}

/**
 * Parse do conteúdo do textarea para array de valores
 * Converte cada linha em número
 * 
 * @param texto Conteúdo do textarea com valores separados por quebra de linha
 * @returns Array com 48 valores numéricos
 * 
 * @example
 * const valores = parseTextareaParaValores("100\n105\n110\n...");
 * // [100, 105, 110, ...]
 */
export function parseTextareaParaValores(texto: string): number[] {
  const linhas = texto.split('\n');
  const valores: number[] = [];
  
  for (let i = 0; i < 48; i++) {
    const linha = linhas[i] || '0';
    const valor = parseFloat(linha.trim()) || 0;
    valores.push(valor);
  }
  
  return valores;
}

/**
 * Calcula total e média dos valores de carga
 * 
 * Total = soma de todos os valores / 2 (energia em MWmed)
 * Média = soma de todos os valores / 48 (carga média em MW)
 * 
 * @param valores Array com 48 valores de carga
 * @returns Objeto com total e média calculados
 * 
 * @example
 * const { total, media } = calcularTotalEMedia([100, 105, 110, ...]);
 * // { total: 2520, media: 105 }
 */
export function calcularTotalEMedia(valores: number[]): { total: number; media: number } {
  const soma = valores.reduce((acc, val) => acc + val, 0);
  
  return {
    total: soma / 2, // Energia total em MWmed
    media: Math.floor(soma / 48) // Média de carga em MW (truncado)
  };
}

/**
 * Valida um valor de carga individual
 * 
 * Regras:
 * - Deve ser numérico
 * - Não pode ser negativo
 * - Deve ter precisão máxima de 2 casas decimais
 * 
 * @param valor Valor a ser validado
 * @returns true se válido, false caso contrário
 * 
 * @example
 * validarValorCarga(100.5); // true
 * validarValorCarga(-10); // false
 * validarValorCarga(100.123); // false
 */
export function validarValorCarga(valor: number): boolean {
  if (isNaN(valor)) {
    return false;
  }
  
  if (valor < 0) {
    return false;
  }
  
  // Valida máximo de 2 casas decimais
  const partes = valor.toString().split('.');
  if (partes.length > 1 && partes[1].length > 2) {
    return false;
  }
  
  return true;
}

/**
 * Valida array completo de valores de carga
 * 
 * @param valores Array com 48 valores
 * @returns Estado de validação com lista de erros
 * 
 * @example
 * const validation = validarValoresCarga([100, 105, -10, ...]);
 * // { isValid: false, errors: ["Valor do intervalo 3 é inválido"] }
 */
export function validarValoresCarga(valores: number[]): LoadValidationState {
  const errors: string[] = [];
  
  if (valores.length !== 48) {
    errors.push(`Devem ser informados exatamente 48 intervalos. Informados: ${valores.length}`);
  }
  
  valores.forEach((valor, index) => {
    if (!validarValorCarga(valor)) {
      errors.push(`Valor do intervalo ${index + 1} (${gerarIntervalos48()[index]}) é inválido: ${valor}`);
    }
  });
  
  return {
    isValid: errors.length === 0,
    errors
  };
}

/**
 * Valida se data e empresa foram selecionadas
 * 
 * @param datPdp Data PDP selecionada
 * @param codEmpre Código da empresa selecionada
 * @returns Estado de validação
 * 
 * @example
 * const validation = validarSelecao("20240115", "FURNAS");
 * // { isValid: true, errors: [] }
 */
export function validarSelecao(datPdp: string, codEmpre: string): LoadValidationState {
  const errors: string[] = [];
  
  if (!datPdp || datPdp === '') {
    errors.push('Selecione uma data PDP');
  }
  
  if (!codEmpre || codEmpre === '') {
    errors.push('Selecione uma empresa');
  }
  
  return {
    isValid: errors.length === 0,
    errors
  };
}

/**
 * Formata data do formato YYYYMMDD para DD/MM/YYYY
 * 
 * @param datPdp Data no formato YYYYMMDD
 * @returns Data formatada DD/MM/YYYY
 * 
 * @example
 * formatarDataPDP("20240115"); // "15/01/2024"
 */
export function formatarDataPDP(datPdp: string): string {
  if (!datPdp || datPdp.length !== 8) {
    return '';
  }
  
  // Valida se contém apenas dígitos
  if (!/^\d{8}$/.test(datPdp)) {
    return '';
  }
  
  const ano = datPdp.substring(0, 4);
  const mes = datPdp.substring(4, 6);
  const dia = datPdp.substring(6, 8);
  
  return `${dia}/${mes}/${ano}`;
}

/**
 * Converte data do formato DD/MM/YYYY para YYYYMMDD
 * 
 * @param dataFormatada Data no formato DD/MM/YYYY
 * @returns Data no formato YYYYMMDD
 * 
 * @example
 * converterDataParaPDP("15/01/2024"); // "20240115"
 */
export function converterDataParaPDP(dataFormatada: string): string {
  if (!dataFormatada || dataFormatada.length !== 10) {
    return '';
  }
  
  const partes = dataFormatada.split('/');
  if (partes.length !== 3) {
    return '';
  }
  
  const [dia, mes, ano] = partes;
  return `${ano}${mes}${dia}`;
}

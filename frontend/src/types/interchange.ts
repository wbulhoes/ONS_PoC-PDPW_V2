/**
 * Tipos para o módulo de Coleta de Intercâmbio entre Subsistemas
 * 
 * Funcionalidade: Coleta de dados de intercâmbio de energia entre empresas/subsistemas
 * Características:
 * - 48 intervalos de meia hora (00:00-23:30)
 * - Grid dinâmico baseado em intercâmbios configurados
 * - Dois modos de visualização: Por Modalidade ou Por Empresa
 * - Edição por intercâmbio individual ou todos simultaneamente
 */

/**
 * Intervalo de intercâmbio (meia hora)
 */
export interface IntercambioIntervalo {
  intervalo: number; // 1 a 48
  valorIntercambio: number; // Valor de intercâmbio em MW
}

/**
 * Definição de um intercâmbio entre empresas
 */
export interface IntercambioDefinicao {
  codEmpresDe: string; // Código empresa origem (2 chars)
  codEmpresPara: string; // Código empresa destino (2 chars)
  codContaDe: string; // Conta origem (2 chars)
  codContaPara: string; // Conta destino (2 chars)
  codContaModal: string; // Modalidade (2 chars)
  tipoIntercambio: string; // Descrição do tipo
}

/**
 * Dados completos de intercâmbio para uma definição específica
 */
export interface DadosIntercambio {
  definicao: IntercambioDefinicao;
  intervalos: IntercambioIntervalo[];
}

/**
 * Coluna do grid de intercâmbios
 */
export interface ColunaIntercambio {
  label: string; // Ex: "SE-NE/01"
  definicao: IntercambioDefinicao;
  valores: number[]; // Array de 48 valores
  total: number;
  media: number;
}

/**
 * Grid completo de intercâmbios
 */
export interface GridIntercambio {
  dataPdp: string;
  codEmpresa: string;
  colunas: ColunaIntercambio[];
  totaisLinha: number[]; // Total por intervalo (soma de todas colunas)
  totalGeral: number;
  mediaGeral: number;
}

/**
 * Modo de visualização do grid
 */
export type ModoVisualizacao = 'modalidade' | 'empresa';

/**
 * Formulário de busca de intercâmbios
 */
export interface InterchangeFormData {
  dataPdp: string;
  codEmpresa: string;
  modoVisualizacao: ModoVisualizacao;
  intercambioSelecionado?: string; // Format: "SE|NE|01|02|03" ou "Todos"
}

/**
 * Opção de intercâmbio para o dropdown
 */
export interface OpcaoIntercambio {
  label: string;
  value: string; // Format: "codEmpresDe|codEmpresPara|codContaDe|codContaPara|codContaModal"
}

/**
 * Resposta da API de intercâmbios
 */
export interface InterchangeApiResponse {
  dataPdp: string;
  codEmpresa: string;
  intercambios: DadosIntercambio[];
}

/**
 * Gera array de 48 intervalos vazios
 */
export function gerarIntervalos(): IntercambioIntervalo[] {
  return Array.from({ length: 48 }, (_, i) => ({
    intervalo: i + 1,
    valorIntercambio: 0,
  }));
}

/**
 * Converte número de intervalo para horário (formato HH:MM-HH:MM)
 */
export function intervaloParaHorario(intervalo: number): string {
  const hora = Math.floor((intervalo - 1) / 2);
  const minuto = (intervalo - 1) % 2 === 0 ? '00' : '30';
  const horaFim = minuto === '00' ? hora : hora + 1;
  const minutoFim = minuto === '00' ? '30' : '00';
  
  return `${hora.toString().padStart(2, '0')}:${minuto}-${horaFim.toString().padStart(2, '0')}:${minutoFim}`;
}

/**
 * Calcula total de uma coluna
 */
export function calcularTotalColuna(valores: number[]): number {
  return valores.reduce((acc, val) => acc + val, 0);
}

/**
 * Calcula média de uma coluna
 */
export function calcularMediaColuna(valores: number[]): number {
  const total = calcularTotalColuna(valores);
  return Math.floor(total / valores.length);
}

/**
 * Valida valor de intercâmbio (pode ser negativo, zero ou positivo)
 */
export function validarValorIntercambio(valor: number): boolean {
  return !isNaN(valor) && isFinite(valor);
}

/**
 * Formata label de coluna
 * @param def Definição do intercâmbio
 * @param modoVisualizacao Modo atual de visualização
 */
export function formatarLabelColuna(def: IntercambioDefinicao, modoVisualizacao: ModoVisualizacao): string {
  if (modoVisualizacao === 'modalidade') {
    return `${def.codEmpresDe}-${def.codEmpresPara}/${def.codContaModal}`;
  } else {
    return `${def.codEmpresDe} - ${def.codEmpresPara}`;
  }
}

/**
 * Parse do valor do dropdown de intercâmbio selecionado
 */
export function parseIntercambioValue(value: string): IntercambioDefinicao | null {
  if (value === 'Todos' || value === '0' || !value) {
    return null;
  }
  
  const parts = value.split('|');
  if (parts.length !== 5) {
    return null;
  }
  
  return {
    codEmpresDe: parts[0],
    codEmpresPara: parts[1],
    codContaDe: parts[2],
    codContaPara: parts[3],
    codContaModal: parts[4],
    tipoIntercambio: '',
  };
}

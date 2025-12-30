/**
 * Interface para Consulta de Vazão Hidráulica
 * Permite consultar dados históricos de vazão de usinas hidrelétricas
 */

/**
 * Dados de vazão por usina e intervalo
 */
export interface FlowData {
  id: number;
  dataPDP: string; // Data da PDP (formato ISO)
  codEmpresa: string; // Código da empresa
  nomeEmpresa: string; // Nome da empresa
  codUsina: string; // Código da usina
  nomeUsina: string; // Nome da usina
  intervalo: number; // Intervalo (1-48)
  horario: string; // Horário do intervalo (HH:mm-HH:mm)
  vazao: number; // Vazão em m³/s
  vazaoAfluente?: number; // Vazão afluente em m³/s
  vazaoDefluente?: number; // Vazão defluente em m³/s
  vazaoTurbinada?: number; // Vazão turbinada em m³/s
  vazaoVertida?: number; // Vazão vertida em m³/s
  observacoes?: string;
  createdAt?: string;
  updatedAt?: string;
}

/**
 * Filtros para consulta de vazão
 */
export interface FlowQueryFilters {
  dataPDPInicio?: string; // Data inicial
  dataPDPFim?: string; // Data final
  codEmpresa?: string; // Filtro por empresa
  codUsina?: string; // Filtro por usina
  tipoVazao?: TipoVazao; // Tipo de vazão a consultar
  intervaloInicio?: number; // Intervalo inicial (1-48)
  intervaloFim?: number; // Intervalo final (1-48)
}

/**
 * Tipos de vazão disponíveis
 */
export enum TipoVazao {
  TOTAL = 'total',
  AFLUENTE = 'afluente',
  DEFLUENTE = 'defluente',
  TURBINADA = 'turbinada',
  VERTIDA = 'vertida',
}

/**
 * Resposta da consulta de vazão
 */
export interface FlowQueryResponse {
  dados: FlowData[];
  total: number; // Total de registros
  pagina: number; // Página atual
  itensPorPagina: number; // Itens por página
  totalPaginas: number; // Total de páginas
}

/**
 * Opções de exportação
 */
export interface FlowExportOptions {
  formato: 'excel' | 'csv' | 'pdf';
  incluirGraficos?: boolean;
  incluirTotais?: boolean;
  agruparPor?: 'usina' | 'empresa' | 'data';
}

/**
 * Dados agregados para visualização
 */
export interface FlowAggregatedData {
  nomeUsina: string;
  vazaoMedia: number;
  vazaoMinima: number;
  vazaoMaxima: number;
  vazaoTotal: number;
  numeroIntervalos: number;
}

/**
 * Constantes para validação
 */
export const FLOW_QUERY_CONSTANTS = {
  NUM_INTERVALS: 48, // 48 intervalos de meia hora
  MIN_FLOW: 0, // Vazão mínima (m³/s)
  MAX_FLOW: 999999, // Vazão máxima (m³/s)
  DEFAULT_PAGE_SIZE: 50, // Itens por página padrão
  MAX_PAGE_SIZE: 200, // Máximo de itens por página
} as const;

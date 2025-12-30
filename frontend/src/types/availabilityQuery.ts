/**
 * Types para Consulta de Disponibilidade de Usinas
 * Módulo Query -> Hydraulic -> AvailabilityQuery
 */

/**
 * Enum para Tipo de Usina
 * H = Hidráulica, T = Térmica
 */
export enum TipoUsina {
  HIDRELETRICA = 'H',
  TERMELETRICA = 'T',
  EOLICA = 'E',
  SOLAR = 'S',
  TODAS = 'A',
}

/**
 * Enum para Status de Disponibilidade
 */
export enum StatusDisponibilidade {
  ATIVA = 'A',
  INATIVA = 'I',
  MANUTENCAO = 'M',
}

/**
 * Interface para dados de uma usina
 */
export interface UsinaInfo {
  id: number;
  codigo: string;
  nome: string;
  capacidadeInstalada: number;
  tipoUsina: TipoUsina;
}

/**
 * Interface para dados de empresa
 */
export interface EmpresaInfo {
  id: number;
  codigo: string;
  nome: string;
}

/**
 * Interface para Intervalo de tempo (24h ou 48 meias-horas)
 */
export interface IntervalData {
  intervalo: number;
  horario: string; // ex: "10:00 - 10:30"
}

/**
 * Interface para um ponto de disponibilidade
 */
export interface DisponibilidadeData {
  id: number;
  dataPDP: string;
  codEmpresa: string;
  nomeEmpresa: string;
  codUsina: string;
  nomeUsina: string;
  tipoUsina: TipoUsina;
  intervalo: number;
  horario: string;
  capacidadeMaximaDisponivel: number;
  capacidadeMinimDisponivel: number;
  percentualDisponibilidade: number;
  statusDisponibilidade: StatusDisponibilidade;
  motivoIndisponibilidade?: string;
  dataRegistro: string;
  usuarioRegistro: string;
  dataAtualizacao?: string;
  usuarioAtualizacao?: string;
}

/**
 * Interface para filtros de busca
 */
export interface AvailabilityQueryFilters {
  dataPDPInicio?: string;
  dataPDPFim?: string;
  codEmpresa?: string;
  codUsina?: string;
  tipoUsina?: TipoUsina;
  statusDisponibilidade?: StatusDisponibilidade;
  intervaloInicio?: number;
  intervaloFim?: number;
}

/**
 * Interface para agregação de dados (resumo por usina)
 */
export interface DisponibilidadeAggregated {
  codUsina: string;
  nomeUsina: string;
  tipoUsina: TipoUsina;
  capacidadeMaximaMedia: number;
  capacidadeMinimaMedia: number;
  percentualDisponibilidadeMedia: number;
  quantidadeRegistros: number;
  statusPredominante: StatusDisponibilidade;
}

/**
 * Interface para resposta da consulta
 */
export interface AvailabilityQueryResponse {
  dados: DisponibilidadeData[];
  total: number;
  pagina: number;
  itensPorPagina: number;
  totalPaginas: number;
}

/**
 * Constantes para a página de consulta
 */
export const AVAILABILITY_QUERY_CONSTANTS = {
  DEFAULT_PAGE_SIZE: 20,
  MAX_PAGE_SIZE: 100,
  INTERVALS_PER_DAY: 48,
  INTERVALS_HOURLY: 24,
  DEFAULT_CAPACITY_UNIT: 'MW',
  DATE_FORMAT: 'YYYY-MM-DD',
};

/**
 * Mapeamento de labels para tipos de usina
 */
export const TIPO_USINA_LABELS: Record<TipoUsina, string> = {
  [TipoUsina.HIDRELETRICA]: 'Hidráulica',
  [TipoUsina.TERMELETRICA]: 'Térmica',
  [TipoUsina.EOLICA]: 'Eólica',
  [TipoUsina.SOLAR]: 'Solar',
  [TipoUsina.TODAS]: 'Todas',
};

/**
 * Mapeamento de labels para status
 */
export const STATUS_LABELS: Record<StatusDisponibilidade, string> = {
  [StatusDisponibilidade.ATIVA]: 'Ativa',
  [StatusDisponibilidade.INATIVA]: 'Inativa',
  [StatusDisponibilidade.MANUTENCAO]: 'Manutenção',
};

/**
 * Gera label de intervalo (ex: "10:00 - 10:30")
 */
export function generateIntervalLabel(interval: number, totalIntervals: number = 48): string {
  if (totalIntervals === 24) {
    // Intervalos horários
    const startHour = interval - 1;
    const endHour = interval;
    return `${String(startHour).padStart(2, '0')}:00 - ${String(endHour).padStart(2, '0')}:00`;
  } else {
    // Intervalos de meia-hora (48 por dia)
    const totalMinutos = (interval - 1) * 30;
    const startHour = Math.floor(totalMinutos / 60);
    const startMin = totalMinutos % 60;
    const endHour = Math.floor((totalMinutos + 30) / 60);
    const endMin = (totalMinutos + 30) % 60;

    return `${String(startHour).padStart(2, '0')}:${String(startMin).padStart(2, '0')} - ${String(endHour).padStart(2, '0')}:${String(endMin).padStart(2, '0')}`;
  }
}

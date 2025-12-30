export interface PlantRestriction {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  tipoRestricao: string;
  dataInicio: string;
  dataFim: string;
  potenciaMaxima: number;
  potenciaMinima: number;
  observacao: string;
  status: string;
}

export interface Usina {
  id: number;
  nome: string;
  sigla: string;
  tipo: string;
}

export const TIPOS_RESTRICAO_USINA = [
  'MANUTENCAO_PROGRAMADA',
  'FALHA_EQUIPAMENTO',
  'RESTRICAO_HIDROLOGICA',
  'RESTRICAO_AMBIENTAL',
  'RESTRICAO_OPERATIVA',
  'OUTROS',
] as const;

export const STATUS_RESTRICAO = ['ATIVA', 'INATIVA', 'CANCELADA'] as const;

export type TipoRestricaoUsina = typeof TIPOS_RESTRICAO_USINA[number];
export type StatusRestricao = typeof STATUS_RESTRICAO[number];

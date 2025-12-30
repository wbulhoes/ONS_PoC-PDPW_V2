export interface UnitOutage {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  unidadeGeradoraId: number;
  unidadeNome: string;
  tipoParada: string;
  dataInicio: string;
  dataFim: string;
  motivoParada: string;
  observacao: string;
  status: string;
}

export const TIPOS_PARADA = ['PROGRAMADA', 'FORCADA', 'EMERGENCIAL', 'MANUTENCAO'] as const;
export const STATUS_PARADA = ['ATIVA', 'ENCERRADA', 'CANCELADA'] as const;

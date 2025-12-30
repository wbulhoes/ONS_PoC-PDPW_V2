export interface UnitMaintenance {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  unidadeGeradoraId: number;
  unidadeNome: string;
  tipoManutencao: string;
  dataInicio: string;
  dataFim: string;
  observacao: string;
  status: string;
}

export interface UnidadeGeradora {
  id: number;
  nome: string;
  usinaId: number;
}

export const TIPOS_MANUTENCAO = ['PREVENTIVA', 'CORRETIVA', 'PREDITIVA', 'EMERGENCIAL'] as const;
export const STATUS_MANUTENCAO = ['PROGRAMADA', 'EM_ANDAMENTO', 'CONCLUIDA', 'CANCELADA'] as const;

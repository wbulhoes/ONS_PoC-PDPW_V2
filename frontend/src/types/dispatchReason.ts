export interface ElectricalDispatchReason {
  id?: number;
  codigo: string;
  descricao: string;
  ativo: boolean;
  dataInclusao?: string;
  dataAtualizacao?: string;
}

export interface InflexibilityDispatchReason {
  id?: number;
  codigo: string;
  descricao: string;
  tipoInflexibilidade: string;
  ativo: boolean;
  dataInclusao?: string;
  dataAtualizacao?: string;
}

export const TIPOS_INFLEXIBILIDADE = [
  'TECNICA',
  'CONTRATUAL',
  'OPERACIONAL',
  'AMBIENTAL',
  'COMBUSTIVEL',
] as const;

export type TipoInflexibilidade = typeof TIPOS_INFLEXIBILIDADE[number];

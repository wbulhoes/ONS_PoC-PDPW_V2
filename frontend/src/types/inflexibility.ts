/**
 * Tipos e interfaces para o módulo de Inflexibilidade Térmica
 */

export interface InflexibilidadeData {
  codusina: string;
  intflexi: number;
  valflexitran: number;
  datpdp: string;
}

export interface InflexibilidadeForm {
  dataPdp: string;
  codEmpresa: string;
  codUsina: string;
  valores: number[];
}

export interface InflexibilidadeTable {
  intervalo: string;
  total: number;
  usinas: { [key: string]: number };
}

export interface InflexibilidadeStats {
  total: number;
  media: number;
}

export interface InflexibilidadeRequest {
  dataPdp: string;
  codEmpresa: string;
  codUsina?: string;
  dados: InflexibilidadeIntervalo[];
}

export interface InflexibilidadeIntervalo {
  intflexi: number;
  valflexitran: number;
}

export interface InflexibilidadeResponse {
  success: boolean;
  message?: string;
  data?: InflexibilidadeData[];
}

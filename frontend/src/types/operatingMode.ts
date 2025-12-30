/**
 * Tipos e interfaces para o módulo de Modalidade Operativa Térmica
 */

export interface ModalidadeOperativaData {
  codusina: string;
  intmot: number;
  valmottran: number;
  datpdp: string;
}

export interface ModalidadeOperativaForm {
  dataPdp: string;
  codEmpresa: string;
  codUsina: string;
  valores: number[];
}

export interface ModalidadeOperativaRequest {
  dataPdp: string;
  codEmpresa: string;
  codUsina?: string;
  dados: ModalidadeOperativaIntervalo[];
}

export interface ModalidadeOperativaIntervalo {
  intmot: number;
  valmottran: number;
}

export interface ModalidadeOperativaResponse {
  success: boolean;
  message?: string;
  data?: ModalidadeOperativaData[];
}

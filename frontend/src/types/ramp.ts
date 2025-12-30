/**
 * Tipos para Coleta de Rampas de Geração
 * Represents geração ramp data (taxa de mudança de potência por unidade de tempo)
 */

export interface RampData {
  codUsina: string;
  intRampa: number; // intervalo de rampas (1-48 para 30 min)
  valRampaTran: number; // valor da rampa em MW/min
}

export interface RampDataRequest {
  dataPDP: string;
  codEmpresa: string;
  codUsina: string;
  valores: number[];
}

export interface RampDataResponse {
  sucesso: boolean;
  mensagem: string;
  dados?: RampData[];
}

export interface CompanyDTO {
  codEmpresa: string;
  nomeEmpresa: string;
}

export interface PlantDTO {
  codUsina: string;
  nomeUsina: string;
  ordem: number;
}

export interface TableRowData {
  intervalo: number;
  valores: { [key: string]: number };
  total: number;
  media: number;
}

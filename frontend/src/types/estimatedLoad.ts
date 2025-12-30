export interface EstimatedLoadData {
  dataPDP: string;
  empresa: string;
  submercado: string;
  intervalo: number; // 1-50
  valorCarga: number;
}

export interface EstimatedLoadTableRow {
  intervalo: number;
  horario: string;
  valores: { [submercado: string]: number };
  total: number;
}

export interface EstimatedLoadFormData {
  dataPDP: string;
  empresa: string;
  submercado: string;
}

export interface EstimatedInterchangeData {
  dataPDP: string;
  empresa: string;
  intercambio: string; // "SubA -> SubB"
  intervalo: number; // 1-50
  valorIntercambio: number;
}

export interface EstimatedInterchangeTableRow {
  intervalo: number;
  horario: string;
  valores: { [intercambio: string]: number };
  total: number;
}

export interface EstimatedInterchangeFormData {
  dataPDP: string;
  empresa: string;
  intercambio: string;
}

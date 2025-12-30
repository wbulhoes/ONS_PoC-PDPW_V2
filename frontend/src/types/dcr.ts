export interface DCRData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorDCR: number;
}

export interface DCRTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface DCRFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

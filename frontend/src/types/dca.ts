export interface DCAData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorDCA: number;
}

export interface DCATableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface DCAFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

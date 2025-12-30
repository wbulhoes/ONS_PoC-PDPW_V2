export interface OutOfMeritCREData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorCREForaMerito: number;
}

export interface OutOfMeritCRETableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface OutOfMeritCREFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

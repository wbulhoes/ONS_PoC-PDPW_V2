export interface SOMData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorSOM: number;
}

export interface SOMTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface SOMFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

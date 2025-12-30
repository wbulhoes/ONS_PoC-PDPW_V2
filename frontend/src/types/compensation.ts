export interface CompensationData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorCompensacao: number;
}

export interface CompensationTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface CompensationFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

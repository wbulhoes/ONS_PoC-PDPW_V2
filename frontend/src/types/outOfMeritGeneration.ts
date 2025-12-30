export interface OutOfMeritGenerationData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorGeracaoForaMerito: number;
}

export interface OutOfMeritGenerationTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface OutOfMeritGenerationFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

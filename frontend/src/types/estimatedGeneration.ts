export interface EstimatedGenerationData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-50
  valorGeracao: number;
}

export interface EstimatedGenerationTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface EstimatedGenerationFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

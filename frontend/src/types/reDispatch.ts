export interface REDispatchData {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-48
  valorDespachoRE: number;
}

export interface REDispatchTableRow {
  intervalo: number;
  horario: string;
  valores: { [usina: string]: number };
  total: number;
}

export interface REDispatchFormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

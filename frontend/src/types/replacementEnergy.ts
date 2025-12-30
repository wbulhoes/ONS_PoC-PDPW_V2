export interface ReplacementEnergy {
  id: string;
  data: string;
  agenteId: string;
  usinaId: string;
  valores: number[]; // 48 values for half-hourly intervals
}

export interface ReplacementEnergyData {
  usinaId: string;
  usinaNome: string;
  valores: number[];
}

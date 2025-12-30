/**
 * Migrado de: legado/pdpw/frmColResFaltaComb.aspx
 */

export interface FuelShortageUsina {
  codUsina: string;
  nomeUsina?: string; // Opcional, se disponível
}

export interface FuelShortageInterval {
  id: number; // 1 a 48
  hora: string; // Ex: "00:00"
  valores: Record<string, number>; // codUsina -> valor
  total: number;
}

export interface FuelShortageData {
  usinas: FuelShortageUsina[];
  intervalos: FuelShortageInterval[];
}

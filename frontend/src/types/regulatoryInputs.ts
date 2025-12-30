// IR1 - Nível de Partida
export interface IR1Data {
  dataPDP: string;
  empresa: string;
  usina: string;
  nivelPartida: number;
}

export interface IR1FormData {
  dataPDP: string;
  empresa: string;
  usina: string;
  nivelPartida: string;
}

// IR2 - Dia -1 (24 intervalos horários)
export interface IR2Data {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-24
  valor: number;
}

export interface IR2TableRow {
  hora: string;
  valor: number;
}

export interface IR2FormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

// IR3 - Dia -2 (24 intervalos horários)
export interface IR3Data {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-24
  valor: number;
}

export interface IR3TableRow {
  hora: string;
  valor: number;
}

export interface IR3FormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

// IR4 - Carga da Ande (24 intervalos horários)
export interface IR4Data {
  dataPDP: string;
  empresa: string;
  usina: string;
  intervalo: number; // 1-24
  carga: number;
}

export interface IR4TableRow {
  hora: string;
  carga: number;
}

export interface IR4FormData {
  dataPDP: string;
  empresa: string;
  usina: string;
}

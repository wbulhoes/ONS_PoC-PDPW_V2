export interface ProgramacaoUsina {
  codUsina: string;
  nomeUsina?: string;
  volumeProgramacao?: number | null;
  precoProgramacao?: number | null;
}

export interface ProgramacaoEnergetica {
  dataPdp: string;
  codEmpresa: string;
  usinas: ProgramacaoUsina[];
}

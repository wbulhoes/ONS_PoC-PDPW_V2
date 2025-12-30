export interface MachineStatus {
  id: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  unidadeGeradoraId: number;
  unidadeNome: string;
  potenciaGerada: number;
  horaInicio: string;
  horaFim?: string;
  observacao: string;
}

export interface OperatingMachine extends MachineStatus {
  statusOperacao: string;
  modoOperacao: string;
}

export interface StoppedMachine extends MachineStatus {
  motivoParada: string;
  tipoParada: string;
}

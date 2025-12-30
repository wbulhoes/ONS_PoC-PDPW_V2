export interface ContractedInflexibility {
  id: string;
  codUsina: string;
  nomeUsina: string;
  dataInicio: string;
  dataFim: string;
  valor: number;
  habilitado: boolean;
  contrato: 'Posterior a 2011' | 'Anterior a 2011'; // Assuming based on legacy code
}

export interface ContractedInflexibilityFilter {
  usina?: string;
}

/**
 * Tipos para Coleta de Dados Elétricos (Razão Elétrica Transformada)
 * Migrado de: legado/pdpw/frmColEletrica.aspx
 */

/**
 * Intervalo de 30 minutos com valor de razão elétrica
 */
export interface RazaoEletricaIntervalo {
  /** Número do intervalo (1-48) */
  intervalo: number;
  /** Valor da razão elétrica transformada */
  valor: number;
}

/**
 * Usina com seus dados de razão elétrica
 */
export interface RazaoEletricaUsina {
  /** Código da usina */
  codUsina: string;
  /** Nome da usina */
  nomeUsina?: string;
  /** Ordem de exibição */
  ordem?: number;
  /** Intervalos de razão elétrica (48 períodos de 30min) */
  intervalos: RazaoEletricaIntervalo[];
  /** Total da usina */
  total?: number;
  /** Média da usina */
  media?: number;
}

/**
 * Dados completos de razão elétrica
 */
export interface DadosEletricosData {
  /** Data PDP (formato: YYYYMMDD) */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Nome da empresa */
  nomeEmpresa: string;
  /** Lista de usinas com seus dados elétricos */
  usinas: RazaoEletricaUsina[];
  /** Total geral de todos os intervalos */
  totalGeral?: number;
}

/**
 * Formulário de filtro
 */
export interface DadosEletricosForm {
  /** Data PDP selecionada */
  dataPdp: string;
  /** Empresa selecionada */
  codEmpresa: string;
  /** Usina selecionada (opcional, vazio = todas) */
  codUsina?: string;
}

/**
 * Opção para dropdown
 */
export interface SelectOption {
  value: string;
  label: string;
}

/**
 * Totalizador por intervalo
 */
export interface TotalIntervalo {
  /** Número do intervalo */
  intervalo: number;
  /** Total do intervalo */
  total: number;
}

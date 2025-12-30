/**
 * Tipos para RRO (Registro de Restrição Operativa)
 * Migrado de: legado/pdpw/frmColRRO.aspx
 */

/**
 * Intervalo de 30 minutos com valor de RRO
 */
export interface RROIntervalo {
  /** Número do intervalo (1-48) */
  intervalo: number;
  /** Valor RRO */
  valor: number;
}

/**
 * Usina com seus dados de RRO
 */
export interface RROUsina {
  /** Código da usina */
  codUsina: string;
  /** Nome da usina (ou código se nome não disponível) */
  nomeUsina: string;
  /** Ordem de exibição */
  ordem: number;
  /** Intervalos de RRO (48 períodos de 30min) */
  intervalos: RROIntervalo[];
}

/**
 * Dados completos de RRO
 */
export interface RROData {
  /** Data PDP (formato: YYYYMMDD) */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Nome da empresa */
  nomeEmpresa: string;
  /** Lista de usinas com seus dados */
  usinas: RROUsina[];
}

/**
 * Formulário de filtro
 */
export interface RROForm {
  /** Data PDP selecionada */
  dataPdp: string;
  /** Empresa selecionada */
  codEmpresa: string;
  /** Usina selecionada (opcional) */
  codUsina?: string;
}

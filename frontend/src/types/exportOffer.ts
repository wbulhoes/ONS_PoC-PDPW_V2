/**
 * Tipos para Oferta de Exportação
 * Migrado de: legado/pdpw/frmCnsOfertaExportacao.aspx
 */

/**
 * Intervalo de 30 minutos com valor de oferta de exportação
 */
export interface OfertaExportacaoIntervalo {
  /** Número do intervalo (1-48) */
  intervalo: number;
  /** Valor de exportação em MW (Valor Sugerido Agente) */
  valor: number;
  /** Valor Sugerido ONS (para análise) */
  valorOns?: number;
}

/**
 * Usina conversora com suas ofertas de exportação
 */
export interface OfertaExportacaoUsina {
  /** Código da usina */
  codUsina: string;
  /** Nome da usina */
  nomeUsina: string;
  /** Código da conversora */
  codConversora: string;
  /** Ordem de despacho */
  ordem: number;
  /** Intervalos de exportação (48 períodos de 30min) */
  intervalos: OfertaExportacaoIntervalo[];
  /** Status da oferta (Aprovada, Reprovada, Pendente) - Opcional para análise */
  status?: 'Aprovada' | 'Reprovada' | 'Pendente';
}

/**
 * Dados completos de oferta de exportação
 */
export interface OfertaExportacaoData {
  /** Data PDP (formato: YYYYMMDD) */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Nome da empresa */
  nomeEmpresa: string;
  /** Lista de usinas conversoras com suas ofertas */
  usinas: OfertaExportacaoUsina[];
}

/**
 * Formulário de filtro
 */
export interface OfertaExportacaoForm {
  /** Data PDP selecionada */
  dataPdp: string;
  /** Empresa selecionada */
  codEmpresa: string;
  /** Usina selecionada (opcional) */
  codUsina?: string;
}

/**
 * Opção para dropdown
 */
export interface SelectOption {
  value: string;
  label: string;
}

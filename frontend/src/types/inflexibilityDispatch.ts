/**
 * Types para Despacho de Inflexibilidade Térmica (frmColDespInflex)
 * Migração de: legado/pdpw/frmColDespInflex.aspx.vb
 */

/**
 * Dados de despacho de inflexibilidade por intervalo
 */
export interface DespachoInflexibilidadeIntervalo {
  intervalo: number; // 1 a 48
  valor: number; // valmiftran
}

/**
 * Dados de despacho de inflexibilidade por usina
 */
export interface DespachoInflexibilidadeUsina {
  codUsina: string;
  intervalos: DespachoInflexibilidadeIntervalo[];
}

/**
 * Estrutura completa de dados de despacho de inflexibilidade
 */
export interface DespachoInflexibilidadeData {
  dataPdp: string; // formato YYYYMMDD
  codEmpresa: string;
  usinas: DespachoInflexibilidadeUsina[];
}

/**
 * Formulário de despacho de inflexibilidade
 */
export interface DespachoInflexibilidadeForm {
  dataPdp: string;
  codEmpresa: string;
  codUsina: string; // pode ser usina individual ou "todas"
}

/**
 * Request para salvar despacho de inflexibilidade
 */
export interface DespachoInflexibilidadeRequest {
  dataPdp: string;
  codEmpresa: string;
  codUsina: string;
  intervalos: DespachoInflexibilidadeIntervalo[];
}

/**
 * Response ao buscar dados de despacho de inflexibilidade
 */
export interface DespachoInflexibilidadeResponse {
  sucesso: boolean;
  mensagem?: string;
  dados?: DespachoInflexibilidadeData;
}

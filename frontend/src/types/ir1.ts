/**
 * Tipos para Insumos Recebimento - IR1 (Nível de Partida)
 * Migrado de: legado/pdpw/frmColIR1.aspx
 * API alvo: /api/insumos-recebimento/ir1
 */

/**
 * Item de nível de partida por usina
 */
export interface NivelPartidaItem {
  /** Identificador da usina (backend) */
  usinaId: number;
  /** Nome da usina (opcional) */
  usinaNome?: string;
  /** Nível (metros) */
  nivel: number;
  /** Volume (hm³) */
  volume: number;
}

/**
 * Dados IR1 (Nível de Partida) retornados pelo backend
 */
export interface IR1Data {
  /** Identificador do registro (opcional) */
  id?: number;
  /** Data de referência no formato ISO (YYYY-MM-DD ou ISO 8601) */
  dataReferencia: string;
  /** Lista de níveis de partida por usina */
  niveisPartida: NivelPartidaItem[];
  /** Metadados opcionais */
  criadoEm?: string;
  atualizadoEm?: string;
  usuarioCriacao?: string;
}

/**
 * DTO para criação/atualização de IR1
 */
export interface IR1Dto {
  /** Data de referência no formato ISO (YYYY-MM-DD ou ISO 8601) */
  dataReferencia: string;
  /** Lista de níveis de partida por usina */
  niveisPartida: NivelPartidaItem[];
}

/**
 * Formulário de filtro/entrada para IR1
 */
export interface IR1Form {
  /** Data PDP selecionada (YYYYMMDD) */
  dataPdp: string;
  /** Empresa selecionada (opcional para filtros por empresa) */
  codEmpresa?: string;
  /** Usina selecionada (opcional, vazio = todas) */
  codUsina?: string;
}

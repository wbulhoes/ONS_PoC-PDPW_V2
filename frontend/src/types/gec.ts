/**
 * Tipos para Coleta de GEC (Geração em Cascata)
 * Representa a geração em cascata por usina e intervalo de tempo
 * Utilizado para coleta de dados operacionais das usinas
 */

/**
 * Dado individual de GEC
 * Armazenado em tb_GEC no banco de dados
 */
export interface GECData {
  /** Data PDP em formato yyyyMMdd */
  datPdp: string;
  /** Código da usina */
  codUsina: string;
  /** Intervalo de 30 minutos (1-48) */
  intGec: number;
  /** Valor de GEC transmitido */
  valGecTran: number;
  /** Código da empresa */
  codEmpresa: string;
}

/**
 * Requisição para pesquisar dados de GEC
 */
export interface GECDataRequest {
  /** Data PDP em formato yyyy-MM-dd */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Código da usina (ou "all" para todas) */
  codUsina: string;
}

/**
 * Resposta com dados de GEC
 */
export interface GECDataResponse {
  /** Lista de dados de GEC */
  dados: GECData[];
  /** Mensagem de sucesso ou erro */
  mensagem?: string;
  /** Indicador de sucesso */
  sucesso: boolean;
}

/**
 * Requisição para salvar dados de GEC
 */
export interface SaveGECDataRequest {
  /** Data PDP em formato yyyy-MM-dd */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Lista de dados alterados */
  dados: GECData[];
}

/**
 * DTO para empresa
 */
export interface CompanyDTO {
  /** Código da empresa */
  codEmpresa: string;
  /** Nome da empresa */
  nomeEmpresa: string;
}

/**
 * DTO para usina
 */
export interface PlantDTO {
  /** Código da usina */
  codUsina: string;
  /** Nome da usina */
  nomeUsina: string;
  /** Ordem de exibição */
  ordem: number;
}

/**
 * Linha da tabela de GEC
 * Agregação de múltiplas usinas por intervalo
 */
export interface TableRowData {
  /** Intervalo de 30 minutos (1-48) */
  intervalo: number;
  /** Mapa de valores por código de usina */
  valores: Record<string, number | string>;
  /** Total (soma de todas as usinas) */
  total: number;
  /** Média (total / número de usinas) */
  media: number;
}

/**
 * Estrutura para requisição de atualização em batch
 */
export interface BatchUpdateRequest {
  dataPdp: string;
  codEmpresa: string;
  dadosAtualizados: GECData[];
  usuarioId: string;
}

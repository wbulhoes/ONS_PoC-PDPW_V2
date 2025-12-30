/**
 * Tipos para Coleta de GES (Geração de Energia Substituição)
 * Representa a geração de substituição por usina e intervalo de tempo
 * Utilizado para coleta de dados operacionais das usinas
 */

/**
 * Dado individual de GES
 * Armazenado em tb_GES no banco de dados
 */
export interface GESData {
  /** Data PDP em formato yyyyMMdd */
  datPdp: string;
  /** Código da usina */
  codUsina: string;
  /** Intervalo de 30 minutos (1-48) */
  intGes: number;
  /** Valor de GES transmitido */
  valGesTran: number;
  /** Código da empresa */
  codEmpresa: string;
}

/**
 * Requisição para pesquisar dados de GES
 */
export interface GESDataRequest {
  /** Data PDP em formato yyyy-MM-dd */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Código da usina (ou "all" para todas) */
  codUsina: string;
}

/**
 * Resposta com dados de GES
 */
export interface GESDataResponse {
  /** Lista de dados de GES */
  dados: GESData[];
  /** Mensagem de sucesso ou erro */
  mensagem?: string;
  /** Indicador de sucesso */
  sucesso: boolean;
}

/**
 * Requisição para salvar dados de GES
 */
export interface SaveGESDataRequest {
  /** Data PDP em formato yyyy-MM-dd */
  dataPdp: string;
  /** Código da empresa */
  codEmpresa: string;
  /** Lista de dados alterados */
  dados: GESData[];
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
 * Linha da tabela de GES
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

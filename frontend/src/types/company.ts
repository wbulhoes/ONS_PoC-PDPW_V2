/**
 * Types para Cadastro/Consulta de Empresas (frmCnsEmpresa)
 * Migração de: legado/pdpw/frmCnsEmpresa.aspx.vb
 */

/**
 * Dados de uma empresa no sistema PDP
 */
export interface Company {
  codempre: string; // Código da empresa
  nomempre: string; // Nome da empresa
  sigempre: string; // Sigla da empresa
  idgtpoempre: string; // ID GTPO
  contr: boolean; // É controladora de área?
  regiao?: string | null; // Região (se controladora)
  sistema?: string | null; // Sistema (se controladora)
  area_contr: boolean; // É controlada por outra empresa?
  infpdp: boolean; // PDP informado
  area_nao_contr?: string | null; // Área (se não controladora)
  empresa_nao_contr?: string | null; // Empresa controladora (se não controladora)
}

/**
 * Response ao buscar lista de empresas
 */
export interface CompanyListResponse {
  sucesso: boolean;
  mensagem?: string;
  empresas: Company[];
  total: number;
}

/**
 * Parâmetros de paginação
 */
export interface PaginationParams {
  page: number;
  pageSize: number;
}

/**
 * Filtros de busca de empresas
 */
export interface CompanyFilters {
  codigo?: string;
  nome?: string;
  sigla?: string;
  controladora?: boolean;
  regiao?: string;
  sistema?: string;
}

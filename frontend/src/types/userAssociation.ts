/**
 * Tipos TypeScript para o módulo de Associação Usuário X Empresa (frmAssocUsuar.aspx)
 *
 * Sistema: PDPw - Programação Diária de Produção
 * Módulo: Administração > Associação Usuário X Empresa
 */

/**
 * Interface para associação usuário-empresa
 */
export interface UserCompanyAssociation {
  codempre: string; // Código da empresa
  sigempre: string; // Sigla da empresa
  usuar_id: string; // ID do usuário
  usuar_nome: string; // Nome do usuário
}

/**
 * Interface para dropdown de empresas
 */
export interface CompanyOption {
  codempre: string;
  sigempre: string;
}

/**
 * Interface para dropdown de usuários
 */
export interface UserOption {
  usuar_id: string;
  usuar_nome: string;
}

/**
 * Interface para resposta da API de listagem de associações
 */
export interface UserCompanyAssociationListResponse {
  sucesso: boolean;
  mensagem?: string;
  associacoes: UserCompanyAssociation[];
  total: number;
}

/**
 * Interface para parâmetros de filtro
 */
export interface AssociationFilters {
  codempre?: string;
  usuar_id?: string;
}

/**
 * Interface para parâmetros de paginação
 */
export interface AssociationPaginationParams {
  page: number;
  pageSize: number;
  filters?: AssociationFilters;
}

/**
 * Interface para operações de associação
 */
export interface AssociationOperationResponse {
  sucesso: boolean;
  mensagem: string;
}

/**
 * Interface para dados de nova associação
 */
export interface NewAssociation {
  codempre: string;
  usuar_id: string;
}

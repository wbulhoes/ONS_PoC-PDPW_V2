/**
 * Tipos TypeScript para o módulo de Usuários (Cadastro de Usuários)
 * Sistema: PDPw - Programação Diária de Produção
 */

/**
 * Interface para dados de usuário conforme backend
 */
export interface User {
  id: number;
  nome: string;
  email: string;
  telefone: string;
  equipePDPId: number;
  perfil: string;
}

/**
 * Interface para formulário de cadastro/edição de usuário
 */
export interface UserFormData {
  nome: string;
  email: string;
  telefone: string;
  equipePDPId: number;
  perfil: string;
}

/**
 * Interface para resposta da API de listagem de usuários
 */
export interface UserListResponse {
  sucesso: boolean;
  mensagem?: string;
  usuarios: User[];
  total: number;
}

/**
 * Interface para parâmetros de filtro de usuários
 */
export interface UserFilters {
  nome?: string;
  email?: string;
  telefone?: string;
  perfil?: string;
}

/**
 * Interface para parâmetros de paginação
 */
export interface UserPaginationParams {
  page: number;
  pageSize: number;
  filters?: UserFilters;
}

/**
 * Interface para operações CRUD de usuário
 */
export interface UserOperationResponse {
  sucesso: boolean;
  mensagem: string;
  usuario?: User;
}

/**
 * Enum para tipos de operação no formulário
 */
export enum UserFormMode {
  CREATE = 'create',
  EDIT = 'edit',
  VIEW = 'view',
}

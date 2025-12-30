/**
 * Tipos para o módulo de Cadastro/Consulta de Usinas
 * Página legada: frmCnsUsina.aspx
 */

export interface Usina {
  codUsina: string;
  sigUsina: string;
  nomUsina: string;
  tipUsina: string;
  codEmpre?: string;
  ordem?: number;
}

export interface PlantQueryParams {
  codEmpresa: string;
  page?: number;
  pageSize?: number;
}

export interface PlantQueryResponse {
  usinas: Usina[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface EmpresaOption {
  codEmpre: string;
  sigEmpre: string;
}

/**
 * Tipos de usina válidos no sistema
 */
export enum TipoUsina {
  HIDRO = 'HIDRO',
  TERMO = 'TERMO',
  EOLICA = 'EOLICA',
  SOLAR = 'SOLAR',
  NUCLEAR = 'NUCLEAR',
}

/**
 * Mapeia código de tipo de usina para descrição
 */
export function getTipoUsinaLabel(tipo: string): string {
  const mapa: Record<string, string> = {
    H: 'Hidro',
    T: 'Termo',
    E: 'Eólica',
    S: 'Solar',
    N: 'Nuclear',
  };
  return mapa[tipo] || tipo;
}

/**
 * Formata código de usina para exibição
 */
export function formatarCodigoUsina(codigo: string): string {
  return codigo.trim().toUpperCase();
}

/**
 * Valida código de empresa
 */
export function isCodigoEmpresaValido(codigo: string): boolean {
  return codigo !== '' && codigo !== '0';
}

/**
 * Interface principal representando uma Usina no sistema PDP
 * (alias para compatibilidade)
 */
export interface Plant extends Usina {}

/**
 * Interface para formulário de criação/edição de usina
 */
export interface PlantForm {
  codUsina?: string;
  sigUsina: string;
  nomUsina: string;
  tipUsina: string;
  codEmpre: string;
  ativo?: boolean;
}

/**
 * Interface para filtros de consulta de usinas
 */
export interface PlantFilters {
  codEmpresa?: string;
  tipUsina?: string;
  sigUsina?: string;
  nomUsina?: string;
  apenasAtivas?: boolean;
}

/**
 * Estados possíveis do componente PlantRegistry
 */
export type PlantRegistryState = 'idle' | 'loading' | 'success' | 'error';

/**
 * Props do componente PlantRegistry
 */
export interface PlantRegistryProps {
  preselectedCompanyCode?: string;
}

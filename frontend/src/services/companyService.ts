/**
 * Company Service
 *
 * Handles fetching and managing company data from backend
 * T009: Create Company service in frontend/src/services/companyService.ts
 */

import { apiClient } from './apiClient';

export interface Empresa {
  id: number;
  nome: string;
  cnpj: string;
  telefone: string;
  email: string;
  endereco: string;
}

export interface CreateEmpresaDto {
  nome: string;
  cnpj: string;
  telefone: string;
  email: string;
  endereco: string;
}

export interface UpdateEmpresaDto {
  nome?: string;
  cnpj?: string;
  telefone?: string;
  email?: string;
  endereco?: string;
}

export const companyService = {
  /**
   * Get all companies
   */
  async getAll(): Promise<Empresa[]> {
    return apiClient.get<Empresa[]>('/empresas');
  },

  /**
   * Get company by ID
   */
  async getById(id: number): Promise<Empresa> {
    return apiClient.get<Empresa>(`/empresas/${id}`);
  },

  /**
   * Create new company
   */
  async create(dto: CreateEmpresaDto): Promise<Empresa> {
    return apiClient.post<Empresa>('/empresas', dto);
  },

  /**
   * Update company
   */
  async update(id: number, dto: UpdateEmpresaDto): Promise<Empresa> {
    return apiClient.put<Empresa>(`/empresas/${id}`, dto);
  },

  /**
   * Delete company
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/empresas/${id}`);
  },
};

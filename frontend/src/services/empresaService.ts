/**
 * Serviço para integração com API de Empresas
 * Endpoint: /api/empresas
 */

import { apiClient } from './apiClient';

export interface Empresa {
  id: string;
  codigo: string;
  nome: string;
  tipo: 'GERADORA' | 'DISTRIBUIDORA' | 'COMERCIALIZADORA' | 'TRANSMISSORA';
  sigla?: string;
  ativo: boolean;
}

export interface CreateEmpresaDto {
  codigo: string;
  nome: string;
  tipo: 'GERADORA' | 'DISTRIBUIDORA' | 'COMERCIALIZADORA' | 'TRANSMISSORA';
  sigla?: string;
}

export interface UpdateEmpresaDto extends Partial<CreateEmpresaDto> {
  ativo?: boolean;
}

/**
 * Serviço para operações com Empresas
 */
export const empresaService = {
  /**
   * Obtém todas as empresas ativas
   */
  async getAll(): Promise<Empresa[]> {
    return apiClient.get<Empresa[]>('/empresas');
  },

  /**
   * Obtém todas as empresas (incluindo inativas)
   */
  async getAllIncludingInactive(): Promise<Empresa[]> {
    return apiClient.get<Empresa[]>('/empresas/all');
  },

  /**
   * Obtém empresa por ID
   */
  async getById(id: string): Promise<Empresa> {
    return apiClient.get<Empresa>(`/empresas/${id}`);
  },

  /**
   * Obtém empresa por código
   */
  async getByCodigo(codigo: string): Promise<Empresa> {
    return apiClient.get<Empresa>(`/empresas/codigo/${codigo}`);
  },

  /**
   * Obtém empresas por tipo
   */
  async getByTipo(tipo: string): Promise<Empresa[]> {
    return apiClient.get<Empresa[]>(`/empresas/tipo/${tipo}`);
  },

  /**
   * Cria nova empresa
   */
  async create(data: CreateEmpresaDto): Promise<Empresa> {
    return apiClient.post<Empresa>('/empresas', data);
  },

  /**
   * Atualiza empresa existente
   */
  async update(id: string, data: UpdateEmpresaDto): Promise<Empresa> {
    return apiClient.put<Empresa>(`/empresas/${id}`, data);
  },

  /**
   * Deleta empresa (soft delete - marca como inativa)
   */
  async delete(id: string): Promise<void> {
    return apiClient.delete(`/empresas/${id}`);
  },
};

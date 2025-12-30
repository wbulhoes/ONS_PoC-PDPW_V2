/**
 * Serviço para integração com API de Usinas
 * Endpoint: /api/usinas
 */

import { apiClient } from './apiClient';

export interface Usina {
  id: string;
  codigo: string;
  nome: string;
  empresaId: string;
  empresaNome?: string;
  tipoUsina: 'HIDROELETRICA' | 'TERMOELETRICA' | 'EOLICA' | 'SOLAR' | 'NUCLEAR';
  subsistema: 'SUDESTE' | 'SUL' | 'NORDESTE' | 'NORTE';
  potenciaInstalada: number; // MW
  ativo: boolean;
}

export interface CreateUsinaDto {
  codigo: string;
  nome: string;
  empresaId: string;
  tipoUsina: 'HIDROELETRICA' | 'TERMOELETRICA' | 'EOLICA' | 'SOLAR' | 'NUCLEAR';
  subsistema: 'SUDESTE' | 'SUL' | 'NORDESTE' | 'NORTE';
  potenciaInstalada: number;
}

export interface UpdateUsinaDto extends Partial<CreateUsinaDto> {
  ativo?: boolean;
}

/**
 * Serviço para operações com Usinas
 */
export const usinaService = {
  /**
   * Obtém todas as usinas ativas
   */
  async getAll(): Promise<Usina[]> {
    return apiClient.get<Usina[]>('/usinas');
  },

  /**
   * Obtém todas as usinas (incluindo inativas)
   */
  async getAllIncludingInactive(): Promise<Usina[]> {
    return apiClient.get<Usina[]>('/usinas/all');
  },

  /**
   * Obtém usina por ID
   */
  async getById(id: string): Promise<Usina> {
    return apiClient.get<Usina>(`/usinas/${id}`);
  },

  /**
   * Obtém usina por código
   */
  async getByCodigo(codigo: string): Promise<Usina> {
    return apiClient.get<Usina>(`/usinas/codigo/${codigo}`);
  },

  /**
   * Obtém usinas de uma empresa
   */
  async getByEmpresa(empresaId: string): Promise<Usina[]> {
    return apiClient.get<Usina[]>(`/usinas/empresa/${empresaId}`);
  },

  /**
   * Obtém usinas por tipo
   */
  async getByTipo(tipo: string): Promise<Usina[]> {
    return apiClient.get<Usina[]>(`/usinas/tipo/${tipo}`);
  },

  /**
   * Obtém usinas por subsistema
   */
  async getBySubsistema(subsistema: string): Promise<Usina[]> {
    return apiClient.get<Usina[]>(`/usinas/subsistema/${subsistema}`);
  },

  /**
   * Cria nova usina
   */
  async create(data: CreateUsinaDto): Promise<Usina> {
    return apiClient.post<Usina>('/usinas', data);
  },

  /**
   * Atualiza usina existente
   */
  async update(id: string, data: UpdateUsinaDto): Promise<Usina> {
    return apiClient.put<Usina>(`/usinas/${id}`, data);
  },

  /**
   * Deleta usina (soft delete - marca como inativa)
   */
  async delete(id: string): Promise<void> {
    return apiClient.delete(`/usinas/${id}`);
  },
};

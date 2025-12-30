/**
 * Plant Service
 * 
 * Handles fetching and managing plant (usina) data from backend
 * T010: Create Plant service in frontend/src/services/plantService.ts
 */

import { apiClient } from './apiClient';

export interface Usina {
  id: number;
  codigo: string;
  nome: string;
  tipoUsinaId: number;
  empresaId: number;
  capacidadeInstalada: number;
  localizacao?: string;
  dataOperacao?: string;
  ativo: boolean;
}

export interface CreateUsinaDto {
  codigo: string;
  nome: string;
  tipoUsinaId: number;
  empresaId: number;
  capacidadeInstalada: number;
  localizacao?: string;
  dataOperacao?: string;
  ativo?: boolean;
}

export interface UpdateUsinaDto {
  codigo?: string;
  nome?: string;
  tipoUsinaId?: number;
  empresaId?: number;
  capacidadeInstalada?: number;
  localizacao?: string;
  dataOperacao?: string;
  ativo?: boolean;
}

export const plantService = {
  /**
   * Get all plants
   */
  async getAll(): Promise<Usina[]> {
    try {
      const data = await apiClient.get<Usina[]>('/usinas');
      return data;
    } catch (error) {
      throw new Error(`Failed to fetch plants: ${error.message}`);
    }
  },

  /**
   * Get plant by ID
   */
  async getById(id: number): Promise<Usina> {
    try {
      const data = await apiClient.get<Usina>(`/usinas/${id}`);
      return data;
    } catch (error) {
      throw new Error(`Failed to fetch plant: ${error.message}`);
    }
  },

  /**
   * Create new plant
   */
  async create(dto: CreateUsinaDto): Promise<Usina> {
    try {
      const data = await apiClient.post<Usina>('/usinas', dto);
      return data;
    } catch (error) {
      throw new Error(`Failed to create plant: ${error.message}`);
    }
  },

  /**
   * Update plant
   */
  async update(id: number, dto: UpdateUsinaDto): Promise<Usina> {
    try {
      const data = await apiClient.put<Usina>(`/usinas/${id}`, dto);
      return data;
    } catch (error) {
      throw new Error(`Failed to update plant: ${error.message}`);
    }
  },

  /**
   * Delete plant
   */
  async delete(id: number): Promise<void> {
    try {
      return await apiClient.delete(`/usinas/${id}`);
    } catch (error) {
      throw new Error(`Failed to delete plant: ${error.message}`);
    }
  },
};

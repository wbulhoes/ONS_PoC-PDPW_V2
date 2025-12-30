import { apiClient } from './apiClient';
import { transformFromApi, transformToApi } from '../utils/dtoTransformers';
import { normalizeError } from '../utils/errorHandling';

/**
 * T023: Update energetic service with backend integration
 * Service for managing energetic data (Razão Energética Transformada)
 */

export interface DadoEnergetico {
  id: number;
  usinaId: number;
  usinaNome?: string;
  dataReferencia: string;
  intervalo: number;
  valorMW: number;
  razaoEnergetica: number;
  observacao?: string;
}

export interface CreateDadoEnergeticoDto {
  usinaId: number;
  dataReferencia: string;
  intervalo: number;
  valorMW: number;
  razaoEnergetica: number;
  observacao?: string;
}

export interface UpdateDadoEnergeticoDto {
  valorMW?: number;
  razaoEnergetica?: number;
  observacao?: string;
}

export const energeticService = {
  /**
   * Obtém todos os dados energéticos
   */
  async getAll(): Promise<DadoEnergetico[]> {
    try {
      const data = await apiClient.get<DadoEnergetico[]>('/dadosenergeticos');
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Obtém um dado energético por ID
   */
  async getById(id: number): Promise<DadoEnergetico> {
    try {
      const data = await apiClient.get<DadoEnergetico>(`/dadosenergeticos/${id}`);
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Obtém dados energéticos por período
   */
  async getByPeriod(dataInicio: string, dataFim: string): Promise<DadoEnergetico[]> {
    try {
      const data = await apiClient.get<DadoEnergetico[]>(
        `/dadosenergeticos/periodo?dataInicio=${dataInicio}&dataFim=${dataFim}`
      );
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Obtém dados energéticos por usina e data
   */
  async getByUsinaAndDate(usinaId: number, dataReferencia: string): Promise<DadoEnergetico[]> {
    try {
      const data = await apiClient.get<DadoEnergetico[]>(
        `/dadosenergeticos/usina/${usinaId}/data/${dataReferencia}`
      );
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Cria um novo dado energético
   */
  async create(data: CreateDadoEnergeticoDto): Promise<DadoEnergetico> {
    try {
      const payload = transformToApi(data);
      const response = await apiClient.post<DadoEnergetico>('/dadosenergeticos', payload);
      return transformFromApi(response);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to create energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Atualiza um dado energético existente
   */
  async update(id: number, data: UpdateDadoEnergeticoDto): Promise<DadoEnergetico> {
    try {
      const payload = transformToApi(data);
      const response = await apiClient.put<DadoEnergetico>(`/dadosenergeticos/${id}`, payload);
      return transformFromApi(response);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to update energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Remove um dado energético
   */
  async delete(id: number): Promise<void> {
    try {
      return await apiClient.delete(`/dadosenergeticos/${id}`);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to delete energetic data: ${normalizedError.message}`);
    }
  },

  /**
   * Cria ou atualiza múltiplos dados energéticos (bulk)
   * T024: DTO transformers applied for bulk operations
   */
  async bulkUpsert(dados: CreateDadoEnergeticoDto[]): Promise<DadoEnergetico[]> {
    try {
      const payload = transformToApi(dados);
      const response = await apiClient.post<DadoEnergetico[]>('/dadosenergeticos/bulk', payload);
      return transformFromApi(response);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to bulk upsert energetic data: ${normalizedError.message}`);
    }
  },
};

/**
 * Plant Type Service
 * 
 * Handles fetching plant type reference data from backend
 * T011: Create PlantType service in frontend/src/services/plantTypeService.ts
 */

import { apiClient } from './apiClient';
import { PlantType } from '../types/api';
import { transformFromApi, transformToApi } from '../utils/dtoTransformers';
import { normalizeError } from '../utils/errorHandling';

export interface CreatePlantTypeDto {
  name: string;
  code: string;
}

export interface UpdatePlantTypeDto {
  name?: string;
  code?: string;
}

export const plantTypeService = {
  /**
   * Get all plant types
   */
  async getAll(): Promise<PlantType[]> {
    try {
      const data = await apiClient.get<PlantType[]>('/plant-types');
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch plant types: ${normalizedError.message}`);
    }
  },

  /**
   * Get plant type by ID
   */
  async getById(id: number): Promise<PlantType> {
    try {
      const data = await apiClient.get<PlantType>(`/plant-types/${id}`);
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to fetch plant type: ${normalizedError.message}`);
    }
  },

  /**
   * Create new plant type
   */
  async create(dto: CreatePlantTypeDto): Promise<PlantType> {
    try {
      const payload = transformToApi(dto);
      const data = await apiClient.post<PlantType>('/plant-types', payload);
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to create plant type: ${normalizedError.message}`);
    }
  },

  /**
   * Update plant type
   */
  async update(id: number, dto: UpdatePlantTypeDto): Promise<PlantType> {
    try {
      const payload = transformToApi(dto);
      const data = await apiClient.put<PlantType>(`/plant-types/${id}`, payload);
      return transformFromApi(data);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to update plant type: ${normalizedError.message}`);
    }
  },

  /**
   * Delete plant type
   */
  async delete(id: number): Promise<void> {
    try {
      return await apiClient.delete(`/plant-types/${id}`);
    } catch (error) {
      const normalizedError = normalizeError(error);
      throw new Error(`Failed to delete plant type: ${normalizedError.message}`);
    }
  },
};

import { PlantConverter } from '../types/plantConverter';
import { apiClient } from './apiClient';

export const plantConverterService = {
  async getAll(): Promise<PlantConverter[]> {
    return apiClient.get<PlantConverter[]>('/plant-converters');
  },

  async getByAgent(agentId: string): Promise<PlantConverter[]> {
    return apiClient.get<PlantConverter[]>(`/plant-converters/agent/${agentId}`);
  },

  async save(data: Omit<PlantConverter, 'id' | 'usinaNome' | 'conversoraNome'>): Promise<PlantConverter> {
    return apiClient.post<PlantConverter>('/plant-converters', data);
  },

  async delete(id: string): Promise<void> {
    return apiClient.delete(`/plant-converters/${id}`);
  }
};

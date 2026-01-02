import { ReplacementEnergy } from '../types/replacementEnergy';
import { apiClient } from './apiClient';

export const replacementEnergyService = {
  async getByDateAndAgent(date: string, agenteId: string): Promise<ReplacementEnergy[]> {
    return apiClient.get<ReplacementEnergy[]>(`/replacement-energy?date=${date}&agenteId=${agenteId}`);
  },

  async save(data: ReplacementEnergy): Promise<ReplacementEnergy> {
    return apiClient.post<ReplacementEnergy>('/replacement-energy', data);
  }
};

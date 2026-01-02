import { apiClient } from './apiClient';

export const insumosService = {
  async getInsumos(dataPdp: string) {
    return apiClient.get<any>(`/insumos/${dataPdp}`);
  },

  async saveInsumos(data: any) {
    return apiClient.post<void>('/insumos', data);
  },
};

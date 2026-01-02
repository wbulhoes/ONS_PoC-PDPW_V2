import { RROData } from '../types/rro';
import { apiClient } from './apiClient';

export const rroService = {
  async getOffers(dataPdp: string, codEmpresa: string, codUsina?: string): Promise<RROData> {
    const query = `?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}${codUsina ? `&codUsina=${codUsina}` : ''}`;
    return apiClient.get<RROData>(`/rro${query}`);
  },

  async saveOffers(data: RROData): Promise<void> {
    return apiClient.post('/rro', data);
  },
};

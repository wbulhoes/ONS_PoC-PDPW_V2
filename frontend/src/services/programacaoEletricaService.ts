import { apiClient } from './apiClient';

export const programacaoEletricaService = {
  async getProgramacaoEletrica(dataPdp: string) {
    return apiClient.get<any>(`/programacao-eletrica?dataPdp=${dataPdp}`);
  },

  async saveProgramacaoEletrica(payload: any) {
    return apiClient.post<void>('/programacao-eletrica', payload);
  },
};

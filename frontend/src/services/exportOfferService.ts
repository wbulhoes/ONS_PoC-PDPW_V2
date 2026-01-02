import { apiClient } from './apiClient';
import { OfertaExportacaoData } from '../types/exportOffer';

const BASE_URL = '/ofertas-exportacao';

export const exportOfferService = {
  async getOffers(dataPdp: string, codEmpresa: string, codUsina?: string): Promise<OfertaExportacaoData> {
    const query = `?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}${codUsina ? `&codUsina=${codUsina}` : ''}`;
    return apiClient.get<OfertaExportacaoData>(`${BASE_URL}${query}`);
  },

  async saveOffers(data: OfertaExportacaoData): Promise<void> {
    return apiClient.post<void>(BASE_URL, data);
  },

  async approveOffers(dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> {
    return apiClient.post<void>(`${BASE_URL}/aprovar`, { dataPdp, codEmpresa, usinas });
  },

  async rejectOffers(dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> {
    return apiClient.post<void>(`${BASE_URL}/rejeitar`, { dataPdp, codEmpresa, usinas });
  },
};

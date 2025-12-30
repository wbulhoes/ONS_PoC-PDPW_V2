import api from './api';
import { OfertaExportacaoData, OfertaExportacaoUsina } from '../types/exportOffer';

const BASE_URL = '/ofertas-exportacao';

export const exportOfferService = {
  async getOffers(dataPdp: string, codEmpresa: string, codUsina?: string): Promise<OfertaExportacaoData> {
    const query = `?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}${codUsina ? `&codUsina=${codUsina}` : ''}`;
    try {
      const response = await api.get(`${BASE_URL}${query}`);
      return response.data;
    } catch (err) {
      console.warn(`exportOfferService.getOffers: fallback mock - ${err}`);
      // fallback mock
      await new Promise((resolve) => setTimeout(resolve, 500));
      const usinas: OfertaExportacaoUsina[] = [];

      const numUsinas = codUsina ? 1 : 3;

      for (let i = 1; i <= numUsinas; i++) {
        const currentCodUsina = codUsina || `US${i.toString().padStart(3, '0')}`;
        usinas.push({
          codUsina: currentCodUsina,
          nomeUsina: `Usina ${currentCodUsina}`,
          codConversora: `CV${i}`,
          ordem: i,
          intervalos: Array.from({ length: 48 }, (_, idx) => ({
            intervalo: idx + 1,
            valor: Math.floor(Math.random() * 100),
            valorOns: Math.floor(Math.random() * 100),
          })),
          status: Math.random() > 0.5 ? 'Aprovada' : 'Pendente',
        });
      }

      return {
        dataPdp,
        codEmpresa,
        nomeEmpresa: `Empresa ${codEmpresa}`,
        usinas,
      };
    }
  },

  async saveOffers(data: OfertaExportacaoData): Promise<void> {
    try {
      await api.post(BASE_URL, data);
    } catch (err) {
      console.warn(`exportOfferService.saveOffers: fallback mock - ${err}`);
      await new Promise((resolve) => setTimeout(resolve, 1000));
      console.log('Saved offers (mock):', data);
    }
  },

  async approveOffers(dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> {
    try {
      await api.post(`${BASE_URL}/aprovar`, { dataPdp, codEmpresa, usinas });
    } catch (err) {
      console.warn(`exportOfferService.approveOffers: fallback mock - ${err}`);
      await new Promise((resolve) => setTimeout(resolve, 800));
      console.log('Approved offers (mock):', usinas);
    }
  },

  async rejectOffers(dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> {
    try {
      await api.post(`${BASE_URL}/rejeitar`, { dataPdp, codEmpresa, usinas });
    } catch (err) {
      console.warn(`exportOfferService.rejectOffers: fallback mock - ${err}`);
      await new Promise((resolve) => setTimeout(resolve, 800));
      console.log('Rejected offers (mock):', usinas);
    }
  },
};

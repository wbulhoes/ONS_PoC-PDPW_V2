import api from './api';
import { OfertaExportacaoData, OfertaExportacaoUsina } from '../types/exportOffer';

const BASE_URL = '/oferta-exportacao';

export const exportOfferService = {
  getOffers: async (dataPdp: string, codEmpresa: string, codUsina?: string): Promise<OfertaExportacaoData> => {
    // Mock implementation
    // In a real scenario, this would be:
    // const response = await api.get(`${BASE_URL}?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}&codUsina=${codUsina || ''}`);
    // return response.data;

    await new Promise((resolve) => setTimeout(resolve, 500));

    // Mock data generation
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
          valorOns: Math.floor(Math.random() * 100), // Mock ONS value
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
  },

  saveOffers: async (data: OfertaExportacaoData): Promise<void> => {
    // Mock implementation
    // await api.post(BASE_URL, data);
    await new Promise((resolve) => setTimeout(resolve, 1000));
    console.log('Saved offers:', data);
  },

  approveOffers: async (dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 800));
    console.log('Approved offers for:', usinas);
  },

  rejectOffers: async (dataPdp: string, codEmpresa: string, usinas: string[]): Promise<void> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 800));
    console.log('Rejected offers for:', usinas);
  },
};

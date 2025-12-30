import { RROData, RROUsina } from '../types/rro';
import { apiClient } from './apiClient';

export const rroService = {
  async getOffers(dataPdp: string, codEmpresa: string, codUsina?: string): Promise<RROData> {
    const query = `?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}${codUsina ? `&codUsina=${codUsina}` : ''}`;
    try {
      return await apiClient.get<RROData>(`/rro${query}`);
    } catch (err) {
      console.warn(`rroService.getOffers fallback mock - ${err}`);
      await new Promise((resolve) => setTimeout(resolve, 500));

      const usinas: RROUsina[] = [];
      const numUsinas = codUsina ? 1 : 3;

      for (let i = 1; i <= numUsinas; i++) {
        const currentCodUsina = codUsina || `US${i.toString().padStart(3, '0')}`;
        usinas.push({
          codUsina: currentCodUsina,
          nomeUsina: `Usina ${currentCodUsina}`,
          ordem: i,
          intervalos: Array.from({ length: 48 }, (_, idx) => ({
            intervalo: idx + 1,
            valor: Math.floor(Math.random() * 50),
          })),
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

  async saveOffers(data: RROData): Promise<void> {
    try {
      await apiClient.post('/rro', data);
    } catch (err) {
      console.warn(`rroService.saveOffers fallback mock - ${err}`);
      await new Promise((resolve) => setTimeout(resolve, 1000));
      console.log('Saved RRO data (mock):', data);
    }
  },
};

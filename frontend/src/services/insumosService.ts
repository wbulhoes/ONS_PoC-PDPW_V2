import { apiClient } from './apiClient';

export const insumosService = {
  async getInsumos(dataPdp: string) {
    try {
      return await apiClient.get<any>(`/insumos/${dataPdp}`);
    } catch (err) {
      console.warn(`insumosService.getInsumos fallback mock - ${err}`);
      await new Promise((r) => setTimeout(r, 400));
      return {
        dataPdp,
        insumos: [
          { id: 'I001', nome: 'Insumo A', valor: 123 },
          { id: 'I002', nome: 'Insumo B', valor: 456 },
        ],
      };
    }
  },

  async saveInsumos(data: any) {
    try {
      await apiClient.post<void>('/insumos', data);
    } catch (err) {
      console.warn(`insumosService.saveInsumos fallback mock - ${err}`);
      await new Promise((r) => setTimeout(r, 600));
      console.log('Saved insumos (mock)', data);
    }
  },
};

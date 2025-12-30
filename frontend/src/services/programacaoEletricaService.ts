import { apiClient } from './apiClient';

export const programacaoEletricaService = {
  async getProgramacaoEletrica(dataPdp: string) {
    const endpoint = `/programacao-eletrica?dataPdp=${dataPdp}`;

    try {
      return await apiClient.get<any>(endpoint);
    } catch (err) {
      console.warn(`programacaoEletricaService.getProgramacaoEletrica: fallback mock - ${err}`);
      await new Promise((r) => setTimeout(r, 400));
      return {
        dataPdp,
        itens: [
          { cod: 'PE001', descricao: 'Carga Prevista', valor: 500 },
          { cod: 'PE002', descricao: 'Perdas', valor: 20 },
        ],
      };
    }
  },

  async saveProgramacaoEletrica(payload: any) {
    const endpoint = `/programacao-eletrica`;

    try {
      await apiClient.post<void>(endpoint, payload);
    } catch (err) {
      console.warn(`programacaoEletricaService.saveProgramacaoEletrica: fallback mock - ${err}`);
      await new Promise((r) => setTimeout(r, 600));
      console.log('Saved programacao eletrica (mock)', payload);
    }
  },
};

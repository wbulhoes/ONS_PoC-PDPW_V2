import { ProgramacaoEnergetica } from '../types/programacao';
import { apiClient } from './apiClient';

export const programacaoService = {
  async getProgramacao(dataPdp: string, codEmpresa: string): Promise<ProgramacaoEnergetica> {
    const endpoint = `/programacao-energetica?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}`;

    try {
      // Tenta consumir endpoint real da API
      const res = await apiClient.get<ProgramacaoEnergetica>(endpoint);
      return res;
    } catch (err) {
      // Fallback para mock em caso de erro/endpoint não existir (mantém POC funcional)
      console.warn(`programacaoService.getProgramacao: fallback para mock - ${err}`);
      await new Promise((r) => setTimeout(r, 500));
      const usinas = [
        { codUsina: 'US001', nomeUsina: 'Usina 1', volumeProgramacao: 100, precoProgramacao: 120.5 },
        { codUsina: 'US002', nomeUsina: 'Usina 2', volumeProgramacao: 80, precoProgramacao: 110.0 },
      ];
      return { dataPdp, codEmpresa, usinas };
    }
  },

  async saveProgramacao(data: ProgramacaoEnergetica): Promise<void> {
    const endpoint = `/programacao-energetica`;

    try {
      await apiClient.post<void>(endpoint, data);
    } catch (err) {
      console.warn(`programacaoService.saveProgramacao: fallback mock - ${err}`);
      // fallback behaviour to keep UI functional in POC
      await new Promise((r) => setTimeout(r, 600));
      console.log('Saved programacao energetica (mock)', data);
    }
  },
};

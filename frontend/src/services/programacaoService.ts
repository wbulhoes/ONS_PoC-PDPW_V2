import { ProgramacaoEnergetica } from '../types/programacao';
import { apiClient } from './apiClient';

export const programacaoService = {
  async getProgramacao(dataPdp: string, codEmpresa: string): Promise<ProgramacaoEnergetica> {
    const endpoint = `/programacao-energetica?dataPdp=${dataPdp}&codEmpresa=${codEmpresa}`;
    return apiClient.get<ProgramacaoEnergetica>(endpoint);
  },

  async saveProgramacao(data: ProgramacaoEnergetica): Promise<void> {
    return apiClient.post<void>('/programacao-energetica', data);
  },
};

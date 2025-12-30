import { ProgramacaoEnergetica } from '../types/programacao';

export const programacaoService = {
  getProgramacao: async (dataPdp: string, codEmpresa: string): Promise<ProgramacaoEnergetica> => {
    // Mock implementation
    await new Promise((r) => setTimeout(r, 500));
    const usinas = [
      { codUsina: 'US001', nomeUsina: 'Usina 1', volumeProgramacao: 100, precoProgramacao: 120.5 },
      { codUsina: 'US002', nomeUsina: 'Usina 2', volumeProgramacao: 80, precoProgramacao: 110.0 },
    ];
    return { dataPdp, codEmpresa, usinas };
  },

  saveProgramacao: async (data: ProgramacaoEnergetica): Promise<void> => {
    await new Promise((r) => setTimeout(r, 600));
    console.log('Saved programacao energetica', data);
  },
};

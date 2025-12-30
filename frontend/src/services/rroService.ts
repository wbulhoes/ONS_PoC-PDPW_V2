import { RROData, RROUsina } from '../types/rro';

export const rroService = {
  getOffers: async (dataPdp: string, codEmpresa: string, codUsina?: string): Promise<RROData> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 500));

    // Mock data generation
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
  },

  saveOffers: async (data: RROData): Promise<void> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 1000));
    console.log('Saved RRO data:', data);
  },
};

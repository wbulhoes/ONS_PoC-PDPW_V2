export const programacaoEletricaService = {
  getProgramacaoEletrica: async (dataPdp: string) => {
    await new Promise((r) => setTimeout(r, 400));
    return {
      dataPdp,
      itens: [
        { cod: 'PE001', descricao: 'Carga Prevista', valor: 500 },
        { cod: 'PE002', descricao: 'Perdas', valor: 20 },
      ],
    };
  },

  saveProgramacaoEletrica: async (payload: any) => {
    await new Promise((r) => setTimeout(r, 600));
    console.log('Saved programacao eletrica', payload);
  },
};

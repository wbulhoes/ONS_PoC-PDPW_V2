export const insumosService = {
  getInsumos: async (dataPdp: string) => {
    // Mock implementation
    await new Promise((r) => setTimeout(r, 400));
    return {
      dataPdp,
      insumos: [
        { id: 'I001', nome: 'Insumo A', valor: 123 },
        { id: 'I002', nome: 'Insumo B', valor: 456 },
      ],
    };
  },

  saveInsumos: async (data: any) => {
    await new Promise((r) => setTimeout(r, 600));
    console.log('Saved insumos', data);
  },
};

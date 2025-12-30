import { ReplacementEnergy } from '../types/replacementEnergy';

const MOCK_DATA: ReplacementEnergy[] = [
  {
    id: '1',
    data: new Date().toISOString().split('T')[0],
    agenteId: '1',
    usinaId: '1',
    valores: Array(48).fill(0).map(() => Math.floor(Math.random() * 100))
  },
  {
    id: '2',
    data: new Date().toISOString().split('T')[0],
    agenteId: '1',
    usinaId: '2',
    valores: Array(48).fill(0).map(() => Math.floor(Math.random() * 100))
  }
];

export const replacementEnergyService = {
  getByDateAndAgent: async (date: string, agenteId: string): Promise<ReplacementEnergy[]> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    return MOCK_DATA.filter(d => d.data === date && d.agenteId === agenteId);
  },

  save: async (data: ReplacementEnergy): Promise<ReplacementEnergy> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    const index = MOCK_DATA.findIndex(d => d.id === data.id);
    if (index >= 0) {
      MOCK_DATA[index] = data;
      return data;
    } else {
      const newData = { ...data, id: Math.random().toString() };
      MOCK_DATA.push(newData);
      return newData;
    }
  }
};

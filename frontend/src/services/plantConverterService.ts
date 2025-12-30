import { PlantConverter } from '../types/plantConverter';

const MOCK_DATA: PlantConverter[] = [
  {
    id: '1',
    usinaId: '1',
    usinaNome: 'Usina A',
    conversoraId: '101',
    conversoraNome: 'Conversora X',
    perda: 2.5,
    prioridade: 1
  },
  {
    id: '2',
    usinaId: '2',
    usinaNome: 'Usina B',
    conversoraId: '102',
    conversoraNome: 'Conversora Y',
    perda: 3.0,
    prioridade: 2
  }
];

export const plantConverterService = {
  getAll: async (): Promise<PlantConverter[]> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    return [...MOCK_DATA];
  },

  getByAgent: async (agentId: string): Promise<PlantConverter[]> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    // Mock filtering by agent (assuming mock data belongs to agent 1)
    if (agentId === '1') return [...MOCK_DATA];
    return [];
  },

  save: async (data: Omit<PlantConverter, 'id' | 'usinaNome' | 'conversoraNome'>): Promise<PlantConverter> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    const newId = Math.random().toString();
    const newItem: PlantConverter = {
      ...data,
      id: newId,
      usinaNome: `Usina ${data.usinaId}`, // Mock name
      conversoraNome: `Conversora ${data.conversoraId}` // Mock name
    };
    MOCK_DATA.push(newItem);
    return newItem;
  },

  delete: async (id: string): Promise<void> => {
    await new Promise(resolve => setTimeout(resolve, 500));
    const index = MOCK_DATA.findIndex(d => d.id === id);
    if (index >= 0) {
      MOCK_DATA.splice(index, 1);
    }
  }
};

import { PlantConverter } from '../types/plantConverter';
import { apiClient } from './apiClient';

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
  async getAll(): Promise<PlantConverter[]> {
    try {
      return await apiClient.get<PlantConverter[]>('/plant-converters');
    } catch (err) {
      console.warn(`plantConverterService.getAll fallback mock - ${err}`);
      await new Promise(resolve => setTimeout(resolve, 500));
      return [...MOCK_DATA];
    }
  },

  async getByAgent(agentId: string): Promise<PlantConverter[]> {
    try {
      return await apiClient.get<PlantConverter[]>(`/plant-converters/agent/${agentId}`);
    } catch (err) {
      console.warn(`plantConverterService.getByAgent fallback mock - ${err}`);
      await new Promise(resolve => setTimeout(resolve, 500));
      if (agentId === '1') return [...MOCK_DATA];
      return [];
    }
  },

  async save(data: Omit<PlantConverter, 'id' | 'usinaNome' | 'conversoraNome'>): Promise<PlantConverter> {
    try {
      return await apiClient.post<PlantConverter>('/plant-converters', data);
    } catch (err) {
      console.warn(`plantConverterService.save fallback mock - ${err}`);
      await new Promise(resolve => setTimeout(resolve, 500));
      const newId = Math.random().toString();
      const newItem: PlantConverter = {
        ...data,
        id: newId,
        usinaNome: `Usina ${data.usinaId}`,
        conversoraNome: `Conversora ${data.conversoraId}`,
      };
      MOCK_DATA.push(newItem);
      return newItem;
    }
  },

  async delete(id: string): Promise<void> {
    try {
      await apiClient.delete(`/plant-converters/${id}`);
    } catch (err) {
      console.warn(`plantConverterService.delete fallback mock - ${err}`);
      await new Promise(resolve => setTimeout(resolve, 500));
      const index = MOCK_DATA.findIndex(d => d.id === id);
      if (index >= 0) {
        MOCK_DATA.splice(index, 1);
      }
    }
  }
};

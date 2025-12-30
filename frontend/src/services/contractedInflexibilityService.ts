import { ContractedInflexibility } from '../types/contractedInflexibility';
import { apiClient } from './apiClient';

const MOCK_DATA: ContractedInflexibility[] = [
  {
    id: '1',
    codUsina: 'US001',
    nomeUsina: 'USINA A',
    dataInicio: '2023-01-01',
    dataFim: '2023-12-31',
    valor: 100.5,
    habilitado: true,
    contrato: 'Posterior a 2011'
  },
  {
    id: '2',
    codUsina: 'US002',
    nomeUsina: 'USINA B',
    dataInicio: '2023-06-01',
    dataFim: '2024-06-01',
    valor: 250.0,
    habilitado: false,
    contrato: 'Anterior a 2011'
  }
];

export const contractedInflexibilityService = {
  async getAll(): Promise<ContractedInflexibility[]> {
    try {
      return await apiClient.get<ContractedInflexibility[]>('/contracted-inflexibilities');
    } catch (err) {
      console.warn(`contractedInflexibilityService.getAll fallback mock - ${err}`);
      return new Promise((resolve) => {
        setTimeout(() => resolve([...MOCK_DATA]), 500);
      });
    }
  },

  async create(data: Omit<ContractedInflexibility, 'id'>): Promise<ContractedInflexibility> {
    try {
      return await apiClient.post<ContractedInflexibility>('/contracted-inflexibilities', data);
    } catch (err) {
      console.warn(`contractedInflexibilityService.create fallback mock - ${err}`);
      return new Promise((resolve) => {
        setTimeout(() => {
          const newItem = { ...data, id: Math.random().toString(36).substr(2, 9) } as ContractedInflexibility;
          MOCK_DATA.push(newItem);
          resolve(newItem);
        }, 500);
      });
    }
  },

  async update(id: string, data: Partial<ContractedInflexibility>): Promise<ContractedInflexibility> {
    try {
      return await apiClient.put<ContractedInflexibility>(`/contracted-inflexibilities/${id}`, data);
    } catch (err) {
      console.warn(`contractedInflexibilityService.update fallback mock - ${err}`);
      return new Promise((resolve, reject) => {
        setTimeout(() => {
          const index = MOCK_DATA.findIndex(item => item.id === id);
          if (index !== -1) {
            MOCK_DATA[index] = { ...MOCK_DATA[index], ...data } as ContractedInflexibility;
            resolve(MOCK_DATA[index]);
          } else {
            reject(new Error('Item not found'));
          }
        }, 500);
      });
    }
  },

  async delete(id: string): Promise<void> {
    try {
      await apiClient.delete(`/contracted-inflexibilities/${id}`);
    } catch (err) {
      console.warn(`contractedInflexibilityService.delete fallback mock - ${err}`);
      return new Promise((resolve) => {
        setTimeout(() => {
          const index = MOCK_DATA.findIndex(item => item.id === id);
          if (index !== -1) {
            MOCK_DATA.splice(index, 1);
          }
          resolve();
        }, 500);
      });
    }
  }
};

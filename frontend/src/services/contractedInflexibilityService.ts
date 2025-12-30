import { ContractedInflexibility } from '../types/contractedInflexibility';

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
  getAll: async (): Promise<ContractedInflexibility[]> => {
    return new Promise((resolve) => {
      setTimeout(() => resolve([...MOCK_DATA]), 500);
    });
  },

  create: async (data: Omit<ContractedInflexibility, 'id'>): Promise<ContractedInflexibility> => {
    return new Promise((resolve) => {
      setTimeout(() => {
        const newItem = { ...data, id: Math.random().toString(36).substr(2, 9) };
        MOCK_DATA.push(newItem);
        resolve(newItem);
      }, 500);
    });
  },

  update: async (id: string, data: Partial<ContractedInflexibility>): Promise<ContractedInflexibility> => {
    return new Promise((resolve, reject) => {
      setTimeout(() => {
        const index = MOCK_DATA.findIndex(item => item.id === id);
        if (index !== -1) {
          MOCK_DATA[index] = { ...MOCK_DATA[index], ...data };
          resolve(MOCK_DATA[index]);
        } else {
          reject(new Error('Item not found'));
        }
      }, 500);
    });
  },

  delete: async (id: string): Promise<void> => {
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
};

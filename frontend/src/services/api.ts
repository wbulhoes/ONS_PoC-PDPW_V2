import axios from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5001/api';

export interface DadoEnergeticoDto {
  id: number;
  dataReferencia: string;
  codigoUsina: string;
  producaoMWh: number;
  capacidadeDisponivel: number;
  status: string;
  observacoes?: string;
}

export interface CriarDadoEnergeticoDto {
  dataReferencia: string;
  codigoUsina: string;
  producaoMWh: number;
  capacidadeDisponivel: number;
  status: string;
  observacoes?: string;
}

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const dadosEnergeticosApi = {
  obterTodos: async (): Promise<DadoEnergeticoDto[]> => {
    const response = await api.get('/dadosenergeticos');
    return response.data;
  },

  obterPorId: async (id: number): Promise<DadoEnergeticoDto> => {
    const response = await api.get(`/dadosenergeticos/${id}`);
    return response.data;
  },

  criar: async (dto: CriarDadoEnergeticoDto): Promise<DadoEnergeticoDto> => {
    const response = await api.post('/dadosenergeticos', dto);
    return response.data;
  },

  atualizar: async (id: number, dto: CriarDadoEnergeticoDto): Promise<void> => {
    await api.put(`/dadosenergeticos/${id}`, dto);
  },

  remover: async (id: number): Promise<void> => {
    await api.delete(`/dadosenergeticos/${id}`);
  },

  obterPorPeriodo: async (dataInicio: string, dataFim: string): Promise<DadoEnergeticoDto[]> => {
    const response = await api.get(`/dadosenergeticos/periodo`, {
      params: { dataInicio, dataFim }
    });
    return response.data;
  }
};

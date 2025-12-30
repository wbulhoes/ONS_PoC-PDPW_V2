import axios from 'axios';

export interface EnergiaVertida {
  id: number;
  dataReferencia: string;
  codigoUsina: string;
  energiaVertida: number;
  motivoVertimento?: string;
  observacoes?: string;
}

export interface CreateEnergiaVertidaDto {
  dataReferencia: string;
  codigoUsina: string;
  energiaVertida: number;
  motivoVertimento?: string;
  observacoes?: string;
}

export interface UpdateEnergiaVertidaDto {
  dataReferencia?: string;
  codigoUsina?: string;
  energiaVertida?: number;
  motivoVertimento?: string;
  observacoes?: string;
}

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5001/api',
});

export const energiaVertidaService = {
  async getAll(): Promise<EnergiaVertida[]> {
    const { data } = await api.get<EnergiaVertida[]>('/energia-vertida');
    return data;
  },
  async getById(id: number): Promise<EnergiaVertida> {
    const { data } = await api.get<EnergiaVertida>(`/energia-vertida/${id}`);
    return data;
  },
  async create(dto: CreateEnergiaVertidaDto): Promise<EnergiaVertida> {
    const { data } = await api.post<EnergiaVertida>('/energia-vertida', dto);
    return data;
  },
  async update(id: number, dto: UpdateEnergiaVertidaDto): Promise<EnergiaVertida> {
    const { data } = await api.put<EnergiaVertida>(`/energia-vertida/${id}`, dto);
    return data;
  },
  async delete(id: number): Promise<void> {
    await api.delete(`/energia-vertida/${id}`);
  },
};

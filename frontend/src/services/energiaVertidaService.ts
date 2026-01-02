import { apiClient } from './apiClient';

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

export const energiaVertidaService = {
  async getAll(): Promise<EnergiaVertida[]> {
    return apiClient.get<EnergiaVertida[]>('/energia-vertida');
  },
  
  async getById(id: number): Promise<EnergiaVertida> {
    return apiClient.get<EnergiaVertida>(`/energia-vertida/${id}`);
  },
  
  async create(dto: CreateEnergiaVertidaDto): Promise<EnergiaVertida> {
    return apiClient.post<EnergiaVertida>('/energia-vertida', dto);
  },
  
  async update(id: number, dto: UpdateEnergiaVertidaDto): Promise<EnergiaVertida> {
    return apiClient.put<EnergiaVertida>(`/energia-vertida/${id}`, dto);
  },
  
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/energia-vertida/${id}`);
  },
};

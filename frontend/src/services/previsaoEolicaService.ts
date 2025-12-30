import { apiClient } from './apiClient';

export interface PrevisaoEolica {
  id: number;
  usinaId: number;
  dataHoraReferencia: string; // ISO 8601
  dataHoraPrevista: string;   // ISO 8601
  geracaoPrevistaMWmed: number;
  velocidadeVentoMS: number;
  direcaoVentoGraus: number;
  modeloPrevisao: string;
  tipoPrevisao: string;
}

export interface CreatePrevisaoEolicaDto {
  usinaId: number;
  dataHoraReferencia: string;
  dataHoraPrevista: string;
  geracaoPrevistaMWmed: number;
  velocidadeVentoMS: number;
  direcaoVentoGraus: number;
  modeloPrevisao: string;
  tipoPrevisao: string;
}

export interface UpdatePrevisaoEolicaDto {
  dataHoraReferencia?: string;
  dataHoraPrevista?: string;
  geracaoPrevistaMWmed?: number;
  velocidadeVentoMS?: number;
  direcaoVentoGraus?: number;
  modeloPrevisao?: string;
  tipoPrevisao?: string;
}

export const previsaoEolicaService = {
  async getAll(): Promise<PrevisaoEolica[]> {
    return apiClient.get<PrevisaoEolica[]>('/previsoes-eolicas');
  },

  async getById(id: number): Promise<PrevisaoEolica> {
    return apiClient.get<PrevisaoEolica>(`/previsoes-eolicas/${id}`);
  },

  async create(dto: CreatePrevisaoEolicaDto): Promise<PrevisaoEolica> {
    return apiClient.post<PrevisaoEolica>('/previsoes-eolicas', dto);
  },

  async update(id: number, dto: UpdatePrevisaoEolicaDto): Promise<PrevisaoEolica> {
    return apiClient.put<PrevisaoEolica>(`/previsoes-eolicas/${id}`, dto);
  },

  async delete(id: number): Promise<void> {
    return apiClient.delete(`/previsoes-eolicas/${id}`);
  },
};

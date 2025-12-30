import { apiClient } from './apiClient';

export interface Carga {
  id: number;
  subsistemaId: string;
  subsistemaNome?: string;
  dataReferencia: string;
  intervalo: number;
  cargaMW: number;
  tipoCarga: 'PREVISTA' | 'REALIZADA' | 'ESTIMADA';
  observacao?: string;
}

export interface CreateCargaDto {
  subsistemaId: string;
  dataReferencia: string;
  intervalo: number;
  cargaMW: number;
  tipoCarga: 'PREVISTA' | 'REALIZADA' | 'ESTIMADA';
  observacao?: string;
}

export interface UpdateCargaDto {
  cargaMW?: number;
  observacao?: string;
}

export const cargaService = {
  /**
   * Obtém todas as cargas
   */
  async getAll(): Promise<Carga[]> {
    return apiClient.get<Carga[]>('/cargas');
  },

  /**
   * Obtém uma carga por ID
   */
  async getById(id: number): Promise<Carga> {
    return apiClient.get<Carga>(`/cargas/${id}`);
  },

  /**
   * Obtém cargas por período
   */
  async getByPeriod(dataInicio: string, dataFim: string): Promise<Carga[]> {
    return apiClient.get<Carga[]>(
      `/cargas/periodo?dataInicio=${dataInicio}&dataFim=${dataFim}`
    );
  },

  /**
   * Obtém cargas por subsistema
   */
  async getBySubsistema(subsistemaId: string): Promise<Carga[]> {
    return apiClient.get<Carga[]>(`/cargas/subsistema/${subsistemaId}`);
  },

  /**
   * Obtém cargas por data de referência
   */
  async getByDataReferencia(dataReferencia: string): Promise<Carga[]> {
    return apiClient.get<Carga[]>(`/cargas/data/${dataReferencia}`);
  },

  /**
   * Cria uma nova carga
   */
  async create(data: CreateCargaDto): Promise<Carga> {
    return apiClient.post<Carga>('/cargas', data);
  },

  /**
   * Atualiza uma carga existente
   */
  async update(id: number, data: UpdateCargaDto): Promise<void> {
    return apiClient.put<void>(`/cargas/${id}`, data);
  },

  /**
   * Remove uma carga
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/cargas/${id}`);
  },

  /**
   * Cria ou atualiza múltiplas cargas (bulk)
   */
  async bulkUpsert(cargas: CreateCargaDto[]): Promise<Carga[]> {
    return apiClient.post<Carga[]>('/cargas/bulk', cargas);
  },
};

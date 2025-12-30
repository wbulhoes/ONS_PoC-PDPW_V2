import { apiClient } from './apiClient';

export interface OfertaExportacao {
  id: number;
  usinaId: number;
  usinaNome?: string;
  dataOferta: string;
  dataPDP: string;
  valorMW: number;
  precoMWh: number;
  horaInicial: string; // formato HH:mm
  horaFinal: string;   // formato HH:mm
  observacoes?: string;
  semanaPMOId?: number;
  status?: string;
}

export interface CreateOfertaExportacaoDto {
  usinaId: number;
  dataOferta: string;
  dataPDP: string;
  valorMW: number;
  precoMWh: number;
  horaInicial: string;
  horaFinal: string;
  observacoes?: string;
  semanaPMOId?: number;
}

export interface UpdateOfertaExportacaoDto {
  valorMW?: number;
  precoMWh?: number;
  horaInicial?: string;
  horaFinal?: string;
  observacoes?: string;
  semanaPMOId?: number;
}

export const ofertaExportacaoService = {
  /**
   * Obtém todas as ofertas de exportação
   */
  async getAll(): Promise<OfertaExportacao[]> {
    return apiClient.get<OfertaExportacao[]>('/ofertas-exportacao');
  },

  /**
   * Obtém uma oferta por ID
   */
  async getById(id: number): Promise<OfertaExportacao> {
    return apiClient.get<OfertaExportacao>(`/ofertas-exportacao/${id}`);
  },

  /**
   * Cria uma nova oferta de exportação
   */
  async create(data: CreateOfertaExportacaoDto): Promise<OfertaExportacao> {
    return apiClient.post<OfertaExportacao>('/ofertas-exportacao', data);
  },

  /**
   * Atualiza uma oferta de exportação
   */
  async update(id: number, data: UpdateOfertaExportacaoDto): Promise<OfertaExportacao> {
    return apiClient.put<OfertaExportacao>(`/ofertas-exportacao/${id}`, data);
  },

  /**
   * Remove uma oferta de exportação
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/ofertas-exportacao/${id}`);
  },

  // Métodos auxiliares para status (se implementados no backend)
  async getByStatus(status: string): Promise<OfertaExportacao[]> {
    return apiClient.get<OfertaExportacao[]>(`/ofertas-exportacao/status/${status}`);
  },
};

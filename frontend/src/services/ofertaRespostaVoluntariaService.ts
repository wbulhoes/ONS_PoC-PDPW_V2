import { apiClient } from './apiClient';

export interface OfertaRespostaVoluntaria {
  id: number;
  empresaId: number;
  empresaNome?: string;
  dataPDP: string;
  tipoPrograma: string;
  reducaoCargaMW: number;
  horaInicio: string;
  horaFim: string;
  statusONS: 'PENDENTE' | 'APROVADA' | 'REJEITADA';
  dataEnvio: string;
  dataAnalise?: string;
  observacaoEmpresa?: string;
  observacaoONS?: string;
  precoOfertadoRS?: number;
}

export interface CreateOfertaRespostaVoluntariaDto {
  empresaId: number;
  dataPDP: string;
  tipoPrograma: string;
  reducaoCargaMW: number;
  horaInicio: string;
  horaFim: string;
  observacaoEmpresa?: string;
  precoOfertadoRS?: number;
}

export interface UpdateOfertaRespostaVoluntariaDto {
  reducaoCargaMW?: number;
  horaInicio?: string;
  horaFim?: string;
  observacaoEmpresa?: string;
  precoOfertadoRS?: number;
}

export const ofertaRespostaVoluntariaService = {
  /**
   * Obtém todas as ofertas de resposta voluntária
   */
  async getAll(): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>('/ofertas-resposta-voluntaria');
  },

  /**
   * Obtém uma oferta por ID
   */
  async getById(id: number): Promise<OfertaRespostaVoluntaria> {
    return apiClient.get<OfertaRespostaVoluntaria>(`/ofertas-resposta-voluntaria/${id}`);
  },

  /**
   * Obtém ofertas pendentes
   */
  async getPendentes(): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>('/ofertas-resposta-voluntaria/pendentes');
  },

  /**
   * Obtém ofertas aprovadas
   */
  async getAprovadas(): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>('/ofertas-resposta-voluntaria/aprovadas');
  },

  /**
   * Obtém ofertas rejeitadas
   */
  async getRejeitadas(): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>('/ofertas-resposta-voluntaria/rejeitadas');
  },

  /**
   * Obtém ofertas por empresa
   */
  async getByEmpresa(empresaId: number): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>(
      `/ofertas-resposta-voluntaria/empresa/${empresaId}`
    );
  },

  /**
   * Obtém ofertas por data PDP
   */
  async getByDataPDP(dataPDP: string): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>(
      `/ofertas-resposta-voluntaria/data-pdp/${dataPDP}`
    );
  },

  /**
   * Obtém ofertas por tipo de programa
   */
  async getByTipoPrograma(tipoPrograma: string): Promise<OfertaRespostaVoluntaria[]> {
    return apiClient.get<OfertaRespostaVoluntaria[]>(
      `/ofertas-resposta-voluntaria/tipo-programa/${tipoPrograma}`
    );
  },

  /**
   * Cria uma nova oferta de resposta voluntária
   */
  async create(data: CreateOfertaRespostaVoluntariaDto): Promise<OfertaRespostaVoluntaria> {
    return apiClient.post<OfertaRespostaVoluntaria>('/ofertas-resposta-voluntaria', data);
  },

  /**
   * Atualiza uma oferta
   */
  async update(id: number, data: UpdateOfertaRespostaVoluntariaDto): Promise<void> {
    return apiClient.put<void>(`/ofertas-resposta-voluntaria/${id}`, data);
  },

  /**
   * Remove uma oferta
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/ofertas-resposta-voluntaria/${id}`);
  },

  /**
   * Aprova uma oferta (ONS)
   */
  async aprovar(id: number, observacao?: string): Promise<void> {
    return apiClient.post<void>(`/ofertas-resposta-voluntaria/${id}/aprovar`, { observacao });
  },

  /**
   * Rejeita uma oferta (ONS)
   */
  async rejeitar(id: number, observacao: string): Promise<void> {
    return apiClient.post<void>(`/ofertas-resposta-voluntaria/${id}/rejeitar`, { observacao });
  },

  /**
   * Envia oferta para análise do ONS
   */
  async enviar(id: number): Promise<void> {
    return apiClient.post<void>(`/ofertas-resposta-voluntaria/${id}/enviar`, {});
  },
};

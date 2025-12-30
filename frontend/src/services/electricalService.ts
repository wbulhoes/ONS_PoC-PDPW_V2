import { apiClient } from './apiClient';
import {
  transformElectricalFromApi,
  transformElectricalListFromApi,
  transformElectricalToApi,
} from '../utils/dtoTransformers';

export interface DadoEletrico {
  id: number;
  usinaId: number;
  usinaNome?: string;
  dataReferencia: string;
  intervalo: number;
  potenciaMW: number;
  razaoEletrica: number;
  fatorPotencia?: number;
  observacao?: string;
}

export interface CreateDadoEletricoDto {
  usinaId: number;
  dataReferencia: string;
  intervalo: number;
  potenciaMW: number;
  razaoEletrica: number;
  fatorPotencia?: number;
  observacao?: string;
}

export interface UpdateDadoEletricoDto {
  potenciaMW?: number;
  razaoEletrica?: number;
  fatorPotencia?: number;
  observacao?: string;
}

export const electricalService = {
  /**
   * Obtém todos os dados elétricos
   */
  async getAll(): Promise<DadoEletrico[]> {
    const res = await apiClient.get<any[]>('/dados-eletricos');
    return transformElectricalListFromApi(res) as DadoEletrico[];
  },

  /**
   * Obtém um dado elétrico por ID
   */
  async getById(id: number): Promise<DadoEletrico> {
    const res = await apiClient.get<any>(`/dados-eletricos/${id}`);
    return transformElectricalFromApi(res) as DadoEletrico;
  },

  /**
   * Obtém dados elétricos por período
   */
  async getByPeriod(dataInicio: string, dataFim: string): Promise<DadoEletrico[]> {
    const res = await apiClient.get<any[]>(
      `/dados-eletricos/periodo?dataInicio=${dataInicio}&dataFim=${dataFim}`
    );
    return transformElectricalListFromApi(res) as DadoEletrico[];
  },

  /**
   * Obtém dados elétricos por usina e data
   */
  async getByUsinaAndDate(usinaId: number, dataReferencia: string): Promise<DadoEletrico[]> {
    const res = await apiClient.get<any[]>(
      `/dados-eletricos/usina/${usinaId}/data/${dataReferencia}`
    );
    return transformElectricalListFromApi(res) as DadoEletrico[];
  },

  /**
   * Cria um novo dado elétrico
   */
  async create(data: CreateDadoEletricoDto): Promise<DadoEletrico> {
    const apiData = transformElectricalToApi<CreateDadoEletricoDto>(data) as any;
    const res = await apiClient.post<any>('/dados-eletricos', apiData);
    return transformElectricalFromApi(res) as DadoEletrico;
  },

  /**
   * Atualiza um dado elétrico existente
   */
  async update(id: number, data: UpdateDadoEletricoDto): Promise<void> {
    const apiData = transformElectricalToApi<UpdateDadoEletricoDto>(data) as any;
    return apiClient.put<void>(`/dados-eletricos/${id}`, apiData);
  },

  /**
   * Remove um dado elétrico
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/dados-eletricos/${id}`);
  },

  /**
   * Cria ou atualiza múltiplos dados elétricos (bulk)
   */
  async bulkUpsert(dados: CreateDadoEletricoDto[]): Promise<DadoEletrico[]> {
    const apiData = transformElectricalToApi<CreateDadoEletricoDto[]>(dados) as any[];
    const res = await apiClient.post<any[]>('/dados-eletricos/bulk', apiData);
    return transformElectricalListFromApi(res) as DadoEletrico[];
  },
};

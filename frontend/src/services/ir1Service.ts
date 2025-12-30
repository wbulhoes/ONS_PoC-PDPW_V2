/**
 * IR1 Service (Nível de Partida)
 * T053: Create IR1 service with CRUD functions
 * API endpoint: /api/insumos-recebimento/ir1
 */

import { apiClient } from './apiClient';
import { IR1Data, IR1Dto } from '../types/ir1';
import {
  transformIR1FromApi,
  transformIR1ListFromApi,
  transformIR1ToApi,
} from '../utils/dtoTransformers';

/**
 * IR1 Service - manages water level data (Nível de Partida) for hydroelectric plants
 */
export const ir1Service = {
  /**
   * Obtém dados de nível de partida por data
   * @param dataReferencia - Data em formato ISO (YYYY-MM-DD) ou YYYYMMDD
   * @returns Dados de nível de partida com lista de usinas
   */
  async getByDate(dataReferencia: string): Promise<IR1Data> {
    // Normalizar data se estiver em formato YYYYMMDD para YYYY-MM-DD
    const dateFormatted =
      dataReferencia.length === 8
        ? `${dataReferencia.slice(0, 4)}-${dataReferencia.slice(4, 6)}-${dataReferencia.slice(6, 8)}`
        : dataReferencia;

    const res = await apiClient.get<any>(`/insumos-recebimento/ir1/${dateFormatted}`);
    return transformIR1FromApi(res) as IR1Data;
  },

  /**
   * Obtém todos os dados de nível de partida (sem filtro)
   * @returns Lista de todos os registros de IR1
   */
  async getAll(): Promise<IR1Data[]> {
    const res = await apiClient.get<any[]>('/insumos-recebimento/ir1');
    return transformIR1ListFromApi(res) as IR1Data[];
  },

  /**
   * Cria novo registro de nível de partida
   * @param dto - Dados de nível de partida a ser criado
   * @returns Dados criados com ID gerado pelo backend
   */
  async create(dto: IR1Dto): Promise<IR1Data> {
    const apiData = transformIR1ToApi<IR1Dto>(dto) as any;
    const res = await apiClient.post<any>('/insumos-recebimento/ir1', apiData);
    return transformIR1FromApi(res) as IR1Data;
  },

  /**
   * Atualiza registro existente de nível de partida
   * @param id - ID do registro a atualizar
   * @param dto - Dados atualizados
   * @returns Dados atualizados
   */
  async update(id: number, dto: Partial<IR1Dto>): Promise<IR1Data> {
    const apiData = transformIR1ToApi<Partial<IR1Dto>>(dto) as any;
    const res = await apiClient.put<any>(`/insumos-recebimento/ir1/${id}`, apiData);
    return transformIR1FromApi(res) as IR1Data;
  },

  /**
   * Remove registro de nível de partida
   * @param id - ID do registro a remover
   */
  async delete(id: number): Promise<void> {
    return apiClient.delete(`/insumos-recebimento/ir1/${id}`);
  },

  /**
   * Cria ou atualiza múltiplos registros de nível de partida em uma única operação
   * @param dados - Lista de dados IR1 a fazer bulk upsert
   * @returns Lista de dados criados/atualizados
   */
  async bulkUpsert(dados: IR1Dto[]): Promise<IR1Data[]> {
    const apiData = transformIR1ToApi<IR1Dto[]>(dados) as any[];
    const res = await apiClient.post<any[]>('/insumos-recebimento/ir1/bulk', apiData);
    return transformIR1ListFromApi(res) as IR1Data[];
  },
};

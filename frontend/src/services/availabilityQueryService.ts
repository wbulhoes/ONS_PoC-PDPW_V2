/**
 * Service para Consulta de Disponibilidade
 * Consome endpoints da API backend
 */

import {
  AvailabilityQueryFilters,
  AvailabilityQueryResponse,
  DisponibilidadeData,
  DisponibilidadeAggregated,
  EmpresaInfo,
  UsinaInfo,
  AVAILABILITY_QUERY_CONSTANTS,
} from '../types/availabilityQuery';

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5001/api';
const API_TIMEOUT = import.meta.env.VITE_API_TIMEOUT || 30000;

/**
 * Faz requisição HTTP com timeout
 */
async function fetchWithTimeout(url: string, options: RequestInit = {}): Promise<Response> {
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), API_TIMEOUT);

  try {
    const response = await fetch(url, {
      ...options,
      signal: controller.signal,
    });

    if (!response.ok) {
      throw new Error(`API Error: ${response.status} ${response.statusText}`);
    }

    return response;
  } finally {
    clearTimeout(timeoutId);
  }
}

/**
 * Serviço para consultas de disponibilidade
 */
export const AvailabilityQueryService = {
  /**
   * Consulta dados de disponibilidade com filtros
   */
  async query(
    filters: AvailabilityQueryFilters,
    pagina: number = 1,
    itensPorPagina: number = AVAILABILITY_QUERY_CONSTANTS.DEFAULT_PAGE_SIZE
  ): Promise<AvailabilityQueryResponse> {
    const params = new URLSearchParams();

    if (filters.dataPDPInicio) params.append('dataPDPInicio', filters.dataPDPInicio);
    if (filters.dataPDPFim) params.append('dataPDPFim', filters.dataPDPFim);
    if (filters.codEmpresa) params.append('codEmpresa', filters.codEmpresa);
    if (filters.codUsina) params.append('codUsina', filters.codUsina);
    if (filters.tipoUsina) params.append('tipoUsina', filters.tipoUsina);
    if (filters.statusDisponibilidade) params.append('statusDisponibilidade', filters.statusDisponibilidade);
    if (filters.intervaloInicio) params.append('intervaloInicio', filters.intervaloInicio.toString());
    if (filters.intervaloFim) params.append('intervaloFim', filters.intervaloFim.toString());

    params.append('pagina', pagina.toString());
    params.append('itensPorPagina', itensPorPagina.toString());

    const response = await fetchWithTimeout(`${API_BASE_URL}/disponibilidade/consulta?${params.toString()}`);
    const data = await response.json();

    return data as AvailabilityQueryResponse;
  },

  /**
   * Obtém empresas disponíveis
   */
  async getEmpresas(): Promise<EmpresaInfo[]> {
    const response = await fetchWithTimeout(`${API_BASE_URL}/empresas`);
    const data = await response.json();
    return data as EmpresaInfo[];
  },

  /**
   * Obtém usinas por empresa
   */
  async getUsinasByEmpresa(empresaId: number): Promise<UsinaInfo[]> {
    const response = await fetchWithTimeout(`${API_BASE_URL}/usinas/empresa/${empresaId}`);
    const data = await response.json();
    return data as UsinaInfo[];
  },

  /**
   * Obtém dados agregados por usina
   */
  async getAggregatedData(filters: AvailabilityQueryFilters): Promise<DisponibilidadeAggregated[]> {
    const params = new URLSearchParams();

    if (filters.dataPDPInicio) params.append('dataPDPInicio', filters.dataPDPInicio);
    if (filters.dataPDPFim) params.append('dataPDPFim', filters.dataPDPFim);
    if (filters.codEmpresa) params.append('codEmpresa', filters.codEmpresa);
    if (filters.codUsina) params.append('codUsina', filters.codUsina);
    if (filters.tipoUsina) params.append('tipoUsina', filters.tipoUsina);

    const response = await fetchWithTimeout(
      `${API_BASE_URL}/disponibilidade/agregado?${params.toString()}`
    );
    const data = await response.json();
    return data as DisponibilidadeAggregated[];
  },

  /**
   * Exporta dados em formato específico
   */
  async exportData(
    filters: AvailabilityQueryFilters,
    formato: 'excel' | 'csv' | 'pdf' = 'excel'
  ): Promise<Blob> {
    const params = new URLSearchParams();

    if (filters.dataPDPInicio) params.append('dataPDPInicio', filters.dataPDPInicio);
    if (filters.dataPDPFim) params.append('dataPDPFim', filters.dataPDPFim);
    if (filters.codEmpresa) params.append('codEmpresa', filters.codEmpresa);
    if (filters.codUsina) params.append('codUsina', filters.codUsina);
    if (filters.tipoUsina) params.append('tipoUsina', filters.tipoUsina);
    params.append('formato', formato);

    const response = await fetchWithTimeout(
      `${API_BASE_URL}/disponibilidade/exportar?${params.toString()}`
    );

    return response.blob();
  },

  /**
   * Calcula estatísticas dos dados
   */
  async getStatistics(filters: AvailabilityQueryFilters): Promise<any> {
    const params = new URLSearchParams();

    if (filters.dataPDPInicio) params.append('dataPDPInicio', filters.dataPDPInicio);
    if (filters.dataPDPFim) params.append('dataPDPFim', filters.dataPDPFim);
    if (filters.codEmpresa) params.append('codEmpresa', filters.codEmpresa);
    if (filters.tipoUsina) params.append('tipoUsina', filters.tipoUsina);

    const response = await fetchWithTimeout(
      `${API_BASE_URL}/disponibilidade/estatisticas?${params.toString()}`
    );
    const data = await response.json();
    return data;
  },
};

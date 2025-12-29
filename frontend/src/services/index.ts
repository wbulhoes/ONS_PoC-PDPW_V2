import apiClient from './apiClient';
import {
  DadoEnergeticoDto,
  CriarDadoEnergeticoDto,
  CargaDto,
  IntercambioDto,
  BalancoDto,
  PrevisaoEolicaDto,
  ArquivoDadgerDto,
  OfertaExportacaoDto,
  OfertaRespostaVoluntariaDto,
  EnergiaVertidaDto,
  UsinaDto,
  SemanaPmoDto,
  UsuarioDto,
  DashboardResumoDto,
} from '../types';

// ========== 1. DADOS ENERGÉTICOS ==========
export const dadosEnergeticosService = {
  obterTodos: () => apiClient.get<DadoEnergeticoDto[]>('/dadosenergeticos'),
  obterPorId: (id: number) => apiClient.get<DadoEnergeticoDto>(`/dadosenergeticos/${id}`),
  criar: (dto: CriarDadoEnergeticoDto) => apiClient.post<DadoEnergeticoDto>('/dadosenergeticos', dto),
  atualizar: (id: number, dto: CriarDadoEnergeticoDto) => apiClient.put(`/dadosenergeticos/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/dadosenergeticos/${id}`),
  obterPorPeriodo: (dataInicio: string, dataFim: string) =>
    apiClient.get<DadoEnergeticoDto[]>('/dadosenergeticos/periodo', { dataInicio, dataFim }),
};

// ========== 2. CARGAS ==========
export const cargasService = {
  obterTodas: () => apiClient.get<CargaDto[]>('/cargas'),
  obterPorId: (id: number) => apiClient.get<CargaDto>(`/cargas/${id}`),
  criar: (dto: Omit<CargaDto, 'id'>) => apiClient.post<CargaDto>('/cargas', dto),
  atualizar: (id: number, dto: Omit<CargaDto, 'id'>) => apiClient.put(`/cargas/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/cargas/${id}`),
  obterPorSemana: (semanaPmoId: number) => apiClient.get<CargaDto[]>(`/cargas/semana/${semanaPmoId}`),
  obterPorSubsistema: (subsistema: string) => apiClient.get<CargaDto[]>(`/cargas/subsistema/${subsistema}`),
};

// ========== 3. INTERCÂMBIOS ==========
export const intercambiosService = {
  obterTodos: () => apiClient.get<IntercambioDto[]>('/intercambios'),
  obterPorId: (id: number) => apiClient.get<IntercambioDto>(`/intercambios/${id}`),
  criar: (dto: Omit<IntercambioDto, 'id'>) => apiClient.post<IntercambioDto>('/intercambios', dto),
  atualizar: (id: number, dto: Omit<IntercambioDto, 'id'>) => apiClient.put(`/intercambios/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/intercambios/${id}`),
  obterPorSubsistemas: (origem: string, destino: string) =>
    apiClient.get<IntercambioDto[]>('/intercambios/subsistema', { origem, destino }),
};

// ========== 4. BALANÇOS ==========
export const balancosService = {
  obterTodos: () => apiClient.get<BalancoDto[]>('/balancos'),
  obterPorId: (id: number) => apiClient.get<BalancoDto>(`/balancos/${id}`),
  criar: (dto: Omit<BalancoDto, 'id'>) => apiClient.post<BalancoDto>('/balancos', dto),
  atualizar: (id: number, dto: Omit<BalancoDto, 'id'>) => apiClient.put(`/balancos/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/balancos/${id}`),
  obterPorSubsistema: (subsistema: string) => apiClient.get<BalancoDto[]>(`/balancos/subsistema/${subsistema}`),
};

// ========== 5. PREVISÕES EÓLICAS ==========
export const previsoesEolicasService = {
  obterTodas: () => apiClient.get<PrevisaoEolicaDto[]>('/previsoeseolicas'),
  obterPorId: (id: number) => apiClient.get<PrevisaoEolicaDto>(`/previsoeseolicas/${id}`),
  criar: (dto: Omit<PrevisaoEolicaDto, 'id'>) => apiClient.post<PrevisaoEolicaDto>('/previsoeseolicas', dto),
  atualizar: (id: number, dto: Omit<PrevisaoEolicaDto, 'id'>) => apiClient.put(`/previsoeseolicas/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/previsoeseolicas/${id}`),
  atualizarPrevisao: (id: number, potenciaPrevistoMW: number) =>
    apiClient.patch(`/previsoeseolicas/${id}/previsao`, { potenciaPrevistoMW }),
};

// ========== 6. ARQUIVOS DADGER ==========
export const arquivosDadgerService = {
  obterTodos: () => apiClient.get<ArquivoDadgerDto[]>('/arquivosdadger'),
  obterPorId: (id: number) => apiClient.get<ArquivoDadgerDto>(`/arquivosdadger/${id}`),
  criar: (dto: Omit<ArquivoDadgerDto, 'id'>) => apiClient.post<ArquivoDadgerDto>('/arquivosdadger', dto),
  atualizar: (id: number, dto: Omit<ArquivoDadgerDto, 'id'>) => apiClient.put(`/arquivosdadger/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/arquivosdadger/${id}`),
  obterPorSemana: (semanaPmoId: number) => apiClient.get<ArquivoDadgerDto[]>(`/arquivosdadger/semana/${semanaPmoId}`),
  gerar: (semanaPmoId: number) => apiClient.post(`/arquivosdadger/gerar/${semanaPmoId}`),
  aprovar: (id: number) => apiClient.patch(`/arquivosdadger/${id}/aprovar`),
  rejeitar: (id: number) => apiClient.patch(`/arquivosdadger/${id}/rejeitar`),
  download: (id: number) => apiClient.get<Blob>(`/arquivosdadger/${id}/download`),
};

// ========== 7. OFERTAS DE EXPORTAÇÃO ==========
export const ofertasExportacaoService = {
  obterTodas: () => apiClient.get<OfertaExportacaoDto[]>('/ofertas-exportacao'),
  obterPorId: (id: number) => apiClient.get<OfertaExportacaoDto>(`/ofertas-exportacao/${id}`),
  criar: (dto: Omit<OfertaExportacaoDto, 'id' | 'status'>) => apiClient.post<OfertaExportacaoDto>('/ofertas-exportacao', dto),
  atualizar: (id: number, dto: Omit<OfertaExportacaoDto, 'id' | 'status'>) => apiClient.put(`/ofertas-exportacao/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/ofertas-exportacao/${id}`),
  obterPorUsina: (usinaId: number) => apiClient.get<OfertaExportacaoDto[]>(`/ofertas-exportacao/usina/${usinaId}`),
  obterPorStatus: (status: string) => apiClient.get<OfertaExportacaoDto[]>(`/ofertas-exportacao/${status.toLowerCase()}`),
  aprovar: (id: number) => apiClient.post(`/ofertas-exportacao/${id}/aprovar`, { usuarioONS: 'Sistema' }),
  rejeitar: (id: number) => apiClient.post(`/ofertas-exportacao/${id}/rejeitar`, { usuarioONS: 'Sistema', observacao: 'Rejeitado' }),
};

// ========== 8. OFERTAS DE RESPOSTA VOLUNTÁRIA ==========
export const ofertasRespostaVoluntariaService = {
  obterTodas: () => apiClient.get<OfertaRespostaVoluntariaDto[]>('/ofertas-resposta-voluntaria'),
  obterPorId: (id: number) => apiClient.get<OfertaRespostaVoluntariaDto>(`/ofertas-resposta-voluntaria/${id}`),
  criar: (dto: Omit<OfertaRespostaVoluntariaDto, 'id' | 'status'>) =>
    apiClient.post<OfertaRespostaVoluntariaDto>('/ofertas-resposta-voluntaria', dto),
  atualizar: (id: number, dto: Omit<OfertaRespostaVoluntariaDto, 'id' | 'status'>) =>
    apiClient.put(`/ofertas-resposta-voluntaria/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/ofertas-resposta-voluntaria/${id}`),
  obterPorStatus: (status: string) =>
    apiClient.get<OfertaRespostaVoluntariaDto[]>(`/ofertas-resposta-voluntaria/${status.toLowerCase()}`),
  aprovar: (id: number) => apiClient.post(`/ofertas-resposta-voluntaria/${id}/aprovar`, { usuarioONS: 'Sistema' }),
  rejeitar: (id: number) =>
    apiClient.post(`/ofertas-resposta-voluntaria/${id}/rejeitar`, { usuarioONS: 'Sistema', observacao: 'Rejeitado' }),
};

// ========== 9. ENERGIA VERTIDA ==========
export const energiaVertidaService = {
  obterTodas: () => apiClient.get<EnergiaVertidaDto[]>('/energiavertida'),
  obterPorId: (id: number) => apiClient.get<EnergiaVertidaDto>(`/energiavertida/${id}`),
  criar: (dto: Omit<EnergiaVertidaDto, 'id'>) => apiClient.post<EnergiaVertidaDto>('/energiavertida', dto),
  atualizar: (id: number, dto: Omit<EnergiaVertidaDto, 'id'>) => apiClient.put(`/energiavertida/${id}`, dto),
  remover: (id: number) => apiClient.delete(`/energiavertida/${id}`),
};

// ========== ENTIDADES AUXILIARES ==========
export const usinasService = {
  obterTodas: () => apiClient.get<UsinaDto[]>('/usinas'),
  obterPorId: (id: number) => apiClient.get<UsinaDto>(`/usinas/${id}`),
  obterPorTipo: (tipoUsinaId: number) => apiClient.get<UsinaDto[]>(`/usinas/tipo/${tipoUsinaId}`),
  buscar: (termo: string) => apiClient.get<UsinaDto[]>('/usinas/buscar', { termo }),
};

export const semanasPmoService = {
  obterTodas: () => apiClient.get<SemanaPmoDto[]>('/semanaspmo'),
  obterAtual: () => apiClient.get<SemanaPmoDto>('/semanaspmo/atual'),
  obterProximas: (quantidade: number = 4) => apiClient.get<SemanaPmoDto[]>('/semanaspmo/proximas', { quantidade }),
};

export const usuariosService = {
  obterTodos: () => apiClient.get<UsuarioDto[]>('/usuarios'),
  obterPorId: (id: number) => apiClient.get<UsuarioDto>(`/usuarios/${id}`),
};

export const dashboardService = {
  obterResumo: () => apiClient.get<DashboardResumoDto>('/dashboard/resumo'),
};

// Types centralizados do sistema PDPw

export interface DadoEnergeticoDto {
  id: number;
  dataReferencia: string;
  codigoUsina: string;
  producaoMWh: number;
  capacidadeDisponivel: number;
  status: string;
  observacoes?: string;
}

export interface CriarDadoEnergeticoDto {
  dataReferencia: string;
  codigoUsina: string;
  producaoMWh: number;
  capacidadeDisponivel: number;
  status: string;
  observacoes?: string;
}

export interface CargaDto {
  id: number;
  semanaPmoId: number;
  subsistema: string;
  dataReferencia: string;
  cargaMWMed: number;
  cargaMWMax?: number;
  observacoes?: string;
}

export interface IntercambioDto {
  id: number;
  semanaPmoId: number;
  subsistemaOrigem: string;
  subsistemaDestino: string;
  limiteMaximoMW: number;
  limiteOperativoMW: number;
  observacoes?: string;
}

export interface BalancoDto {
  id: number;
  semanaPmoId: number;
  subsistema: string;
  geracaoMWMed: number;
  cargaMWMed: number;
  intercambioLiquidoMW: number;
  observacoes?: string;
}

export interface PrevisaoEolicaDto {
  id: number;
  parqueEolicoId: number;
  semanaPmoId: number;
  dataPrevisao: string;
  potenciaPrevistoMW: number;
  fatorCapacidade: number;
  velocidadeVentoMS?: number;
  observacoes?: string;
}

export interface ArquivoDadgerDto {
  id: number;
  semanaPmoId: number;
  nomeArquivo: string;
  caminhoArquivo: string;
  versao: number;
  status: string;
  dataCriacao: string;
  criadoPorUsuarioId: number;
  observacoes?: string;
}

export interface OfertaExportacaoDto {
  id: number;
  usinaId: number;
  semanaPmoId: number;
  dataOferta: string;
  potenciaOfertadaMW: number;
  precoOfertaRS: number;
  periodoInicio: string;
  periodoFim: string;
  status: string;
  observacoes?: string;
}

export interface OfertaRespostaVoluntariaDto {
  id: number;
  consumidorId: number;
  semanaPmoId: number;
  dataOferta: string;
  reducaoDemandaMW: number;
  precoOfertaRS: number;
  periodoInicio: string;
  periodoFim: string;
  status: string;
  observacoes?: string;
}

export interface EnergiaVertidaDto {
  id: number;
  usinaId: number;
  semanaPmoId: number;
  dataReferencia: string;
  energiaVertidaMWh: number;
  motivoVertimento: string;
  observacoes?: string;
}

export interface UsinaDto {
  id: number;
  codigo: string;
  nome: string;
  tipoUsinaId: number;
  empresaId: number;
  capacidadeInstalada: number;
  subsistema: string;
  municipio?: string;
  estado?: string;
  ativo: boolean;
}

export interface SemanaPmoDto {
  id: number;
  numero: number;
  ano: number;
  dataInicio: string;
  dataFim: string;
  status: string;
}

export interface UsuarioDto {
  id: number;
  nome: string;
  email: string;
  perfil: string;
  ativo: boolean;
}

export interface NotificacaoDto {
  id: number;
  titulo: string;
  mensagem: string;
  tipo: string;
  lida: boolean;
  dataCriacao: string;
}

export interface DashboardResumoDto {
  totalUsinas: number;
  totalUnidadesGeradoras: number;
  capacidadeTotalMW: number;
  ultimaAtualizacao: string;
  programacoesEmAndamento: number;
  programacoesFinalizadas: number;
}

export interface ResultDto<T> {
  success: boolean;
  data?: T;
  errors?: string[];
  message?: string;
}

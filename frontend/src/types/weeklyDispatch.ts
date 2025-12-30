/**
 * Tipos para Oferta Semanal de Despacho Complementar
 * Migrado de: legado/pdpw/frmColOfertaSemanalDespComp.aspx
 */

/**
 * Dados de uma usina na oferta semanal
 */
export interface WeeklyDispatchUsina {
  codUsina: string;
  nomeUsina: string;
  potenciaInstalada: number;
  
  // Campos editáveis
  cvu: number; // R$/MWh
  tempoUgeLigada: number; // horas
  tempoUgeDesligada: number; // horas
  geracaoMinima: number; // MW
  rampaSubidaQuente: number; // MW/min
  rampaSubidaMorno: number; // MW/min
  rampaSubidaFrio: number; // MW/min
  rampaDescida: number; // MW/min
}

/**
 * Dados da semana PMO
 */
export interface PMOWeek {
  idAnoMes: number;
  idSemanaPmo: number;
  semana: string; // Ex: "Semana 1"
  dataInicio: string;
  dataFim: string;
  tipo: 'Consulta' | 'Edicao';
  dataLimiteEnvio?: string;
}

/**
 * Dados completos para a tela
 */
export interface WeeklyDispatchData {
  pmoConsulta: PMOWeek;
  pmoEdicao: PMOWeek;
  usinas: WeeklyDispatchUsina[];
}

/**
 * Formulário de filtro
 */
export interface WeeklyDispatchForm {
  tipoSemana: 'Consulta' | 'Edicao';
  codEmpresa: string;
}

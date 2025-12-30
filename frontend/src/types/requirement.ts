/**
 * Types para Cadastro de Requisitos (Área NE)
 * Migrado de: legado/pdpw/frmCadRequisito.aspx
 */

/**
 * Representa um requisito de área no sistema
 */
export interface RequisitoArea {
  datPdp: string; // Formato: YYYYMMDD
  codArea: string; // Ex: 'NE'
  valReqMax: number; // Requisito Máximo em MW
  hReqMax: string; // Hora do Requisito Máximo (HH:mm)
  valResReqMax: number; // Reserva Mínima no Requisito Máximo em MW
  hResReqMax: string; // Hora da Reserva (HH:mm)
}

/**
 * Opção de data PDP para dropdown
 */
export interface DataPDPOption {
  datPdp: string; // YYYYMMDD
  datPdpFormatada: string; // DD/MM/YYYY
}

/**
 * DTO para criar/atualizar requisito
 */
export interface SalvarRequisitoDTO {
  datPdp: string;
  codArea: string;
  valReqMax: number;
  hReqMax: string;
  valResReqMax: number;
  hResReqMax: string;
}

/**
 * Estado do formulário de requisito
 */
export interface RequirementFormState {
  datPdp: string;
  valReqMax: string;
  hReqMax: string;
  valResReqMax: string;
  hResReqMax: string;
  errors: Record<string, string>;
}

/**
 * Valida valor numérico (MW)
 * @param valor - Valor a ser validado
 * @param nomeCampo - Nome do campo para mensagem de erro
 * @returns true se válido, string com mensagem de erro caso contrário
 */
export function validarValorMW(valor: string, nomeCampo: string): true | string {
  const valorTrimmed = valor.trim();

  if (!valorTrimmed) {
    return `${nomeCampo} Requerido`;
  }

  const numero = parseFloat(valorTrimmed);
  if (isNaN(numero)) {
    return `${nomeCampo} deve ser um número válido`;
  }

  if (numero < 0) {
    return `${nomeCampo} não pode ser negativo`;
  }

  return true;
}

/**
 * Valida formato de horário (HH:mm)
 * @param hora - Hora a ser validada
 * @param nomeCampo - Nome do campo para mensagem de erro
 * @returns true se válido, string com mensagem de erro caso contrário
 */
export function validarHorario(hora: string, nomeCampo: string): true | string {
  const horaTrimmed = hora.trim();

  if (!horaTrimmed) {
    return `${nomeCampo} Requerida`;
  }

  // Validar formato HH:mm
  const regexHora = /^([0-1][0-9]|2[0-3]):([0-5][0-9])$/;
  if (!regexHora.test(horaTrimmed)) {
    return 'Horários Inválidos.';
  }

  return true;
}

/**
 * Valida se data PDP foi selecionada
 * @param datPdp - Data PDP selecionada
 * @returns true se válido, string com mensagem de erro caso contrário
 */
export function validarDataPDP(datPdp: string): true | string {
  if (!datPdp || datPdp === '0' || datPdp === '') {
    return 'Selecione uma Data do PDP';
  }
  return true;
}

/**
 * Valida formulário completo de requisito
 * @param form - Estado do formulário
 * @returns Objeto com validade e erros
 */
export function validarFormularioRequisito(form: RequirementFormState): {
  valido: boolean;
  erros: Record<string, string>;
} {
  const erros: Record<string, string> = {};

  // Validar data PDP
  const validacaoData = validarDataPDP(form.datPdp);
  if (validacaoData !== true) {
    erros.datPdp = validacaoData;
  }

  // Validar valor do requisito
  const validacaoValReq = validarValorMW(form.valReqMax, 'Requisito Máximo');
  if (validacaoValReq !== true) {
    erros.valReqMax = validacaoValReq;
  }

  // Validar hora do requisito
  const validacaoHReq = validarHorario(form.hReqMax, 'Hora do Requisito');
  if (validacaoHReq !== true) {
    erros.hReqMax = validacaoHReq;
  }

  // Validar valor da reserva
  const validacaoValRes = validarValorMW(form.valResReqMax, 'Valor da Reserva');
  if (validacaoValRes !== true) {
    erros.valResReqMax = validacaoValRes;
  }

  // Validar hora da reserva
  const validacaoHRes = validarHorario(form.hResReqMax, 'Hora da Reserva');
  if (validacaoHRes !== true) {
    erros.hResReqMax = validacaoHRes;
  }

  return {
    valido: Object.keys(erros).length === 0,
    erros,
  };
}

/**
 * Formata data de YYYYMMDD para DD/MM/YYYY
 * @param datPdp - Data no formato YYYYMMDD
 * @returns Data formatada DD/MM/YYYY
 */
export function formatarDataPDP(datPdp: string): string {
  if (!datPdp || datPdp.length !== 8) {
    return '';
  }
  const ano = datPdp.substring(0, 4);
  const mes = datPdp.substring(4, 6);
  const dia = datPdp.substring(6, 8);
  return `${dia}/${mes}/${ano}`;
}

/**
 * Formata data de DD/MM/YYYY para YYYYMMDD
 * @param dataFormatada - Data no formato DD/MM/YYYY
 * @returns Data no formato YYYYMMDD
 */
export function desformatarDataPDP(dataFormatada: string): string {
  if (!dataFormatada) {
    return '';
  }
  const partes = dataFormatada.split('/');
  if (partes.length !== 3) {
    return '';
  }
  return `${partes[2]}${partes[1]}${partes[0]}`;
}

/**
 * Converte RequisitoArea para estado do formulário
 * @param requisito - Requisito a ser convertido
 * @returns Estado do formulário
 */
export function requisitoParaForm(requisito: RequisitoArea | null): RequirementFormState {
  if (!requisito) {
    return {
      datPdp: '',
      valReqMax: '',
      hReqMax: '',
      valResReqMax: '',
      hResReqMax: '',
      errors: {},
    };
  }

  return {
    datPdp: requisito.datPdp,
    valReqMax: requisito.valReqMax.toString(),
    hReqMax: requisito.hReqMax,
    valResReqMax: requisito.valResReqMax.toString(),
    hResReqMax: requisito.hResReqMax,
    errors: {},
  };
}

/**
 * Converte estado do formulário para DTO
 * @param form - Estado do formulário
 * @param codArea - Código da área (ex: 'NE')
 * @returns DTO para salvar
 */
export function formParaDTO(
  form: RequirementFormState,
  codArea: string = 'NE'
): SalvarRequisitoDTO {
  return {
    datPdp: form.datPdp,
    codArea,
    valReqMax: parseFloat(form.valReqMax),
    hReqMax: form.hReqMax.trim(),
    valResReqMax: parseFloat(form.valResReqMax),
    hResReqMax: form.hResReqMax.trim(),
  };
}

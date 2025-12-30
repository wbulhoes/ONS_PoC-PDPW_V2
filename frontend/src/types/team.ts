/**
 * Types para Cadastro de Equipes PDP
 * Migrado de: legado/pdpw/frmCadEquipePDP.aspx
 */

/**
 * Representa uma equipe PDP no sistema
 */
export interface EquipePDP {
  idEquipePdp: number;
  nomEquipePdp: string;
}

/**
 * DTO para criar nova equipe
 */
export interface CriarEquipeDTO {
  nomEquipePdp: string;
}

/**
 * DTO para atualizar equipe existente
 */
export interface AtualizarEquipeDTO {
  idEquipePdp: number;
  nomEquipePdp: string;
}

/**
 * Resposta de consulta de equipes com paginação
 */
export interface TeamQueryResponse {
  equipes: EquipePDP[];
  total: number;
}

/**
 * Modo de operação do formulário
 */
export type FormMode = 'view' | 'create' | 'edit';

/**
 * Estado do formulário de equipe
 */
export interface TeamFormState {
  mode: FormMode;
  equipe: Partial<EquipePDP>;
  errors: Record<string, string>;
}

/**
 * Validação do nome da equipe
 * @param nome - Nome da equipe a ser validado
 * @returns true se válido, string com mensagem de erro caso contrário
 */
export function validarNomeEquipe(nome: string): true | string {
  const nomeTrimmed = nome.trim();
  
  if (!nomeTrimmed) {
    return 'Informe o Nome.';
  }
  
  if (nomeTrimmed.length > 40) {
    return 'Nome da equipe não pode ter mais de 40 caracteres.';
  }
  
  return true;
}

/**
 * Valida se a equipe pode ser salva
 * @param equipe - Dados da equipe
 * @returns Objeto com validade e mensagens de erro
 */
export function validarEquipe(equipe: Partial<EquipePDP>): {
  valido: boolean;
  erros: Record<string, string>;
} {
  const erros: Record<string, string> = {};
  
  const validacaoNome = validarNomeEquipe(equipe.nomEquipePdp || '');
  if (validacaoNome !== true) {
    erros.nomEquipePdp = validacaoNome;
  }
  
  return {
    valido: Object.keys(erros).length === 0,
    erros,
  };
}

/**
 * Verifica se houve erro de constraint ao excluir
 * @param errorMessage - Mensagem de erro retornada
 * @returns true se é erro de constraint
 */
export function isConstraintError(errorMessage: string): boolean {
  return errorMessage.toLowerCase().includes('key value for constraint');
}

/**
 * Formata nome da equipe em uppercase e sem espaços extras
 * @param nome - Nome a ser formatado
 * @returns Nome formatado
 */
export function formatarNomeEquipe(nome: string): string {
  return nome.trim().toUpperCase();
}

/**
 * Valida seleção de itens para exclusão
 * @param ids - Array de IDs selecionados
 * @returns Objeto com validade e mensagem
 */
export function validarSelecaoExclusao(ids: number[]): {
  valido: boolean;
  mensagem: string;
} {
  if (ids.length === 0) {
    return {
      valido: false,
      mensagem: 'Selecione um item.',
    };
  }
  
  return {
    valido: true,
    mensagem: '',
  };
}

/**
 * Valida seleção de item para alteração (exige exatamente 1 item)
 * @param ids - Array de IDs selecionados
 * @returns Objeto com validade e mensagem
 */
export function validarSelecaoAlteracao(ids: number[]): {
  valido: boolean;
  mensagem: string;
} {
  if (ids.length === 0) {
    return {
      valido: false,
      mensagem: 'Selecione um item.',
    };
  }
  
  if (ids.length > 1) {
    return {
      valido: false,
      mensagem: 'Selecione apenas um item para alterar.',
    };
  }
  
  return {
    valido: true,
    mensagem: '',
  };
}

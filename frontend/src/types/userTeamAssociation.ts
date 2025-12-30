/**
 * Tipos para o módulo de Associação Usuário-Equipe PDP
 * Página legada: frmAssocUsuarEquipe.aspx
 */

export interface UsuarioEquipeAssociacao {
  idUsuarEquipePdp: number;
  idEquipePdp: string;
  nomEquipePdp: string;
  usuarId: string;
  usuarNome: string;
}

export interface EquipeOption {
  idEquipePdp: string;
  nomEquipePdp: string;
}

export interface UsuarioOption {
  usuarId: string;
  usuarNome: string;
}

export interface UserTeamQueryParams {
  idEquipePdp?: string;
  usuarId?: string;
}

export interface UserTeamQueryResponse {
  associacoes: UsuarioEquipeAssociacao[];
  total: number;
}

/**
 * Valida se equipe está selecionada
 */
export function isEquipeValida(idEquipe: string): boolean {
  return idEquipe !== '' && idEquipe !== '0';
}

/**
 * Valida se usuário está selecionado
 */
export function isUsuarioValido(usuarId: string): boolean {
  return usuarId !== '' && usuarId !== '0';
}

/**
 * Verifica se pode incluir nova associação
 */
export function podeIncluir(idEquipe: string, usuarId: string, associacoesExistentes: number): boolean {
  return isEquipeValida(idEquipe) && isUsuarioValido(usuarId) && associacoesExistentes === 0;
}

/**
 * Valida seleção para exclusão
 */
export function validarSelecaoExclusao(idsSelecionados: number[]): { valido: boolean; mensagem: string } {
  if (idsSelecionados.length === 0) {
    return { valido: false, mensagem: 'Selecione pelo menos um item para excluir.' };
  }
  return { valido: true, mensagem: '' };
}

/**
 * Formata nome em uppercase
 */
export function formatarNomeUpperCase(nome: string): string {
  return nome.toUpperCase().trim();
}

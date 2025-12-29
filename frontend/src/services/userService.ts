import { apiClient } from './apiClient';
import { HttpError } from '../utils/httpError';
import {
  User,
  UserFormData,
  UserListResponse,
  UserPaginationParams,
  UserOperationResponse,
} from '../types/user';

/**
 * User Management Service
 * Handles all HTTP communication with /api/usuarios backend endpoints
 * Throws HttpError on failure; does not swallow exceptions
 */
export const userService = {
  /**
   * Lista usuários com paginação e filtros
   * Calls GET /api/usuarios?page=1&pageSize=4&nome=...
   */
  list: async (
    params: UserPaginationParams,
    signal?: AbortSignal
  ): Promise<UserListResponse> => {
    const queryParams = new URLSearchParams();
    queryParams.append('page', params.page.toString());
    queryParams.append('pageSize', params.pageSize.toString());

    if (params.filters?.nome) {
      queryParams.append('nome', params.filters.nome);
    }
    if (params.filters?.email) {
      queryParams.append('email', params.filters.email);
    }
    if (params.filters?.telefone) {
      queryParams.append('telefone', params.filters.telefone);
    }
    if (params.filters?.perfil) {
      queryParams.append('perfil', params.filters.perfil);
    }

    try {
      const response = await apiClient.get<UserListResponse>(
        `/usuarios?${queryParams.toString()}`,
        { signal }
      );
      return response;
    } catch (error: any) {
      console.error(
        `[userService.list] Error:`,
        {
          status: error.response?.status,
          method: 'GET',
          url: `/usuarios?${queryParams.toString()}`,
          data: error.response?.data,
        }
      );
      if (error.response?.status) {
        throw new HttpError(
          error.response.status,
          error.response.data,
          `Failed to fetch users: HTTP ${error.response.status}`
        );
      }
      if (error.code === 'ECONNABORTED') {
        throw new HttpError(
          0,
          { message: 'Request timeout' },
          'Request timeout - backend took too long to respond'
        );
      }
      throw new HttpError(
        500,
        { message: error.message },
        'Failed to fetch users'
      );
    }
  },

  /**
   * Busca todos os usuários sem paginação
   */
  getAll: async (signal?: AbortSignal): Promise<User[]> => {
    try {
      const response = await apiClient.get<User[]>('/usuarios', { signal });
      return response;
    } catch (error: any) {
      console.error('[userService.getAll] Error:', error);
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        'Failed to fetch all users'
      );
    }
  },

  /**
   * Busca usuário por ID
   */
  getById: async (id: number, signal?: AbortSignal): Promise<User> => {
    try {
      const response = await apiClient.get<User>(`/usuarios/${id}`, { signal });
      return response;
    } catch (error: any) {
      console.error('[userService.getById] Error:', error);
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        `Failed to fetch user ${id}`
      );
    }
  },

  /**
   * Cria novo usuário
   * Calls POST /api/usuarios
   */
  create: async (
    user: UserFormData,
    signal?: AbortSignal
  ): Promise<UserOperationResponse> => {
    try {
      const response = await apiClient.post<User>('/usuarios', user, { signal });
      return {
        sucesso: true,
        mensagem: 'Usuário incluído com sucesso!',
        usuario: response,
      };
    } catch (error: any) {
      console.error('[userService.create] Error:', error);
      if (error.response?.status === 409) {
        throw new HttpError(
          409,
          error.response.data,
          'Usuário já existe'
        );
      }
      if (error.response?.status === 400) {
        throw new HttpError(
          400,
          error.response.data,
          'Erro de validação'
        );
      }
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        'Failed to create user'
      );
    }
  },

  /**
   * Atualiza usuário existente
   * Calls PUT /api/usuarios/{id}
   */
  update: async (
    id: number,
    user: UserFormData,
    signal?: AbortSignal
  ): Promise<UserOperationResponse> => {
    try {
      const response = await apiClient.put<User>(
        `/usuarios/${id}`,
        user,
        { signal }
      );
      return {
        sucesso: true,
        mensagem: 'Usuário alterado com sucesso!',
        usuario: response,
      };
    } catch (error: any) {
      console.error('[userService.update] Error:', error);
      if (error.response?.status === 404) {
        throw new HttpError(
          404,
          error.response.data,
          'Usuário não encontrado'
        );
      }
      if (error.response?.status === 400) {
        throw new HttpError(
          400,
          error.response.data,
          'Erro de validação'
        );
      }
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        'Failed to update user'
      );
    }
  },

  /**
   * Exclui um ou mais usuários
   * Calls DELETE /api/usuarios/{id} for each ID (serial)
   */
  delete: async (
    userIds: number[],
    signal?: AbortSignal
  ): Promise<UserOperationResponse> => {
    if (!userIds || userIds.length === 0) {
      throw new HttpError(400, null, 'No user IDs provided');
    }
    try {
      for (const userId of userIds) {
        await apiClient.delete(`/usuarios/${userId}`, { signal });
      }
      return {
        sucesso: true,
        mensagem: userIds.length === 1
          ? 'Usuário excluído com sucesso!'
          : `${userIds.length} usuário(s) excluído(s) com sucesso!`,
      };
    } catch (error: any) {
      console.error('[userService.delete] Error:', error);
      if (error.response?.status === 404) {
        throw new HttpError(
          404,
          error.response.data,
          'Usuário não encontrado'
        );
      }
      if (error.response?.status === 403) {
        throw new HttpError(
          403,
          error.response.data,
          'Permissão negada'
        );
      }
      throw new HttpError(
        error.response?.status || 500,
        error.response?.data,
        'Failed to delete user(s)'
      );
    }
  },
};

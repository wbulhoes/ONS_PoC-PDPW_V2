/**
 * Container para conectar UserRegistry com userService
 * Conecta o componente de apresentação com as chamadas reais à API
 */

import React from 'react';
import UserRegistry from './UserRegistry';
import { userService } from '../../services/userService';
import { useCreateUser } from '@/hooks/useCreateUser';
import { useUpdateUser } from '@/hooks/useUpdateUser';
import { useDeleteUser } from '@/hooks/useDeleteUser';
import { USERS_QUERY_KEY } from '@/hooks/useUsers';
import { useQueryClient } from '@tanstack/react-query';
import {
  UserFormData,
  UserPaginationParams,
  UserListResponse,
  UserFormMode,
} from '../../types/user';

const UserRegistryContainer: React.FC = () => {
  const queryClient = useQueryClient();
  const createUser = useCreateUser();
  const updateUser = useUpdateUser();
  const deleteUser = useDeleteUser();

  const handleLoadUsers = async (params: UserPaginationParams): Promise<UserListResponse> => {
    // Use React Query cache + fetch to enable caching and deduplication
    return await queryClient.fetchQuery({
      queryKey: [USERS_QUERY_KEY, params],
      queryFn: ({ signal }) => userService.list(params, signal as AbortSignal),
      staleTime: 5 * 60 * 1000,
      gcTime: 10 * 60 * 1000,
    });
  };

  const handleSaveUser = async (
    user: UserFormData,
    mode: UserFormMode
  ): Promise<{ sucesso: boolean; mensagem: string }> => {
    if (mode === UserFormMode.EDIT) {
      const res = await updateUser.mutateAsync({ id: user.usuar_id, data: user });
      return (res as any) ?? { sucesso: true, mensagem: 'Usuário alterado com sucesso!' };
    }
    const res = await createUser.mutateAsync(user);
    return (res as any) ?? { sucesso: true, mensagem: 'Usuário incluído com sucesso!' };
  };

  const handleDeleteUsers = async (
    userIds: string[]
  ): Promise<{ sucesso: boolean; mensagem: string }> => {
    const res = await deleteUser.mutateAsync({ userIds });
    return (res as any) ?? { sucesso: true, mensagem: 'Usuário(s) excluído(s) com sucesso!' };
  };

  return (
    <UserRegistry
      onLoadUsers={handleLoadUsers}
      onSaveUser={handleSaveUser}
      onDeleteUsers={handleDeleteUsers}
    />
  );
};

export default UserRegistryContainer;

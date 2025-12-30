import { useMutation, useQueryClient } from '@tanstack/react-query';
import { userService } from '@/services/userService';
import { type UserFormData, type User, type UserListResponse } from '@/types/user';
import { USERS_QUERY_KEY } from './useUsers';

const RETRY_COUNT = 3;
const RETRY_DELAYS = [100, 200, 400];

function isRetryableError(error: unknown): boolean {
  if (error && typeof error === 'object' && 'status' in (error as any)) {
    const status = (error as any).status as number;
    return status >= 500;
  }
  return false;
}

interface UpdateUserInput {
  id: string;
  data: UserFormData;
}

export function useUpdateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, data }: UpdateUserInput) => userService.update(id, data),
    retry: (failureCount, error: any) => {
      if (!isRetryableError(error)) return false;
      return failureCount < RETRY_COUNT;
    },
    retryDelay: attempt => RETRY_DELAYS[attempt - 1] ?? RETRY_DELAYS[RETRY_DELAYS.length - 1],
    onMutate: async ({ id, data }) => {
      const queryKeyPrefix = [USERS_QUERY_KEY];
      const previousCaches: Array<{ key: unknown[]; data: unknown }> = [];

      await queryClient.cancelQueries({ queryKey: queryKeyPrefix });

      queryClient.getQueriesData<UserListResponse>({ queryKey: queryKeyPrefix }).forEach(([key, previous]) => {
        if (previous) {
          previousCaches.push({ key: key as unknown[], data: previous });
          const updatedUsuarios = (previous.usuarios || []).map((user) =>
            user.usuar_id === id
              ? {
                  ...(user as User),
                  usuar_nome: data.usuar_nome,
                  usuar_email: data.usuar_email,
                  usuar_telefone: data.usuar_telefone,
                }
              : user
          );
          queryClient.setQueryData<UserListResponse>(key, {
            ...previous,
            usuarios: updatedUsuarios,
          });
        }
      });

      return { previousCaches };
    },
    onError: (_error, _variables, context) => {
      context?.previousCaches?.forEach(({ key, data }) => {
        queryClient.setQueryData(key, data);
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [USERS_QUERY_KEY] });
    },
  });
}

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

export function useCreateUser() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UserFormData) => userService.create(data),
    retry: (failureCount, error: any) => {
      if (!isRetryableError(error)) return false;
      return failureCount < RETRY_COUNT;
    },
    retryDelay: attempt => RETRY_DELAYS[attempt - 1] ?? RETRY_DELAYS[RETRY_DELAYS.length - 1],
    onMutate: async (data) => {
      const queryKeyPrefix = [USERS_QUERY_KEY];
      const previousCaches: Array<{ key: unknown[]; data: unknown }> = [];

      // Pause outgoing queries
      await queryClient.cancelQueries({ queryKey: queryKeyPrefix });

      // Snapshot and optimistically update all user list caches
      queryClient.getQueriesData<UserListResponse>({ queryKey: queryKeyPrefix }).forEach(([key, previous]) => {
        if (previous) {
          previousCaches.push({ key: key as unknown[], data: previous });
          const optimisticUser: User = {
            usuar_id: data.usuar_id,
            usuar_nome: data.usuar_nome,
            usuar_email: data.usuar_email,
            usuar_telefone: data.usuar_telefone,
          };
          queryClient.setQueryData<UserListResponse>(key, {
            ...previous,
            usuarios: [optimisticUser, ...(previous.usuarios || [])],
            total: (previous.total ?? 0) + 1,
          });
        }
      });

      return { previousCaches };
    },
    onError: (_error, _variables, context) => {
      // Rollback to previous caches
      context?.previousCaches?.forEach(({ key, data }) => {
        queryClient.setQueryData(key, data);
      });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [USERS_QUERY_KEY] });
    },
  });
}

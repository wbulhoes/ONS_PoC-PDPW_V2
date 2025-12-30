import { useQuery } from '@tanstack/react-query';
import { userService } from '@/services/userService';
import { type UserListResponse, type UserPaginationParams } from '@/types/user';

const USERS_QUERY_KEY = 'users';
const FIVE_MINUTES_MS = 5 * 60 * 1000;

interface UseUsersOptions {
  enabled?: boolean;
}

export function useUsers(
  params: UserPaginationParams,
  options: UseUsersOptions = {}
) {
  const { enabled = true } = options;

  return useQuery<UserListResponse>({
    queryKey: [USERS_QUERY_KEY, params],
    queryFn: ({ signal }) => userService.list(params, signal),
    staleTime: FIVE_MINUTES_MS,
    enabled,
    refetchOnWindowFocus: false,
  });
}

export { USERS_QUERY_KEY };

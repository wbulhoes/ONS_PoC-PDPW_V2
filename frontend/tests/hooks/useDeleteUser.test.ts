import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { createElement, type ReactNode } from 'react';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useDeleteUser } from '../../src/hooks/useDeleteUser';
import { USERS_QUERY_KEY } from '../../src/hooks/useUsers';
import { userService } from '../../src/services/userService';
import type { UserListResponse, UserPaginationParams } from '../../src/types/user';

vi.mock('@/services/userService', () => ({
  userService: {
    list: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn(),
  },
}));

const mockedUserService = vi.mocked(userService, true);

function createTestQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

function createWrapper(queryClient: QueryClient) {
  return ({ children }: { children: ReactNode }) =>
    createElement(QueryClientProvider, { client: queryClient, children });
}

const paramsPage1: UserPaginationParams = { page: 1, pageSize: 5 };
const paramsPage2: UserPaginationParams = { page: 2, pageSize: 5, filters: { nome: 'Filtro' } };

const listPage1: UserListResponse = {
  sucesso: true,
  total: 2,
  usuarios: [
    { usuar_id: 'ADMIN', usuar_nome: 'Administrador', usuar_email: 'admin@test.com', usuar_telefone: '123' },
    { usuar_id: 'JOAO', usuar_nome: 'Joao', usuar_email: 'joao@test.com', usuar_telefone: '999' },
  ],
};

const listPage2: UserListResponse = {
  sucesso: true,
  total: 2,
  usuarios: [
    { usuar_id: 'JOAO', usuar_nome: 'Joao', usuar_email: 'joao@test.com', usuar_telefone: '999' },
    { usuar_id: 'MARIA', usuar_nome: 'Maria', usuar_email: 'maria@test.com', usuar_telefone: '888' },
  ],
};

function seedUserCaches(queryClient: QueryClient) {
  queryClient.setQueryData([USERS_QUERY_KEY, paramsPage1], listPage1);
  queryClient.setQueryData([USERS_QUERY_KEY, paramsPage2], listPage2);
}

describe('useDeleteUser', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useRealTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('optimistically removes users across caches and invalidates on success', async () => {
    const queryClient = createTestQueryClient();
    const invalidateSpy = vi.spyOn(queryClient, 'invalidateQueries');
    seedUserCaches(queryClient);

    mockedUserService.delete.mockResolvedValue({ sucesso: true });

    const { result } = renderHook(() => useDeleteUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync({ userIds: ['JOAO'] });

    await waitFor(() => {
      const cacheOne = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage1]);
      const cacheTwo = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage2]);
      return cacheOne?.usuarios.every((u) => u.usuar_id !== 'JOAO') && cacheTwo?.usuarios.every((u) => u.usuar_id !== 'JOAO');
    });

    await mutatePromise;

    expect(mockedUserService.delete).toHaveBeenCalledWith(['JOAO']);
    expect(invalidateSpy).toHaveBeenCalledWith({ queryKey: [USERS_QUERY_KEY] });
  });

  it('rolls back optimistic delete on error', async () => {
    const queryClient = createTestQueryClient();
    seedUserCaches(queryClient);

    let rejectFn: ((error: unknown) => void) | undefined;
    const deferred = new Promise((_resolve, reject) => {
      rejectFn = reject;
    });

    mockedUserService.delete.mockReturnValue(deferred as unknown as Promise<unknown>);

    const { result } = renderHook(() => useDeleteUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate({ userIds: ['JOAO'] });

    await waitFor(() => {
      const cacheOne = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage1]);
      const cacheTwo = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage2]);
      return cacheOne?.usuarios.every((u) => u.usuar_id !== 'JOAO') && cacheTwo?.usuarios.every((u) => u.usuar_id !== 'JOAO');
    });

    rejectFn?.({ status: 400 });

    await waitFor(() => expect(result.current.isError).toBe(true));

    const cacheOne = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage1]);
    const cacheTwo = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, paramsPage2]);

    expect(cacheOne).toEqual(listPage1);
    expect(cacheTwo).toEqual(listPage2);
  });

  it('retries on server errors up to three attempts', async () => {
    const queryClient = createTestQueryClient();
    seedUserCaches(queryClient);

    mockedUserService.delete
      .mockRejectedValueOnce({ status: 503 })
      .mockRejectedValueOnce({ status: 500 })
      .mockResolvedValueOnce({ sucesso: true });

    vi.useFakeTimers();

    const { result } = renderHook(() => useDeleteUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync({ userIds: ['JOAO'] });

    await vi.runAllTimersAsync();
    await mutatePromise;

    expect(mockedUserService.delete).toHaveBeenCalledTimes(3);
  });

  it('does not retry on client errors', async () => {
    const queryClient = createTestQueryClient();
    seedUserCaches(queryClient);

    mockedUserService.delete.mockRejectedValue({ status: 403 });

    const { result } = renderHook(() => useDeleteUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate({ userIds: ['JOAO'] });

    await waitFor(() => expect(result.current.isError).toBe(true));
    expect(mockedUserService.delete).toHaveBeenCalledTimes(1);
  });
});

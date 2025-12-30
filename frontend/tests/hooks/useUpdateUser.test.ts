import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { createElement, type ReactNode } from 'react';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useUpdateUser } from '../../src/hooks/useUpdateUser';
import { USERS_QUERY_KEY } from '../../src/hooks/useUsers';
import { userService } from '../../src/services/userService';
import type { UserFormData, UserListResponse, UserPaginationParams } from '../../src/types/user';

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

const baseParams: UserPaginationParams = { page: 1, pageSize: 5 };

const initialList: UserListResponse = {
  sucesso: true,
  total: 2,
  usuarios: [
    { usuar_id: 'ADMIN', usuar_nome: 'Administrador', usuar_email: 'admin@test.com', usuar_telefone: '123' },
    { usuar_id: 'JOAO', usuar_nome: 'Joao', usuar_email: 'joao@test.com', usuar_telefone: '999' },
  ],
};

function seedUserCache(queryClient: QueryClient, data: UserListResponse = initialList, params: UserPaginationParams = baseParams) {
  queryClient.setQueryData([USERS_QUERY_KEY, params], data);
}

describe('useUpdateUser', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useRealTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('optimistically updates user and invalidates list on success', async () => {
    const queryClient = createTestQueryClient();
    const invalidateSpy = vi.spyOn(queryClient, 'invalidateQueries');
    seedUserCache(queryClient);

    const updatedData: UserFormData = {
      usuar_id: 'ADMIN',
      usuar_nome: 'Administrador Atualizado',
      usuar_email: 'admin@pdpw.com',
      usuar_telefone: '321',
    };

    mockedUserService.update.mockResolvedValue({ sucesso: true });

    const { result } = renderHook(() => useUpdateUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync({ id: 'ADMIN', data: updatedData });

    await waitFor(() => {
      const optimistic = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
      return optimistic?.usuarios.find((u) => u.usuar_id === 'ADMIN')?.usuar_nome === updatedData.usuar_nome;
    });

    await mutatePromise;

    expect(mockedUserService.update).toHaveBeenCalledWith('ADMIN', updatedData);
    expect(invalidateSpy).toHaveBeenCalledWith({ queryKey: [USERS_QUERY_KEY] });
  });

  it('rolls back optimistic update on error', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const updatedData: UserFormData = {
      usuar_id: 'ADMIN',
      usuar_nome: 'Tentativa',
      usuar_email: 'tentativa@test.com',
      usuar_telefone: '000',
    };

    let rejectFn: ((error: unknown) => void) | undefined;
    const deferred = new Promise((_resolve, reject) => {
      rejectFn = reject;
    });

    mockedUserService.update.mockReturnValue(deferred as unknown as Promise<unknown>);

    const { result } = renderHook(() => useUpdateUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate({ id: 'ADMIN', data: updatedData });

    await waitFor(() => {
      const optimistic = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
      return optimistic?.usuarios.find((u) => u.usuar_id === 'ADMIN')?.usuar_nome === 'Tentativa';
    });

    rejectFn?.({ status: 404 });

    await waitFor(() => expect(result.current.isError).toBe(true));

    const restored = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
    expect(restored).toEqual(initialList);
  });

  it('retries on server errors up to three attempts', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const updatedData: UserFormData = {
      usuar_id: 'ADMIN',
      usuar_nome: 'Retry Admin',
      usuar_email: 'retry@test.com',
      usuar_telefone: '444',
    };

    mockedUserService.update
      .mockRejectedValueOnce({ status: 503 })
      .mockRejectedValueOnce({ status: 500 })
      .mockResolvedValueOnce({ sucesso: true });

    vi.useFakeTimers();

    const { result } = renderHook(() => useUpdateUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync({ id: 'ADMIN', data: updatedData });

    await vi.runAllTimersAsync();
    await mutatePromise;

    expect(mockedUserService.update).toHaveBeenCalledTimes(3);
  });

  it('does not retry on client errors', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const updatedData: UserFormData = {
      usuar_id: 'ADMIN',
      usuar_nome: 'Erro',
      usuar_email: 'erro@test.com',
      usuar_telefone: '777',
    };

    mockedUserService.update.mockRejectedValue({ status: 400 });

    const { result } = renderHook(() => useUpdateUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate({ id: 'ADMIN', data: updatedData });

    await waitFor(() => expect(result.current.isError).toBe(true));
    expect(mockedUserService.update).toHaveBeenCalledTimes(1);
  });
});

import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { createElement, type ReactNode } from 'react';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useCreateUser } from '../../src/hooks/useCreateUser';
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
  total: 1,
  usuarios: [
    { usuar_id: 'ADMIN', usuar_nome: 'Administrador', usuar_email: 'admin@test.com', usuar_telefone: '123' },
  ],
};

function seedUserCache(queryClient: QueryClient, data: UserListResponse = initialList, params: UserPaginationParams = baseParams) {
  queryClient.setQueryData([USERS_QUERY_KEY, params], data);
}

describe('useCreateUser', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useRealTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('optimistically adds user and invalidates list on success', async () => {
    const queryClient = createTestQueryClient();
    const invalidateSpy = vi.spyOn(queryClient, 'invalidateQueries');
    seedUserCache(queryClient);

    const newUser: UserFormData = {
      usuar_id: 'NOVO',
      usuar_nome: 'Novo Usuario',
      usuar_email: 'novo@test.com',
      usuar_telefone: '555',
    };

    mockedUserService.create.mockResolvedValue({ sucesso: true });

    const { result } = renderHook(() => useCreateUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync(newUser);

    await waitFor(() => {
      const optimistic = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
      return optimistic?.usuarios[0]?.usuar_id === newUser.usuar_id;
    });

    await mutatePromise;

    expect(mockedUserService.create).toHaveBeenCalledWith(newUser);
    expect(invalidateSpy).toHaveBeenCalledWith({ queryKey: [USERS_QUERY_KEY] });
  });

  it('rolls back optimistic user on error', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const newUser: UserFormData = {
      usuar_id: 'DUP',
      usuar_nome: 'Duplicado',
      usuar_email: 'dup@test.com',
      usuar_telefone: '555',
    };

    let rejectFn: ((error: unknown) => void) | undefined;
    const deferred = new Promise((_resolve, reject) => {
      rejectFn = reject;
    });

    mockedUserService.create.mockReturnValue(deferred as unknown as Promise<unknown>);

    const { result } = renderHook(() => useCreateUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate(newUser);

    await waitFor(() => {
      const optimistic = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
      return optimistic?.usuarios.some((u) => u.usuar_id === 'DUP');
    });

    rejectFn?.({ status: 409 });

    await waitFor(() => expect(result.current.isError).toBe(true));

    const restored = queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, baseParams]);
    expect(restored).toEqual(initialList);
  });

  it('retries up to three times for server errors', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const newUser: UserFormData = {
      usuar_id: 'RETRY',
      usuar_nome: 'Retry User',
      usuar_email: 'retry@test.com',
      usuar_telefone: '222',
    };

    mockedUserService.create
      .mockRejectedValueOnce({ status: 500 })
      .mockRejectedValueOnce({ status: 502 })
      .mockResolvedValueOnce({ sucesso: true });

    vi.useFakeTimers();

    const { result } = renderHook(() => useCreateUser(), {
      wrapper: createWrapper(queryClient),
    });

    const mutatePromise = result.current.mutateAsync(newUser);

    await vi.runAllTimersAsync();
    await mutatePromise;

    expect(mockedUserService.create).toHaveBeenCalledTimes(3);
  });

  it('does not retry on client errors', async () => {
    const queryClient = createTestQueryClient();
    seedUserCache(queryClient);

    const newUser: UserFormData = {
      usuar_id: 'ERR',
      usuar_nome: 'Erro',
      usuar_email: 'erro@test.com',
      usuar_telefone: '333',
    };

    mockedUserService.create.mockRejectedValue({ status: 400 });

    const { result } = renderHook(() => useCreateUser(), {
      wrapper: createWrapper(queryClient),
    });

    result.current.mutate(newUser);

    await waitFor(() => expect(result.current.isError).toBe(true));
    expect(mockedUserService.create).toHaveBeenCalledTimes(1);
  });
});

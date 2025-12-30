import { describe, it, expect, beforeEach, vi } from 'vitest';
import { createElement, type ReactNode } from 'react';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useUsers, USERS_QUERY_KEY } from '../../src/hooks/useUsers';
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

const defaultParams: UserPaginationParams = {
  page: 1,
  pageSize: 5,
  filters: { login: 'adm', nome: 'Admin' },
};

const firstPage: UserListResponse = {
  sucesso: true,
  total: 2,
  usuarios: [
    { usuar_id: 'ADMIN', usuar_nome: 'Administrador', usuar_email: 'admin@test.com', usuar_telefone: '123' },
    { usuar_id: 'JOAO', usuar_nome: 'Joao', usuar_email: 'joao@test.com', usuar_telefone: '999' },
  ],
};

const secondPage: UserListResponse = {
  sucesso: true,
  total: 2,
  usuarios: [
    { usuar_id: 'MARIA', usuar_nome: 'Maria', usuar_email: 'maria@test.com', usuar_telefone: '111' },
  ],
};

describe('useUsers', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('fetches users with params and passes abort signal', async () => {
    mockedUserService.list.mockResolvedValue(firstPage);
    const queryClient = createTestQueryClient();

    const { result } = renderHook(() => useUsers(defaultParams), {
      wrapper: createWrapper(queryClient),
    });

    await waitFor(() => expect(result.current.isSuccess).toBe(true));

    expect(result.current.data).toEqual(firstPage);
    expect(mockedUserService.list).toHaveBeenCalledWith(defaultParams, expect.any(AbortSignal));
  });

  it('returns cached data without refetch when params are unchanged', async () => {
    mockedUserService.list.mockResolvedValue(firstPage);
    const queryClient = createTestQueryClient();

    const { result, rerender } = renderHook(
      (params: UserPaginationParams) => useUsers(params),
      {
        initialProps: defaultParams,
        wrapper: createWrapper(queryClient),
      }
    );

    await waitFor(() => expect(result.current.isSuccess).toBe(true));
    expect(mockedUserService.list).toHaveBeenCalledTimes(1);

    mockedUserService.list.mockClear();
    rerender(defaultParams);

    expect(mockedUserService.list).not.toHaveBeenCalled();
    expect(queryClient.getQueryData<UserListResponse>([USERS_QUERY_KEY, defaultParams])).toEqual(firstPage);
  });

  it('refetches when params change', async () => {
    mockedUserService.list
      .mockResolvedValueOnce(firstPage)
      .mockResolvedValueOnce(secondPage);

    const queryClient = createTestQueryClient();
    const { result, rerender } = renderHook(
      (params: UserPaginationParams) => useUsers(params),
      {
        initialProps: defaultParams,
        wrapper: createWrapper(queryClient),
      }
    );

    await waitFor(() => expect(result.current.data).toEqual(firstPage));

    const newParams: UserPaginationParams = { ...defaultParams, page: 2 };
    rerender(newParams);

    await waitFor(() => expect(result.current.data).toEqual(secondPage));
    expect(mockedUserService.list).toHaveBeenCalledTimes(2);
    expect(mockedUserService.list).toHaveBeenLastCalledWith(newParams, expect.any(AbortSignal));
  });

  it('does not fetch when disabled', () => {
    const queryClient = createTestQueryClient();

    const { result } = renderHook(
      () => useUsers(defaultParams, { enabled: false }),
      { wrapper: createWrapper(queryClient) }
    );

    expect(result.current.fetchStatus).toBe('idle');
    expect(result.current.data).toBeUndefined();
    expect(mockedUserService.list).not.toHaveBeenCalled();
  });
});

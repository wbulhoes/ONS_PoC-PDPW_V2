import { describe, it, expect, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { usePlants, usePlantsByCompany, usePlantById, useCreatePlant, useUpdatePlant, useDeletePlant } from '../../src/hooks/usePlants';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

function createTestQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

function createWrapper(queryClient: QueryClient) {
  return ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  );
}

describe('usePlants Hook', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('usePlants', () => {
    it('should fetch all plants', async () => {
      const { result } = renderHook(() => usePlants(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(Array.isArray(result.current.data)).toBe(true);
    });
  });

  describe('usePlantsByCompany', () => {
    it('should fetch plants by company ID', async () => {
      mockEndpoint('get', '/plants?companyId=1', [
        { id: 1, name: 'Plant 1', code: 'P1', type: 'HYDRO', companyId: 1 },
      ]);

      const { result } = renderHook(() => usePlantsByCompany(1), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(Array.isArray(result.current.data)).toBe(true);
    });

    it('should not fetch when company ID is undefined', () => {
      const { result } = renderHook(() => usePlantsByCompany(undefined), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.data).toBeUndefined();
    });
  });

  describe('usePlantById', () => {
    it('should fetch plant by ID', async () => {
      mockEndpoint('get', '/plants/1', {
        id: 1,
        name: 'Plant 1',
        code: 'P1',
        type: 'HYDRO',
        companyId: 1,
      });

      const { result } = renderHook(() => usePlantById(1), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
    });
  });

  describe('useCreatePlant', () => {
    it('should create plant', async () => {
      mockEndpoint(
        'post',
        '/plants',
        { id: 2, name: 'New Plant', code: 'NP', type: 'SOLAR', companyId: 1 },
        { status: 201 }
      );

      const { result } = renderHook(() => useCreatePlant(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        name: 'New Plant',
        code: 'NP',
        type: 'SOLAR',
        companyId: 1,
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(2);
    });
  });

  describe('useUpdatePlant', () => {
    it('should update plant', async () => {
      mockEndpoint('put', '/plants/1', {
        id: 1,
        name: 'Updated Plant',
        code: 'P1',
        type: 'HYDRO',
        companyId: 1,
      });

      const { result } = renderHook(() => useUpdatePlant(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ id: 1, dto: { name: 'Updated Plant' } });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.name).toBe('Updated Plant');
    });
  });

  describe('useDeletePlant', () => {
    it('should delete plant', async () => {
      mockEndpoint('delete', '/plants/1', {}, { status: 204 });

      const { result } = renderHook(() => useDeletePlant(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(1);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });
  });
});

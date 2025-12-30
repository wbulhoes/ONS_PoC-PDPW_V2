import { describe, it, expect, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { usePlantTypes, usePlantTypeById, useCreatePlantType, useUpdatePlantType, useDeletePlantType } from '../../src/hooks/usePlantTypes';
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

describe('usePlantTypes Hook', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('usePlantTypes', () => {
    it('should fetch all plant types', async () => {
      const { result } = renderHook(() => usePlantTypes(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(Array.isArray(result.current.data)).toBe(true);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('get', '/plant-types', 500, 'Server error');

      const { result } = renderHook(() => usePlantTypes(), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('usePlantTypeById', () => {
    it('should fetch plant type by ID', async () => {
      mockEndpoint('get', '/plant-types/1', { id: 1, name: 'Hydroelectric', code: 'HYDRO' });

      const { result } = renderHook(() => usePlantTypeById(1), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
    });

    it('should not fetch when ID is undefined', () => {
      const { result } = renderHook(() => usePlantTypeById(undefined), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.data).toBeUndefined();
    });
  });

  describe('useCreatePlantType', () => {
    it('should create plant type', async () => {
      mockEndpoint(
        'post',
        '/plant-types',
        { id: 2, name: 'Solar', code: 'SOLAR' },
        { status: 201 }
      );

      const { result } = renderHook(() => useCreatePlantType(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ name: 'Solar', code: 'SOLAR' });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(2);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('post', '/plant-types', 400, 'Validation failed');

      const { result } = renderHook(() => useCreatePlantType(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ name: '', code: '' });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('useUpdatePlantType', () => {
    it('should update plant type', async () => {
      mockEndpoint('put', '/plant-types/1', { id: 1, name: 'Updated Type', code: 'HYDRO' });

      const { result } = renderHook(() => useUpdatePlantType(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ id: 1, dto: { name: 'Updated Type' } });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.name).toBe('Updated Type');
    });
  });

  describe('useDeletePlantType', () => {
    it('should delete plant type', async () => {
      mockEndpoint('delete', '/plant-types/1', {}, { status: 204 });

      const { result } = renderHook(() => useDeletePlantType(), {
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

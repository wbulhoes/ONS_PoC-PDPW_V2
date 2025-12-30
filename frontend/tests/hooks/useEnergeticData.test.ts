import { describe, it, expect, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {
  useEnergeticData,
  useEnergeticDataByPeriod,
  useEnergeticDataByUsinaAndDate,
  useCreateEnergeticData,
  useUpdateEnergeticData,
  useDeleteEnergeticData,
  useBulkUpsertEnergeticData,
} from '../../src/hooks/useEnergeticData';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

/**
 * T029: Create hook tests (loading/success/error states)
 */

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

describe('useEnergeticData Hook', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('useEnergeticData', () => {
    it('should load all energetic data successfully', async () => {
      const { result } = renderHook(() => useEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      // Initial state: loading
      expect(result.current.isLoading).toBe(true);

      // Wait for data to load
      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      // Success state
      expect(result.current.data).toBeDefined();
      expect(Array.isArray(result.current.data)).toBe(true);
    });

    it('should handle error state', async () => {
      mockErrorEndpoint('get', '/dadosenergeticos', 500, 'Server error');

      const { result } = renderHook(() => useEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      // Error state
      expect(result.current.error).toBeDefined();
    });
  });

  describe('useEnergeticDataByPeriod', () => {
    it('should fetch data for period successfully', async () => {
      mockEndpoint('get', '/dadosenergeticos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31', [
        { id: 1, dataReferencia: '2024-01-15T00:00:00Z' },
      ]);

      const { result } = renderHook(
        () => useEnergeticDataByPeriod('2024-01-01', '2024-01-31'),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
    });
  });

  describe('useEnergeticDataByUsinaAndDate', () => {
    it('should fetch data for usina and date successfully', async () => {
      mockEndpoint('get', '/dadosenergeticos/usina/100/data/2024-01-15', [
        { id: 1, intervalo: 1, valorMW: 100 },
      ]);

      const { result } = renderHook(
        () => useEnergeticDataByUsinaAndDate(100, '2024-01-15'),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(result.current.data?.[0].intervalo).toBe(1);
    });
  });

  describe('useCreateEnergeticData', () => {
    it('should create energetic data successfully', async () => {
      mockEndpoint(
        'post',
        '/dadosenergeticos',
        { id: 1, usinaId: 100, valorMW: 100, razaoEnergetica: 50 },
        { status: 201 }
      );

      const { result } = renderHook(() => useCreateEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isPending).toBe(false);

      result.current.mutate({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        valorMW: 100,
        razaoEnergetica: 50,
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle creation error', async () => {
      mockErrorEndpoint('post', '/dadosenergeticos', 400, 'Validation failed');

      const { result } = renderHook(() => useCreateEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        valorMW: 100,
        razaoEnergetica: 50,
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useUpdateEnergeticData', () => {
    it('should update energetic data successfully', async () => {
      mockEndpoint('put', '/dadosenergeticos/1', { id: 1, valorMW: 120 });

      const { result } = renderHook(() => useUpdateEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 1,
        dto: { valorMW: 120 },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle update error', async () => {
      mockErrorEndpoint('put', '/dadosenergeticos/999', 404, 'Not found');

      const { result } = renderHook(() => useUpdateEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 999,
        dto: { valorMW: 120 },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('useDeleteEnergeticData', () => {
    it('should delete energetic data successfully', async () => {
      mockEndpoint('delete', '/dadosenergeticos/1', {}, { status: 204 });

      const { result } = renderHook(() => useDeleteEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(1);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle deletion error', async () => {
      mockErrorEndpoint('delete', '/dadosenergeticos/999', 404, 'Not found');

      const { result } = renderHook(() => useDeleteEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(999);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('useBulkUpsertEnergeticData', () => {
    it('should bulk upsert 48 intervals successfully', async () => {
      const dados = Array.from({ length: 48 }, (_, i) => ({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: i + 1,
        valorMW: 100 + i,
        razaoEnergetica: 50 + i,
      }));

      mockEndpoint(
        'post',
        '/dadosenergeticos/bulk',
        dados.map((d, i) => ({ id: i + 1, ...d })),
        { status: 201 }
      );

      const { result } = renderHook(() => useBulkUpsertEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(dados);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data).toHaveLength(48);
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle bulk upsert error', async () => {
      mockErrorEndpoint('post', '/dadosenergeticos/bulk', 500, 'Server error');

      const dados = Array.from({ length: 48 }, (_, i) => ({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: i + 1,
        valorMW: 100,
        razaoEnergetica: 50,
      }));

      const { result } = renderHook(() => useBulkUpsertEnergeticData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(dados);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });
});

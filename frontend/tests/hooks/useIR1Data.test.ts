import { describe, it, expect, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {
  useIR1Data,
  useIR1DataByDate,
  useCreateIR1Data,
  useUpdateIR1Data,
  useDeleteIR1Data,
  useBulkUpsertIR1Data,
} from '../../src/hooks/useIR1Data';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

/**
 * T059: Create hook tests (loading/success/error states)
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

describe('useIR1Data Hook', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('useIR1Data', () => {
    it('should load all IR1 data successfully', async () => {
      const { result } = renderHook(() => useIR1Data(), {
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
      mockErrorEndpoint('get', '/insumos-recebimento/ir1', 500, 'Server error');

      const { result } = renderHook(() => useIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      // Error state
      expect(result.current.error).toBeDefined();
    });
  });

  describe('useIR1DataByDate', () => {
    it('should fetch IR1 data by date successfully', async () => {
      mockEndpoint('get', '/insumos-recebimento/ir1/2024-01-15', {
        id: 1,
        dataReferencia: '2024-01-15T00:00:00Z',
        niveisPartida: [
          { usinaId: 100, usinaNome: 'Usina Teste', nivel: 500.5, volume: 1000.0 },
        ],
      });

      const { result } = renderHook(() => useIR1DataByDate('2024-01-15'), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(result.current.data?.dataReferencia).toBe('2024-01-15T00:00:00Z');
      expect(result.current.data?.niveisPartida).toHaveLength(1);
    });

    it('should not fetch when date is not provided', async () => {
      const { result } = renderHook(() => useIR1DataByDate(''), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(false);
      expect(result.current.data).toBeUndefined();
    });

    it('should handle error state for date query', async () => {
      mockErrorEndpoint('get', '/insumos-recebimento/ir1/2024-01-15', 404, 'Not found');

      const { result } = renderHook(() => useIR1DataByDate('2024-01-15'), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });

    it('should format date from YYYYMMDD to YYYY-MM-DD', async () => {
      mockEndpoint('get', '/insumos-recebimento/ir1/2024-01-15', {
        id: 1,
        dataReferencia: '2024-01-15T00:00:00Z',
        niveisPartida: [],
      });

      const { result } = renderHook(() => useIR1DataByDate('20240115'), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
    });
  });

  describe('useCreateIR1Data', () => {
    it('should create IR1 data successfully', async () => {
      mockEndpoint(
        'post',
        '/insumos-recebimento/ir1',
        {
          id: 1,
          dataReferencia: '2024-01-15T00:00:00Z',
          niveisPartida: [{ usinaId: 100, nivel: 500.5, volume: 1000.0 }],
        },
        { status: 201 }
      );

      const { result } = renderHook(() => useCreateIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isPending).toBe(false);

      result.current.mutate({
        dataReferencia: '2024-01-15',
        niveisPartida: [{ usinaId: 100, nivel: 500.5, volume: 1000.0 }],
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle creation error', async () => {
      mockErrorEndpoint('post', '/insumos-recebimento/ir1', 400, 'Invalid data');

      const { result } = renderHook(() => useCreateIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        dataReferencia: '2024-01-15',
        niveisPartida: [],
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });

    it('should invalidate queries on success', async () => {
      mockEndpoint('post', '/insumos-recebimento/ir1', { id: 1 }, { status: 201 });

      const { result } = renderHook(() => useCreateIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        dataReferencia: '2024-01-15',
        niveisPartida: [],
      });

      await waitFor(() => {
        expect(result.current.isSuccess).toBe(true);
      });

      // Verify that queries were invalidated
      expect(queryClient.getQueryState(['ir1', 'all'])?.isInvalidated).toBe(true);
    });
  });

  describe('useUpdateIR1Data', () => {
    it('should update IR1 data successfully', async () => {
      mockEndpoint('put', '/insumos-recebimento/ir1/1', {
        id: 1,
        dataReferencia: '2024-01-15T00:00:00Z',
        niveisPartida: [{ usinaId: 100, nivel: 600.0, volume: 1200.0 }],
      });

      const { result } = renderHook(() => useUpdateIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 1,
        dto: {
          niveisPartida: [{ usinaId: 100, nivel: 600.0, volume: 1200.0 }],
        },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
      expect(result.current.data?.niveisPartida[0].nivel).toBe(600.0);
    });

    it('should handle update error', async () => {
      mockErrorEndpoint('put', '/insumos-recebimento/ir1/1', 404, 'Not found');

      const { result } = renderHook(() => useUpdateIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 1,
        dto: { niveisPartida: [] },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useDeleteIR1Data', () => {
    it('should delete IR1 data successfully', async () => {
      mockEndpoint('delete', '/insumos-recebimento/ir1/1', null, { status: 204 });

      const { result } = renderHook(() => useDeleteIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(1);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle deletion error', async () => {
      mockErrorEndpoint('delete', '/insumos-recebimento/ir1/999', 404, 'Not found');

      const { result } = renderHook(() => useDeleteIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(999);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useBulkUpsertIR1Data', () => {
    it('should bulk upsert IR1 data successfully', async () => {
      mockEndpoint(
        'post',
        '/insumos-recebimento/ir1/bulk',
        [
          {
            id: 1,
            dataReferencia: '2024-01-15T00:00:00Z',
            niveisPartida: [{ usinaId: 100, nivel: 500.5, volume: 1000.0 }],
          },
          {
            id: 2,
            dataReferencia: '2024-01-16T00:00:00Z',
            niveisPartida: [{ usinaId: 101, nivel: 550.0, volume: 1100.0 }],
          },
        ],
        { status: 200 }
      );

      const { result } = renderHook(() => useBulkUpsertIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate([
        {
          dataReferencia: '2024-01-15',
          niveisPartida: [{ usinaId: 100, nivel: 500.5, volume: 1000.0 }],
        },
        {
          dataReferencia: '2024-01-16',
          niveisPartida: [{ usinaId: 101, nivel: 550.0, volume: 1100.0 }],
        },
      ]);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
      expect(result.current.data).toHaveLength(2);
    });

    it('should handle bulk upsert error', async () => {
      mockErrorEndpoint('post', '/insumos-recebimento/ir1/bulk', 400, 'Invalid bulk data');

      const { result } = renderHook(() => useBulkUpsertIR1Data(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate([]);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });
});

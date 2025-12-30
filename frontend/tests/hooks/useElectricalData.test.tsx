/**
 * Testes para hooks de Dados Elétricos (Razão Elétrica)
 * Testa os hooks do React Query que gerenciam estado de dados elétricos
 * 
 * T044: Create hook tests (loading/success/error states)
 */

import { describe, it, expect, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import {
  useElectricalData,
  useElectricalDataByPeriod,
  useElectricalDataByUsinaAndDate,
  useCreateElectricalData,
  useUpdateElectricalData,
  useDeleteElectricalData,
  useBulkUpsertElectricalData,
} from '../../src/hooks/useElectricalData';
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

describe('useElectricalData hooks (T044)', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('useElectricalData - Loading/Success/Error states', () => {
    it('should load all electrical data successfully', async () => {
      const { result } = renderHook(() => useElectricalData(), {
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
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle error state when fetch fails', async () => {
      mockErrorEndpoint('get', '/dados-eletricos', 500, 'Server error');

      const { result } = renderHook(() => useElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      // Error state
      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useElectricalDataByPeriod - Loading/Success/Error states', () => {
    it('should fetch data for period successfully', async () => {
      mockEndpoint('get', '/dados-eletricos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31', [
        { id: 1, dataReferencia: '2024-01-15T00:00:00Z', potenciaMW: 100 },
      ]);

      const { result } = renderHook(
        () => useElectricalDataByPeriod('2024-01-01', '2024-01-31'),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(result.current.isSuccess).toBe(true);
    });

    it('should not fetch if dates are empty', () => {
      const { result } = renderHook(
        () => useElectricalDataByPeriod('', ''),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(false);
    });
  });

  describe('useElectricalDataByUsinaAndDate - Loading/Success/Error states', () => {
    it('should fetch data for usina and date successfully', async () => {
      mockEndpoint('get', '/dados-eletricos/usina/100/data/2024-01-15', [
        { id: 1, intervalo: 1, potenciaMW: 100 },
      ]);

      const { result } = renderHook(
        () => useElectricalDataByUsinaAndDate(100, '2024-01-15'),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data).toBeDefined();
      expect(result.current.isSuccess).toBe(true);
    });

    it('should not fetch if usina id is missing', () => {
      const { result } = renderHook(
        () => useElectricalDataByUsinaAndDate(0, '2024-01-15'),
        { wrapper: createWrapper(queryClient) }
      );

      expect(result.current.isLoading).toBe(false);
    });
  });

  describe('useCreateElectricalData - Loading/Success/Error states', () => {
    it('should create electrical data successfully', async () => {
      mockEndpoint(
        'post',
        '/dados-eletricos',
        { id: 1, usinaId: 100, potenciaMW: 100, razaoEletrica: 50 },
        { status: 201 }
      );

      const { result } = renderHook(() => useCreateElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isPending).toBe(false);

      result.current.mutate({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle creation error state', async () => {
      mockErrorEndpoint('post', '/dados-eletricos', 400, 'Validation failed');

      const { result } = renderHook(() => useCreateElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useUpdateElectricalData - Loading/Success/Error states', () => {
    it('should update electrical data successfully', async () => {
      mockEndpoint('put', '/dados-eletricos/1', { id: 1, potenciaMW: 120 });

      const { result } = renderHook(() => useUpdateElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 1,
        dto: { potenciaMW: 120 },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle update error state', async () => {
      mockErrorEndpoint('put', '/dados-eletricos/999', 404, 'Not found');

      const { result } = renderHook(() => useUpdateElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({
        id: 999,
        dto: { potenciaMW: 120 },
      });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });

  describe('useDeleteElectricalData - Loading/Success/Error states', () => {
    it('should delete electrical data successfully', async () => {
      mockEndpoint('delete', '/dados-eletricos/1', {}, { status: 204 });

      const { result } = renderHook(() => useDeleteElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(1);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle deletion error state', async () => {
      mockErrorEndpoint('delete', '/dados-eletricos/999', 404, 'Not found');

      const { result } = renderHook(() => useDeleteElectricalData(), {
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

  describe('useBulkUpsertElectricalData - Loading/Success/Error states', () => {
    it('should bulk upsert 48 intervals successfully', async () => {
      const dados = Array.from({ length: 48 }, (_, i) => ({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: i + 1,
        potenciaMW: 100 + i,
        razaoEletrica: 50 + i,
      }));

      mockEndpoint(
        'post',
        '/dados-eletricos/bulk',
        dados.map((d, i) => ({ id: i + 1, ...d })),
        { status: 201 }
      );

      const { result } = renderHook(() => useBulkUpsertElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(dados);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data).toHaveLength(48);
      expect(result.current.isSuccess).toBe(true);
    });

    it('should handle bulk upsert error state', async () => {
      mockErrorEndpoint('post', '/dados-eletricos/bulk', 500, 'Server error');

      const dados = Array.from({ length: 48 }, (_, i) => ({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: i + 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      }));

      const { result } = renderHook(() => useBulkUpsertElectricalData(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate(dados);

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
      expect(result.current.isError).toBe(true);
    });
  });
});
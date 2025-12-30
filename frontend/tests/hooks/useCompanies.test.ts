import { describe, it, expect, beforeEach, vi } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { useCompanies, useCompanyById, useCreateCompany, useUpdateCompany, useDeleteCompany } from '../../src/hooks/useCompanies';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

// Create a query client for tests
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

describe('useCompanies Hook', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    server.resetHandlers();
  });

  describe('useCompanies', () => {
    it('should fetch all companies', async () => {
      const { result } = renderHook(() => useCompanies(), {
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
      mockErrorEndpoint('get', '/companies', 500, 'Server error');

      const { result } = renderHook(() => useCompanies(), {
        wrapper: createWrapper(queryClient),
      });

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('useCompanyById', () => {
    it('should fetch company by ID when ID is provided', async () => {
      mockEndpoint('get', '/companies/1', { id: 1, name: 'Company 1', code: 'C1' });

      const { result } = renderHook(() => useCompanyById(1), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.isLoading).toBe(true);

      await waitFor(() => {
        expect(result.current.isLoading).toBe(false);
      });

      expect(result.current.data?.id).toBe(1);
    });

    it('should not fetch when ID is undefined', () => {
      const { result } = renderHook(() => useCompanyById(undefined), {
        wrapper: createWrapper(queryClient),
      });

      expect(result.current.data).toBeUndefined();
    });
  });

  describe('useCreateCompany', () => {
    it('should create company', async () => {
      mockEndpoint('post', '/companies', { id: 2, name: 'New Company', code: 'NEW' }, { status: 201 });

      const { result } = renderHook(() => useCreateCompany(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ name: 'New Company', code: 'NEW' });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.id).toBe(2);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('post', '/companies', 400, 'Validation failed');

      const { result } = renderHook(() => useCreateCompany(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ name: '', code: '' });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.error).toBeDefined();
    });
  });

  describe('useUpdateCompany', () => {
    it('should update company', async () => {
      mockEndpoint('put', '/companies/1', { id: 1, name: 'Updated', code: 'C1' });

      const { result } = renderHook(() => useUpdateCompany(), {
        wrapper: createWrapper(queryClient),
      });

      result.current.mutate({ id: 1, dto: { name: 'Updated' } });

      await waitFor(() => {
        expect(result.current.isPending).toBe(false);
      });

      expect(result.current.data?.name).toBe('Updated');
    });
  });

  describe('useDeleteCompany', () => {
    it('should delete company', async () => {
      mockEndpoint('delete', '/companies/1', {}, { status: 204 });

      const { result } = renderHook(() => useDeleteCompany(), {
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

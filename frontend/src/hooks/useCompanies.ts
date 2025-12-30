/**
 * useCompanies Hook
 * 
 * React Query hook for managing company data
 * T012: Create useCompanies hook in frontend/src/hooks/useCompanies.ts
 *
 * Adjusted to use dominio PDP: Empresa (codigo/nome)
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { empresaService, type CreateEmpresaDto, type UpdateEmpresaDto, type Empresa } from '../services/empresaService';

const COMPANIES_QUERY_KEY = 'empresas';

/**
 * Hook to fetch all companies (Empresas)
 */
export function useCompanies() {
  return useQuery<Empresa[]>({
    queryKey: [COMPANIES_QUERY_KEY],
    queryFn: () => empresaService.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

/**
 * Hook to fetch single company by ID
 */
export function useCompanyById(id?: string) {
  return useQuery<Empresa | null>({
    queryKey: [COMPANIES_QUERY_KEY, id],
    queryFn: () => (id ? empresaService.getById(id) : null),
    enabled: !!id,
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Hook to create company
 */
export function useCreateCompany() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateEmpresaDto) => empresaService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [COMPANIES_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update company
 */
export function useUpdateCompany() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateEmpresaDto }) =>
      empresaService.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [COMPANIES_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete company
 */
export function useDeleteCompany() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => empresaService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [COMPANIES_QUERY_KEY] });
    },
  });
}

/**
 * Combined hook for company CRUD operations
 */
export function useCompanyData() {
  const query = useCompanies();
  const createMutation = useCreateCompany();
  const updateMutation = useUpdateCompany();
  const deleteMutation = useDeleteCompany();

  return {
    // Query data and state
    companies: (query.data as Empresa[]) || [],
    isLoading: query.isLoading,
    isFetching: query.isFetching,
    error: query.error,

    // Mutations
    create: createMutation.mutate,
    update: updateMutation.mutate,
    delete: deleteMutation.mutate,

    // Mutation states
    isCreating: createMutation.isPending,
    isUpdating: updateMutation.isPending,
    isDeleting: deleteMutation.isPending,

    // Refetch
    refetch: query.refetch,
  };
}

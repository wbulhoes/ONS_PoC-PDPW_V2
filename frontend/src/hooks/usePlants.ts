/**
 * usePlants Hook
 * 
 * React Query hook for managing plant (usina) data
 * T013: Create usePlants hook in frontend/src/hooks/usePlants.ts
 *
 * Adjusted to use dominio PDP: Usina (codigo/nome)
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { usinaService, type CreateUsinaDto, type UpdateUsinaDto, type Usina } from '../services/usinaService';

const PLANTS_QUERY_KEY = 'usinas';

/**
 * Hook to fetch all plants (Usinas)
 */
export function usePlants() {
  return useQuery<Usina[]>({
    queryKey: [PLANTS_QUERY_KEY],
    queryFn: () => usinaService.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

/**
 * Hook to fetch plants by company (empresa)
 */
export function usePlantsByCompany(empresaId?: string) {
  return useQuery<Usina[] | null>({
    queryKey: [PLANTS_QUERY_KEY, 'empresa', empresaId],
    queryFn: () => (empresaId ? usinaService.getByEmpresa(empresaId) : null),
    enabled: !!empresaId,
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Hook to fetch single plant by ID
 */
export function usePlantById(id?: string) {
  return useQuery<Usina | null>({
    queryKey: [PLANTS_QUERY_KEY, id],
    queryFn: () => (id ? usinaService.getById(id) : null),
    enabled: !!id,
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Hook to create plant
 */
export function useCreatePlant() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateUsinaDto) => usinaService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANTS_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update plant
 */
export function useUpdatePlant() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: string; dto: UpdateUsinaDto }) =>
      usinaService.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANTS_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete plant
 */
export function useDeletePlant() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => usinaService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANTS_QUERY_KEY] });
    },
  });
}

/**
 * Combined hook for plant CRUD operations
 */
export function usePlantData() {
  const query = usePlants();
  const createMutation = useCreatePlant();
  const updateMutation = useUpdatePlant();
  const deleteMutation = useDeletePlant();

  return {
    // Query data and state
    plants: (query.data as Usina[]) || [],
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

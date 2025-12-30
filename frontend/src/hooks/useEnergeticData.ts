/**
 * useEnergeticData Hook
 * 
 * React Query hook for managing energetic data (Razão Energética)
 * T028: Create React Query hooks for energetic data
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  energeticService,
  DadoEnergetico,
  CreateDadoEnergeticoDto,
  UpdateDadoEnergeticoDto,
} from '../services/energeticService';

const ENERGETIC_QUERY_KEY = 'energetic';

/**
 * Hook to fetch all energetic data
 */
export function useEnergeticData() {
  return useQuery({
    queryKey: [ENERGETIC_QUERY_KEY, 'all'],
    queryFn: () => energeticService.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

/**
 * Hook to fetch energetic data by period
 */
export function useEnergeticDataByPeriod(dataInicio: string, dataFim: string) {
  return useQuery({
    queryKey: [ENERGETIC_QUERY_KEY, 'period', dataInicio, dataFim],
    queryFn: () => energeticService.getByPeriod(dataInicio, dataFim),
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Hook to fetch energetic data by usina and date
 */
export function useEnergeticDataByUsinaAndDate(usinaId: number, dataReferencia: string) {
  return useQuery({
    queryKey: [ENERGETIC_QUERY_KEY, 'usina', usinaId, dataReferencia],
    queryFn: () => energeticService.getByUsinaAndDate(usinaId, dataReferencia),
    staleTime: 3 * 60 * 1000, // 3 minutes
  });
}

/**
 * Hook to create energetic data
 */
export function useCreateEnergeticData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateDadoEnergeticoDto) => energeticService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ENERGETIC_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update energetic data
 */
export function useUpdateEnergeticData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdateDadoEnergeticoDto }) =>
      energeticService.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ENERGETIC_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete energetic data
 */
export function useDeleteEnergeticData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => energeticService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ENERGETIC_QUERY_KEY] });
    },
  });
}

/**
 * Hook to bulk upsert energetic data (48 intervals at once)
 */
export function useBulkUpsertEnergeticData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dados: CreateDadoEnergeticoDto[]) => energeticService.bulkUpsert(dados),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ENERGETIC_QUERY_KEY] });
    },
  });
}

/**
 * Combined hook for energetic data CRUD operations with all states
 */
export function useEnergeticDataManager() {
  const queryAll = useEnergeticData();
  const createMutation = useCreateEnergeticData();
  const updateMutation = useUpdateEnergeticData();
  const deleteMutation = useDeleteEnergeticData();
  const bulkUpsertMutation = useBulkUpsertEnergeticData();

  return {
    // Query data and state
    data: queryAll.data || [],
    isLoading: queryAll.isLoading,
    isFetching: queryAll.isFetching,
    error: queryAll.error,

    // Mutations
    create: createMutation.mutate,
    update: updateMutation.mutate,
    delete: deleteMutation.mutate,
    bulkUpsert: bulkUpsertMutation.mutate,

    // Mutation states
    isCreating: createMutation.isPending,
    isUpdating: updateMutation.isPending,
    isDeleting: deleteMutation.isPending,
    isBulkUpserting: bulkUpsertMutation.isPending,

    // Refetch
    refetch: queryAll.refetch,
  };
}

/**
 * useElectricalData Hook
 * 
 * T043: Create React Query hooks for electrical data
 * React Query hook for managing electrical data (Razão Elétrica)
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import {
  electricalService,
  DadoEletrico,
  CreateDadoEletricoDto,
  UpdateDadoEletricoDto,
} from '../services/electricalService';

const ELECTRICAL_QUERY_KEY = 'electrical';

/**
 * Hook to fetch all electrical data
 */
export function useElectricalData() {
  return useQuery({
    queryKey: [ELECTRICAL_QUERY_KEY, 'all'],
    queryFn: () => electricalService.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

/**
 * Hook to fetch electrical data by period
 */
export function useElectricalDataByPeriod(dataInicio: string, dataFim: string) {
  return useQuery({
    queryKey: [ELECTRICAL_QUERY_KEY, 'period', dataInicio, dataFim],
    queryFn: () => electricalService.getByPeriod(dataInicio, dataFim),
    enabled: !!dataInicio && !!dataFim,
    staleTime: 5 * 60 * 1000,
  });
}

/**
 * Hook to fetch electrical data by usina and date
 */
export function useElectricalDataByUsinaAndDate(usinaId: number, dataReferencia: string) {
  return useQuery({
    queryKey: [ELECTRICAL_QUERY_KEY, 'usina', usinaId, dataReferencia],
    queryFn: () => electricalService.getByUsinaAndDate(usinaId, dataReferencia),
    enabled: !!usinaId && !!dataReferencia,
    staleTime: 3 * 60 * 1000, // 3 minutes
  });
}

/**
 * Hook to create electrical data
 */
export function useCreateElectricalData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateDadoEletricoDto) => electricalService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ELECTRICAL_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update electrical data
 */
export function useUpdateElectricalData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdateDadoEletricoDto }) =>
      electricalService.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ELECTRICAL_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete electrical data
 */
export function useDeleteElectricalData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => electricalService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ELECTRICAL_QUERY_KEY] });
    },
  });
}

/**
 * Hook to bulk upsert electrical data (48 intervals at once)
 */
export function useBulkUpsertElectricalData() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dados: CreateDadoEletricoDto[]) => electricalService.bulkUpsert(dados),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [ELECTRICAL_QUERY_KEY] });
    },
  });
}

/**
 * Combined hook for electrical data CRUD operations with all states
 */
export function useElectricalDataManager() {
  const queryAll = useElectricalData();
  const createMutation = useCreateElectricalData();
  const updateMutation = useUpdateElectricalData();
  const deleteMutation = useDeleteElectricalData();
  const bulkUpsertMutation = useBulkUpsertElectricalData();

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

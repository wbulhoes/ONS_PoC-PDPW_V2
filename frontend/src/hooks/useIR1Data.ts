/**
 * useIR1Data Hook
 * 
 * React Query hook for managing IR1 data (Nível de Partida)
 * T058: Create React Query hooks for IR1 data
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { ir1Service } from '../services/ir1Service';
import type { IR1Data, IR1Dto } from '../types/ir1';

const IR1_QUERY_KEY = 'ir1';

/**
 * Hook to fetch all IR1 data (Nível de Partida)
 */
export function useIR1Data() {
  return useQuery({
    queryKey: [IR1_QUERY_KEY, 'all'],
    queryFn: () => ir1Service.getAll(),
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
}

/**
 * Hook to fetch IR1 data by date (Nível de Partida)
 * @param dataReferencia - Data de referência em formato ISO (YYYY-MM-DD) ou YYYYMMDD
 */
export function useIR1DataByDate(dataReferencia: string) {
  return useQuery({
    queryKey: [IR1_QUERY_KEY, 'date', dataReferencia],
    queryFn: () => ir1Service.getByDate(dataReferencia),
    staleTime: 3 * 60 * 1000, // 3 minutes
    enabled: !!dataReferencia, // Only run if date is provided
  });
}

/**
 * Hook to create IR1 data (Nível de Partida)
 */
export function useCreateIR1Data() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: IR1Dto) => ir1Service.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [IR1_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update IR1 data (Nível de Partida)
 */
export function useUpdateIR1Data() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: Partial<IR1Dto> }) =>
      ir1Service.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [IR1_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete IR1 data (Nível de Partida)
 */
export function useDeleteIR1Data() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => ir1Service.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [IR1_QUERY_KEY] });
    },
  });
}

/**
 * Hook to bulk upsert IR1 data (multiple records at once)
 */
export function useBulkUpsertIR1Data() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dados: IR1Dto[]) => ir1Service.bulkUpsert(dados),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [IR1_QUERY_KEY] });
    },
  });
}

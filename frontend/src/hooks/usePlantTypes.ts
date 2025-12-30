/**
 * usePlantTypes Hook
 * 
 * React Query hook for managing plant type reference data
 * T014: Create usePlantTypes hook in frontend/src/hooks/usePlantTypes.ts
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { PlantType } from '../types/api';
import { plantTypeService, CreatePlantTypeDto, UpdatePlantTypeDto } from '../services/plantTypeService';

const PLANT_TYPES_QUERY_KEY = 'plantTypes';

/**
 * Hook to fetch all plant types
 */
export function usePlantTypes() {
  return useQuery({
    queryKey: [PLANT_TYPES_QUERY_KEY],
    queryFn: () => plantTypeService.getAll(),
    staleTime: 30 * 60 * 1000, // 30 minutes - rarely changes
  });
}

/**
 * Hook to fetch single plant type by ID
 */
export function usePlantTypeById(id?: number) {
  return useQuery({
    queryKey: [PLANT_TYPES_QUERY_KEY, id],
    queryFn: () => (id ? plantTypeService.getById(id) : null),
    enabled: !!id,
    staleTime: 30 * 60 * 1000,
  });
}

/**
 * Hook to create plant type
 */
export function useCreatePlantType() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreatePlantTypeDto) => plantTypeService.create(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANT_TYPES_QUERY_KEY] });
    },
  });
}

/**
 * Hook to update plant type
 */
export function useUpdatePlantType() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: UpdatePlantTypeDto }) =>
      plantTypeService.update(id, dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANT_TYPES_QUERY_KEY] });
    },
  });
}

/**
 * Hook to delete plant type
 */
export function useDeletePlantType() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => plantTypeService.delete(id),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [PLANT_TYPES_QUERY_KEY] });
    },
  });
}

/**
 * Combined hook for plant type operations
 */
export function usePlantTypeData() {
  const query = usePlantTypes();
  const createMutation = useCreatePlantType();
  const updateMutation = useUpdatePlantType();
  const deleteMutation = useDeletePlantType();

  return {
    // Query data and state
    plantTypes: query.data || [],
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

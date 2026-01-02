import { ContractedInflexibility } from '../types/contractedInflexibility';
import { apiClient } from './apiClient';

export const contractedInflexibilityService = {
  async getAll(): Promise<ContractedInflexibility[]> {
    return apiClient.get<ContractedInflexibility[]>('/contracted-inflexibilities');
  },

  async create(data: Omit<ContractedInflexibility, 'id'>): Promise<ContractedInflexibility> {
    return apiClient.post<ContractedInflexibility>('/contracted-inflexibilities', data);
  },

  async update(id: string, data: Partial<ContractedInflexibility>): Promise<ContractedInflexibility> {
    return apiClient.put<ContractedInflexibility>(`/contracted-inflexibilities/${id}`, data);
  },

  async delete(id: string): Promise<void> {
    return apiClient.delete(`/contracted-inflexibilities/${id}`);
  }
};

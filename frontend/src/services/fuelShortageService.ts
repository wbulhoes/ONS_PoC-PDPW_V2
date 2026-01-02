import { FuelShortageData } from '../types/fuelShortage';
import { apiClient } from './apiClient';

export const fuelShortageService = {
  async getData(date: string, companyId: string): Promise<FuelShortageData> {
    return apiClient.get<FuelShortageData>(`/fuel-shortage?date=${date}&companyId=${companyId}`);
  },

  async saveData(data: FuelShortageData, date: string, companyId: string): Promise<void> {
    return apiClient.post('/fuel-shortage', { date, companyId, data });
  }
};

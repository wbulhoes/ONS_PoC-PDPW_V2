import { FuelShortageData, FuelShortageInterval, FuelShortageUsina } from '../types/fuelShortage';

const generateIntervals = (): FuelShortageInterval[] => {
  const intervals: FuelShortageInterval[] = [];
  for (let i = 1; i <= 48; i++) {
    const hour = Math.floor((i - 1) / 2);
    const minute = (i - 1) % 2 === 0 ? '00' : '30';
    const nextHour = (i % 2 === 0) ? (hour + 1) : hour;
    const nextMinute = (i % 2 === 0) ? '00' : '30';
    
    // Format: HH:mm
    const label = `${hour.toString().padStart(2, '0')}:${minute}`;
    
    intervals.push({
      id: i,
      hora: label,
      valores: {},
      total: 0
    });
  }
  return intervals;
};

export const fuelShortageService = {
  getData: async (date: string, companyId: string): Promise<FuelShortageData> => {
    // Simulating API delay
    await new Promise((resolve) => setTimeout(resolve, 500));

    const usinas: FuelShortageUsina[] = [
      { codUsina: 'USINA1', nomeUsina: 'Usina A' },
      { codUsina: 'USINA2', nomeUsina: 'Usina B' },
      { codUsina: 'USINA3', nomeUsina: 'Usina C' },
    ];

    const intervalos = generateIntervals();

    // Populate with mock data
    intervalos.forEach(interval => {
      let rowTotal = 0;
      usinas.forEach(usina => {
        const val = Math.floor(Math.random() * 100);
        interval.valores[usina.codUsina] = val;
        rowTotal += val;
      });
      interval.total = rowTotal;
    });

    return {
      usinas,
      intervalos
    };
  },

  saveData: async (data: FuelShortageData, date: string, companyId: string): Promise<void> => {
    await new Promise((resolve) => setTimeout(resolve, 1000));
    console.log('Saving data for', date, companyId, data);
  }
};

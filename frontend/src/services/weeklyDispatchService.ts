import { WeeklyDispatchData, WeeklyDispatchUsina, PMOWeek } from '../types/weeklyDispatch';

export const weeklyDispatchService = {
  getData: async (codEmpresa: string): Promise<WeeklyDispatchData> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 500));

    const today = new Date();
    const nextWeek = new Date(today);
    nextWeek.setDate(today.getDate() + 7);

    const pmoConsulta: PMOWeek = {
      idAnoMes: 202310,
      idSemanaPmo: 1,
      semana: 'Semana 1',
      dataInicio: today.toISOString(),
      dataFim: new Date(today.getTime() + 5 * 24 * 60 * 60 * 1000).toISOString(),
      tipo: 'Consulta',
      dataLimiteEnvio: new Date(today.getTime() + 2 * 24 * 60 * 60 * 1000).toISOString(),
    };

    const pmoEdicao: PMOWeek = {
      idAnoMes: 202310,
      idSemanaPmo: 2,
      semana: 'Semana 2',
      dataInicio: nextWeek.toISOString(),
      dataFim: new Date(nextWeek.getTime() + 5 * 24 * 60 * 60 * 1000).toISOString(),
      tipo: 'Edicao',
      dataLimiteEnvio: new Date(nextWeek.getTime() + 2 * 24 * 60 * 60 * 1000).toISOString(),
    };

    // Mock usinas generation
    const usinas: WeeklyDispatchUsina[] = [];
    if (codEmpresa) {
      for (let i = 1; i <= 5; i++) {
        usinas.push({
          codUsina: `US${i.toString().padStart(3, '0')}`,
          nomeUsina: `Usina Térmica ${i}`,
          potenciaInstalada: 100 * i,
          cvu: Math.floor(Math.random() * 500),
          tempoUgeLigada: Math.floor(Math.random() * 24),
          tempoUgeDesligada: Math.floor(Math.random() * 24),
          geracaoMinima: Math.floor(Math.random() * 50),
          rampaSubidaQuente: Math.floor(Math.random() * 10),
          rampaSubidaMorno: Math.floor(Math.random() * 10),
          rampaSubidaFrio: Math.floor(Math.random() * 10),
          rampaDescida: Math.floor(Math.random() * 10),
        });
      }
    }

    return {
      pmoConsulta,
      pmoEdicao,
      usinas,
    };
  },

  saveData: async (data: WeeklyDispatchUsina[], pmo: PMOWeek, codEmpresa: string): Promise<void> => {
    // Mock implementation
    await new Promise((resolve) => setTimeout(resolve, 1000));
    console.log('Saved Weekly Dispatch data:', { pmo, codEmpresa, data });
  },
};

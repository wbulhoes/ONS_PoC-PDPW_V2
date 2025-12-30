/**
 * Tipos para Coleta de Potência Sincronizada
 */

/**
 * Representa um intervalo horário de potência sincronizada
 */
export interface PotenciaSincronizadaIntervalo {
  intervalo: number; // 1-24 (valores compactados de 48 meias-horas)
  horario: string; // "01:00", "02:00", etc.
  valPotSincronizadaSup: number; // Valor da potência sincronizada
}

/**
 * Estrutura completa de dados de potência sincronizada
 */
export interface DadosPotenciaSincronizada {
  dataPdp: string;
  codArea: string; // Sempre 'NE' (Nordeste)
  intervalos: PotenciaSincronizadaIntervalo[];
}

/**
 * Props para o formulário de coleta
 */
export interface SyncPowerFormData {
  dataPdp: string;
}

/**
 * Converte número do intervalo (1-24) para o formato de horário
 */
export function intervaloParaHorario(intervalo: number): string {
  return `${intervalo.toString().padStart(2, '0')}:00`;
}

/**
 * Gera array com todos os 24 intervalos
 */
export function gerarIntervalos(): PotenciaSincronizadaIntervalo[] {
  return Array.from({ length: 24 }, (_, index) => ({
    intervalo: index + 1,
    horario: intervaloParaHorario(index + 1),
    valPotSincronizadaSup: 0,
  }));
}

/**
 * Valida se um valor de potência é válido
 */
export function validarValorPotencia(valor: number): boolean {
  return !isNaN(valor) && valor >= 0;
}

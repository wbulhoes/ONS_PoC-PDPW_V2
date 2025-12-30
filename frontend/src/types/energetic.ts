/**
 * Tipos para Coleta de Dados Energéticos (Razão Energética Transformada)
 */

/**
 * Representa um intervalo (meia hora) de dados energéticos
 */
export interface RazaoEnergeticaIntervalo {
  intervalo: number; // 1-48
  horario: string; // "00:00-00:30", etc.
  valRazaoEnerTran: number; // Valor da razão energética transformada
}

/**
 * Dados energéticos completos de uma usina para um dia PDP
 */
export interface RazaoEnergeticaUsina {
  codUsina: string;
  intervalos: RazaoEnergeticaIntervalo[];
  total: number; // Soma dos 48 intervalos
  media: number; // Média dos 48 intervalos
}

/**
 * Estrutura completa de dados energéticos
 */
export interface DadosEnergeticosData {
  dataPdp: string;
  codEmpresa: string;
  usinas: RazaoEnergeticaUsina[];
  totaisPorIntervalo: TotalIntervalo[];
}

/**
 * Total de todas as usinas por intervalo
 */
export interface TotalIntervalo {
  intervalo: number;
  horario: string;
  total: number;
}

/**
 * Props para o formulário de coleta
 */
export interface EnergeticFormData {
  dataPdp: string;
  codEmpresa: string;
  codUsina: string; // Pode ser "TODAS" para editar todas
}

/**
 * Converte número do intervalo (1-48) para o formato de horário
 */
export function intervaloParaHorario(intervalo: number): string {
  const hora = Math.floor((intervalo - 1) / 2);
  const minuto = (intervalo - 1) % 2 === 0 ? '00' : '30';
  const horaFim = intervalo % 2 === 0 ? hora + 1 : hora;
  const minutoFim = intervalo % 2 === 0 ? '00' : '30';
  
  return `${hora.toString().padStart(2, '0')}:${minuto}-${horaFim.toString().padStart(2, '0')}:${minutoFim}`;
}

/**
 * Gera array com todos os 48 intervalos do dia
 */
export function gerarIntervalos(): RazaoEnergeticaIntervalo[] {
  return Array.from({ length: 48 }, (_, index) => ({
    intervalo: index + 1,
    horario: intervaloParaHorario(index + 1),
    valRazaoEnerTran: 0,
  }));
}

/**
 * Calcula total dos intervalos
 */
export function calcularTotal(intervalos: RazaoEnergeticaIntervalo[]): number {
  return intervalos.reduce((sum, int) => sum + int.valRazaoEnerTran, 0);
}

/**
 * Calcula média dos intervalos
 */
export function calcularMedia(intervalos: RazaoEnergeticaIntervalo[]): number {
  const total = calcularTotal(intervalos);
  return Math.floor(total / 48);
}

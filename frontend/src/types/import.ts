/**
 * Tipos para o módulo de Importação de Energia
 * Página legada: frmColImportacao.aspx
 */

export interface GridImportacao {
  colunas: ColunaImportacao[];
  intervalos: string[];
  totais: number[];
  medias: number[];
}

export interface ColunaImportacao {
  codUsina: string;
  label: string;
  ordem: number;
  valores: number[];
}

export interface ImportFormData {
  dataPdp: string;
  codEmpresa: string;
}

export interface OpcaoUsina {
  label: string;
  value: string;
}

/**
 * Gera array de 48 intervalos horários
 * Importação possui 48 intervalos de meia hora
 * @returns Array com 48 strings no formato "00:00-00:30", "00:30-01:00", etc.
 */
export function gerarIntervalos(): string[] {
  const intervalos: string[] = [];
  let hora = 0;
  let minuto = 0;

  for (let i = 0; i < 48; i++) {
    const horaFormatada = String(hora).padStart(2, '0');
    const minutoFormatado = String(minuto).padStart(2, '0');
    const proximoMinuto = minuto === 0 ? 30 : 0;
    const proximaHora = minuto === 30 ? hora + 1 : hora;

    const proximoMinutoFormatado = String(proximoMinuto).padStart(2, '0');
    const proximaHoraFormatada = String(proximaHora % 24).padStart(2, '0');

    intervalos.push(`${horaFormatada}:${minutoFormatado}-${proximaHoraFormatada}:${proximoMinutoFormatado}`);

    minuto = proximoMinuto;
    if (minuto === 0 && i < 47) {
      hora = proximaHora;
    }
  }

  return intervalos;
}

/**
 * Converte número de intervalo para formato de horário
 * @param intervalo Número do intervalo (1-48)
 * @returns String no formato "HH:MM-HH:MM"
 */
export function intervaloParaHorario(intervalo: number): string {
  const intervalos = gerarIntervalos();
  return intervalos[intervalo - 1] || '';
}

/**
 * Calcula o total de uma coluna
 * @param valores Array de números
 * @returns Soma de todos os valores
 */
export function calcularTotalColuna(valores: number[]): number {
  return valores.reduce((acc, val) => acc + val, 0);
}

/**
 * Calcula a média de uma coluna
 * @param valores Array de números
 * @returns Média dos valores (total / quantidade)
 */
export function calcularMediaColuna(valores: number[]): number {
  if (valores.length === 0) return 0;
  return calcularTotalColuna(valores) / valores.length;
}

/**
 * Realiza parse de string de usina para extrair código
 * @param value String do valor da usina (ex: "UHE_ABC")
 * @returns Código da usina
 */
export function parseUsinaValue(value: string): string {
  return value.trim();
}

/**
 * Converte array de valores para formato de textarea
 * Valores separados por ENTER (\n) para intervalo separado
 * Valores separados por TAB (\t) para usinas diferentes
 * @param valores Array bidimensional [intervalo][usina]
 * @returns String formatada para textarea
 */
export function formatarValoresParaTextarea(valores: number[][]): string {
  return valores
    .map((linha) => linha.join('\t'))
    .join('\n');
}

/**
 * Converte string de textarea para array de valores
 * @param textarea String do textarea
 * @returns Array bidimensional de números
 */
export function parseValoresDoTextarea(textarea: string): number[][] {
  return textarea
    .split('\n')
    .map((linha) =>
      linha
        .split('\t')
        .map((valor) => {
          const num = parseFloat(valor.trim());
          return isNaN(num) ? 0 : num;
        })
    );
}

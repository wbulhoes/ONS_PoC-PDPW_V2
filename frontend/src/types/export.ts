/**
 * Tipos para o módulo de Coleta de Exportação de Energia
 * 
 * Funcionalidade: Coleta de dados de exportação de usinas termelétricas
 * Características:
 * - 48 intervalos de meia hora (00:00-23:30)
 * - Edição por usina individual ou todas as usinas simultaneamente
 * - Grid dinâmico baseado em usinas da empresa
 * - Cálculos de total e média por intervalo
 */

/**
 * Intervalo de exportação (meia hora)
 */
export interface ExportacaoIntervalo {
  intervalo: number; // 1 a 48
  valorExportacao: number; // Valor de exportação em MW
}

/**
 * Dados de exportação para uma usina
 */
export interface DadosExportacao {
  codUsina: string; // Código da usina
  intervalos: ExportacaoIntervalo[];
}

/**
 * Coluna do grid de exportação (uma usina)
 */
export interface ColunaExportacao {
  label: string; // Código da usina
  codUsina: string;
  valores: number[]; // Array de 48 valores
  total: number;
  media: number;
}

/**
 * Grid completo de exportação
 */
export interface GridExportacao {
  dataPdp: string;
  codEmpresa: string;
  colunas: ColunaExportacao[];
  totaisLinha: number[]; // Total por intervalo (soma de todas colunas)
  totalGeral: number;
  mediaGeral: number;
}

/**
 * Formulário de busca de exportação
 */
export interface ExportFormData {
  dataPdp: string;
  codEmpresa: string;
  usinasCodigos?: string[]; // Array de códigos de usinas
}

/**
 * Opção de usina para o dropdown
 */
export interface OpcaoUsina {
  label: string;
  value: string; // Código da usina
}

/**
 * Resposta da API de exportação
 */
export interface ExportApiResponse {
  dataPdp: string;
  codEmpresa: string;
  usinas: DadosExportacao[];
}

/**
 * Gera array de 48 intervalos vazios
 */
export function gerarIntervalos(): ExportacaoIntervalo[] {
  return Array.from({ length: 48 }, (_, i) => ({
    intervalo: i + 1,
    valorExportacao: 0,
  }));
}

/**
 * Converte número de intervalo para horário (formato HH:MM-HH:MM)
 */
export function intervaloParaHorario(intervalo: number): string {
  const hora = Math.floor((intervalo - 1) / 2);
  const minuto = (intervalo - 1) % 2 === 0 ? '00' : '30';
  const horaFim = minuto === '00' ? hora : hora + 1;
  const minutoFim = minuto === '00' ? '30' : '00';
  
  return `${hora.toString().padStart(2, '0')}:${minuto}-${horaFim.toString().padStart(2, '0')}:${minutoFim}`;
}

/**
 * Calcula total de uma coluna
 */
export function calcularTotalColuna(valores: number[]): number {
  return valores.reduce((acc, val) => acc + val, 0);
}

/**
 * Calcula média de uma coluna
 */
export function calcularMediaColuna(valores: number[]): number {
  const total = calcularTotalColuna(valores);
  return Math.floor(total / valores.length);
}

/**
 * Valida valor de exportação (pode ser negativo, zero ou positivo)
 */
export function validarValorExportacao(valor: number): boolean {
  return !isNaN(valor) && isFinite(valor);
}

/**
 * Calcula total geral de todas as colunas
 */
export function calcularTotalGeral(colunas: ColunaExportacao[]): number {
  return colunas.reduce((sum, col) => sum + col.total, 0);
}

/**
 * Calcula média geral de todas as colunas
 */
export function calcularMediaGeral(colunas: ColunaExportacao[]): number {
  if (colunas.length === 0) return 0;
  const totalGeral = calcularTotalGeral(colunas);
  return Math.floor(totalGeral / (48 * colunas.length));
}

/**
 * Parse do valor do dropdown de usina selecionado
 */
export function parseUsinaValue(value: string): string | null {
  if (value === 'Todas as Usinas' || value === 'Selecione uma Usina' || !value) {
    return null;
  }
  return value;
}

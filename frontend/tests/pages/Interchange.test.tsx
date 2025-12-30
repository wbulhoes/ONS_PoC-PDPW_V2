import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Interchange from '../../src/pages/Collection/Interchange/Interchange';
import {
  GridIntercambio,
  InterchangeFormData,
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  validarValorIntercambio,
  formatarLabelColuna,
  parseIntercambioValue,
} from '../../src/types/interchange';

describe('Interchange Component', () => {
  const mockOnLoadData = vi.fn();
  const mockOnSave = vi.fn();

  const mockGridData: GridIntercambio = {
    dataPdp: '15/01/2025',
    codEmpresa: 'SE',
    colunas: [
      {
        label: 'SE-NE/01',
        definicao: {
          codEmpresDe: 'SE',
          codEmpresPara: 'NE',
          codContaDe: '01',
          codContaPara: '02',
          codContaModal: '01',
          tipoIntercambio: 'SE-NE/01',
        },
        valores: Array(48).fill(100),
        total: 4800,
        media: 100,
      },
      {
        label: 'SE-SU/02',
        definicao: {
          codEmpresDe: 'SE',
          codEmpresPara: 'SU',
          codContaDe: '01',
          codContaPara: '02',
          codContaModal: '02',
          tipoIntercambio: 'SE-SU/02',
        },
        valores: Array(48).fill(50),
        total: 2400,
        media: 50,
      },
    ],
    totaisLinha: Array(48).fill(150),
    totalGeral: 7200,
    mediaGeral: 150,
  };

  beforeEach(() => {
    mockOnLoadData.mockClear();
    mockOnSave.mockClear();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByText('Intercâmbio')).toBeInTheDocument();
    });

    it('deve renderizar os campos do formulário', () => {
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByLabelText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByTestId('radio-modalidade')).toBeInTheDocument();
      expect(screen.getByTestId('radio-empresa')).toBeInTheDocument();
    });

    it('deve iniciar com modalidade "Por Modalidade" selecionada', () => {
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const radioModalidade = screen.getByTestId('radio-modalidade') as HTMLInputElement;
      expect(radioModalidade.checked).toBe(true);
    });

    it('não deve mostrar grid inicialmente', () => {
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.queryByTestId('table-intercambio')).not.toBeInTheDocument();
    });

    it('deve ter dropdown de intercâmbio desabilitado inicialmente', () => {
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const selectIntercambio = screen.getByTestId('select-intercambio') as HTMLSelectElement;
      expect(selectIntercambio.disabled).toBe(true);
    });
  });

  describe('Interação com Formulário', () => {
    it('deve permitir selecionar data', async () => {
      const user = userEvent.setup();
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp') as HTMLSelectElement;
      await user.selectOptions(dataSelect, '15/01/2025');

      expect(dataSelect.value).toBe('15/01/2025');
    });

    it('deve permitir selecionar empresa', async () => {
      const user = userEvent.setup();
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const empresaSelect = screen.getByTestId('select-empresa') as HTMLSelectElement;
      await user.selectOptions(empresaSelect, 'SE');

      expect(empresaSelect.value).toBe('SE');
    });

    it('deve permitir alternar modo de visualização', async () => {
      const user = userEvent.setup();
      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const radioEmpresa = screen.getByTestId('radio-empresa');
      await user.click(radioEmpresa);

      expect(radioEmpresa).toBeChecked();
    });

    it('deve chamar onLoadData quando data e empresa são selecionados', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledWith({
          dataPdp: '15/01/2025',
          codEmpresa: 'SE',
          modoVisualizacao: 'modalidade',
        });
      });
    });
  });

  describe('Carregamento de Dados', () => {
    it('deve exibir loading durante carregamento', async () => {
      mockOnLoadData.mockImplementation(() => new Promise((resolve) => setTimeout(resolve, 100)));
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      expect(screen.getByTestId('loading')).toBeInTheDocument();
    });

    it('deve exibir erro quando carregamento falha', async () => {
      mockOnLoadData.mockRejectedValue(new Error('Erro ao carregar'));
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByText(/Não foi possível acessar a Base de Dados/i)).toBeInTheDocument();
      });
    });

    it('deve exibir grid quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-intercambio')).toBeInTheDocument();
      });
    });

    it('deve exibir 48 linhas de intervalos', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        const rows = screen.getAllByTestId(/^row-intervalo-/);
        expect(rows).toHaveLength(48);
      });
    });

    it('deve exibir colunas de intercâmbio', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('header-coluna-0')).toBeInTheDocument();
        expect(screen.getByTestId('header-coluna-1')).toBeInTheDocument();
      });
    });

    it('deve habilitar dropdown de intercâmbio quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        const selectIntercambio = screen.getByTestId('select-intercambio') as HTMLSelectElement;
        expect(selectIntercambio.disabled).toBe(false);
      });
    });
  });

  describe('Estado Vazio', () => {
    it('deve exibir mensagem quando não há dados', async () => {
      const gridVazio: GridIntercambio = {
        dataPdp: '15/01/2025',
        codEmpresa: 'SE',
        colunas: [],
        totaisLinha: [],
        totalGeral: 0,
        mediaGeral: 0,
      };
      mockOnLoadData.mockResolvedValue(gridVazio);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('empty-state')).toBeInTheDocument();
      });
    });
  });

  describe('Edição de Dados', () => {
    it('deve mostrar textarea quando intercâmbio é selecionado', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-intercambio')).toBeInTheDocument();
      });

      const selectIntercambio = screen.getByTestId('select-intercambio');
      await user.selectOptions(selectIntercambio, 'SE|NE|01|02|01');

      await waitFor(() => {
        expect(screen.getByTestId('textarea-valores')).toBeInTheDocument();
      });
    });

    it('deve mostrar botão salvar quando textarea está visível', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Interchange onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-intercambio')).toBeInTheDocument();
      });

      const selectIntercambio = screen.getByTestId('select-intercambio');
      await user.selectOptions(selectIntercambio, 'SE|NE|01|02|01');

      await waitFor(() => {
        expect(screen.getByTestId('btn-save')).toBeInTheDocument();
      });
    });
  });
});

describe('Interchange Helper Functions', () => {
  describe('gerarIntervalos', () => {
    it('deve gerar 48 intervalos', () => {
      const intervalos = gerarIntervalos();
      expect(intervalos).toHaveLength(48);
    });

    it('deve iniciar intervalos com valor zero', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach((int) => {
        expect(int.valorIntercambio).toBe(0);
      });
    });

    it('deve ter intervalos numerados de 1 a 48', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach((int, idx) => {
        expect(int.intervalo).toBe(idx + 1);
      });
    });
  });

  describe('intervaloParaHorario', () => {
    it('deve converter intervalo 1 para 00:00-00:30', () => {
      expect(intervaloParaHorario(1)).toBe('00:00-00:30');
    });

    it('deve converter intervalo 2 para 00:30-01:00', () => {
      expect(intervaloParaHorario(2)).toBe('00:30-01:00');
    });

    it('deve converter intervalo 48 para 23:30-24:00', () => {
      expect(intervaloParaHorario(48)).toBe('23:30-24:00');
    });
  });

  describe('calcularTotalColuna', () => {
    it('deve calcular total corretamente', () => {
      const valores = [10, 20, 30, 40];
      expect(calcularTotalColuna(valores)).toBe(100);
    });

    it('deve retornar zero para array vazio', () => {
      expect(calcularTotalColuna([])).toBe(0);
    });
  });

  describe('calcularMediaColuna', () => {
    it('deve calcular média corretamente', () => {
      const valores = [10, 20, 30, 40];
      expect(calcularMediaColuna(valores)).toBe(25);
    });

    it('deve arredondar para baixo', () => {
      const valores = [10, 15, 20];
      expect(calcularMediaColuna(valores)).toBe(15);
    });
  });

  describe('validarValorIntercambio', () => {
    it('deve validar valor positivo', () => {
      expect(validarValorIntercambio(100)).toBe(true);
    });

    it('deve validar valor zero', () => {
      expect(validarValorIntercambio(0)).toBe(true);
    });

    it('deve validar valor negativo', () => {
      expect(validarValorIntercambio(-10)).toBe(true);
    });

    it('deve invalidar NaN', () => {
      expect(validarValorIntercambio(NaN)).toBe(false);
    });

    it('deve invalidar Infinity', () => {
      expect(validarValorIntercambio(Infinity)).toBe(false);
    });
  });

  describe('formatarLabelColuna', () => {
    it('deve formatar label por modalidade', () => {
      const def = {
        codEmpresDe: 'SE',
        codEmpresPara: 'NE',
        codContaDe: '01',
        codContaPara: '02',
        codContaModal: '01',
        tipoIntercambio: '',
      };
      expect(formatarLabelColuna(def, 'modalidade')).toBe('SE-NE/01');
    });

    it('deve formatar label por empresa', () => {
      const def = {
        codEmpresDe: 'SE',
        codEmpresPara: 'NE',
        codContaDe: '01',
        codContaPara: '02',
        codContaModal: '01',
        tipoIntercambio: '',
      };
      expect(formatarLabelColuna(def, 'empresa')).toBe('SE - NE');
    });
  });

  describe('parseIntercambioValue', () => {
    it('deve fazer parse de valor válido', () => {
      const result = parseIntercambioValue('SE|NE|01|02|03');
      expect(result).toEqual({
        codEmpresDe: 'SE',
        codEmpresPara: 'NE',
        codContaDe: '01',
        codContaPara: '02',
        codContaModal: '03',
        tipoIntercambio: '',
      });
    });

    it('deve retornar null para "Todos"', () => {
      expect(parseIntercambioValue('Todos')).toBeNull();
    });

    it('deve retornar null para valor inválido', () => {
      expect(parseIntercambioValue('SE|NE|01')).toBeNull();
    });
  });
});

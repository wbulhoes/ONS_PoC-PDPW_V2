import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Export from '../../src/pages/Collection/Thermal/Export';
import {
  GridExportacao,
  ExportFormData,
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  validarValorExportacao,
  calcularTotalGeral,
  calcularMediaGeral,
  parseUsinaValue,
} from '../../src/types/export';

describe('Export Component', () => {
  const mockOnLoadData = vi.fn();
  const mockOnSave = vi.fn();

  const mockGridData: GridExportacao = {
    dataPdp: '15/01/2025',
    codEmpresa: 'SE',
    colunas: [
      {
        label: 'UH001',
        codUsina: 'UH001',
        valores: Array(48).fill(200),
        total: 9600,
        media: 200,
      },
      {
        label: 'UH002',
        codUsina: 'UH002',
        valores: Array(48).fill(150),
        total: 7200,
        media: 150,
      },
    ],
    totaisLinha: Array(48).fill(350),
    totalGeral: 16800,
    mediaGeral: 350,
  };

  beforeEach(() => {
    mockOnLoadData.mockClear();
    mockOnSave.mockClear();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByText('Exportação')).toBeInTheDocument();
    });

    it('deve renderizar os campos do formulário', () => {
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByLabelText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Usinas:/i)).toBeInTheDocument();
    });

    it('não deve mostrar grid inicialmente', () => {
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.queryByTestId('table-exportacao')).not.toBeInTheDocument();
    });

    it('deve ter dropdown de usina desabilitado inicialmente', () => {
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const selectUsina = screen.getByTestId('select-usina') as HTMLSelectElement;
      expect(selectUsina.disabled).toBe(true);
    });
  });

  describe('Interação com Formulário', () => {
    it('deve permitir selecionar data', async () => {
      const user = userEvent.setup();
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp') as HTMLSelectElement;
      await user.selectOptions(dataSelect, '15/01/2025');

      expect(dataSelect.value).toBe('15/01/2025');
    });

    it('deve permitir selecionar empresa', async () => {
      const user = userEvent.setup();
      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const empresaSelect = screen.getByTestId('select-empresa') as HTMLSelectElement;
      await user.selectOptions(empresaSelect, 'SE');

      expect(empresaSelect.value).toBe('SE');
    });

    it('deve chamar onLoadData quando data e empresa são selecionados', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledWith({
          dataPdp: '15/01/2025',
          codEmpresa: 'SE',
        });
      });
    });
  });

  describe('Carregamento de Dados', () => {
    it('deve exibir loading durante carregamento', async () => {
      mockOnLoadData.mockImplementation(() => new Promise((resolve) => setTimeout(resolve, 100)));
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      expect(screen.getByTestId('loading')).toBeInTheDocument();
    });

    it('deve exibir erro quando carregamento falha', async () => {
      mockOnLoadData.mockRejectedValue(new Error('Erro ao carregar'));
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

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

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-exportacao')).toBeInTheDocument();
      });
    });

    it('deve exibir 48 linhas de intervalos', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        const rows = screen.getAllByTestId(/^row-intervalo-/);
        expect(rows).toHaveLength(48);
      });
    });

    it('deve exibir colunas de usinas', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('header-usina-0')).toBeInTheDocument();
        expect(screen.getByTestId('header-usina-1')).toBeInTheDocument();
      });
    });

    it('deve habilitar dropdown de usina quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        const selectUsina = screen.getByTestId('select-usina') as HTMLSelectElement;
        expect(selectUsina.disabled).toBe(false);
      });
    });
  });

  describe('Estado Vazio', () => {
    it('deve exibir mensagem quando não há dados', async () => {
      const gridVazio: GridExportacao = {
        dataPdp: '15/01/2025',
        codEmpresa: 'SE',
        colunas: [],
        totaisLinha: [],
        totalGeral: 0,
        mediaGeral: 0,
      };
      mockOnLoadData.mockResolvedValue(gridVazio);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

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
    it('deve mostrar textarea quando usina é selecionada', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-exportacao')).toBeInTheDocument();
      });

      const selectUsina = screen.getByTestId('select-usina');
      await user.selectOptions(selectUsina, 'UH001');

      await waitFor(() => {
        expect(screen.getByTestId('textarea-valores')).toBeInTheDocument();
      });
    });

    it('deve mostrar botão salvar quando textarea está visível', async () => {
      mockOnLoadData.mockResolvedValue(mockGridData);
      const user = userEvent.setup();

      render(<Export onLoadData={mockOnLoadData} onSave={mockOnSave} />);

      const dataSelect = screen.getByTestId('select-data-pdp');
      const empresaSelect = screen.getByTestId('select-empresa');

      await user.selectOptions(dataSelect, '15/01/2025');
      await user.selectOptions(empresaSelect, 'SE');

      await waitFor(() => {
        expect(screen.getByTestId('table-exportacao')).toBeInTheDocument();
      });

      const selectUsina = screen.getByTestId('select-usina');
      await user.selectOptions(selectUsina, 'UH001');

      await waitFor(() => {
        expect(screen.getByTestId('btn-save')).toBeInTheDocument();
      });
    });
  });
});

describe('Export Helper Functions', () => {
  describe('gerarIntervalos', () => {
    it('deve gerar 48 intervalos', () => {
      const intervalos = gerarIntervalos();
      expect(intervalos).toHaveLength(48);
    });

    it('deve iniciar intervalos com valor zero', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach((int) => {
        expect(int.valorExportacao).toBe(0);
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

  describe('validarValorExportacao', () => {
    it('deve validar valor positivo', () => {
      expect(validarValorExportacao(100)).toBe(true);
    });

    it('deve validar valor zero', () => {
      expect(validarValorExportacao(0)).toBe(true);
    });

    it('deve validar valor negativo', () => {
      expect(validarValorExportacao(-10)).toBe(true);
    });

    it('deve invalidar NaN', () => {
      expect(validarValorExportacao(NaN)).toBe(false);
    });

    it('deve invalidar Infinity', () => {
      expect(validarValorExportacao(Infinity)).toBe(false);
    });
  });

  describe('calcularTotalGeral', () => {
    it('deve calcular total geral de todas as colunas', () => {
      const colunas = [
        {
          label: 'UH001',
          codUsina: 'UH001',
          valores: Array(48).fill(100),
          total: 4800,
          media: 100,
        },
        {
          label: 'UH002',
          codUsina: 'UH002',
          valores: Array(48).fill(50),
          total: 2400,
          media: 50,
        },
      ];
      expect(calcularTotalGeral(colunas)).toBe(7200);
    });
  });

  describe('calcularMediaGeral', () => {
    it('deve calcular média geral de todas as colunas', () => {
      const colunas = [
        {
          label: 'UH001',
          codUsina: 'UH001',
          valores: Array(48).fill(100),
          total: 4800,
          media: 100,
        },
        {
          label: 'UH002',
          codUsina: 'UH002',
          valores: Array(48).fill(50),
          total: 2400,
          media: 50,
        },
      ];
      expect(calcularMediaGeral(colunas)).toBe(75);
    });

    it('deve retornar zero para array vazio', () => {
      expect(calcularMediaGeral([])).toBe(0);
    });
  });

  describe('parseUsinaValue', () => {
    it('deve fazer parse de valor válido', () => {
      expect(parseUsinaValue('UH001')).toBe('UH001');
    });

    it('deve retornar null para "Todas as Usinas"', () => {
      expect(parseUsinaValue('Todas as Usinas')).toBeNull();
    });

    it('deve retornar null para "Selecione uma Usina"', () => {
      expect(parseUsinaValue('Selecione uma Usina')).toBeNull();
    });

    it('deve retornar null para valor vazio', () => {
      expect(parseUsinaValue('')).toBeNull();
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import SyncPower from '../../src/pages/Collection/Electrical/SyncPower';
import {
  DadosPotenciaSincronizada,
  SyncPowerFormData,
  gerarIntervalos,
  intervaloParaHorario,
  validarValorPotencia,
} from '../../src/types/syncPower';

describe('SyncPower Component', () => {
  const mockOnLoadData = vi.fn();
  const mockOnSave = vi.fn();

  const mockData: DadosPotenciaSincronizada = {
    dataPdp: '15/01/2025',
    codArea: 'NE',
    intervalos: gerarIntervalos(),
  };

  beforeEach(() => {
    mockOnLoadData.mockClear();
    mockOnSave.mockClear();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByText('Potência Sincronizada')).toBeInTheDocument();
    });

    it('deve renderizar o campo de data', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByLabelText(/Data:/i)).toBeInTheDocument();
    });

    it('deve renderizar botão fechar', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByTestId('btn-close')).toBeInTheDocument();
    });

    it('deve iniciar com select de data vazio', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const dataSelect = screen.getByTestId('select-data-pdp') as HTMLSelectElement;
      expect(dataSelect.value).toBe('');
    });

    it('não deve mostrar tabela inicialmente', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.queryByTestId('table-potencia')).not.toBeInTheDocument();
    });

    it('não deve mostrar botão salvar inicialmente', () => {
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.queryByTestId('btn-save')).not.toBeInTheDocument();
    });
  });

  describe('Interação com Formulário', () => {
    it('deve permitir selecionar data', async () => {
      const user = userEvent.setup();
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp') as HTMLSelectElement;
      await user.selectOptions(dataSelect, '15/01/2025');
      
      expect(dataSelect.value).toBe('15/01/2025');
    });

    it('deve chamar onLoadData quando data é selecionada', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledWith({
          dataPdp: '15/01/2025',
        });
      });
    });
  });

  describe('Carregamento de Dados', () => {
    it('deve exibir loading durante carregamento', async () => {
      mockOnLoadData.mockImplementation(() => new Promise(resolve => setTimeout(resolve, 100)));
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      expect(screen.getByTestId('loading')).toBeInTheDocument();
    });

    it('deve exibir erro quando carregamento falha', async () => {
      mockOnLoadData.mockRejectedValue(new Error('Erro ao carregar'));
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByText(/Não foi possível acessar a Base de Dados/i)).toBeInTheDocument();
      });
    });

    it('deve exibir tabela quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByTestId('table-potencia')).toBeInTheDocument();
      });
    });

    it('deve exibir 24 linhas de intervalos', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        const rows = screen.getAllByTestId(/^row-intervalo-/);
        expect(rows).toHaveLength(24);
      });
    });

    it('deve exibir textarea quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByTestId('textarea-valores')).toBeInTheDocument();
      });
    });

    it('deve exibir botão salvar quando dados são carregados', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByTestId('btn-save')).toBeInTheDocument();
      });
    });
  });

  describe('Estado Vazio', () => {
    it('deve exibir mensagem quando não há dados', async () => {
      mockOnLoadData.mockResolvedValue(null);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByTestId('empty-state')).toBeInTheDocument();
      });
    });
  });

  describe('Edição de Dados', () => {
    it('deve permitir editar valores no textarea', async () => {
      mockOnLoadData.mockResolvedValue(mockData);
      const user = userEvent.setup();
      
      render(<SyncPower onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const dataSelect = screen.getByTestId('select-data-pdp');
      await user.selectOptions(dataSelect, '15/01/2025');
      
      await waitFor(() => {
        expect(screen.getByTestId('textarea-valores')).toBeInTheDocument();
      });
      
      const textarea = screen.getByTestId('textarea-valores') as HTMLTextAreaElement;
      await user.clear(textarea);
      await user.type(textarea, '100\n200\n300');
      
      expect(textarea.value).toContain('100');
      expect(textarea.value).toContain('200');
      expect(textarea.value).toContain('300');
    });
  });
});

describe('SyncPower Helper Functions', () => {
  describe('gerarIntervalos', () => {
    it('deve gerar 24 intervalos', () => {
      const intervalos = gerarIntervalos();
      expect(intervalos).toHaveLength(24);
    });

    it('deve iniciar intervalos com valor zero', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach(int => {
        expect(int.valPotSincronizadaSup).toBe(0);
      });
    });

    it('deve ter intervalos numerados de 1 a 24', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach((int, idx) => {
        expect(int.intervalo).toBe(idx + 1);
      });
    });
  });

  describe('intervaloParaHorario', () => {
    it('deve converter intervalo 1 para 01:00', () => {
      expect(intervaloParaHorario(1)).toBe('01:00');
    });

    it('deve converter intervalo 12 para 12:00', () => {
      expect(intervaloParaHorario(12)).toBe('12:00');
    });

    it('deve converter intervalo 24 para 24:00', () => {
      expect(intervaloParaHorario(24)).toBe('24:00');
    });
  });

  describe('validarValorPotencia', () => {
    it('deve validar valor positivo', () => {
      expect(validarValorPotencia(100)).toBe(true);
    });

    it('deve validar valor zero', () => {
      expect(validarValorPotencia(0)).toBe(true);
    });

    it('deve invalidar valor negativo', () => {
      expect(validarValorPotencia(-10)).toBe(false);
    });

    it('deve invalidar NaN', () => {
      expect(validarValorPotencia(NaN)).toBe(false);
    });
  });
});

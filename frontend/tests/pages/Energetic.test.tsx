import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Energetic from '../../src/pages/Collection/Energetic/Energetic';
import {
  DadosEnergeticosData,
  gerarIntervalos,
  calcularTotal,
  calcularMedia,
  intervaloParaHorario,
} from '../../src/types/energetic';
import { useCompanies } from '../../src/hooks/useCompanies';
import { usePlantsByCompany } from '../../src/hooks/usePlants';
import { useEnergeticDataByPeriod, useBulkUpsertEnergeticData } from '../../src/hooks/useEnergeticData';

vi.mock('../../src/hooks/useCompanies');
vi.mock('../../src/hooks/usePlants');
vi.mock('../../src/hooks/useEnergeticData');

const mockUseCompanies = useCompanies as unknown as vi.Mock;
const mockUsePlantsByCompany = usePlantsByCompany as unknown as vi.Mock;
const mockUseEnergeticDataByPeriod = useEnergeticDataByPeriod as unknown as vi.Mock;
const mockUseBulkUpsertEnergeticData = useBulkUpsertEnergeticData as unknown as vi.Mock;

describe('Energetic Component', () => {
  const mockCompanies = [{ id: 1, codigo: 'EMP001', nome: 'Empresa 1' }];
  const mockPlants = [{ id: 10, codigo: 'UHE001', nome: 'Usina 1' }];
  const mockEnergeticData = [
    {
      id: 1,
      usinaId: 10,
      dataReferencia: '2025-01-01',
      intervalo: 1,
      valorMW: 10,
      razaoEnergetica: 10,
      observacao: '',
    },
  ];

  beforeEach(() => {
    const bulkMutate = vi.fn();

    mockUseCompanies.mockReturnValue({ data: mockCompanies, isLoading: false });
    mockUsePlantsByCompany.mockImplementation((companyId: number) => ({
      data: companyId ? mockPlants : [],
      isLoading: false,
    }));
    mockUseEnergeticDataByPeriod.mockReturnValue({
      data: mockEnergeticData,
      isLoading: false,
      error: null,
      refetch: vi.fn(),
    });
    mockUseBulkUpsertEnergeticData.mockReturnValue({ mutate: bulkMutate, isPending: false, error: null });
  });

  const selectFirstDate = async () => {
    const user = userEvent.setup();
    const select = screen.getByLabelText(/Data PDP:/i) as HTMLSelectElement;
    const option = Array.from(select.options).find(opt => opt.value);
    if (option) {
      await user.selectOptions(select, option.value);
      return option.value;
    }
    return '';
  };

  const selectCompany = async (value: string) => {
    const user = userEvent.setup();
    const select = screen.getByLabelText(/Empresa:/i) as HTMLSelectElement;
    await user.selectOptions(select, value);
  };

  const selectPlant = async (value: string) => {
    const user = userEvent.setup();
    const select = screen.getByLabelText(/Usinas:/i) as HTMLSelectElement;
    await user.selectOptions(select, value);
  };

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Energetic />);
      expect(screen.getByText('Razão Energética Transformada')).toBeInTheDocument();
    });

    it('deve renderizar os campos do formulário', () => {
      render(<Energetic />);
      
      expect(screen.getByLabelText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/Usinas:/i)).toBeInTheDocument();
    });

    it('não deve mostrar tabela inicialmente', () => {
      render(<Energetic />);
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
    });
  });

  describe('Interação com Formulário', () => {
    it('deve permitir selecionar data e empresa', async () => {
      render(<Energetic />);
      const selectedDate = await selectFirstDate();
      await selectCompany('EMP001');

      expect((screen.getByLabelText(/Data PDP:/i) as HTMLSelectElement).value).toBe(selectedDate);
      expect((screen.getByLabelText(/Empresa:/i) as HTMLSelectElement).value).toBe('EMP001');
    });
  });

  describe('Carregamento de Dados', () => {
    it('deve carregar dados e exibir tabela', async () => {
      render(<Energetic />);

      await selectFirstDate();
      await selectCompany('EMP001');

      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem de erro quando hook retorna erro', async () => {
      mockUseEnergeticDataByPeriod.mockReturnValue({
        data: undefined,
        isLoading: false,
        error: new Error('Erro ao carregar'),
        refetch: vi.fn(),
      });

      render(<Energetic />);
      await selectFirstDate();
      await selectCompany('EMP001');

      await waitFor(() => {
        expect(screen.getByText(/Não foi possível carregar os dados/i)).toBeInTheDocument();
      });
    });

    it('deve mostrar loading quando hooks estão carregando', () => {
      mockUseCompanies.mockReturnValue({ data: [], isLoading: true });

      render(<Energetic />);
      expect(screen.getByText(/Carregando dados.../i)).toBeInTheDocument();
    });
  });

  describe('Visualização de Dados', () => {
    it('deve exibir tabela com colunas de usinas', async () => {
      render(<Energetic />);

      await selectFirstDate();
      await selectCompany('EMP001');

      await waitFor(() => {
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
        expect(screen.getAllByText('UHE001').length).toBeGreaterThanOrEqual(1);
      });
    });
  });

  describe('Salvar dados', () => {
    it('deve chamar mutate ao salvar', async () => {
      const mutateSpy = vi.fn();
      mockUseBulkUpsertEnergeticData.mockReturnValue({ mutate: mutateSpy, isPending: false, error: null });

      render(<Energetic />);

      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');

      await waitFor(() => {
        expect(screen.getByRole('button', { name: /salvar/i })).toBeInTheDocument();
      });

      const user = userEvent.setup();
      await user.click(screen.getByRole('button', { name: /salvar/i }));

      expect(mutateSpy).toHaveBeenCalled();
    });
  });
});

describe('Energetic Helper Functions', () => {
  describe('gerarIntervalos', () => {
    it('deve gerar 48 intervalos', () => {
      const intervalos = gerarIntervalos();
      expect(intervalos).toHaveLength(48);
    });

    it('deve iniciar intervalos com valor zero', () => {
      const intervalos = gerarIntervalos();
      intervalos.forEach(int => {
        expect(int.valRazaoEnerTran).toBe(0);
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

  describe('calcularTotal', () => {
    it('deve calcular total de intervalos zerados', () => {
      const intervalos = gerarIntervalos();
      expect(calcularTotal(intervalos)).toBe(0);
    });

    it('deve calcular total com valores', () => {
      const intervalos = gerarIntervalos().map(int => ({
        ...int,
        valRazaoEnerTran: 10,
      }));
      expect(calcularTotal(intervalos)).toBe(480); // 10 * 48
    });
  });

  describe('calcularMedia', () => {
    it('deve calcular média de intervalos zerados', () => {
      const intervalos = gerarIntervalos();
      expect(calcularMedia(intervalos)).toBe(0);
    });

    it('deve calcular média com valores', () => {
      const intervalos = gerarIntervalos().map(int => ({
        ...int,
        valRazaoEnerTran: 48,
      }));
      expect(calcularMedia(intervalos)).toBe(48); // floor(48*48/48) = 48
    });

    it('deve arredondar média para baixo', () => {
      const intervalos = gerarIntervalos().map((int, idx) => ({
        ...int,
        valRazaoEnerTran: idx % 2 === 0 ? 10 : 11,
      }));
      const total = calcularTotal(intervalos);
      expect(calcularMedia(intervalos)).toBe(Math.floor(total / 48));
    });
  });
});

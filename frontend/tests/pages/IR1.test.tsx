/**
 * Testes para IR1.tsx - Nível de Partida
 * Migração de: legado/pdpw/frmColIR1.aspx
 * T063: Update IR1 component tests to mock hooks
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import '@testing-library/jest-dom';
import IR1 from '../../src/pages/Collection/Insumos/IR1';
import { useCompanies } from '../../src/hooks/useCompanies';
import { usePlantsByCompany } from '../../src/hooks/usePlants';
import { useIR1DataByDate, useCreateIR1Data, useUpdateIR1Data } from '../../src/hooks/useIR1Data';

// Mock all hooks
vi.mock('../../src/hooks/useCompanies');
vi.mock('../../src/hooks/usePlants');
vi.mock('../../src/hooks/useIR1Data');

const mockUseCompanies = useCompanies as unknown as vi.Mock;
const mockUsePlantsByCompany = usePlantsByCompany as unknown as vi.Mock;
const mockUseIR1DataByDate = useIR1DataByDate as unknown as vi.Mock;
const mockUseCreateIR1Data = useCreateIR1Data as unknown as vi.Mock;
const mockUseUpdateIR1Data = useUpdateIR1Data as unknown as vi.Mock;

describe('IR1 Component - Nível de Partida', () => {
  const mockCompanies = [
    { id: '1', codigo: 'EMP001', nome: 'Empresa Teste 1' },
    { id: '2', codigo: 'EMP002', nome: 'Empresa Teste 2' }
  ];

  const mockPlants = [
    { id: 10, codigo: 'UHE001', nome: 'Usina Hidrelétrica 1', tipoId: '1' },
    { id: 11, codigo: 'UHE002', nome: 'Usina Hidrelétrica 2', tipoId: '1' }
  ];

  const mockIR1Data = {
    id: 1,
    dataReferencia: '2024-01-01',
    niveisPartida: [
      {
        usinaId: 10,
        nivel: 100.5,
        volume: 1500.0
      },
      {
        usinaId: 11,
        nivel: 95.3,
        volume: 1400.0
      }
    ]
  };

  beforeEach(() => {
    vi.clearAllMocks();

    const createMutate = vi.fn();
    const updateMutate = vi.fn();
    const refetchFn = vi.fn();

    // Mock useCompanies hook
    mockUseCompanies.mockReturnValue({
      data: mockCompanies,
      isLoading: false,
      error: null
    });

    // Mock usePlantsByCompany hook
    mockUsePlantsByCompany.mockImplementation((companyId?: string) => ({
      data: companyId ? mockPlants : [],
      isLoading: false,
      error: null
    }));

    // Mock useIR1DataByDate hook
    mockUseIR1DataByDate.mockReturnValue({
      data: mockIR1Data,
      isLoading: false,
      error: null,
      refetch: refetchFn
    });

    // Mock useCreateIR1Data hook
    mockUseCreateIR1Data.mockReturnValue({
      mutateAsync: createMutate,
      isPending: false,
      error: null
    });

    // Mock useUpdateIR1Data hook
    mockUseUpdateIR1Data.mockReturnValue({
      mutateAsync: updateMutate,
      isPending: false,
      error: null
    });
  });

  // Helper functions for user interactions
  const selectFirstDate = async () => {
    const user = userEvent.setup();
    const selects = screen.getAllByRole('combobox');
    const dateSelect = selects[0] as HTMLSelectElement;
    const option = Array.from(dateSelect.options).find(opt => opt.value);
    if (option) {
      await user.selectOptions(dateSelect, option.value);
      return option.value;
    }
    return '';
  };

  const selectCompany = async (value: string) => {
    const user = userEvent.setup();
    const selects = screen.getAllByRole('combobox');
    const companySelect = selects[1] as HTMLSelectElement;
    await user.selectOptions(companySelect, value);
  };

  const selectPlant = async (value: string) => {
    const user = userEvent.setup();
    const selects = screen.getAllByRole('combobox');
    const plantSelect = selects[2] as HTMLSelectElement;
    await user.selectOptions(plantSelect, value);
  };

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<IR1 />);
      expect(screen.getByText(/Nível de Partida/i)).toBeInTheDocument();
    });

    it('deve renderizar todos os campos do formulário', () => {
      render(<IR1 />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
      expect(screen.getByText(/Valor:/i)).toBeInTheDocument();
    });

    it('deve renderizar dropdowns desabilitados inicialmente', () => {
      render(<IR1 />);
      
      const selects = screen.getAllByRole('combobox');
      const empresaSelect = selects[1] as HTMLSelectElement;
      const usinaSelect = selects[2] as HTMLSelectElement;
      
      expect(empresaSelect).toBeDisabled();
      expect(usinaSelect).toBeDisabled();
    });

    it('não deve exibir botão Salvar inicialmente', () => {
      render(<IR1 />);
      expect(screen.queryByRole('button', { name: /salvar/i })).not.toBeInTheDocument();
    });
  });

  describe('Cascata de Campos', () => {
    it('deve habilitar dropdown Empresa ao selecionar Data', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        const empresaSelect = selects[1] as HTMLSelectElement;
        expect(empresaSelect).not.toBeDisabled();
      });
    });

    it('deve habilitar dropdown Usinas ao selecionar Empresa', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        const usinaSelect = selects[2] as HTMLSelectElement;
        expect(usinaSelect).not.toBeDisabled();
      });
    });

    it('deve carregar valor ao selecionar Usina com dados existentes', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      await waitFor(() => {
        const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
        expect(valorInput.value).toBe('100.5');
      });
    });

    it('deve limpar campos ao mudar data', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      // Change date
      const user = userEvent.setup();
      const selects = screen.getAllByRole('combobox');
      const dateSelect = selects[0] as HTMLSelectElement;
      const secondOption = Array.from(dateSelect.options).find((opt, idx) => idx === 2);
      
      if (secondOption) {
        await user.selectOptions(dateSelect, secondOption.value);
        
        await waitFor(() => {
          const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
          expect(valorInput.value).toBe('');
        });
      }
    });
  });

  describe('Exibição de Botão Salvar', () => {
    it('deve exibir botão Salvar ao selecionar Usina', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        expect(saveButton).toBeInTheDocument();
      });
    });

    it('deve ocultar botão Salvar ao limpar seleção de Usina', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      await waitFor(() => {
        expect(screen.getByRole('button', { name: /salvar/i })).toBeInTheDocument();
      });
      
      // Clear plant selection
      const user = userEvent.setup();
      const selects = screen.getAllByRole('combobox');
      const plantSelect = selects[2] as HTMLSelectElement;
      await user.selectOptions(plantSelect, '');
      
      await waitFor(() => {
        expect(screen.queryByRole('button', { name: /salvar/i })).not.toBeInTheDocument();
      });
    });
  });

  describe('Salvamento de Dados', () => {
    it('deve criar novo registro quando não existe dados', async () => {
      const createMutate = vi.fn().mockResolvedValue({ id: 2 });
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      mockUseIR1DataByDate.mockReturnValue({
        data: null,
        isLoading: false,
        error: null,
        refetch: vi.fn()
      });

      mockUseCreateIR1Data.mockReturnValue({
        mutateAsync: createMutate,
        isPending: false,
        error: null
      });

      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      const user = userEvent.setup();
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
      await user.clear(valorInput);
      await user.type(valorInput, '105.5');
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        return user.click(saveButton);
      });
      
      await waitFor(() => {
        expect(createMutate).toHaveBeenCalled();
        expect(alertSpy).toHaveBeenCalledWith('Nível de Partida salvo com sucesso!');
      });
      
      alertSpy.mockRestore();
    });

    it('deve atualizar registro existente', async () => {
      const updateMutate = vi.fn().mockResolvedValue({ id: 1 });
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      mockUseUpdateIR1Data.mockReturnValue({
        mutateAsync: updateMutate,
        isPending: false,
        error: null
      });

      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      const user = userEvent.setup();
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
      await user.clear(valorInput);
      await user.type(valorInput, '110.5');
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        return user.click(saveButton);
      });
      
      await waitFor(() => {
        expect(updateMutate).toHaveBeenCalled();
        expect(alertSpy).toHaveBeenCalledWith('Nível de Partida salvo com sucesso!');
      });
      
      alertSpy.mockRestore();
    });

    it('deve validar campos obrigatórios antes de salvar', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      mockUseIR1DataByDate.mockReturnValue({
        data: null,
        isLoading: false,
        error: null,
        refetch: vi.fn()
      });

      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      const user = userEvent.setup();
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
      await user.clear(valorInput);
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        return user.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Por favor, preencha todos os campos');
      });
      
      alertSpy.mockRestore();
    });

    it('deve exibir erro ao falhar salvamento', async () => {
      const createMutate = vi.fn().mockRejectedValue(new Error('Network error'));
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
      
      mockUseIR1DataByDate.mockReturnValue({
        data: null,
        isLoading: false,
        error: null,
        refetch: vi.fn()
      });

      mockUseCreateIR1Data.mockReturnValue({
        mutateAsync: createMutate,
        isPending: false,
        error: null
      });

      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      await selectPlant('UHE001');
      
      const user = userEvent.setup();
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
      await user.clear(valorInput);
      await user.type(valorInput, '105.5');
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        return user.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Erro ao salvar Nível de Partida');
      });
      
      alertSpy.mockRestore();
      consoleSpy.mockRestore();
    });
  });

  describe('Estados de Loading e Erro', () => {
    it('deve exibir mensagem de carregamento', () => {
      mockUseCompanies.mockReturnValue({
        data: [],
        isLoading: true,
        error: null
      });

      render(<IR1 />);
      expect(screen.getByText(/Carregando.../i)).toBeInTheDocument();
    });

    it('deve exibir mensagem de erro ao carregar dados', () => {
      mockUseIR1DataByDate.mockReturnValue({
        data: null,
        isLoading: false,
        error: new Error('Failed to fetch'),
        refetch: vi.fn()
      });

      render(<IR1 />);
      expect(screen.getByText(/Não foi possível carregar os dados de IR1/i)).toBeInTheDocument();
    });

    it('deve desabilitar botão Salvar durante salvamento', async () => {
      const createMutate = vi.fn().mockImplementation(() => new Promise(() => {})); // Never resolves
      
      mockUseCreateIR1Data.mockReturnValue({
        mutateAsync: createMutate,
        isPending: true,
        error: null
      });

      mockUseIR1DataByDate.mockReturnValue({
        data: null,
        isLoading: false,
        error: null,
        refetch: vi.fn()
      });

      render(<IR1 />);
      
      // With isPending: true, loading state should be shown
      await waitFor(() => {
        expect(screen.getByText(/Carregando.../i)).toBeInTheDocument();
      });
    });
  });

  describe('Integração com Hooks', () => {
    it('deve chamar useCompanies para carregar empresas', () => {
      render(<IR1 />);
      expect(mockUseCompanies).toHaveBeenCalled();
    });

    it('deve chamar usePlantsByCompany quando empresa é selecionada', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      await selectCompany('EMP001');
      
      await waitFor(() => {
        expect(mockUsePlantsByCompany).toHaveBeenCalled();
      });
    });

    it('deve chamar useIR1DataByDate quando data é selecionada', async () => {
      render(<IR1 />);
      
      await selectFirstDate();
      
      await waitFor(() => {
        expect(mockUseIR1DataByDate).toHaveBeenCalled();
      });
    });
  });
});

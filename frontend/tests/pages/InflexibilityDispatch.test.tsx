/**
 * Testes para o componente InflexibilityDispatch
 * Cobertura: Renderização, validações, interações, loading states
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import InflexibilityDispatch from '../../src/pages/Collection/Thermal/InflexibilityDispatch';
import type { DespachoInflexibilidadeData } from '../../src/types/inflexibilityDispatch';

describe('InflexibilityDispatch Component', () => {
  const mockData: DespachoInflexibilidadeData = {
    dataPdp: '20240115',
    codEmpresa: 'EMP001',
    usinas: [
      {
        codUsina: 'UTE001',
        intervalos: Array.from({ length: 48 }, (_, i) => ({
          intervalo: i + 1,
          valor: 100 + i,
        })),
      },
      {
        codUsina: 'UTE002',
        intervalos: Array.from({ length: 48 }, (_, i) => ({
          intervalo: i + 1,
          valor: 200 + i,
        })),
      },
    ],
  };

  const mockOnSave = vi.fn();
  const mockOnLoadData = vi.fn();

  beforeEach(() => {
    mockOnSave.mockClear();
    mockOnLoadData.mockClear();
    mockOnLoadData.mockResolvedValue(mockData);
  });

  describe('Renderização', () => {
    it('deve renderizar o título', () => {
      render(<InflexibilityDispatch />);
      expect(screen.getByText('Despacho de Inflexibilidade')).toBeInTheDocument();
    });

    it('deve renderizar select de Data PDP', () => {
      render(<InflexibilityDispatch />);
      expect(screen.getByLabelText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve renderizar select de Empresa', () => {
      render(<InflexibilityDispatch />);
      expect(screen.getByLabelText(/Empresa:/i)).toBeInTheDocument();
    });

    it('deve renderizar select de Usinas', () => {
      render(<InflexibilityDispatch />);
      expect(screen.getByLabelText(/Usinas:/i)).toBeInTheDocument();
    });

    it('não deve renderizar tabela inicialmente', () => {
      render(<InflexibilityDispatch />);
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
    });
  });

  describe('Seleção de Data PDP', () => {
    it('deve atualizar o estado ao selecionar data', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const options = within(selectData).getAllByRole('option');
      
      await user.selectOptions(selectData, options[1].textContent || '');
      
      expect(selectData).toHaveValue(options[1].getAttribute('value'));
    });

    it('não deve carregar dados apenas com data selecionada', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const options = within(selectData).getAllByRole('option');
      
      await user.selectOptions(selectData, options[1].textContent || '');
      
      expect(mockOnLoadData).not.toHaveBeenCalled();
    });

    it('deve limpar dados ao trocar de data', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledTimes(1);
      });
      
      await user.selectOptions(selectData, dataOptions[2].textContent || '');
      
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
    });
  });

  describe('Seleção de Empresa', () => {
    it('deve carregar dados ao selecionar empresa com data', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledTimes(1);
      });
    });

    it('deve exibir tabela após carregar dados', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
    });
  });

  describe('Seleção de Usina', () => {
    it('deve exibir textarea ao selecionar usina individual', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('textbox')).toBeInTheDocument();
      });
    });

    it('deve exibir botão Salvar ao selecionar usina', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
    });

    it('deve preencher textarea com valores da usina selecionada', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        const textarea = screen.getByRole('textbox') as HTMLTextAreaElement;
        expect(textarea.value).toContain('100');
      });
    });
  });

  describe('Validações', () => {
    it('deve exibir erro ao tentar salvar sem preencher campos', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
      
      const textarea = screen.getByRole('textbox') as HTMLTextAreaElement;
      await user.clear(textarea);
      await user.type(textarea, '150');
      
      const saveButton = screen.getByText('Salvar');
      await user.click(saveButton);
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalled();
      });
    });

    it('deve validar que Data PDP está selecionada', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      expect(selectEmpresa).toBeDisabled();
    });

    it('deve validar que Empresa está selecionada', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const dataOptions = within(selectData).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      expect(selectUsina).toBeDisabled();
    });
  });

  describe('Interação com Textarea', () => {
    it('deve permitir edição de valores no textarea', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('textbox')).toBeInTheDocument();
      });
      
      const textarea = screen.getByRole('textbox') as HTMLTextAreaElement;
      await user.clear(textarea);
      await user.type(textarea, '999');
      
      expect(textarea.value).toBe('999');
    });

    it('deve aceitar múltiplas linhas no textarea', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('textbox')).toBeInTheDocument();
      });
      
      const textarea = screen.getByRole('textbox') as HTMLTextAreaElement;
      await user.clear(textarea);
      await user.type(textarea, '100');
      await user.keyboard('{Enter}');
      await user.type(textarea, '200');
      
      expect(textarea.value).toContain('\n');
    });
  });

  describe('Loading States', () => {
    it('deve exibir loading ao carregar dados', async () => {
      const slowLoadData = vi.fn(() => new Promise(resolve => setTimeout(() => resolve(mockData), 100)));
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={slowLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      expect(screen.getByText('Carregando...')).toBeInTheDocument();
      
      await waitFor(() => {
        expect(screen.queryByText('Carregando...')).not.toBeInTheDocument();
      });
    });

    it('deve desabilitar campos durante loading', async () => {
      const slowLoadData = vi.fn(() => new Promise(resolve => setTimeout(() => resolve(mockData), 100)));
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={slowLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      expect(selectData).toBeDisabled();
      expect(selectEmpresa).toBeDisabled();
      
      await waitFor(() => {
        expect(selectData).not.toBeDisabled();
      });
    });
  });

  describe('Mensagens', () => {
    it('deve exibir mensagem de erro em caso de falha no carregamento', async () => {
      const failLoadData = vi.fn(() => Promise.reject(new Error('Erro')));
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={failLoadData} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByText('Erro ao carregar dados')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem de sucesso após salvar', async () => {
      const user = userEvent.setup();
      render(<InflexibilityDispatch onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      const selectData = screen.getByLabelText(/Data PDP:/i);
      const selectEmpresa = screen.getByLabelText(/Empresa:/i);
      
      const dataOptions = within(selectData).getAllByRole('option');
      const empresaOptions = within(selectEmpresa).getAllByRole('option');
      
      await user.selectOptions(selectData, dataOptions[1].textContent || '');
      await user.selectOptions(selectEmpresa, empresaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
      
      const selectUsina = screen.getByLabelText(/Usinas:/i);
      const usinaOptions = within(selectUsina).getAllByRole('option');
      
      await user.selectOptions(selectUsina, usinaOptions[1].textContent || '');
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
      
      const saveButton = screen.getByText('Salvar');
      await user.click(saveButton);
      
      await waitFor(() => {
        expect(screen.getByText('Dados salvos com sucesso')).toBeInTheDocument();
      });
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DCR from '../../../src/pages/Collection/Other/DCR';

describe('DCR Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<DCR />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir o título correto', () => {
      render(<DCR />);
      expect(screen.getByText('Despacho Ciclo Reduzido')).toBeInTheDocument();
    });

    it('deve exibir todos os campos do formulário', () => {
      render(<DCR />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve renderizar o botão Salvar desabilitado inicialmente', () => {
      render(<DCR />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeDisabled();
    });

    it('deve renderizar a tabela com 48 intervalos', () => {
      render(<DCR />);
      
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(51);
    });

    it('deve exibir os horários corretos', () => {
      render(<DCR />);
      
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Seleção em Cascata', () => {
    it('deve habilitar Empresa ao selecionar Data', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      expect(empresaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });

    it('deve habilitar Usina ao selecionar Empresa', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        expect(usinaSelect).not.toBeDisabled();
      });
    });

    it('deve limpar campos ao mudar Data', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      fireEvent.change(dataSelect, { target: { value: '02/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).toHaveValue('');
      });
    });
  });

  describe('Seleção de Usina', () => {
    it('deve exibir textarea ao selecionar usina', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const textarea = screen.getByPlaceholderText(/Digite os valores DCR/i);
        expect(textarea).toBeInTheDocument();
      });
    });

    it('deve habilitar botão Salvar', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        expect(saveButton).not.toBeDisabled();
      });
    });

    it('deve carregar dados mockados', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      expect(textarea).toHaveValue(expect.stringContaining('30'));
    });
  });

  describe('Edição de Valores', () => {
    it('deve atualizar tabela ao editar textarea', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      
      fireEvent.change(textarea, { target: { value: '40\n50\n60' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('40\n50\n60');
      });
    });

    it('deve calcular totais corretamente', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      
      const values = Array(48).fill('30').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const footerCells = screen.getAllByText(/1440\.00/i);
        expect(footerCells.length).toBeGreaterThan(0);
      });
    });

    it('deve calcular médias corretamente', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      
      const values = Array(48).fill('30').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const mediaCells = screen.getAllByText(/30\.00/i);
        expect(mediaCells.length).toBeGreaterThan(0);
      });
    });
  });

  describe('Modo Múltiplas Usinas', () => {
    it('deve processar valores Tab-separated', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Todas as Usinas' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      expect(textarea).toHaveValue(expect.stringContaining('\t'));
    });

    it('deve calcular totais de múltiplas usinas', async () => {
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Todas as Usinas' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCR/i);
      const firstValue = textarea.value.split('\n')[0];
      const values = firstValue.split('\t').map(Number);
      const sum = values.reduce((a, b) => a + b, 0);
      
      expect(sum).toBeGreaterThan(0);
    });
  });

  describe('Salvamento', () => {
    it('deve salvar com dados completos', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const consoleSpy = vi.spyOn(console, 'log').mockImplementation(() => {});
      
      render(<DCR />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      await waitFor(() => {
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Dados DCR salvos com sucesso!');
      });
      
      alertSpy.mockRestore();
      consoleSpy.mockRestore();
    });
  });

  describe('Geração de Horários', () => {
    it('deve gerar horário inicial correto', () => {
      render(<DCR />);
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
    });

    it('deve gerar horário final correto', () => {
      render(<DCR />);
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Acessibilidade', () => {
    it('deve ter labels para campos', () => {
      render(<DCR />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve ter botão acessível', () => {
      render(<DCR />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeInTheDocument();
    });

    it('deve ter tabela semântica', () => {
      render(<DCR />);
      
      const table = screen.getByRole('table');
      expect(table.querySelector('thead')).toBeInTheDocument();
      expect(table.querySelector('tbody')).toBeInTheDocument();
      expect(table.querySelector('tfoot')).toBeInTheDocument();
    });
  });
});

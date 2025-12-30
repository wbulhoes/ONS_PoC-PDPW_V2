import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import REDispatch from '../../../src/pages/Collection/Other/REDispatch';

describe('REDispatch Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<REDispatch />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir todos os campos do formulário', () => {
      render(<REDispatch />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve renderizar botão Salvar desabilitado', () => {
      render(<REDispatch />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeDisabled();
    });

    it('deve renderizar tabela com 48 intervalos', () => {
      render(<REDispatch />);
      
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(51);
    });

    it('deve exibir horários corretos', () => {
      render(<REDispatch />);
      
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Cascata de Seleção', () => {
    it('deve habilitar Empresa ao selecionar Data', async () => {
      render(<REDispatch />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      expect(empresaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });

    it('deve habilitar Usina ao selecionar Empresa', async () => {
      render(<REDispatch />);
      
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

    it('deve limpar campos dependentes', async () => {
      render(<REDispatch />);
      
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
      render(<REDispatch />);
      
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
        const textarea = screen.getByPlaceholderText(/Digite valores de Reserva/i);
        expect(textarea).toBeInTheDocument();
      });
    });

    it('deve habilitar botão Salvar', async () => {
      render(<REDispatch />);
      
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
      render(<REDispatch />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Reserva/i);
      expect(textarea).toHaveValue(expect.stringContaining('20'));
    });
  });

  describe('Edição de Valores', () => {
    it('deve atualizar tabela ao editar textarea', async () => {
      render(<REDispatch />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Reserva/i);
      
      fireEvent.change(textarea, { target: { value: '25\n35\n45' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('25\n35\n45');
      });
    });

    it('deve calcular totais corretamente', async () => {
      render(<REDispatch />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Reserva/i);
      
      const values = Array(48).fill('20').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const footerCells = screen.getAllByText(/960\.00/i);
        expect(footerCells.length).toBeGreaterThan(0);
      });
    });

    it('deve calcular médias corretamente', async () => {
      render(<REDispatch />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Reserva/i);
      
      const values = Array(48).fill('20').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const mediaCells = screen.getAllByText(/20\.00/i);
        expect(mediaCells.length).toBeGreaterThan(0);
      });
    });
  });

  describe('Modo Múltiplas Usinas', () => {
    it('deve processar valores Tab-separated', async () => {
      render(<REDispatch />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Reserva/i);
      expect(textarea).toHaveValue(expect.stringContaining('\t'));
    });
  });

  describe('Salvamento', () => {
    it('deve salvar com dados completos', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const consoleSpy = vi.spyOn(console, 'log').mockImplementation(() => {});
      
      render(<REDispatch />);
      
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
        expect(alertSpy).toHaveBeenCalledWith('Despacho de Reserva de Energia salvo com sucesso!');
      });
      
      alertSpy.mockRestore();
      consoleSpy.mockRestore();
    });
  });

  describe('Geração de Horários', () => {
    it('deve gerar horário inicial', () => {
      render(<REDispatch />);
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
    });

    it('deve gerar horário final', () => {
      render(<REDispatch />);
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Acessibilidade', () => {
    it('deve ter labels', () => {
      render(<REDispatch />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve ter tabela semântica', () => {
      render(<REDispatch />);
      
      const table = screen.getByRole('table');
      expect(table.querySelector('thead')).toBeInTheDocument();
      expect(table.querySelector('tbody')).toBeInTheDocument();
      expect(table.querySelector('tfoot')).toBeInTheDocument();
    });
  });
});

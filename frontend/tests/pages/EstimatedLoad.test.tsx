import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import EstimatedLoad from '../../src/pages/Collection/Load/EstimatedLoad';

describe('EstimatedLoad Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<EstimatedLoad />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir formulário completo', () => {
      render(<EstimatedLoad />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Submercado:/i)).toBeInTheDocument();
    });

    it('deve renderizar tabela com 50 intervalos', () => {
      render(<EstimatedLoad />);
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(53); // header + 50 rows + 2 footer
    });

    it('deve renderizar botão calendário', () => {
      render(<EstimatedLoad />);
      const calendarButton = screen.getByRole('button', { name: '...' });
      expect(calendarButton).toBeInTheDocument();
    });
  });

  describe('Seleção de Data', () => {
    it('deve mostrar calendário ao clicar no botão', () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]');
      expect(datePicker).toBeInTheDocument();
    });

    it('deve habilitar Empresa após selecionar data', async () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        expect(empresaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Cascata de Seleção', () => {
    it('deve habilitar Submercado após Empresa', async () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const submercadoSelect = screen.getAllByRole('combobox')[1];
        expect(submercadoSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção de Submercado', () => {
    it('deve exibir textarea ao selecionar submercado', async () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const submercadoSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(submercadoSelect, { target: { value: 'Norte' } });
      });
      
      await waitFor(() => {
        const textarea = screen.getByPlaceholderText(/Digite valores de Carga/i);
        expect(textarea).toBeInTheDocument();
      });
    });

    it('deve habilitar botão Salvar', async () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const submercadoSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(submercadoSelect, { target: { value: 'Norte' } });
      });
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        expect(saveButton).not.toBeDisabled();
      });
    });
  });

  describe('Edição', () => {
    it('deve atualizar valores na tabela', async () => {
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const submercadoSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(submercadoSelect, { target: { value: 'Norte' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite valores de Carga/i);
      fireEvent.change(textarea, { target: { value: '1500\n1600' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('1500\n1600');
      });
    });
  });

  describe('Salvamento', () => {
    it('deve salvar com sucesso', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      const { container } = render(<EstimatedLoad />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const submercadoSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(submercadoSelect, { target: { value: 'Norte' } });
      });
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      await waitFor(() => {
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Carga Estimada salva com sucesso!');
      });
      
      alertSpy.mockRestore();
    });
  });

  describe('Horários', () => {
    it('deve gerar horários corretos (50 intervalos)', () => {
      render(<EstimatedLoad />);
      
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
      expect(screen.getByText('24:00-24:30')).toBeInTheDocument();
    });
  });
});

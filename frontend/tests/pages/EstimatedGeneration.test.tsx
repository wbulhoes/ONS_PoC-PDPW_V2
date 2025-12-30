import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import EstimatedGeneration from '../../../src/pages/Collection/Load/EstimatedGeneration';

describe('EstimatedGeneration Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<EstimatedGeneration />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir formulário', () => {
      render(<EstimatedGeneration />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usina:/i)).toBeInTheDocument();
    });

    it('deve renderizar tabela com 50 intervalos', () => {
      render(<EstimatedGeneration />);
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(53);
    });
  });

  describe('Seleção de Data', () => {
    it('deve mostrar calendário', () => {
      render(<EstimatedGeneration />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      expect(datePicker).toBeInTheDocument();
    });

    it('deve habilitar Empresa', async () => {
      render(<EstimatedGeneration />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        expect(empresaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Cascata', () => {
    it('deve habilitar Usina', async () => {
      render(<EstimatedGeneration />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[1];
        expect(usinaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção', () => {
    it('deve exibir textarea', async () => {
      render(<EstimatedGeneration />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const textarea = screen.getByPlaceholderText(/Digite valores de Geração/i);
        expect(textarea).toBeInTheDocument();
      });
    });
  });

  describe('Salvamento', () => {
    it('deve salvar', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<EstimatedGeneration />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      await waitFor(() => {
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Geração Estimada salva com sucesso!');
      });
      
      alertSpy.mockRestore();
    });
  });
});

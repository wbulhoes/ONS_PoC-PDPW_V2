import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import EstimatedInterchange from '../../../src/pages/Collection/Interchange/EstimatedInterchange';

describe('EstimatedInterchange Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<EstimatedInterchange />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir formulário', () => {
      render(<EstimatedInterchange />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Intercâmbios:/i)).toBeInTheDocument();
    });

    it('deve renderizar tabela com 50 intervalos', () => {
      render(<EstimatedInterchange />);
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(53);
    });
  });

  describe('Seleção de Data', () => {
    it('deve mostrar calendário', () => {
      render(<EstimatedInterchange />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      expect(datePicker).toBeInTheDocument();
    });

    it('deve habilitar Empresa', async () => {
      render(<EstimatedInterchange />);
      
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
    it('deve habilitar Intercâmbios', async () => {
      render(<EstimatedInterchange />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const intercambioSelect = screen.getAllByRole('combobox')[1];
        expect(intercambioSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção', () => {
    it('deve exibir textarea', async () => {
      render(<EstimatedInterchange />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const intercambioSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(intercambioSelect, { target: { value: 'Norte -> Nordeste' } });
      });
      
      await waitFor(() => {
        const textarea = screen.getByPlaceholderText(/Digite valores de Intercâmbio/i);
        expect(textarea).toBeInTheDocument();
      });
    });
  });

  describe('Salvamento', () => {
    it('deve salvar', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<EstimatedInterchange />);
      
      const calendarButton = screen.getByRole('button', { name: '...' });
      fireEvent.click(calendarButton);
      
      const datePicker = screen.getByDisplayValue('');
      fireEvent.change(datePicker, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[0];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const intercambioSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(intercambioSelect, { target: { value: 'Norte -> Nordeste' } });
      });
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      await waitFor(() => {
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Intercâmbio Estimado salvo com sucesso!');
      });
      
      alertSpy.mockRestore();
    });
  });

  describe('Validação', () => {
    it('deve exigir todos os campos', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<EstimatedInterchange />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      expect(saveButton).toBeDisabled();
      
      alertSpy.mockRestore();
    });
  });
});

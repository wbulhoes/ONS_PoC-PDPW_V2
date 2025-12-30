import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import OutOfMeritGeneration from '../../../src/pages/Collection/Other/OutOfMeritGeneration';

describe('OutOfMeritGeneration Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<OutOfMeritGeneration />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir formulário', () => {
      render(<OutOfMeritGeneration />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve renderizar tabela', () => {
      render(<OutOfMeritGeneration />);
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(51);
    });
  });

  describe('Cascata', () => {
    it('deve habilitar Empresa', async () => {
      render(<OutOfMeritGeneration />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      expect(empresaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção', () => {
    it('deve exibir textarea', async () => {
      render(<OutOfMeritGeneration />);
      
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
        const textarea = screen.getByPlaceholderText(/Digite valores de Geração/i);
        expect(textarea).toBeInTheDocument();
      });
    });
  });

  describe('Salvamento', () => {
    it('deve salvar', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<OutOfMeritGeneration />);
      
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
        expect(alertSpy).toHaveBeenCalledWith('Geração Fora de Mérito salva com sucesso!');
      });
      
      alertSpy.mockRestore();
    });
  });
});

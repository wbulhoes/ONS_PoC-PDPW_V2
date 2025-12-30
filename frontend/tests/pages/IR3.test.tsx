import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import IR3 from '../../../src/pages/Collection/Insumos/IR3';

describe('IR3 Component - Dia -2', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<IR3 />);
      expect(screen.getByText(/Dia -2/i)).toBeInTheDocument();
    });

    it('deve exibir formulário', () => {
      render(<IR3 />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });
  });

  describe('Tabela', () => {
    it('deve exibir tabela ao selecionar Usina', async () => {
      render(<IR3 />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      fireEvent.change(dataSelect, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[2];
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const table = screen.getByRole('table');
        expect(table).toBeInTheDocument();
      });
    });

    it('deve ter 24 linhas', async () => {
      render(<IR3 />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      fireEvent.change(dataSelect, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[2];
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const rows = screen.getAllByRole('row');
        expect(rows.length).toBe(25); // 24 + header
      });
    });
  });

  describe('Salvamento', () => {
    it('deve salvar dados', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<IR3 />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      fireEvent.change(dataSelect, { target: { value: '2024-01-01' } });
      
      await waitFor(() => {
        const empresaSelect = screen.getAllByRole('combobox')[1];
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        const usinaSelect = screen.getAllByRole('combobox')[2];
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const saveButton = screen.getByRole('button', { name: /salvar/i });
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Dados do Dia -2 salvos com sucesso!');
      });
      
      alertSpy.mockRestore();
    });
  });
});

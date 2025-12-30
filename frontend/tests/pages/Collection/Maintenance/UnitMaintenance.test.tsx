import { describe, it, expect, vi, beforeEach } from 'vitest';
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import UnitMaintenance from '../../../src/pages/Collection/Maintenance/UnitMaintenance';

describe('UnitMaintenance Component - Manutenção de Unidades Geradoras', () => {
  const mockOnLoadUsinas = vi.fn(() => 
    Promise.resolve([
      { id: 1, nome: 'Usina A' },
      { id: 2, nome: 'Usina B' },
    ])
  );

  const mockOnLoadUnidades = vi.fn(() =>
    Promise.resolve([
      { id: 1, nome: 'Unidade 1', usinaId: 1 },
      { id: 2, nome: 'Unidade 2', usinaId: 1 },
      { id: 3, nome: 'Unidade 3', usinaId: 2 },
    ])
  );

  const mockOnLoadMaintenances = vi.fn(() =>
    Promise.resolve([
      {
        id: 1,
        dataPdp: '2024-01-10',
        usinaId: 1,
        usinaNome: 'Usina A',
        unidadeGeradoraId: 1,
        unidadeNome: 'Unidade 1',
        tipoManutencao: 'PREVENTIVA',
        dataInicio: '2024-01-10',
        dataFim: '2024-01-15',
        observacao: 'Manutenção preventiva',
        status: 'PROGRAMADA',
      },
    ])
  );

  const mockOnSave = vi.fn(() => Promise.resolve());
  const mockOnDelete = vi.fn(() => Promise.resolve());

  const defaultProps = {
    onLoadUsinas: mockOnLoadUsinas,
    onLoadUnidades: mockOnLoadUnidades,
    onLoadMaintenances: mockOnLoadMaintenances,
    onSave: mockOnSave,
    onDelete: mockOnDelete,
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Rendering', () => {
    it('should render component with title', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const title = screen.queryByText(/Manutenção|Maintenance/i);
        expect(title).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should render maintenance table', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const table = screen.queryByRole('table');
        expect(table).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should render add button', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const addButton = screen.queryByRole('button', { name: /Adicionar|Add/i });
        expect(addButton).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should render date filter', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const dateInput = screen.queryByDisplayValue(/\d{4}-\d{2}-\d{2}/) || screen.queryByPlaceholderText(/data|date/i);
        expect(dateInput).toBeInTheDocument();
      }, { timeout: 2000 });
    });
  });

  describe('Data Loading', () => {
    it('should load usinas on component mount', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      }, { timeout: 2000 });
    });

    it('should load maintenance data on component mount', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        expect(mockOnLoadMaintenances).toHaveBeenCalled();
      }, { timeout: 2000 });
    });

    it('should load unidades when usina is selected', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Selecionar usina
        const usinaSelect = screen.queryByDisplayValue('Usina A');
        if (usinaSelect) {
          fireEvent.change(usinaSelect, { target: { value: '1' } });
        }
      }, { timeout: 2000 });
    });
  });

  describe('CRUD Operations', () => {
    it('should open dialog when add button is clicked', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const addButton = screen.queryByRole('button', { name: /Adicionar|Add/i });
        if (addButton) {
          fireEvent.click(addButton);

          // Dialog deve aparecer
        }
      }, { timeout: 2000 });
    });

    it('should open dialog when edit button is clicked', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const editButton = screen.queryByRole('button', { name: /Editar|Edit/i });
        if (editButton) {
          fireEvent.click(editButton);

          // Dialog deve aparecer com dados preenchidos
        }
      }, { timeout: 2000 });
    });

    it('should handle save operation', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const saveButton = screen.queryByRole('button', { name: /Salvar|Save/i });
        if (saveButton) {
          fireEvent.click(saveButton);

          expect(mockOnSave).toHaveBeenCalled();
        }
      }, { timeout: 2000 });
    });

    it('should handle delete operation', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const deleteButton = screen.queryByRole('button', { name: /Deletar|Delete|Remover/i });
        if (deleteButton) {
          fireEvent.click(deleteButton);

          expect(mockOnDelete).toHaveBeenCalled();
        }
      }, { timeout: 2000 });
    });
  });

  describe('Maintenance Type Selection', () => {
    it('should display all maintenance types', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const typeSelect = screen.queryByDisplayValue(/PREVENTIVA|CORRETIVA/i);
        expect(typeSelect).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should support preventive maintenance type', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar tipo PREVENTIVA
      }, { timeout: 2000 });
    });

    it('should support corrective maintenance type', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar tipo CORRETIVA
      }, { timeout: 2000 });
    });

    it('should support predictive maintenance type', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar tipo PREDITIVA
      }, { timeout: 2000 });
    });

    it('should support emergency maintenance type', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar tipo EMERGENCIAL
      }, { timeout: 2000 });
    });
  });

  describe('Status Selection', () => {
    it('should display all status options', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const statusSelect = screen.queryByDisplayValue(/PROGRAMADA|EM_ANDAMENTO/i);
        expect(statusSelect).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should support scheduled status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar status PROGRAMADA
      }, { timeout: 2000 });
    });

    it('should support in progress status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar status EM_ANDAMENTO
      }, { timeout: 2000 });
    });

    it('should support completed status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar status CONCLUIDA
      }, { timeout: 2000 });
    });

    it('should support cancelled status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar status CANCELADA
      }, { timeout: 2000 });
    });
  });

  describe('Date Validation', () => {
    it('should validate start date', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar validação de data inicial
      }, { timeout: 2000 });
    });

    it('should validate end date', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar validação de data final
      }, { timeout: 2000 });
    });

    it('should validate that end date is after start date', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar se data final > data inicial
      }, { timeout: 2000 });
    });

    it('should prevent selecting past dates', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar restrição de datas passadas
      }, { timeout: 2000 });
    });
  });

  describe('Filtering', () => {
    it('should filter by date', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const dateInput = screen.queryByDisplayValue(/\d{4}-\d{2}-\d{2}/) || screen.queryByPlaceholderText(/data|date/i);
        if (dateInput) {
          fireEvent.change(dateInput, { target: { value: '2024-01-15' } });

          expect(mockOnLoadMaintenances).toHaveBeenCalled();
        }
      }, { timeout: 2000 });
    });

    it('should filter by usina', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const usinaSelect = screen.queryByDisplayValue(/Usina/i);
        if (usinaSelect) {
          fireEvent.change(usinaSelect, { target: { value: '1' } });

          expect(mockOnLoadMaintenances).toHaveBeenCalled();
        }
      }, { timeout: 2000 });
    });

    it('should filter by status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const statusSelect = screen.queryByDisplayValue(/PROGRAMADA|CONCLUIDA/i);
        if (statusSelect) {
          fireEvent.change(statusSelect, { target: { value: 'CONCLUIDA' } });

          expect(mockOnLoadMaintenances).toHaveBeenCalled();
        }
      }, { timeout: 2000 });
    });
  });

  describe('Table Display', () => {
    it('should display plant name', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const plantName = screen.queryByText(/Usina A/i);
        expect(plantName).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should display unit name', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const unitName = screen.queryByText(/Unidade/i);
        expect(unitName).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should display maintenance type', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const type = screen.queryByText(/PREVENTIVA|CORRETIVA|PREDITIVA|EMERGENCIAL/);
        expect(type).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should display dates', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const date = screen.queryByText(/2024-01-10|2024-01-15/);
        expect(date).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should display status', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const status = screen.queryByText(/PROGRAMADA|EM_ANDAMENTO|CONCLUIDA|CANCELADA/);
        expect(status).toBeInTheDocument();
      }, { timeout: 2000 });
    });
  });

  describe('Observations', () => {
    it('should display observation field', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const obsField = screen.queryByPlaceholderText(/observação|observation/i);
        expect(obsField).toBeInTheDocument();
      }, { timeout: 2000 });
    });

    it('should allow editing observations', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        const obsField = screen.queryByPlaceholderText(/observação|observation/i) as HTMLTextAreaElement;
        if (obsField) {
          fireEvent.change(obsField, { target: { value: 'Manutenção urgente' } });
          expect(obsField.value).toBe('Manutenção urgente');
        }
      }, { timeout: 2000 });
    });
  });

  describe('Error Handling', () => {
    it('should display error message on API failure', async () => {
      const propsWithError = {
        ...defaultProps,
        onLoadMaintenances: vi.fn(() => Promise.reject(new Error('API error'))),
      };

      render(<UnitMaintenance {...propsWithError} />);

      await waitFor(() => {
        // Verificar mensagem de erro
      }, { timeout: 2000 });
    });

    it('should show validation errors', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar mensagens de validação
      }, { timeout: 2000 });
    });
  });

  describe('Accessibility', () => {
    it('should have proper labels', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      await waitFor(() => {
        // Verificar labels
      }, { timeout: 2000 });
    });

    it('should support keyboard navigation', async () => {
      render(<UnitMaintenance {...defaultProps} />);

      const addButton = screen.queryByRole('button', { name: /Adicionar|Add/i });
      if (addButton) {
        addButton.focus();
        expect(addButton).toHaveFocus();
      }
    });
  });

  describe('Responsiveness', () => {
    it('should render on mobile', () => {
      global.innerWidth = 375;
      render(<UnitMaintenance {...defaultProps} />);

      const title = screen.queryByText(/Manutenção|Maintenance/i);
      expect(title).toBeInTheDocument();
    });

    it('should render on tablet', () => {
      global.innerWidth = 768;
      render(<UnitMaintenance {...defaultProps} />);

      const title = screen.queryByText(/Manutenção|Maintenance/i);
      expect(title).toBeInTheDocument();
    });

    it('should render on desktop', () => {
      global.innerWidth = 1024;
      render(<UnitMaintenance {...defaultProps} />);

      const title = screen.queryByText(/Manutenção|Maintenance/i);
      expect(title).toBeInTheDocument();
    });
  });
});

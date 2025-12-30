import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import OperatingMachines from '../../src/pages/Collection/Maintenance/OperatingMachines';
import type { OperatingMachine } from '../../src/types/machineStatus';

describe('OperatingMachines Component', () => {
  const mockUsinas = [
    { id: 1, nome: 'UHE Itaipu' },
    { id: 2, nome: 'UHE Belo Monte' },
  ];

  const mockUnidades = [
    { id: 1, nome: 'Unidade 1', usinaId: 1 },
    { id: 2, nome: 'Unidade 2', usinaId: 1 },
  ];

  const mockMachines: OperatingMachine[] = [
    {
      id: 1,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 1,
      unidadeNome: 'Unidade 1',
      potenciaGerada: 700,
      horaInicio: '08:00',
      horaFim: '18:00',
      observacao: 'Operação normal',
      statusOperacao: 'OPERANDO',
      modoOperacao: 'AUTOMATICO',
    },
    {
      id: 2,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 2,
      unidadeNome: 'Unidade 2',
      potenciaGerada: 680,
      horaInicio: '06:00',
      observacao: 'Sincronizando',
      statusOperacao: 'SINCRONIZANDO',
      modoOperacao: 'MANUAL',
    },
  ];

  const mockOnLoadUsinas = vi.fn().mockResolvedValue(mockUsinas);
  const mockOnLoadUnidades = vi.fn().mockResolvedValue(mockUnidades);
  const mockOnLoadMachines = vi.fn().mockResolvedValue(mockMachines);
  const mockOnUpdateStatus = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByText('Coleta - Máquinas Operando')).toBeInTheDocument();
    });

    it('deve renderizar filtros corretamente', () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByTestId('filter-data-pdp')).toBeInTheDocument();
      expect(screen.getByTestId('filter-usina')).toBeInTheDocument();
    });

    it('deve renderizar botão de atualizar operação', () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByTestId('btn-atualizar-operacao')).toBeInTheDocument();
    });
  });

  describe('Carregamento de dados', () => {
    it('deve carregar lista de usinas ao montar', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });
    });

    it('deve carregar máquinas quando filtros são aplicados', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      const filterDataPdp = screen.getByTestId('filter-data-pdp');
      fireEvent.change(filterDataPdp, { target: { value: '2024-01-15' } });

      await waitFor(() => {
        expect(mockOnLoadMachines).toHaveBeenCalledWith({
          dataPdp: '2024-01-15',
          usinaId: 0,
        });
      });
    });

    it('deve exibir mensagem quando não há máquinas operando', async () => {
      const mockEmptyLoad = vi.fn().mockResolvedValue([]);
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockEmptyLoad}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-data-pdp'), {
        target: { value: '2024-01-15' },
      });

      await waitFor(() => {
        expect(screen.getByText('Nenhuma máquina operando encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de formulário', () => {
    it('deve abrir diálogo ao clicar em atualizar operação', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      const btnNovo = screen.getByTestId('btn-atualizar-operacao');
      fireEvent.click(btnNovo);

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });
      expect(screen.getByText('Nova Operação')).toBeInTheDocument();
    });

    it('deve fechar diálogo ao clicar em cancelar', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-cancel'));

      await waitFor(() => {
        expect(screen.queryByTestId('dialog-form')).not.toBeInTheDocument();
      });
    });

    it('deve carregar unidades ao selecionar usina', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('form-usina')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });

      await waitFor(() => {
        expect(mockOnLoadUnidades).toHaveBeenCalledWith(1);
      });
    });
  });

  describe('Atualizar operação', () => {
    it('deve atualizar operação com dados válidos', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('form-data-pdp')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });

      await waitFor(() => {
        expect(mockOnLoadUnidades).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('form-unidade'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-potencia'), {
        target: { value: '700' },
      });
      fireEvent.change(screen.getByTestId('form-status-operacao'), {
        target: { value: 'OPERANDO' },
      });
      fireEvent.change(screen.getByTestId('form-modo-operacao'), {
        target: { value: 'AUTOMATICO' },
      });
      fireEvent.change(screen.getByTestId('form-hora-inicio'), {
        target: { value: '08:00' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnUpdateStatus).toHaveBeenCalled();
      });
      await waitFor(() => {
        expect(screen.getByTestId('success-message')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao atualizar sem preencher campos obrigatórios', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('btn-save')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
      expect(mockOnUpdateStatus).not.toHaveBeenCalled();
    });
  });

  describe('Edição de máquina', () => {
    it('deve abrir diálogo para edição ao clicar no botão editar', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-data-pdp'), {
        target: { value: '2024-01-15' },
      });

      await waitFor(() => {
        expect(mockOnLoadMachines).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-edit-1'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });
      expect(screen.getByText('Editar Operação')).toBeInTheDocument();
    });
  });

  describe('Verificação de campos específicos', () => {
    it('deve renderizar campos de status e modo de operação', async () => {
      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('form-status-operacao')).toBeInTheDocument();
        expect(screen.getByTestId('form-modo-operacao')).toBeInTheDocument();
      });
    });
  });

  describe('Tratamento de erros', () => {
    it('deve exibir erro ao falhar no carregamento de usinas', async () => {
      const mockError = vi.fn().mockRejectedValue(new Error('Erro ao carregar'));

      render(
        <OperatingMachines
          onLoadUsinas={mockError}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar na atualização', async () => {
      const mockUpdateError = vi.fn().mockRejectedValue(new Error('Erro ao atualizar'));

      render(
        <OperatingMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockUpdateError}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-atualizar-operacao'));

      await waitFor(() => {
        expect(screen.getByTestId('form-data-pdp')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });

      await waitFor(() => {
        expect(mockOnLoadUnidades).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('form-unidade'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-potencia'), {
        target: { value: '700' },
      });
      fireEvent.change(screen.getByTestId('form-hora-inicio'), {
        target: { value: '08:00' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
    });
  });
});

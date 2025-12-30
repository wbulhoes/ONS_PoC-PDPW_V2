import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import StoppedMachines from '../../src/pages/Collection/Maintenance/StoppedMachines';
import type { StoppedMachine } from '../../src/types/machineStatus';

describe('StoppedMachines Component', () => {
  const mockUsinas = [
    { id: 1, nome: 'UHE Itaipu' },
    { id: 2, nome: 'UHE Belo Monte' },
  ];

  const mockUnidades = [
    { id: 1, nome: 'Unidade 1', usinaId: 1 },
    { id: 2, nome: 'Unidade 2', usinaId: 1 },
  ];

  const mockMachines: StoppedMachine[] = [
    {
      id: 1,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 1,
      unidadeNome: 'Unidade 1',
      potenciaGerada: 0,
      horaInicio: '08:00',
      horaFim: '18:00',
      observacao: 'Manutenção preventiva',
      motivoParada: 'Manutenção Preventiva',
      tipoParada: 'PROGRAMADA',
    },
    {
      id: 2,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 2,
      unidadeNome: 'Unidade 2',
      potenciaGerada: 0,
      horaInicio: '06:00',
      observacao: 'Falha no gerador',
      motivoParada: 'Falha Mecânica',
      tipoParada: 'FORCADA',
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
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByText('Coleta - Máquinas Paradas')).toBeInTheDocument();
    });

    it('deve renderizar filtros corretamente', () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByTestId('filter-data-pdp')).toBeInTheDocument();
      expect(screen.getByTestId('filter-usina')).toBeInTheDocument();
    });

    it('deve renderizar botão de registrar parada', () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );
      expect(screen.getByTestId('btn-registrar-parada')).toBeInTheDocument();
    });
  });

  describe('Carregamento de dados', () => {
    it('deve carregar lista de usinas ao montar', async () => {
      render(
        <StoppedMachines
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
        <StoppedMachines
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

    it('deve exibir mensagem quando não há máquinas paradas', async () => {
      const mockEmptyLoad = vi.fn().mockResolvedValue([]);
      render(
        <StoppedMachines
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
        expect(screen.getByText('Nenhuma máquina parada encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de formulário', () => {
    it('deve abrir diálogo ao clicar em registrar parada', async () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      const btnNovo = screen.getByTestId('btn-registrar-parada');
      fireEvent.click(btnNovo);

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });
      expect(screen.getByText('Registrar Parada')).toBeInTheDocument();
    });

    it('deve fechar diálogo ao clicar em cancelar', async () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

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
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

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

  describe('Registrar parada', () => {
    it('deve registrar parada com dados válidos', async () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

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
      fireEvent.change(screen.getByTestId('form-tipo-parada'), {
        target: { value: 'PROGRAMADA' },
      });
      fireEvent.change(screen.getByTestId('form-motivo-parada'), {
        target: { value: 'Manutenção Preventiva' },
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

    it('deve exibir erro ao registrar sem preencher campos obrigatórios', async () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

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
        <StoppedMachines
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
      expect(screen.getByText('Editar Parada')).toBeInTheDocument();
    });
  });

  describe('Verificação de campos específicos', () => {
    it('deve renderizar campos de tipo e motivo de parada', async () => {
      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockOnUpdateStatus}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

      await waitFor(() => {
        expect(screen.getByTestId('form-tipo-parada')).toBeInTheDocument();
        expect(screen.getByTestId('form-motivo-parada')).toBeInTheDocument();
      });
    });
  });

  describe('Tratamento de erros', () => {
    it('deve exibir erro ao falhar no carregamento de usinas', async () => {
      const mockError = vi.fn().mockRejectedValue(new Error('Erro ao carregar'));

      render(
        <StoppedMachines
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

    it('deve exibir erro ao falhar no registro', async () => {
      const mockUpdateError = vi.fn().mockRejectedValue(new Error('Erro ao registrar'));

      render(
        <StoppedMachines
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadMachines={mockOnLoadMachines}
          onUpdateStatus={mockUpdateError}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-registrar-parada'));

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
      fireEvent.change(screen.getByTestId('form-tipo-parada'), {
        target: { value: 'PROGRAMADA' },
      });
      fireEvent.change(screen.getByTestId('form-motivo-parada'), {
        target: { value: 'Manutenção Preventiva' },
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

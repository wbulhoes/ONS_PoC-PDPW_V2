import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import UnitOutage from '../../src/pages/Collection/Maintenance/UnitOutage';
import type { UnitOutage as UnitOutageType } from '../../src/types/unitOutage';

describe('UnitOutage Component', () => {
  const mockUsinas = [
    { id: 1, nome: 'UHE Itaipu' },
    { id: 2, nome: 'UHE Belo Monte' },
  ];

  const mockUnidades = [
    { id: 1, nome: 'Unidade 1', usinaId: 1 },
    { id: 2, nome: 'Unidade 2', usinaId: 1 },
  ];

  const mockOutages: UnitOutageType[] = [
    {
      id: 1,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 1,
      unidadeNome: 'Unidade 1',
      tipoParada: 'PROGRAMADA',
      dataInicio: '2024-01-15',
      dataFim: '2024-01-20',
      motivoParada: 'Manutenção preventiva',
      observacao: 'Substituição de peças',
      status: 'ATIVA',
    },
    {
      id: 2,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 2,
      unidadeNome: 'Unidade 2',
      tipoParada: 'FORCADA',
      dataInicio: '2024-01-16',
      dataFim: '2024-01-18',
      motivoParada: 'Falha no gerador',
      observacao: 'Reparo emergencial',
      status: 'ENCERRADA',
    },
  ];

  const mockOnLoadUsinas = vi.fn().mockResolvedValue(mockUsinas);
  const mockOnLoadUnidades = vi.fn().mockResolvedValue(mockUnidades);
  const mockOnLoadOutages = vi.fn().mockResolvedValue(mockOutages);
  const mockOnSave = vi.fn().mockResolvedValue(undefined);
  const mockOnDelete = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Coleta - Paradas de Unidades')).toBeInTheDocument();
    });

    it('deve renderizar filtros corretamente', () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('filter-data-pdp')).toBeInTheDocument();
      expect(screen.getByTestId('filter-usina')).toBeInTheDocument();
      expect(screen.getByTestId('filter-status')).toBeInTheDocument();
    });

    it('deve renderizar botão de nova parada', () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('btn-nova-parada')).toBeInTheDocument();
    });
  });

  describe('Carregamento de dados', () => {
    it('deve carregar lista de usinas ao montar', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });
    });

    it('deve carregar paradas quando filtros são aplicados', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      const filterDataPdp = screen.getByTestId('filter-data-pdp');
      fireEvent.change(filterDataPdp, { target: { value: '2024-01-15' } });

      await waitFor(() => {
        expect(mockOnLoadOutages).toHaveBeenCalledWith({
          dataPdp: '2024-01-15',
          usinaId: 0,
          status: '',
        });
      });
    });

    it('deve exibir mensagem quando não há paradas', async () => {
      const mockEmptyLoad = vi.fn().mockResolvedValue([]);
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockEmptyLoad}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-data-pdp'), {
        target: { value: '2024-01-15' },
      });

      await waitFor(() => {
        expect(screen.getByText('Nenhuma parada encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de formulário', () => {
    it('deve abrir diálogo ao clicar em nova parada', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      const btnNova = screen.getByTestId('btn-nova-parada');
      fireEvent.click(btnNova);

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });
      expect(screen.getByText('Nova Parada')).toBeInTheDocument();
    });

    it('deve fechar diálogo ao clicar em cancelar', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-parada'));

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
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-parada'));

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

  describe('Salvar parada', () => {
    it('deve salvar nova parada com dados válidos', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-parada'));

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
      fireEvent.change(screen.getByTestId('form-data-inicio'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-data-fim'), {
        target: { value: '2024-01-20' },
      });
      fireEvent.change(screen.getByTestId('form-motivo-parada'), {
        target: { value: 'Manutenção' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalled();
      });
      await waitFor(() => {
        expect(screen.getByTestId('success-message')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao salvar sem preencher campos obrigatórios', async () => {
      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-parada'));

      await waitFor(() => {
        expect(screen.getByTestId('btn-save')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
      expect(mockOnSave).not.toHaveBeenCalled();
    });
  });

  describe('Excluir parada', () => {
    it('deve excluir parada com confirmação', async () => {
      const mockConfirm = vi.spyOn(window, 'confirm').mockReturnValue(true);

      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-data-pdp'), {
        target: { value: '2024-01-15' },
      });

      await waitFor(() => {
        expect(mockOnLoadOutages).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      await waitFor(() => {
        expect(mockConfirm).toHaveBeenCalled();
        expect(mockOnDelete).toHaveBeenCalledWith(1);
      });

      mockConfirm.mockRestore();
    });
  });

  describe('Tratamento de erros', () => {
    it('deve exibir erro ao falhar no carregamento de usinas', async () => {
      const mockError = vi.fn().mockRejectedValue(new Error('Erro ao carregar'));

      render(
        <UnitOutage
          onLoadUsinas={mockError}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar no salvamento', async () => {
      const mockSaveError = vi.fn().mockRejectedValue(new Error('Erro ao salvar'));

      render(
        <UnitOutage
          onLoadUsinas={mockOnLoadUsinas}
          onLoadUnidades={mockOnLoadUnidades}
          onLoadOutages={mockOnLoadOutages}
          onSave={mockSaveError}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-parada'));

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
        target: { value: 'Teste' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
      });
    });
  });
});

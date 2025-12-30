import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import UnitMaintenance from '../../src/pages/Collection/Maintenance/UnitMaintenance';

describe('UnitMaintenance Component', () => {
  const mockUsinas = [
    { id: 1, nome: 'UHE Itaipu' },
    { id: 2, nome: 'UHE Belo Monte' },
  ];

  const mockUnidades = [
    { id: 1, nome: 'UG01', usinaId: 1 },
    { id: 2, nome: 'UG02', usinaId: 1 },
  ];

  const mockMaintenances = [
    {
      id: 1,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      unidadeGeradoraId: 1,
      unidadeNome: 'UG01',
      tipoManutencao: 'PREVENTIVA',
      dataInicio: '2024-01-15',
      dataFim: '2024-01-20',
      observacao: 'Manutenção anual',
      status: 'PROGRAMADA',
    },
  ];

  const mockOnLoadUsinas = vi.fn().mockResolvedValue(mockUsinas);
  const mockOnLoadUnidades = vi.fn().mockResolvedValue(mockUnidades);
  const mockOnLoadMaintenances = vi.fn().mockResolvedValue(mockMaintenances);
  const mockOnSave = vi.fn().mockResolvedValue(undefined);
  const mockOnDelete = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o componente', () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );
    expect(screen.getByText('Coleta - Manutenção de Unidades Geradoras')).toBeInTheDocument();
  });

  it('deve carregar usinas ao montar', async () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );
    await waitFor(() => {
      expect(mockOnLoadUsinas).toHaveBeenCalled();
    });
  });

  it('deve abrir diálogo ao clicar em Nova Manutenção', async () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.click(screen.getByTestId('btn-nova-manutencao'));

    await waitFor(() => {
      expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
    });
  });

  it('deve carregar manutenções ao aplicar filtro', async () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.change(screen.getByTestId('filter-data-pdp'), {
      target: { value: '2024-01-15' },
    });

    await waitFor(() => {
      expect(mockOnLoadMaintenances).toHaveBeenCalled();
    });
  });

  it('deve carregar unidades ao selecionar usina no formulário', async () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.click(screen.getByTestId('btn-nova-manutencao'));

    await waitFor(() => {
      expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
    });

    fireEvent.change(screen.getByTestId('form-usina'), {
      target: { value: '1' },
    });

    await waitFor(() => {
      expect(mockOnLoadUnidades).toHaveBeenCalledWith(1);
    });
  });

  it('deve salvar manutenção com dados corretos', async () => {
    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.click(screen.getByTestId('btn-nova-manutencao'));

    await waitFor(() => {
      expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
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
    fireEvent.change(screen.getByTestId('form-tipo-manutencao'), {
      target: { value: 'PREVENTIVA' },
    });

    fireEvent.click(screen.getByTestId('btn-save'));

    await waitFor(() => {
      expect(mockOnSave).toHaveBeenCalled();
    });
  });

  it('deve excluir manutenção ao confirmar', async () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);

    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={mockOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.change(screen.getByTestId('filter-data-pdp'), {
      target: { value: '2024-01-15' },
    });

    await waitFor(() => {
      expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
    });

    fireEvent.click(screen.getByTestId('btn-delete-1'));

    await waitFor(() => {
      expect(mockOnDelete).toHaveBeenCalledWith(1);
    });

    confirmSpy.mockRestore();
  });

  it('deve exibir erro ao falhar ao salvar', async () => {
    const errorOnSave = vi.fn().mockRejectedValue(new Error('Erro'));

    render(
      <UnitMaintenance
        onLoadUsinas={mockOnLoadUsinas}
        onLoadUnidades={mockOnLoadUnidades}
        onLoadMaintenances={mockOnLoadMaintenances}
        onSave={errorOnSave}
        onDelete={mockOnDelete}
      />
    );

    await waitFor(() => expect(mockOnLoadUsinas).toHaveBeenCalled());

    fireEvent.click(screen.getByTestId('btn-nova-manutencao'));

    await waitFor(() => {
      expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
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
    fireEvent.change(screen.getByTestId('form-tipo-manutencao'), {
      target: { value: 'PREVENTIVA' },
    });

    fireEvent.click(screen.getByTestId('btn-save'));

    await waitFor(() => {
      expect(screen.getByTestId('error-message')).toBeInTheDocument();
    });
  });
});

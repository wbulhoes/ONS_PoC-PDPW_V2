import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PlantRestriction from '../../src/pages/Collection/Restrictions/PlantRestriction';
import type { PlantRestriction as PlantRestrictionType, Usina } from '../../src/types/plantRestriction';

describe('PlantRestriction Component', () => {
  const mockUsinas: Usina[] = [
    { id: 1, nome: 'UHE Itaipu', sigla: 'ITAIPU', tipo: 'HIDR' },
    { id: 2, nome: 'UHE Belo Monte', sigla: 'BELO_MONTE', tipo: 'HIDR' },
    { id: 3, nome: 'UTE Angra 1', sigla: 'ANGRA1', tipo: 'TERM' },
  ];

  const mockRestrictions: PlantRestrictionType[] = [
    {
      id: 1,
      dataPdp: '2024-01-15',
      usinaId: 1,
      usinaNome: 'UHE Itaipu',
      tipoRestricao: 'MANUTENCAO_PROGRAMADA',
      dataInicio: '2024-01-15',
      dataFim: '2024-01-20',
      potenciaMaxima: 14000,
      potenciaMinima: 0,
      observacao: 'Manutenção preventiva',
      status: 'ATIVA',
    },
    {
      id: 2,
      dataPdp: '2024-01-15',
      usinaId: 2,
      usinaNome: 'UHE Belo Monte',
      tipoRestricao: 'RESTRICAO_HIDROLOGICA',
      dataInicio: '2024-01-15',
      dataFim: '2024-01-25',
      potenciaMaxima: 11233,
      potenciaMinima: 0,
      observacao: 'Nível baixo do reservatório',
      status: 'ATIVA',
    },
  ];

  const mockOnLoadUsinas = vi.fn().mockResolvedValue(mockUsinas);
  const mockOnLoadRestrictions = vi.fn().mockResolvedValue(mockRestrictions);
  const mockOnSave = vi.fn().mockResolvedValue(undefined);
  const mockOnDelete = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Coleta - Restrições de Usinas')).toBeInTheDocument();
    });

    it('deve renderizar filtros corretamente', () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('filter-data-pdp')).toBeInTheDocument();
      expect(screen.getByTestId('filter-usina')).toBeInTheDocument();
      expect(screen.getByTestId('filter-status')).toBeInTheDocument();
    });

    it('deve renderizar botão de nova restrição', () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('btn-nova-restricao')).toBeInTheDocument();
    });

    it('deve carregar lista de usinas ao montar', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });
    });
  });

  describe('Filtros', () => {
    it('deve carregar restrições ao aplicar filtro de data', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(mockOnLoadRestrictions).toHaveBeenCalledWith(
          expect.objectContaining({ dataPdp: '2024-01-15' })
        );
      });
    });

    it('deve carregar restrições ao aplicar filtro de usina', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-usina'), {
        target: { value: '1' },
      });

      await waitFor(() => {
        expect(mockOnLoadRestrictions).toHaveBeenCalledWith(
          expect.objectContaining({ usinaId: 1 })
        );
      });
    });

    it('deve carregar restrições ao aplicar filtro de status', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-status'), {
        target: { value: 'ATIVA' },
      });

      await waitFor(() => {
        expect(mockOnLoadRestrictions).toHaveBeenCalledWith(
          expect.objectContaining({ status: 'ATIVA' })
        );
      });
    });
  });

  describe('Tabela de Restrições', () => {
    it('deve exibir mensagem quando não há restrições', async () => {
      mockOnLoadRestrictions.mockResolvedValueOnce([]);
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(mockOnLoadRestrictions).toHaveBeenCalled();
      });

      await waitFor(() => {
        expect(screen.getByText('Nenhuma restrição encontrada')).toBeInTheDocument();
      });
    });

    it('deve renderizar lista de restrições', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByText('UHE Itaipu')).toBeInTheDocument();
        expect(screen.getByText('UHE Belo Monte')).toBeInTheDocument();
      });
    });

    it('deve exibir botões de ação para cada restrição', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument();
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de Criação/Edição', () => {
    it('deve abrir diálogo ao clicar em Nova Restrição', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
        expect(screen.getByText('Nova Restrição')).toBeInTheDocument();
      });
    });

    it('deve fechar diálogo ao clicar em Cancelar', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-cancel'));

      await waitFor(() => {
        expect(screen.queryByTestId('dialog-form')).not.toBeInTheDocument();
      });
    });

    it('deve fechar diálogo ao clicar no X', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-close-dialog'));

      await waitFor(() => {
        expect(screen.queryByTestId('dialog-form')).not.toBeInTheDocument();
      });
    });

    it('deve preencher formulário corretamente', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-tipo-restricao'), {
        target: { value: 'MANUTENCAO_PROGRAMADA' },
      });

      expect(screen.getByTestId('form-data-pdp')).toHaveValue('2024-01-15');
      expect(screen.getByTestId('form-usina')).toHaveValue('1');
      expect(screen.getByTestId('form-tipo-restricao')).toHaveValue('MANUTENCAO_PROGRAMADA');
    });

    it('deve exibir erro ao tentar salvar sem campos obrigatórios', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Preencha todos os campos obrigatórios')).toBeInTheDocument();
      });
    });

    it('deve chamar onSave com dados corretos ao criar restrição', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-tipo-restricao'), {
        target: { value: 'MANUTENCAO_PROGRAMADA' },
      });
      fireEvent.change(screen.getByTestId('form-data-inicio'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-data-fim'), {
        target: { value: '2024-01-20' },
      });
      fireEvent.change(screen.getByTestId('form-potencia-minima'), {
        target: { value: '0' },
      });
      fireEvent.change(screen.getByTestId('form-potencia-maxima'), {
        target: { value: '14000' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          expect.objectContaining({
            dataPdp: '2024-01-15',
            usinaId: 1,
            usinaNome: 'UHE Itaipu',
            tipoRestricao: 'MANUTENCAO_PROGRAMADA',
            dataInicio: '2024-01-15',
            dataFim: '2024-01-20',
          })
        );
      });
    });

    it('deve exibir mensagem de sucesso ao criar restrição', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-tipo-restricao'), {
        target: { value: 'MANUTENCAO_PROGRAMADA' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('success-message')).toBeInTheDocument();
        expect(screen.getByText('Restrição criada com sucesso')).toBeInTheDocument();
      });
    });

    it('deve abrir diálogo em modo edição ao clicar em editar', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-edit-1'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
        expect(screen.getByText('Editar Restrição')).toBeInTheDocument();
        expect(screen.getByTestId('form-data-pdp')).toHaveValue('2024-01-15');
      });
    });

    it('deve chamar onSave com dados corretos ao editar restrição', async () => {
      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-edit-1'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-observacao'), {
        target: { value: 'Observação atualizada' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          expect.objectContaining({
            id: 1,
            observacao: 'Observação atualizada',
          })
        );
      });
    });
  });

  describe('Exclusão de Restrições', () => {
    it('deve chamar onDelete ao confirmar exclusão', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);

      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      await waitFor(() => {
        expect(mockOnDelete).toHaveBeenCalledWith(1);
      });

      confirmSpy.mockRestore();
    });

    it('não deve chamar onDelete ao cancelar exclusão', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);

      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      expect(mockOnDelete).not.toHaveBeenCalled();

      confirmSpy.mockRestore();
    });

    it('deve exibir mensagem de sucesso após exclusão', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);

      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
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
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      await waitFor(() => {
        expect(screen.getByTestId('success-message')).toBeInTheDocument();
        expect(screen.getByText('Restrição excluída com sucesso')).toBeInTheDocument();
      });

      confirmSpy.mockRestore();
    });
  });

  describe('Tratamento de Erros', () => {
    it('deve exibir erro ao falhar ao carregar usinas', async () => {
      const errorOnLoadUsinas = vi.fn().mockRejectedValue(new Error('Erro de conexão'));

      render(
        <PlantRestriction
          onLoadUsinas={errorOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Erro ao carregar dados das usinas')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar ao salvar restrição', async () => {
      const errorOnSave = vi.fn().mockRejectedValue(new Error('Erro ao salvar'));

      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={errorOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.click(screen.getByTestId('btn-nova-restricao'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('form-data-pdp'), {
        target: { value: '2024-01-15' },
      });
      fireEvent.change(screen.getByTestId('form-usina'), {
        target: { value: '1' },
      });
      fireEvent.change(screen.getByTestId('form-tipo-restricao'), {
        target: { value: 'MANUTENCAO_PROGRAMADA' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Erro ao salvar restrição')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar ao excluir restrição', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
      const errorOnDelete = vi.fn().mockRejectedValue(new Error('Erro ao excluir'));

      render(
        <PlantRestriction
          onLoadUsinas={mockOnLoadUsinas}
          onLoadRestrictions={mockOnLoadRestrictions}
          onSave={mockOnSave}
          onDelete={errorOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsinas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('filter-data-pdp'), {
        target: { value: '2024-01-15' },
      });

      await waitFor(() => {
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Erro ao excluir restrição')).toBeInTheDocument();
      });

      confirmSpy.mockRestore();
    });
  });
});

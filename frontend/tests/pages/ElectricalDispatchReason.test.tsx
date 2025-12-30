import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ElectricalDispatchReason from '../../src/pages/Administration/ElectricalDispatchReason';

describe('ElectricalDispatchReason Component', () => {
  const mockReasons = [
    { id: 1, codigo: 'RE001', descricao: 'Suprimento de carga', ativo: true },
    { id: 2, codigo: 'RE002', descricao: 'Estabilidade da rede', ativo: true },
    { id: 3, codigo: 'RE003', descricao: 'Regulação de tensão', ativo: false },
  ];

  const mockOnLoadReasons = vi.fn().mockResolvedValue(mockReasons);
  const mockOnSave = vi.fn().mockResolvedValue(undefined);
  const mockOnDelete = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente', () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Cadastro - Motivos de Despacho Razão Elétrica')).toBeInTheDocument();
    });

    it('deve carregar motivos ao montar', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      await waitFor(() => {
        expect(mockOnLoadReasons).toHaveBeenCalled();
      });
    });

    it('deve renderizar filtro de status', () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('filter-ativo')).toBeInTheDocument();
    });

    it('deve renderizar botão de novo motivo', () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('btn-novo-motivo')).toBeInTheDocument();
    });
  });

  describe('Tabela de Motivos', () => {
    it('deve exibir lista de motivos', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('RE001')).toBeInTheDocument();
        expect(screen.getByText('Suprimento de carga')).toBeInTheDocument();
      });
    });

    it('deve filtrar motivos por status ativo', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('RE001')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('filter-ativo'), {
        target: { value: 'false' },
      });

      await waitFor(() => {
        expect(screen.queryByText('RE001')).not.toBeInTheDocument();
        expect(screen.getByText('RE003')).toBeInTheDocument();
      });
    });

    it('deve exibir botões de ação para cada motivo', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument();
        expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de Criação/Edição', () => {
    it('deve abrir diálogo ao clicar em Novo Motivo', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
        expect(screen.getByText('Novo Motivo')).toBeInTheDocument();
      });
    });

    it('deve fechar diálogo ao clicar em Cancelar', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-cancel'));

      await waitFor(() => {
        expect(screen.queryByTestId('dialog-form')).not.toBeInTheDocument();
      });
    });

    it('deve preencher formulário corretamente', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), {
        target: { value: 'RE999' },
      });
      fireEvent.change(screen.getByTestId('form-descricao'), {
        target: { value: 'Teste de motivo' },
      });

      expect(screen.getByTestId('form-codigo')).toHaveValue('RE999');
      expect(screen.getByTestId('form-descricao')).toHaveValue('Teste de motivo');
    });

    it('deve exibir erro ao salvar sem campos obrigatórios', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Preencha todos os campos obrigatórios')).toBeInTheDocument();
      });
    });

    it('deve salvar motivo com dados corretos', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), {
        target: { value: 'RE999' },
      });
      fireEvent.change(screen.getByTestId('form-descricao'), {
        target: { value: 'Teste de motivo' },
      });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          expect.objectContaining({
            codigo: 'RE999',
            descricao: 'Teste de motivo',
            ativo: true,
          })
        );
      });
    });

    it('deve abrir diálogo em modo edição', async () => {
      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-edit-1'));

      await waitFor(() => {
        expect(screen.getByTestId('dialog-form')).toBeInTheDocument();
        expect(screen.getByText('Editar Motivo')).toBeInTheDocument();
        expect(screen.getByTestId('form-codigo')).toHaveValue('RE001');
      });
    });
  });

  describe('Exclusão', () => {
    it('deve excluir motivo ao confirmar', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);

      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      await waitFor(() => {
        expect(mockOnDelete).toHaveBeenCalledWith(1);
      });

      confirmSpy.mockRestore();
    });

    it('não deve excluir ao cancelar', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);

      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('btn-delete-1')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-delete-1'));

      expect(mockOnDelete).not.toHaveBeenCalled();

      confirmSpy.mockRestore();
    });
  });

  describe('Tratamento de Erros', () => {
    it('deve exibir erro ao falhar ao carregar', async () => {
      const errorOnLoad = vi.fn().mockRejectedValue(new Error('Erro'));

      render(
        <ElectricalDispatchReason
          onLoadReasons={errorOnLoad}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Erro ao carregar motivos de despacho')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar ao salvar', async () => {
      const errorOnSave = vi.fn().mockRejectedValue(new Error('Erro'));

      render(
        <ElectricalDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={errorOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), { target: { value: 'RE999' } });
      fireEvent.change(screen.getByTestId('form-descricao'), { target: { value: 'Teste' } });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByText('Erro ao salvar motivo de despacho')).toBeInTheDocument();
      });
    });
  });
});

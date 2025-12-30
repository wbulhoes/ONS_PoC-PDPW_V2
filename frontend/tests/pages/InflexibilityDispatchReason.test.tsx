import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import InflexibilityDispatchReason from '../../src/pages/Administration/InflexibilityDispatchReason';

describe('InflexibilityDispatchReason Component', () => {
  const mockReasons = [
    { id: 1, codigo: 'INF001', descricao: 'Restrição técnica', tipoInflexibilidade: 'TECNICA', ativo: true },
    { id: 2, codigo: 'INF002', descricao: 'Contrato de fornecimento', tipoInflexibilidade: 'CONTRATUAL', ativo: true },
    { id: 3, codigo: 'INF003', descricao: 'Falta de combustível', tipoInflexibilidade: 'COMBUSTIVEL', ativo: false },
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
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Cadastro - Motivos de Despacho por Inflexibilidade')).toBeInTheDocument();
    });

    it('deve carregar motivos ao montar', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      await waitFor(() => {
        expect(mockOnLoadReasons).toHaveBeenCalled();
      });
    });

    it('deve renderizar filtros', () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('filter-tipo')).toBeInTheDocument();
      expect(screen.getByTestId('filter-ativo')).toBeInTheDocument();
    });
  });

  describe('Tabela de Motivos', () => {
    it('deve exibir lista de motivos', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('INF001')).toBeInTheDocument();
      });
      
      expect(screen.getByText('Restrição técnica')).toBeInTheDocument();
      expect(screen.getByTestId('reasons-table')).toBeInTheDocument();
    });

    it('deve filtrar por tipo de inflexibilidade', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('INF001')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('filter-tipo'), {
        target: { value: 'CONTRATUAL' },
      });

      await waitFor(() => {
        expect(screen.queryByText('INF001')).not.toBeInTheDocument();
        expect(screen.getByText('INF002')).toBeInTheDocument();
      });
    });

    it('deve filtrar por status', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('INF001')).toBeInTheDocument();
      });

      fireEvent.change(screen.getByTestId('filter-ativo'), {
        target: { value: 'false' },
      });

      await waitFor(() => {
        expect(screen.queryByText('INF001')).not.toBeInTheDocument();
        expect(screen.getByText('INF003')).toBeInTheDocument();
      });
    });
  });

  describe('Diálogo de Criação/Edição', () => {
    it('deve abrir diálogo ao clicar em Novo Motivo', async () => {
      render(
        <InflexibilityDispatchReason
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

    it('deve preencher formulário com todos os campos', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), {
        target: { value: 'INF999' },
      });
      fireEvent.change(screen.getByTestId('form-tipo-inflexibilidade'), {
        target: { value: 'TECNICA' },
      });
      fireEvent.change(screen.getByTestId('form-descricao'), {
        target: { value: 'Teste de inflexibilidade' },
      });

      expect(screen.getByTestId('form-codigo')).toHaveValue('INF999');
      expect(screen.getByTestId('form-tipo-inflexibilidade')).toHaveValue('TECNICA');
      expect(screen.getByTestId('form-descricao')).toHaveValue('Teste de inflexibilidade');
    });

    it('deve exibir erro ao salvar sem tipo', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), { target: { value: 'INF999' } });
      fireEvent.change(screen.getByTestId('form-descricao'), { target: { value: 'Teste' } });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(screen.getByText('Preencha todos os campos obrigatórios')).toBeInTheDocument();
      });
    });

    it('deve salvar com dados corretos', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(mockOnLoadReasons).toHaveBeenCalled());

      fireEvent.click(screen.getByTestId('btn-novo-motivo'));
      await waitFor(() => expect(screen.getByTestId('dialog-form')).toBeInTheDocument());

      fireEvent.change(screen.getByTestId('form-codigo'), { target: { value: 'INF999' } });
      fireEvent.change(screen.getByTestId('form-tipo-inflexibilidade'), { target: { value: 'TECNICA' } });
      fireEvent.change(screen.getByTestId('form-descricao'), { target: { value: 'Teste' } });

      fireEvent.click(screen.getByTestId('btn-save'));

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          expect.objectContaining({
            codigo: 'INF999',
            tipoInflexibilidade: 'TECNICA',
            descricao: 'Teste',
            ativo: true,
          })
        );
      });
    });

    it('deve abrir em modo edição', async () => {
      render(
        <InflexibilityDispatchReason
          onLoadReasons={mockOnLoadReasons}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('btn-edit-1')).toBeInTheDocument());

      fireEvent.click(screen.getByTestId('btn-edit-1'));

      await waitFor(() => {
        expect(screen.getByText('Editar Motivo')).toBeInTheDocument();
        expect(screen.getByTestId('form-codigo')).toHaveValue('INF001');
      });
    });
  });

  describe('Exclusão', () => {
    it('deve excluir ao confirmar', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);

      render(
        <InflexibilityDispatchReason
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
  });

  describe('Tratamento de Erros', () => {
    it('deve exibir erro ao falhar ao carregar', async () => {
      const errorOnLoad = vi.fn().mockRejectedValue(new Error('Erro'));

      render(
        <InflexibilityDispatchReason
          onLoadReasons={errorOnLoad}
          onSave={mockOnSave}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('Erro ao carregar motivos de inflexibilidade')).toBeInTheDocument();
      });
    });
  });
});

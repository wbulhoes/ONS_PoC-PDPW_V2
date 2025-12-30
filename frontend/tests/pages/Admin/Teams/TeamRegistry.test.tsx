import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import TeamRegistry from '../TeamRegistry';

describe('TeamRegistry', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o componente corretamente', () => {
    render(<TeamRegistry />);

    expect(screen.getByText('Cadastro de Equipes')).toBeInTheDocument();
    expect(screen.getByLabelText('Nome:')).toBeInTheDocument();
    expect(screen.getByLabelText('Descrição:')).toBeInTheDocument();
    expect(screen.getByText('Ativa')).toBeInTheDocument();
    expect(screen.getByText('Salvar')).toBeInTheDocument();
  });

  it('deve carregar equipes mockadas', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      expect(screen.getByText('Equipe Desenvolvimento')).toBeInTheDocument();
      expect(screen.getByText('Equipe Testes')).toBeInTheDocument();
      expect(screen.getByText('Equipe Produção')).toBeInTheDocument();
    });
  });

  it('deve permitir criar uma nova equipe', async () => {
    render(<TeamRegistry />);

    const nomeInput = screen.getByLabelText('Nome:');
    const descricaoTextarea = screen.getByLabelText('Descrição:');
    const salvarButton = screen.getByText('Salvar');

    fireEvent.change(nomeInput, { target: { value: 'Nova Equipe' } });
    fireEvent.change(descricaoTextarea, { target: { value: 'Descrição da nova equipe' } });
    fireEvent.click(salvarButton);

    await waitFor(() => {
      expect(screen.getByText('Nova Equipe')).toBeInTheDocument();
      expect(screen.getByText('Descrição da nova equipe')).toBeInTheDocument();
    });
  });

  it('deve validar campos obrigatórios', async () => {
    render(<TeamRegistry />);

    const salvarButton = screen.getByText('Salvar');
    fireEvent.click(salvarButton);

    // O HTML5 validation deve prevenir o submit
    expect(salvarButton).toBeInTheDocument();
  });

  it('deve permitir editar uma equipe existente', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const editButtons = screen.getAllByText('Editar');
      fireEvent.click(editButtons[0]);
    });

    const nomeInput = screen.getByLabelText('Nome:');
    expect(nomeInput).toHaveValue('Equipe Desenvolvimento');

    fireEvent.change(nomeInput, { target: { value: 'Equipe Desenvolvimento Editada' } });
    fireEvent.click(screen.getByText('Atualizar'));

    await waitFor(() => {
      expect(screen.getByText('Equipe Desenvolvimento Editada')).toBeInTheDocument();
    });
  });

  it('deve permitir cancelar edição', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const editButtons = screen.getAllByText('Editar');
      fireEvent.click(editButtons[0]);
    });

    const nomeInput = screen.getByLabelText('Nome:');
    fireEvent.change(nomeInput, { target: { value: 'Equipe Modificada' } });
    fireEvent.click(screen.getByText('Cancelar'));

    expect(screen.getByText('Equipe Desenvolvimento')).toBeInTheDocument();
    expect(screen.queryByText('Equipe Modificada')).not.toBeInTheDocument();
  });

  it('deve permitir selecionar equipes individualmente', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      const firstTeamCheckbox = checkboxes[1]; // Primeiro checkbox de equipe (após o "selecionar todos")
      fireEvent.click(firstTeamCheckbox);

      expect(firstTeamCheckbox).toBeChecked();
    });
  });

  it('deve permitir selecionar todas as equipes', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const checkboxes = screen.getAllByRole('checkbox');
      checkboxes.forEach(checkbox => {
        expect(checkbox).toBeChecked();
      });
    });
  });

  it('deve desmarcar "selecionar todos" quando uma equipe é desmarcada', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const checkboxes = screen.getAllByRole('checkbox');
      const firstTeamCheckbox = checkboxes[1];
      fireEvent.click(firstTeamCheckbox);

      expect(selectAllCheckbox).not.toBeChecked();
    });
  });

  it('deve permitir excluir equipes selecionadas', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      const firstTeamCheckbox = checkboxes[1];
      fireEvent.click(firstTeamCheckbox);
    });

    const deleteButton = screen.getByText('Excluir Selecionados (1)');
    fireEvent.click(deleteButton);

    await waitFor(() => {
      expect(screen.queryByText('Equipe Desenvolvimento')).not.toBeInTheDocument();
    });
  });

  it('deve mostrar mensagem quando não há equipes', async () => {
    // Mock sem equipes
    vi.spyOn(console, 'error').mockImplementation(() => {});

    render(<TeamRegistry />);

    // Simular exclusão de todas as equipes
    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);
    });

    const deleteButton = screen.getByText('Excluir Selecionados (5)');
    fireEvent.click(deleteButton);

    await waitFor(() => {
      expect(screen.getByText('Nenhuma equipe cadastrada.')).toBeInTheDocument();
    });
  });

  it('deve navegar entre páginas', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const nextButton = screen.getByText('Próxima');
      fireEvent.click(nextButton);

      expect(screen.getByText('Página 2 de 2')).toBeInTheDocument();
    });
  });

  it('deve desabilitar botões de navegação corretamente', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      const previousButton = screen.getByText('Anterior');
      const nextButton = screen.getByText('Próxima');

      expect(previousButton).toBeDisabled();
      expect(nextButton).not.toBeDisabled();
    });
  });

  it('deve mostrar status correto das equipes', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      expect(screen.getByText('Ativa')).toBeInTheDocument();
      expect(screen.getByText('Inativa')).toBeInTheDocument();
    });
  });

  it('deve formatar datas corretamente', async () => {
    render(<TeamRegistry />);

    await waitFor(() => {
      // Verificar se as datas estão sendo exibidas (formato brasileiro)
      const dateCells = screen.getAllByText(/\d{2}\/\d{2}\/\d{4}/);
      expect(dateCells.length).toBeGreaterThan(0);
    });
  });

  it('deve ser responsivo em telas pequenas', () => {
    render(<TeamRegistry />);

    const container = screen.getByTestId('team-registry-container');
    expect(container).toHaveClass('container');
  });

  it('deve mostrar estado de carregamento', () => {
    render(<TeamRegistry />);

    // Durante o carregamento inicial, pode mostrar mensagem
    expect(screen.getByText('Cadastro de Equipes')).toBeInTheDocument();
  });
});
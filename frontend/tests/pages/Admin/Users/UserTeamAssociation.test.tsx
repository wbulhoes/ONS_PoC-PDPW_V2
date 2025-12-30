import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import UserTeamAssociation from '../UserTeamAssociation';

describe('UserTeamAssociation', () => {
  const mockUsers = [
    { id: 1, nome: 'João Silva', login: 'joao.silva', email: 'joao@email.com' },
    { id: 2, nome: 'Maria Santos', login: 'maria.santos', email: 'maria@email.com' },
    { id: 3, nome: 'Pedro Costa', login: 'pedro.costa', email: 'pedro@email.com' },
  ];

  const mockTeams = [
    { id: 1, nome: 'Equipe A', descricao: 'Equipe de Desenvolvimento' },
    { id: 2, nome: 'Equipe B', descricao: 'Equipe de Testes' },
    { id: 3, nome: 'Equipe C', descricao: 'Equipe de Produção' },
  ];

  const mockAssociatedUsers = [
    { id: 1, nome: 'João Silva', login: 'joao.silva', equipe: 'Equipe A' },
    { id: 2, nome: 'Maria Santos', login: 'maria.santos', equipe: 'Equipe B' },
  ];

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o componente corretamente', () => {
    render(<UserTeamAssociation />);

    expect(screen.getByText('Associação Usuário x Equipe')).toBeInTheDocument();
    expect(screen.getByLabelText('Equipe:')).toBeInTheDocument();
    expect(screen.getByText('Associar')).toBeInTheDocument();
    expect(screen.getByText('Desassociar')).toBeInTheDocument();
  });

  it('deve carregar usuários associados quando uma equipe é selecionada', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      expect(screen.getByText('João Silva')).toBeInTheDocument();
      expect(screen.getByText('Maria Santos')).toBeInTheDocument();
    });
  });

  it('deve permitir selecionar usuários para associação', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      expect(checkboxes).toHaveLength(2);
    });

    const firstCheckbox = screen.getAllByRole('checkbox')[0];
    fireEvent.click(firstCheckbox);

    expect(firstCheckbox).toBeChecked();
  });

  it('deve permitir selecionar todos os usuários', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const checkboxes = screen.getAllByRole('checkbox');
      checkboxes.forEach(checkbox => {
        expect(checkbox).toBeChecked();
      });
    });
  });

  it('deve desmarcar "selecionar todos" quando um usuário é desmarcado', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const firstUserCheckbox = screen.getAllByRole('checkbox')[1];
      fireEvent.click(firstUserCheckbox);

      expect(selectAllCheckbox).not.toBeChecked();
    });
  });

  it('deve mostrar mensagem de carregamento', () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    expect(screen.getByText('Carregando usuários...')).toBeInTheDocument();
  });

  it('deve mostrar mensagem quando não há usuários associados', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '3' } });

    await waitFor(() => {
      expect(screen.getByText('Nenhum usuário associado a esta equipe.')).toBeInTheDocument();
    });
  });

  it('deve permitir associar usuários selecionados', async () => {
    const mockAssociate = vi.fn();
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      fireEvent.click(checkboxes[1]);
    });

    const associateButton = screen.getByText('Associar');
    fireEvent.click(associateButton);

    await waitFor(() => {
      expect(mockAssociate).toHaveBeenCalled();
    });
  });

  it('deve permitir desassociar usuários selecionados', async () => {
    const mockDisassociate = vi.fn();
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      fireEvent.click(checkboxes[0]);
    });

    const disassociateButton = screen.getByText('Desassociar');
    fireEvent.click(disassociateButton);

    await waitFor(() => {
      expect(mockDisassociate).toHaveBeenCalled();
    });
  });

  it('deve navegar entre páginas', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const nextButton = screen.getByText('Próxima');
      fireEvent.click(nextButton);

      expect(screen.getByText('Página 2 de 2')).toBeInTheDocument();
    });
  });

  it('deve desabilitar botões de navegação corretamente', async () => {
    render(<UserTeamAssociation />);

    const teamSelect = screen.getByLabelText('Equipe:');
    fireEvent.change(teamSelect, { target: { value: '1' } });

    await waitFor(() => {
      const previousButton = screen.getByText('Anterior');
      const nextButton = screen.getByText('Próxima');

      expect(previousButton).toBeDisabled();
      expect(nextButton).not.toBeDisabled();
    });
  });

  it('deve ser responsivo em telas pequenas', () => {
    render(<UserTeamAssociation />);

    const container = screen.getByTestId('user-team-association-container');
    expect(container).toHaveClass('container');
  });
});
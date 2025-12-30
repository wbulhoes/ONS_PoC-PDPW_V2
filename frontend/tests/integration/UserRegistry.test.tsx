import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { createElement, type ReactNode } from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import UserRegistry from '../../src/pages/Administration/UserRegistry';
import type { UserListResponse, UserPaginationParams } from '../../src/types/user';

function createTestQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: { retry: false },
      mutations: { retry: false },
    },
  });
}

function createWrapper(queryClient: QueryClient) {
  return ({ children }: { children: ReactNode }) =>
    createElement(QueryClientProvider, { client: queryClient, children });
}

function getFormInputs() {
  const inputs = screen.getAllByRole('textbox');
  const [loginInput, nomeInput, emailInput, telefoneInput] = inputs;
  return { loginInput, nomeInput, emailInput, telefoneInput };
}

const initialUsers = [
  { usuar_id: 'ADMIN', usuar_nome: 'Administrador', usuar_email: 'admin@test.com', usuar_telefone: '123' },
  { usuar_id: 'JOAO', usuar_nome: 'Joao', usuar_email: 'joao@test.com', usuar_telefone: '999' },
];

const mockLoadUsers = vi.fn(async (params: UserPaginationParams): Promise<UserListResponse> => {
  return {
    sucesso: true,
    total: initialUsers.length,
    usuarios: initialUsers,
  };
});

const mockSaveUser = vi.fn(async (data: any, mode: string) => {
  return { sucesso: true, mensagem: mode === 'edit' ? 'Usuário alterado com sucesso!' : 'Usuário incluído com sucesso!' };
});

const mockDeleteUsers = vi.fn(async (userIds: string[]) => {
  return { sucesso: true, mensagem: 'Usuário(s) excluído(s) com sucesso!' };
});

describe('UserRegistry Component Integration', () => {
  let queryClient: QueryClient;

  beforeEach(() => {
    queryClient = createTestQueryClient();
    mockLoadUsers.mockReset();
    mockLoadUsers.mockResolvedValue({
      sucesso: true,
      total: initialUsers.length,
      usuarios: initialUsers,
    });
    mockSaveUser.mockReset();
    mockSaveUser.mockResolvedValue({ sucesso: true, mensagem: 'Usuário incluído com sucesso!' });
    mockDeleteUsers.mockReset();
    mockDeleteUsers.mockResolvedValue({ sucesso: true, mensagem: 'Usuário(s) excluído(s) com sucesso!' });
    vi.clearAllMocks();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('renders search form and buttons', () => {
    render(<UserRegistry />, { wrapper: createWrapper(queryClient) });

    expect(screen.getByText('Cadastro de Usuários')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Pesquisar' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Alterar' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Salvar' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Excluir' })).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Cancelar' })).toBeInTheDocument();
  });

  it('loads users when clicking Pesquisar button', async () => {
    render(<UserRegistry onLoadUsers={mockLoadUsers} />, { wrapper: createWrapper(queryClient) });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    await waitFor(() => {
      expect(screen.getByText('ADMIN')).toBeInTheDocument();
      expect(screen.getByText('Administrador')).toBeInTheDocument();
    });
  });

  it('shows error banner when list fails', async () => {
    mockLoadUsers.mockRejectedValue(new Error('Load failed'));

    render(<UserRegistry onLoadUsers={mockLoadUsers} />, { wrapper: createWrapper(queryClient) });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    await waitFor(() => {
      expect(screen.getByText('Erro ao carregar usuários')).toBeInTheDocument();
    });
  });

  it('displays error banner with retry button on failure', async () => {
    mockLoadUsers.mockRejectedValueOnce(new Error('Load failed'));

    render(<UserRegistry onLoadUsers={mockLoadUsers} />, { wrapper: createWrapper(queryClient) });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    await waitFor(() => {
      expect(screen.getByText('Erro ao carregar usuários')).toBeInTheDocument();
    });

    const retryButton = screen.getByText('Tentar novamente');
    expect(retryButton).toBeInTheDocument();
  });

  it('allows user to select and edit user', async () => {
    render(<UserRegistry onLoadUsers={mockLoadUsers} onSaveUser={mockSaveUser} />, {
      wrapper: createWrapper(queryClient),
    });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    await waitFor(() => {
      expect(screen.getByText('ADMIN')).toBeInTheDocument();
    });

    const checkboxes = screen.getAllByRole('checkbox');
    await userEvent.click(checkboxes[0]);

    const alterarButton = screen.getByRole('button', { name: 'Alterar' });
    await userEvent.click(alterarButton);

    expect(screen.getByDisplayValue('ADMIN')).toBeInTheDocument();
  });

  it('creates new user and shows success message', async () => {
    const user = userEvent.setup();
    render(<UserRegistry onSaveUser={mockSaveUser} onLoadUsers={mockLoadUsers} />, {
      wrapper: createWrapper(queryClient),
    });

    const { loginInput, nomeInput, emailInput, telefoneInput } = getFormInputs();

    await user.type(loginInput, 'NOVO');
    await user.type(nomeInput, 'Novo Usuario');
    await user.type(emailInput, 'novo@test.com');
    await user.type(telefoneInput, '555');

    const salvarButton = screen.getByRole('button', { name: 'Salvar' });
    await userEvent.click(salvarButton);

    await waitFor(() => {
      expect(mockSaveUser).toHaveBeenCalledWith(expect.objectContaining({
        usuar_id: 'NOVO',
        usuar_nome: 'Novo Usuario',
        usuar_email: 'novo@test.com',
        usuar_telefone: '555',
      }), 'create');
    });

    await waitFor(() => {
      expect(screen.getByText('Usuário incluído com sucesso!')).toBeInTheDocument();
    });
  });

  it('validates form fields before saving', async () => {
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
    render(<UserRegistry onSaveUser={mockSaveUser} />, { wrapper: createWrapper(queryClient) });

    const salvarButton = screen.getByRole('button', { name: 'Salvar' });
    await userEvent.click(salvarButton);

    expect(alertSpy).toHaveBeenCalledWith('Não foi possível incluir o usuário! Preencha todos os campos.');
    expect(mockSaveUser).not.toHaveBeenCalled();
  });

  it('deletes selected users and shows success message', async () => {
    const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
    render(<UserRegistry onLoadUsers={mockLoadUsers} onDeleteUsers={mockDeleteUsers} />, {
      wrapper: createWrapper(queryClient),
    });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    await waitFor(() => {
      expect(screen.getByText('ADMIN')).toBeInTheDocument();
    });

    const checkboxes = screen.getAllByRole('checkbox');
    await userEvent.click(checkboxes[0]);

    const excluirButton = screen.getByRole('button', { name: 'Excluir' });
    await userEvent.click(excluirButton);

    await waitFor(() => {
      expect(mockDeleteUsers).toHaveBeenCalledWith(['ADMIN']);
    });

    expect(confirmSpy).toHaveBeenCalled();
  });

  it('shows success message and auto-dismisses after 3 seconds', async () => {
    render(<UserRegistry onSaveUser={mockSaveUser} />, { wrapper: createWrapper(queryClient) });

    const { loginInput, nomeInput, emailInput, telefoneInput } = getFormInputs();

    await userEvent.type(loginInput, 'TEST');
    await userEvent.type(nomeInput, 'Test User');
    await userEvent.type(emailInput, 'test@user.com');
    await userEvent.type(telefoneInput, '123');

    const salvarButton = screen.getByRole('button', { name: 'Salvar' });
    await userEvent.click(salvarButton);

    await screen.findByText('Usuário incluído com sucesso!');
    await waitFor(
      () => {
        expect(screen.queryByText('Usuário incluído com sucesso!')).not.toBeInTheDocument();
      },
      { timeout: 6000 }
    );
  });

  it('shows error message and auto-dismisses after 5 seconds', async () => {
    mockSaveUser.mockRejectedValue({ sucesso: false, mensagem: 'Erro ao salvar' });

    render(<UserRegistry onSaveUser={mockSaveUser} />, { wrapper: createWrapper(queryClient) });

    const { loginInput, nomeInput, emailInput, telefoneInput } = getFormInputs();

    await userEvent.type(loginInput, 'ERR');
    await userEvent.type(nomeInput, 'Error User');
    await userEvent.type(emailInput, 'err@test.com');
    await userEvent.type(telefoneInput, '999');

    const salvarButton = screen.getByRole('button', { name: 'Salvar' });
    await userEvent.click(salvarButton);

    await screen.findByText('Não foi possível salvar o usuário!');
    await waitFor(
      () => {
        expect(screen.queryByText('Não foi possível salvar o usuário!')).not.toBeInTheDocument();
      },
      { timeout: 7000 }
    );
  }, 10000);

  it('shows loading skeleton while fetching', async () => {
    const slowLoad = vi.fn(async () => {
      await new Promise(resolve => setTimeout(resolve, 100));
      return {
        sucesso: true,
        total: 2,
        usuarios: initialUsers,
      };
    });

    render(<UserRegistry onLoadUsers={slowLoad} />, { wrapper: createWrapper(queryClient) });

    const pesquisarButton = screen.getByRole('button', { name: 'Pesquisar' });
    await userEvent.click(pesquisarButton);

    const rowsWhileLoading = screen.getAllByRole('row');
    expect(rowsWhileLoading.length).toBeGreaterThan(1);

    await waitFor(() => {
      expect(screen.getByText('ADMIN')).toBeInTheDocument();
    });
  });

  it('disables alterar/excluir buttons when no users are selected', () => {
    render(<UserRegistry />, { wrapper: createWrapper(queryClient) });

    const alterarButton = screen.getByRole('button', { name: 'Alterar' });
    const excluirButton = screen.getByRole('button', { name: 'Excluir' });

    expect(alterarButton).toBeDisabled();
    expect(excluirButton).toBeDisabled();
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import UserManagementPage from '../../src/pages/Admin/User/UserManagement';
import { userService } from '../../src/services/userService';

// Mock service
vi.mock('../../src/services/userService', () => ({
  userService: {
    getAll: vi.fn(),
    search: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn(),
  },
}));

const mockUsers = [
  {
    usuar_id: 'admin',
    usuar_nome: 'Administrador',
    usuar_email: 'admin@test.com',
    usuar_telefone: '123456'
  }
];

describe('UserManagementPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    (userService.getAll as any).mockResolvedValue(mockUsers);
    // Mock window.alert
    vi.spyOn(window, 'alert').mockImplementation(() => {});
  });

  it('deve renderizar a página e carregar usuários', async () => {
    render(<UserManagementPage />);
    
    expect(screen.getByText('Cadastro de Usuários')).toBeInTheDocument();
    expect(screen.getByText('Carregando...')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText('Administrador')).toBeInTheDocument();
    });
  });

  it('deve preencher formulário ao clicar em Alterar', async () => {
    render(<UserManagementPage />);
    
    await waitFor(() => {
      expect(screen.getByText('Administrador')).toBeInTheDocument();
    });

    const checkbox = screen.getByTestId('checkbox-admin');
    fireEvent.click(checkbox);

    const btnAlterar = screen.getByTestId('btn-alterar');
    fireEvent.click(btnAlterar);

    expect(screen.getByTestId('input-login')).toHaveValue('admin');
    expect(screen.getByTestId('input-nome')).toHaveValue('Administrador');
    expect(screen.getByTestId('btn-salvar')).toHaveTextContent('Salvar Alterações');
  });

  it('deve chamar serviço de criação ao incluir novo usuário', async () => {
    render(<UserManagementPage />);
    
    fireEvent.change(screen.getByTestId('input-login'), { target: { value: 'novo' } });
    fireEvent.change(screen.getByTestId('input-nome'), { target: { value: 'Novo Usuário' } });
    
    fireEvent.click(screen.getByTestId('btn-salvar'));

    await waitFor(() => {
      expect(userService.create).toHaveBeenCalledWith(expect.objectContaining({
        usuar_id: 'novo',
        usuar_nome: 'Novo Usuário'
      }));
    });
  });

  it('deve chamar serviço de exclusão ao clicar em Excluir', async () => {
    // Mock window.confirm
    vi.spyOn(window, 'confirm').mockImplementation(() => true);

    render(<UserManagementPage />);
    
    await waitFor(() => {
      expect(screen.getByText('Administrador')).toBeInTheDocument();
    });

    const checkbox = screen.getByTestId('checkbox-admin');
    fireEvent.click(checkbox);

    const btnExcluir = screen.getByTestId('btn-excluir');
    fireEvent.click(btnExcluir);

    await waitFor(() => {
      expect(userService.delete).toHaveBeenCalledWith('admin');
    });
  });

  it('deve limpar formulário ao clicar em Cancelar', async () => {
    render(<UserManagementPage />);
    
    fireEvent.change(screen.getByTestId('input-login'), { target: { value: 'teste' } });
    fireEvent.click(screen.getByTestId('btn-cancelar'));

    expect(screen.getByTestId('input-login')).toHaveValue('');
  });
});

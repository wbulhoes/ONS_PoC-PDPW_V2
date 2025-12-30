/**
 * Testes para o componente UserRegistry
 * Tela: Cadastro de Usuários (frmCadUsuario.aspx)
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import UserRegistry from '../../src/pages/Administration/UserRegistry';
import { User, UserListResponse, UserFormMode } from '../../src/types/user';

describe('UserRegistry Component', () => {
  
  // Mock data
  const mockUsers: User[] = [
    {
      usuar_id: 'admin',
      usuar_nome: 'Administrador do Sistema',
      usuar_email: 'admin@ons.org.br',
      usuar_telefone: '(21) 3444-9000'
    },
    {
      usuar_id: 'jsilva',
      usuar_nome: 'João da Silva',
      usuar_email: 'joao.silva@ons.org.br',
      usuar_telefone: '(21) 3444-9001'
    },
    {
      usuar_id: 'mferreira',
      usuar_nome: 'Maria Ferreira',
      usuar_email: 'maria.ferreira@ons.org.br',
      usuar_telefone: '(21) 3444-9002'
    },
    {
      usuar_id: 'psantos',
      usuar_nome: 'Pedro Santos',
      usuar_email: 'pedro.santos@ons.org.br',
      usuar_telefone: '(21) 3444-9003'
    }
  ];

  const mockLoadResponse: UserListResponse = {
    sucesso: true,
    usuarios: mockUsers,
    total: mockUsers.length
  };

  describe('Renderização', () => {
    it('deve renderizar o título', () => {
      render(<UserRegistry />);
      expect(screen.getByText('Cadastro de Usuários')).toBeInTheDocument();
    });

    it('deve renderizar todos os campos do formulário', () => {
      render(<UserRegistry />);
      
      expect(screen.getByText(/Login:/)).toBeInTheDocument();
      expect(screen.getByText(/Nome:/)).toBeInTheDocument();
      expect(screen.getByText(/E-mail:/)).toBeInTheDocument();
      expect(screen.getByText(/Telefone:/)).toBeInTheDocument();
    });

    it('deve renderizar todos os botões', () => {
      render(<UserRegistry />);
      
      expect(screen.getByText('Pesquisar')).toBeInTheDocument();
      expect(screen.getByText('Alterar')).toBeInTheDocument();
      expect(screen.getByText('Salvar')).toBeInTheDocument();
      expect(screen.getByText('Excluir')).toBeInTheDocument();
      expect(screen.getByText('Cancelar')).toBeInTheDocument();
    });

    it('deve renderizar a tabela com dados após pesquisa', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      const btnPesquisar = screen.getByText('Pesquisar');
      fireEvent.click(btnPesquisar);
      
      await waitFor(() => {
        expect(screen.getByText('admin')).toBeInTheDocument();
        expect(screen.getByText('João da Silva')).toBeInTheDocument();
      });
    });

    it('deve renderizar cabeçalhos da tabela', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        expect(screen.getByText('Login')).toBeInTheDocument();
        expect(screen.getByText('Nome')).toBeInTheDocument();
        expect(screen.getByText('E-mail')).toBeInTheDocument();
        expect(screen.getByText('Telefone')).toBeInTheDocument();
      });
    });
  });

  describe('Estado Inicial', () => {
    it('deve iniciar com botão Alterar desabilitado', () => {
      render(<UserRegistry />);
      const btnAlterar = screen.getByText('Alterar');
      expect(btnAlterar).toBeDisabled();
    });

    it('deve iniciar com botão Excluir desabilitado', () => {
      render(<UserRegistry />);
      const btnExcluir = screen.getByText('Excluir');
      expect(btnExcluir).toBeDisabled();
    });

    it('deve iniciar com botão Pesquisar habilitado', () => {
      render(<UserRegistry />);
      const btnPesquisar = screen.getByText('Pesquisar');
      expect(btnPesquisar).not.toBeDisabled();
    });

    it('deve iniciar com botão Salvar habilitado', () => {
      render(<UserRegistry />);
      const btnSalvar = screen.getByText('Salvar');
      expect(btnSalvar).not.toBeDisabled();
    });

    it('deve iniciar com campo Login habilitado', () => {
      render(<UserRegistry />);
      const inputs = screen.getAllByRole('textbox');
      // Primeiro input é o Login
      expect(inputs[0]).not.toBeDisabled();
    });
  });

  describe('Pesquisa de Usuários', () => {
    it('deve chamar onLoadUsers ao clicar em Pesquisar', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        expect(mockOnLoad).toHaveBeenCalled();
      });
    });

    it('deve exibir loading durante carregamento', async () => {
      const mockOnLoad = vi.fn(() => new Promise(resolve => setTimeout(() => resolve(mockLoadResponse), 100)));
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      expect(screen.getByText('Carregando...')).toBeInTheDocument();
    });

    it('deve habilitar botões Alterar e Excluir após pesquisa com resultados', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).not.toBeDisabled();
        expect(screen.getByText('Excluir')).not.toBeDisabled();
      });
    });

    it('deve exibir mensagem de erro em caso de falha', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue({
        sucesso: false,
        mensagem: 'Erro ao carregar usuários',
        usuarios: [],
        total: 0
      });
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        expect(screen.getByText('Erro ao carregar usuários')).toBeInTheDocument();
      });
    });
  });

  describe('Seleção de Usuários', () => {
    it('deve permitir selecionar um usuário', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        expect(checkboxes[0]).toBeChecked();
      });
    });

    it('deve permitir selecionar múltiplos usuários', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(checkboxes[1]);
        expect(checkboxes[0]).toBeChecked();
        expect(checkboxes[1]).toBeChecked();
      });
    });

    it('deve desmarcar usuário ao clicar novamente', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        expect(checkboxes[0]).toBeChecked();
        
        fireEvent.click(checkboxes[0]);
        expect(checkboxes[0]).not.toBeChecked();
      });
    });
  });

  describe('Alteração de Usuário', () => {
    it('deve alertar se nenhum usuário selecionado', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        fireEvent.click(screen.getByText('Alterar'));
        expect(alertSpy).toHaveBeenCalledWith('Selecione pelo menos um item para alteração.');
      });
      
      alertSpy.mockRestore();
    });

    it('deve alertar se mais de um usuário selecionado', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(async () => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(checkboxes[1]);
        
        fireEvent.click(screen.getByText('Alterar'));
        expect(alertSpy).toHaveBeenCalledWith('Marque somente um item para alteração!');
      });
      
      alertSpy.mockRestore();
    });

    it('deve preencher formulário com dados do usuário selecionado', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(screen.getByText('Alterar'));
        
        const inputs = screen.getAllByRole('textbox');
        expect(inputs[0]).toHaveValue('admin');
        expect(inputs[1]).toHaveValue('Administrador do Sistema');
        expect(inputs[2]).toHaveValue('admin@ons.org.br');
        expect(inputs[3]).toHaveValue('(21) 3444-9000');
      });
    });

    it('deve desabilitar campo Login em modo edição', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(screen.getByText('Alterar'));
        
        const inputs = screen.getAllByRole('textbox');
        expect(inputs[0]).toBeDisabled();
      });
    });

    it('deve desabilitar botões Pesquisar, Alterar e Excluir em modo edição', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(screen.getByText('Alterar'));
        
        expect(screen.getByText('Pesquisar')).toBeDisabled();
        expect(screen.getByText('Alterar')).toBeDisabled();
        expect(screen.getByText('Excluir')).toBeDisabled();
      });
    });
  });

  describe('Salvamento de Usuário', () => {
    it('deve alertar se campos obrigatórios não preenchidos', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      render(<UserRegistry />);
      
      fireEvent.click(screen.getByText('Salvar'));
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Não foi possível incluir o usuário! Preencha todos os campos.');
      });
      
      alertSpy.mockRestore();
    });

    it('deve chamar onSaveUser com dados corretos', async () => {
      const mockOnSave = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Usuário incluído!' });
      const mockOnLoad = vi.fn().mockResolvedValue({ sucesso: true, usuarios: [], total: 0 });
      render(<UserRegistry onSaveUser={mockOnSave} onLoadUsers={mockOnLoad} />);
      
      const inputs = screen.getAllByRole('textbox');
      fireEvent.change(inputs[0], { target: { value: 'teste' } });
      fireEvent.change(inputs[1], { target: { value: 'Usuário Teste' } });
      fireEvent.change(inputs[2], { target: { value: 'teste@test.com' } });
      fireEvent.change(inputs[3], { target: { value: '1234-5678' } });
      
      fireEvent.click(screen.getByText('Salvar'));
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          {
            usuar_id: 'teste',
            usuar_nome: 'Usuário Teste',
            usuar_email: 'teste@test.com',
            usuar_telefone: '1234-5678'
          },
          UserFormMode.CREATE
        );
      });
    });

    it('deve exibir mensagem de sucesso após salvar', async () => {
      const mockOnSave = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Usuário incluído com sucesso!' });
      const mockOnLoad = vi.fn().mockResolvedValue({ sucesso: true, usuarios: [], total: 0 });
      render(<UserRegistry onSaveUser={mockOnSave} onLoadUsers={mockOnLoad} />);
      
      const inputs = screen.getAllByRole('textbox');
      fireEvent.change(inputs[0], { target: { value: 'teste' } });
      fireEvent.change(inputs[1], { target: { value: 'Teste' } });
      fireEvent.change(inputs[2], { target: { value: 'teste@test.com' } });
      fireEvent.change(inputs[3], { target: { value: '1234' } });
      
      fireEvent.click(screen.getByText('Salvar'));
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalled();
        expect(mockOnLoad).toHaveBeenCalled();
      });
    });

    it('deve limpar formulário após salvar com sucesso', async () => {
      const mockOnSave = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Sucesso!' });
      const mockOnLoad = vi.fn().mockResolvedValue({ sucesso: true, usuarios: [], total: 0 });
      render(<UserRegistry onSaveUser={mockOnSave} onLoadUsers={mockOnLoad} />);
      
      const inputs = screen.getAllByRole('textbox');
      fireEvent.change(inputs[0], { target: { value: 'teste' } });
      fireEvent.change(inputs[1], { target: { value: 'Teste' } });
      fireEvent.change(inputs[2], { target: { value: 'teste@test.com' } });
      fireEvent.change(inputs[3], { target: { value: '1234' } });
      
      fireEvent.click(screen.getByText('Salvar'));
      
      await waitFor(() => {
        expect(inputs[0]).toHaveValue('');
        expect(inputs[1]).toHaveValue('');
        expect(inputs[2]).toHaveValue('');
        expect(inputs[3]).toHaveValue('');
      });
    });
  });

  describe('Exclusão de Usuários', () => {
    it('deve alertar se nenhum usuário selecionado', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        fireEvent.click(screen.getByText('Excluir'));
        expect(alertSpy).toHaveBeenCalledWith('Selecione pelo menos um item para exclusão.');
      });
      
      alertSpy.mockRestore();
    });

    it('deve solicitar confirmação antes de excluir', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(screen.getByText('Excluir'));
        
        expect(confirmSpy).toHaveBeenCalled();
      });
      
      confirmSpy.mockRestore();
    });

    it('deve chamar onDeleteUsers com IDs corretos', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
      const mockOnDelete = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Excluído!' });
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} onDeleteUsers={mockOnDelete} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(async () => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(checkboxes[1]);
        
        fireEvent.click(screen.getByText('Excluir'));
        
        await waitFor(() => {
          expect(mockOnDelete).toHaveBeenCalledWith(['admin', 'jsilva']);
        });
      });
      
      confirmSpy.mockRestore();
    });
  });

  describe('Cancelamento', () => {
    it('deve limpar formulário ao clicar em Cancelar', () => {
      render(<UserRegistry />);
      
      const inputs = screen.getAllByRole('textbox');
      fireEvent.change(inputs[0], { target: { value: 'teste' } });
      fireEvent.change(inputs[1], { target: { value: 'Teste' } });
      
      fireEvent.click(screen.getByText('Cancelar'));
      
      expect(inputs[0]).toHaveValue('');
      expect(inputs[1]).toHaveValue('');
    });

    it('deve reabilitar campo Login ao cancelar', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        fireEvent.click(screen.getByText('Alterar'));
        
        const inputs = screen.getAllByRole('textbox');
        expect(inputs[0]).toBeDisabled();
        
        fireEvent.click(screen.getByText('Cancelar'));
        expect(inputs[0]).not.toBeDisabled();
      });
    });

    it('deve desselecionar todos os usuários ao cancelar', async () => {
      const mockOnLoad = vi.fn().mockResolvedValue(mockLoadResponse);
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        fireEvent.click(checkboxes[0]);
        expect(checkboxes[0]).toBeChecked();
        
        fireEvent.click(screen.getByText('Cancelar'));
        expect(checkboxes[0]).not.toBeChecked();
      });
    });
  });

  describe('Paginação', () => {
    it('deve exibir paginação quando há mais de 4 usuários', async () => {
      const manyUsers = Array.from({ length: 10 }, (_, i) => ({
        usuar_id: `user${i}`,
        usuar_nome: `Usuário ${i}`,
        usuar_email: `user${i}@test.com`,
        usuar_telefone: `12345${i}`
      }));
      
      const mockOnLoad = vi.fn().mockResolvedValue({
        sucesso: true,
        usuarios: manyUsers.slice(0, 4),
        total: 10
      });
      
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        expect(screen.getByText(/Página 1 de 3/)).toBeInTheDocument();
      });
    });

    it('deve navegar para próxima página', async () => {
      const manyUsers = Array.from({ length: 10 }, (_, i) => ({
        usuar_id: `user${i}`,
        usuar_nome: `Usuário ${i}`,
        usuar_email: `user${i}@test.com`,
        usuar_telefone: `12345${i}`
      }));
      
      const mockOnLoad = vi.fn()
        .mockResolvedValueOnce({
          sucesso: true,
          usuarios: manyUsers.slice(0, 4),
          total: 10
        })
        .mockResolvedValueOnce({
          sucesso: true,
          usuarios: manyUsers.slice(4, 8),
          total: 10
        });
      
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        const nextBtn = screen.getByText(/Próxima>/);
        fireEvent.click(nextBtn);
      });
      
      await waitFor(() => {
        expect(screen.getByText(/Página 2 de 3/)).toBeInTheDocument();
      });
    });

    it('deve navegar para página anterior', async () => {
      const manyUsers = Array.from({ length: 10 }, (_, i) => ({
        usuar_id: `user${i}`,
        usuar_nome: `Usuário ${i}`,
        usuar_email: `user${i}@test.com`,
        usuar_telefone: `12345${i}`
      }));
      
      const mockOnLoad = vi.fn()
        .mockResolvedValueOnce({
          sucesso: true,
          usuarios: manyUsers.slice(0, 4),
          total: 10
        })
        .mockResolvedValueOnce({
          sucesso: true,
          usuarios: manyUsers.slice(4, 8),
          total: 10
        })
        .mockResolvedValueOnce({
          sucesso: true,
          usuarios: manyUsers.slice(0, 4),
          total: 10
        });
      
      render(<UserRegistry onLoadUsers={mockOnLoad} />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(async () => {
        fireEvent.click(screen.getByText(/Próxima>/));
        
        await waitFor(() => {
          fireEvent.click(screen.getByText(/<Anterior/));
        });
        
        await waitFor(() => {
          expect(screen.getByText(/Página 1 de 3/)).toBeInTheDocument();
        });
      });
    });
  });

  describe('Mock Data', () => {
    it('deve usar mock data quando onLoadUsers não fornecido', async () => {
      render(<UserRegistry />);
      
      fireEvent.click(screen.getByText('Pesquisar'));
      
      await waitFor(() => {
        // Verifica se algum dado mock aparece
        const table = screen.queryByRole('table');
        expect(table).toBeInTheDocument();
      });
    });
  });
});

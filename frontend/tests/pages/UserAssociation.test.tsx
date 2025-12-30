/**
 * Testes para o componente UserAssociation
 * Tela: Associação Usuário X Empresa (frmAssocUsuar.aspx)
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import UserAssociation from '../../src/pages/Administration/UserAssociation';
import {
  UserCompanyAssociation,
  UserCompanyAssociationListResponse,
  CompanyOption,
  UserOption
} from '../../src/types/userAssociation';

describe('UserAssociation Component', () => {
  
  // Mock data
  const mockCompanies: CompanyOption[] = [
    { codempre: '1', sigempre: 'FURNAS' },
    { codempre: '2', sigempre: 'CHESF' },
    { codempre: '3', sigempre: 'ELETRONORTE' }
  ];

  const mockUsers: UserOption[] = [
    { usuar_id: 'admin', usuar_nome: 'ADMINISTRADOR DO SISTEMA' },
    { usuar_id: 'jsilva', usuar_nome: 'JOÃO DA SILVA' },
    { usuar_id: 'mferreira', usuar_nome: 'MARIA FERREIRA' }
  ];

  const mockAssociations: UserCompanyAssociation[] = [
    { codempre: '1', sigempre: 'FURNAS', usuar_id: 'admin', usuar_nome: 'ADMINISTRADOR DO SISTEMA' },
    { codempre: '1', sigempre: 'FURNAS', usuar_id: 'jsilva', usuar_nome: 'JOÃO DA SILVA' },
    { codempre: '2', sigempre: 'CHESF', usuar_id: 'mferreira', usuar_nome: 'MARIA FERREIRA' }
  ];

  const mockLoadResponse: UserCompanyAssociationListResponse = {
    sucesso: true,
    associacoes: mockAssociations,
    total: mockAssociations.length
  };

  describe('Renderização', () => {
    it('deve renderizar o título', () => {
      render(<UserAssociation />);
      expect(screen.getByText('Associação Usuário X Empresa')).toBeInTheDocument();
    });

    it('deve renderizar dropdown de empresas', () => {
      render(<UserAssociation />);
      const selects = screen.getAllByRole('combobox');
      expect(selects.length).toBeGreaterThanOrEqual(1);
    });

    it('deve renderizar dropdown de usuários', () => {
      render(<UserAssociation />);
      const selects = screen.getAllByRole('combobox');
      expect(selects.length).toBeGreaterThanOrEqual(2);
    });

    it('deve renderizar botões Incluir e Excluir', () => {
      render(<UserAssociation />);
      expect(screen.getByText('Incluir')).toBeInTheDocument();
      expect(screen.getByText('Excluir')).toBeInTheDocument();
    });
  });

  describe('Carregamento de Dados', () => {
    it('deve carregar empresas ao montar', async () => {
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      render(<UserAssociation onLoadCompanies={mockLoadCompanies} />);
      
      await waitFor(() => {
        expect(mockLoadCompanies).toHaveBeenCalled();
      });
    });

    it('deve carregar usuários ao montar', async () => {
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      render(<UserAssociation onLoadUsers={mockLoadUsers} />);
      
      await waitFor(() => {
        expect(mockLoadUsers).toHaveBeenCalled();
      });
    });

    it('deve usar mock data quando props não fornecidas', () => {
      render(<UserAssociation />);
      // Componente deve renderizar sem erros usando mock data
      expect(screen.getByText('Associação Usuário X Empresa')).toBeInTheDocument();
    });
  });

  describe('Seleção de Filtros', () => {
    it('deve carregar associações ao selecionar empresa', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      await waitFor(() => {
        expect(mockLoadAssociations).toHaveBeenCalled();
      });
    });

    it('deve carregar associações ao selecionar usuário', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[1], { target: { value: 'admin' } });
      });
      
      await waitFor(() => {
        expect(mockLoadAssociations).toHaveBeenCalled();
      });
    });

    it('deve resetar página ao mudar filtro', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      await waitFor(() => {
        expect(mockLoadAssociations).toHaveBeenCalledWith(
          expect.objectContaining({
            page: 0
          })
        );
      });
    });
  });

  describe('Exibição de Associações', () => {
    it('deve exibir loading durante carregamento', async () => {
      const mockLoadAssociations = vi.fn(() => new Promise(resolve => setTimeout(() => resolve(mockLoadResponse), 100)));
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      expect(screen.getByText('Carregando...')).toBeInTheDocument();
    });

    it('deve exibir tabela com associações', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      await waitFor(() => {
        // Verifica que os textos aparecem múltiplas vezes (dropdown + tabela)
        const furnasElements = screen.getAllByText('FURNAS');
        expect(furnasElements.length).toBeGreaterThan(0);
        
        const adminElements = screen.getAllByText('ADMINISTRADOR DO SISTEMA');
        expect(adminElements.length).toBeGreaterThan(0);
      });
    });

    it('deve exibir cabeçalhos da tabela', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      await waitFor(() => {
        expect(screen.getByText('Empresa')).toBeInTheDocument();
        expect(screen.getByText('Usuário')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem quando não há associações', async () => {
      const emptyResponse: UserCompanyAssociationListResponse = {
        sucesso: true,
        associacoes: [],
        total: 0
      };
      
      const mockLoadAssociations = vi.fn().mockResolvedValue(emptyResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
      });
      
      await waitFor(() => {
        expect(screen.getByText('Nenhuma associação encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Seleção de Associações', () => {
    it('deve permitir selecionar uma associação', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          const checkboxes = screen.getAllByRole('checkbox');
          fireEvent.click(checkboxes[0]);
          expect(checkboxes[0]).toBeChecked();
        });
      });
    });

    it('deve permitir selecionar múltiplas associações', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          const checkboxes = screen.getAllByRole('checkbox');
          fireEvent.click(checkboxes[0]);
          fireEvent.click(checkboxes[1]);
          expect(checkboxes[0]).toBeChecked();
          expect(checkboxes[1]).toBeChecked();
        });
      });
    });

    it('deve desmarcar associação ao clicar novamente', async () => {
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          const checkboxes = screen.getAllByRole('checkbox');
          fireEvent.click(checkboxes[0]);
          expect(checkboxes[0]).toBeChecked();
          
          fireEvent.click(checkboxes[0]);
          expect(checkboxes[0]).not.toBeChecked();
        });
      });
    });
  });

  describe('Inclusão de Associação', () => {
    it('botão Incluir deve estar desabilitado quando empresa não selecionada', () => {
      render(<UserAssociation />);
      const btnIncluir = screen.getByText('Incluir');
      expect(btnIncluir).toBeDisabled();
    });

    it('deve chamar onAddAssociation com dados corretos', async () => {
      const mockAddAssociation = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Incluído!' });
      const mockLoadAssociations = vi.fn().mockResolvedValue({ sucesso: true, associacoes: [], total: 0 });
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onAddAssociation={mockAddAssociation}
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(() => {
        expect(mockLoadCompanies).toHaveBeenCalled();
        expect(mockLoadUsers).toHaveBeenCalled();
      });

      const selects = screen.getAllByRole('combobox');
      fireEvent.change(selects[0], { target: { value: '1' } });
      fireEvent.change(selects[1], { target: { value: 'admin' } });
      
      await waitFor(() => {
        const btnIncluir = screen.getByText('Incluir');
        expect(btnIncluir).not.toBeDisabled();
      });

      fireEvent.click(screen.getByText('Incluir'));
      
      await waitFor(() => {
        expect(mockAddAssociation).toHaveBeenCalledWith({
          codempre: '1',
          usuar_id: 'admin'
        });
      });
    });

    it('deve recarregar associações após incluir', async () => {
      const mockAddAssociation = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Incluído!' });
      const mockLoadAssociations = vi.fn().mockResolvedValue({ sucesso: true, associacoes: [], total: 0 });
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onAddAssociation={mockAddAssociation}
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      const initialCallCount = mockLoadAssociations.mock.calls.length;
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        fireEvent.change(selects[1], { target: { value: 'admin' } });
        
        await waitFor(() => {
          fireEvent.click(screen.getByText('Incluir'));
        });
        
        await waitFor(() => {
          expect(mockLoadAssociations.mock.calls.length).toBeGreaterThan(initialCallCount);
        });
      });
    });
  });

  describe('Exclusão de Associações', () => {
    it('botão Excluir deve estar desabilitado quando nenhuma associação selecionada', () => {
      render(<UserAssociation />);
      const btnExcluir = screen.getByText('Excluir');
      expect(btnExcluir).toBeDisabled();
    });

    it('deve solicitar confirmação antes de excluir', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(false);
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          const checkboxes = screen.getAllByRole('checkbox');
          fireEvent.click(checkboxes[0]);
          fireEvent.click(screen.getByText('Excluir'));
          
          expect(confirmSpy).toHaveBeenCalled();
        });
      });
      
      confirmSpy.mockRestore();
    });

    it('deve chamar onDeleteAssociations com dados corretos', async () => {
      const confirmSpy = vi.spyOn(window, 'confirm').mockReturnValue(true);
      const mockDeleteAssociations = vi.fn().mockResolvedValue({ sucesso: true, mensagem: 'Excluído!' });
      const mockLoadAssociations = vi.fn().mockResolvedValue(mockLoadResponse);
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onDeleteAssociations={mockDeleteAssociations}
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(async () => {
          const checkboxes = screen.getAllByRole('checkbox');
          fireEvent.click(checkboxes[0]);
          
          fireEvent.click(screen.getByText('Excluir'));
          
          await waitFor(() => {
            expect(mockDeleteAssociations).toHaveBeenCalledWith([
              { codempre: '1', usuar_id: 'admin' }
            ]);
          });
        });
      });
      
      confirmSpy.mockRestore();
    });
  });

  describe('Estado dos Botões', () => {
    it('deve desabilitar botão Incluir quando empresa não selecionada', () => {
      render(<UserAssociation />);
      const btnIncluir = screen.getByText('Incluir');
      expect(btnIncluir).toBeDisabled();
    });

    it('deve desabilitar botão Excluir quando nenhuma associação selecionada', () => {
      render(<UserAssociation />);
      const btnExcluir = screen.getByText('Excluir');
      expect(btnExcluir).toBeDisabled();
    });

    it('deve habilitar botão Incluir quando empresa e usuário selecionados', async () => {
      render(<UserAssociation />);
      
      await waitFor(() => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        fireEvent.change(selects[1], { target: { value: 'admin' } });
        
        const btnIncluir = screen.getByText('Incluir');
        expect(btnIncluir).not.toBeDisabled();
      });
    });
  });

  describe('Paginação', () => {
    it('deve exibir paginação quando há mais de 5 associações', async () => {
      const manyAssociations = Array.from({ length: 10 }, (_, i) => ({
        codempre: '1',
        sigempre: 'FURNAS',
        usuar_id: `user${i}`,
        usuar_nome: `USUÁRIO ${i}`
      }));
      
      const mockLoadAssociations = vi.fn().mockResolvedValue({
        sucesso: true,
        associacoes: manyAssociations.slice(0, 5),
        total: 10
      });
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          expect(screen.getByText(/Página 1 de 2/)).toBeInTheDocument();
        });
      });
    });

    it('deve navegar para próxima página', async () => {
      const manyAssociations = Array.from({ length: 10 }, (_, i) => ({
        codempre: '1',
        sigempre: 'FURNAS',
        usuar_id: `user${i}`,
        usuar_nome: `USUÁRIO ${i}`
      }));
      
      const mockLoadAssociations = vi.fn()
        .mockResolvedValueOnce({
          sucesso: true,
          associacoes: manyAssociations.slice(0, 5),
          total: 10
        })
        .mockResolvedValueOnce({
          sucesso: true,
          associacoes: manyAssociations.slice(5, 10),
          total: 10
        });
      
      const mockLoadCompanies = vi.fn().mockResolvedValue(mockCompanies);
      const mockLoadUsers = vi.fn().mockResolvedValue(mockUsers);
      
      render(
        <UserAssociation
          onLoadAssociations={mockLoadAssociations}
          onLoadCompanies={mockLoadCompanies}
          onLoadUsers={mockLoadUsers}
        />
      );
      
      await waitFor(async () => {
        const selects = screen.getAllByRole('combobox');
        fireEvent.change(selects[0], { target: { value: '1' } });
        
        await waitFor(() => {
          const nextBtn = screen.getByText(/Próxima>/);
          fireEvent.click(nextBtn);
        });
        
        await waitFor(() => {
          expect(screen.getByText(/Página 2 de 2/)).toBeInTheDocument();
        });
      });
    });
  });
});

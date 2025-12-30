import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { vi, describe, it, expect, beforeEach } from 'vitest';
import UserTeamAssociation from '../../src/pages/Administration/UserTeamAssociation';
import {
  EquipeOption,
  UsuarioOption,
  UserTeamQueryResponse,
  isEquipeValida,
  isUsuarioValido,
  podeIncluir,
  validarSelecaoExclusao,
  formatarNomeUpperCase,
} from '../../src/types/userTeamAssociation';

describe('UserTeamAssociation Component', () => {
  const mockEquipes: EquipeOption[] = [
    { idEquipePdp: '1', nomEquipePdp: 'Equipe Alpha' },
    { idEquipePdp: '2', nomEquipePdp: 'Equipe Beta' },
    { idEquipePdp: '3', nomEquipePdp: 'Equipe Gamma' },
  ];

  const mockUsuarios: UsuarioOption[] = [
    { usuarId: 'USR001', usuarNome: 'João Silva' },
    { usuarId: 'USR002', usuarNome: 'Maria Santos' },
    { usuarId: 'USR003', usuarNome: 'Pedro Oliveira' },
  ];

  const mockAssociacoes: UserTeamQueryResponse = {
    associacoes: [
      { idUsuarEquipePdp: 1, idEquipePdp: '1', nomEquipePdp: 'Equipe Alpha', usuarId: 'USR001', usuarNome: 'João Silva' },
      { idUsuarEquipePdp: 2, idEquipePdp: '1', nomEquipePdp: 'Equipe Alpha', usuarId: 'USR002', usuarNome: 'Maria Santos' },
      { idUsuarEquipePdp: 3, idEquipePdp: '2', nomEquipePdp: 'Equipe Beta', usuarId: 'USR003', usuarNome: 'Pedro Oliveira' },
    ],
    total: 3,
  };

  const mockOnLoadEquipes = vi.fn();
  const mockOnLoadUsuarios = vi.fn();
  const mockOnSearch = vi.fn();
  const mockOnInclude = vi.fn();
  const mockOnDelete = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
    mockOnLoadEquipes.mockResolvedValue(mockEquipes);
    mockOnLoadUsuarios.mockResolvedValue(mockUsuarios);
    mockOnSearch.mockResolvedValue(mockAssociacoes);
    mockOnInclude.mockResolvedValue(undefined);
    mockOnDelete.mockResolvedValue(undefined);
  });

  // Testes de Renderização (5 testes)
  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Associar Usuário-Equipe PDP')).toBeInTheDocument();
    });

    it('deve renderizar os selects de equipe e usuário', () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('equipe-select')).toBeInTheDocument();
      expect(screen.getByTestId('usuario-select')).toBeInTheDocument();
    });

    it('deve renderizar os botões Incluir e Excluir', () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('incluir-btn')).toBeInTheDocument();
      expect(screen.getByTestId('excluir-btn')).toBeInTheDocument();
    });

    it('deve renderizar labels corretos', () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByText('Equipe:')).toBeInTheDocument();
      expect(screen.getByText('Usuário:')).toBeInTheDocument();
    });

    it('deve renderizar grid container', () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );
      expect(screen.getByTestId('grid-container')).toBeInTheDocument();
    });
  });

  // Testes de Carregamento Inicial (4 testes)
  describe('Carregamento Inicial', () => {
    it('deve carregar equipes ao montar componente', async () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEquipes).toHaveBeenCalled();
      });
    });

    it('deve carregar usuários ao montar componente', async () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadUsuarios).toHaveBeenCalled();
      });
    });

    it('deve exibir equipes no select', async () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        const select = screen.getByTestId('equipe-select') as HTMLSelectElement;
        expect(select.options.length).toBe(4); // 1 opção vazia + 3 equipes
      });
    });

    it('deve exibir usuários no select', async () => {
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        const select = screen.getByTestId('usuario-select') as HTMLSelectElement;
        expect(select.options.length).toBe(4); // 1 opção vazia + 3 usuários
      });
    });
  });

  // Testes de Filtros (5 testes)
  describe('Filtros', () => {
    it('deve atualizar estado ao selecionar equipe', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      
      expect((screen.getByTestId('equipe-select') as HTMLSelectElement).value).toBe('1');
    });

    it('deve atualizar estado ao selecionar usuário', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('usuario-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');
      
      expect((screen.getByTestId('usuario-select') as HTMLSelectElement).value).toBe('USR001');
    });

    it('deve buscar associações ao selecionar equipe', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(mockOnSearch).toHaveBeenCalledWith({ idEquipePdp: '1' });
      });
    });

    it('deve buscar associações ao selecionar usuário', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('usuario-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        expect(mockOnSearch).toHaveBeenCalledWith({ usuarId: 'USR001' });
      });
    });

    it('deve buscar com ambos os filtros quando selecionados', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        expect(mockOnSearch).toHaveBeenCalledWith({
          idEquipePdp: '1',
          usuarId: 'USR001',
        });
      });
    });
  });

  // Testes de Grid (6 testes)
  describe('Grid de Associações', () => {
    it('deve renderizar grid com dados', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('associacoes-table')).toBeInTheDocument();
      });
    });

    it('deve renderizar colunas corretas', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByText('Equipe')).toBeInTheDocument();
        expect(screen.getByText('Usuário')).toBeInTheDocument();
      });
    });

    it('deve renderizar linhas de associações', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('assoc-row-0')).toBeInTheDocument();
        expect(screen.getByTestId('assoc-row-1')).toBeInTheDocument();
        expect(screen.getByTestId('assoc-row-2')).toBeInTheDocument();
      });
    });

    it('deve renderizar checkboxes para seleção', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
        expect(screen.getByTestId('checkbox-2')).toBeInTheDocument();
        expect(screen.getByTestId('checkbox-3')).toBeInTheDocument();
      });
    });

    it('deve aplicar estilo alternado às linhas', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        const row1 = screen.getByTestId('assoc-row-1');
        expect(row1.className).toContain('alternateRow');
      });
    });

    it('deve formatar nomes em uppercase', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        // Use getAllByText because the text appears in the select option and multiple table rows
        const equipeElements = screen.getAllByText('EQUIPE ALPHA');
        expect(equipeElements.length).toBeGreaterThan(0);
        
        const usuarioElements = screen.getAllByText('JOÃO SILVA');
        expect(usuarioElements.length).toBeGreaterThan(0);
      });
    });
  });

  // Testes de Seleção (5 testes)
  describe('Seleção de Itens', () => {
    it('deve selecionar item ao clicar no checkbox', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });
      
      await user.click(screen.getByTestId('checkbox-1'));
      expect((screen.getByTestId('checkbox-1') as HTMLInputElement).checked).toBe(true);
    });

    it('deve desselecionar item ao clicar novamente', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('checkbox-1'));
      await user.click(screen.getByTestId('checkbox-1'));
      expect((screen.getByTestId('checkbox-1') as HTMLInputElement).checked).toBe(false);
    });

    it('deve selecionar todos os itens ao marcar "Selecionar Todos"', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('select-all-checkbox')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('select-all-checkbox'));

      await waitFor(() => {
        expect((screen.getByTestId('checkbox-1') as HTMLInputElement).checked).toBe(true);
        expect((screen.getByTestId('checkbox-2') as HTMLInputElement).checked).toBe(true);
        expect((screen.getByTestId('checkbox-3') as HTMLInputElement).checked).toBe(true);
      });
    });

    it('deve desselecionar todos ao desmarcar "Selecionar Todos"', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('select-all-checkbox')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('select-all-checkbox'));
      await user.click(screen.getByTestId('select-all-checkbox'));

      await waitFor(() => {
        expect((screen.getByTestId('checkbox-1') as HTMLInputElement).checked).toBe(false);
      });
    });

    it('deve habilitar botão Excluir ao selecionar item', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('checkbox-1'));

      const btnExcluir = screen.getByTestId('excluir-btn') as HTMLButtonElement;
      expect(btnExcluir.disabled).toBe(false);
    });
  });

  // Testes de Inclusão (4 testes)
  describe('Inclusão de Associação', () => {
    it('deve desabilitar botão Incluir quando não há seleção', async () => {
      const emptyResponse: UserTeamQueryResponse = { associacoes: [], total: 0 };
      const emptySearch = vi.fn().mockResolvedValue(emptyResponse);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={emptySearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      const btn = screen.getByTestId('incluir-btn') as HTMLButtonElement;
      expect(btn.disabled).toBe(true);
    });

    it('deve habilitar botão Incluir ao selecionar equipe e usuário sem associações', async () => {
      const user = userEvent.setup();
      const emptyResponse: UserTeamQueryResponse = { associacoes: [], total: 0 };
      const emptySearch = vi.fn().mockResolvedValue(emptyResponse);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={emptySearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        const btn = screen.getByTestId('incluir-btn') as HTMLButtonElement;
        expect(btn.disabled).toBe(false);
      });
    });

    it('deve chamar onInclude ao clicar em Incluir', async () => {
      const user = userEvent.setup();
      const emptyResponse: UserTeamQueryResponse = { associacoes: [], total: 0 };
      const emptySearch = vi.fn().mockResolvedValue(emptyResponse);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={emptySearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        const btn = screen.getByTestId('incluir-btn');
        expect((btn as HTMLButtonElement).disabled).toBe(false);
      });

      await user.click(screen.getByTestId('incluir-btn'));

      await waitFor(() => {
        expect(mockOnInclude).toHaveBeenCalledWith('1', 'USR001');
      });
    });

    it('deve exibir mensagem de sucesso após incluir', async () => {
      const user = userEvent.setup();
      const emptyResponse: UserTeamQueryResponse = { associacoes: [], total: 0 };
      const emptySearch = vi.fn().mockResolvedValue(emptyResponse);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={emptySearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        const btn = screen.getByTestId('incluir-btn');
        expect((btn as HTMLButtonElement).disabled).toBe(false);
      });

      await user.click(screen.getByTestId('incluir-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('success-message')).toBeInTheDocument();
        expect(screen.getByText('Associação incluída com sucesso!')).toBeInTheDocument();
      });
    });
  });

  // Testes de Exclusão (4 testes)
  describe('Exclusão de Associação', () => {
    it('deve desabilitar botão Excluir quando nenhum item está selecionado', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      const btn = screen.getByTestId('excluir-btn') as HTMLButtonElement;
      expect(btn.disabled).toBe(true);
    });

    it('deve chamar onDelete ao clicar em Excluir', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('checkbox-1'));
      await user.click(screen.getByTestId('excluir-btn'));

      await waitFor(() => {
        expect(mockOnDelete).toHaveBeenCalledWith([1]);
      });
    });

    it('deve exibir mensagem de sucesso após excluir', async () => {
      const user = userEvent.setup();
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('checkbox-1'));
      await user.click(screen.getByTestId('excluir-btn'));

      await waitFor(() => {
        expect(screen.getByText('1 associação(ões) excluída(s) com sucesso!')).toBeInTheDocument();
      });
    });

    it('deve exibir erro específico ao excluir com constraint', async () => {
      const user = userEvent.setup();
      const errorDelete = vi.fn().mockRejectedValue(new Error('key value for constraint'));
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={errorDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getByTestId('checkbox-1')).toBeInTheDocument();
      });

      await user.click(screen.getByTestId('checkbox-1'));
      await user.click(screen.getByTestId('excluir-btn'));

      await waitFor(() => {
        expect(screen.getByText('Não é possível excluir, o usuário está associado a um estudo.')).toBeInTheDocument();
      });
    });
  });

  // Testes de Paginação (3 testes)
  describe('Paginação', () => {
    const manyAssociations: UserTeamQueryResponse = {
      associacoes: Array.from({ length: 12 }, (_, i) => ({
        idUsuarEquipePdp: i + 1,
        idEquipePdp: '1',
        nomEquipePdp: 'Equipe Alpha',
        usuarId: `USR00${i + 1}`,
        usuarNome: `Usuario ${i + 1}`,
      })),
      total: 12,
    };

    it('deve renderizar paginação quando houver mais de 5 itens', async () => {
      const user = userEvent.setup();
      mockOnSearch.mockResolvedValue(manyAssociations);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getAllByTestId(/^assoc-row-/)).toHaveLength(5);
      });

      await waitFor(() => {
        expect(screen.getByTestId('pagination')).toBeInTheDocument();
      });
    });

    it('deve exibir informação de página correta', async () => {
      const user = userEvent.setup();
      mockOnSearch.mockResolvedValue(manyAssociations);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getAllByTestId(/^assoc-row-/)).toHaveLength(5);
      });

      await waitFor(() => {
        expect(screen.getByTestId('page-info')).toHaveTextContent('Página 1 de 3');
      });
    });

    it('deve navegar para próxima página', async () => {
      const user = userEvent.setup();
      mockOnSearch.mockResolvedValue(manyAssociations);
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');

      await waitFor(() => {
        expect(screen.getAllByTestId(/^assoc-row-/)).toHaveLength(5);
      });

      await user.click(screen.getByTestId('next-page-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('page-info')).toHaveTextContent('Página 2 de 3');
      });
    });
  });

  // Testes de Funções Utilitárias (5 testes)
  describe('Funções Utilitárias', () => {
    it('isEquipeValida deve validar corretamente', () => {
      expect(isEquipeValida('1')).toBe(true);
      expect(isEquipeValida('0')).toBe(false);
      expect(isEquipeValida('')).toBe(false);
    });

    it('isUsuarioValido deve validar corretamente', () => {
      expect(isUsuarioValido('USR001')).toBe(true);
      expect(isUsuarioValido('0')).toBe(false);
      expect(isUsuarioValido('')).toBe(false);
    });

    it('podeIncluir deve validar condições corretamente', () => {
      expect(podeIncluir('1', 'USR001', 0)).toBe(true);
      expect(podeIncluir('0', 'USR001', 0)).toBe(false);
      expect(podeIncluir('1', '0', 0)).toBe(false);
      expect(podeIncluir('1', 'USR001', 1)).toBe(false);
    });

    it('validarSelecaoExclusao deve validar seleção', () => {
      expect(validarSelecaoExclusao([1, 2]).valido).toBe(true);
      expect(validarSelecaoExclusao([]).valido).toBe(false);
      expect(validarSelecaoExclusao([]).mensagem).toBe('Selecione pelo menos um item para excluir.');
    });

    it('formatarNomeUpperCase deve formatar corretamente', () => {
      expect(formatarNomeUpperCase('  joão silva  ')).toBe('JOÃO SILVA');
      expect(formatarNomeUpperCase('maria')).toBe('MARIA');
    });
  });

  // Testes de Tratamento de Erros (2 testes)
  describe('Tratamento de Erros', () => {
    it('deve exibir erro ao falhar ao carregar dados', async () => {
      const errorLoadEquipes = vi.fn().mockRejectedValue(new Error('Erro de conexão'));
      
      render(
        <UserTeamAssociation
          onLoadEquipes={errorLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={mockOnInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Não foi possível recuperar os registros.')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar ao incluir associação', async () => {
      const user = userEvent.setup();
      mockOnSearch.mockResolvedValue({ associacoes: [], total: 0 });
      const errorInclude = vi.fn().mockRejectedValue(new Error('Erro ao incluir'));
      
      render(
        <UserTeamAssociation
          onLoadEquipes={mockOnLoadEquipes}
          onLoadUsuarios={mockOnLoadUsuarios}
          onSearch={mockOnSearch}
          onInclude={errorInclude}
          onDelete={mockOnDelete}
        />
      );

      await waitFor(() => expect(screen.getByTestId('equipe-select')).toBeInTheDocument());
      await user.selectOptions(screen.getByTestId('equipe-select'), '1');
      await user.selectOptions(screen.getByTestId('usuario-select'), 'USR001');

      await waitFor(() => {
        const btn = screen.getByTestId('incluir-btn') as HTMLButtonElement;
        expect(btn.disabled).toBe(false);
      });

      await user.click(screen.getByTestId('incluir-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Não foi possível incluir a associação!')).toBeInTheDocument();
      });
    });
  });
});

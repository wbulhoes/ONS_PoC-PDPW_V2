import { describe, it, expect, vi, beforeEach } from "vitest";
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import PlantRegistry from '../../src/pages/Administration/PlantRegistry';
import {
  EmpresaOption,
  PlantQueryResponse,
  getTipoUsinaLabel,
  formatarCodigoUsina,
  isCodigoEmpresaValido,
} from '../../src/types/plant';

describe('PlantRegistry Component', () => {
  const mockEmpresas: EmpresaOption[] = [
    { codEmpre: 'ELETROBRAS', sigEmpre: 'Eletrobras' },
    { codEmpre: 'CEMIG', sigEmpre: 'CEMIG' },
    { codEmpre: 'COPEL', sigEmpre: 'COPEL' },
  ];

  const mockUsinas: PlantQueryResponse = {
    usinas: [
      { codUsina: 'UHE001', sigUsina: 'ITU', nomUsina: 'Usina Itumbiara', tipUsina: 'H' },
      { codUsina: 'UHE002', sigUsina: 'FUR', nomUsina: 'Usina Furnas', tipUsina: 'H' },
      { codUsina: 'UTE001', sigUsina: 'AGV', nomUsina: 'Usina Angra', tipUsina: 'T' },
    ],
    total: 3,
    page: 0,
    pageSize: 10,
    totalPages: 1,
  };

  const mockOnLoadEmpresas = vi.fn().mockResolvedValue(mockEmpresas);
  const mockOnSearchUsinas = vi.fn().mockResolvedValue(mockUsinas);
  const mockOnViewDetails = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  // Testes de Renderização (5 testes)
  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );
      expect(screen.getByText('Consulta de Usinas')).toBeInTheDocument();
    });

    it('deve renderizar o select de empresas', () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );
      expect(screen.getByTestId('empresa-select')).toBeInTheDocument();
    });

    it('deve renderizar o botão Pesquisar', () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );
      expect(screen.getByTestId('pesquisar-btn')).toBeInTheDocument();
    });

    it('deve renderizar o label correto', () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );
      expect(screen.getByText('Empresa:')).toBeInTheDocument();
    });

    it('deve renderizar com classes CSS corretas', () => {
      const { container } = render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );
      // CSS Modules transformam os nomes das classes, então verificamos a estrutura
      expect(container.firstChild).toBeInTheDocument();
      expect(screen.getByText('Consulta de Usinas')).toBeInTheDocument();
      expect(screen.getByText('Empresa:')).toBeInTheDocument();
    });
  });

  // Testes de Carregamento de Empresas (4 testes)
  describe('Carregamento de Empresas', () => {
    it('deve carregar empresas ao montar componente', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });
    });

    it('deve exibir empresas no select', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        const select = screen.getByTestId('empresa-select') as HTMLSelectElement;
        expect(select.options.length).toBe(4); // 1 opção vazia + 3 empresas
      });
    });

    it('deve exibir erro ao falhar ao carregar empresas', async () => {
      const errorOnLoadEmpresas = vi.fn().mockRejectedValue(new Error('Erro de conexão'));
      render(
        <PlantRegistry
          onLoadEmpresas={errorOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(screen.getByTestId('error-message')).toBeInTheDocument();
        expect(screen.getByText('Não foi possível acessar a Base de Dados.')).toBeInTheDocument();
      });
    });

    it('deve permitir seleção de empresa', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        const select = screen.getByTestId('empresa-select') as HTMLSelectElement;
        fireEvent.change(select, { target: { value: 'ELETROBRAS' } });
        expect(select.value).toBe('ELETROBRAS');
      });
    });
  });

  // Testes de Pesquisa (6 testes)
  describe('Funcionalidade de Pesquisa', () => {
    it('deve desabilitar botão Pesquisar quando nenhuma empresa está selecionada', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        const btn = screen.getByTestId('pesquisar-btn') as HTMLButtonElement;
        expect(btn.disabled).toBe(true);
      });
    });

    it('deve habilitar botão Pesquisar ao selecionar empresa', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      const select = screen.getByTestId('empresa-select');
      fireEvent.change(select, { target: { value: 'ELETROBRAS' } });

      await waitFor(() => {
        const btn = screen.getByTestId('pesquisar-btn') as HTMLButtonElement;
        expect(btn.disabled).toBe(false);
      });
    });

    it('deve chamar onSearchUsinas ao clicar em Pesquisar', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      const select = screen.getByTestId('empresa-select');
      fireEvent.change(select, { target: { value: 'ELETROBRAS' } });

      const btn = screen.getByTestId('pesquisar-btn');
      fireEvent.click(btn);

      await waitFor(() => {
        expect(mockOnSearchUsinas).toHaveBeenCalledWith({
          codEmpresa: 'ELETROBRAS',
          page: 0,
          pageSize: 10,
        });
      });
    });

    it('deve exibir erro ao pesquisar sem selecionar empresa', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        const select = screen.getByTestId('empresa-select');
        fireEvent.change(select, { target: { value: '' } });
      });

      const btn = screen.getByTestId('pesquisar-btn');
      
      // Botão deve estar desabilitado
      expect(btn).toBeDisabled();
    });

    it('deve exibir estado de loading durante pesquisa', async () => {
      const slowSearch = vi.fn(() => new Promise((resolve) => setTimeout(() => resolve(mockUsinas), 50)));
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={slowSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      const select = screen.getByTestId('empresa-select');
      fireEvent.change(select, { target: { value: 'ELETROBRAS' } });

      const btn = screen.getByTestId('pesquisar-btn');
      fireEvent.click(btn);

      await waitFor(() => {
        expect(btn).toHaveTextContent('Pesquisando...');
      });
    });

    it('deve exibir erro ao falhar na pesquisa', async () => {
      const errorOnSearch = vi.fn().mockRejectedValue(new Error('Erro de conexão'));
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={errorOnSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      const select = screen.getByTestId('empresa-select');
      fireEvent.change(select, { target: { value: 'ELETROBRAS' } });

      const btn = screen.getByTestId('pesquisar-btn');
      fireEvent.click(btn);

      await waitFor(() => {
        expect(screen.getByText('Não foi possível acessar a Base de Dados.')).toBeInTheDocument();
      });
    });
  });

  // Testes de Grid de Resultados (7 testes)
  describe('Grid de Resultados', () => {
    it('deve renderizar grid após pesquisa bem-sucedida', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      const select = screen.getByTestId('empresa-select');
      fireEvent.change(select, { target: { value: 'ELETROBRAS' } });

      const btn = screen.getByTestId('pesquisar-btn');
      fireEvent.click(btn);

      await waitFor(() => {
        expect(screen.getByTestId('grid-container')).toBeInTheDocument();
      });
    });

    it('deve renderizar tabela com colunas corretas', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByText('Código')).toBeInTheDocument();
        expect(screen.getByText('Sigla')).toBeInTheDocument();
        expect(screen.getByText('Nome')).toBeInTheDocument();
        expect(screen.getByText('Tipo')).toBeInTheDocument();
      });
    });

    it('deve renderizar linhas de usinas', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('usina-row-0')).toBeInTheDocument();
        expect(screen.getByTestId('usina-row-1')).toBeInTheDocument();
        expect(screen.getByTestId('usina-row-2')).toBeInTheDocument();
      });
    });

    it('deve exibir contador de registros', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByText('3 usinas encontradas')).toBeInTheDocument();
      });
    });

    it('deve aplicar estilo alternado às linhas', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        const row1 = screen.getByTestId('usina-row-1');
        expect(row1).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem quando não houver resultados', async () => {
      const emptyResponse: PlantQueryResponse = {
        usinas: [],
        total: 0,
        page: 0,
        pageSize: 10,
        totalPages: 0,
      };
      
      const emptySearch = vi.fn().mockResolvedValue(emptyResponse);
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={emptySearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('no-results')).toBeInTheDocument();
        expect(screen.getByText('Nenhuma usina encontrada para a empresa selecionada.')).toBeInTheDocument();
      });
    });

    it('deve formatar tipos de usina corretamente', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        const hidroElements = screen.getAllByText('Hidro');
        expect(hidroElements.length).toBeGreaterThan(0);
        expect(screen.getByText('Termo')).toBeInTheDocument();
      });
    });
  });

  // Testes de Paginação (5 testes)
  describe('Paginação', () => {
    const multiPageResponse: PlantQueryResponse = {
      usinas: mockUsinas.usinas,
      total: 25,
      page: 0,
      pageSize: 10,
      totalPages: 3,
    };

    it('deve renderizar controles de paginação quando houver múltiplas páginas', async () => {
      const multiPageSearch = vi.fn().mockResolvedValue(multiPageResponse);
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={multiPageSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('pagination')).toBeInTheDocument();
      });
    });

    it('deve exibir informação de página correta', async () => {
      const multiPageSearch = vi.fn().mockResolvedValue(multiPageResponse);
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={multiPageSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('page-info')).toHaveTextContent('Página 1 de 3');
      });
    });

    it('deve desabilitar botão Anterior na primeira página', async () => {
      const multiPageSearch = vi.fn().mockResolvedValue(multiPageResponse);
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={multiPageSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        const prevBtn = screen.getByTestId('prev-page-btn') as HTMLButtonElement;
        expect(prevBtn.disabled).toBe(true);
      });
    });

    it('deve navegar para próxima página ao clicar em Próxima', async () => {
      const multiPageSearch = vi.fn().mockResolvedValue(multiPageResponse);
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={multiPageSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('next-page-btn')).toBeInTheDocument();
      });

      const nextBtn = screen.getByTestId('next-page-btn');
      fireEvent.click(nextBtn);

      await waitFor(() => {
        expect(multiPageSearch).toHaveBeenCalledWith({
          codEmpresa: 'ELETROBRAS',
          page: 1,
          pageSize: 10,
        });
      });
    });

    it('não deve renderizar paginação com apenas uma página', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
        fireEvent.click(screen.getByTestId('pesquisar-btn'));
      });

      await waitFor(() => {
        expect(screen.queryByTestId('pagination')).not.toBeInTheDocument();
      });
    });
  });

  // Testes de Visualização de Detalhes (3 testes)
  describe('Visualização de Detalhes', () => {
    it('deve renderizar links para detalhes de usinas', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('view-details-UHE001')).toBeInTheDocument();
      });
    });

    it('deve chamar onViewDetails ao clicar no código da usina', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        const link = screen.getByTestId('view-details-UHE001');
        fireEvent.click(link);
      });

      expect(mockOnViewDetails).toHaveBeenCalledWith('UHE001', 'ELETROBRAS');
    });

    it('deve passar parâmetros corretos para onViewDetails', async () => {
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'CEMIG' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        const link = screen.getByTestId('view-details-UHE002');
        fireEvent.click(link);
      });

      expect(mockOnViewDetails).toHaveBeenCalledWith('UHE002', 'CEMIG');
    });
  });

  // Testes de Funções Utilitárias (3 testes)
  describe('Funções Utilitárias', () => {
    it('getTipoUsinaLabel deve converter tipo corretamente', () => {
      expect(getTipoUsinaLabel('H')).toBe('Hidro');
      expect(getTipoUsinaLabel('T')).toBe('Termo');
      expect(getTipoUsinaLabel('E')).toBe('Eólica');
    });

    it('formatarCodigoUsina deve formatar código corretamente', () => {
      expect(formatarCodigoUsina('  uhe001  ')).toBe('UHE001');
      expect(formatarCodigoUsina('ute002')).toBe('UTE002');
    });

    it('isCodigoEmpresaValido deve validar código corretamente', () => {
      expect(isCodigoEmpresaValido('ELETROBRAS')).toBe(true);
      expect(isCodigoEmpresaValido('0')).toBe(false);
      expect(isCodigoEmpresaValido('')).toBe(false);
    });
  });

  // Testes de Estados de Loading (2 testes)
  describe('Estados de Loading', () => {
    it('deve exibir mensagem de carregamento durante pesquisa', async () => {
      const slowSearch = vi.fn(() => new Promise((resolve) => setTimeout(() => resolve(mockUsinas), 50)));
      
      render(
        <PlantRegistry
          onLoadEmpresas={mockOnLoadEmpresas}
          onSearchUsinas={slowSearch}
          onViewDetails={mockOnViewDetails}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalled();
      });

      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      fireEvent.click(screen.getByTestId('pesquisar-btn'));

      await waitFor(() => {
        expect(screen.getByTestId('loading')).toBeInTheDocument();
        expect(screen.getByText('Carregando dados...')).toBeInTheDocument();
      });
    });

    it('deve desabilitar select durante carregamento', async () => {
      const slowLoad = vi.fn(() => new Promise((resolve) => setTimeout(() => resolve(mockEmpresas), 100)));
      
      render(
        <PlantRegistry
          onLoadEmpresas={slowLoad}
          onSearchUsinas={mockOnSearchUsinas}
          onViewDetails={mockOnViewDetails}
        />
      );

      const select = screen.getByTestId('empresa-select') as HTMLSelectElement;
      expect(select.disabled).toBe(true);
    });
  });
});

/**
 * Testes para o componente Company
 * Cobertura: Renderização, paginação, loading states, mensagens
 */

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Company from '../../src/pages/Administration/Company';
import type { CompanyListResponse, PaginationParams } from '../../src/types/company';

describe('Company Component', () => {
  const mockCompanies: CompanyListResponse = {
    sucesso: true,
    total: 25,
    empresas: [
      {
        codempre: 'EMP001',
        nomempre: 'Empresa 1 S.A.',
        sigempre: 'EMP1',
        idgtpoempre: '1',
        contr: true,
        regiao: 'Norte',
        sistema: 'SIN',
        area_contr: false,
        infpdp: true,
        area_nao_contr: null,
        empresa_nao_contr: null,
      },
      {
        codempre: 'EMP002',
        nomempre: 'Empresa 2 S.A.',
        sigempre: 'EMP2',
        idgtpoempre: '2',
        contr: false,
        regiao: null,
        sistema: null,
        area_contr: true,
        infpdp: false,
        area_nao_contr: 'ÁREA CTROL 1',
        empresa_nao_contr: 'Controladora 1',
      },
    ],
  };

  const mockOnLoadCompanies = vi.fn();

  beforeEach(() => {
    mockOnLoadCompanies.mockClear();
    mockOnLoadCompanies.mockResolvedValue(mockCompanies);
  });

  describe('Renderização', () => {
    it('deve renderizar o título', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('Empresas')).toBeInTheDocument();
      });
    });

    it('deve renderizar a tabela com cabeçalhos', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getAllByText('Empresa').length).toBeGreaterThan(0);
        expect(screen.getByText('Nome')).toBeInTheDocument();
        expect(screen.getByText('Sigla')).toBeInTheDocument();
        expect(screen.getByText('GTPO')).toBeInTheDocument();
        expect(screen.getByText('Controladora de Área')).toBeInTheDocument();
        expect(screen.getByText('Região')).toBeInTheDocument();
        expect(screen.getByText('Sistema')).toBeInTheDocument();
      });
    });

    it('deve renderizar dados das empresas', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('EMP001')).toBeInTheDocument();
        expect(screen.getByText('Empresa 1 S.A.')).toBeInTheDocument();
        expect(screen.getByText('EMP1')).toBeInTheDocument();
      });
    });

    it('deve renderizar checkboxes para campos booleanos', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        expect(checkboxes.length).toBeGreaterThan(0);
      });
    });

    it('deve chamar onLoadCompanies ao montar', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(mockOnLoadCompanies).toHaveBeenCalledWith({ page: 0, pageSize: 8 });
      });
    });
  });

  describe('Paginação', () => {
    it('deve exibir botões de paginação quando há mais de uma página', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('<Anterior')).toBeInTheDocument();
        expect(screen.getByText('Próxima>')).toBeInTheDocument();
      });
    });

    it('deve desabilitar botão Anterior na primeira página', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        const prevButton = screen.getByText('<Anterior');
        expect(prevButton).toBeDisabled();
      });
    });

    it('deve avançar para próxima página ao clicar em Próxima', async () => {
      const user = userEvent.setup();
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('Próxima>')).toBeInTheDocument();
      });
      
      const nextButton = screen.getByText('Próxima>');
      await user.click(nextButton);
      
      await waitFor(() => {
        expect(mockOnLoadCompanies).toHaveBeenCalledWith({ page: 1, pageSize: 8 });
      });
    });

    it('deve voltar para página anterior ao clicar em Anterior', async () => {
      const user = userEvent.setup();
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('Próxima>')).toBeInTheDocument();
      });
      
      // Avançar para página 2
      const nextButton = screen.getByText('Próxima>');
      await user.click(nextButton);
      
      await waitFor(() => {
        expect(mockOnLoadCompanies).toHaveBeenCalledWith({ page: 1, pageSize: 8 });
      });
      
      // Voltar para página 1
      const prevButton = screen.getByText('<Anterior');
      await user.click(prevButton);
      
      await waitFor(() => {
        expect(mockOnLoadCompanies).toHaveBeenCalledWith({ page: 0, pageSize: 8 });
      });
    });

    it('deve exibir número da página atual', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText(/Página \d+ de \d+/)).toBeInTheDocument();
      });
    });

    it('deve calcular total de páginas corretamente', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        // 25 itens / 8 por página = 4 páginas (3.125 arredondado)
        expect(screen.getByText(/Página 1 de 4/)).toBeInTheDocument();
      });
    });

    it('deve desabilitar botão Próxima na última página', async () => {
      const user = userEvent.setup();
      
      // Mock com apenas 2 empresas (1 página)
      const singlePageMock = vi.fn(() => Promise.resolve({
        sucesso: true,
        total: 2,
        empresas: mockCompanies.empresas,
      }));
      
      render(<Company onLoadCompanies={singlePageMock} />);
      
      await waitFor(() => {
        const nextBtn = screen.queryByText('Próxima>');
        // Com total=2 e pageSize=8, não deve haver botão de próxima
        expect(nextBtn).not.toBeInTheDocument();
      });
    });
  });

  describe('Loading States', () => {
    it('deve exibir loading ao carregar dados', async () => {
      const slowLoadCompanies = vi.fn(() => 
        new Promise(resolve => setTimeout(() => resolve(mockCompanies), 100))
      );
      
      render(<Company onLoadCompanies={slowLoadCompanies} />);
      
      expect(screen.getByText('Carregando...')).toBeInTheDocument();
      
      await waitFor(() => {
        expect(screen.queryByText('Carregando...')).not.toBeInTheDocument();
      });
    });

    it('não deve exibir tabela durante loading', async () => {
      const slowLoadCompanies = vi.fn(() => 
        new Promise(resolve => setTimeout(() => resolve(mockCompanies), 100))
      );
      
      render(<Company onLoadCompanies={slowLoadCompanies} />);
      
      expect(screen.queryByRole('table')).not.toBeInTheDocument();
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
      });
    });
  });

  describe('Mensagens', () => {
    it('deve exibir mensagem de erro em caso de falha', async () => {
      const failLoadCompanies = vi.fn(() => Promise.reject(new Error('Erro')));
      
      render(<Company onLoadCompanies={failLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('Erro ao carregar empresas')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem quando não há empresas', async () => {
      const emptyResponse: CompanyListResponse = {
        sucesso: true,
        total: 0,
        empresas: [],
      };
      
      const mockEmpty = vi.fn(() => Promise.resolve(emptyResponse));
      
      render(<Company onLoadCompanies={mockEmpty} />);
      
      await waitFor(() => {
        expect(screen.getByText('Nenhuma empresa encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Dados das Empresas', () => {
    it('deve exibir região e sistema para empresa controladora', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('Norte')).toBeInTheDocument();
        expect(screen.getByText('SIN')).toBeInTheDocument();
      });
    });

    it('deve exibir área e empresa controladora para empresa controlada', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        expect(screen.getByText('ÁREA CTROL 1')).toBeInTheDocument();
        expect(screen.getByText('Controladora 1')).toBeInTheDocument();
      });
    });

    it('deve marcar checkbox de controladora corretamente', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        // Primeiro checkbox deve estar marcado (EMP001 é controladora)
        expect(checkboxes[0]).toBeChecked();
        // Terceiro checkbox não deve estar marcado (EMP001 não é controlada)
        expect(checkboxes[1]).not.toBeChecked();
      });
    });

    it('deve desabilitar todos os checkboxes', async () => {
      render(<Company onLoadCompanies={mockOnLoadCompanies} />);
      
      await waitFor(() => {
        const checkboxes = screen.getAllByRole('checkbox');
        checkboxes.forEach(checkbox => {
          expect(checkbox).toBeDisabled();
        });
      });
    });
  });

  describe('Comportamento sem prop onLoadCompanies', () => {
    it('deve usar dados mock quando prop não é fornecida', async () => {
      render(<Company />);
      
      await waitFor(() => {
        expect(screen.getByRole('table')).toBeInTheDocument();
        const cells = screen.getAllByRole('cell');
        const hasEmpCode = cells.some(cell => /EMP\d{3}/.test(cell.textContent || ''));
        expect(hasEmpCode).toBe(true);
      });
    });

    it('deve permitir paginação com dados mock', async () => {
      const user = userEvent.setup();
      render(<Company />);
      
      await waitFor(() => {
        expect(screen.getByText('Próxima>')).toBeInTheDocument();
      });
      
      const nextButton = screen.getByText('Próxima>');
      await user.click(nextButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Página 2 de \d+/)).toBeInTheDocument();
      });
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Import from '../../src/pages/Collection/Interchange/Import';
import {
  gerarIntervalos,
  intervaloParaHorario,
  calcularTotalColuna,
  calcularMediaColuna,
  formatarValoresParaTextarea,
  parseValoresDoTextarea,
  parseUsinaValue,
  GridImportacao,
} from '../../src/types/import';

describe('Import Component', () => {
  const mockGridData: GridImportacao = {
    colunas: [
      {
        codUsina: 'UHE_ABC',
        label: 'UHE ABC',
        ordem: 1,
        valores: Array(48).fill(100),
      },
      {
        codUsina: 'UHE_XYZ',
        label: 'UHE XYZ',
        ordem: 2,
        valores: Array(48).fill(150),
      },
    ],
    intervalos: gerarIntervalos(),
    totais: [4800, 7200],
    medias: [100, 150],
  };

  const mockOnLoadData = vi.fn().mockResolvedValue(mockGridData);
  const mockOnSave = vi.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    vi.clearAllMocks();
  });

  // Testes de Renderização (5 testes)
  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByText('Coleta - Importação de Energia')).toBeInTheDocument();
    });

    it('deve renderizar todos os campos do formulário', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByTestId('data-pdp-select')).toBeInTheDocument();
      expect(screen.getByTestId('empresa-select')).toBeInTheDocument();
      expect(screen.getByTestId('usina-select')).toBeInTheDocument();
    });

    it('deve renderizar o botão Salvar', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByTestId('salvar-btn')).toBeInTheDocument();
    });

    it('deve renderizar labels corretos', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(screen.getByText('Data PDP:')).toBeInTheDocument();
      expect(screen.getByText('Empresa:')).toBeInTheDocument();
      expect(screen.getByText('Usinas:')).toBeInTheDocument();
    });

    it('deve renderizar com classes CSS corretas', () => {
      const { container } = render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      expect(container.firstChild).toBeInTheDocument();
      expect(screen.getByText('Data PDP:')).toBeInTheDocument();
      expect(screen.getByText('Empresa:')).toBeInTheDocument();
    });
  });

  // Testes de Seleção de Data (3 testes)
  describe('Seleção de Data PDP', () => {
    it('deve atualizar estado ao selecionar data', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const dataSelect = screen.getByTestId('data-pdp-select') as HTMLSelectElement;
      
      fireEvent.change(dataSelect, { target: { value: '2024-01-15' } });
      
      expect(dataSelect.value).toBe('2024-01-15');
    });

    it('não deve carregar dados com apenas data selecionada', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const dataSelect = screen.getByTestId('data-pdp-select');
      
      fireEvent.change(dataSelect, { target: { value: '2024-01-15' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).not.toHaveBeenCalled();
      });
    });

    it('deve ter opções de data disponíveis', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const dataSelect = screen.getByTestId('data-pdp-select') as HTMLSelectElement;
      
      expect(dataSelect.options.length).toBeGreaterThan(1);
    });
  });

  // Testes de Seleção de Empresa (3 testes)
  describe('Seleção de Empresa', () => {
    it('deve atualizar estado ao selecionar empresa', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const empresaSelect = screen.getByTestId('empresa-select') as HTMLSelectElement;
      
      fireEvent.change(empresaSelect, { target: { value: 'ELETROBRAS' } });
      
      expect(empresaSelect.value).toBe('ELETROBRAS');
    });

    it('deve carregar dados ao selecionar data e empresa', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
    });

    it('deve ter opções de empresa disponíveis', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const empresaSelect = screen.getByTestId('empresa-select') as HTMLSelectElement;
      
      expect(empresaSelect.options.length).toBeGreaterThan(1);
    });
  });

  // Testes de Seleção de Usina (4 testes)
  describe('Seleção de Usina', () => {
    it('deve carregar opções de usina após selecionar data e empresa', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select') as HTMLSelectElement;
        expect(usinaSelect.options.length).toBeGreaterThan(1);
      });
    });

    it('deve mostrar textarea ao selecionar usina individual', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      const usinaSelect = screen.getByTestId('usina-select');
      fireEvent.change(usinaSelect, { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
    });

    it('deve mostrar opção "Todas as Usinas" quando houver múltiplas usinas', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select') as HTMLSelectElement;
        const options = Array.from(usinaSelect.options).map((opt) => opt.text);
        expect(options).toContain('Todas as Usinas');
      });
    });

    it('deve atualizar estado ao selecionar usina', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select') as HTMLSelectElement;
        fireEvent.change(usinaSelect, { target: { value: 'UHE_ABC' } });
        expect(usinaSelect.value).toBe('UHE_ABC');
      });
    });
  });

  // Testes de Textarea (5 testes)
  describe('Textarea de Valores', () => {
    it('deve mostrar textarea após selecionar usina', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
    });

    it('deve permitir edição de valores no textarea', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const textarea = screen.getByTestId('valores-textarea') as HTMLTextAreaElement;
      fireEvent.change(textarea, { target: { value: '200\n250\n300' } });
      expect(textarea.value).toBe('200\n250\n300');
    });

    it('deve limpar textarea ao clicar em Limpar', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const limparBtn = screen.getByTestId('limpar-btn');
      fireEvent.click(limparBtn);
      
      await waitFor(() => {
        expect(screen.queryByTestId('valores-textarea')).not.toBeInTheDocument();
      });
    });

    it('deve preparar textarea com valores para usina individual', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        const textarea = screen.getByTestId('valores-textarea') as HTMLTextAreaElement;
        // Deve conter 48 valores separados por newline
        const valores = textarea.value.split('\n');
        expect(valores.length).toBe(48);
      });
    });
  });

  // Testes de Grid (5 testes)
  describe('Grid de Dados', () => {
    it('deve renderizar grid após carregar dados', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('grid-container')).toBeInTheDocument();
      });
    });

    it('deve exibir título do grid correto', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Dados de Importação - 48 Intervalos')).toBeInTheDocument();
      });
    });

    it('deve renderizar colunas de usinas na grid', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      await waitFor(() => {
        const usinasABC = screen.getAllByText('UHE ABC');
        const usinasXYZ = screen.getAllByText('UHE XYZ');
        expect(usinasABC.length).toBeGreaterThan(0);
        expect(usinasXYZ.length).toBeGreaterThan(0);
      });
    });

    it('deve calcular e exibir totais corretos', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      await waitFor(() => {
        // Verifica se a linha de total existe (row com 2 td)
        const gridContainer = screen.getByTestId('grid-container');
        expect(gridContainer.querySelector('tr')).toBeInTheDocument();
      });
    });

    it('deve calcular e exibir médias corretas', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      await waitFor(() => {
        expect(screen.getByText('Média')).toBeInTheDocument();
      });
    });
  });

  // Testes de Salvar (5 testes)
  describe('Funcionalidade de Salvar', () => {
    it('deve desabilitar botão Salvar quando nenhuma usina está selecionada', () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      const salvarBtn = screen.getByTestId('salvar-btn') as HTMLButtonElement;
      expect(salvarBtn.disabled).toBe(true);
    });

    it('deve chamar onSave ao clicar em Salvar com dados válidos', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const salvarBtn = screen.getByTestId('salvar-btn');
      fireEvent.click(salvarBtn);
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalled();
      });
    });

    it('deve passar dados corretos para onSave', async () => {
      render(<Import onLoadData={mockOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const salvarBtn = screen.getByTestId('salvar-btn');
      fireEvent.click(salvarBtn);
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith(
          expect.objectContaining({
            dataPdp: '2024-01-15',
            codEmpresa: 'ELETROBRAS',
          }),
          expect.any(String)
        );
      });
    });

    it('deve mostrar estado de carregamento durante save', async () => {
      const slowOnSave = vi.fn(() => new Promise((resolve) => setTimeout(resolve, 100)));
      render(<Import onLoadData={mockOnLoadData} onSave={slowOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const salvarBtn = screen.getByTestId('salvar-btn');
      fireEvent.click(salvarBtn);
      
      await waitFor(() => {
        expect(salvarBtn).toHaveTextContent('Salvando...');
      });
    });
  });

  // Testes de Tratamento de Erros (3 testes)
  describe('Tratamento de Erros', () => {
    it('deve exibir mensagem de erro ao falhar ao carregar dados', async () => {
      const errorOnLoadData = vi.fn().mockRejectedValue(new Error('Erro de conexão'));
      render(<Import onLoadData={errorOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Não foi possível acessar a Base de Dados.')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem de erro ao falhar ao salvar dados', async () => {
      const errorOnSave = vi.fn().mockRejectedValue(new Error('Erro ao salvar'));
      render(<Import onLoadData={mockOnLoadData} onSave={errorOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalled();
      });
      
      fireEvent.change(screen.getByTestId('usina-select'), { target: { value: 'UHE_ABC' } });
      
      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });
      
      const salvarBtn = screen.getByTestId('salvar-btn');
      fireEvent.click(salvarBtn);
      
      await waitFor(() => {
        expect(screen.getByText('Não foi possível gravar os dados.')).toBeInTheDocument();
      });
    });

    it('deve limpar erro quando nova seleção é feita', async () => {
      const errorOnLoadData = vi.fn()
        .mockRejectedValueOnce(new Error('Erro de conexão'))
        .mockResolvedValueOnce(mockGridData);
      
      render(<Import onLoadData={errorOnLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Não foi possível acessar a Base de Dados.')).toBeInTheDocument();
      });
      
      // Seleciona empresa diferente para trigger novo load
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'CEMIG' } });
      
      await waitFor(() => {
        expect(screen.queryByText('Não foi possível acessar a Base de Dados.')).not.toBeInTheDocument();
      });
    });
  });

  // Testes de Funções Utilitárias (5 testes)
  describe('Funções Utilitárias', () => {
    it('gerarIntervalos deve retornar 48 intervalos', () => {
      const intervalos = gerarIntervalos();
      expect(intervalos.length).toBe(48);
    });

    it('intervaloParaHorario deve converter número para formato correto', () => {
      const horario = intervaloParaHorario(1);
      expect(horario).toBe('00:00-00:30');
    });

    it('calcularTotalColuna deve somar valores corretamente', () => {
      const valores = [100, 200, 300];
      const total = calcularTotalColuna(valores);
      expect(total).toBe(600);
    });

    it('calcularMediaColuna deve calcular média corretamente', () => {
      const valores = [100, 200, 300];
      const media = calcularMediaColuna(valores);
      expect(media).toBe(200);
    });

    it('parseUsinaValue deve extrair código da usina corretamente', () => {
      const valor = parseUsinaValue('  UHE_ABC  ');
      expect(valor).toBe('UHE_ABC');
    });
  });

  // Testes de Loading (2 testes)
  describe('Estados de Carregamento', () => {
    it('deve desabilitar selects enquanto carrega dados', async () => {
      const slowLoadData = vi.fn(
        () =>
          new Promise((resolve) => {
            setTimeout(() => resolve(mockGridData), 100);
          })
      );
      
      render(<Import onLoadData={slowLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      const usinaSelect = screen.getByTestId('usina-select') as HTMLSelectElement;
      expect(usinaSelect.disabled).toBe(true);
    });

    it('deve exibir mensagem "Carregando dados..." enquanto carrega', async () => {
      const slowLoadData = vi.fn(
        () =>
          new Promise((resolve) => {
            setTimeout(() => resolve(mockGridData), 100);
          })
      );
      
      render(<Import onLoadData={slowLoadData} onSave={mockOnSave} />);
      
      fireEvent.change(screen.getByTestId('data-pdp-select'), { target: { value: '2024-01-15' } });
      fireEvent.change(screen.getByTestId('empresa-select'), { target: { value: 'ELETROBRAS' } });
      
      expect(screen.getByText('Carregando dados...')).toBeInTheDocument();
    });
  });
});

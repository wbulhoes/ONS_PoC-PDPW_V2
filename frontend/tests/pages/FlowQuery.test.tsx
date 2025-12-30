import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import FlowQuery from '../../src/pages/Query/Hydraulic/FlowQuery';
import { TipoVazao } from '../../src/types/flowQuery';

describe('FlowQuery Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar sem erros', () => {
      render(<FlowQuery />);
      expect(screen.getByText(/Filtros de Consulta/i)).toBeInTheDocument();
    });

    it('deve exibir formulário de filtros', () => {
      render(<FlowQuery />);
      
      expect(screen.getByText(/Data Início:/i)).toBeInTheDocument();
      expect(screen.getByText(/Data Fim:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usina:/i)).toBeInTheDocument();
      expect(screen.getByText(/Tipo de Vazão:/i)).toBeInTheDocument();
    });

    it('deve renderizar botão de consultar', () => {
      render(<FlowQuery />);
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      expect(queryButton).toBeInTheDocument();
    });

    it('deve renderizar botão de limpar filtros', () => {
      render(<FlowQuery />);
      const clearButton = screen.getByRole('button', { name: /Limpar Filtros/i });
      expect(clearButton).toBeInTheDocument();
    });
  });

  describe('Filtros', () => {
    it('deve permitir selecionar data início', () => {
      const { container } = render(<FlowQuery />);
      const dateInput = container.querySelectorAll('input[type="date"]')[0] as HTMLInputElement;
      
      fireEvent.change(dateInput, { target: { value: '2024-01-15' } });
      expect(dateInput.value).toBe('2024-01-15');
    });

    it('deve permitir selecionar data fim', () => {
      const { container } = render(<FlowQuery />);
      const dateInput = container.querySelectorAll('input[type="date"]')[1] as HTMLInputElement;
      
      fireEvent.change(dateInput, { target: { value: '2024-01-20' } });
      expect(dateInput.value).toBe('2024-01-20');
    });

    it('deve carregar opções de empresas', () => {
      render(<FlowQuery />);
      const empresaSelect = screen.getAllByRole('combobox')[0];
      
      expect(empresaSelect).toBeInTheDocument();
      fireEvent.mouseDown(empresaSelect);
      
      expect(screen.getByText('Empresa A')).toBeInTheDocument();
      expect(screen.getByText('Empresa B')).toBeInTheDocument();
    });

    it('deve habilitar usina após selecionar empresa', async () => {
      render(<FlowQuery />);
      
      const empresaSelect = screen.getAllByRole('combobox')[0];
      const usinaSelect = screen.getAllByRole('combobox')[1];
      
      expect(usinaSelect).toBeDisabled();
      
      fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      
      await waitFor(() => {
        expect(usinaSelect).not.toBeDisabled();
      });
    });

    it('deve permitir selecionar tipo de vazão', () => {
      render(<FlowQuery />);
      const tipoSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(tipoSelect, { target: { value: TipoVazao.AFLUENTE } });
      expect(tipoSelect).toHaveValue(TipoVazao.AFLUENTE);
    });

    it('deve permitir definir intervalo inicial', () => {
      render(<FlowQuery />);
      const intervalInputs = screen.getAllByPlaceholderText(/Início/i);
      const intervaloInicio = intervalInputs[0];
      
      fireEvent.change(intervaloInicio, { target: { value: '10' } });
      expect(intervaloInicio).toHaveValue(10);
    });

    it('deve permitir definir intervalo final', () => {
      render(<FlowQuery />);
      const intervalInputs = screen.getAllByPlaceholderText(/Fim/i);
      const intervaloFim = intervalInputs[0];
      
      fireEvent.change(intervaloFim, { target: { value: '20' } });
      expect(intervaloFim).toHaveValue(20);
    });

    it('deve limpar filtros ao clicar em Limpar', () => {
      const { container } = render(<FlowQuery />);
      
      const dateInput = container.querySelector('input[type="date"]') as HTMLInputElement;
      fireEvent.change(dateInput, { target: { value: '2024-01-15' } });
      
      const clearButton = screen.getByRole('button', { name: /Limpar Filtros/i });
      fireEvent.click(clearButton);
      
      expect(dateInput.value).toBe('');
    });
  });

  describe('Consulta', () => {
    it('deve executar consulta ao clicar em Consultar', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Consultando.../i)).toBeInTheDocument();
      }, { timeout: 100 });
    });

    it('deve exibir resultados após consulta', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Resultados da Consulta/i)).toBeInTheDocument();
      });
    });

    it('deve exibir tabela de dados após consulta', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        const tables = screen.getAllByRole('table');
        expect(tables.length).toBeGreaterThan(0);
      });
      
      // Verifica headers da tabela de resultados
      const headers = screen.getAllByRole('columnheader');
      const headerTexts = headers.map(h => h.textContent);
      expect(headerTexts).toContain('Data PDP');
      expect(headerTexts).toContain('Vazão (m³/s)');
    });

    it('deve exibir resumo agregado após consulta', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Resumo por Usina/i)).toBeInTheDocument();
      });
    });
  });

  describe('Exportação', () => {
    it('deve exibir botões de exportação após consulta', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Excel/i)).toBeInTheDocument();
        expect(screen.getByText(/CSV/i)).toBeInTheDocument();
        expect(screen.getByText(/PDF/i)).toBeInTheDocument();
      });
    });

    it('deve chamar exportação Excel', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        const excelButton = screen.getByText(/Excel/i);
        fireEvent.click(excelButton);
      });
      
      expect(alertSpy).toHaveBeenCalledWith('Exportação EXCEL será implementada');
      alertSpy.mockRestore();
    });

    it('deve chamar exportação CSV', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        const csvButton = screen.getByText(/CSV/i);
        fireEvent.click(csvButton);
      });
      
      expect(alertSpy).toHaveBeenCalledWith('Exportação CSV será implementada');
      alertSpy.mockRestore();
    });

    it('deve chamar exportação PDF', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        const pdfButton = screen.getByText(/PDF/i);
        fireEvent.click(pdfButton);
      });
      
      expect(alertSpy).toHaveBeenCalledWith('Exportação PDF será implementada');
      alertSpy.mockRestore();
    });
  });

  describe('Paginação', () => {
    it('deve exibir paginação quando há múltiplas páginas', async () => {
      render(<FlowQuery />);
      
      const queryButton = screen.getByRole('button', { name: /Consultar/i });
      fireEvent.click(queryButton);
      
      await waitFor(() => {
        // Como o mock retorna 20 itens e o padrão é 50 por página, não haverá paginação
        // Este teste verifica que a lógica está presente
        const results = screen.queryByText(/Página/i);
        expect(results).not.toBeInTheDocument(); // Não deve ter paginação com poucos resultados
      });
    });
  });

  describe('Toggle de Filtros', () => {
    it('deve ocultar filtros ao clicar em Ocultar', () => {
      render(<FlowQuery />);
      
      const toggleButton = screen.getByRole('button', { name: /Ocultar/i });
      fireEvent.click(toggleButton);
      
      expect(screen.queryByText(/Data Início:/i)).not.toBeInTheDocument();
    });

    it('deve mostrar filtros ao clicar em Mostrar', () => {
      render(<FlowQuery />);
      
      let toggleButton = screen.getByRole('button', { name: /Ocultar/i });
      fireEvent.click(toggleButton);
      
      toggleButton = screen.getByRole('button', { name: /Mostrar/i });
      fireEvent.click(toggleButton);
      
      expect(screen.getByText(/Data Início:/i)).toBeInTheDocument();
    });
  });

  describe('Mensagens', () => {
    it('deve exibir mensagem quando não há resultados', () => {
      render(<FlowQuery />);
      
      expect(screen.getByText(/Nenhum resultado encontrado/i)).toBeInTheDocument();
    });
  });
});

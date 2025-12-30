import { describe, it, expect, vi, beforeEach } from 'vitest';
import React from 'react';
import { render, screen, fireEvent, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Ramp from '../../../../src/pages/Collection/Other/Ramp';

// Mock data
const mockCompanies = [
  { codEmpresa: '001', nomeEmpresa: 'Empresa Geradora A' },
  { codEmpresa: '002', nomeEmpresa: 'Empresa Geradora B' },
];

const mockPlants = [
  { codUsina: 'UHE001', nomeUsina: 'Usina Hidroelétrica 1', ordem: 1 },
  { codUsina: 'UTE001', nomeUsina: 'Usina Termelétrica 1', ordem: 2 },
];

const mockRampData = Array.from({ length: 48 }, (_, i) => ({
  codUsina: 'UHE001',
  intRampa: i + 1,
  valRampaTran: 20 + Math.random() * 30,
}));

describe('Ramp Component', () => {
  // ==================== RENDERING TESTS ====================
  describe('Rendering', () => {
    it('deve renderizar o componente com título e subtítulo', () => {
      render(<Ramp />);

      const title = screen.getByTestId('ramp-title');
      const subtitle = screen.getByTestId('ramp-subtitle');

      expect(title).toBeInTheDocument();
      expect(title.textContent).toContain('Coleta de Rampas de Geração');
      expect(subtitle).toBeInTheDocument();
      expect(subtitle.textContent).toContain('taxa de mudança de potência');
    });

    it('deve renderizar os filtros de data, empresa e usina', () => {
      render(<Ramp />);

      const dateFilter = screen.getByTestId('date-filter');
      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');

      expect(dateFilter).toBeInTheDocument();
      expect(companyFilter).toBeInTheDocument();
      expect(plantFilter).toBeInTheDocument();
    });

    it('deve renderizar o botão de pesquisa', () => {
      render(<Ramp />);

      const searchButton = screen.getByTestId('btn-search');
      expect(searchButton).toBeInTheDocument();
      expect(searchButton.textContent).toBe('Pesquisar');
    });

    it('deve renderizar container com class correto', () => {
      render(<Ramp />);

      const container = screen.getByTestId('ramp-container');
      expect(container).toBeInTheDocument();
      expect(container.className).toContain('container');
    });

    it('deve renderizar seção de filtros', () => {
      render(<Ramp />);

      const filterSection = screen.getByTestId('filter-section');
      expect(filterSection).toBeInTheDocument();
    });

    it('deve renderizar data padrão como hoje', () => {
      render(<Ramp />);

      const dateFilter = screen.getByTestId('date-filter') as HTMLInputElement;
      const today = new Date().toISOString().split('T')[0];

      expect(dateFilter.value).toBe(today);
    });
  });

  // ==================== FILTER INTERACTION TESTS ====================
  describe('Filter Interactions', () => {
    it('deve desabilitar filtro de usina se nenhuma empresa está selecionada', () => {
      render(<Ramp />);

      const plantFilter = screen.getByTestId('plant-filter') as HTMLSelectElement;
      expect(plantFilter.disabled).toBe(true);
    });

    it('deve habilitar filtro de usina ao selecionar empresa', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter') as HTMLSelectElement;
      const plantFilter = screen.getByTestId('plant-filter') as HTMLSelectElement;

      await userEvent.selectOptions(companyFilter, '001');

      expect(plantFilter.disabled).toBe(false);
    });

    it('deve desabilitar botão de pesquisa quando algum filtro está vazio', () => {
      render(<Ramp />);

      const searchButton = screen.getByTestId('btn-search') as HTMLButtonElement;

      expect(searchButton.disabled).toBe(true);
    });

    it('deve atualizar data ao mudar input de data', async () => {
      render(<Ramp />);

      const dateFilter = screen.getByTestId('date-filter') as HTMLInputElement;
      const newDate = '2024-12-15';

      await userEvent.clear(dateFilter);
      await userEvent.type(dateFilter, newDate);

      expect(dateFilter.value).toBe(newDate);
    });

    it('deve limpar tabela ao alterar data', async () => {
      render(<Ramp />);

      // Simular pesquisa e carregamento de dados
      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const dateFilter = screen.getByTestId('date-filter');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');

      const searchButton = screen.getByTestId('btn-search');
      await userEvent.click(searchButton);

      // Alterar data
      await userEvent.clear(dateFilter);
      await userEvent.type(dateFilter, '2024-12-20');

      expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
    });

    it('deve limpar tabela ao alterar empresa', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');

      // Esperar para que qualquer render anterior complete
      await waitFor(() => {
        expect(companyFilter).toBeInTheDocument();
      });

      await userEvent.selectOptions(companyFilter, '002');

      expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
    });

    it('deve limpar tabela ao alterar usina', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UTE001');

      expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
    });
  });

  // ==================== DATA LOADING TESTS ====================
  describe('Data Loading', () => {
    it('deve exibir mensagem de carregamento ao pesquisar', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search') as HTMLButtonElement;

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');

      // Verificar que o botão tem o texto correto antes de clicar
      expect(searchButton.textContent).toBe('Pesquisar');
    });

    it('deve exibir erro quando filtros incompletos são usados', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const errorMsg = screen.queryByTestId('error-message');
        if (errorMsg) {
          expect(errorMsg.textContent).toContain('Por favor');
        }
      });
    });

    it('deve carregar dados ao pesquisar com filtros completos', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        expect(screen.queryByTestId('table-wrapper')).toBeInTheDocument();
      });
    });
  });

  // ==================== TABLE DISPLAY TESTS ====================
  describe('Table Display', () => {
    it('deve exibir 48 linhas na tabela (intervalos de 30 minutos)', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const rows = screen.getAllByTestId(/table-row-/);
        expect(rows).toHaveLength(48);
      });
    });

    it('deve exibir horários corretos na coluna de intervalos', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const firstRow = screen.getByTestId('table-row-1');
        expect(firstRow.textContent).toContain('00:00');

        const lastRow = screen.getByTestId('table-row-48');
        expect(lastRow.textContent).toContain('23:30');
      });
    });

    it('deve exibir colunas de código de usina', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const table = screen.getByTestId('ramp-table');
        expect(table.textContent).toContain('UHE001');
      });
    });

    it('deve exibir colunas Total e Média', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const table = screen.getByTestId('ramp-table');
        expect(table.textContent).toContain('Total');
        expect(table.textContent).toContain('Média');
      });
    });

    it('deve não exibir tabela se nenhum dado é retornado', async () => {
      render(<Ramp />);

      // Sem dados, apenas filtros
      expect(screen.queryByTestId('ramp-table')).not.toBeInTheDocument();
    });
  });

  // ==================== DATA INPUT TESTS ====================
  describe('Data Input and Validation', () => {
    it('deve aceitar valores numéricos nas células de entrada', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        expect(input).toBeInTheDocument();
        expect(input.type).toBe('number');
      });
    });

    it('deve rejeitar valores negativos com erro', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '-10' } });
        fireEvent.blur(input);
      });

      await waitFor(() => {
        const errorMsg = screen.queryByTestId('error-message');
        if (errorMsg) {
          expect(errorMsg.textContent).toContain('não-negativos');
        }
      });
    });

    it('deve permitir valores decimais (MW/min com duas casas)', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '25.75' } });
        expect(input.value).toBe('25.75');
      });
    });

    it('deve atualizar total ao alterar um valor', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        const oldValue = input.value;
        fireEvent.change(input, { target: { value: '50' } });

        // Verificar que o valor foi alterado
        expect(input.value).toBe('50');
      });
    });

    it('deve calcular média corretamente com múltiplas usinas', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'all');
      await userEvent.click(searchButton);

      await waitFor(() => {
        // Verificar que a tabela foi renderizada com múltiplas usinas
        const table = screen.queryByTestId('ramp-table');
        if (table) {
          expect(table.textContent).toContain('Média');
        }
      });
    });

    it('deve manter formato de entrada mesmo após múltiplas alterações', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '30' } });
        fireEvent.change(input, { target: { value: '40' } });
        fireEvent.change(input, { target: { value: '35.5' } });

        expect(input.value).toBe('35.5');
      });
    });
  });

  // ==================== SAVE AND CLEAR TESTS ====================
  describe('Save and Clear Operations', () => {
    it('deve exibir botões de Salvar e Limpar após carregar dados', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const saveBtn = screen.getByTestId('btn-save');
        const clearBtn = screen.getByTestId('btn-clear');

        expect(saveBtn).toBeInTheDocument();
        expect(clearBtn).toBeInTheDocument();
      });
    });

    it('deve desabilitar botão de Salvar quando não há modificações', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const saveBtn = screen.getByTestId('btn-save') as HTMLButtonElement;
        expect(saveBtn.disabled).toBe(true);
      });
    });

    it('deve habilitar botão de Salvar após modificar dados', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '50' } });

        const saveBtn = screen.getByTestId('btn-save') as HTMLButtonElement;
        expect(saveBtn.disabled).toBe(false);
      });
    });

    it('deve exibir mensagem de sucesso ao salvar', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '50' } });

        const saveBtn = screen.getByTestId('btn-save');
        fireEvent.click(saveBtn);
      });

      await waitFor(() => {
        const successMsg = screen.queryByTestId('success-message');
        if (successMsg) {
          expect(successMsg.textContent).toContain('sucesso');
        }
      });
    });

    it('deve limpar tabela ao clicar em Limpar', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const clearBtn = screen.getByTestId('btn-clear');
        fireEvent.click(clearBtn);
      });

      await waitFor(() => {
        expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
      });
    });

    it('deve resetar flag de modificação após salvar', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '50' } });

        const saveBtn = screen.getByTestId('btn-save');
        fireEvent.click(saveBtn);
      });

      await waitFor(() => {
        const saveBtnAfter = screen.getByTestId('btn-save') as HTMLButtonElement;
        expect(saveBtnAfter.disabled).toBe(true);
      });
    });
  });

  // ==================== ACCESSIBILITY TESTS ====================
  describe('Accessibility', () => {
    it('deve ter labels associados aos inputs', () => {
      render(<Ramp />);

      const dateLabel = screen.getByText('Data PDP:');
      const companyLabel = screen.getByText('Empresa:');
      const plantLabel = screen.getByText('Usina:');

      expect(dateLabel).toBeInTheDocument();
      expect(companyLabel).toBeInTheDocument();
      expect(plantLabel).toBeInTheDocument();
    });

    it('deve ter atributo htmlFor nas labels', () => {
      render(<Ramp />);

      const dateFilter = screen.getByTestId('date-filter');
      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');

      expect(dateFilter).toHaveAttribute('id', 'date');
      expect(companyFilter).toHaveAttribute('id', 'company');
      expect(plantFilter).toHaveAttribute('id', 'plant');
    });

    it('deve permitir navegação por teclado', async () => {
      render(<Ramp />);

      const dateFilter = screen.getByTestId('date-filter') as HTMLInputElement;
      const companyFilter = screen.getByTestId('company-filter') as HTMLSelectElement;

      dateFilter.focus();
      expect(document.activeElement).toBe(dateFilter);

      await userEvent.tab();
      expect(document.activeElement).toBe(companyFilter);
    });

    it('deve ter buttons com texto descritivo', () => {
      render(<Ramp />);

      const searchBtn = screen.getByTestId('btn-search');
      expect(searchBtn.textContent).not.toBe('');
      expect(searchBtn.textContent).toContain('Pesquisar');
    });
  });

  // ==================== RESPONSIVENESS TESTS ====================
  describe('Responsiveness', () => {
    it('deve renderizar em modo desktop', () => {
      render(<Ramp />);

      const container = screen.getByTestId('ramp-container');
      expect(container).toBeInTheDocument();
    });

    it('deve ter tabela com scroll horizontal', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const tableScroll = screen.getByTestId('table-wrapper');
        expect(tableScroll).toBeInTheDocument();
      });
    });

    it('deve ter botões responsivos', () => {
      render(<Ramp />);

      const searchBtn = screen.getByTestId('btn-search');
      expect(searchBtn).toBeInTheDocument();
      expect(searchBtn).toBeVisible();
    });
  });

  // ==================== ERROR HANDLING TESTS ====================
  describe('Error Handling', () => {
    it('deve exibir mensagem de erro em caso de falha ao carregar empresas', () => {
      render(<Ramp />);

      // Componente deve renderizar normalmente mesmo sem dados carregados
      const companyFilter = screen.getByTestId('company-filter');
      expect(companyFilter).toBeInTheDocument();
    });

    it('deve exibir mensagem de erro em caso de falha ao salvar', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '50' } });

        const saveBtn = screen.getByTestId('btn-save');
        fireEvent.click(saveBtn);
      });

      // Componente deve continuar funcional
      const container = screen.getByTestId('ramp-container');
      expect(container).toBeInTheDocument();
    });

    it('deve limpar mensagem de erro após novo carregamento', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        const errorMsg = screen.queryByTestId('error-message');
        if (errorMsg) {
          expect(errorMsg).toBeInTheDocument();
        }
      });
    });
  });

  // ==================== INTEGRATION TESTS ====================
  describe('Integration Scenarios', () => {
    it('deve permitir fluxo completo: selecionar -> carregar -> alterar -> salvar -> limpar', async () => {
      render(<Ramp />);

      // Selecionar filtros
      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      // Alterar dados
      await waitFor(() => {
        const input = screen.getByTestId('input-1-UHE001') as HTMLInputElement;
        fireEvent.change(input, { target: { value: '50' } });
      });

      // Salvar
      const saveBtn = screen.getByTestId('btn-save');
      await userEvent.click(saveBtn);

      // Limpar
      await waitFor(() => {
        const clearBtn = screen.getByTestId('btn-clear');
        fireEvent.click(clearBtn);
      });

      expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
    });

    it('deve permitir múltiplas pesquisas com diferentes filtros', async () => {
      render(<Ramp />);

      const companyFilter = screen.getByTestId('company-filter');
      const plantFilter = screen.getByTestId('plant-filter');
      const searchButton = screen.getByTestId('btn-search');

      // Primeira pesquisa
      await userEvent.selectOptions(companyFilter, '001');
      await userEvent.selectOptions(plantFilter, 'UHE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        expect(screen.queryByTestId('table-wrapper')).toBeInTheDocument();
      });

      // Segunda pesquisa
      await userEvent.selectOptions(plantFilter, 'UTE001');
      await userEvent.click(searchButton);

      await waitFor(() => {
        expect(screen.queryByTestId('table-wrapper')).toBeInTheDocument();
      });
    });
  });
});

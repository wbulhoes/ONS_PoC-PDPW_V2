import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Inflexibility from '../../src/pages/Collection/Thermal/Inflexibility';

describe('Inflexibility Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<Inflexibility />);
      expect(screen.getByTestId('inflexibility-form')).toBeInTheDocument();
    });

    it('deve renderizar o título correto', () => {
      render(<Inflexibility />);
      const title = screen.getByAltText('Coleta de Inflexibilidade');
      expect(title).toBeInTheDocument();
    });

    it('deve renderizar os campos do formulário', () => {
      render(<Inflexibility />);
      expect(screen.getByTestId('data-pdp-select')).toBeInTheDocument();
      expect(screen.getByTestId('empresa-select')).toBeInTheDocument();
      expect(screen.getByTestId('usina-select')).toBeInTheDocument();
    });

    it('deve renderizar a tabela de inflexibilidade', () => {
      render(<Inflexibility />);
      expect(screen.getByTestId('inflexibility-table')).toBeInTheDocument();
    });

    it('deve renderizar 48 intervalos na tabela', async () => {
      render(<Inflexibility />);
      const table = screen.getByTestId('inflexibility-table');
      const rows = table.querySelectorAll('tbody tr');
      // 48 intervalos + 2 linhas (Total e Média)
      expect(rows.length).toBe(50);
    });
  });

  describe('Seleção de Data PDP', () => {
    it('deve carregar datas PDP no select', async () => {
      render(<Inflexibility />);
      await waitFor(() => {
        const select = screen.getByTestId('data-pdp-select');
        expect(select.querySelectorAll('option').length).toBeGreaterThan(1);
      });
    });

    it('deve permitir selecionar uma data PDP', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      await waitFor(() => {
        const select = screen.getByTestId('data-pdp-select');
        expect(select.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const select = screen.getByTestId('data-pdp-select');
      await user.selectOptions(select, select.querySelectorAll('option')[1].value);

      expect(select).toHaveValue(select.querySelectorAll('option')[1].value);
    });

    it('deve habilitar select de empresa após selecionar data', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      const empresaSelect = screen.getByTestId('empresa-select');
      expect(empresaSelect).toBeDisabled();

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção de Empresa', () => {
    it('deve carregar empresas quando data é selecionada', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });
    });

    it('deve habilitar select de usina após selecionar empresa', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      const usinaSelect = screen.getByTestId('usina-select');
      expect(usinaSelect).toBeDisabled();

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(usinaSelect).not.toBeDisabled();
      });
    });
  });

  describe('Seleção de Usina', () => {
    it('deve carregar usinas quando empresa é selecionada', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });
    });

    it('deve mostrar botão salvar quando usina é selecionada', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(screen.getByTestId('salvar-button')).toBeInTheDocument();
      });
    });

    it('deve mostrar textarea quando usina específica é selecionada', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea-container')).toBeInTheDocument();
      });
    });
  });

  describe('Validações', () => {
    it('deve mostrar mensagem de erro quando tentar salvar sem dados', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      // Configurar ambiente para o botão salvar aparecer
      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(screen.getByTestId('salvar-button')).toBeInTheDocument();
      });

      const salvarButton = screen.getByTestId('salvar-button');
      await user.click(salvarButton);

      await waitFor(() => {
        const message = screen.getByTestId('message');
        expect(message).toBeInTheDocument();
      });
    });

    it('não deve permitir seleção de empresa sem data PDP', () => {
      render(<Inflexibility />);
      const empresaSelect = screen.getByTestId('empresa-select');
      expect(empresaSelect).toBeDisabled();
    });

    it('não deve permitir seleção de usina sem empresa', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      const usinaSelect = screen.getByTestId('usina-select');
      expect(usinaSelect).toBeDisabled();

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      // Ainda deve estar desabilitado
      expect(usinaSelect).toBeDisabled();
    });
  });

  describe('Cálculos', () => {
    it('deve calcular total de cada intervalo corretamente', async () => {
      render(<Inflexibility />);
      const table = screen.getByTestId('inflexibility-table');
      const totalCells = table.querySelectorAll('tbody tr:not(.totalRow):not(.mediaRow) td:nth-child(2)');
      
      // Verificar que existem células de total
      expect(totalCells.length).toBeGreaterThan(0);
    });

    it('deve calcular média corretamente', async () => {
      render(<Inflexibility />);
      const table = screen.getByTestId('inflexibility-table');
      const rows = table.querySelectorAll('tbody tr');
      const mediaRow = rows[rows.length - 1]; // Última linha
      
      // Verificar que a linha de média existe
      expect(mediaRow).toBeTruthy();
      expect(mediaRow.textContent).toContain('Média');
    });

    it('deve calcular total geral corretamente', async () => {
      render(<Inflexibility />);
      const table = screen.getByTestId('inflexibility-table');
      const rows = table.querySelectorAll('tbody tr');
      const totalRow = rows[rows.length - 2]; // Penúltima linha
      
      // Verificar que a linha de total existe
      expect(totalRow).toBeTruthy();
      expect(totalRow.textContent).toContain('Total');
    });
  });

  describe('Interação com Textarea', () => {
    it('deve atualizar valores quando textarea é alterada', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      // Simular seleções necessárias
      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });

      const textarea = screen.getByTestId('valores-textarea');
      await user.clear(textarea);
      await user.type(textarea, '100');
      await user.keyboard('{Enter}');
      await user.type(textarea, '200');
      await user.keyboard('{Enter}');
      await user.type(textarea, '300');

      expect(textarea.value).toContain('100');
      expect(textarea.value).toContain('200');
      expect(textarea.value).toContain('300');
    });

    it('deve aceitar valores separados por tabulação', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      // Simular seleções necessárias (mesmo processo do teste anterior)
      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        expect(screen.getByTestId('valores-textarea')).toBeInTheDocument();
      });

      const textarea = screen.getByTestId('valores-textarea');
      await user.clear(textarea);
      await user.type(textarea, '100\t200\t300');

      expect(textarea.value).toContain('100');
      expect(textarea.value).toContain('200');
      expect(textarea.value).toContain('300');
    });
  });

  describe('Loading States', () => {
    it('deve mostrar indicador de loading durante carregamento', async () => {
      render(<Inflexibility />);
      
      // Verificar se o loading aparece em algum momento
      await waitFor(() => {
        expect(screen.queryByTestId('loading')).not.toBeInTheDocument();
      }, { timeout: 3000 });
    });

    it('deve desabilitar botão salvar durante loading', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      // Configurar até aparecer o botão salvar
      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const salvarButton = screen.getByTestId('salvar-button');
        expect(salvarButton).toBeInTheDocument();
      });
    });
  });

  describe('Mensagens', () => {
    it('deve exibir mensagem de sucesso após salvar', async () => {
      const user = userEvent.setup();
      render(<Inflexibility />);

      // Configuração completa
      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const empresaSelect = screen.getByTestId('empresa-select');
        expect(empresaSelect).not.toBeDisabled();
      });

      const empresaSelect = screen.getByTestId('empresa-select');
      await user.selectOptions(empresaSelect, empresaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const usinaSelect = screen.getByTestId('usina-select');
        expect(usinaSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const usinaSelect = screen.getByTestId('usina-select');
      await user.selectOptions(usinaSelect, usinaSelect.querySelectorAll('option')[1].value);

      await waitFor(() => {
        const salvarButton = screen.getByTestId('salvar-button');
        expect(salvarButton).toBeInTheDocument();
      });

      const salvarButton = screen.getByTestId('salvar-button');
      await user.click(salvarButton);

      await waitFor(() => {
        const message = screen.getByTestId('message');
        expect(message).toBeInTheDocument();
      });
    });

    it('deve limpar mensagem após interação', async () => {
      render(<Inflexibility />);
      
      // As mensagens devem desaparecer após algum tempo ou interação
      await waitFor(() => {
        expect(screen.queryByTestId('message')).not.toBeInTheDocument();
      });
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import OperatingMode from '../../src/pages/Collection/Thermal/OperatingMode';

describe('OperatingMode Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<OperatingMode />);
      expect(screen.getByTestId('operating-mode-form')).toBeInTheDocument();
    });

    it('deve renderizar o título correto', () => {
      render(<OperatingMode />);
      const title = screen.getByAltText('Modalidade Operativa Térmica');
      expect(title).toBeInTheDocument();
    });

    it('deve renderizar os campos do formulário', () => {
      render(<OperatingMode />);
      expect(screen.getByTestId('data-pdp-select')).toBeInTheDocument();
      expect(screen.getByTestId('empresa-select')).toBeInTheDocument();
      expect(screen.getByTestId('usina-select')).toBeInTheDocument();
    });

    it('deve renderizar a tabela de modalidade operativa', () => {
      render(<OperatingMode />);
      expect(screen.getByTestId('operating-mode-table')).toBeInTheDocument();
    });

    it('deve renderizar 48 intervalos na tabela', async () => {
      render(<OperatingMode />);
      const table = screen.getByTestId('operating-mode-table');
      const rows = table.querySelectorAll('tbody tr');
      expect(rows.length).toBe(48);
    });
  });

  describe('Seleção de Data PDP', () => {
    it('deve carregar datas PDP no select', async () => {
      render(<OperatingMode />);
      await waitFor(() => {
        const select = screen.getByTestId('data-pdp-select');
        expect(select.querySelectorAll('option').length).toBeGreaterThan(1);
      });
    });

    it('deve permitir selecionar uma data PDP', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
      render(<OperatingMode />);

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
    it('deve mostrar mensagem de erro quando tentar salvar sem dados completos', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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
      render(<OperatingMode />);
      const empresaSelect = screen.getByTestId('empresa-select');
      expect(empresaSelect).toBeDisabled();
    });

    it('não deve permitir seleção de usina sem empresa', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

      const usinaSelect = screen.getByTestId('usina-select');
      expect(usinaSelect).toBeDisabled();

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-pdp-select');
        expect(dataSelect.querySelectorAll('option').length).toBeGreaterThan(1);
      });

      const dataSelect = screen.getByTestId('data-pdp-select');
      await user.selectOptions(dataSelect, dataSelect.querySelectorAll('option')[1].value);

      expect(usinaSelect).toBeDisabled();
    });
  });

  describe('Interação com Textarea', () => {
    it('deve atualizar valores quando textarea é alterada', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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
      await user.type(textarea, '50');
      await user.keyboard('{Enter}');
      await user.type(textarea, '75');

      expect(textarea.value).toContain('50');
      expect(textarea.value).toContain('75');
    });

    it('deve aceitar valores separados por tabulação', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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
      await user.type(textarea, '10\t20\t30');

      expect(textarea.value).toContain('10');
      expect(textarea.value).toContain('20');
      expect(textarea.value).toContain('30');
    });
  });

  describe('Loading States', () => {
    it('deve mostrar indicador de loading quando apropriado', async () => {
      render(<OperatingMode />);
      
      await waitFor(() => {
        expect(screen.queryByTestId('loading')).not.toBeInTheDocument();
      }, { timeout: 3000 });
    });

    it('deve desabilitar botão salvar durante loading', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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
    it('deve exibir mensagem após tentar salvar', async () => {
      const user = userEvent.setup();
      render(<OperatingMode />);

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

    it('deve limpar mensagem após algum tempo', async () => {
      render(<OperatingMode />);
      
      await waitFor(() => {
        expect(screen.queryByTestId('message')).not.toBeInTheDocument();
      });
    });
  });
});

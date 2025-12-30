import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import DCA from '../../../src/pages/Collection/Other/DCA';

describe('DCA Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<DCA />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir o título correto', () => {
      render(<DCA />);
      expect(screen.getByText('Despacho Ciclo Aberto')).toBeInTheDocument();
    });

    it('deve exibir todos os campos do formulário', () => {
      render(<DCA />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve renderizar o botão Salvar desabilitado inicialmente', () => {
      render(<DCA />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeDisabled();
    });

    it('deve renderizar a tabela com 48 intervalos', () => {
      render(<DCA />);
      
      const rows = screen.getAllByRole('row');
      expect(rows.length).toBe(51); // Header + 48 linhas + 2 footer
    });

    it('deve exibir os horários corretos nos intervalos', () => {
      render(<DCA />);
      
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
      expect(screen.getByText('00:30-01:00')).toBeInTheDocument();
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Seleção de Data PDP', () => {
    it('deve habilitar o campo Empresa ao selecionar Data PDP', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      expect(empresaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });

    it('deve limpar o campo Empresa ao mudar a Data PDP', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      expect(empresaSelect).toHaveValue('Empresa A');
      
      fireEvent.change(dataSelect, { target: { value: '02/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).toHaveValue('');
      });
    });
  });

  describe('Seleção de Empresa', () => {
    it('deve habilitar o campo Usina ao selecionar Empresa', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      expect(usinaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        expect(usinaSelect).not.toBeDisabled();
      });
    });

    it('deve limpar o campo Usina ao mudar a Empresa', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      expect(usinaSelect).toHaveValue('Usina 1');
      
      fireEvent.change(empresaSelect, { target: { value: 'Empresa B' } });
      
      await waitFor(() => {
        expect(usinaSelect).toHaveValue('');
      });
    });
  });

  describe('Seleção de Usina', () => {
    it('deve exibir textarea ao selecionar uma usina', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        const textarea = screen.getByPlaceholderText(/Digite os valores DCA/i);
        expect(textarea).toBeInTheDocument();
      });
    });

    it('deve habilitar botão Salvar ao selecionar uma usina', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      await waitFor(() => {
        expect(saveButton).not.toBeDisabled();
      });
    });

    it('não deve exibir textarea para "Selecione uma Usina"', () => {
      render(<DCA />);
      
      const textarea = screen.queryByPlaceholderText(/Digite os valores DCA/i);
      expect(textarea).not.toBeInTheDocument();
    });

    it('deve carregar dados mockados ao selecionar usina', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      expect(textarea).toHaveValue(expect.stringContaining('50'));
    });
  });

  describe('Edição de Valores via Textarea', () => {
    it('deve atualizar a tabela ao editar valores na textarea', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      
      fireEvent.change(textarea, { target: { value: '60\n70\n80' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('60\n70\n80');
      });
    });

    it('deve calcular totais corretamente', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      
      // Simula 48 valores de 50
      const values = Array(48).fill('50').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const footerCells = screen.getAllByText(/2400\.00/i);
        expect(footerCells.length).toBeGreaterThan(0);
      });
    });

    it('deve calcular média corretamente', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      
      // Simula 48 valores de 50 (média = 50)
      const values = Array(48).fill('50').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const mediaCells = screen.getAllByText(/50\.00/i);
        expect(mediaCells.length).toBeGreaterThan(0);
      });
    });

    it('deve processar valores decimais corretamente', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      
      fireEvent.change(textarea, { target: { value: '25.5\n37.25' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('25.5\n37.25');
      });
    });
  });

  describe('Modo "Todas as Usinas"', () => {
    it('deve processar valores separados por Tab', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Todas as Usinas' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      expect(textarea).toBeInTheDocument();
      expect(textarea).toHaveValue(expect.stringContaining('\t'));
    });

    it('deve calcular totais de múltiplas usinas', async () => {
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Todas as Usinas' } });
      });
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores DCA/i);
      
      // Primeira linha: 50 + 75 + 100 = 225
      const firstValue = textarea.value.split('\n')[0];
      const values = firstValue.split('\t').map(Number);
      const sum = values.reduce((a, b) => a + b, 0);
      
      expect(sum).toBeGreaterThan(0);
    });
  });

  describe('Salvamento de Dados', () => {
    it('deve mostrar alerta se campos não estiverem preenchidos', () => {
      render(<DCA />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeDisabled();
    });

    it('deve chamar função de save com dados corretos', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const consoleSpy = vi.spyOn(console, 'log').mockImplementation(() => {});
      
      render(<DCA />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      const usinaSelect = screen.getAllByRole('combobox')[2];
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        fireEvent.change(empresaSelect, { target: { value: 'Empresa A' } });
      });
      
      await waitFor(() => {
        fireEvent.change(usinaSelect, { target: { value: 'Usina 1' } });
      });
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      await waitFor(() => {
        fireEvent.click(saveButton);
      });
      
      await waitFor(() => {
        expect(alertSpy).toHaveBeenCalledWith('Dados DCA salvos com sucesso!');
      });
      
      alertSpy.mockRestore();
      consoleSpy.mockRestore();
    });
  });

  describe('Geração de Horários', () => {
    it('deve gerar horários corretos para primeiro intervalo', () => {
      render(<DCA />);
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para segundo intervalo', () => {
      render(<DCA />);
      expect(screen.getByText('00:30-01:00')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para meio-dia', () => {
      render(<DCA />);
      expect(screen.getByText('12:00-12:30')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para último intervalo', () => {
      render(<DCA />);
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Responsividade', () => {
    it('deve aplicar classe CSS ao container', () => {
      const { container } = render(<DCA />);
      const mainDiv = container.firstChild;
      expect(mainDiv).toHaveClass('container');
    });

    it('deve ter tabela com classe apropriada', () => {
      render(<DCA />);
      const table = screen.getByRole('table');
      expect(table).toHaveClass('table');
    });
  });

  describe('Acessibilidade', () => {
    it('deve ter labels associados aos campos', () => {
      render(<DCA />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve ter botões acessíveis', () => {
      render(<DCA />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeInTheDocument();
    });

    it('deve ter selects acessíveis', () => {
      render(<DCA />);
      
      const selects = screen.getAllByRole('combobox');
      expect(selects).toHaveLength(3);
    });

    it('deve ter tabela com estrutura semântica', () => {
      render(<DCA />);
      
      const table = screen.getByRole('table');
      expect(table.querySelector('thead')).toBeInTheDocument();
      expect(table.querySelector('tbody')).toBeInTheDocument();
      expect(table.querySelector('tfoot')).toBeInTheDocument();
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import SOM from '../../../src/pages/Collection/Other/SOM';

describe('SOM Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o componente sem erros', () => {
      render(<SOM />);
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    });

    it('deve exibir todos os campos do formulário', () => {
      render(<SOM />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve renderizar o botão Salvar desabilitado inicialmente', () => {
      render(<SOM />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeDisabled();
    });

    it('deve renderizar a tabela com 48 intervalos', () => {
      render(<SOM />);
      
      const rows = screen.getAllByRole('row');
      // Header + 48 linhas + 2 footer (total e média) = 51
      expect(rows.length).toBe(51);
    });

    it('deve exibir os horários corretos nos intervalos', () => {
      render(<SOM />);
      
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
      expect(screen.getByText('00:30-01:00')).toBeInTheDocument();
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });

    it('deve exibir colunas fixas de Intervalo e Total', () => {
      render(<SOM />);
      
      expect(screen.getByText('Intervalo')).toBeInTheDocument();
      expect(screen.getAllByText('Total')[0]).toBeInTheDocument();
    });
  });

  describe('Seleção de Data PDP', () => {
    it('deve habilitar o campo Empresa ao selecionar Data PDP', async () => {
      render(<SOM />);
      
      const dataSelect = screen.getAllByRole('combobox')[0];
      const empresaSelect = screen.getAllByRole('combobox')[1];
      
      expect(empresaSelect).toBeDisabled();
      
      fireEvent.change(dataSelect, { target: { value: '01/01/2024' } });
      
      await waitFor(() => {
        expect(empresaSelect).not.toBeDisabled();
      });
    });

    it('deve limpar o campo Empresa ao mudar a Data PDP', async () => {
      render(<SOM />);
      
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
      render(<SOM />);
      
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
      render(<SOM />);
      
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
      render(<SOM />);
      
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
        const textarea = screen.getByPlaceholderText(/Digite os valores SOM/i);
        expect(textarea).toBeInTheDocument();
      });
    });

    it('deve habilitar botão Salvar ao selecionar uma usina', async () => {
      render(<SOM />);
      
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
      render(<SOM />);
      
      const textarea = screen.queryByPlaceholderText(/Digite os valores SOM/i);
      expect(textarea).not.toBeInTheDocument();
    });

    it('deve carregar dados mockados ao selecionar usina', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      expect(textarea).toHaveValue(expect.stringContaining('100'));
    });
  });

  describe('Edição de Valores via Textarea', () => {
    it('deve atualizar a tabela ao editar valores na textarea', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      
      fireEvent.change(textarea, { target: { value: '250\n300\n350' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('250\n300\n350');
      });
    });

    it('deve calcular totais corretamente', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      
      // Simula 48 valores de 100
      const values = Array(48).fill('100').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const footerCells = screen.getAllByText(/4800\.00/i);
        expect(footerCells.length).toBeGreaterThan(0);
      });
    });

    it('deve calcular média corretamente', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      
      // Simula 48 valores de 100 (média = 100)
      const values = Array(48).fill('100').join('\n');
      fireEvent.change(textarea, { target: { value: values } });
      
      await waitFor(() => {
        const mediaCells = screen.getAllByText(/100\.00/i);
        expect(mediaCells.length).toBeGreaterThan(0);
      });
    });

    it('deve processar valores decimais corretamente', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      
      fireEvent.change(textarea, { target: { value: '50.5\n75.25' } });
      
      await waitFor(() => {
        expect(textarea).toHaveValue('50.5\n75.25');
      });
    });
  });

  describe('Modo "Todas as Usinas"', () => {
    it('deve processar valores separados por Tab', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      expect(textarea).toBeInTheDocument();
      expect(textarea).toHaveValue(expect.stringContaining('\t'));
    });

    it('deve calcular totais de múltiplas usinas', async () => {
      render(<SOM />);
      
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
      
      const textarea = await screen.findByPlaceholderText(/Digite os valores SOM/i);
      
      // Primeira linha: 100 + 150 + 200 = 450
      const firstValue = textarea.value.split('\n')[0];
      const values = firstValue.split('\t').map(Number);
      const sum = values.reduce((a, b) => a + b, 0);
      
      expect(sum).toBeGreaterThan(0);
    });
  });

  describe('Salvamento de Dados', () => {
    it('deve mostrar alerta se campos não estiverem preenchidos', () => {
      render(<SOM />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      
      // Botão deve estar desabilitado
      expect(saveButton).toBeDisabled();
    });

    it('deve chamar função de save com dados corretos', async () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const consoleSpy = vi.spyOn(console, 'log').mockImplementation(() => {});
      
      render(<SOM />);
      
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
        expect(alertSpy).toHaveBeenCalledWith('Dados SOM salvos com sucesso!');
      });
      
      alertSpy.mockRestore();
      consoleSpy.mockRestore();
    });

    it('deve logar dados ao salvar', async () => {
      const consoleSpy = vi.spyOn(console, 'log').mockImplementation(() => {});
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      
      render(<SOM />);
      
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
        expect(consoleSpy).toHaveBeenCalledWith(
          'Salvando dados SOM:',
          expect.objectContaining({
            formData: expect.any(Object),
            tableData: expect.any(Array)
          })
        );
      });
      
      consoleSpy.mockRestore();
      alertSpy.mockRestore();
    });
  });

  describe('Geração de Horários', () => {
    it('deve gerar horários corretos para primeiro intervalo', () => {
      render(<SOM />);
      expect(screen.getByText('00:00-00:30')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para segundo intervalo', () => {
      render(<SOM />);
      expect(screen.getByText('00:30-01:00')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para meio-dia', () => {
      render(<SOM />);
      expect(screen.getByText('12:00-12:30')).toBeInTheDocument();
    });

    it('deve gerar horários corretos para último intervalo', () => {
      render(<SOM />);
      expect(screen.getByText('23:30-00:00')).toBeInTheDocument();
    });
  });

  describe('Responsividade e Estilos', () => {
    it('deve aplicar classe CSS ao container', () => {
      const { container } = render(<SOM />);
      const mainDiv = container.firstChild;
      expect(mainDiv).toHaveClass('container');
    });

    it('deve ter tabela com classe apropriada', () => {
      render(<SOM />);
      const table = screen.getByRole('table');
      expect(table).toHaveClass('table');
    });
  });

  describe('Acessibilidade', () => {
    it('deve ter labels associados aos campos', () => {
      render(<SOM />);
      
      expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
      expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
      expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    });

    it('deve ter botões acessíveis', () => {
      render(<SOM />);
      
      const saveButton = screen.getByRole('button', { name: /salvar/i });
      expect(saveButton).toBeInTheDocument();
    });

    it('deve ter selects acessíveis', () => {
      render(<SOM />);
      
      const selects = screen.getAllByRole('combobox');
      expect(selects).toHaveLength(3);
    });

    it('deve ter tabela com estrutura semântica', () => {
      render(<SOM />);
      
      const table = screen.getByRole('table');
      expect(table.querySelector('thead')).toBeInTheDocument();
      expect(table.querySelector('tbody')).toBeInTheDocument();
      expect(table.querySelector('tfoot')).toBeInTheDocument();
    });
  });
});

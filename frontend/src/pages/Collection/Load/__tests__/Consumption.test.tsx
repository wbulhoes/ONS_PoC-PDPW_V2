import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Consumption from '../Consumption';
import api from '../../../../services/api';

vi.mock('../../../../services/api');

const mockSubsistemas = [
  { id: 1, nome: 'Sudeste' },
  { id: 2, nome: 'Sul' },
  { id: 3, nome: 'Norte' },
];

const mockEmpresas = [
  { id: 1, nome: 'Empresa A' },
  { id: 2, nome: 'Empresa B' },
  { id: 3, nome: 'Empresa C' },
];

const mockConsumptionData = {
  previsto: 1000,
  realizado: 950,
  diferenca: -50,
};

describe('Consumption Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    (api.get as any).mockImplementation((url: string) => {
      if (url === '/subsistemas') {
        return Promise.resolve({ data: mockSubsistemas });
      }
      if (url === '/empresas') {
        return Promise.resolve({ data: mockEmpresas });
      }
      if (url === '/coleta/consumo') {
        return Promise.resolve({ data: mockConsumptionData });
      }
      return Promise.reject(new Error('Not found'));
    });
    (api.post as any).mockResolvedValue({ data: { success: true } });
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Consumption />);
      expect(screen.getByText('Coleta de Consumo')).toBeInTheDocument();
    });

    it('deve renderizar os campos de filtro', () => {
      render(<Consumption />);
      expect(screen.getByLabelText('Data PDP')).toBeInTheDocument();
      expect(screen.getByLabelText('Subsistema')).toBeInTheDocument();
      expect(screen.getByLabelText('Empresa')).toBeInTheDocument();
    });

    it('deve carregar subsistemas ao montar', async () => {
      render(<Consumption />);
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });
    });

    it('deve carregar empresas ao montar', async () => {
      render(<Consumption />);
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/empresas');
      });
    });
  });

  describe('Filtros', () => {
    it('deve permitir selecionar data', async () => {
      const user = userEvent.setup();
      render(<Consumption />);
      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');
      expect(dateInput).toHaveValue('2024-01-15');
    });

    it('deve permitir selecionar subsistema', async () => {
      const user = userEvent.setup();
      render(<Consumption />);
      
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      
      const option = await screen.findByText('Sudeste');
      await user.click(option);
      
      expect(subsistemaSelect).toHaveTextContent('Sudeste');
    });

    it('deve permitir selecionar empresa', async () => {
      const user = userEvent.setup();
      render(<Consumption />);
      
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/empresas');
      });

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      
      const option = await screen.findByText('Empresa A');
      await user.click(option);
      
      expect(empresaSelect).toHaveTextContent('Empresa A');
    });

    it('deve carregar dados quando todos os filtros estiverem preenchidos', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/coleta/consumo', {
          params: {
            dataPdp: '2024-01-15',
            subsistema: 'Sudeste',
            empresa: 'Empresa A',
          },
        });
      });
    });
  });

  describe('Exibição de Dados', () => {
    it('não deve exibir dados sem filtros preenchidos', () => {
      render(<Consumption />);
      expect(screen.queryByText('Dados de Consumo')).not.toBeInTheDocument();
    });

    it('deve exibir campos de consumo quando filtros estiverem preenchidos', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Dados de Consumo')).toBeInTheDocument();
        expect(screen.getByText('Consumo Previsto (MWh)')).toBeInTheDocument();
        expect(screen.getByText('Consumo Realizado (MWh)')).toBeInTheDocument();
        expect(screen.getByText('Diferença (MWh)')).toBeInTheDocument();
      });
    });

    it('deve exibir botões de ação', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Recarregar')).toBeInTheDocument();
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
    });

    it('deve carregar e exibir dados de consumo', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        const inputs = screen.getAllByRole('spinbutton');
        expect(inputs[0]).toHaveValue(1000);
        expect(inputs[1]).toHaveValue(950);
      });
    });
  });

  describe('Edição de Valores', () => {
    it('deve permitir editar consumo previsto', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Dados de Consumo')).toBeInTheDocument();
      });

      const inputs = screen.getAllByRole('spinbutton');
      await user.clear(inputs[0]);
      await user.type(inputs[0], '1200');

      expect(inputs[0]).toHaveValue(1200);
    });

    it('deve permitir editar consumo realizado', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Dados de Consumo')).toBeInTheDocument();
      });

      const inputs = screen.getAllByRole('spinbutton');
      await user.clear(inputs[1]);
      await user.type(inputs[1], '1100');

      expect(inputs[1]).toHaveValue(1100);
    });
  });

  describe('Cálculo de Diferença', () => {
    it('deve exibir diferença negativa corretamente', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('-50.00')).toBeInTheDocument();
      });
    });

    it('deve exibir diferença positiva corretamente', async () => {
      (api.get as any).mockImplementation((url: string) => {
        if (url === '/subsistemas') {
          return Promise.resolve({ data: mockSubsistemas });
        }
        if (url === '/empresas') {
          return Promise.resolve({ data: mockEmpresas });
        }
        if (url === '/coleta/consumo') {
          return Promise.resolve({ data: { previsto: 1000, realizado: 1100, diferenca: 100 } });
        }
        return Promise.reject(new Error('Not found'));
      });

      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('100.00')).toBeInTheDocument();
      });
    });
  });

  describe('Operações CRUD', () => {
    it('deve salvar dados com sucesso', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });

      const saveButton = screen.getByText('Salvar');
      await user.click(saveButton);

      await waitFor(() => {
        expect(api.post).toHaveBeenCalledWith('/coleta/consumo', expect.objectContaining({
          dataPdp: '2024-01-15',
          subsistema: 'Sudeste',
          empresa: 'Empresa A',
        }));
      });

      await waitFor(() => {
        expect(screen.getByText('Dados salvos com sucesso')).toBeInTheDocument();
      });
    });

    it('deve exibir erro ao falhar ao salvar', async () => {
      (api.post as any).mockRejectedValueOnce(new Error('Erro ao salvar'));
      
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });

      const saveButton = screen.getByText('Salvar');
      await user.click(saveButton);

      await waitFor(() => {
        expect(screen.getByText('Erro ao salvar dados')).toBeInTheDocument();
      });
    });

    it('deve recarregar dados ao clicar em Recarregar', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Recarregar')).toBeInTheDocument();
      });

      vi.clearAllMocks();

      const reloadButton = screen.getByText('Recarregar');
      await user.click(reloadButton);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/coleta/consumo', {
          params: {
            dataPdp: '2024-01-15',
            subsistema: 'Sudeste',
            empresa: 'Empresa A',
          },
        });
      });
    });
  });

  describe('Tratamento de Erros', () => {
    it('deve resetar dados quando falhar ao carregar', async () => {
      (api.get as any).mockImplementation((url: string) => {
        if (url === '/subsistemas') {
          return Promise.resolve({ data: mockSubsistemas });
        }
        if (url === '/empresas') {
          return Promise.resolve({ data: mockEmpresas });
        }
        if (url === '/coleta/consumo') {
          return Promise.reject(new Error('Erro ao carregar'));
        }
        return Promise.reject(new Error('Not found'));
      });

      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        const inputs = screen.getAllByRole('spinbutton');
        expect(inputs[0]).toHaveValue(0);
        expect(inputs[1]).toHaveValue(0);
      });
    });
  });

  describe('Validações', () => {
    it('deve aceitar valores numéricos válidos', async () => {
      const user = userEvent.setup();
      render(<Consumption />);

      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });

      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');

      const subsistemaSelect = screen.getByLabelText('Subsistema');
      await user.click(subsistemaSelect);
      const subsistemaOption = await screen.findByText('Sudeste');
      await user.click(subsistemaOption);

      const empresaSelect = screen.getByLabelText('Empresa');
      await user.click(empresaSelect);
      const empresaOption = await screen.findByText('Empresa A');
      await user.click(empresaOption);

      await waitFor(() => {
        expect(screen.getByText('Dados de Consumo')).toBeInTheDocument();
      });

      const inputs = screen.getAllByRole('spinbutton');
      await user.clear(inputs[0]);
      await user.type(inputs[0], '1500.50');

      expect(inputs[0]).toHaveValue(1500.5);
    });
  });
});

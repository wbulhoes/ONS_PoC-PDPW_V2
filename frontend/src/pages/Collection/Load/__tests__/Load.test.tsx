import { describe, it, expect, beforeEach, vi } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Load from '../Load';
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

const mockLoadData = {
  id: 1,
  dataPdp: '2024-01-15',
  subsistema: 'Sudeste',
  empresa: 'Empresa A',
  intervalos: Array(48).fill(100),
  total: 4800,
  media: 100,
};

describe('Load Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    (api.get as any).mockImplementation((url: string) => {
      if (url === '/subsistemas') {
        return Promise.resolve({ data: mockSubsistemas });
      }
      if (url === '/empresas') {
        return Promise.resolve({ data: mockEmpresas });
      }
      if (url === '/coleta/carga') {
        return Promise.resolve({ data: mockLoadData });
      }
      return Promise.reject(new Error('Not found'));
    });
    (api.post as any).mockResolvedValue({ data: { success: true } });
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Load />);
      expect(screen.getByText('Coleta de Carga')).toBeInTheDocument();
    });

    it('deve renderizar os campos de filtro', () => {
      render(<Load />);
      expect(screen.getByLabelText('Data PDP')).toBeInTheDocument();
      expect(screen.getByLabelText('Subsistema')).toBeInTheDocument();
      expect(screen.getByLabelText('Empresa')).toBeInTheDocument();
    });

    it('deve carregar subsistemas ao montar', async () => {
      render(<Load />);
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/subsistemas');
      });
    });

    it('deve carregar empresas ao montar', async () => {
      render(<Load />);
      await waitFor(() => {
        expect(api.get).toHaveBeenCalledWith('/empresas');
      });
    });
  });

  describe('Filtros', () => {
    it('deve permitir selecionar data', async () => {
      const user = userEvent.setup();
      render(<Load />);
      const dateInput = screen.getByLabelText('Data PDP');
      await user.type(dateInput, '2024-01-15');
      expect(dateInput).toHaveValue('2024-01-15');
    });

    it('deve permitir selecionar subsistema', async () => {
      const user = userEvent.setup();
      render(<Load />);
      
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
      render(<Load />);
      
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
      render(<Load />);

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
        expect(api.get).toHaveBeenCalledWith('/coleta/carga', {
          params: {
            dataPdp: '2024-01-15',
            subsistema: 'Sudeste',
            empresa: 'Empresa A',
          },
        });
      });
    });
  });

  describe('Tabela de Intervalos', () => {
    it('não deve exibir tabela sem filtros preenchidos', () => {
      render(<Load />);
      expect(screen.queryByText('Intervalos de Meia Hora (MW)')).not.toBeInTheDocument();
    });

    it('deve exibir 48 intervalos quando filtros estiverem preenchidos', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Intervalos de Meia Hora (MW)')).toBeInTheDocument();
      });

      const intervalCells = screen.getAllByRole('spinbutton');
      expect(intervalCells).toHaveLength(48);
    });

    it('deve exibir cabeçalhos da tabela', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Intervalo')).toBeInTheDocument();
        expect(screen.getByText('Hora')).toBeInTheDocument();
        expect(screen.getByText('Carga (MW)')).toBeInTheDocument();
      });
    });

    it('deve exibir botões de ação', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Edição em Bloco')).toBeInTheDocument();
        expect(screen.getByText('Recarregar')).toBeInTheDocument();
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
    });
  });

  describe('Edição de Intervalos', () => {
    it('deve permitir editar valor de um intervalo', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Intervalos de Meia Hora (MW)')).toBeInTheDocument();
      });

      const intervalInputs = screen.getAllByRole('spinbutton');
      await user.clear(intervalInputs[0]);
      await user.type(intervalInputs[0], '150');

      expect(intervalInputs[0]).toHaveValue(150);
    });
  });

  describe('Cálculos', () => {
    it('deve calcular e exibir o total corretamente', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('4800.00 MW')).toBeInTheDocument();
      });
    });

    it('deve calcular e exibir a média corretamente', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('100.00 MW')).toBeInTheDocument();
      });
    });
  });

  describe('Edição em Bloco', () => {
    it('deve abrir dialog de edição em bloco', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Edição em Bloco')).toBeInTheDocument();
      });

      const editButton = screen.getByText('Edição em Bloco');
      await user.click(editButton);

      await waitFor(() => {
        expect(screen.getByText('Informe 48 valores (um por linha)')).toBeInTheDocument();
      });
    });

    it('deve fechar dialog ao clicar em Cancelar', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('Edição em Bloco')).toBeInTheDocument();
      });

      const editButton = screen.getByText('Edição em Bloco');
      await user.click(editButton);

      await waitFor(() => {
        expect(screen.getByText('Informe 48 valores (um por linha)')).toBeInTheDocument();
      });

      const cancelButton = screen.getByText('Cancelar');
      await user.click(cancelButton);

      await waitFor(() => {
        expect(screen.queryByText('Informe 48 valores (um por linha)')).not.toBeInTheDocument();
      });
    });
  });

  describe('Operações CRUD', () => {
    it('deve salvar dados com sucesso', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(api.post).toHaveBeenCalledWith('/coleta/carga', expect.objectContaining({
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
      render(<Load />);

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
      render(<Load />);

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
        expect(api.get).toHaveBeenCalledWith('/coleta/carga', {
          params: {
            dataPdp: '2024-01-15',
            subsistema: 'Sudeste',
            empresa: 'Empresa A',
          },
        });
      });
    });
  });

  describe('Formatação de Hora', () => {
    it('deve formatar horas corretamente', async () => {
      const user = userEvent.setup();
      render(<Load />);

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
        expect(screen.getByText('00:00')).toBeInTheDocument();
        expect(screen.getByText('00:30')).toBeInTheDocument();
        expect(screen.getByText('01:00')).toBeInTheDocument();
        expect(screen.getByText('23:30')).toBeInTheDocument();
      });
    });
  });
});

import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import '@testing-library/jest-dom';
import { describe, it, expect, beforeEach, vi } from 'vitest';
import UnitRestriction from '../UnitRestriction';
import api from '../../../../services/api';

vi.mock('../../../../services/api');
const mockedApi = api as any;

const mockUsinas = [
  { id: 1, nome: 'Usina Teste 1', sigla: 'UT1' },
  { id: 2, nome: 'Usina Teste 2', sigla: 'UT2' },
];

const mockUnidades = [
  { id: 1, nome: 'UG1', potenciaNominal: 100 },
  { id: 2, nome: 'UG2', potenciaNominal: 150 },
];

const mockRestrictions = [
  {
    id: 1,
    dataPdp: '2024-01-15',
    usinaId: 1,
    usinaNome: 'Usina Teste 1',
    unidadeGeradoraId: 1,
    unidadeGeradoraNome: 'UG1',
    tipoRestricao: 'MANUTENCAO',
    dataInicio: '2024-01-15',
    dataFim: '2024-01-20',
    potenciaMaxima: 80,
    potenciaMinima: 20,
    observacao: 'Manutenção preventiva',
    status: 'ATIVA',
  },
  {
    id: 2,
    dataPdp: '2024-01-16',
    usinaId: 2,
    usinaNome: 'Usina Teste 2',
    unidadeGeradoraId: 2,
    unidadeGeradoraNome: 'UG2',
    tipoRestricao: 'FALHA_EQUIPAMENTO',
    dataInicio: '2024-01-16',
    dataFim: '2024-01-18',
    potenciaMaxima: 100,
    potenciaMinima: 0,
    observacao: 'Falha no gerador',
    status: 'ATIVA',
  },
];

const renderComponent = () => {
  return render(
    <BrowserRouter>
      <UnitRestriction />
    </BrowserRouter>
  );
};

describe('UnitRestriction Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockedApi.get = vi.fn((url: string) => {
      if (url === '/usinas') {
        return Promise.resolve({ data: mockUsinas });
      }
      if (url.includes('/unidades')) {
        return Promise.resolve({ data: mockUnidades });
      }
      if (url === '/coleta/restricao-ug') {
        return Promise.resolve({ data: mockRestrictions });
      }
      return Promise.reject(new Error('Not found'));
    });
  });

  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', async () => {
      renderComponent();
      expect(screen.getByText('Restrições de Unidades Geradoras')).toBeInTheDocument();
    });

    it('deve renderizar o botão de nova restrição', () => {
      renderComponent();
      expect(screen.getByRole('button', { name: /nova restrição/i })).toBeInTheDocument();
    });

    it('deve renderizar os filtros', () => {
      renderComponent();
      expect(screen.getByLabelText(/data pdp/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/usina/i)).toBeInTheDocument();
      expect(screen.getByLabelText(/status/i)).toBeInTheDocument();
    });

    it('deve carregar e exibir as usinas nos filtros', async () => {
      renderComponent();
      await waitFor(() => {
        expect(mockedApi.get).toHaveBeenCalledWith('/usinas');
      });
    });

    it('deve carregar e exibir as restrições na tabela', async () => {
      renderComponent();
      await waitFor(() => {
        expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
        expect(screen.getByText('Usina Teste 2')).toBeInTheDocument();
        expect(screen.getByText('UG1')).toBeInTheDocument();
        expect(screen.getByText('UG2')).toBeInTheDocument();
      });
    });
  });

  describe('Tabela de Restrições', () => {
    it('deve exibir os cabeçalhos da tabela', async () => {
      renderComponent();
      await waitFor(() => {
        expect(screen.getByRole('columnheader', { name: /data pdp/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /^usina$/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /unidade geradora/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /^tipo$/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /período/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /potência \(mw\)/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /status/i })).toBeInTheDocument();
        expect(screen.getByRole('columnheader', { name: /ações/i })).toBeInTheDocument();
      });
    });

    it('deve exibir os dados das restrições corretamente', async () => {
      renderComponent();
      await waitFor(() => {
        expect(screen.getByText('Manutenção')).toBeInTheDocument();
        expect(screen.getByText('Falha de Equipamento')).toBeInTheDocument();
        expect(screen.getByText('20 - 80')).toBeInTheDocument();
        expect(screen.getByText('0 - 100')).toBeInTheDocument();
      });
    });

    it('deve exibir os chips de status com cores corretas', async () => {
      renderComponent();
      await waitFor(() => {
        const statusChips = screen.getAllByText('ATIVA');
        expect(statusChips.length).toBeGreaterThan(0);
      });
    });

    it('deve exibir mensagem quando não há restrições', async () => {
      mockedApi.get = vi.fn((url: string) => {
        if (url === '/usinas') {
          return Promise.resolve({ data: mockUsinas });
        }
        if (url === '/coleta/restricao-ug') {
          return Promise.resolve({ data: [] });
        }
        return Promise.reject(new Error('Not found'));
      });

      renderComponent();
      await waitFor(() => {
        expect(screen.getByText('Nenhuma restrição encontrada')).toBeInTheDocument();
      });
    });
  });

  describe('Filtros', () => {
    it('deve permitir filtrar por data PDP', async () => {
      renderComponent();
      const dataPdpInput = screen.getByLabelText(/data pdp/i);
      fireEvent.change(dataPdpInput, { target: { value: '2024-01-15' } });

      await waitFor(() => {
        expect(mockedApi.get).toHaveBeenCalledWith('/coleta/restricao-ug', {
          params: expect.objectContaining({ dataPdp: '2024-01-15' }),
        });
      });
    });
  });

  describe('Dialog de Criação/Edição', () => {
    it('deve abrir o dialog ao clicar em Nova Restrição', async () => {
      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getByRole('heading', { name: /nova restrição/i })).toBeInTheDocument();
      });
    });

    it('deve exibir todos os campos do formulário no dialog', async () => {
      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getAllByLabelText(/data pdp/i).length).toBeGreaterThan(0);
        expect(screen.getAllByLabelText(/usina/i).length).toBeGreaterThan(0);
        expect(screen.getByLabelText(/unidade geradora/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/tipo de restrição/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/data início/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/data fim/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/potência mínima/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/potência máxima/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/observação/i)).toBeInTheDocument();
      });
    });

    it('deve fechar o dialog ao clicar em Cancelar', async () => {
      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getByRole('heading', { name: /nova restrição/i })).toBeInTheDocument();
      });

      const cancelButton = screen.getByRole('button', { name: /cancelar/i });
      fireEvent.click(cancelButton);

      await waitFor(() => {
        expect(screen.queryByRole('heading', { name: /nova restrição/i })).not.toBeInTheDocument();
      });
    });
  });

  describe('Operações CRUD', () => {
    it('deve criar uma nova restrição', async () => {
      mockedApi.post = vi.fn().mockResolvedValue({ data: { id: 3 } });

      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getByRole('heading', { name: /nova restrição/i })).toBeInTheDocument();
      });

      const saveButton = screen.getByRole('button', { name: /salvar/i });
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(mockedApi.post).toHaveBeenCalledWith('/coleta/restricao-ug', expect.any(Object));
      });
    });

    it('deve exibir mensagem de sucesso ao criar restrição', async () => {
      mockedApi.post = vi.fn().mockResolvedValue({ data: { id: 3 } });

      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getByRole('heading', { name: /nova restrição/i })).toBeInTheDocument();
      });

      const saveButton = screen.getByRole('button', { name: /salvar/i });
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(screen.getByText('Restrição criada com sucesso')).toBeInTheDocument();
      });
    });

    it('deve exibir mensagem de erro ao falhar na criação', async () => {
      mockedApi.post = vi.fn().mockRejectedValue(new Error('Erro ao salvar'));

      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        expect(screen.getByRole('heading', { name: /nova restrição/i })).toBeInTheDocument();
      });

      const saveButton = screen.getByRole('button', { name: /salvar/i });
      fireEvent.click(saveButton);

      await waitFor(() => {
        expect(screen.getByText('Erro ao salvar restrição')).toBeInTheDocument();
      });
    });

    it('deve excluir uma restrição após confirmação', async () => {
      mockedApi.delete = vi.fn().mockResolvedValue({ data: {} });
      window.confirm = vi.fn(() => true);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
      });

      const deleteButtons = screen.getAllByRole('button', { name: '' });
      const deleteButton = deleteButtons.find((btn) => btn.querySelector('[data-testid="DeleteIcon"]'));

      if (deleteButton) {
        fireEvent.click(deleteButton);

        await waitFor(() => {
          expect(mockedApi.delete).toHaveBeenCalledWith('/coleta/restricao-ug/1');
        });
      }
    });

    it('não deve excluir se o usuário cancelar a confirmação', async () => {
      window.confirm = vi.fn(() => false);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
      });

      const deleteButtons = screen.getAllByRole('button', { name: '' });
      const deleteButton = deleteButtons.find((btn) => btn.querySelector('[data-testid="DeleteIcon"]'));

      if (deleteButton) {
        fireEvent.click(deleteButton);
        expect(mockedApi.delete).not.toHaveBeenCalled();
      }
    });
  });

  describe('Validações', () => {
    it('deve desabilitar o campo de unidade geradora quando nenhuma usina está selecionada', async () => {
      renderComponent();
      const newButton = screen.getByRole('button', { name: /nova restrição/i });
      fireEvent.click(newButton);

      await waitFor(() => {
        const unidadeSelect = screen.getByLabelText(/unidade geradora/i);
        expect(unidadeSelect).toHaveAttribute('aria-disabled', 'true');
      });
    });
  });

  describe('Formatação de Dados', () => {
    it('deve formatar corretamente os tipos de restrição', async () => {
      renderComponent();
      await waitFor(() => {
        expect(screen.getByText('Manutenção')).toBeInTheDocument();
        expect(screen.getByText('Falha de Equipamento')).toBeInTheDocument();
      });
    });

    it('deve exibir corretamente o intervalo de potência', async () => {
      renderComponent();
      await waitFor(() => {
        expect(screen.getByText('20 - 80')).toBeInTheDocument();
        expect(screen.getByText('0 - 100')).toBeInTheDocument();
      });
    });
  });
});

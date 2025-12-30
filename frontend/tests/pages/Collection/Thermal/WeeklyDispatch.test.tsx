import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import WeeklyDispatch from '../../../../src/pages/Collection/Thermal/WeeklyDispatch';
import { weeklyDispatchService } from '../../../../src/services/weeklyDispatchService';

// Mock do service
vi.mock('../../../../src/services/weeklyDispatchService', () => ({
  weeklyDispatchService: {
    getData: vi.fn(),
    saveData: vi.fn(),
  },
}));

const mockData = {
  pmoConsulta: {
    semana: 1,
    tipo: 'PMO',
    dataInicio: '2023-10-01T00:00:00',
    dataFim: '2023-10-07T23:59:59',
  },
  pmoEdicao: {
    semana: 2,
    tipo: 'Revisão 1',
    dataInicio: '2023-10-08T00:00:00',
    dataFim: '2023-10-14T23:59:59',
    dataLimiteEnvio: '2023-10-06T12:00:00',
  },
  usinas: [
    {
      codUsina: 'USINA1',
      nomeUsina: 'Usina Teste 1',
      potenciaInstalada: 100,
      cvu: 50.5,
      tempoUgeLigada: 4,
      tempoUgeDesligada: 2,
      geracaoMinima: 10,
      rampaSubidaQuente: 1,
      rampaSubidaMorno: 1,
      rampaSubidaFrio: 1,
      rampaDescida: 1,
    },
  ],
};

describe('WeeklyDispatch Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o título corretamente', () => {
    render(<WeeklyDispatch />);
    expect(screen.getByText('Oferta Semanal Desp. Complementar')).toBeInTheDocument();
  });

  it('deve carregar dados ao selecionar uma empresa', async () => {
    vi.mocked(weeklyDispatchService.getData).mockResolvedValue(mockData);

    render(<WeeklyDispatch />);

    // Seleciona empresa
    const select = screen.getByLabelText('Empresa:');
    fireEvent.change(select, { target: { value: 'EMP001' } });

    // Verifica loading
    expect(screen.getByText('Carregando...')).toBeInTheDocument();

    // Aguarda carregamento
    await waitFor(() => {
      expect(screen.queryByText('Carregando...')).not.toBeInTheDocument();
    });

    // Verifica se dados foram renderizados
    expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
    expect(screen.getByDisplayValue('50.5')).toBeInTheDocument();
  });

  it('deve permitir edição de valores quando em modo Edição', async () => {
    vi.mocked(weeklyDispatchService.getData).mockResolvedValue(mockData);

    render(<WeeklyDispatch />);

    // Seleciona empresa
    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
    });

    // Garante que está em modo edição (default)
    const radioEdicao = screen.getByLabelText(/Revisão 1/i);
    expect(radioEdicao).toBeChecked();

    // Edita CVU
    const inputCVU = screen.getByDisplayValue('50.5');
    fireEvent.change(inputCVU, { target: { value: '60.0' } });

    expect(inputCVU).toHaveValue(60.0);
  });

  it('não deve permitir edição de valores quando em modo Consulta', async () => {
    vi.mocked(weeklyDispatchService.getData).mockResolvedValue(mockData);

    render(<WeeklyDispatch />);

    // Seleciona empresa
    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(screen.getByText('Usina Teste 1')).toBeInTheDocument();
    });

    // Muda para modo Consulta
    const radioConsulta = screen.getByLabelText(/PMO/i); // PMO é o tipo da consulta no mock
    fireEvent.click(radioConsulta);

    // Verifica se input está desabilitado
    const inputCVU = screen.getByDisplayValue('50.5');
    expect(inputCVU).toBeDisabled();
  });

  it('deve salvar os dados corretamente', async () => {
    vi.mocked(weeklyDispatchService.getData).mockResolvedValue(mockData);
    vi.mocked(weeklyDispatchService.saveData).mockResolvedValue();

    render(<WeeklyDispatch />);

    // Seleciona empresa e carrega dados
    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });
    await waitFor(() => screen.getByText('Usina Teste 1'));

    // Clica em Salvar
    const saveButton = screen.getByText('Salvar');
    fireEvent.click(saveButton);

    // Verifica chamada do service
    await waitFor(() => {
      expect(weeklyDispatchService.saveData).toHaveBeenCalledWith(
        mockData.usinas,
        mockData.pmoEdicao,
        'EMP001'
      );
      expect(screen.getByText('Dados salvos com sucesso!')).toBeInTheDocument();
    });
  });

  it('deve exibir erro se falhar ao carregar dados', async () => {
    vi.mocked(weeklyDispatchService.getData).mockRejectedValue(new Error('Erro API'));

    render(<WeeklyDispatch />);

    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(screen.getByText('Erro ao carregar dados.')).toBeInTheDocument();
    });
  });
});

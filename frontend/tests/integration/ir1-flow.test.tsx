/**
 * T064: Integration test for IR1 flow (Nível de Partida)
 * Covers load, selection cascade, create, update, validation, error handling, and field reset.
 */

import { describe, it, expect, beforeAll, afterAll, afterEach, vi } from 'vitest';
import { render, screen, waitFor, fireEvent } from '@testing-library/react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import IR1 from '../../src/pages/Collection/Insumos/IR1';
import { server, mockEndpoint, mockErrorEndpoint } from '../setup/mswServer';

const FIXED_DATE = '2025-01-01';
const SECOND_DATE = '2025-01-02';

const COMPANY = { id: '1', codigo: 'EMP001', nome: 'Empresa 1', tipo: 'GERADORA', ativo: true };
const PLANT = {
  id: '10',
  codigo: 'UHE001',
  nome: 'Usina 1',
  empresaId: '1',
  tipoUsina: 'HIDROELETRICA',
  subsistema: 'SUDESTE',
  potenciaInstalada: 100,
  ativo: true,
};

const EXISTING_RESPONSE = {
  Id: 1,
  DataReferencia: `${FIXED_DATE}T00:00:00Z`,
  NiveisPartida: [
    { UsinaId: '10', UsinaNome: 'Usina 1', Nivel: 100.5, Volume: 0 },
  ],
};

describe('Integration: IR1 flow - Nível de Partida', () => {
  beforeAll(() => {
    vi.useFakeTimers({ shouldAdvanceTime: true });
    vi.setSystemTime(new Date(`${FIXED_DATE}T00:00:00Z`));
    server.listen();
  });

  afterEach(() => {
    server.resetHandlers();
    vi.clearAllMocks();
  });

  afterAll(() => {
    server.close();
    vi.useRealTimers();
  });

  const createClient = () =>
    new QueryClient({
      defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
    });

  const renderWithClient = (ui: React.ReactElement) => {
    const client = createClient();
    return render(<QueryClientProvider client={client}>{ui}</QueryClientProvider>);
  };

  const setupBaseEndpoints = () => {
    mockEndpoint('get', '/empresas', [COMPANY]);
    mockEndpoint('get', `/usinas/empresa/${COMPANY.id}`, [PLANT]);
  };

  const setupExistingDataEndpoints = () => {
    setupBaseEndpoints();
    mockEndpoint('get', `/insumos-recebimento/ir1/${FIXED_DATE}`, EXISTING_RESPONSE);
  };

  const setupCreateEndpoints = () => {
    setupBaseEndpoints();
    mockEndpoint('get', `/insumos-recebimento/ir1/${SECOND_DATE}`, {
      Id: null,
      DataReferencia: `${SECOND_DATE}T00:00:00Z`,
      NiveisPartida: [],
    });
    mockEndpoint('post', '/insumos-recebimento/ir1', {
      Id: 2,
      DataReferencia: `${SECOND_DATE}T00:00:00Z`,
      NiveisPartida: [{ UsinaId: '10', UsinaNome: 'Usina 1', Nivel: 105.0, Volume: 0 }],
    });
  };

  it('should render core form fields', () => {
    renderWithClient(<IR1 />);

    expect(screen.getByText(/Nível de Partida/i)).toBeInTheDocument();
    expect(screen.getByText(/Data PDP:/i)).toBeInTheDocument();
    expect(screen.getByText(/Empresa:/i)).toBeInTheDocument();
    expect(screen.getByText(/Usinas:/i)).toBeInTheDocument();
    expect(screen.getByText(/Valor:/i)).toBeInTheDocument();
  });

  it('should keep empresa/usina disabled before selecting date', () => {
    renderWithClient(<IR1 />);
    const selects = screen.getAllByRole('combobox');
    expect(selects[1]).toBeDisabled();
    expect(selects[2]).toBeDisabled();
  });

  it('should enable empresa after selecting date', async () => {
    setupBaseEndpoints();
    renderWithClient(<IR1 />);

    const selects = screen.getAllByRole('combobox');
    const dateSelect = selects[0] as HTMLSelectElement;

    fireEvent.change(dateSelect, { target: { value: FIXED_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });
  });

  it('should load existing IR1 data and show value', async () => {
    setupExistingDataEndpoints();
    renderWithClient(<IR1 />);
    const dateSelect = screen.getAllByRole('combobox')[0];
    fireEvent.change(dateSelect, { target: { value: FIXED_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    await waitFor(() => {
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
      expect(valorInput.value).toBe('100.5');
    });
  });

  it('should create new IR1 record when no data exists', async () => {
    setupCreateEndpoints();
    renderWithClient(<IR1 />);
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
    fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: SECOND_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
    fireEvent.input(valorInput, { target: { value: '105.0' } });

    fireEvent.click(screen.getByRole('button', { name: /salvar/i }));

    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalledWith('Nível de Partida salvo com sucesso!');
    });

    alertSpy.mockRestore();
  });

  it('should update existing IR1 record', async () => {
    setupExistingDataEndpoints();
    mockEndpoint('put', '/insumos-recebimento/ir1/1', {
      Id: 1,
      DataReferencia: `${FIXED_DATE}T00:00:00Z`,
      NiveisPartida: [{ UsinaId: '10', UsinaNome: 'Usina 1', Nivel: 110.5, Volume: 0 }],
    });

    renderWithClient(<IR1 />);
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
    fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: FIXED_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
    await waitFor(() => {
      expect(valorInput.value).toBe('100.5');
    });

    fireEvent.input(valorInput, { target: { value: '110.5' } });

    fireEvent.click(screen.getByRole('button', { name: /salvar/i }));

    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalledWith('Nível de Partida salvo com sucesso!');
    });

    alertSpy.mockRestore();
  });

  it('should require value before saving', async () => {
    setupCreateEndpoints();
    renderWithClient(<IR1 />);
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
    fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: SECOND_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    fireEvent.click(screen.getByRole('button', { name: /salvar/i }));

    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalledWith('Por favor, preencha todos os campos');
    });

    alertSpy.mockRestore();
  });

  it('should show error when fetch fails', async () => {
    setupBaseEndpoints();
    mockErrorEndpoint('get', `/insumos-recebimento/ir1/${FIXED_DATE}`, 500, 'Server error');

    renderWithClient(<IR1 />);
    fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: FIXED_DATE } });

    await waitFor(() => {
      expect(screen.getByText(/Não foi possível carregar os dados de IR1/i)).toBeInTheDocument();
    });
  });

  it('should show alert when save fails', async () => {
    setupBaseEndpoints();
    mockEndpoint('get', `/insumos-recebimento/ir1/${SECOND_DATE}`, {
      Id: null,
      DataReferencia: `${SECOND_DATE}T00:00:00Z`,
      NiveisPartida: [],
    });
    mockErrorEndpoint('post', '/insumos-recebimento/ir1', 500, 'Failed to save');

    renderWithClient(<IR1 />);
    const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
    const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
    fireEvent.change(screen.getAllByRole('combobox')[0], { target: { value: SECOND_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;
    fireEvent.input(valorInput, { target: { value: '105.0' } });

    fireEvent.click(screen.getByRole('button', { name: /salvar/i }));

    await waitFor(() => {
      expect(alertSpy).toHaveBeenCalledWith('Erro ao salvar Nível de Partida');
    });

    alertSpy.mockRestore();
    consoleSpy.mockRestore();
  });

  it('should reset dependent fields when date changes', async () => {
    setupExistingDataEndpoints();
    mockEndpoint('get', `/insumos-recebimento/ir1/${SECOND_DATE}`, {
      Id: null,
      DataReferencia: `${SECOND_DATE}T00:00:00Z`,
      NiveisPartida: [],
    });

    renderWithClient(<IR1 />);
    const dateSelect = screen.getAllByRole('combobox')[0] as HTMLSelectElement;

    fireEvent.change(dateSelect, { target: { value: FIXED_DATE } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[1] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[1], { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect((screen.getAllByRole('combobox')[2] as HTMLSelectElement)).not.toBeDisabled();
    });

    fireEvent.change(screen.getAllByRole('combobox')[2], { target: { value: 'UHE001' } });

    fireEvent.change(dateSelect, { target: { value: SECOND_DATE } });

    await waitFor(() => {
      const empresaSelect = screen.getAllByRole('combobox')[1] as HTMLSelectElement;
      const usinaSelect = screen.getAllByRole('combobox')[2] as HTMLSelectElement;
      const valorInput = screen.getByPlaceholderText(/Nível de Partida/i) as HTMLInputElement;

      expect(empresaSelect.value).toBe('');
      expect(usinaSelect.value).toBe('');
      expect(valorInput.value).toBe('');
    });

    expect(screen.queryByRole('button', { name: /salvar/i })).not.toBeInTheDocument();
  });
});

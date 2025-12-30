import { describe, it, expect, beforeAll, afterEach, afterAll } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Energetic from '../../src/pages/Collection/Energetic/Energetic';
import { server, mockEndpoint } from '../setup/mswServer';

/**
 * T034: Integration test for energetic flow (load → edit/save trigger)
 */

describe('Integration: Energetic flow', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());

  const createClient = () =>
    new QueryClient({
      defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
    });

  const renderWithClient = (ui: React.ReactElement) => {
    const client = createClient();
    return render(<QueryClientProvider client={client}>{ui}</QueryClientProvider>);
  };

  const setupHappyPathHandlers = () => {
    mockEndpoint('get', '/empresas', [
      { id: '1', codigo: 'EMP001', nome: 'Empresa 1', tipo: 'GERADORA', ativo: true },
    ]);

    mockEndpoint('get', '/usinas/empresa/1', [
      {
        id: '10',
        codigo: 'UHE001',
        nome: 'Usina 1',
        empresaId: '1',
        tipoUsina: 'HIDROELETRICA',
        subsistema: 'SUDESTE',
        potenciaInstalada: 100,
        ativo: true,
      },
    ]);

    mockEndpoint(
      'get',
      '/dadosenergeticos/periodo?dataInicio=2025-01-01&dataFim=2025-01-01',
      [
        {
          Id: 1,
          UsinaId: 10,
          DataReferencia: '2025-01-01T00:00:00Z',
          Intervalo: 1,
          ValorMW: 10,
          RazaoEnergetica: 10,
        },
      ]
    );

    mockEndpoint('post', '/dadosenergeticos/bulk', [
      {
        Id: 999,
        UsinaId: 10,
        DataReferencia: '2025-01-01T00:00:00Z',
        Intervalo: 1,
        ValorMW: 10,
        RazaoEnergetica: 10,
      },
    ]);
  };

  it('should load energetic data and allow saving', async () => {
    setupHappyPathHandlers();

    renderWithClient(<Energetic />);

    const user = userEvent.setup();

    // Select date (first non-empty option)
    const dataSelect = screen.getByLabelText(/Data PDP:/i) as HTMLSelectElement;
    const dateOption = Array.from(dataSelect.options).find(opt => opt.value);
    expect(dateOption).toBeDefined();
    await user.selectOptions(dataSelect, dateOption!.value);

    // Select company
    await user.selectOptions(screen.getByLabelText(/Empresa:/i), 'EMP001');

    // Wait for table
    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    // Select plant to show textarea and save
    await user.selectOptions(screen.getByLabelText(/Usinas:/i), 'UHE001');

    await waitFor(() => {
      expect(screen.getByRole('button', { name: /salvar/i })).toBeInTheDocument();
    });

    await user.click(screen.getByRole('button', { name: /salvar/i }));

    // After save, textarea should close (no selected usina)
    await waitFor(() => {
      const select = screen.getByLabelText(/Usinas:/i) as HTMLSelectElement;
      expect(select.value).toBe('');
    });
  });
});

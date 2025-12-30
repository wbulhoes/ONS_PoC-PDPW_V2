/**
 * MSW Server Setup for Integration Tests
 * 
 * Provides Mock Service Worker (MSW) setup for API mocking
 * Used by all integration tests to intercept HTTP requests
 */

import { setupServer } from 'msw/node';
import { http, HttpResponse } from 'msw';

const API_BASE_URL = 'http://localhost:5001/api';

/**
 * Default mock handlers - can be extended per test
 */
const defaultHandlers = [
  // Health check endpoint
  http.get(`${API_BASE_URL}/health`, () => {
    return HttpResponse.json({ status: 'ok' });
  }),

  // Common metadata endpoints (domain-specific)
  http.get(`${API_BASE_URL}/empresas`, () => {
    return HttpResponse.json([
      { id: '1', codigo: 'EMP001', nome: 'Empresa 1', tipo: 'GERADORA', ativo: true },
      { id: '2', codigo: 'EMP002', nome: 'Empresa 2', tipo: 'GERADORA', ativo: true },
    ]);
  }),

  http.get(`${API_BASE_URL}/usinas/empresa/:empresaId`, ({ params }) => {
    const { empresaId } = params as { empresaId: string };
    // Return plants for the given empresa
    const all = [
      { id: '10', codigo: 'UHE001', nome: 'Usina 1', empresaId: '1', tipoUsina: 'HIDROELETRICA', subsistema: 'SUDESTE', potenciaInstalada: 100, ativo: true },
      { id: '20', codigo: 'UHE002', nome: 'Usina 2', empresaId: '2', tipoUsina: 'HIDROELETRICA', subsistema: 'SUL', potenciaInstalada: 200, ativo: true },
    ];
    return HttpResponse.json(all.filter(p => p.empresaId === empresaId));
  }),

  http.get(`${API_BASE_URL}/plant-types`, () => {
    return HttpResponse.json([
      { id: 1, name: 'Hydroelectric', code: 'HYDRO' },
      { id: 2, name: 'Thermal', code: 'THERMAL' },
    ]);
  }),

  // Energetic data endpoints
  http.get(`${API_BASE_URL}/dadosenergeticos`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        UsinaId: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        Intervalo: 1,
        ValorMW: 100,
        RazaoEnergetica: 50,
      },
    ]);
  }),

  // Match period endpoint via path and use query params
  http.get(`${API_BASE_URL}/dadosenergeticos/periodo`, ({ request }) => {
    const url = new URL(request.url);
    const dataInicio = url.searchParams.get('dataInicio');
    const dataFim = url.searchParams.get('dataFim');
    // Return a simple mocked record within period
    const date = (dataInicio || dataFim || '2025-01-01') + 'T00:00:00Z';
    return HttpResponse.json([
      {
        Id: 1,
        UsinaId: 10,
        DataReferencia: date,
        Intervalo: 1,
        ValorMW: 10,
        RazaoEnergetica: 10,
      },
    ]);
  }),

  http.post(`${API_BASE_URL}/dadosenergeticos`, async ({ request }) => {
    const data = await request.json();
    return HttpResponse.json(
      {
        Id: 999,
        ...data,
      },
      { status: 201 }
    );
  }),

  http.post(`${API_BASE_URL}/dadosenergeticos/bulk`, async ({ request }) => {
    const payload = await request.json();
    // Echo the payload back with created Id
    return HttpResponse.json(
      Array.isArray(payload)
        ? payload.map((item: any, idx: number) => ({ Id: 900 + idx, ...item }))
        : [{ Id: 999, ...payload }],
      { status: 201 }
    );
  }),

  // Electrical data endpoints
  http.get(`${API_BASE_URL}/dados-eletricos`, () => {
    return HttpResponse.json([
      {
        id: 1,
        usinaId: 1,
        dataReferencia: '2024-01-15T00:00:00Z',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      },
    ]);
  }),

  http.get(`${API_BASE_URL}/dados-eletricos/periodo`, ({ request }) => {
    const url = new URL(request.url);
    const dataInicio = url.searchParams.get('dataInicio');
    const dataFim = url.searchParams.get('dataFim');
    const date = (dataInicio || dataFim || '2024-01-01') + 'T00:00:00Z';
    return HttpResponse.json([
      {
        id: 1,
        usinaId: 1,
        dataReferencia: date,
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      },
    ]);
  }),

  http.get(`${API_BASE_URL}/dados-eletricos/usina/:usinaId/data/:dataReferencia`, ({ params }) => {
    const { usinaId, dataReferencia } = params as { usinaId: string; dataReferencia: string };
    return HttpResponse.json([
      {
        id: 1,
        usinaId: Number(usinaId),
        dataReferencia: `${dataReferencia}T00:00:00Z`,
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      },
    ]);
  }),

  http.post(`${API_BASE_URL}/dados-eletricos`, async ({ request }) => {
    const data = await request.json();
    return HttpResponse.json(
      {
        id: 999,
        ...data,
      },
      { status: 201 }
    );
  }),

  http.post(`${API_BASE_URL}/dados-eletricos/bulk`, async ({ request }) => {
    const payload = await request.json();
    return HttpResponse.json(
      Array.isArray(payload)
        ? payload.map((item: any, idx: number) => ({ id: 900 + idx, ...item }))
        : [{ id: 999, ...payload }],
      { status: 201 }
    );
  }),

  http.put(`${API_BASE_URL}/dados-eletricos/:id`, async ({ request, params }) => {
    const { id } = params as { id: string };
    const data = await request.json();
    return HttpResponse.json({
      id: Number(id),
      ...data,
    });
  }),

  http.delete(`${API_BASE_URL}/dados-eletricos/:id`, () => {
    return new HttpResponse(null, { status: 204 });
  }),

  // Electrical data endpoints (old format - keep for compatibility)
  http.get(`${API_BASE_URL}/dadoseletricos`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        UsinaId: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        Intervalo: 1,
        TensaoMV: 230,
        RazaoEletrica: 75,
      },
    ]);
  }),

  http.post(`${API_BASE_URL}/dadoseletricos`, async ({ request }) => {
    const data = await request.json();
    return HttpResponse.json(
      {
        Id: 999,
        ...data,
      },
      { status: 201 }
    );
  }),

  // IR endpoints
  http.get(`${API_BASE_URL}/insumos-recebimento/ir1`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        NiveisPartida: [
          { UsinaId: 1, UsinaNome: 'Itaipu', Nivel: 219.5, Volume: 28500.0 },
          { UsinaId: 2, UsinaNome: 'Tucuruí', Nivel: 72.3, Volume: 45000.0 },
        ],
        CriadoEm: '2024-01-15T10:00:00Z',
        AtualizadoEm: '2024-01-15T10:00:00Z',
        UsuarioCriacao: 'system',
      },
    ]);
  }),

  http.get(`${API_BASE_URL}/insumos-recebimento/ir1/:date`, ({ params }) => {
    const { date } = params as { date: string };
    return HttpResponse.json({
      Id: 1,
      DataReferencia: date.includes('T') ? date : `${date}T00:00:00Z`,
      NiveisPartida: [
        { UsinaId: 1, UsinaNome: 'Itaipu', Nivel: 219.5, Volume: 28500.0 },
        { UsinaId: 2, UsinaNome: 'Tucuruí', Nivel: 72.3, Volume: 45000.0 },
      ],
      CriadoEm: '2024-01-15T10:00:00Z',
      AtualizadoEm: '2024-01-15T10:00:00Z',
      UsuarioCriacao: 'system',
    });
  }),

  http.post(`${API_BASE_URL}/insumos-recebimento/ir1`, async ({ request }) => {
    const body = await request.json();
    return HttpResponse.json(
      {
        Id: 1,
        ...body,
        CriadoEm: new Date().toISOString(),
        AtualizadoEm: new Date().toISOString(),
        UsuarioCriacao: 'system',
      },
      { status: 201 }
    );
  }),

  http.post(`${API_BASE_URL}/insumos-recebimento/ir1/bulk`, async ({ request }) => {
    const body = await request.json();
    return HttpResponse.json(
      Array.isArray(body)
        ? body.map((item: any, idx: number) => ({
            Id: idx + 1,
            ...item,
            CriadoEm: new Date().toISOString(),
            AtualizadoEm: new Date().toISOString(),
            UsuarioCriacao: 'system',
          }))
        : body
    );
  }),

  http.put(`${API_BASE_URL}/insumos-recebimento/ir1/:id`, async ({ request }) => {
    const body = await request.json();
    return HttpResponse.json({
      Id: 1,
      ...body,
      AtualizadoEm: new Date().toISOString(),
    });
  }),

  http.delete(`${API_BASE_URL}/insumos-recebimento/ir1/:id`, () => {
    return HttpResponse.json(null, { status: 204 });
  }),

  http.get(`${API_BASE_URL}/insumos-recebimento/ir2`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        DiaMenos1: 45,
      },
    ]);
  }),

  http.get(`${API_BASE_URL}/insumos-recebimento/ir3`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        DiaMenos2: 40,
      },
    ]);
  }),

  http.get(`${API_BASE_URL}/insumos-recebimento/ir4`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        CargaAnde: 3500,
      },
    ]);
  }),

  // Export offer endpoints
  http.get(`${API_BASE_URL}/ofertas-exportacao`, () => {
    return HttpResponse.json([
      {
        Id: 1,
        UsinaId: 1,
        DataReferencia: '2024-01-15T00:00:00Z',
        Preco: 150,
        Quantidade: 100,
      },
    ]);
  }),

  http.post(`${API_BASE_URL}/ofertas-exportacao`, async ({ request }) => {
    const data = await request.json();
    return HttpResponse.json(
      {
        Id: 999,
        ...data,
      },
      { status: 201 }
    );
  }),
];

/**
 * Create MSW server with default handlers
 */
export const server = setupServer(...defaultHandlers);

/**
 * Setup MSW to intercept requests before tests run
 */
export function setupMSW() {
  beforeAll(() => {
    server.listen();
  });

  afterEach(() => {
    server.resetHandlers();
  });

  afterAll(() => {
    server.close();
  });
}

/**
 * Helper to add custom handlers for specific tests
 */
export function mockEndpoint(
  method: 'get' | 'post' | 'put' | 'delete' | 'patch',
  path: string,
  response: unknown,
  options: { status?: number } = {}
) {
  const handler =
    method === 'get'
      ? http.get(`${API_BASE_URL}${path}`, () =>
          HttpResponse.json(response, { status: options.status || 200 })
        )
      : method === 'post'
        ? http.post(`${API_BASE_URL}${path}`, () =>
            HttpResponse.json(response, { status: options.status || 201 })
          )
        : method === 'put'
          ? http.put(`${API_BASE_URL}${path}`, () =>
              HttpResponse.json(response, { status: options.status || 200 })
            )
          : method === 'delete'
            ? http.delete(`${API_BASE_URL}${path}`, () =>
                HttpResponse.json(response, { status: options.status || 204 })
              )
            : http.patch(`${API_BASE_URL}${path}`, () =>
                HttpResponse.json(response, { status: options.status || 200 })
              );

  server.use(handler);
}

/**
 * Helper to mock error responses
 */
export function mockErrorEndpoint(
  method: 'get' | 'post' | 'put' | 'delete' | 'patch',
  path: string,
  statusCode: number = 500,
  errorMessage: string = 'Server error'
) {
  const handler =
    method === 'get'
      ? http.get(`${API_BASE_URL}${path}`, () =>
          HttpResponse.json({ message: errorMessage }, { status: statusCode })
        )
      : method === 'post'
        ? http.post(`${API_BASE_URL}${path}`, () =>
            HttpResponse.json({ message: errorMessage }, { status: statusCode })
          )
        : method === 'put'
          ? http.put(`${API_BASE_URL}${path}`, () =>
              HttpResponse.json({ message: errorMessage }, { status: statusCode })
            )
          : method === 'delete'
            ? http.delete(`${API_BASE_URL}${path}`, () =>
                HttpResponse.json({ message: errorMessage }, { status: statusCode })
              )
            : http.patch(`${API_BASE_URL}${path}`, () =>
                HttpResponse.json({ message: errorMessage }, { status: statusCode })
              );

  server.use(handler);
}

/**
 * Helper to mock network error (connection failure)
 */
export function mockNetworkError(method: 'get' | 'post' | 'put' | 'delete' | 'patch', path: string) {
  const handler =
    method === 'get'
      ? http.get(`${API_BASE_URL}${path}`, () => {
          throw new Error('Network error');
        })
      : method === 'post'
        ? http.post(`${API_BASE_URL}${path}`, () => {
            throw new Error('Network error');
          })
        : method === 'put'
          ? http.put(`${API_BASE_URL}${path}`, () => {
              throw new Error('Network error');
            })
          : method === 'delete'
            ? http.delete(`${API_BASE_URL}${path}`, () => {
                throw new Error('Network error');
              })
            : http.patch(`${API_BASE_URL}${path}`, () => {
                throw new Error('Network error');
              });

  server.use(handler);
}

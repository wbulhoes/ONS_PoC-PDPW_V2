/**
 * IR1 Service Tests
 * T055-T057: Comprehensive tests for IR1 service layer
 * - T055: Success scenarios
 * - T056: Error scenarios (400/404/500)
 * - T057: Network errors
 */

import { describe, it, expect, beforeEach } from 'vitest';
import { server } from '../setup/mswServer';
import { http, HttpResponse } from 'msw';
import { ir1Service } from '../../src/services/ir1Service';
import type { IR1Dto } from '../../src/types/ir1';

const API_BASE_URL = 'http://localhost:5001/api';

const mockIR1Dto: IR1Dto = {
  dataReferencia: '2024-01-15',
  niveisPartida: [
    {
      usinaId: 1,
      usinaNome: 'Itaipu',
      nivel: 219.5,
      volume: 28500.0,
    },
  ],
};

describe('ir1Service', () => {
  describe('getByDate - success scenarios (T055)', () => {
    it('should fetch IR1 data by date', async () => {
      const result = await ir1Service.getByDate('2024-01-15');
      expect(result).toBeDefined();
      expect(result.dataReferencia).toBeInstanceOf(Date);
      expect(result.dataReferencia.toISOString()).toMatch(/2024-01-15/);
      expect(Array.isArray(result.niveisPartida)).toBe(true);
    });

    it('should handle YYYYMMDD format and convert to ISO', async () => {
      const result = await ir1Service.getByDate('20240115');
      expect(result).toBeDefined();
      expect(result.niveisPartida).toBeDefined();
    });

    it('should return nível de partida items with required fields', async () => {
      const result = await ir1Service.getByDate('2024-01-15');
      if (result.niveisPartida && result.niveisPartida.length > 0) {
        const item = result.niveisPartida[0];
        expect(item).toHaveProperty('usinaId');
        expect(item).toHaveProperty('nivel');
        expect(item).toHaveProperty('volume');
      }
    });
  });

  describe('getAll - success scenarios (T055)', () => {
    it('should fetch all IR1 data', async () => {
      const result = await ir1Service.getAll();
      expect(Array.isArray(result)).toBe(true);
      expect(result.length).toBeGreaterThan(0);
    });

    it('should return properly transformed data', async () => {
      const result = await ir1Service.getAll();
      expect(result[0]).toHaveProperty('dataReferencia');
      expect(result[0]).toHaveProperty('niveisPartida');
    });
  });

  describe('create - success scenarios (T055)', () => {
    it('should create new IR1 data', async () => {
      const result = await ir1Service.create(mockIR1Dto);
      expect(result).toBeDefined();
      expect(result.niveisPartida).toBeDefined();
    });

    it('should return created data with ID', async () => {
      const result = await ir1Service.create(mockIR1Dto);
      expect(result.id).toBeDefined();
    });
  });

  describe('update - success scenarios (T055)', () => {
    it('should update existing IR1 data', async () => {
      const updateDto = {
        niveisPartida: [
          {
            usinaId: 1,
            usinaNome: 'Itaipu Updated',
            nivel: 220.0,
            volume: 29000.0,
          },
        ],
      };
      const result = await ir1Service.update(1, updateDto);
      expect(result).toBeDefined();
    });

    it('should support partial updates', async () => {
      const result = await ir1Service.update(1, { dataReferencia: '2024-01-16' });
      expect(result).toBeDefined();
    });
  });

  describe('delete - success scenarios (T055)', () => {
    it('should delete IR1 data', async () => {
      const result = await ir1Service.delete(1);
      expect(result).toBeUndefined();
    });
  });

  describe('bulkUpsert - success scenarios (T055)', () => {
    it('should bulk upsert IR1 data', async () => {
      const dados = [mockIR1Dto];
      const result = await ir1Service.bulkUpsert(dados);
      expect(Array.isArray(result)).toBe(true);
      expect(result.length).toBeGreaterThan(0);
    });

    it('should handle multiple records', async () => {
      const dados = [
        mockIR1Dto,
        {
          dataReferencia: '2024-01-16',
          niveisPartida: [
            {
              usinaId: 3,
              usinaNome: 'Sobradinho',
              nivel: 380.0,
              volume: 34000.0,
            },
          ],
        },
      ];
      const result = await ir1Service.bulkUpsert(dados);
      expect(Array.isArray(result)).toBe(true);
      expect(result.length).toBeGreaterThanOrEqual(dados.length);
    });
  });

  describe('Error scenarios - 400 Bad Request (T056)', () => {
    beforeEach(() => {
      server.use(
        http.post(`${API_BASE_URL}/insumos-recebimento/ir1`, () => {
          return HttpResponse.json(
            {
              message: 'Dados inválidos',
              code: 'VALIDATION_ERROR',
              errors: {
                dataReferencia: ['Data deve estar no formato ISO 8601'],
                niveisPartida: ['Lista não pode estar vazia'],
              },
            },
            { status: 400 }
          );
        })
      );
    });

    it('should handle validation error on create', async () => {
      try {
        await ir1Service.create({
          dataReferencia: 'invalid-date',
          niveisPartida: [],
        });
        expect.fail('Should have thrown an error');
      } catch (error: any) {
        expect(error.status).toBe(400);
      }
    });
  });

  describe('Error scenarios - 404 Not Found (T056)', () => {
    beforeEach(() => {
      server.use(
        http.get(`${API_BASE_URL}/insumos-recebimento/ir1/2025-12-25`, () => {
          return HttpResponse.json(
            {
              message: 'Dados não encontrados',
              code: 'NOT_FOUND',
            },
            { status: 404 }
          );
        }),
        http.put(`${API_BASE_URL}/insumos-recebimento/ir1/999`, () => {
          return HttpResponse.json(
            { message: 'Registro não encontrado', code: 'NOT_FOUND' },
            { status: 404 }
          );
        }),
        http.delete(`${API_BASE_URL}/insumos-recebimento/ir1/999`, () => {
          return HttpResponse.json(
            { message: 'Registro não encontrado', code: 'NOT_FOUND' },
            { status: 404 }
          );
        })
      );
    });

    it('should handle not found on getByDate', async () => {
      try {
        await ir1Service.getByDate('2025-12-25');
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error.status).toBe(404);
      }
    });

    it('should handle not found on update', async () => {
      try {
        await ir1Service.update(999, { dataReferencia: '2024-01-20' });
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error.status).toBe(404);
      }
    });

    it('should handle not found on delete', async () => {
      try {
        await ir1Service.delete(999);
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error.status).toBe(404);
      }
    });
  });

  describe('Error scenarios - 500 Server Error (T056)', () => {
    beforeEach(() => {
      server.use(
        http.get(`${API_BASE_URL}/insumos-recebimento/ir1`, () => {
          return HttpResponse.json(
            { message: 'Erro interno', code: 'SERVER_ERROR' },
            { status: 500 }
          );
        }),
        http.post(`${API_BASE_URL}/insumos-recebimento/ir1`, () => {
          return HttpResponse.json(
            { message: 'Erro ao processar', code: 'SERVER_ERROR' },
            { status: 500 }
          );
        })
      );
    });

    it('should handle server error on getAll', async () => {
      try {
        await ir1Service.getAll();
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error.status).toBe(500);
      }
    });

    it('should handle server error on create', async () => {
      try {
        await ir1Service.create(mockIR1Dto);
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error.status).toBe(500);
      }
    });
  });

  describe('Network error scenarios (T057)', () => {
    beforeEach(() => {
      server.use(
        http.get(`${API_BASE_URL}/insumos-recebimento/ir1/:date`, () => {
          return HttpResponse.error();
        }),
        http.post(`${API_BASE_URL}/insumos-recebimento/ir1`, () => {
          return HttpResponse.error();
        })
      );
    });

    it('should handle network error on getByDate', async () => {
      try {
        await ir1Service.getByDate('2024-01-15');
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error).toBeDefined();
      }
    });

    it('should handle network error on create', async () => {
      try {
        await ir1Service.create(mockIR1Dto);
        expect.fail('Should have thrown error');
      } catch (error: any) {
        expect(error).toBeDefined();
      }
    });
  });
});

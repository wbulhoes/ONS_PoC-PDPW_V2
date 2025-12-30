import { describe, it, expect, beforeEach } from 'vitest';
import {
  electricalService,
  CreateDadoEletricoDto,
  UpdateDadoEletricoDto,
} from '../../src/services/electricalService';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

/**
 * T040-T042: Service tests for electrical data (success, errors 400/404/500, network errors)
 */
describe('electricalService', () => {
  beforeEach(() => {
    server.resetHandlers();
  });

  describe('getAll - success scenarios (T040)', () => {
    it('should fetch all electrical data', async () => {
      mockEndpoint('get', '/dados-eletricos', [
        {
          id: 1,
          usinaId: 100,
          usinaNome: 'Usina A',
          dataReferencia: '2024-01-15T00:00:00Z',
          intervalo: 1,
          potenciaMW: 100,
          razaoEletrica: 50,
        },
      ]);

      const result = await electricalService.getAll();

      expect(result).toBeDefined();
      expect(Array.isArray(result)).toBe(true);
      expect(result.length).toBeGreaterThan(0);
    });

    it('should return empty array if no data', async () => {
      mockEndpoint('get', '/dados-eletricos', []);

      const result = await electricalService.getAll();

      expect(Array.isArray(result)).toBe(true);
      expect(result).toHaveLength(0);
    });

    it('should transform camelCase keys from API response', async () => {
      mockEndpoint('get', '/dados-eletricos', [
        {
          id: 1,
          usinaId: 100,
          usinaNome: 'Usina A',
          dataReferencia: '2024-01-15T00:00:00Z',
          intervalo: 1,
          potenciaMW: 100,
          razaoEletrica: 50,
          fatorPotencia: 0.95,
          observacao: 'Test',
        },
      ]);

      const result = await electricalService.getAll();

      expect(result[0].id).toBe(1);
      expect(result[0].usinaId).toBe(100);
      expect(result[0].usinaNome).toBe('Usina A');
      expect(result[0].intervalo).toBe(1);
      expect(result[0].potenciaMW).toBe(100);
      expect(result[0].razaoEletrica).toBe(50);
    });

    it('should include optional fields if provided', async () => {
      mockEndpoint('get', '/dados-eletricos', [
        {
          id: 1,
          usinaId: 100,
          dataReferencia: '2024-01-15T00:00:00Z',
          intervalo: 1,
          potenciaMW: 100,
          razaoEletrica: 50,
          fatorPotencia: 0.95,
          observacao: 'Test observation',
        },
      ]);

      const result = await electricalService.getAll();

      expect(result[0].fatorPotencia).toBe(0.95);
      expect(result[0].observacao).toBe('Test observation');
    });
  });

  describe('getById - success scenarios (T040)', () => {
    it('should fetch electrical data by ID', async () => {
      mockEndpoint('get', '/dados-eletricos/1', {
        id: 1,
        usinaId: 100,
        dataReferencia: '2024-01-15T00:00:00Z',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      });

      const result = await electricalService.getById(1);

      expect(result.id).toBe(1);
      expect(result.usinaId).toBe(100);
      expect(result.potenciaMW).toBe(100);
    });
  });

  describe('getByPeriod - success scenarios (T040)', () => {
    it('should fetch electrical data by period', async () => {
      mockEndpoint(
        'get',
        '/dados-eletricos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31',
        [
          {
            id: 1,
            usinaId: 100,
            dataReferencia: '2024-01-15T00:00:00Z',
            intervalo: 1,
            potenciaMW: 100,
            razaoEletrica: 50,
          },
          {
            id: 2,
            usinaId: 100,
            dataReferencia: '2024-01-15T00:00:00Z',
            intervalo: 2,
            potenciaMW: 105,
            razaoEletrica: 51,
          },
        ]
      );

      const result = await electricalService.getByPeriod('2024-01-01', '2024-01-31');

      expect(Array.isArray(result)).toBe(true);
      expect(result.length).toBe(2);
    });
  });

  describe('getByUsinaAndDate - success scenarios (T040)', () => {
    it('should fetch electrical data for usina and date', async () => {
      mockEndpoint('get', '/dados-eletricos/usina/100/data/2024-01-15', [
        {
          id: 1,
          usinaId: 100,
          dataReferencia: '2024-01-15T00:00:00Z',
          intervalo: 1,
          potenciaMW: 100,
          razaoEletrica: 50,
        },
        {
          id: 2,
          usinaId: 100,
          dataReferencia: '2024-01-15T00:00:00Z',
          intervalo: 2,
          potenciaMW: 105,
          razaoEletrica: 51,
        },
      ]);

      const result = await electricalService.getByUsinaAndDate(100, '2024-01-15');

      expect(Array.isArray(result)).toBe(true);
      expect(result[0].intervalo).toBe(1);
      expect(result[1].intervalo).toBe(2);
    });
  });

  describe('create - success scenarios (T040)', () => {
    it('should create new electrical data', async () => {
      const dto: CreateDadoEletricoDto = {
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      };

      mockEndpoint('post', '/dados-eletricos', { id: 1, ...dto }, { status: 201 });

      const result = await electricalService.create(dto);

      expect(result.id).toBe(1);
      expect(result.usinaId).toBe(100);
      expect(result.potenciaMW).toBe(100);
    });

    it('should include optional fields if provided', async () => {
      const dto: CreateDadoEletricoDto = {
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
        fatorPotencia: 0.95,
        observacao: 'Test observation',
      };

      mockEndpoint('post', '/dados-eletricos', { id: 1, ...dto }, { status: 201 });

      const result = await electricalService.create(dto);

      expect(result.fatorPotencia).toBe(0.95);
      expect(result.observacao).toBe('Test observation');
    });
  });

  describe('update - success scenarios (T040)', () => {
    it('should update electrical data', async () => {
      const dto: UpdateDadoEletricoDto = {
        potenciaMW: 120,
        razaoEletrica: 60,
      };

      mockEndpoint('put', '/dados-eletricos/1', { id: 1, ...dto }, { status: 200 });

      const result = await electricalService.update(1, dto);

      expect(result.potenciaMW).toBe(120);
      expect(result.razaoEletrica).toBe(60);
    });

    it('should support partial updates', async () => {
      const dto: UpdateDadoEletricoDto = { potenciaMW: 120 };

      mockEndpoint('put', '/dados-eletricos/1', { id: 1, ...dto });

      const result = await electricalService.update(1, dto);

      expect(result.potenciaMW).toBe(120);
    });

    it('should support updating observacao', async () => {
      const dto: UpdateDadoEletricoDto = { observacao: 'Updated observation' };

      mockEndpoint('put', '/dados-eletricos/1', { id: 1, ...dto });

      const result = await electricalService.update(1, dto);

      expect(result.observacao).toBe('Updated observation');
    });
  });

  describe('delete - success scenarios (T040)', () => {
    it('should delete electrical data', async () => {
      mockEndpoint('delete', '/dados-eletricos/1', {}, { status: 204 });

      await expect(electricalService.delete(1)).resolves.not.toThrow();
    });
  });

  describe('bulkUpsert - success scenarios (T040)', () => {
    it('should bulk upsert electrical data (48 intervals)', async () => {
      const dados: CreateDadoEletricoDto[] = Array.from({ length: 48 }, (_, i) => ({
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: i + 1,
        potenciaMW: 100 + i,
        razaoEletrica: 50 + i,
      }));

      mockEndpoint('post', '/dados-eletricos/bulk', dados.map((d, i) => ({ id: i + 1, ...d })), {
        status: 201,
      });

      const result = await electricalService.bulkUpsert(dados);

      expect(result).toHaveLength(48);
      expect(result[0].intervalo).toBe(1);
      expect(result[47].intervalo).toBe(48);
    });
  });

  describe('Error scenarios - 400 Bad Request (T041)', () => {
    it('should handle validation error on create', async () => {
      mockErrorEndpoint(
        'post',
        '/dados-eletricos',
        400,
        'Invalid data: potenciaMW must be positive'
      );

      const dto: CreateDadoEletricoDto = {
        usinaId: -1,
        dataReferencia: 'invalid',
        intervalo: 0,
        potenciaMW: -100,
        razaoEletrica: -50,
      };

      await expect(electricalService.create(dto)).rejects.toThrow();
    });

    it('should handle validation error on update', async () => {
      mockErrorEndpoint('put', '/dados-eletricos/1', 400, 'potenciaMW must be positive');

      await expect(electricalService.update(1, { potenciaMW: -100 })).rejects.toThrow();
    });

    it('should handle validation error on bulkUpsert', async () => {
      mockErrorEndpoint('post', '/dados-eletricos/bulk', 400, 'Invalid bulk data');

      const dados: CreateDadoEletricoDto[] = [
        {
          usinaId: 100,
          dataReferencia: '2024-01-15',
          intervalo: 1,
          potenciaMW: -100,
          razaoEletrica: 50,
        },
      ];

      await expect(electricalService.bulkUpsert(dados)).rejects.toThrow();
    });
  });

  describe('Error scenarios - 404 Not Found (T041)', () => {
    it('should handle not found on getById', async () => {
      mockErrorEndpoint('get', '/dados-eletricos/999', 404, 'Data not found');

      await expect(electricalService.getById(999)).rejects.toThrow();
    });

    it('should handle not found on update', async () => {
      mockErrorEndpoint('put', '/dados-eletricos/999', 404, 'Data not found');

      await expect(
        electricalService.update(999, { potenciaMW: 120 })
      ).rejects.toThrow();
    });

    it('should handle not found on delete', async () => {
      mockErrorEndpoint('delete', '/dados-eletricos/999', 404, 'Data not found');

      await expect(electricalService.delete(999)).rejects.toThrow();
    });
  });

  describe('Error scenarios - 409 Conflict (T041)', () => {
    it('should handle conflict on create duplicate', async () => {
      mockErrorEndpoint(
        'post',
        '/dados-eletricos',
        409,
        'Data with same usina, date and interval already exists'
      );

      const dto: CreateDadoEletricoDto = {
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      };

      await expect(electricalService.create(dto)).rejects.toThrow();
    });
  });

  describe('Error scenarios - 500 Server Error (T041)', () => {
    it('should handle server error on getAll', async () => {
      mockErrorEndpoint('get', '/dados-eletricos', 500, 'Internal server error');

      await expect(electricalService.getAll()).rejects.toThrow();
    });

    it('should handle server error on create', async () => {
      mockErrorEndpoint('post', '/dados-eletricos', 500, 'Internal server error');

      const dto: CreateDadoEletricoDto = {
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      };

      await expect(electricalService.create(dto)).rejects.toThrow();
    });

    it('should handle server error on update', async () => {
      mockErrorEndpoint('put', '/dados-eletricos/1', 500, 'Internal server error');

      await expect(electricalService.update(1, { potenciaMW: 120 })).rejects.toThrow();
    });

    it('should handle server error on delete', async () => {
      mockErrorEndpoint('delete', '/dados-eletricos/1', 500, 'Internal server error');

      await expect(electricalService.delete(1)).rejects.toThrow();
    });

    it('should handle server error on bulkUpsert', async () => {
      mockErrorEndpoint('post', '/dados-eletricos/bulk', 500, 'Internal server error');

      const dados: CreateDadoEletricoDto[] = [
        {
          usinaId: 100,
          dataReferencia: '2024-01-15',
          intervalo: 1,
          potenciaMW: 100,
          razaoEletrica: 50,
        },
      ];

      await expect(electricalService.bulkUpsert(dados)).rejects.toThrow();
    });
  });

  describe('Network error scenarios (T042)', () => {
    // Network errors are tested via mockErrorEndpoint with 500 status
    // Real network failures would be caught by the error boundary above
    it('should handle service unavailable on getAll (simulated network error)', async () => {
      mockErrorEndpoint('get', '/dados-eletricos', 503, 'Service Unavailable');

      await expect(electricalService.getAll()).rejects.toThrow();
    });

    it('should handle gateway timeout on create (simulated network error)', async () => {
      mockErrorEndpoint('post', '/dados-eletricos', 504, 'Gateway Timeout');

      const dto: CreateDadoEletricoDto = {
        usinaId: 100,
        dataReferencia: '2024-01-15',
        intervalo: 1,
        potenciaMW: 100,
        razaoEletrica: 50,
      };

      await expect(electricalService.create(dto)).rejects.toThrow();
    });
  });
});

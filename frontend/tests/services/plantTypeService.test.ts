import { describe, it, expect, beforeEach } from 'vitest';
import { plantTypeService, CreatePlantTypeDto, UpdatePlantTypeDto } from '../../src/services/plantTypeService';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

describe('plantTypeService', () => {
  beforeEach(() => {
    server.resetHandlers();
  });

  describe('getAll', () => {
    it('should fetch all plant types', async () => {
      const result = await plantTypeService.getAll();

      expect(result).toBeDefined();
      expect(Array.isArray(result)).toBe(true);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('get', '/plant-types', 500, 'Server error');

      await expect(plantTypeService.getAll()).rejects.toThrow();
    });
  });

  describe('getById', () => {
    it('should fetch plant type by ID', async () => {
      const typeData = { id: 1, name: 'Hydroelectric', code: 'HYDRO' };
      mockEndpoint('get', '/plant-types/1', typeData);

      const result = await plantTypeService.getById(1);

      expect(result.id).toBe(1);
      expect(result.name).toBe('Hydroelectric');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('get', '/plant-types/999', 404, 'Plant type not found');

      await expect(plantTypeService.getById(999)).rejects.toThrow();
    });
  });

  describe('create', () => {
    it('should create new plant type', async () => {
      const dto: CreatePlantTypeDto = {
        name: 'Solar',
        code: 'SOLAR',
      };

      mockEndpoint('post', '/plant-types', { id: 2, ...dto }, { status: 201 });

      const result = await plantTypeService.create(dto);

      expect(result.id).toBe(2);
      expect(result.name).toBe('Solar');
    });

    it('should handle validation errors', async () => {
      mockErrorEndpoint('post', '/plant-types', 400, 'Validation failed');

      const dto: CreatePlantTypeDto = { name: '', code: '' };

      await expect(plantTypeService.create(dto)).rejects.toThrow();
    });
  });

  describe('update', () => {
    it('should update plant type', async () => {
      const dto: UpdatePlantTypeDto = { name: 'Updated Type' };

      mockEndpoint('put', '/plant-types/1', {
        id: 1,
        name: 'Updated Type',
        code: 'HYDRO',
      });

      const result = await plantTypeService.update(1, dto);

      expect(result.name).toBe('Updated Type');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('put', '/plant-types/999', 404, 'Plant type not found');

      await expect(plantTypeService.update(999, { name: 'Test' })).rejects.toThrow();
    });
  });

  describe('delete', () => {
    it('should delete plant type', async () => {
      mockEndpoint('delete', '/plant-types/1', {}, { status: 204 });

      await expect(plantTypeService.delete(1)).resolves.not.toThrow();
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('delete', '/plant-types/999', 404, 'Plant type not found');

      await expect(plantTypeService.delete(999)).rejects.toThrow();
    });
  });
});

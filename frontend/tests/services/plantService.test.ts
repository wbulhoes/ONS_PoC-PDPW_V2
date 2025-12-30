import { describe, it, expect, beforeEach } from 'vitest';
import { plantService, CreatePlantDto, UpdatePlantDto } from '../../src/services/plantService';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

describe('plantService', () => {
  beforeEach(() => {
    server.resetHandlers();
  });

  describe('getAll', () => {
    it('should fetch all plants', async () => {
      const result = await plantService.getAll();

      expect(result).toBeDefined();
      expect(Array.isArray(result)).toBe(true);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('get', '/plants', 500, 'Server error');

      await expect(plantService.getAll()).rejects.toThrow();
    });
  });

  describe('getByCompany', () => {
    it('should fetch plants by company ID', async () => {
      const plantData = [
        { id: 1, name: 'Plant 1', code: 'P1', type: 'HYDRO', companyId: 1 },
      ];
      mockEndpoint('get', '/plants?companyId=1', plantData);

      const result = await plantService.getByCompany(1);

      expect(Array.isArray(result)).toBe(true);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('get', '/plants?companyId=999', 404, 'Company not found');

      await expect(plantService.getByCompany(999)).rejects.toThrow();
    });
  });

  describe('getById', () => {
    it('should fetch plant by ID', async () => {
      const plantData = { id: 1, name: 'Plant 1', code: 'P1', type: 'HYDRO', companyId: 1 };
      mockEndpoint('get', '/plants/1', plantData);

      const result = await plantService.getById(1);

      expect(result.id).toBe(1);
      expect(result.name).toBe('Plant 1');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('get', '/plants/999', 404, 'Plant not found');

      await expect(plantService.getById(999)).rejects.toThrow();
    });
  });

  describe('create', () => {
    it('should create new plant', async () => {
      const dto: CreatePlantDto = {
        name: 'New Plant',
        code: 'NP',
        type: 'HYDRO',
        companyId: 1,
      };

      mockEndpoint('post', '/plants', { id: 2, ...dto }, { status: 201 });

      const result = await plantService.create(dto);

      expect(result.id).toBe(2);
      expect(result.name).toBe('New Plant');
    });

    it('should handle validation errors', async () => {
      mockErrorEndpoint('post', '/plants', 400, 'Validation failed');

      const dto: CreatePlantDto = { name: '', code: '', type: '', companyId: 0 };

      await expect(plantService.create(dto)).rejects.toThrow();
    });
  });

  describe('update', () => {
    it('should update plant', async () => {
      const dto: UpdatePlantDto = { name: 'Updated Plant' };

      mockEndpoint('put', '/plants/1', {
        id: 1,
        name: 'Updated Plant',
        code: 'P1',
        type: 'HYDRO',
        companyId: 1,
      });

      const result = await plantService.update(1, dto);

      expect(result.name).toBe('Updated Plant');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('put', '/plants/999', 404, 'Plant not found');

      await expect(plantService.update(999, { name: 'Test' })).rejects.toThrow();
    });
  });

  describe('delete', () => {
    it('should delete plant', async () => {
      mockEndpoint('delete', '/plants/1', {}, { status: 204 });

      await expect(plantService.delete(1)).resolves.not.toThrow();
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('delete', '/plants/999', 404, 'Plant not found');

      await expect(plantService.delete(999)).rejects.toThrow();
    });
  });
});

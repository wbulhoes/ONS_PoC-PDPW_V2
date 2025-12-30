import { describe, it, expect, beforeEach, vi } from 'vitest';
import { companyService, CreateCompanyDto, UpdateCompanyDto } from '../../src/services/companyService';
import { mockEndpoint, mockErrorEndpoint, server } from '../setup/mswServer';

describe('companyService', () => {
  beforeEach(() => {
    server.resetHandlers();
  });

  describe('getAll', () => {
    it('should fetch all companies', async () => {
      const result = await companyService.getAll();

      expect(result).toBeDefined();
      expect(Array.isArray(result)).toBe(true);
    });

    it('should handle errors', async () => {
      mockErrorEndpoint('get', '/companies', 500, 'Server error');

      await expect(companyService.getAll()).rejects.toThrow();
    });
  });

  describe('getById', () => {
    it('should fetch company by ID', async () => {
      mockEndpoint('get', '/companies/1', { id: 1, name: 'Company 1', code: 'C1' });

      const result = await companyService.getById(1);

      expect(result.id).toBe(1);
      expect(result.name).toBe('Company 1');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('get', '/companies/999', 404, 'Company not found');

      await expect(companyService.getById(999)).rejects.toThrow();
    });
  });

  describe('create', () => {
    it('should create new company', async () => {
      const dto: CreateCompanyDto = {
        name: 'New Company',
        code: 'NEW',
      };

      mockEndpoint('post', '/companies', { id: 2, ...dto }, { status: 201 });

      const result = await companyService.create(dto);

      expect(result.id).toBe(2);
      expect(result.name).toBe('New Company');
    });

    it('should handle validation errors', async () => {
      mockErrorEndpoint('post', '/companies', 400, 'Validation failed');

      const dto: CreateCompanyDto = { name: '', code: '' };

      await expect(companyService.create(dto)).rejects.toThrow();
    });
  });

  describe('update', () => {
    it('should update company', async () => {
      const dto: UpdateCompanyDto = { name: 'Updated Company' };

      mockEndpoint('put', '/companies/1', { id: 1, name: 'Updated Company', code: 'C1' });

      const result = await companyService.update(1, dto);

      expect(result.name).toBe('Updated Company');
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('put', '/companies/999', 404, 'Company not found');

      await expect(companyService.update(999, { name: 'Test' })).rejects.toThrow();
    });
  });

  describe('delete', () => {
    it('should delete company', async () => {
      mockEndpoint('delete', '/companies/1', {}, { status: 204 });

      await expect(companyService.delete(1)).resolves.not.toThrow();
    });

    it('should handle not found error', async () => {
      mockErrorEndpoint('delete', '/companies/999', 404, 'Company not found');

      await expect(companyService.delete(999)).rejects.toThrow();
    });
  });
});

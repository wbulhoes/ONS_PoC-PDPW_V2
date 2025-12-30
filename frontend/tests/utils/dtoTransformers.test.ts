import { describe, it, expect } from 'vitest';
import {
  transformFromApi,
  transformToApi,
  toCamelCase,
  toPascalCase,
  isDateString,
  transformEnergeticFromApi,
  transformEnergeticToApi,
  transformElectricalFromApi,
  transformElectricalToApi,
} from '../../src/utils/dtoTransformers';

describe('DTO Transformers', () => {
  describe('toCamelCase', () => {
    it('should convert PascalCase to camelCase', () => {
      expect(toCamelCase('DataReferencia')).toBe('dataReferencia');
      expect(toCamelCase('ValorMW')).toBe('valorMW');
      expect(toCamelCase('UssinaId')).toBe('ussinaId');
    });

    it('should handle already camelCase strings', () => {
      expect(toCamelCase('dataReferencia')).toBe('dataReferencia');
      expect(toCamelCase('valorMW')).toBe('valorMW');
    });
  });

  describe('toPascalCase', () => {
    it('should convert camelCase to PascalCase', () => {
      expect(toPascalCase('dataReferencia')).toBe('DataReferencia');
      expect(toPascalCase('valorMW')).toBe('ValorMW');
      expect(toPascalCase('usinaId')).toBe('UsinaId');
    });

    it('should handle already PascalCase strings', () => {
      expect(toPascalCase('DataReferencia')).toBe('DataReferencia');
      expect(toPascalCase('ValorMW')).toBe('ValorMW');
    });
  });

  describe('isDateString', () => {
    it('should identify ISO date strings', () => {
      expect(isDateString('2024-01-15')).toBe(true);
      expect(isDateString('2024-01-15T10:30:00')).toBe(true);
      expect(isDateString('2024-01-15T10:30:00Z')).toBe(true);
      expect(isDateString('2024-01-15T10:30:00.123Z')).toBe(true);
    });

    it('should reject invalid date strings', () => {
      expect(isDateString('01/15/2024')).toBe(false);
      expect(isDateString('15-01-2024')).toBe(false);
      expect(isDateString('not-a-date')).toBe(false);
      expect(isDateString('2024-13-01')).toBe(false); // Invalid month
    });

    it('should reject non-string inputs', () => {
      expect(isDateString(123 as any)).toBe(false);
      expect(isDateString(null as any)).toBe(false);
      expect(isDateString(undefined as any)).toBe(false);
      expect(isDateString({} as any)).toBe(false);
    });
  });

  describe('transformFromApi', () => {
    it('should transform PascalCase object to camelCase', () => {
      const apiData = {
        DataReferencia: '2024-01-15',
        ValorMW: 100,
        RazaoEnergetica: 50,
      };

      const result = transformFromApi(apiData);

      expect(result.dataReferencia).toBe('2024-01-15');
      expect(result.valorMW).toBe(100);
      expect(result.razaoEnergetica).toBe(50);
    });

    it('should convert date strings to Date objects', () => {
      const apiData = {
        DataReferencia: '2024-01-15T10:30:00Z',
        Id: 1,
      };

      const result: any = transformFromApi(apiData);

      expect(result.dataReferencia).toBeInstanceOf(Date);
      expect(result.dataReferencia.getFullYear()).toBe(2024);
      expect(result.id).toBe(1);
    });

    it('should handle nested objects', () => {
      const apiData = {
        Id: 1,
        Usina: {
          Id: 100,
          Nome: 'Usina A',
        },
        DataReferencia: '2024-01-15',
      };

      const result: any = transformFromApi(apiData);

      expect(result.id).toBe(1);
      expect(result.usina.id).toBe(100);
      expect(result.usina.nome).toBe('Usina A');
      expect(result.dataReferencia).toBeInstanceOf(Date);
    });

    it('should handle arrays', () => {
      const apiData = [
        { Id: 1, Valor: 100 },
        { Id: 2, Valor: 200 },
      ];

      const result: any[] = transformFromApi(apiData);

      expect(result).toHaveLength(2);
      expect(result[0].id).toBe(1);
      expect(result[0].valor).toBe(100);
      expect(result[1].id).toBe(2);
      expect(result[1].valor).toBe(200);
    });

    it('should handle arrays of objects with nested data', () => {
      const apiData = [
        {
          Id: 1,
          Usina: { Id: 10, Nome: 'A' },
          DataReferencia: '2024-01-15',
        },
      ];

      const result: any[] = transformFromApi(apiData);

      expect(result[0].id).toBe(1);
      expect(result[0].usina.nome).toBe('A');
      expect(result[0].dataReferencia).toBeInstanceOf(Date);
    });

    it('should handle null and undefined values', () => {
      const apiData = {
        Id: 1,
        Optional: null,
        Undefined: undefined,
      };

      const result: any = transformFromApi(apiData);

      expect(result.id).toBe(1);
      expect(result.optional).toBeNull();
      expect(result.undefined).toBeUndefined();
    });

    it('should handle null and undefined inputs', () => {
      expect(transformFromApi(null)).toBeNull();
      expect(transformFromApi(undefined)).toBeUndefined();
    });

    it('should preserve primitive types', () => {
      expect(transformFromApi(123)).toBe(123);
      expect(transformFromApi('string')).toBe('string');
      expect(transformFromApi(true)).toBe(true);
    });
  });

  describe('transformToApi', () => {
    it('should transform camelCase object to PascalCase', () => {
      const frontendData = {
        dataReferencia: '2024-01-15',
        valorMW: 100,
        razaoEnergetica: 50,
      };

      const result = transformToApi(frontendData);

      expect(result.DataReferencia).toBe('2024-01-15');
      expect(result.ValorMW).toBe(100);
      expect(result.RazaoEnergetica).toBe(50);
    });

    it('should convert Date objects to ISO strings', () => {
      const frontendData = {
        id: 1,
        dataReferencia: new Date('2024-01-15T10:30:00Z'),
      };

      const result: any = transformToApi(frontendData);

      expect(typeof result.DataReferencia).toBe('string');
      expect(result.DataReferencia).toContain('2024-01-15');
    });

    it('should handle nested objects', () => {
      const frontendData = {
        id: 1,
        usina: {
          id: 100,
          nome: 'Usina A',
        },
      };

      const result: any = transformToApi(frontendData);

      expect(result.Id).toBe(1);
      expect(result.Usina.Id).toBe(100);
      expect(result.Usina.Nome).toBe('Usina A');
    });

    it('should handle arrays', () => {
      const frontendData = [
        { id: 1, valor: 100 },
        { id: 2, valor: 200 },
      ];

      const result: any[] = transformToApi(frontendData);

      expect(result).toHaveLength(2);
      expect(result[0].Id).toBe(1);
      expect(result[0].Valor).toBe(100);
    });

    it('should handle null and undefined values', () => {
      const frontendData = {
        id: 1,
        optional: null,
        undefined: undefined,
      };

      const result: any = transformToApi(frontendData);

      expect(result.Id).toBe(1);
      expect(result.Optional).toBeNull();
      expect(result.Undefined).toBeUndefined();
    });
  });

  describe('Domain-specific transformers', () => {
    it('should transform energetic data from API', () => {
      const apiData = {
        Id: 1,
        DataReferencia: '2024-01-15',
        ValorMW: 100,
      };

      const result: any = transformEnergeticFromApi(apiData);

      expect(result.id).toBe(1);
      expect(result.dataReferencia).toBeInstanceOf(Date);
      expect(result.valorMW).toBe(100);
    });

    it('should transform energetic data to API', () => {
      const frontendData = {
        id: 1,
        dataReferencia: new Date('2024-01-15'),
        valorMW: 100,
      };

      const result: any = transformEnergeticToApi(frontendData);

      expect(result.Id).toBe(1);
      expect(typeof result.DataReferencia).toBe('string');
      expect(result.ValorMW).toBe(100);
    });

    it('should transform electrical data from API', () => {
      const apiData = {
        Id: 1,
        DataReferencia: '2024-01-15',
        TensaoMV: 230,
      };

      const result: any = transformElectricalFromApi(apiData);

      expect(result.id).toBe(1);
      expect(result.dataReferencia).toBeInstanceOf(Date);
      expect(result.tensaoMV).toBe(230);
    });

    it('should transform electrical data to API', () => {
      const frontendData = {
        id: 1,
        dataReferencia: new Date('2024-01-15'),
        tensaoMV: 230,
      };

      const result: any = transformElectricalToApi(frontendData);

      expect(result.Id).toBe(1);
      expect(typeof result.DataReferencia).toBe('string');
      expect(result.TensaoMV).toBe(230);
    });
  });
});

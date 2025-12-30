import { describe, it, expect } from 'vitest';
import {
  extractValidationErrors,
  getFieldError,
  getFieldErrors,
  hasFieldError,
  getErrorFields,
  hasValidationErrors,
} from '@/utils/extractValidationErrors';

describe('extractValidationErrors', () => {
  describe('extractValidationErrors()', () => {
    it('should extract flat object errors', () => {
      const data = { email: 'Invalid email', login: 'Required' };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
        login: ['Required'],
      });
    });

    it('should extract nested errors object', () => {
      const data = {
        errors: {
          email: 'Invalid email',
          login: 'Required',
        },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
        login: ['Required'],
      });
    });

    it('should extract validationErrors object', () => {
      const data = {
        validationErrors: {
          email: 'Invalid email',
        },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
      });
    });

    it('should extract details object', () => {
      const data = {
        details: {
          email: 'Invalid email',
        },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
      });
    });

    it('should handle array of errors per field', () => {
      const data = {
        email: ['Invalid email', 'Too long'],
        login: ['Required'],
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email', 'Too long'],
        login: ['Required'],
      });
    });

    it('should handle nested error objects', () => {
      const data = {
        email: { message: 'Invalid email' },
        login: { error: 'Required' },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
        login: ['Required'],
      });
    });

    it('should handle mixed error structures', () => {
      const data = {
        email: 'Invalid email',
        login: ['Required', 'Too short'],
        telefone: { message: 'Invalid phone' },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
        login: ['Required', 'Too short'],
        telefone: ['Invalid phone'],
      });
    });

    it('should skip empty values', () => {
      const data = {
        email: 'Invalid email',
        login: null,
        telefone: undefined,
        nome: '',
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email'],
      });
    });

    it('should handle null data', () => {
      const result = extractValidationErrors(null);
      expect(result).toEqual({});
    });

    it('should handle undefined data', () => {
      const result = extractValidationErrors(undefined);
      expect(result).toEqual({});
    });

    it('should handle non-object data', () => {
      expect(extractValidationErrors('error')).toEqual({});
      expect(extractValidationErrors(123)).toEqual({});
      expect(extractValidationErrors(true)).toEqual({});
    });

    it('should handle deeply nested error objects', () => {
      const data = {
        errors: {
          email: {
            message: 'Invalid email format',
          },
        },
      };
      const result = extractValidationErrors(data);

      expect(result).toEqual({
        email: ['Invalid email format'],
      });
    });

    it('should handle multiple messages in nested object', () => {
      const data = {
        email: {
          message: 'Invalid',
          error: 'Must be unique',
        },
      };
      const result = extractValidationErrors(data);

      expect(result.email).toContain('Invalid');
      expect(result.email).toContain('Must be unique');
    });
  });

  describe('getFieldError()', () => {
    it('should get first error for field', () => {
      const errors = {
        email: ['Invalid', 'Too long'],
        login: ['Required'],
      };

      expect(getFieldError(errors, 'email')).toBe('Invalid');
      expect(getFieldError(errors, 'login')).toBe('Required');
    });

    it('should return null for field without errors', () => {
      const errors = { email: ['Invalid'] };
      expect(getFieldError(errors, 'login')).toBeNull();
    });

    it('should return null for missing field', () => {
      const errors = {};
      expect(getFieldError(errors, 'email')).toBeNull();
    });
  });

  describe('getFieldErrors()', () => {
    it('should get all errors for field', () => {
      const errors = {
        email: ['Invalid', 'Too long'],
      };

      expect(getFieldErrors(errors, 'email')).toEqual(['Invalid', 'Too long']);
    });

    it('should return empty array for missing field', () => {
      const errors = {};
      expect(getFieldErrors(errors, 'email')).toEqual([]);
    });
  });

  describe('hasFieldError()', () => {
    it('should return true if field has errors', () => {
      const errors = { email: ['Invalid'] };
      expect(hasFieldError(errors, 'email')).toBe(true);
    });

    it('should return false if field has no errors', () => {
      const errors = { email: [] };
      expect(hasFieldError(errors, 'email')).toBe(false);
    });

    it('should return false if field missing', () => {
      const errors = {};
      expect(hasFieldError(errors, 'email')).toBe(false);
    });
  });

  describe('getErrorFields()', () => {
    it('should get all fields with errors', () => {
      const errors = {
        email: ['Invalid'],
        login: ['Required'],
        telefone: [],
      };

      const fields = getErrorFields(errors);
      expect(fields).toContain('email');
      expect(fields).toContain('login');
      expect(fields).not.toContain('telefone');
    });

    it('should return empty array if no errors', () => {
      expect(getErrorFields({})).toEqual([]);
    });
  });

  describe('hasValidationErrors()', () => {
    it('should return true if any field has errors', () => {
      const errors = { email: ['Invalid'] };
      expect(hasValidationErrors(errors)).toBe(true);
    });

    it('should return false if no errors', () => {
      expect(hasValidationErrors({})).toBe(false);
    });

    it('should return false if all fields have empty error arrays', () => {
      const errors = { email: [], login: [] };
      expect(hasValidationErrors(errors)).toBe(false);
    });
  });
});

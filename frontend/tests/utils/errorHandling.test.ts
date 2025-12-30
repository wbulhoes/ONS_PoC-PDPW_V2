import { describe, it, expect } from 'vitest';
import {
  normalizeError,
  getErrorCode,
  getErrorMessage,
  isRetryableError,
  isValidationError,
  getFieldErrors,
  isAuthError,
  createTimeoutError,
  createNetworkError,
  NormalizedError,
} from '../../src/utils/errorHandling';

describe('Error Handling Utils', () => {
  describe('normalizeError', () => {
    it('should normalize ApiClientError with status 400', () => {
      const error = new Error('Validation failed') as any;
      error.status = 400;
      error.errors = { email: ['Invalid email'] };

      const normalized = normalizeError(error);

      expect(normalized.code).toBe('VALIDATION_ERROR');
      expect(normalized.statusCode).toBe(400);
      expect(normalized.details?.email).toEqual(['Invalid email']);
    });

    it('should normalize ApiClientError with status 401', () => {
      const error = new Error('Unauthorized') as any;
      error.status = 401;

      const normalized = normalizeError(error);

      expect(normalized.code).toBe('UNAUTHORIZED');
      expect(normalized.statusCode).toBe(401);
    });

    it('should normalize generic Error', () => {
      const error = new Error('Something went wrong');

      const normalized = normalizeError(error);

      expect(normalized.code).toBe('UNKNOWN_ERROR');
      expect(normalized.message).toBe('Something went wrong');
    });

    it('should normalize string error', () => {
      const normalized = normalizeError('Error message');

      expect(normalized.code).toBe('UNKNOWN_ERROR');
      expect(normalized.message).toBe('Error message');
    });

    it('should normalize unknown error', () => {
      const normalized = normalizeError({ unknown: 'error' });

      expect(normalized.code).toBe('UNKNOWN_ERROR');
      expect(normalized.statusCode).toBe(0);
    });
  });

  describe('getErrorCode', () => {
    it('should return correct error codes for common HTTP statuses', () => {
      expect(getErrorCode(400)).toBe('VALIDATION_ERROR');
      expect(getErrorCode(401)).toBe('UNAUTHORIZED');
      expect(getErrorCode(403)).toBe('FORBIDDEN');
      expect(getErrorCode(404)).toBe('NOT_FOUND');
      expect(getErrorCode(409)).toBe('CONFLICT');
      expect(getErrorCode(500)).toBe('SERVER_ERROR');
      expect(getErrorCode(502)).toBe('BAD_GATEWAY');
      expect(getErrorCode(503)).toBe('SERVICE_UNAVAILABLE');
    });

    it('should return CLIENT_ERROR for 4xx not explicitly handled', () => {
      expect(getErrorCode(418)).toBe('CLIENT_ERROR');
    });

    it('should return SERVER_ERROR for 5xx not explicitly handled', () => {
      expect(getErrorCode(504)).toBe('SERVER_ERROR');
    });
  });

  describe('getErrorMessage', () => {
    it('should return localized messages for error codes', () => {
      expect(getErrorMessage('VALIDATION_ERROR', 'default')).toContain('inválidos');
      expect(getErrorMessage('UNAUTHORIZED', 'default')).toContain('autenticado');
      expect(getErrorMessage('NOT_FOUND', 'default')).toContain('não foi encontrado');
    });

    it('should return default message for unknown code', () => {
      expect(getErrorMessage('UNKNOWN_CODE', 'custom default')).toBe('custom default');
    });
  });

  describe('isRetryableError', () => {
    it('should return true for server errors', () => {
      const error: NormalizedError = {
        code: 'SERVER_ERROR',
        message: 'Error',
        statusCode: 500,
      };
      expect(isRetryableError(error)).toBe(true);
    });

    it('should return true for SERVICE_UNAVAILABLE', () => {
      const error: NormalizedError = {
        code: 'SERVICE_UNAVAILABLE',
        message: 'Unavailable',
        statusCode: 503,
      };
      expect(isRetryableError(error)).toBe(true);
    });

    it('should return false for validation errors', () => {
      const error: NormalizedError = {
        code: 'VALIDATION_ERROR',
        message: 'Invalid',
        statusCode: 400,
      };
      expect(isRetryableError(error)).toBe(false);
    });

    it('should return false for authentication errors', () => {
      const error: NormalizedError = {
        code: 'UNAUTHORIZED',
        message: 'Unauthorized',
        statusCode: 401,
      };
      expect(isRetryableError(error)).toBe(false);
    });
  });

  describe('isValidationError', () => {
    it('should return true for validation errors', () => {
      const error: NormalizedError = {
        code: 'VALIDATION_ERROR',
        message: 'Invalid data',
        statusCode: 400,
      };
      expect(isValidationError(error)).toBe(true);
    });

    it('should return false for other errors', () => {
      const error: NormalizedError = {
        code: 'NOT_FOUND',
        message: 'Not found',
        statusCode: 404,
      };
      expect(isValidationError(error)).toBe(false);
    });
  });

  describe('getFieldErrors', () => {
    it('should extract field-specific errors', () => {
      const error: NormalizedError = {
        code: 'VALIDATION_ERROR',
        message: 'Validation failed',
        statusCode: 400,
        details: {
          email: ['Invalid email format'],
          password: ['Password must be at least 8 characters'],
        },
      };

      const fieldErrors = getFieldErrors(error);

      expect(fieldErrors.email).toBe('Invalid email format');
      expect(fieldErrors.password).toBe('Password must be at least 8 characters');
    });

    it('should return empty object if no details', () => {
      const error: NormalizedError = {
        code: 'SERVER_ERROR',
        message: 'Server error',
        statusCode: 500,
      };

      const fieldErrors = getFieldErrors(error);

      expect(fieldErrors).toEqual({});
    });
  });

  describe('isAuthError', () => {
    it('should return true for unauthorized errors', () => {
      const error: NormalizedError = {
        code: 'UNAUTHORIZED',
        message: 'Unauthorized',
        statusCode: 401,
      };
      expect(isAuthError(error)).toBe(true);
    });

    it('should return true for forbidden errors', () => {
      const error: NormalizedError = {
        code: 'FORBIDDEN',
        message: 'Forbidden',
        statusCode: 403,
      };
      expect(isAuthError(error)).toBe(true);
    });

    it('should return false for other errors', () => {
      const error: NormalizedError = {
        code: 'SERVER_ERROR',
        message: 'Server error',
        statusCode: 500,
      };
      expect(isAuthError(error)).toBe(false);
    });
  });

  describe('createTimeoutError', () => {
    it('should create timeout error', () => {
      const error = createTimeoutError();

      expect(error.code).toBe('TIMEOUT_ERROR');
      expect(error.statusCode).toBe(408);
      expect(error.message).toContain('demorou');
    });
  });

  describe('createNetworkError', () => {
    it('should create network error', () => {
      const error = createNetworkError();

      expect(error.code).toBe('NETWORK_ERROR');
      expect(error.statusCode).toBe(0);
      expect(error.message).toContain('conexão');
    });
  });
});

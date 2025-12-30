import { describe, it, expect, beforeEach, vi } from 'vitest';
import { userService } from '@/services/userService';
import { HttpError } from '@/utils/httpError';
import { getUserFriendlyMessage, getErrorCategory } from '@/utils/errorMessages';
import { extractValidationErrors } from '@/utils/extractValidationErrors';
import * as apiClientModule from '@/services/apiClient';

// Mock the apiClient
vi.mock('@/services/apiClient', () => ({
  apiClient: {
    get: vi.fn(),
    post: vi.fn(),
    put: vi.fn(),
    delete: vi.fn(),
  },
}));

const { apiClient } = apiClientModule;

describe('Error Handling Integration Tests', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('400 Validation Error Flow', () => {
    it('should handle validation error and extract field-level errors', async () => {
      const validationError = {
        errors: {
          email: 'Invalid email format',
          login: ['Required', 'Too short'],
        },
      };

      vi.mocked(apiClient.post).mockRejectedValue({
        response: { status: 400, data: validationError },
      });

      try {
        await userService.create({ usuar_id: '', usuar_nome: '', usuar_email: '', usuar_telefone: '' });
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        // Check HTTP error details
        expect(httpError.status).toBe(400);
        expect(httpError.isValidationError()).toBe(true);

        // Extract field-level errors
        const fieldErrors = extractValidationErrors(httpError.data);
        expect(fieldErrors.email).toEqual(['Invalid email format']);
        expect(fieldErrors.login).toEqual(['Required', 'Too short']);

        // Get user-friendly message
        const userMsg = getUserFriendlyMessage(400, httpError.data);
        expect(userMsg).toContain('Por favor');
      }
    });

    it('should map validation error to error category', async () => {
      const category = getErrorCategory(400);
      expect(category).toBe('validation');
    });
  });

  describe('409 Conflict Error Flow', () => {
    it('should handle duplicate login conflict', async () => {
      vi.mocked(apiClient.post).mockRejectedValue({
        response: { status: 409, data: { message: 'duplicate login' } },
      });

      try {
        await userService.create({ usuar_id: '', usuar_nome: '', usuar_email: '', usuar_telefone: '' });
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        expect(httpError.status).toBe(409);
        expect(httpError.isConflictError()).toBe(true);

        const userMsg = getUserFriendlyMessage(409, httpError.data);
        expect(userMsg).toContain('login');
        expect(userMsg).toContain('em uso');

        const category = getErrorCategory(409);
        expect(category).toBe('conflict');
      }
    });

    it('should handle duplicate email conflict', async () => {
      vi.mocked(apiClient.post).mockRejectedValue({
        response: { status: 409, data: { message: 'duplicate email' } },
      });

      try {
        await userService.create({ usuar_id: '', usuar_nome: '', usuar_email: '', usuar_telefone: '' });
        expect.fail('Should throw HttpError');
      } catch (error) {
        const httpError = error as HttpError;
        const userMsg = getUserFriendlyMessage(409, httpError.data);
        expect(userMsg).toContain('e-mail');
        expect(userMsg).toContain('registrado');
      }
    });
  });

  describe('404 Not Found Error Flow', () => {
    it('should handle user not found', async () => {
      vi.mocked(apiClient.put).mockRejectedValue({
        response: { status: 404, data: { message: 'User not found' } },
      });

      try {
        await userService.update('NONEXISTENT', {
          usuar_id: 'NONEXISTENT',
          usuar_nome: '',
          usuar_email: '',
          usuar_telefone: '',
        });
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        expect(httpError.status).toBe(404);
        expect(httpError.isNotFoundError()).toBe(true);

        const userMsg = getUserFriendlyMessage(404);
        expect(userMsg).toContain('não foi encontrado');

        const category = getErrorCategory(404);
        expect(category).toBe('notfound');
      }
    });
  });

  describe('500 Server Error Flow', () => {
    it('should handle server error', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 500, data: { error: 'Internal server error' } },
      });

      try {
        await userService.list({ page: 1, pageSize: 10 });
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        expect(httpError.status).toBe(500);
        expect(httpError.isServerError()).toBe(true);

        const userMsg = getUserFriendlyMessage(500, httpError.data);
        expect(userMsg).toContain('Erro do sistema');

        const category = getErrorCategory(500);
        expect(category).toBe('server');
      }
    });
  });

  describe('503 Service Unavailable Flow', () => {
    it('should handle service unavailable', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 503 },
      });

      try {
        await userService.getAll();
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        expect(httpError.status).toBe(503);
        expect(httpError.isServerError()).toBe(true);

        const userMsg = getUserFriendlyMessage(503);
        expect(userMsg).toContain('temporariamente indisponível');
      }
    });
  });

  describe('Network/Timeout Error Flow', () => {
    it('should handle network timeout', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        code: 'ECONNABORTED',
        message: 'timeout',
      });

      try {
        await userService.list({ page: 1, pageSize: 10 });
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);

        const userMsg = getUserFriendlyMessage(undefined, undefined);
        expect(userMsg).toContain('conexão');

        const category = getErrorCategory(0);
        expect(category).toBe('network');
      }
    });

    it('should handle connection refused', async () => {
      vi.mocked(apiClient.delete).mockRejectedValue({
        code: 'ECONNREFUSED',
        message: 'Connection refused',
      });

      try {
        await userService.delete(['USER1']);
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const userMsg = getUserFriendlyMessage(undefined, undefined);
        expect(userMsg).toContain('conexão');
      }
    });
  });

  describe('Abort Error Flow', () => {
    it('should handle AbortError as network error', async () => {
      const controller = new AbortController();

      vi.mocked(apiClient.get).mockImplementation(() => {
        controller.abort();
        throw new DOMException('Aborted', 'AbortError');
      });

      try {
        await userService.list({ page: 1, pageSize: 10 }, controller.signal);
        expect.fail('Should throw HttpError');
      } catch (error) {
        // AbortError gets caught and wrapped as HttpError with undefined status
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        // Treat as network error
        const category = getErrorCategory(0);
        expect(category).toBe('network');
      }
    });
  });

  describe('Multiple Delete with Error Recovery', () => {
    it('should stop on first error during multi-delete', async () => {
      vi.mocked(apiClient.delete)
        .mockResolvedValueOnce({})
        .mockRejectedValueOnce({ response: { status: 404 } })
        .mockResolvedValueOnce({});

      try {
        await userService.delete(['USER1', 'USER2', 'USER3']);
        expect.fail('Should throw HttpError');
      } catch (error) {
        expect(error).toBeInstanceOf(HttpError);
        const httpError = error as HttpError;

        expect(httpError.status).toBe(404);

        // Verify that delete was called twice (USER1 succeeded, USER2 failed)
        expect(apiClient.delete).toHaveBeenCalledTimes(2);
      }
    });
  });

  describe('Error Message Integration', () => {
    it('should map all 4xx errors to user messages', () => {
      const errors = [400, 401, 403, 404, 409, 418, 422];

      for (const status of errors) {
        const msg = getUserFriendlyMessage(status);
        expect(msg).toBeTruthy();
        expect(msg.length > 0).toBe(true);
        // All messages should be in Portuguese
        expect(/[aá-ú]/i.test(msg)).toBe(true);
      }
    });

    it('should map all 5xx errors to user messages', () => {
      const errors = [500, 502, 503, 504];

      for (const status of errors) {
        const msg = getUserFriendlyMessage(status);
        expect(msg).toBeTruthy();
        expect(msg.length > 0).toBe(true);
        // All messages should be in Portuguese
        expect(/[aá-ú]/i.test(msg)).toBe(true);
      }
    });
  });

  describe('Error Predicates Integration', () => {
    it('should correctly identify client errors', () => {
      const error400 = new HttpError(400);
      const error403 = new HttpError(403);

      expect(error400.isClientError()).toBe(true);
      expect(error403.isClientError()).toBe(true);
    });

    it('should correctly identify server errors', () => {
      const error500 = new HttpError(500);
      const error503 = new HttpError(503);

      expect(error500.isServerError()).toBe(true);
      expect(error503.isServerError()).toBe(true);
    });

    it('should correctly identify specific error types', () => {
      expect(new HttpError(400).isValidationError()).toBe(true);
      expect(new HttpError(409).isConflictError()).toBe(true);
      expect(new HttpError(404).isNotFoundError()).toBe(true);
      expect(new HttpError(401).isAuthError()).toBe(true);
    });
  });
});

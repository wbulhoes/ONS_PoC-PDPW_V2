import { describe, it, expect } from 'vitest';
import { HttpError } from '@/utils/httpError';

describe('HttpError', () => {
  describe('Constructor', () => {
    it('should create HttpError with status and data', () => {
      const error = new HttpError(400, { message: 'Bad Request' });
      expect(error.status).toBe(400);
      expect(error.data).toEqual({ message: 'Bad Request' });
      expect(error.name).toBe('HttpError');
    });

    it('should create HttpError with custom message', () => {
      const error = new HttpError(400, { field: 'email' }, 'Invalid email format');
      expect(error.message).toBe('Invalid email format');
    });

    it('should create HttpError with default message', () => {
      const error = new HttpError(500);
      expect(error.message).toBe('HTTP Error 500');
    });

    it('should be instanceof HttpError', () => {
      const error = new HttpError(404);
      expect(error instanceof HttpError).toBe(true);
      expect(error instanceof Error).toBe(true);
    });
  });

  describe('getUserFriendlyMessage()', () => {
    it('should return message for 400 (Bad Request)', () => {
      const error = new HttpError(400);
      expect(error.getUserFriendlyMessage()).toBe('Please check your input and try again');
    });

    it('should return message for 401 (Unauthorized)', () => {
      const error = new HttpError(401);
      expect(error.getUserFriendlyMessage()).toBe('Your session has expired. Please log in again.');
    });

    it('should return message for 403 (Forbidden)', () => {
      const error = new HttpError(403);
      expect(error.getUserFriendlyMessage()).toBe('You do not have permission to perform this action.');
    });

    it('should return message for 404 (Not Found)', () => {
      const error = new HttpError(404);
      expect(error.getUserFriendlyMessage()).toBe('The requested resource was not found.');
    });

    it('should return message for 409 (Conflict)', () => {
      const error = new HttpError(409);
      expect(error.getUserFriendlyMessage()).toBe('This operation conflicts with existing data.');
    });

    it('should return message for 500 (Server Error)', () => {
      const error = new HttpError(500);
      expect(error.getUserFriendlyMessage()).toBe('System temporarily unavailable. Please try again later.');
    });

    it('should return message for 503 (Service Unavailable)', () => {
      const error = new HttpError(503);
      expect(error.getUserFriendlyMessage()).toBe('System temporarily unavailable. Please try again later.');
    });

    it('should return generic message for unknown status', () => {
      const error = new HttpError(418); // I'm a teapot
      expect(error.getUserFriendlyMessage()).toBe('An error occurred. Please try again.');
    });
  });

  describe('isClientError()', () => {
    it('should return true for 4xx errors', () => {
      expect(new HttpError(400).isClientError()).toBe(true);
      expect(new HttpError(404).isClientError()).toBe(true);
      expect(new HttpError(409).isClientError()).toBe(true);
    });

    it('should return false for 5xx errors', () => {
      expect(new HttpError(500).isClientError()).toBe(false);
      expect(new HttpError(503).isClientError()).toBe(false);
    });

    it('should return false for 3xx errors', () => {
      expect(new HttpError(301).isClientError()).toBe(false);
    });
  });

  describe('isServerError()', () => {
    it('should return true for 5xx errors', () => {
      expect(new HttpError(500).isServerError()).toBe(true);
      expect(new HttpError(503).isServerError()).toBe(true);
    });

    it('should return false for 4xx errors', () => {
      expect(new HttpError(400).isServerError()).toBe(false);
      expect(new HttpError(404).isServerError()).toBe(false);
    });
  });

  describe('isValidationError()', () => {
    it('should return true for 400', () => {
      expect(new HttpError(400).isValidationError()).toBe(true);
    });

    it('should return true for 422', () => {
      expect(new HttpError(422).isValidationError()).toBe(true);
    });

    it('should return false for other errors', () => {
      expect(new HttpError(409).isValidationError()).toBe(false);
      expect(new HttpError(500).isValidationError()).toBe(false);
    });
  });

  describe('isConflictError()', () => {
    it('should return true for 409', () => {
      expect(new HttpError(409).isConflictError()).toBe(true);
    });

    it('should return false for other errors', () => {
      expect(new HttpError(400).isConflictError()).toBe(false);
      expect(new HttpError(404).isConflictError()).toBe(false);
    });
  });

  describe('isNotFoundError()', () => {
    it('should return true for 404', () => {
      expect(new HttpError(404).isNotFoundError()).toBe(true);
    });

    it('should return false for other errors', () => {
      expect(new HttpError(400).isNotFoundError()).toBe(false);
      expect(new HttpError(409).isNotFoundError()).toBe(false);
    });
  });

  describe('isAuthError()', () => {
    it('should return true for 401', () => {
      expect(new HttpError(401).isAuthError()).toBe(true);
    });

    it('should return true for 403', () => {
      expect(new HttpError(403).isAuthError()).toBe(true);
    });

    it('should return false for other errors', () => {
      expect(new HttpError(400).isAuthError()).toBe(false);
      expect(new HttpError(404).isAuthError()).toBe(false);
      expect(new HttpError(500).isAuthError()).toBe(false);
    });
  });

  describe('Error properties', () => {
    it('should be throwable and catchable', () => {
      expect(() => {
        throw new HttpError(500, { error: 'test' });
      }).toThrow(HttpError);
    });

    it('should have stack trace', () => {
      const error = new HttpError(400);
      expect(error.stack).toBeDefined();
      expect(error.stack).toContain('HttpError');
    });
  });
});

import { describe, it, expect } from 'vitest';
import {
  getUserFriendlyMessage,
  getErrorSeverity,
  getErrorCategory,
} from '@/utils/errorMessages';

describe('errorMessages', () => {
  describe('getUserFriendlyMessage()', () => {
    it('should return PT-BR message for 400 validation error', () => {
      const msg = getUserFriendlyMessage(400);
      expect(msg).toContain('Por favor');
    });

    it('should return PT-BR message for 401 auth error', () => {
      const msg = getUserFriendlyMessage(401);
      expect(msg).toContain('sessão expirou');
      expect(msg).toContain('login');
    });

    it('should return PT-BR message for 403 forbidden', () => {
      const msg = getUserFriendlyMessage(403);
      expect(msg).toContain('permissão');
    });

    it('should return PT-BR message for 404 not found', () => {
      const msg = getUserFriendlyMessage(404);
      expect(msg).toContain('não foi encontrado');
    });

    it('should return PT-BR message for 409 conflict', () => {
      const msg = getUserFriendlyMessage(409);
      expect(msg).toContain('já existe');
    });

    it('should return specific message for 409 duplicate login', () => {
      const msg = getUserFriendlyMessage(409, { message: 'duplicate login' });
      expect(msg).toContain('login');
      expect(msg).toContain('já está em uso');
    });

    it('should return specific message for 409 duplicate email', () => {
      const msg = getUserFriendlyMessage(409, { message: 'duplicate email' });
      expect(msg).toContain('e-mail');
      expect(msg).toContain('registrado');
    });

    it('should return PT-BR message for 500 server error', () => {
      const msg = getUserFriendlyMessage(500);
      expect(msg).toContain('Erro do sistema');
    });

    it('should return PT-BR message for 503 unavailable', () => {
      const msg = getUserFriendlyMessage(503);
      expect(msg).toContain('temporariamente indisponível');
    });

    it('should handle network error (status 0)', () => {
      const msg = getUserFriendlyMessage(0);
      expect(msg).toContain('conexão');
    });

    it('should handle undefined status', () => {
      const msg = getUserFriendlyMessage(undefined as any);
      expect(msg).toContain('conexão');
    });

    it('should return generic client error for unknown 4xx', () => {
      const msg = getUserFriendlyMessage(418);
      expect(msg).toContain('inválida');
    });

    it('should return generic server error for unknown 5xx', () => {
      const msg = getUserFriendlyMessage(502);
      expect(msg).toContain('servidor');
    });

    it('should use custom message from serverData when available', () => {
      const msg = getUserFriendlyMessage(400, { message: 'Custom error message' });
      expect(msg).toBe('Custom error message');
    });

    it('should use error field from serverData', () => {
      const msg = getUserFriendlyMessage(400, { error: 'Custom error from field' });
      expect(msg).toBe('Custom error from field');
    });
  });

  describe('getErrorSeverity()', () => {
    it('should return critical for 5xx errors', () => {
      expect(getErrorSeverity(500)).toBe('critical');
      expect(getErrorSeverity(502)).toBe('critical');
      expect(getErrorSeverity(503)).toBe('critical');
    });

    it('should return warning for 409 and 404', () => {
      expect(getErrorSeverity(409)).toBe('warning');
      expect(getErrorSeverity(404)).toBe('warning');
    });

    it('should return error for other 4xx', () => {
      expect(getErrorSeverity(400)).toBe('error');
      expect(getErrorSeverity(401)).toBe('error');
      expect(getErrorSeverity(403)).toBe('error');
    });

    it('should return info for 2xx/3xx', () => {
      expect(getErrorSeverity(200)).toBe('info');
      expect(getErrorSeverity(301)).toBe('info');
    });
  });

  describe('getErrorCategory()', () => {
    it('should categorize validation errors', () => {
      expect(getErrorCategory(400)).toBe('validation');
      expect(getErrorCategory(422)).toBe('validation');
    });

    it('should categorize not found errors', () => {
      expect(getErrorCategory(404)).toBe('notfound');
    });

    it('should categorize conflict errors', () => {
      expect(getErrorCategory(409)).toBe('conflict');
    });

    it('should categorize network errors', () => {
      expect(getErrorCategory(0)).toBe('network');
      expect(getErrorCategory(undefined as any)).toBe('network');
    });

    it('should categorize client errors', () => {
      expect(getErrorCategory(401)).toBe('client');
      expect(getErrorCategory(403)).toBe('client');
    });

    it('should categorize server errors', () => {
      expect(getErrorCategory(500)).toBe('server');
      expect(getErrorCategory(502)).toBe('server');
      expect(getErrorCategory(503)).toBe('server');
    });
  });
});

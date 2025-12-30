import { describe, it, expect, beforeEach, vi } from 'vitest';
import { userService } from '@/services/userService';
import { HttpError } from '@/utils/httpError';
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

describe('userService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('list()', () => {
    const mockParams = {
      page: 1,
      pageSize: 4,
      filters: { login: 'ADMIN', nome: 'Admin' },
    };

    const mockResponse = {
      sucesso: true,
      usuarios: [
        { usuar_id: 'ADMIN', usuar_nome: 'Admin User', usuar_email: 'admin@test.com', usuar_telefone: '1199999999' },
      ],
      total: 42,
    };

    it('should fetch users with pagination and filters', async () => {
      vi.mocked(apiClient.get).mockResolvedValue(mockResponse);

      const result = await userService.list(mockParams);

      expect(result).toEqual(mockResponse);
      expect(apiClient.get).toHaveBeenCalledWith(
        expect.stringContaining('/usuarios?page=1&pageSize=4&login=ADMIN&nome=Admin'),
        expect.any(Object)
      );
    });

    it('should build query params correctly', async () => {
      vi.mocked(apiClient.get).mockResolvedValue(mockResponse);

      const params = {
        page: 2,
        pageSize: 10,
        filters: { email: 'test@example.com', telefone: '1122334455' },
      };

      await userService.list(params);

      expect(apiClient.get).toHaveBeenCalledWith(
        expect.stringContaining('page=2'),
        expect.anything()
      );
    });

    it('should handle missing filters gracefully', async () => {
      vi.mocked(apiClient.get).mockResolvedValue(mockResponse);

      const params = { page: 1, pageSize: 4, filters: {} };
      await userService.list(params);

      expect(apiClient.get).toHaveBeenCalledWith(
        '/usuarios?page=1&pageSize=4',
        expect.any(Object)
      );
    });

    it('should throw HttpError on 400 response', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 400, data: { error: 'Bad Request' } },
      });

      await expect(userService.list(mockParams)).rejects.toThrow(HttpError);
      await expect(userService.list(mockParams)).rejects.toMatchObject({
        status: 400,
      });
    });

    it('should throw HttpError on 500 response', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 500, data: { error: 'Internal Server Error' } },
      });

      await expect(userService.list(mockParams)).rejects.toThrow(HttpError);
      await expect(userService.list(mockParams)).rejects.toMatchObject({
        status: 500,
      });
    });

    it('should handle timeout error', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        code: 'ECONNABORTED',
        message: 'timeout',
      });

      await expect(userService.list(mockParams)).rejects.toThrow(HttpError);
    });

    it('should support AbortSignal', async () => {
      vi.mocked(apiClient.get).mockResolvedValue(mockResponse);
      const signal = new AbortController().signal;

      await userService.list(mockParams, signal);

      expect(apiClient.get).toHaveBeenCalledWith(
        expect.any(String),
        expect.objectContaining({ signal })
      );
    });
  });

  describe('getAll()', () => {
    it('should fetch all users', async () => {
      const mockUsers = [
        { usuar_id: 'USER1', usuar_nome: 'User 1', usuar_email: 'user1@test.com', usuar_telefone: '1111111111' },
      ];

      vi.mocked(apiClient.get).mockResolvedValue(mockUsers);
      const result = await userService.getAll();

      expect(result).toEqual(mockUsers);
      expect(apiClient.get).toHaveBeenCalledWith('/usuarios', expect.any(Object));
    });

    it('should throw HttpError on failure', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 500 },
      });

      await expect(userService.getAll()).rejects.toThrow(HttpError);
    });
  });

  describe('getById()', () => {
    it('should fetch user by ID', async () => {
      const mockUser = {
        usuar_id: 'ADMIN',
        usuar_nome: 'Admin User',
        usuar_email: 'admin@test.com',
        usuar_telefone: '1199999999',
      };

      vi.mocked(apiClient.get).mockResolvedValue(mockUser);
      const result = await userService.getById('ADMIN');

      expect(result).toEqual(mockUser);
      expect(apiClient.get).toHaveBeenCalledWith('/usuarios/ADMIN', expect.any(Object));
    });

    it('should throw HttpError 404 when not found', async () => {
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 404 },
      });

      await expect(userService.getById('NONEXISTENT')).rejects.toThrow(HttpError);
    });
  });

  describe('create()', () => {
    const mockFormData = {
      usuar_id: 'NEWUSER',
      usuar_nome: 'New User',
      usuar_email: 'new@test.com',
      usuar_telefone: '1188888888',
    };

    it('should create user successfully', async () => {
      vi.mocked(apiClient.post).mockResolvedValue(mockFormData);

      const result = await userService.create(mockFormData);

      expect(result.sucesso).toBe(true);
      expect(result.mensagem).toBe('Usuário incluído com sucesso!');
      expect(apiClient.post).toHaveBeenCalledWith('/usuarios', mockFormData, expect.any(Object));
    });

    it('should throw HttpError 409 on duplicate login', async () => {
      vi.mocked(apiClient.post).mockRejectedValue({
        response: { status: 409, data: { error: 'Duplicate' } },
      });

      await expect(userService.create(mockFormData)).rejects.toThrow(HttpError);
      await expect(userService.create(mockFormData)).rejects.toMatchObject({
        status: 409,
      });
    });

    it('should throw HttpError 400 on validation error', async () => {
      vi.mocked(apiClient.post).mockRejectedValue({
        response: { status: 400, data: { email: 'Invalid' } },
      });

      await expect(userService.create(mockFormData)).rejects.toThrow(HttpError);
      await expect(userService.create(mockFormData)).rejects.toMatchObject({
        status: 400,
      });
    });
  });

  describe('update()', () => {
    const mockData = {
      usuar_id: 'ADMIN',
      usuar_nome: 'Updated',
      usuar_email: 'admin@test.com',
      usuar_telefone: '1199999999',
    };

    it('should update user successfully', async () => {
      vi.mocked(apiClient.put).mockResolvedValue(mockData);

      const result = await userService.update('ADMIN', mockData);

      expect(result.sucesso).toBe(true);
      expect(result.mensagem).toBe('Usuário alterado com sucesso!');
      expect(apiClient.put).toHaveBeenCalledWith('/usuarios/ADMIN', mockData, expect.any(Object));
    });

    it('should throw HttpError 404 when not found', async () => {
      vi.mocked(apiClient.put).mockRejectedValue({
        response: { status: 404 },
      });

      await expect(userService.update('NONEXISTENT', mockData)).rejects.toThrow(HttpError);
    });

    it('should throw HttpError 400 on validation', async () => {
      vi.mocked(apiClient.put).mockRejectedValue({
        response: { status: 400 },
      });

      await expect(userService.update('ADMIN', mockData)).rejects.toThrow(HttpError);
    });
  });

  describe('delete()', () => {
    it('should delete single user successfully', async () => {
      vi.mocked(apiClient.delete).mockResolvedValue({});

      const result = await userService.delete(['OLDUSER']);

      expect(result.sucesso).toBe(true);
      expect(result.mensagem).toBe('Usuário excluído com sucesso!');
      expect(apiClient.delete).toHaveBeenCalledWith('/usuarios/OLDUSER', expect.any(Object));
    });

    it('should delete multiple users serially', async () => {
      vi.mocked(apiClient.delete).mockResolvedValue({});

      const result = await userService.delete(['USER1', 'USER2', 'USER3']);

      expect(result.sucesso).toBe(true);
      expect(result.mensagem).toBe('3 usuário(s) excluído(s) com sucesso!');
      expect(apiClient.delete).toHaveBeenCalledTimes(3);
    });

    it('should throw HttpError 404 when not found', async () => {
      vi.mocked(apiClient.delete).mockRejectedValue({
        response: { status: 404 },
      });

      await expect(userService.delete(['NONEXISTENT'])).rejects.toThrow(HttpError);
    });

    it('should throw HttpError 403 on permission denied', async () => {
      vi.mocked(apiClient.delete).mockRejectedValue({
        response: { status: 403 },
      });

      await expect(userService.delete(['PROTECTED'])).rejects.toThrow(HttpError);
    });

    it('should throw HttpError on empty IDs', async () => {
      await expect(userService.delete([])).rejects.toThrow(HttpError);
    });

    it('should fail on first error in multi-delete', async () => {
      vi.mocked(apiClient.delete)
        .mockResolvedValueOnce({})
        .mockRejectedValueOnce({ response: { status: 500 } });

      await expect(userService.delete(['USER1', 'USER2'])).rejects.toThrow(HttpError);
      expect(apiClient.delete).toHaveBeenCalledTimes(2);
    });
  });

  describe('Error logging', () => {
    it('should log errors for debugging', async () => {
      const consoleSpy = vi.spyOn(console, 'error').mockImplementation(() => {});
      vi.mocked(apiClient.get).mockRejectedValue({
        response: { status: 500, data: { error: 'Server error' } },
      });

      try {
        await userService.list({ page: 1, pageSize: 4 });
      } catch {
        // Expected
      }

      expect(consoleSpy).toHaveBeenCalledWith(
        '[userService.list] Error:',
        expect.any(Object)
      );

      consoleSpy.mockRestore();
    });
  });
});

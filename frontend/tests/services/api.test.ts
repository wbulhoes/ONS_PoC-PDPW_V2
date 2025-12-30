import { describe, it, expect, beforeEach } from 'vitest';
import { useAuthStore } from '../../src/store/authStore';
import { useAppStore } from '../../src/store/appStore';

describe('ApiClient', () => {
  beforeEach(() => {
    useAuthStore.setState({ user: null, token: null, isAuthenticated: false });
    useAppStore.setState({ isLoading: false, error: null });
  });

  it('should handle authentication token in store', () => {
    useAuthStore.setState({ token: 'test-token', isAuthenticated: true });
    expect(useAuthStore.getState().token).toBe('test-token');
  });

  it('should handle request without token', () => {
    const state = useAuthStore.getState();
    expect(state.token).toBeNull();
  });

  it('should manage error state', () => {
    useAppStore.getState().setError('Test error');
    expect(useAppStore.getState().error).toBe('Test error');

    useAppStore.getState().clearError();
    expect(useAppStore.getState().error).toBeNull();
  });
});
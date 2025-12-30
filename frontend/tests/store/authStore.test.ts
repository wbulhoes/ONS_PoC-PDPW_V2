import { render } from '@testing-library/react';
import { describe, it, expect, beforeEach } from 'vitest';
import { useAuthStore } from '../../src/store/authStore';

describe('AuthStore', () => {
  beforeEach(() => {
    useAuthStore.setState({
      user: null,
      isAuthenticated: false,
      token: null,
    });
  });

  it('should initialize with default values', () => {
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
    expect(state.token).toBeNull();
  });

  it('should login user', () => {
    const user = { id: '1', name: 'Test User', email: 'test@test.com', role: 'admin' };
    const token = 'test-token';

    useAuthStore.getState().login(user, token);

    const state = useAuthStore.getState();
    expect(state.user).toEqual(user);
    expect(state.token).toBe(token);
    expect(state.isAuthenticated).toBe(true);
  });

  it('should logout user', () => {
    const user = { id: '1', name: 'Test User', email: 'test@test.com', role: 'admin' };
    useAuthStore.getState().login(user, 'token');
    
    useAuthStore.getState().logout();

    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.token).toBeNull();
    expect(state.isAuthenticated).toBe(false);
  });

  it('should update user data', () => {
    const user = { id: '1', name: 'Test User', email: 'test@test.com', role: 'admin' };
    useAuthStore.getState().login(user, 'token');

    useAuthStore.getState().updateUser({ name: 'Updated Name' });

    const state = useAuthStore.getState();
    expect(state.user?.name).toBe('Updated Name');
    expect(state.user?.email).toBe('test@test.com');
  });
});

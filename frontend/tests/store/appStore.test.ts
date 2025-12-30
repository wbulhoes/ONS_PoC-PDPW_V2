import { describe, it, expect, beforeEach } from 'vitest';
import { useAppStore } from '../../src/store/appStore';

describe('AppStore', () => {
  beforeEach(() => {
    useAppStore.setState({
      isLoading: false,
      error: null,
    });
  });

  it('should initialize with default values', () => {
    const state = useAppStore.getState();
    expect(state.isLoading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should set loading state', () => {
    useAppStore.getState().setLoading(true);
    expect(useAppStore.getState().isLoading).toBe(true);

    useAppStore.getState().setLoading(false);
    expect(useAppStore.getState().isLoading).toBe(false);
  });

  it('should set error message', () => {
    const errorMessage = 'Test error';
    useAppStore.getState().setError(errorMessage);
    expect(useAppStore.getState().error).toBe(errorMessage);
  });

  it('should clear error', () => {
    useAppStore.getState().setError('Test error');
    expect(useAppStore.getState().error).toBe('Test error');

    useAppStore.getState().clearError();
    expect(useAppStore.getState().error).toBeNull();
  });
});

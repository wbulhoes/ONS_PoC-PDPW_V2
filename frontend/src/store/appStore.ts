import { create } from 'zustand';
import { devtools } from 'zustand/middleware';

interface AppState {
  isLoading: boolean;
  error: string | null;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  clearError: () => void;
}

export const useAppStore = create<AppState>()(
  devtools(
    (set) => ({
      isLoading: false,
      error: null,
      setLoading: (loading) => set({ isLoading: loading }, false, 'app/setLoading'),
      setError: (error) => set({ error }, false, 'app/setError'),
      clearError: () => set({ error: null }, false, 'app/clearError'),
    }),
    { name: 'app-store' }
  )
);

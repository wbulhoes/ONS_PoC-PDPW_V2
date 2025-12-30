import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';

interface User {
  id: string;
  name: string;
  email: string;
  role: string;
}

interface AuthState {
  user: User | null;
  isAuthenticated: boolean;
  token: string | null;
  isLoading: boolean;
  login: (user: User, token: string) => void;
  logout: () => void;
  updateUser: (user: Partial<User>) => void;
  setLoading: (isLoading: boolean) => void;
}

export const useAuthStore = create<AuthState>()(
  devtools(
    persist(
      (set) => ({
        user: null,
        isAuthenticated: false,
        token: null,
        isLoading: false,
        login: (user, token) => set({ user, token, isAuthenticated: true }, false, 'auth/login'),
        logout: () =>
          set({ user: null, token: null, isAuthenticated: false }, false, 'auth/logout'),
        updateUser: (userData) =>
          set(
            (state) => ({
              user: state.user ? { ...state.user, ...userData } : null,
            }),
            false,
            'auth/updateUser'
          ),
        setLoading: (isLoading) => set({ isLoading }, false, 'auth/setLoading'),
      }),
      {
        name: 'auth-storage',
      }
    )
  )
);

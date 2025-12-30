import { render, screen, waitFor, act } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { MemoryRouter } from 'react-router-dom';
import userEvent from '@testing-library/user-event';
import IntegrationAuth from '../../src/pages/Auth/IntegrationAuth';

const mockNavigate = vi.fn();

vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom');
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  };
});

describe('IntegrationAuth', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useFakeTimers();
    localStorage.clear();
    delete (window as any).location;
    (window as any).location = { href: '', search: '' };
  });

  afterEach(() => {
    vi.clearAllTimers();
    vi.useRealTimers();
  });

  const renderIntegrationAuth = (searchParams = '') => {
    const initialEntries = searchParams ? [`/?${searchParams}`] : ['/'];
    return render(
      <MemoryRouter initialEntries={initialEntries}>
        <IntegrationAuth />
      </MemoryRouter>
    );
  };

  describe('Rendering', () => {
    it('renders integration auth container', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-container')).toBeInTheDocument();
    });

    it('renders integration auth content', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-content')).toBeInTheDocument();
    });

    it('renders logo container', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-logo-container')).toBeInTheDocument();
    });

    it('renders logo image', () => {
      renderIntegrationAuth();
      const logo = screen.getByTestId('integration-auth-logo');
      expect(logo).toBeInTheDocument();
      expect(logo).toHaveAttribute('src', '/logo.png');
      expect(logo).toHaveAttribute('alt', 'PDPw Logo');
    });

    it('renders title', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-title')).toHaveTextContent('Autenticação Integrada');
    });

    it('renders subtitle', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-subtitle')).toHaveTextContent(
        'Sistema de Single Sign-On (SSO)'
      );
    });

    it('renders footer', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-footer')).toBeInTheDocument();
    });

    it('renders footer text', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-footer-text')).toHaveTextContent(
        '© 2024 ONS - Operador Nacional do Sistema Elétrico'
      );
    });

    it('renders footer secure text', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-footer-secure')).toHaveTextContent('🔒 Conexão Segura');
    });
  });

  describe('Initializing state', () => {
    it('shows loading container initially', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-loading-container')).toBeInTheDocument();
    });

    it('shows spinner', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-spinner')).toBeInTheDocument();
    });

    it('shows progress bar', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-progress-bar')).toBeInTheDocument();
    });

    it('shows progress fill', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-progress-fill')).toBeInTheDocument();
    });

    it('shows status text with "Inicializando..."', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-status-text')).toHaveTextContent('Inicializando...');
    });

    it('shows progress text with initial progress', () => {
      renderIntegrationAuth();
      expect(screen.getByTestId('integration-auth-progress-text')).toHaveTextContent('0%');
    });
  });

  describe('Progress functionality', () => {
    it('updates progress over time', async () => {
      renderIntegrationAuth();

      expect(screen.getByTestId('integration-auth-progress-text')).toHaveTextContent('0%');

      await act(async () => {
        vi.advanceTimersByTime(300);
      });

      expect(screen.getByTestId('integration-auth-progress-text')).toHaveTextContent('15%');
    });
  });

  describe('Authenticating state', () => {
    it('transitions to authenticating state', async () => {
      renderIntegrationAuth();

      await act(async () => {
        vi.advanceTimersByTime(500);
      });

      expect(screen.getByTestId('integration-auth-status-text')).toHaveTextContent('Autenticando...');
    });
  });

  describe('Success state', () => {
    it('shows success state with valid token', async () => {
      renderIntegrationAuth('token=valid-token-123');

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('integration-auth-success-container')).toBeInTheDocument();
    });

    it('shows success icon', async () => {
      renderIntegrationAuth('token=valid-token-123');

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('integration-auth-success-icon')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-success-icon')).toHaveTextContent('✓');
    });

    it('shows success text', async () => {
      renderIntegrationAuth('token=valid-token-123');

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('integration-auth-success-text')).toHaveTextContent(
        'Autenticação realizada com sucesso!'
      );
    });

    it('shows redirect text', async () => {
      renderIntegrationAuth('token=valid-token-123');

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('integration-auth-redirect-text')).toHaveTextContent('Redirecionando...');
    });

    it('stores token in localStorage', async () => {
      renderIntegrationAuth('token=valid-token-123');

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(localStorage.getItem('auth_token')).toBe('valid-token-123');
    });

  });

  describe('Error state', () => {
    it('shows error state when error param is present', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-error-container')).toBeInTheDocument();
    });

    it('shows error icon', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-error-icon')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-error-icon')).toHaveTextContent('✕');
    });

    it('shows error title', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-error-title')).toHaveTextContent('Erro na Autenticação');
    });

    it('shows error message from URL param', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-error-message')).toHaveTextContent('Authentication failed');
    });

    it('shows default error message when no specific error', async () => {
      renderIntegrationAuth();

      await act(async () => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('integration-auth-error-container')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-error-message')).toHaveTextContent(
        'Token de autenticação não encontrado'
      );
    });

    it('shows error actions', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-error-actions')).toBeInTheDocument();
    });

    it('shows retry button', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-btn-retry')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-btn-retry')).toHaveTextContent('Tentar Novamente');
    });

    it('shows cancel button', () => {
      renderIntegrationAuth('error=Authentication%20failed');
      expect(screen.getByTestId('integration-auth-btn-cancel')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-btn-cancel')).toHaveTextContent('Cancelar');
    });
  });

  describe('Data-testid attributes', () => {
    it('has all required data-testid attributes in loading state', () => {
      renderIntegrationAuth();

      expect(screen.getByTestId('integration-auth-container')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-content')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-logo-container')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-logo')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-title')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-subtitle')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-loading-container')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-spinner')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-progress-bar')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-progress-fill')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-status-text')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-progress-text')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-footer')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-footer-text')).toBeInTheDocument();
      expect(screen.getByTestId('integration-auth-footer-secure')).toBeInTheDocument();
    });
  });

  describe('Cleanup', () => {
    it('cleans up timers on unmount', async () => {
      const { unmount } = renderIntegrationAuth();

      await act(async () => {
        unmount();
      });

      expect(vi.getTimerCount()).toBe(0);
    });
  });
});

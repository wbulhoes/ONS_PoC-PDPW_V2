import { render, screen, waitFor, act } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { BrowserRouter } from 'react-router-dom';
import Splash from '../../src/pages/Auth/Splash';

const mockNavigate = vi.fn();

vi.mock('react-router-dom', async () => {
  const actual = await vi.importActual('react-router-dom');
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  };
});

describe('Splash', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.clearAllTimers();
    vi.useRealTimers();
  });

  const renderSplash = () => {
    return render(
      <BrowserRouter>
        <Splash />
      </BrowserRouter>
    );
  };

  describe('Rendering', () => {
    it('renders splash container', () => {
      renderSplash();
      expect(screen.getByTestId('splash-container')).toBeInTheDocument();
    });

    it('renders splash content', () => {
      renderSplash();
      expect(screen.getByTestId('splash-content')).toBeInTheDocument();
    });

    it('renders logo container', () => {
      renderSplash();
      expect(screen.getByTestId('splash-logo-container')).toBeInTheDocument();
    });

    it('renders logo image', () => {
      renderSplash();
      const logo = screen.getByTestId('splash-logo');
      expect(logo).toBeInTheDocument();
      expect(logo).toHaveAttribute('src', '/logo.png');
      expect(logo).toHaveAttribute('alt', 'PDPw Logo');
    });

    it('renders title', () => {
      renderSplash();
      expect(screen.getByTestId('splash-title')).toHaveTextContent('PDPw');
    });

    it('renders subtitle', () => {
      renderSplash();
      expect(screen.getByTestId('splash-subtitle')).toHaveTextContent(
        'Sistema de Planejamento e Programação da Operação Energética'
      );
    });

    it('renders footer', () => {
      renderSplash();
      expect(screen.getByTestId('splash-footer')).toBeInTheDocument();
    });

    it('renders footer text', () => {
      renderSplash();
      expect(screen.getByTestId('splash-footer-text')).toHaveTextContent(
        '© 2024 ONS - Operador Nacional do Sistema Elétrico'
      );
    });
  });

  describe('Loading state', () => {
    it('shows loading container initially', () => {
      renderSplash();
      expect(screen.getByTestId('splash-loading-container')).toBeInTheDocument();
    });

    it('shows progress bar', () => {
      renderSplash();
      expect(screen.getByTestId('splash-progress-bar')).toBeInTheDocument();
    });

    it('shows progress fill', () => {
      renderSplash();
      expect(screen.getByTestId('splash-progress-fill')).toBeInTheDocument();
    });

    it('shows loading text with initial progress', () => {
      renderSplash();
      expect(screen.getByTestId('splash-loading-text')).toHaveTextContent('Carregando... 0%');
    });

    it('does not show ready container initially', () => {
      renderSplash();
      expect(screen.queryByTestId('splash-ready-container')).not.toBeInTheDocument();
    });
  });

  describe('Progress functionality', () => {
    it('updates progress over time', async () => {
      renderSplash();

      expect(screen.getByTestId('splash-loading-text')).toHaveTextContent('Carregando... 0%');

      act(() => {
        vi.advanceTimersByTime(200);
      });

      expect(screen.getByTestId('splash-loading-text')).toHaveTextContent('Carregando... 10%');

      act(() => {
        vi.advanceTimersByTime(200);
      });

      expect(screen.getByTestId('splash-loading-text')).toHaveTextContent('Carregando... 20%');
    });

    it('progress bar width increases with progress', async () => {
      renderSplash();

      const progressFill = screen.getByTestId('splash-progress-fill');
      expect(progressFill).toHaveStyle({ width: '0%' });

      act(() => {
        vi.advanceTimersByTime(200);
      });

      expect(progressFill).toHaveStyle({ width: '10%' });

      act(() => {
        vi.advanceTimersByTime(400);
      });

      expect(progressFill).toHaveStyle({ width: '30%' });
    });

    it('progress reaches 100%', async () => {
      renderSplash();

      act(() => {
        vi.advanceTimersByTime(2000);
      });

      expect(screen.getByTestId('splash-loading-text')).toHaveTextContent('Carregando... 100%');
    });
  });

  describe('Ready state', () => {
    it('shows ready container after loading completes', async () => {
      renderSplash();

      act(() => {
        vi.advanceTimersByTime(2500);
      });

      expect(screen.queryByTestId('splash-loading-container')).not.toBeInTheDocument();
      expect(screen.getByTestId('splash-ready-container')).toBeInTheDocument();
    });

    it('shows ready text', async () => {
      renderSplash();

      act(() => {
        vi.advanceTimersByTime(2500);
      });

      expect(screen.getByTestId('splash-ready-text')).toHaveTextContent('Sistema pronto!');
    });
  });

  describe('Navigation', () => {
    it('navigates to home after loading completes', async () => {
      renderSplash();

      act(() => {
        vi.advanceTimersByTime(3000);
      });

      expect(mockNavigate).toHaveBeenCalledWith('/');
    });

    it('navigates only once', async () => {
      renderSplash();

      act(() => {
        vi.advanceTimersByTime(3000);
      });

      expect(mockNavigate).toHaveBeenCalledTimes(1);
    });
  });

  describe('Data-testid attributes', () => {
    it('has all required data-testid attributes', () => {
      renderSplash();
      
      expect(screen.getByTestId('splash-container')).toBeInTheDocument();
      expect(screen.getByTestId('splash-content')).toBeInTheDocument();
      expect(screen.getByTestId('splash-logo-container')).toBeInTheDocument();
      expect(screen.getByTestId('splash-logo')).toBeInTheDocument();
      expect(screen.getByTestId('splash-title')).toBeInTheDocument();
      expect(screen.getByTestId('splash-subtitle')).toBeInTheDocument();
      expect(screen.getByTestId('splash-loading-container')).toBeInTheDocument();
      expect(screen.getByTestId('splash-progress-bar')).toBeInTheDocument();
      expect(screen.getByTestId('splash-progress-fill')).toBeInTheDocument();
      expect(screen.getByTestId('splash-loading-text')).toBeInTheDocument();
      expect(screen.getByTestId('splash-footer')).toBeInTheDocument();
      expect(screen.getByTestId('splash-footer-text')).toBeInTheDocument();
    });
  });

  describe('Cleanup', () => {
    it('cleans up timers on unmount', () => {
      const { unmount } = renderSplash();

      unmount();

      act(() => {
        vi.advanceTimersByTime(3000);
      });

      expect(mockNavigate).not.toHaveBeenCalled();
    });
  });
});

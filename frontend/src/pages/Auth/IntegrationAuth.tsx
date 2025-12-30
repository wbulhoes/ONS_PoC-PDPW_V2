import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import styles from './IntegrationAuth.module.css';

type AuthStatus = 'initializing' | 'authenticating' | 'success' | 'error';

export default function IntegrationAuth() {
  const [status, setStatus] = useState<AuthStatus>('initializing');
  const [errorMessage, setErrorMessage] = useState('');
  const [progress, setProgress] = useState(0);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  useEffect(() => {
    const token = searchParams.get('token');
    const error = searchParams.get('error');

    if (error) {
      setStatus('error');
      setErrorMessage(decodeURIComponent(error));
      return;
    }

    const progressInterval = setInterval(() => {
      setProgress((prev) => {
        if (prev >= 90) {
          clearInterval(progressInterval);
          return 90;
        }
        return prev + 15;
      });
    }, 300);

    const authenticateTimer = setTimeout(() => {
      setStatus('authenticating');
    }, 500);

    const authTimer = setTimeout(() => {
      if (token) {
        localStorage.setItem('auth_token', token);
        setProgress(100);
        setStatus('success');

        setTimeout(() => {
          navigate('/');
        }, 1000);
      } else {
        setStatus('error');
        setErrorMessage('Token de autenticação não encontrado');
      }
    }, 2000);

    return () => {
      clearInterval(progressInterval);
      clearTimeout(authenticateTimer);
      clearTimeout(authTimer);
    };
  }, [navigate, searchParams]);

  const handleRetry = () => {
    window.location.href = '/api/auth/sso/login';
  };

  const handleCancel = () => {
    navigate('/');
  };

  return (
    <div className={styles.container} data-testid="integration-auth-container">
      <div className={styles.content} data-testid="integration-auth-content">
        <div className={styles.logoContainer} data-testid="integration-auth-logo-container">
          <img
            src="/logo.png"
            alt="PDPw Logo"
            className={styles.logo}
            data-testid="integration-auth-logo"
          />
        </div>

        <h1 className={styles.title} data-testid="integration-auth-title">
          Autenticação Integrada
        </h1>

        <p className={styles.subtitle} data-testid="integration-auth-subtitle">
          Sistema de Single Sign-On (SSO)
        </p>

        {(status === 'initializing' || status === 'authenticating') && (
          <div className={styles.loadingContainer} data-testid="integration-auth-loading-container">
            <div className={styles.spinner} data-testid="integration-auth-spinner">
              <div className={styles.spinnerCircle}></div>
            </div>
            <div className={styles.progressBar} data-testid="integration-auth-progress-bar">
              <div
                className={styles.progressFill}
                style={{ width: `${progress}%` }}
                data-testid="integration-auth-progress-fill"
              />
            </div>
            <p className={styles.statusText} data-testid="integration-auth-status-text">
              {status === 'initializing' ? 'Inicializando...' : 'Autenticando...'}
            </p>
            <p className={styles.progressText} data-testid="integration-auth-progress-text">
              {progress}%
            </p>
          </div>
        )}

        {status === 'success' && (
          <div className={styles.successContainer} data-testid="integration-auth-success-container">
            <div className={styles.successIcon} data-testid="integration-auth-success-icon">
              ✓
            </div>
            <p className={styles.successText} data-testid="integration-auth-success-text">
              Autenticação realizada com sucesso!
            </p>
            <p className={styles.redirectText} data-testid="integration-auth-redirect-text">
              Redirecionando...
            </p>
          </div>
        )}

        {status === 'error' && (
          <div className={styles.errorContainer} data-testid="integration-auth-error-container">
            <div className={styles.errorIcon} data-testid="integration-auth-error-icon">
              ✕
            </div>
            <p className={styles.errorTitle} data-testid="integration-auth-error-title">
              Erro na Autenticação
            </p>
            <p className={styles.errorMessage} data-testid="integration-auth-error-message">
              {errorMessage || 'Ocorreu um erro durante o processo de autenticação'}
            </p>
            <div className={styles.errorActions} data-testid="integration-auth-error-actions">
              <button
                onClick={handleRetry}
                className={styles.retryButton}
                data-testid="integration-auth-btn-retry"
              >
                Tentar Novamente
              </button>
              <button
                onClick={handleCancel}
                className={styles.cancelButton}
                data-testid="integration-auth-btn-cancel"
              >
                Cancelar
              </button>
            </div>
          </div>
        )}
      </div>

      <footer className={styles.footer} data-testid="integration-auth-footer">
        <p data-testid="integration-auth-footer-text">
          © 2024 ONS - Operador Nacional do Sistema Elétrico
        </p>
        <p className={styles.footerSecure} data-testid="integration-auth-footer-secure">
          🔒 Conexão Segura
        </p>
      </footer>
    </div>
  );
}

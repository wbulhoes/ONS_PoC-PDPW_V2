import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './Splash.module.css';

export default function Splash() {
  const [loading, setLoading] = useState(true);
  const [progress, setProgress] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    const progressInterval = setInterval(() => {
      setProgress((prev) => {
        if (prev >= 100) {
          clearInterval(progressInterval);
          return 100;
        }
        return prev + 10;
      });
    }, 200);

    const timer = setTimeout(() => {
      setLoading(false);
      setTimeout(() => {
        navigate('/');
      }, 500);
    }, 2500);

    return () => {
      clearInterval(progressInterval);
      clearTimeout(timer);
    };
  }, [navigate]);

  return (
    <div className={styles.container} data-testid="splash-container">
      <div className={styles.content} data-testid="splash-content">
        <div className={styles.logoContainer} data-testid="splash-logo-container">
          <img src="/logo.png" alt="PDPw Logo" className={styles.logo} data-testid="splash-logo" />
        </div>

        <h1 className={styles.title} data-testid="splash-title">
          PDPw
        </h1>

        <p className={styles.subtitle} data-testid="splash-subtitle">
          Sistema de Planejamento e Programação da Operação Energética
        </p>

        {loading && (
          <div className={styles.loadingContainer} data-testid="splash-loading-container">
            <div className={styles.progressBar} data-testid="splash-progress-bar">
              <div
                className={styles.progressFill}
                style={{ width: `${progress}%` }}
                data-testid="splash-progress-fill"
              />
            </div>
            <p className={styles.loadingText} data-testid="splash-loading-text">
              Carregando... {progress}%
            </p>
          </div>
        )}

        {!loading && (
          <div className={styles.readyContainer} data-testid="splash-ready-container">
            <p className={styles.readyText} data-testid="splash-ready-text">
              Sistema pronto!
            </p>
          </div>
        )}
      </div>

      <footer className={styles.footer} data-testid="splash-footer">
        <p data-testid="splash-footer-text">© 2024 ONS - Operador Nacional do Sistema Elétrico</p>
      </footer>
    </div>
  );
}

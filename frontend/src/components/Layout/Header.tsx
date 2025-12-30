import React from 'react';
import styles from './Header.module.css';

interface HeaderProps {
  userName?: string;
}

const Header: React.FC<HeaderProps> = ({ userName = 'Usuário' }) => {
  return (
    <header className={styles.header} data-testid="header">
      <div className={styles['header-container']} data-testid="header-container">
        <div className={styles['header-logo']} data-testid="header-logo">
          <img src="/images/TituloPDPW.gif" alt="PDPw Logo" data-testid="header-logo-image" />
          <h1 className={styles['header-title']} data-testid="header-title">
            Programação Diária de Produção
          </h1>
        </div>
        <div className={styles['header-user']} data-testid="header-user">
          <span data-testid="header-user-name">Bem-vindo, {userName}</span>
        </div>
      </div>
    </header>
  );
};

export default Header;

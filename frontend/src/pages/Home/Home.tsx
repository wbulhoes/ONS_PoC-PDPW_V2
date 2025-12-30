import styles from './Home.module.css';

export default function Home() {
  return (
    <div className={styles.container} data-testid="home-container">
      <div className={styles.welcomeCard} data-testid="home-welcome-card">
        <h1 className={styles.title} data-testid="home-title">
          Bem-vindo ao PDPw
        </h1>
        <p className={styles.subtitle} data-testid="home-subtitle">
          Sistema de Planejamento e Programação da Operação Energética
        </p>
        <div className={styles.description} data-testid="home-description">
          <p data-testid="home-description-text-1">
            O PDPw é o sistema responsável pelo gerenciamento e controle das atividades relacionadas
            ao planejamento da operação do Sistema Interligado Nacional (SIN).
          </p>
          <p data-testid="home-description-text-2">
            Utilize o menu de navegação para acessar as funcionalidades disponíveis.
          </p>
        </div>
      </div>
    </div>
  );
}

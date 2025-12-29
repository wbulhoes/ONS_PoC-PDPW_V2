import React, { useState, useEffect } from 'react';
import { dashboardService } from '../services';
import { DashboardResumoDto } from '../types';
import styles from './Dashboard.module.css';

const Dashboard: React.FC = () => {
  const [resumo, setResumo] = useState<DashboardResumoDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    carregarResumo();
  }, []);

  const carregarResumo = async () => {
    try {
      setLoading(true);
      const data = await dashboardService.obterResumo();
      setResumo(data);
    } catch (err) {
      console.error('Erro ao carregar resumo:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className={styles.loading}>Carregando dashboard...</div>;
  }

  if (!resumo) {
    return <div className={styles.error}>Erro ao carregar dados do dashboard</div>;
  }

  return (
    <div className={styles.container}>
      <h1>Dashboard - PDPw</h1>
      <p className={styles.subtitle}>Programação Diária da Produção - Sistema Elétrico Brasileiro</p>

      <div className={styles.cards}>
        <div className={styles.card}>
          <div className={styles.cardIcon}>🏭</div>
          <div className={styles.cardContent}>
            <h3>Usinas</h3>
            <p className={styles.cardValue}>{resumo.totalUsinas}</p>
            <p className={styles.cardLabel}>Total cadastradas</p>
          </div>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>⚡</div>
          <div className={styles.cardContent}>
            <h3>Unidades Geradoras</h3>
            <p className={styles.cardValue}>{resumo.totalUnidadesGeradoras}</p>
            <p className={styles.cardLabel}>Em operação</p>
          </div>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>🔋</div>
          <div className={styles.cardContent}>
            <h3>Capacidade Total</h3>
            <p className={styles.cardValue}>
              {resumo.capacidadeTotalMW !== undefined && resumo.capacidadeTotalMW !== null
                ? resumo.capacidadeTotalMW.toLocaleString()
                : '--'} MW
            </p>
            <p className={styles.cardLabel}>Potência instalada</p>
          </div>
        </div>

        <div className={styles.card}>
          <div className={styles.cardIcon}>📊</div>
          <div className={styles.cardContent}>
            <h3>Programações</h3>
            <p className={styles.cardValue}>{resumo.programacoesEmAndamento}</p>
            <p className={styles.cardLabel}>Em andamento</p>
          </div>
        </div>
      </div>

      <div className={styles.workflow}>
        <h2>Workflow da Programação Diária</h2>
        <div className={styles.workflowSteps}>
          <div className={styles.step}>
            <div className={styles.stepNumber}>1</div>
            <div className={styles.stepContent}>
              <h4>Dados Energéticos</h4>
              <p>Cadastro de produção e capacidade das usinas</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>2</div>
            <div className={styles.stepContent}>
              <h4>Programação Elétrica</h4>
              <p>Cargas, Intercâmbios e Balanços Energéticos</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>3</div>
            <div className={styles.stepContent}>
              <h4>Previsão Eólica</h4>
              <p>Estimativas de geração eólica</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>4</div>
            <div className={styles.stepContent}>
              <h4>Geração de Arquivos</h4>
              <p>Arquivos DADGER para modelos</p>
            </div>
          </div>

          <div className={styles.stepArrow}>→</div>

          <div className={styles.step}>
            <div className={styles.stepNumber}>5</div>
            <div className={styles.stepContent}>
              <h4>Finalização</h4>
              <p>Aprovação e publicação</p>
            </div>
          </div>
        </div>
      </div>

      <div className={styles.infoSection}>
        <div className={styles.infoCard}>
          <h3>📋 Módulos Adicionais</h3>
          <ul>
            <li>
              <strong>Insumos de Agentes:</strong> Recebimento de dados das empresas
            </li>
            <li>
              <strong>Ofertas de Exportação:</strong> Ofertas de térmicas para exportação
            </li>
            <li>
              <strong>Ofertas de Resposta Voluntária:</strong> Redução voluntária de demanda
            </li>
            <li>
              <strong>Energia Vertida:</strong> Controle de vertimento turbinável
            </li>
          </ul>
        </div>

        <div className={styles.infoCard}>
          <h3>ℹ️ Sobre o Sistema</h3>
          <p>
            O PDPw é o sistema de Programação Diária da Produção do ONS (Operador Nacional do Sistema Elétrico),
            responsável pela coordenação da operação do Sistema Interligado Nacional (SIN).
          </p>
          <p className={styles.updateInfo}>
            Última atualização: {resumo.ultimaAtualizacao
                ? new Date(resumo.ultimaAtualizacao).toLocaleString()
                : '--'}
          </p>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;

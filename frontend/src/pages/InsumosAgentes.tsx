import React, { useState } from 'react';
import styles from './OfertasExportacao.module.css';

const InsumosAgentes: React.FC = () => {
  const [arquivo, setArquivo] = useState<File | null>(null);
  const [enviando, setEnviando] = useState(false);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setArquivo(e.target.files[0]);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!arquivo) {
      alert('Selecione um arquivo');
      return;
    }

    try {
      setEnviando(true);
      // Aqui você implementaria o upload real
      await new Promise((resolve) => setTimeout(resolve, 2000)); // Simulação
      alert('Insumo enviado com sucesso!');
      setArquivo(null);
    } catch (err) {
      console.error('Erro ao enviar insumo:', err);
      alert('Erro ao enviar insumo');
    } finally {
      setEnviando(false);
    }
  };

  return (
    <div className={styles.container}>
      <h2>6. Recebimento de Insumos dos Agentes</h2>

      <div className={styles.infoBox}>
        <h3>ℹ️ Sobre Insumos dos Agentes</h3>
        <p>
          Os agentes do setor elétrico (empresas geradoras, distribuidoras e transmissoras) devem submeter seus dados
          operacionais e previsões para a programação diária. Este módulo permite o recebimento, validação e
          processamento desses insumos.
        </p>
      </div>

      <div className={styles.form}>
        <h3>📤 Envio de Insumos</h3>
        <form onSubmit={handleSubmit}>
          <div className={styles.formGroup}>
            <label>Arquivo de Insumo:</label>
            <input
              type="file"
              accept=".xml,.csv,.xlsx"
              onChange={handleFileChange}
              required
            />
            {arquivo && (
              <p style={{ marginTop: '0.5rem', fontSize: '0.9rem', color: '#6b7280' }}>
                Arquivo selecionado: <strong>{arquivo.name}</strong> ({(arquivo.size / 1024).toFixed(2)} KB)
              </p>
            )}
          </div>

          <div className={styles.formGroup}>
            <label>Tipo de Insumo:</label>
            <select required>
              <option value="">Selecione...</option>
              <option value="geracao">Dados de Geração</option>
              <option value="carga">Previsão de Carga</option>
              <option value="disponibilidade">Disponibilidade de Máquinas</option>
              <option value="restricoes">Restrições Operativas</option>
              <option value="manutencao">Programação de Manutenção</option>
            </select>
          </div>

          <div className={styles.formGroup}>
            <label>Agente Submetedor:</label>
            <input type="text" placeholder="Nome do agente" required />
          </div>

          <div className={styles.formGroup}>
            <label>Observações:</label>
            <textarea rows={3} placeholder="Informações adicionais sobre o insumo..." />
          </div>

          <button type="submit" className={styles.btnSalvar} disabled={enviando}>
            {enviando ? '📤 Enviando...' : '📤 Enviar Insumo'}
          </button>
        </form>
      </div>

      <div style={{ marginTop: '2rem' }}>
        <h3>📋 Formatos Aceitos</h3>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '1rem', marginTop: '1rem' }}>
          <div style={{ background: '#f9fafb', padding: '1.5rem', borderRadius: '0.5rem', border: '1px solid #e5e7eb' }}>
            <h4 style={{ color: '#1e3a8a', marginBottom: '0.5rem' }}>📄 XML</h4>
            <p style={{ fontSize: '0.9rem', color: '#6b7280', margin: 0 }}>
              Formato padrão para intercâmbio de dados estruturados
            </p>
          </div>
          <div style={{ background: '#f9fafb', padding: '1.5rem', borderRadius: '0.5rem', border: '1px solid #e5e7eb' }}>
            <h4 style={{ color: '#1e3a8a', marginBottom: '0.5rem' }}>📊 CSV</h4>
            <p style={{ fontSize: '0.9rem', color: '#6b7280', margin: 0 }}>
              Dados tabulares separados por vírgula
            </p>
          </div>
          <div style={{ background: '#f9fafb', padding: '1.5rem', borderRadius: '0.5rem', border: '1px solid #e5e7eb' }}>
            <h4 style={{ color: '#1e3a8a', marginBottom: '0.5rem' }}>📈 Excel</h4>
            <p style={{ fontSize: '0.9rem', color: '#6b7280', margin: 0 }}>
              Planilhas Excel (.xlsx) com dados estruturados
            </p>
          </div>
        </div>
      </div>

      <div className={styles.infoBox} style={{ marginTop: '2rem' }}>
        <h3>🔄 Processo de Validação</h3>
        <ol style={{ paddingLeft: '1.5rem', color: '#374151' }}>
          <li>Recepção do arquivo submetido pelo agente</li>
          <li>Validação do formato e estrutura dos dados</li>
          <li>Verificação de consistência com período de referência</li>
          <li>Processamento e integração aos dados da programação</li>
          <li>Notificação ao agente sobre status da submissão</li>
        </ol>
      </div>
    </div>
  );
};

export default InsumosAgentes;

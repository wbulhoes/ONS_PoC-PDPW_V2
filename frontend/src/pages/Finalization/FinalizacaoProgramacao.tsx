import React, { useState } from 'react';
import styles from './FinalizacaoProgramacao.module.css';

const FinalizacaoProgramacao: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [loading, setLoading] = useState(false);
  const [status, setStatus] = useState<string | null>(null);

  const handleFinalize = async () => {
    if (!dataPdp) return;
    setLoading(true);
    setStatus(null);
    try {
      await new Promise((r) => setTimeout(r, 800));
      setStatus('Programação finalizada com sucesso');
    } catch (err) {
      setStatus('Erro ao finalizar programação');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="finalizacao-programacao">
      <h2>Finalização da Programação</h2>
      <div className={styles.form}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />
        <button onClick={handleFinalize} disabled={!dataPdp || loading}>Finalizar</button>
      </div>
      {status && <div className={styles.status}>{status}</div>}
    </div>
  );
};

export default FinalizacaoProgramacao;

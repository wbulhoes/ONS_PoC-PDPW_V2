import React, { useState } from 'react';
import styles from './GenerateModelFiles.module.css';

const GenerateModelFiles: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [dessem, setDessem] = useState(false);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  const handleGenerate = async () => {
    setLoading(true);
    setMessage(null);
    try {
      // mock generation
      await new Promise((r) => setTimeout(r, 800));
      setMessage('Arquivos gerados com sucesso');
    } catch (err) {
      setMessage('Erro ao gerar arquivos');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="generate-model-files">
      <h2>Gerar Arquivo Texto (Modelos)</h2>
      <div className={styles.form}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />
        <label>
          <input type="checkbox" checked={dessem} onChange={(e) => setDessem(e.target.checked)} /> DESSEM
        </label>
        <button onClick={handleGenerate} disabled={!dataPdp || loading}>Gerar</button>
      </div>

      {message && <div className={styles.message}>{message}</div>}
    </div>
  );
};

export default GenerateModelFiles;

import React, { useState } from 'react';
import styles from './PrevisaoEolica.module.css';
import { previsaoEolicaService } from '../../../services/previsaoEolicaService';

const PrevisaoEolica: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [parques, setParques] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const handleLoad = async () => {
    setLoading(true);
    try {
      const res = await previsaoEolicaService.getPrevisao(dataPdp.replace(/-/g, ''));
      setParques(res.parques || []);
    } finally { setLoading(false); }
  };

  const handleChange = (idx: number, field: string, value: string) => {
    const copy = [...parques];
    copy[idx] = { ...copy[idx], [field]: value };
    setParques(copy);
  };

  const handleSave = async () => {
    setLoading(true);
    try {
      await previsaoEolicaService.savePrevisao({ dataPdp, parques });
      alert('Previsão salva');
    } finally { setLoading(false); }
  };

  return (
    <div className={styles.container} data-testid="previsao-eolica-page">
      <h2>Previsão Eólica</h2>
      <div className={styles.controls}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />
        <button onClick={handleLoad} disabled={loading || !dataPdp}>Carregar</button>
      </div>

      <div className={styles.table}>
        <table>
          <thead>
            <tr><th>Código</th><th>Parque</th><th>Previsão (MW)</th></tr>
          </thead>
          <tbody>
            {parques.map((p, idx) => (
              <tr key={p.cod}>
                <td>{p.cod}</td>
                <td>{p.nome}</td>
                <td>
                  <input type="number" value={p.previsaoMW} onChange={(e) => handleChange(idx, 'previsaoMW', e.target.value)} />
                </td>
              </tr>
            ))}
            {parques.length === 0 && (
              <tr><td colSpan={3}>Nenhum dado</td></tr>
            )}
          </tbody>
        </table>
      </div>

      <div className={styles.actions}>
        <button onClick={handleSave} disabled={loading || parques.length===0}>Salvar</button>
      </div>
    </div>
  );
};

export default PrevisaoEolica;

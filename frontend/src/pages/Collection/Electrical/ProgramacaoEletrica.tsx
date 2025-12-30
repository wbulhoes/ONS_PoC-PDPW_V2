import React, { useState } from 'react';
import styles from './ProgramacaoEletrica.module.css';
import { programacaoEletricaService } from '../../../services/programacaoEletricaService';

const ProgramacaoEletrica: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [itens, setItens] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const handleLoad = async () => {
    setLoading(true);
    try {
      const res = await programacaoEletricaService.getProgramacaoEletrica(dataPdp.replace(/-/g, ''));
      setItens(res.itens || []);
    } finally { setLoading(false); }
  };

  const handleChange = (idx: number, field: string, value: string) => {
    const copy = [...itens];
    copy[idx] = { ...copy[idx], [field]: value };
    setItens(copy);
  };

  const handleSave = async () => {
    setLoading(true);
    try {
      await programacaoEletricaService.saveProgramacaoEletrica({ dataPdp, itens });
      alert('Programação elétrica salva');
    } finally { setLoading(false); }
  };

  return (
    <div className={styles.container} data-testid="programacao-eletrica-page">
      <h2>Programação Elétrica</h2>
      <div className={styles.controls}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />
        <button onClick={handleLoad} disabled={loading || !dataPdp}>Carregar</button>
      </div>

      <div className={styles.table}>
        <table>
          <thead>
            <tr><th>Código</th><th>Descrição</th><th>Valor</th></tr>
          </thead>
          <tbody>
            {itens.map((it, idx) => (
              <tr key={it.cod}>
                <td>{it.cod}</td>
                <td>{it.descricao}</td>
                <td>
                  <input type="number" value={it.valor} onChange={(e) => handleChange(idx, 'valor', e.target.value)} />
                </td>
              </tr>
            ))}
            {itens.length === 0 && (
              <tr><td colSpan={3}>Nenhum dado</td></tr>
            )}
          </tbody>
        </table>
      </div>

      <div className={styles.actions}>
        <button onClick={handleSave} disabled={loading || itens.length===0}>Salvar</button>
      </div>
    </div>
  );
};

export default ProgramacaoEletrica;

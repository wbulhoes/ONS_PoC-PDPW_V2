import React, { useState } from 'react';
import styles from './Insumos.module.css';
import { insumosService } from '../../../services/insumosService';

const Insumos: React.FC = () => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [insumos, setInsumos] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const handleLoad = async () => {
    setLoading(true);
    try {
      const res = await insumosService.getInsumos(dataPdp.replace(/-/g, ''));
      setInsumos(res.insumos || []);
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    setLoading(true);
    try {
      await insumosService.saveInsumos({ dataPdp, insumos });
      alert('Insumos salvos');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="insumos-page">
      <h2>Coleta de Insumos</h2>
      <div className={styles.controls}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />
        <button onClick={handleLoad} disabled={loading}>Carregar</button>
      </div>

      <div className={styles.list}>
        {insumos.length === 0 ? (
          <div>Nenhum insumo carregado</div>
        ) : (
          <table>
            <thead>
              <tr><th>ID</th><th>Nome</th><th>Valor</th></tr>
            </thead>
            <tbody>
              {insumos.map((i) => (
                <tr key={i.id}><td>{i.id}</td><td>{i.nome}</td><td>{i.valor}</td></tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      <div className={styles.actions}>
        <button onClick={handleSave} disabled={loading || insumos.length === 0}>Salvar</button>
      </div>
    </div>
  );
};

export default Insumos;

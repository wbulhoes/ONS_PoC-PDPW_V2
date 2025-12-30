import React, { useEffect, useState } from 'react';
import styles from './RROQuery.module.css';
import { rroService } from '../../../services/rroService';

interface RROItem {
  id: string;
  codUsina: string;
  descricao: string;
  dataPdp: string;
}

const RROQuery: React.FC = () => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [empresa, setEmpresa] = useState<string>('');
  const [items, setItems] = useState<RROItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // placeholder
  }, []);

  const handleSearch = async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await rroService.getOffers(dataPdp.replace(/-/g, ''), empresa);
      const itemsMapped: RROItem[] = (result.usinas || []).map((u, idx) => ({
        id: `${u.codUsina}-${idx}`,
        codUsina: u.codUsina,
        descricao: u.nomeUsina,
        dataPdp: result.dataPdp || dataPdp,
      }));
      setItems(itemsMapped);
    } catch (err) {
      console.error(err);
      setError('Erro ao consultar RRO');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="rro-query-container">
      <h2>Consulta RRO</h2>

      <div className={styles.filters}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />

        <label htmlFor="empresaSelect">Empresa</label>
        <select id="empresaSelect" value={empresa} onChange={(e) => setEmpresa(e.target.value)}>
          <option value="">Todas</option>
          <option value="EMP001">EMP001</option>
          <option value="EMP002">EMP002</option>
        </select>

        <button onClick={handleSearch} disabled={loading}>Buscar</button>
      </div>

      {error && <div className={styles.error}>{error}</div>}

      <div className={styles.table} data-testid="rro-results">
        <table>
          <thead>
            <tr>
              <th>Usina</th>
              <th>Descrição</th>
              <th>Data PDP</th>
            </tr>
          </thead>
          <tbody>
            {items.map(it => (
              <tr key={it.id}>
                <td>{it.codUsina}</td>
                <td>{it.descricao}</td>
                <td>{it.dataPdp}</td>
              </tr>
            ))}
            {items.length === 0 && (
              <tr>
                <td colSpan={3}>Nenhum registro encontrado</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default RROQuery;

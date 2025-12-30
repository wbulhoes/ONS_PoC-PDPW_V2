import React, { useEffect, useState } from 'react';
import styles from './ProgramacaoEnergetica.module.css';
import { programacaoService } from '../../../services/programacaoService';
import { ProgramacaoEnergetica, ProgramacaoUsina } from '../../../types/programacao';

const ProgramacaoEnergeticaPage: React.FC = () => {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [codEmpresa, setCodEmpresa] = useState<string>('');
  const [programacao, setProgramacao] = useState<ProgramacaoEnergetica | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    // noop
  }, []);

  const handleLoad = async () => {
    if (!dataPdp || !codEmpresa) return;
    setLoading(true);
    try {
      const res = await programacaoService.getProgramacao(dataPdp.replace(/-/g, ''), codEmpresa);
      setProgramacao(res);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleChangeUsina = (idx: number, field: keyof ProgramacaoUsina, value: string) => {
    if (!programacao) return;
    const copy = { ...programacao } as ProgramacaoEnergetica;
    const num = value === '' ? null : Number(value);
    (copy.usinas[idx] as any)[field] = num;
    setProgramacao(copy);
  };

  const handleSave = async () => {
    if (!programacao) return;
    setLoading(true);
    try {
      await programacaoService.saveProgramacao(programacao);
      alert('Programação salva com sucesso');
    } catch (err) {
      console.error(err);
      alert('Erro ao salvar');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="programacao-energetica-page">
      <h2>Cadastro Programação Energética</h2>

      <div className={styles.controls}>
        <label htmlFor="dataPdpInput">Data PDP</label>
        <input id="dataPdpInput" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} />

        <label htmlFor="empresaSelect">Empresa</label>
        <select id="empresaSelect" value={codEmpresa} onChange={(e) => setCodEmpresa(e.target.value)}>
          <option value="">Selecione...</option>
          <option value="EMP001">EMP001</option>
          <option value="EMP002">EMP002</option>
        </select>

        <button onClick={handleLoad} disabled={loading || !dataPdp || !codEmpresa}>Carregar</button>
      </div>

      {programacao ? (
        <div className={styles.tableWrapper}>
          <table>
            <thead>
              <tr>
                <th>Usina</th>
                <th>Volume Programação</th>
                <th>Preço Programação</th>
              </tr>
            </thead>
            <tbody>
              {programacao.usinas.map((u, idx) => (
                <tr key={u.codUsina}>
                  <td>{u.nomeUsina || u.codUsina}</td>
                  <td>
                    <input
                      type="number"
                      value={u.volumeProgramacao ?? ''}
                      onChange={(e) => handleChangeUsina(idx, 'volumeProgramacao', e.target.value)}
                    />
                  </td>
                  <td>
                    <input
                      type="number"
                      step="0.01"
                      value={u.precoProgramacao ?? ''}
                      onChange={(e) => handleChangeUsina(idx, 'precoProgramacao', e.target.value)}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className={styles.actions}>
            <button onClick={handleSave} disabled={loading}>Salvar</button>
          </div>
        </div>
      ) : (
        <div>Nenhuma programação carregada.</div>
      )}
    </div>
  );
};

export default ProgramacaoEnergeticaPage;

/**
 * Componente: RRO (Registro de Restrição Operativa)
 * Migração de: legado/pdpw/frmColRRO.aspx
 *
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Seleção de Empresa
 * - Seleção de Usina (individual ou todas)
 * - Edição de valores de RRO por intervalo (48 intervalos de 30min)
 * - Salvamento de dados
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './RRO.module.css';
import type {
  RROData,
  RROForm,
} from '../../../types/rro';
import { rroService } from '../../../services/rroService';
import { SelectOption } from '../../../types/exportOffer'; // Reusing SelectOption

const RRO: React.FC = () => {
  const [form, setForm] = useState<RROForm>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
  });

  const [data, setData] = useState<RROData | null>(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  // Opções de Data PDP (mock)
  const datasPdp = useMemo(() => {
    const dates: SelectOption[] = [];
    const today = new Date();
    for (let i = -5; i <= 5; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      const formatted = date.toISOString().split('T')[0].replace(/-/g, '');
      const display = date.toLocaleDateString('pt-BR');
      dates.push({ value: formatted, label: display });
    }
    return dates;
  }, []);

  // Opções de Empresas (mock)
  const empresas = useMemo(
    () => [
      { value: 'EMP001', label: 'Empresa Termelétrica A' },
      { value: 'EMP002', label: 'Empresa Termelétrica B' },
      { value: 'EMP003', label: 'Empresa Termelétrica C' },
    ],
    []
  );

  // Opções de Usinas
  const usinas = useMemo(() => {
    if (!data || !data.usinas) return [];
    const options = data.usinas.map((u) => ({
      value: u.codUsina,
      label: `${u.codUsina} - ${u.nomeUsina}`,
    }));
    if (options.length > 1) {
      options.push({ value: 'TODAS', label: 'Todas as Usinas' });
    }
    return [{ value: '', label: 'Selecione uma Usina' }, ...options];
  }, [data]);

  useEffect(() => {
    if (form.dataPdp && form.codEmpresa) {
      loadData();
    }
  }, [form.dataPdp, form.codEmpresa]);

  const loadData = async () => {
    setLoading(true);
    setMessage(null);
    try {
      // If codUsina is 'TODAS', we might want to fetch all, or handle it in the service
      // For now, passing undefined if 'TODAS' or empty
      const usinaParam = form.codUsina === 'TODAS' ? undefined : form.codUsina;
      const result = await rroService.getOffers(form.dataPdp, form.codEmpresa, usinaParam);
      setData(result);
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao carregar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    if (!data) return;
    setLoading(true);
    try {
      await rroService.saveOffers(data);
      setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao salvar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleValueChange = (codUsina: string, intervalo: number, newValue: string) => {
    if (!data) return;
    const numValue = parseFloat(newValue);
    if (isNaN(numValue)) return;

    const newData = { ...data };
    const usinaIndex = newData.usinas.findIndex((u) => u.codUsina === codUsina);
    if (usinaIndex >= 0) {
      const intervaloIndex = newData.usinas[usinaIndex].intervalos.findIndex(
        (i) => i.intervalo === intervalo
      );
      if (intervaloIndex >= 0) {
        newData.usinas[usinaIndex].intervalos[intervaloIndex].valor = numValue;
        setData(newData);
      }
    }
  };

  // Filter displayed usinas based on selection
  const displayedUsinas = useMemo(() => {
    if (!data) return [];
    if (!form.codUsina || form.codUsina === 'TODAS') return data.usinas;
    return data.usinas.filter((u) => u.codUsina === form.codUsina);
  }, [data, form.codUsina]);

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>RRO - Registro de Restrição Operativa</h2>

      <div className={styles.filters}>
        <div className={styles.filterGroup}>
          <label htmlFor="dataPdp">Data PDP:</label>
          <select
            id="dataPdp"
            value={form.dataPdp}
            onChange={(e) => setForm({ ...form, dataPdp: e.target.value })}
            className={styles.select}
          >
            <option value="">Selecione...</option>
            {datasPdp.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="codEmpresa">Empresa:</label>
          <select
            id="codEmpresa"
            value={form.codEmpresa}
            onChange={(e) => setForm({ ...form, codEmpresa: e.target.value })}
            className={styles.select}
          >
            <option value="">Selecione...</option>
            {empresas.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.filterGroup}>
          <label htmlFor="codUsina">Usina:</label>
          <select
            id="codUsina"
            value={form.codUsina}
            onChange={(e) => setForm({ ...form, codUsina: e.target.value })}
            className={styles.select}
            disabled={!data}
          >
            {usinas.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>
      </div>

      {message && (
        <div className={`${styles.message} ${message.type === 'error' ? styles.error : styles.success}`}>
          {message.text}
        </div>
      )}

      {loading && <div className={styles.loading}>Carregando...</div>}

      {data && displayedUsinas.length > 0 && (
        <div className={styles.content}>
          <div className={styles.actions}>
            <button onClick={handleSave} className={styles.button} disabled={loading}>
              Salvar
            </button>
          </div>

          <div className={styles.tableContainer}>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Intervalo</th>
                  {displayedUsinas.map((usina) => (
                    <th key={usina.codUsina}>{usina.nomeUsina}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {Array.from({ length: 48 }, (_, i) => i + 1).map((intervalo) => (
                  <tr key={intervalo}>
                    <td>{intervalo}</td>
                    {displayedUsinas.map((usina) => {
                      const val = usina.intervalos.find((i) => i.intervalo === intervalo)?.valor ?? 0;
                      return (
                        <td key={`${usina.codUsina}-${intervalo}`}>
                          <input
                            type="number"
                            value={val}
                            onChange={(e) =>
                              handleValueChange(usina.codUsina, intervalo, e.target.value)
                            }
                            className={styles.input}
                          />
                        </td>
                      );
                    })}
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default RRO;

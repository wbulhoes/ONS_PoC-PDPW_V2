/**
 * Componente: Restrição por Falta de Combustível
 * Migração de: legado/pdpw/frmColResFaltaComb.aspx
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './FuelShortageRestriction.module.css';
import { FuelShortageData, FuelShortageInterval } from '../../../types/fuelShortage';
import { fuelShortageService } from '../../../services/fuelShortageService';

const FuelShortageRestriction: React.FC = () => {
  const [date, setDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [companyId, setCompanyId] = useState<string>('');
  const [usinaId, setUsinaId] = useState<string>('all');
  const [data, setData] = useState<FuelShortageData | null>(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);

  // Mock Empresas
  const empresas = useMemo(
    () => [
      { value: 'EMP001', label: 'Empresa Termelétrica A' },
      { value: 'EMP002', label: 'Empresa Termelétrica B' },
    ],
    []
  );

  useEffect(() => {
    if (companyId && date) {
      loadData();
    }
  }, [companyId, date]);

  const loadData = async () => {
    setLoading(true);
    setMessage(null);
    try {
      const result = await fuelShortageService.getData(date, companyId);
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
      await fuelShortageService.saveData(data, date, companyId);
      setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao salvar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleValueChange = (intervalId: number, codUsina: string, value: string) => {
    if (!data) return;
    const numValue = parseFloat(value);
    if (isNaN(numValue) && value !== '') return;

    const newData = { ...data };
    const intervalIndex = newData.intervalos.findIndex(i => i.id === intervalId);
    if (intervalIndex === -1) return;

    newData.intervalos[intervalIndex].valores[codUsina] = value === '' ? 0 : numValue;
    
    // Recalculate row total
    let rowTotal = 0;
    data.usinas.forEach(u => {
      rowTotal += newData.intervalos[intervalIndex].valores[u.codUsina] || 0;
    });
    newData.intervalos[intervalIndex].total = rowTotal;

    setData(newData);
  };

  const filteredUsinas = useMemo(() => {
    if (!data) return [];
    if (usinaId === 'all') return data.usinas;
    return data.usinas.filter(u => u.codUsina === usinaId);
  }, [data, usinaId]);

  const calculateColumnTotal = (codUsina: string) => {
    if (!data) return 0;
    return data.intervalos.reduce((acc, curr) => acc + (curr.valores[codUsina] || 0), 0);
  };

  const calculateColumnAverage = (codUsina: string) => {
    if (!data) return 0;
    const total = calculateColumnTotal(codUsina);
    return total / 48;
  };

  const calculateGrandTotal = () => {
    if (!data) return 0;
    return data.intervalos.reduce((acc, curr) => acc + curr.total, 0);
  };

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>Restrição por Falta de Combustível</h2>

      <div className={styles.filters}>
        <div className={styles.filterRow}>
          <label htmlFor="date">Data PDP:</label>
          <input
            type="date"
            id="date"
            value={date}
            onChange={(e) => setDate(e.target.value)}
            className={styles.input}
          />
        </div>

        <div className={styles.filterRow}>
          <label htmlFor="company">Empresa:</label>
          <select
            id="company"
            value={companyId}
            onChange={(e) => setCompanyId(e.target.value)}
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

        {data && (
          <div className={styles.filterRow}>
            <label htmlFor="usina">Usina:</label>
            <select
              id="usina"
              value={usinaId}
              onChange={(e) => setUsinaId(e.target.value)}
              className={styles.select}
            >
              <option value="all">Todas as Usinas</option>
              {data.usinas.map((u) => (
                <option key={u.codUsina} value={u.codUsina}>
                  {u.nomeUsina || u.codUsina}
                </option>
              ))}
            </select>
          </div>
        )}
      </div>

      {message && (
        <div className={`${styles.message} ${message.type === 'error' ? styles.error : styles.success}`}>
          {message.text}
        </div>
      )}

      {loading && <div className={styles.loading}>Carregando...</div>}

      {data && (
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
                  <th>Total</th>
                  {filteredUsinas.map(u => (
                    <th key={u.codUsina}>{u.nomeUsina || u.codUsina}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {data.intervalos.map((interval) => (
                  <tr key={interval.id}>
                    <td>{interval.hora}</td>
                    <td>{interval.total.toFixed(2)}</td>
                    {filteredUsinas.map(u => (
                      <td key={u.codUsina}>
                        <input
                          type="number"
                          value={interval.valores[u.codUsina] || 0}
                          onChange={(e) => handleValueChange(interval.id, u.codUsina, e.target.value)}
                          className={styles.inputNumber}
                        />
                      </td>
                    ))}
                  </tr>
                ))}
                <tr className={styles.totalRow}>
                  <td>Total</td>
                  <td>{calculateGrandTotal().toFixed(2)}</td>
                  {filteredUsinas.map(u => (
                    <td key={u.codUsina}>{calculateColumnTotal(u.codUsina).toFixed(2)}</td>
                  ))}
                </tr>
                <tr className={styles.totalRow}>
                  <td>Média</td>
                  <td>{(calculateGrandTotal() / 48).toFixed(2)}</td>
                  {filteredUsinas.map(u => (
                    <td key={u.codUsina}>{calculateColumnAverage(u.codUsina).toFixed(2)}</td>
                  ))}
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default FuelShortageRestriction;

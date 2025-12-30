/**
 * Componente: Despacho de Inflexibilidade Térmica
 * Migração de: legado/pdpw/frmColDespInflex.aspx
 *
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Seleção de Empresa
 * - Seleção de Usina (individual ou todas)
 * - Edição de valores de despacho de inflexibilidade por intervalo (48 intervalos de 30min)
 * - Salvamento de dados
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './InflexibilityDispatch.module.css';
import type {
  DespachoInflexibilidadeData,
  DespachoInflexibilidadeForm,
  DespachoInflexibilidadeIntervalo,
  DespachoInflexibilidadeUsina,
} from '../../../types/inflexibilityDispatch';

interface InflexibilityDispatchProps {
  onSave?: (data: DespachoInflexibilidadeData) => Promise<void>;
  onLoadData?: (dataPdp: string, codEmpresa: string) => Promise<DespachoInflexibilidadeData>;
}

const InflexibilityDispatch: React.FC<InflexibilityDispatchProps> = ({ onSave, onLoadData }) => {
  const [form, setForm] = useState<DespachoInflexibilidadeForm>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
  });

  const [data, setData] = useState<DespachoInflexibilidadeData | null>(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [textareaValue, setTextareaValue] = useState('');
  const [textareaVisible, setTextareaVisible] = useState(false);

  // Opções de Data PDP (mock - virá do backend)
  const datasPdp = useMemo(() => {
    const dates = [];
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

  // Opções de Empresas (mock - virá do backend)
  const empresas = useMemo(
    () => [
      { value: 'EMP001', label: 'Empresa Termelétrica A' },
      { value: 'EMP002', label: 'Empresa Termelétrica B' },
      { value: 'EMP003', label: 'Empresa Termelétrica C' },
    ],
    []
  );

  // Opções de Usinas filtradas por empresa
  const usinas = useMemo(() => {
    if (!data || !data.usinas) return [];
    const options = data.usinas.map((u) => ({ value: u.codUsina, label: u.codUsina }));
    if (options.length > 1) {
      options.push({ value: 'TODAS', label: 'Todas as Usinas' });
    }
    return [{ value: '', label: 'Selecione uma Usina' }, ...options];
  }, [data]);

  // Gerar array de 48 intervalos de 30 minutos
  const intervalos = useMemo(() => {
    const arr = [];
    for (let i = 1; i <= 48; i++) {
      const hour = Math.floor((i - 1) / 2);
      const minute = (i - 1) % 2 === 0 ? '00' : '30';
      const nextHour = Math.floor(i / 2);
      const nextMinute = i % 2 === 0 ? '00' : '30';
      arr.push({
        numero: i,
        label: `${String(hour).padStart(2, '0')}:${minute}-${String(nextHour).padStart(2, '0')}:${nextMinute}`,
      });
    }
    return arr;
  }, []);

  // Carregar dados quando data e empresa são selecionadas
  useEffect(() => {
    if (form.dataPdp && form.codEmpresa && onLoadData) {
      setLoading(true);
      setMessage(null);
      onLoadData(form.dataPdp, form.codEmpresa)
        .then((result) => {
          setData(result);
          setForm((prev) => ({ ...prev, codUsina: '' }));
          setTextareaVisible(false);
        })
        .catch(() => {
          setMessage({ type: 'error', text: 'Erro ao carregar dados' });
        })
        .finally(() => {
          setLoading(false);
        });
    }
  }, [form.dataPdp, form.codEmpresa, onLoadData]);

  // Atualizar textarea quando usina é selecionada
  useEffect(() => {
    if (!form.codUsina || form.codUsina === '' || !data) {
      setTextareaVisible(false);
      return;
    }

    if (form.codUsina === 'TODAS') {
      // Modo: Todas as usinas (grid com TABs separando usinas)
      const lines = intervalos.map((intervalo) => {
        const valores = data.usinas.map((usina) => {
          const int = usina.intervalos.find((i) => i.intervalo === intervalo.numero);
          return int ? int.valor : 0;
        });
        return valores.join('\t');
      });
      setTextareaValue(lines.join('\n'));
    } else {
      // Modo: Usina individual (valores em linhas)
      const usina = data.usinas.find((u) => u.codUsina === form.codUsina);
      if (usina) {
        const valores = intervalos.map((intervalo) => {
          const int = usina.intervalos.find((i) => i.intervalo === intervalo.numero);
          return int ? int.valor : 0;
        });
        setTextareaValue(valores.join('\n'));
      }
    }

    setTextareaVisible(true);
  }, [form.codUsina, data, intervalos]);

  const handleDataPdpChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newDataPdp = e.target.value;

    // Se a data mudou, limpar empresa e dados
    if (newDataPdp !== form.dataPdp) {
      setForm({ dataPdp: newDataPdp, codEmpresa: '', codUsina: '' });
      setData(null);
      setTextareaVisible(false);
    }
  };

  const handleEmpresaChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setForm({ ...form, codEmpresa: e.target.value, codUsina: '' });
    setData(null);
    setTextareaVisible(false);
  };

  const handleUsinaChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setForm({ ...form, codUsina: e.target.value });
  };

  const handleTextareaChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setTextareaValue(e.target.value);
  };

  const handleSave = async () => {
    if (!form.dataPdp || !form.codEmpresa || !form.codUsina || form.codUsina === '') {
      setMessage({ type: 'error', text: 'Preencha todos os campos' });
      return;
    }

    if (!data) {
      setMessage({ type: 'error', text: 'Nenhum dado disponível' });
      return;
    }

    setLoading(true);
    setMessage(null);

    try {
      // Parsear valores do textarea
      const lines = textareaValue.split('\n');

      if (form.codUsina === 'TODAS') {
        // Modo: Todas as usinas
        const updatedUsinas: DespachoInflexibilidadeUsina[] = data.usinas.map(
          (usina, usinaIndex) => {
            const intervalos: DespachoInflexibilidadeIntervalo[] = lines.map((line, lineIndex) => {
              const valores = line.split('\t');
              const valor = valores[usinaIndex] ? parseFloat(valores[usinaIndex]) || 0 : 0;
              return { intervalo: lineIndex + 1, valor };
            });
            return { codUsina: usina.codUsina, intervalos };
          }
        );

        const updatedData: DespachoInflexibilidadeData = {
          ...data,
          usinas: updatedUsinas,
        };

        if (onSave) {
          await onSave(updatedData);
        }
      } else {
        // Modo: Usina individual
        const intervalos: DespachoInflexibilidadeIntervalo[] = lines.map((line, index) => ({
          intervalo: index + 1,
          valor: parseFloat(line) || 0,
        }));

        const updatedUsinas = data.usinas.map((usina) => {
          if (usina.codUsina === form.codUsina) {
            return { ...usina, intervalos };
          }
          return usina;
        });

        const updatedData: DespachoInflexibilidadeData = {
          ...data,
          usinas: updatedUsinas,
        };

        if (onSave) {
          await onSave(updatedData);
        }
      }

      setMessage({ type: 'success', text: 'Dados salvos com sucesso' });
      setForm({ ...form, codUsina: '' });
      setTextareaVisible(false);
    } catch (error) {
      setMessage({ type: 'error', text: 'Erro ao salvar dados' });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h2 className={styles.title}>Despacho de Inflexibilidade</h2>
      </div>

      {message && <div className={`${styles.message} ${styles[message.type]}`}>{message.text}</div>}

      <div className={styles.form}>
        <div className={styles.formRow}>
          <label htmlFor="data-pdp" className={styles.label}>
            <strong>Data PDP:</strong>
          </label>
          <select
            id="data-pdp"
            value={form.dataPdp}
            onChange={handleDataPdpChange}
            className={styles.select}
            disabled={loading}
          >
            <option value="">Selecione uma Data</option>
            {datasPdp.map((d) => (
              <option key={d.value} value={d.value}>
                {d.label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="empresa" className={styles.label}>
            <strong>Empresa:</strong>
          </label>
          <select
            id="empresa"
            value={form.codEmpresa}
            onChange={handleEmpresaChange}
            className={styles.select}
            disabled={loading || !form.dataPdp}
          >
            <option value="">Selecione uma Empresa</option>
            {empresas.map((e) => (
              <option key={e.value} value={e.value}>
                {e.label}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="usina" className={styles.label}>
            <strong>Usinas:</strong>
          </label>
          <select
            id="usina"
            value={form.codUsina}
            onChange={handleUsinaChange}
            className={styles.select}
            disabled={loading || !data || usinas.length === 0}
          >
            {usinas.map((u) => (
              <option key={u.value} value={u.value}>
                {u.label}
              </option>
            ))}
          </select>
          {textareaVisible && (
            <button
              id="salvar-button"
              onClick={handleSave}
              disabled={loading}
              className={styles.saveButton}
            >
              {loading ? 'Salvando...' : 'Salvar'}
            </button>
          )}
        </div>
      </div>

      {data && data.usinas && data.usinas.length > 0 && (
        <div className={styles.tableContainer}>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Intervalo</th>
                {data.usinas.map((usina) => (
                  <th
                    key={usina.codUsina}
                    className={form.codUsina === usina.codUsina ? styles.selectedColumn : ''}
                  >
                    {usina.codUsina}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {intervalos.map((intervalo) => (
                <tr key={intervalo.numero}>
                  <td className={styles.intervaloCell}>{intervalo.label}</td>
                  {data.usinas.map((usina) => {
                    const int = usina.intervalos.find((i) => i.intervalo === intervalo.numero);
                    const valor = int ? int.valor : 0;
                    return (
                      <td key={usina.codUsina} className={styles.valueCell}>
                        {valor}
                      </td>
                    );
                  })}
                </tr>
              ))}
            </tbody>
          </table>

          {textareaVisible && (
            <textarea
              id="textarea-edit"
              value={textareaValue}
              onChange={handleTextareaChange}
              className={styles.textarea}
              rows={48}
              disabled={loading}
              style={{
                width: form.codUsina === 'TODAS' ? `${data.usinas.length * 65 + 16}px` : '81px',
              }}
            />
          )}
        </div>
      )}

      {loading && <div className={styles.loading}>Carregando...</div>}
    </div>
  );
};

export default InflexibilityDispatch;

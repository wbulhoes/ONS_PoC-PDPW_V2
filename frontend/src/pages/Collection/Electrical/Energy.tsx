/**
 * Componente: Coleta de Dados Energéticos (Razão Energética Transformada)
 * Migração de: legado/pdpw/frmColEnergetica.aspx
 *
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Seleção de Empresa
 * - Seleção de Usina (individual ou todas)
 * - Visualização em tabela com totais por intervalo
 * - Edição de valores de razão energética por intervalo (48 intervalos de 30min)
 * - Cálculo automático de totais e médias
 * - Salvamento de dados
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './Energy.module.css';
import type {
  DadosEnergeticosData,
  EnergeticFormData,
  TotalIntervalo,
} from '../../../types/energetic';
import type { SelectOption } from '../../../types/electrical'; // Reusing SelectOption

interface EnergyProps {
  onSave?: (data: DadosEnergeticosData) => Promise<void>;
  onLoadData?: (dataPdp: string, codEmpresa: string) => Promise<DadosEnergeticosData>;
}

const Energy: React.FC<EnergyProps> = ({ onSave, onLoadData }) => {
  const [form, setForm] = useState<EnergeticFormData>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
  });

  const [data, setData] = useState<DadosEnergeticosData | null>(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [textareaValue, setTextareaValue] = useState('');
  const [textareaVisible, setTextareaVisible] = useState(false);

  // Opções de Data PDP (mock - virá do backend)
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

  // Opções de Empresas (mock - virá do backend)
  const empresas = useMemo(
    () => [
      { value: 'EMP001', label: 'Empresa Hidrelétrica A' },
      { value: 'EMP002', label: 'Empresa Hidrelétrica B' },
      { value: 'EMP003', label: 'Empresa Hidrelétrica C' },
    ],
    []
  );

  // Opções de Usinas filtradas por empresa
  const usinas = useMemo(() => {
    if (!data || !data.usinas) return [];
    const options = data.usinas.map((u) => ({
      value: u.codUsina,
      label: u.codUsina,
    }));
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

  // Calcular totais por intervalo
  const totaisPorIntervalo = useMemo((): TotalIntervalo[] => {
    if (!data || !data.usinas) return [];

    return intervalos.map((intervalo) => {
      const total = data.usinas.reduce((sum, usina) => {
        const int = usina.intervalos.find((i) => i.intervalo === intervalo.numero);
        return sum + (int ? int.valRazaoEnerTran : 0);
      }, 0);

      return {
        intervalo: intervalo.numero,
        horario: intervalo.label,
        total,
      };
    });
  }, [data, intervalos]);

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
          return int ? int.valRazaoEnerTran : 0;
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
          return int ? int.valRazaoEnerTran : 0;
        });
        setTextareaValue(valores.join('\n'));
      }
    }

    setTextareaVisible(true);
  }, [form.codUsina, data, intervalos]);

  const handleDataPdpChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newDataPdp = e.target.value;

    if (newDataPdp !== form.dataPdp) {
      setForm({ dataPdp: newDataPdp, codEmpresa: '', codUsina: '' });
      setData(null);
      setTextareaVisible(false);
    }
  };

  const handleEmpresaChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newCodEmpresa = e.target.value;

    if (newCodEmpresa !== form.codEmpresa) {
      setForm((prev) => ({ ...prev, codEmpresa: newCodEmpresa, codUsina: '' }));
      setTextareaVisible(false);
    }
  };

  const handleUsinaChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newCodUsina = e.target.value;
    setForm((prev) => ({ ...prev, codUsina: newCodUsina }));
  };

  const handleTextareaChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setTextareaValue(e.target.value);
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    // Apenas números, Enter, Backspace e Tab
    const allowedKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'Enter', 'Backspace', 'Tab', '.', '-'];
    if (!allowedKeys.includes(e.key)) {
      e.preventDefault();
    }
  };

  const handleSave = async () => {
    if (!data || !form.codUsina) {
      setMessage({ type: 'error', text: 'Selecione uma usina para salvar' });
      return;
    }

    setLoading(true);
    setMessage(null);

    try {
      const lines = textareaValue.split('\n');

      if (form.codUsina === 'TODAS') {
        const updatedData = { ...data };

        lines.forEach((line, lineIndex) => {
          if (lineIndex >= 48) return;

          const valores = line.split('\t');

          valores.forEach((valor, usinaIndex) => {
            if (usinaIndex < data.usinas.length) {
              const usina = updatedData.usinas[usinaIndex];
              const intervalo = usina.intervalos.find((i) => i.intervalo === lineIndex + 1);

              if (intervalo) {
                intervalo.valRazaoEnerTran = parseFloat(valor) || 0;
              }
            }
          });
        });

        if (onSave) {
          await onSave(updatedData);
        }
        setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
      } else {
        const updatedData = { ...data };
        const usina = updatedData.usinas.find((u) => u.codUsina === form.codUsina);

        if (usina) {
          lines.forEach((line, index) => {
            if (index < 48) {
              const intervalo = usina.intervalos.find((i) => i.intervalo === index + 1);
              if (intervalo) {
                intervalo.valRazaoEnerTran = parseFloat(line) || 0;
              }
            }
          });

          if (onSave) {
            await onSave(updatedData);
          }
          setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
        }
      }
    } catch (error) {
      setMessage({ type: 'error', text: 'Erro ao salvar dados' });
    } finally {
      setLoading(false);
    }
  };

  const calculateUsinaTotal = (usina: any): number => {
    return usina.intervalos.reduce((sum: number, int: any) => sum + int.valRazaoEnerTran, 0);
  };

  const calculateUsinaMedia = (usina: any): number => {
    const total = calculateUsinaTotal(usina);
    return total / 48;
  };

  return (
    <div className={styles.container} data-testid="energy-container">
      <div className={styles.header}>
        <h1 data-testid="page-title">Razão Energética Transformada</h1>
        <p className={styles.subtitle} data-testid="page-subtitle">
          Coleta de dados energéticos de usinas
        </p>
      </div>

      {message && (
        <div
          className={`${styles.message} ${message.type === 'success' ? styles.messageSuccess : styles.messageError}`}
          data-testid={`message-${message.type}`}
        >
          {message.text}
        </div>
      )}

      <div className={styles.formSection}>
        <div className={styles.formRow}>
          <div className={styles.formGroup}>
            <label htmlFor="data-pdp" data-testid="label-data-pdp">
              Data PDP:
            </label>
            <select
              id="data-pdp"
              data-testid="select-data-pdp"
              value={form.dataPdp}
              onChange={handleDataPdpChange}
              className={styles.select}
              disabled={loading}
            >
              <option value="">Selecione uma data</option>
              {datasPdp.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="empresa" data-testid="label-empresa">
              Empresa:
            </label>
            <select
              id="empresa"
              data-testid="select-empresa"
              value={form.codEmpresa}
              onChange={handleEmpresaChange}
              className={styles.select}
              disabled={loading || !form.dataPdp}
            >
              <option value="">Selecione uma empresa</option>
              {empresas.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="usina" data-testid="label-usina">
              Usina:
            </label>
            <select
              id="usina"
              data-testid="select-usina"
              value={form.codUsina}
              onChange={handleUsinaChange}
              className={styles.select}
              disabled={loading || !data || usinas.length === 0}
            >
              {usinas.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {loading && (
        <div className={styles.loadingOverlay} data-testid="loading-indicator">
          <div className={styles.spinner}></div>
        </div>
      )}

      {data && !loading && (
        <div className={styles.dataSection}>
          <div className={styles.tableContainer}>
            <table className={styles.table} data-testid="data-table">
              <thead>
                <tr>
                  <th data-testid="th-intervalo">Intervalo</th>
                  <th data-testid="th-total">Total</th>
                  {data.usinas.map((usina, index) => (
                    <th
                      key={usina.codUsina}
                      data-testid={`th-usina-${index}`}
                      className={usina.codUsina === form.codUsina ? styles.selectedColumn : ''}
                    >
                      {usina.codUsina}
                    </th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {intervalos.map((intervalo, idx) => {
                  const totalIntervalo = totaisPorIntervalo[idx]?.total || 0;

                  return (
                    <tr key={intervalo.numero} data-testid={`row-intervalo-${intervalo.numero}`}>
                      <td className={styles.intervalCell} data-testid={`cell-label-${intervalo.numero}`}>
                        {intervalo.label}
                      </td>
                      <td className={styles.totalCell} data-testid={`cell-total-${intervalo.numero}`}>
                        {totalIntervalo.toFixed(2)}
                      </td>
                      {data.usinas.map((usina, usinaIdx) => {
                        const int = usina.intervalos.find((i) => i.intervalo === intervalo.numero);
                        const valor = int ? int.valRazaoEnerTran : 0;

                        return (
                          <td
                            key={`${usina.codUsina}-${intervalo.numero}`}
                            data-testid={`cell-value-${usinaIdx}-${intervalo.numero}`}
                            className={usina.codUsina === form.codUsina ? styles.selectedColumn : ''}
                          >
                            {valor.toFixed(2)}
                          </td>
                        );
                      })}
                    </tr>
                  );
                })}
                {/* Linha de Total */}
                <tr className={styles.totalRow} data-testid="row-total">
                  <td className={styles.totalLabel} data-testid="cell-total-label">
                    Total
                  </td>
                  <td className={styles.totalCell} data-testid="cell-total-geral">
                    {totaisPorIntervalo.reduce((sum, t) => sum + t.total, 0).toFixed(2)}
                  </td>
                  {data.usinas.map((usina, idx) => (
                    <td
                      key={`total-${usina.codUsina}`}
                      className={usina.codUsina === form.codUsina ? styles.selectedColumn : ''}
                      data-testid={`cell-usina-total-${idx}`}
                    >
                      {calculateUsinaTotal(usina).toFixed(2)}
                    </td>
                  ))}
                </tr>
                {/* Linha de Média */}
                <tr className={styles.totalRow} data-testid="row-media">
                  <td className={styles.totalLabel} data-testid="cell-media-label">
                    Média
                  </td>
                  <td className={styles.totalCell} data-testid="cell-media-geral">
                    {(totaisPorIntervalo.reduce((sum, t) => sum + t.total, 0) / 48).toFixed(2)}
                  </td>
                  {data.usinas.map((usina, idx) => (
                    <td
                      key={`media-${usina.codUsina}`}
                      className={usina.codUsina === form.codUsina ? styles.selectedColumn : ''}
                      data-testid={`cell-usina-media-${idx}`}
                    >
                      {calculateUsinaMedia(usina).toFixed(2)}
                    </td>
                  ))}
                </tr>
              </tbody>
            </table>
          </div>

          {textareaVisible && (
            <div className={styles.textareaContainer}>
              <div className={styles.editHeader}>
                <h2 data-testid="edit-section-title">
                  {form.codUsina === 'TODAS'
                    ? 'Edição de Todas as Usinas (separadas por TAB)'
                    : `Edição de Razão Energética: ${form.codUsina}`}
                </h2>
              </div>

              <textarea
                data-testid="textarea-values"
                className={styles.textarea}
                value={textareaValue}
                onChange={handleTextareaChange}
                onKeyPress={handleKeyPress}
                rows={48}
                spellCheck={false}
                placeholder="Digite os valores..."
              />

              <div className={styles.actions}>
                <button
                  type="button"
                  onClick={handleSave}
                  disabled={loading}
                  className={`${styles.button} ${styles.buttonPrimary}`}
                  data-testid="btn-save"
                >
                  {loading ? 'Salvando...' : 'Salvar Dados'}
                </button>
              </div>
            </div>
          )}
        </div>
      )}

      {!loading && !data && form.dataPdp && form.codEmpresa && (
        <div className={styles.message} data-testid="empty-state">
          <p>Não há dados disponíveis para a data e empresa selecionadas.</p>
        </div>
      )}
    </div>
  );
};

export default Energy;

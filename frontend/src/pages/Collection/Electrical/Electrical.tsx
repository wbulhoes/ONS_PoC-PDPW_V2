/**
 * Componente: Coleta de Dados Elétricos (Razão Elétrica Transformada)
 * Migração de: legado/pdpw/frmColEletrica.aspx
 *
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Seleção de Empresa
 * - Seleção de Usina (individual ou todas)
 * - Visualização em tabela com totais por intervalo
 * - Edição de valores de razão elétrica por intervalo (48 intervalos de 30min)
 * - Cálculo automático de totais e médias
 * - Salvamento de dados
 * - Integração com APIs reais do backend via React Query hooks
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './Electrical.module.css';
import type {
  DadosEletricosData,
  DadosEletricosForm,
  SelectOption,
  TotalIntervalo,
} from '../../../types/electrical';
import { useCompanies } from '../../../hooks/useCompanies';
import { usePlantsByCompany } from '../../../hooks/usePlants';
import { useElectricalDataByPeriod, useBulkUpsertElectricalData } from '../../../hooks/useElectricalData';
import { type CreateDadoEletricoDto } from '../../../services/electricalService';

const Electrical: React.FC = () => {
  const [form, setForm] = useState<DadosEletricosForm>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
  });

  const [data, setData] = useState<DadosEletricosData | null>(null);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [textareaValue, setTextareaValue] = useState('');
  const [textareaVisible, setTextareaVisible] = useState(false);
  const [selectedCompanyId, setSelectedCompanyId] = useState<string | null>(null);

  // React Query hooks
  const { data: empresas = [], isLoading: loadingEmpresas } = useCompanies();
  const { data: usinas = [], isLoading: loadingUsinas } = usePlantsByCompany(selectedCompanyId || undefined);
  const electricalDataQuery = useElectricalDataByPeriod(
    form.dataPdp,
    form.dataPdp
  );
  const bulkUpsertMutation = useBulkUpsertElectricalData();

  const isLoading = loadingEmpresas || loadingUsinas || electricalDataQuery.isLoading || bulkUpsertMutation.isPending;

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

  // Opções de Empresas (carregadas do backend)
  const empresasOptions = useMemo(() => {
    const options = empresas.map((emp) => ({
      value: emp.codigo,
      label: `${emp.codigo} - ${emp.nome}`,
    }));
    return [{ value: '', label: 'Selecione uma Empresa' }, ...options];
  }, [empresas]);

  // Atualiza selectedCompanyId quando empresa é selecionada
  useEffect(() => {
    if (form.codEmpresa) {
      const empresa = empresas.find(e => e.codigo === form.codEmpresa);
      setSelectedCompanyId(empresa?.id ?? null);
    } else {
      setSelectedCompanyId(null);
    }
  }, [form.codEmpresa, empresas]);

  // Opções de Usinas filtradas por empresa (carregadas do backend)
  const usinasOptions = useMemo(() => {
    if (!data || !data.usinas) return [];
    const options = data.usinas.map((u) => ({
      value: u.codUsina,
      label: `${u.codUsina} - ${u.nomeUsina || u.codUsina}`,
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
        return sum + (int ? int.valor : 0);
      }, 0);

      return {
        intervalo: intervalo.numero,
        total,
      };
    });
  }, [data, intervalos]);

  // Carregar dados quando data e empresa são selecionadas
  useEffect(() => {
    if (form.dataPdp && form.codEmpresa && electricalDataQuery.data) {
      processElectricalData();
    }
  }, [form.dataPdp, form.codEmpresa, electricalDataQuery.data, usinas]);

  /**
   * Processa dados carregados pela query do React Query
   */
  const processElectricalData = () => {
    if (!electricalDataQuery.data || !form.codEmpresa || !form.dataPdp) return;

    // Agrupar dados por usina
    const usinaMap = new Map<string, { codUsina: string; nomeUsina: string; intervalos: any[] }>();

    electricalDataQuery.data.forEach((dado) => {
      const codUsina = dado.codigoUsina;
      if (!usinaMap.has(codUsina)) {
        const usinaInfo = usinas.find(u => u.codigo === codUsina);
        usinaMap.set(codUsina, {
          codUsina,
          nomeUsina: usinaInfo?.nome || codUsina,
          intervalos: [],
        });
      }

      const usina = usinaMap.get(codUsina)!;
      usina.intervalos.push({
        intervalo: dado.intervalo,
        valor: dado.potenciaMW || 0,
      });
    });

    // Normalizar intervalos (preencher os faltantes com zero)
    const usinasComIntervalosCompletos = Array.from(usinaMap.values()).map((usina) => ({
      ...usina,
      intervalos: intervalos.map((int) => {
        const intervaloExistente = usina.intervalos.find((i) => i.intervalo === int.numero);
        return intervaloExistente || { intervalo: int.numero, valor: 0 };
      }),
    }));

    setData({
      dataPdp: form.dataPdp,
      codEmpresa: form.codEmpresa,
      usinas: usinasComIntervalosCompletos,
    });

    setForm((prev) => ({ ...prev, codUsina: '' }));
    setTextareaVisible(false);
  };

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

    setMessage(null);

    try {
      const lines = textareaValue.split('\n');
      const dadosParaSalvar: CreateDadoEletricoDto[] = [];

      if (form.codUsina === 'TODAS') {
        // Modo: Todas as usinas (grid com TABs separando usinas)
        lines.forEach((line, lineIndex) => {
          if (lineIndex >= 48) return;

          const valores = line.split('\t');
          const intervalo = lineIndex + 1;

          valores.forEach((valor, usinaIndex) => {
            if (usinaIndex < data.usinas.length) {
              const usina = data.usinas[usinaIndex];
              const usinaInfo = usinas.find(u => u.codigo === usina.codUsina);
              
              if (usinaInfo) {
                dadosParaSalvar.push({
                  dataPdp: form.dataPdp,
                  codigoEmpresa: form.codEmpresa,
                  codigoUsina: usina.codUsina,
                  intervalo,
                  potenciaMW: parseFloat(valor) || 0,
                });
              }
            }
          });
        });
      } else {
        // Modo: Usina individual (valores em linhas)
        const usinaInfo = usinas.find(u => u.codigo === form.codUsina);
        
        if (usinaInfo) {
          lines.forEach((line, index) => {
            if (index < 48) {
              const intervalo = index + 1;
              dadosParaSalvar.push({
                dataPdp: form.dataPdp,
                codigoEmpresa: form.codEmpresa,
                codigoUsina: form.codUsina,
                intervalo,
                potenciaMW: parseFloat(line) || 0,
              });
            }
          });
        }
      }

      await bulkUpsertMutation.mutateAsync(dadosParaSalvar);
      setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });

      // Atualizar dados locais
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
                intervalo.valor = parseFloat(valor) || 0;
              }
            }
          });
        });
        setData(updatedData);
      } else {
        const updatedData = { ...data };
        const usina = updatedData.usinas.find((u) => u.codUsina === form.codUsina);
        if (usina) {
          lines.forEach((line, index) => {
            if (index < 48) {
              const intervalo = usina.intervalos.find((i) => i.intervalo === index + 1);
              if (intervalo) {
                intervalo.valor = parseFloat(line) || 0;
              }
            }
          });
        }
        setData(updatedData);
      }
    } catch (error) {
      console.error('Erro ao salvar dados:', error);
      setMessage({ type: 'error', text: 'Erro ao salvar dados' });
    }
  };

  const calculateUsinaTotal = (usina: any): number => {
    return usina.intervalos.reduce((sum: number, int: any) => sum + int.valor, 0);
  };

  const calculateUsinaMedia = (usina: any): number => {
    const total = calculateUsinaTotal(usina);
    return total / 48;
  };

  return (
    <div className={styles.container} data-testid="electrical-container">
      <div className={styles.header}>
        <h1 data-testid="page-title">Razão Elétrica Transformada</h1>
        <p className={styles.subtitle} data-testid="page-subtitle">
          Coleta de dados elétricos de usinas
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
              disabled={isLoading}
            >
              <option value="">Selecione uma data</option>
              {datasPdp.map((option, idx) => (
                <option key={`${option.value}-${idx}`}
                  value={option.value}>
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
              disabled={isLoading || !form.dataPdp}
            >
              <option value="">Selecione uma empresa</option>
              {empresasOptions.map((option, idx) => (
                <option key={`${option.value}-${idx}`}
                  value={option.value}>
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
              disabled={isLoading || !data || usinasOptions.length === 0}
            >
              {usinasOptions.map((option, idx) => (
                <option key={`${option.value}-${idx}`}
                  value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {isLoading && (
        <div className={styles.loading} data-testid="loading-indicator">
          Carregando...
        </div>
      )}

      {electricalDataQuery.error && (
        <div className={styles.error} data-testid="error-message">
          Erro ao carregar dados. Por favor, tente novamente.
        </div>
      )}

      {data && !isLoading && (
        <div className={styles.dataSection}>
          <div className={styles.tableContainer}>
            <table className={styles.table} data-testid="data-table">
              <thead>
                <tr>
                  <th data-testid="th-intervalo">Intervalo</th>
                  <th data-testid="th-total">Total</th>
                  {data.usinas.map((usina, index) => (
                    <th
                      key={`${usina.codUsina}-${index}`}
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
                        const valor = int ? int.valor : 0;

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
            <div className={styles.editSection}>
              <div className={styles.editHeader}>
                <h2 data-testid="edit-section-title">
                  {form.codUsina === 'TODAS'
                    ? 'Edição de Todas as Usinas (separadas por TAB)'
                    : `Edição de Razão Elétrica: ${form.codUsina}`}
                </h2>
              </div>

              <div className={styles.textareaContainer}>
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
              </div>

              <div className={styles.actionsSection}>
                <button
                  type="button"
                  onClick={handleSave}
                  disabled={isLoading || bulkUpsertMutation.isPending}
                  className={styles.btnSave}
                  data-testid="btn-save"
                >
                  {bulkUpsertMutation.isPending ? 'Salvando...' : 'Salvar Dados'}
                </button>
              </div>
            </div>
          )}
        </div>
      )}

      {!isLoading && !data && form.dataPdp && form.codEmpresa && (
        <div className={styles.emptyState} data-testid="empty-state">
          <p>Não há dados disponíveis para a data e empresa selecionadas.</p>
        </div>
      )}
    </div>
  );
};

export default Electrical;

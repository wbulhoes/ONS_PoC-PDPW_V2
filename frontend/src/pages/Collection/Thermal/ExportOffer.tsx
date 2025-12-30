/**
 * Componente: Oferta de Exportação
 * Migração de: legado/pdpw/frmCnsOfertaExportacao.aspx
 *
 * Funcionalidades:
 * - Seleção de Data PDP
 * - Seleção de Empresa
 * - Seleção de Usina conversora (individual ou todas)
 * - Edição de valores de oferta de exportação por intervalo (48 intervalos de 30min)
 * - Salvamento de dados
 */

import React, { useState, useEffect, useMemo } from 'react';
import styles from './ExportOffer.module.css';
import type {
  OfertaExportacaoData,
  OfertaExportacaoForm,
  SelectOption,
} from '../../../types/exportOffer';
import { exportOfferService } from '../../../services/exportOfferService';

interface ExportOfferProps {
  onSave?: (data: OfertaExportacaoData) => Promise<void>;
  onLoadData?: (dataPdp: string, codEmpresa: string) => Promise<OfertaExportacaoData>;
}

const ExportOffer: React.FC<ExportOfferProps> = ({ onSave, onLoadData }) => {
  const [form, setForm] = useState<OfertaExportacaoForm>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
  });

  const [data, setData] = useState<OfertaExportacaoData | null>(null);
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
      { value: 'EMP001', label: 'Empresa Termelétrica A' },
      { value: 'EMP002', label: 'Empresa Termelétrica B' },
      { value: 'EMP003', label: 'Empresa Termelétrica C' },
    ],
    []
  );

  // Opções de Usinas conversoras filtradas por empresa
  const usinas = useMemo(() => {
    if (!data || !data.usinas) return [];
    const options = data.usinas.map((u) => ({
      value: u.codUsina,
      label: `${u.codUsina} - ${u.nomeUsina}`,
    }));
    if (options.length > 1) {
      options.push({ value: 'TODAS', label: 'Todas as Usinas Conversoras' });
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
    if (form.dataPdp && form.codEmpresa) {
      setLoading(true);
      setMessage(null);
      
      const loadFunc = onLoadData || exportOfferService.getOffers;

      loadFunc(form.dataPdp, form.codEmpresa)
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
    const newCodEmpresa = e.target.value;

    // Se a empresa mudou, limpar usina
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
    // Apenas números, Enter e Backspace
    const allowedKeys = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'Enter', 'Backspace', 'Tab'];
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
      // Parse dos valores do textarea
      const lines = textareaValue.split('\n');

      if (form.codUsina === 'TODAS') {
        // Modo: Todas as usinas (separadas por TAB)
        const updatedData = { ...data };

        lines.forEach((line, lineIndex) => {
          if (lineIndex >= 48) return; // Máximo 48 intervalos

          const valores = line.split('\t');

          valores.forEach((valor, usinaIndex) => {
            if (usinaIndex < data.usinas.length) {
              const usina = updatedData.usinas[usinaIndex];
              const intervalo = usina.intervalos.find((i) => i.intervalo === lineIndex + 1);

              if (intervalo) {
                intervalo.valor = parseInt(valor) || 0;
              }
            }
          });
        });

        const saveFunc = onSave || exportOfferService.saveOffers;
        await saveFunc(updatedData);
        setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
      } else {
        // Modo: Usina individual
        const updatedData = { ...data };
        const usina = updatedData.usinas.find((u) => u.codUsina === form.codUsina);

        if (usina) {
          lines.forEach((line, index) => {
            if (index < 48) {
              const intervalo = usina.intervalos.find((i) => i.intervalo === index + 1);
              if (intervalo) {
                intervalo.valor = parseInt(line) || 0;
              }
            }
          });

          const saveFunc = onSave || exportOfferService.saveOffers;
          await saveFunc(updatedData);
          setMessage({ type: 'success', text: 'Dados salvos com sucesso!' });
        }
      }
    } catch (error) {
      setMessage({ type: 'error', text: 'Erro ao salvar dados' });
    } finally {
      setLoading(false);
    }
  };

  const calculateTotal = (): number => {
    if (!textareaValue) return 0;

    const lines = textareaValue.split('\n');
    let total = 0;

    if (form.codUsina === 'TODAS') {
      // Somar todos os valores de todas as usinas
      lines.forEach((line) => {
        const valores = line.split('\t');
        valores.forEach((valor) => {
          total += parseInt(valor) || 0;
        });
      });
    } else {
      // Somar valores da usina individual
      lines.forEach((line) => {
        total += parseInt(line) || 0;
      });
    }

    return total;
  };

  return (
    <div className={styles.container} data-testid="export-offer-container">
      <div className={styles.header}>
        <h1 data-testid="page-title">Oferta de Exportação</h1>
        <p className={styles.subtitle} data-testid="page-subtitle">
          Coleta de ofertas de exportação de usinas termoelétricas conversoras
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
              Usina Conversora:
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
        <div className={styles.loading} data-testid="loading-indicator">
          Carregando...
        </div>
      )}

      {textareaVisible && (
        <div className={styles.dataSection}>
          <div className={styles.dataHeader}>
            <h2 data-testid="data-section-title">
              {form.codUsina === 'TODAS'
                ? 'Edição de Todas as Usinas Conversoras (separadas por TAB)'
                : `Edição de Oferta: ${form.codUsina}`}
            </h2>
            <p className={styles.instructions} data-testid="instructions">
              Digite os valores de exportação (MW) para cada intervalo de 30 minutos (48 intervalos total).
              {form.codUsina === 'TODAS' && ' Valores de cada usina separados por TAB.'}
            </p>
          </div>

          <div className={styles.gridContainer}>
            <div className={styles.intervalsLabels}>
              <div className={styles.labelHeader} data-testid="intervals-header">
                Intervalo
              </div>
              {intervalos.map((intervalo) => (
                <div key={intervalo.numero} className={styles.intervalLabel} data-testid={`interval-label-${intervalo.numero}`}>
                  {intervalo.label}
                </div>
              ))}
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
          </div>

          <div className={styles.summarySection}>
            <div className={styles.totalContainer} data-testid="total-container">
              <strong>Total de Exportação:</strong>
              <span className={styles.totalValue} data-testid="total-value">
                {calculateTotal()} MW
              </span>
            </div>
          </div>

          <div className={styles.actionsSection}>
            <button
              type="button"
              onClick={handleSave}
              disabled={loading}
              className={styles.btnSave}
              data-testid="btn-save"
            >
              {loading ? 'Salvando...' : 'Salvar Dados'}
            </button>
          </div>
        </div>
      )}

      {!loading && !textareaVisible && form.dataPdp && form.codEmpresa && (
        <div className={styles.emptyState} data-testid="empty-state">
          <p>Selecione uma usina conversora para visualizar e editar as ofertas de exportação.</p>
        </div>
      )}
    </div>
  );
};

export default ExportOffer;

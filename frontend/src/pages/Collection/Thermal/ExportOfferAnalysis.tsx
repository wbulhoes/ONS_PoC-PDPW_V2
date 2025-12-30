/**
 * Componente: Análise de Oferta de Exportação
 * Migração de: legado/pdpw/frmCnsAnaliseOfertaExportacao.aspx
 *
 * Funcionalidades:
 * - Visualização de ofertas de exportação (Agente vs ONS)
 * - Aprovação/Reprovação de ofertas (se AnaliseONS=true)
 * - Visualização de status
 */

import React, { useState, useEffect, useMemo } from 'react';
import { useSearchParams } from 'react-router-dom';
import styles from './ExportOffer.module.css'; // Reusing styles for now
import type {
  OfertaExportacaoData,
  OfertaExportacaoForm,
  SelectOption,
} from '../../../types/exportOffer';
import { exportOfferService } from '../../../services/exportOfferService';

const ExportOfferAnalysis: React.FC = () => {
  const [searchParams] = useSearchParams();
  const analiseOnsParam = searchParams.get('AnaliseONS');
  const isAnaliseOns = analiseOnsParam === 'S';

  const [form, setForm] = useState<OfertaExportacaoForm>({
    dataPdp: '',
    codEmpresa: '',
  });

  const [data, setData] = useState<OfertaExportacaoData | null>(null);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ type: 'success' | 'error'; text: string } | null>(null);
  const [selectedUsinas, setSelectedUsinas] = useState<string[]>([]);

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

  useEffect(() => {
    if (form.dataPdp && form.codEmpresa) {
      loadData();
    }
  }, [form.dataPdp, form.codEmpresa]);

  const loadData = async () => {
    setLoading(true);
    setMessage(null);
    try {
      const result = await exportOfferService.getOffers(form.dataPdp, form.codEmpresa);
      setData(result);
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao carregar dados.' });
    } finally {
      setLoading(false);
    }
  };

  const handleApprove = async () => {
    if (selectedUsinas.length === 0) {
      setMessage({ type: 'error', text: 'Selecione pelo menos uma usina para aprovar.' });
      return;
    }
    setLoading(true);
    try {
      await exportOfferService.approveOffers(form.dataPdp, form.codEmpresa, selectedUsinas);
      setMessage({ type: 'success', text: 'Ofertas aprovadas com sucesso.' });
      loadData(); // Reload to update status
      setSelectedUsinas([]);
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao aprovar ofertas.' });
    } finally {
      setLoading(false);
    }
  };

  const handleReject = async () => {
    if (selectedUsinas.length === 0) {
      setMessage({ type: 'error', text: 'Selecione pelo menos uma usina para reprovar.' });
      return;
    }
    if (!window.confirm('Confirma a reprovação das ofertas selecionadas?')) return;

    setLoading(true);
    try {
      await exportOfferService.rejectOffers(form.dataPdp, form.codEmpresa, selectedUsinas);
      setMessage({ type: 'success', text: 'Ofertas reprovadas com sucesso.' });
      loadData();
      setSelectedUsinas([]);
    } catch (error) {
      console.error(error);
      setMessage({ type: 'error', text: 'Erro ao reprovar ofertas.' });
    } finally {
      setLoading(false);
    }
  };

  const toggleUsinaSelection = (codUsina: string) => {
    setSelectedUsinas((prev) =>
      prev.includes(codUsina) ? prev.filter((id) => id !== codUsina) : [...prev, codUsina]
    );
  };

  const toggleAllUsinas = () => {
    if (!data) return;
    if (selectedUsinas.length === data.usinas.length) {
      setSelectedUsinas([]);
    } else {
      setSelectedUsinas(data.usinas.map((u) => u.codUsina));
    }
  };

  return (
    <div className={styles.container}>
      <h2 className={styles.title}>
        {isAnaliseOns ? 'Análise de Oferta de Exportação' : 'Análise de Exportação Agente'}
      </h2>

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
      </div>

      {message && (
        <div className={`${styles.message} ${message.type === 'error' ? styles.error : styles.success}`}>
          {message.text}
        </div>
      )}

      {loading && <div className={styles.loading}>Carregando...</div>}

      {data && (
        <div className={styles.content}>
          {isAnaliseOns && (
            <div className={styles.actions}>
              <button onClick={handleApprove} className={styles.button} disabled={loading}>
                Aprovar Selecionados
              </button>
              <button onClick={handleReject} className={`${styles.button} ${styles.secondary}`} disabled={loading}>
                Reprovar Selecionados
              </button>
            </div>
          )}

          <div className={styles.tableContainer}>
            <table className={styles.table}>
              <thead>
                <tr>
                  {isAnaliseOns && (
                    <th>
                      <input
                        type="checkbox"
                        checked={data.usinas.length > 0 && selectedUsinas.length === data.usinas.length}
                        onChange={toggleAllUsinas}
                      />
                    </th>
                  )}
                  <th>Usina / Conversora</th>
                  <th>Status</th>
                  <th>Intervalo</th>
                  <th>Valor Agente (MW)</th>
                  <th>Valor ONS (MW)</th>
                </tr>
              </thead>
              <tbody>
                {data.usinas.map((usina) => (
                  <React.Fragment key={usina.codUsina}>
                    <tr className={styles.groupHeader}>
                      {isAnaliseOns && (
                        <td>
                          <input
                            type="checkbox"
                            checked={selectedUsinas.includes(usina.codUsina)}
                            onChange={() => toggleUsinaSelection(usina.codUsina)}
                          />
                        </td>
                      )}
                      <td colSpan={5}>
                        <strong>{usina.nomeUsina}</strong> - {usina.codConversora}
                      </td>
                    </tr>
                    {usina.intervalos.map((intervalo) => (
                      <tr key={`${usina.codUsina}-${intervalo.intervalo}`}>
                        {isAnaliseOns && <td></td>}
                        <td></td>
                        <td>{usina.status || 'Pendente'}</td>
                        <td>{intervalo.intervalo}</td>
                        <td>{intervalo.valor}</td>
                        <td>{intervalo.valorOns ?? '-'}</td>
                      </tr>
                    ))}
                  </React.Fragment>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default ExportOfferAnalysis;

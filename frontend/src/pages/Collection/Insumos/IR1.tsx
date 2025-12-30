import React, { useState, useEffect } from 'react';
import styles from './IR1.module.css';
import { IR1FormData } from '../../../types/regulatoryInputs';
import { useIR1DataByDate, useCreateIR1Data, useUpdateIR1Data } from '../../../hooks/useIR1Data';
import { useCompanies } from '../../../hooks/useCompanies';
import { usePlantsByCompany } from '../../../hooks/usePlants';
import type { IR1Dto, NivelPartidaItem } from '../../../types/ir1';

/**
 * Componente para Coleta de IR1 - Nível de Partida
 * Registra nível de partida de reservatórios hidrelétricos
 * Dado regulatório para planejamento operacional
 * 
 * T060: Connect IR1 component to hooks (replace mock data)
 */
const IR1: React.FC = () => {
  const [formData, setFormData] = useState<IR1FormData>({
    dataPDP: '',
    empresa: '',
    usina: '',
    nivelPartida: ''
  });

  const [dataOptions, setDataOptions] = useState<string[]>([]);
  const [showSaveButton, setShowSaveButton] = useState(false);
  const [selectedCompanyId, setSelectedCompanyId] = useState<string | null>(null);
  const [selectedUsinaId, setSelectedUsinaId] = useState<number | null>(null);
  const [ir1RecordId, setIr1RecordId] = useState<number | null>(null);

  // React Query hooks
  const { data: empresas = [], isLoading: loadingEmpresas } = useCompanies();
  const { data: usinas = [], isLoading: loadingUsinas } = usePlantsByCompany(selectedCompanyId || undefined);
  const ir1DataQuery = useIR1DataByDate(formData.dataPDP);
  const createIR1Mutation = useCreateIR1Data();
  const updateIR1Mutation = useUpdateIR1Data();

  const isLoading = loadingEmpresas || loadingUsinas || ir1DataQuery.isLoading || createIR1Mutation.isPending || updateIR1Mutation.isPending;
  const error = ir1DataQuery.error 
    ? 'Não foi possível carregar os dados de IR1.' 
    : (createIR1Mutation.error || updateIR1Mutation.error ? 'Não foi possível salvar os dados de IR1.' : '');

  // Generate date options (7 days forward from today)
  useEffect(() => {
    const hoje = new Date();
    const datas = [];
    for (let i = 0; i < 7; i++) {
      const data = new Date(hoje);
      data.setDate(hoje.getDate() + i);
      datas.push(data.toISOString().split('T')[0]); // formato YYYY-MM-DD
    }
    setDataOptions(datas);
  }, []);

  // Update selectedCompanyId when empresa is selected
  useEffect(() => {
    if (formData.empresa) {
      const empresa = empresas.find(e => e.codigo === formData.empresa);
      setSelectedCompanyId(empresa?.id ?? null);
    } else {
      setSelectedCompanyId(null);
    }
  }, [formData.empresa, empresas]);

  // Load IR1 data for selected usina when data is available
  useEffect(() => {
    if (ir1DataQuery.data && selectedUsinaId) {
      const nivelData = ir1DataQuery.data.niveisPartida?.find(
        (n: NivelPartidaItem) => n.usinaId === selectedUsinaId
      );
      if (nivelData) {
        setFormData(prev => ({ ...prev, nivelPartida: nivelData.nivel.toString() }));
        setShowSaveButton(true);
      }
      setIr1RecordId(ir1DataQuery.data.id ?? null);
    }
  }, [ir1DataQuery.data, selectedUsinaId]);

  const handleDataChange = (value: string) => {
    setFormData(prev => ({ ...prev, dataPDP: value, empresa: '', usina: '', nivelPartida: '' }));
    setShowSaveButton(false);
    setSelectedCompanyId(null);
    setSelectedUsinaId(null);
  };

  const handleEmpresaChange = (value: string) => {
    setFormData(prev => ({ ...prev, empresa: value, usina: '', nivelPartida: '' }));
    setShowSaveButton(false);
    setSelectedUsinaId(null);
  };

  const handleUsinaChange = (value: string) => {
    setFormData(prev => ({ ...prev, usina: value, nivelPartida: '' }));
    
    if (value) {
      const usina = usinas.find(u => u.codigo === value);
      if (usina) {
        setSelectedUsinaId(usina.id);
        // Check if there's existing data for this usina
        if (ir1DataQuery.data && ir1DataQuery.data.niveisPartida) {
          const nivelData = ir1DataQuery.data.niveisPartida.find(
            (n: NivelPartidaItem) => n.usinaId === usina.id
          );
          if (nivelData) {
            setFormData(prev => ({ ...prev, nivelPartida: nivelData.nivel.toString() }));
          }
        }
        setShowSaveButton(true);
      }
    } else {
      setShowSaveButton(false);
      setSelectedUsinaId(null);
    }
  };

  const handleNivelPartidaChange = (value: string) => {
    setFormData(prev => ({ ...prev, nivelPartida: value }));
  };

  const handleSave = async () => {
    if (!formData.dataPDP || !formData.empresa || !formData.usina || !formData.nivelPartida) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    if (!selectedUsinaId) {
      alert('Usina não identificada');
      return;
    }

    try {
      const nivelPartidaItem: NivelPartidaItem = {
        usinaId: selectedUsinaId,
        nivel: parseFloat(formData.nivelPartida),
        volume: 0, // TODO: Calculate volume based on nivel if needed
      };

      if (ir1RecordId) {
        // Update existing record
        await updateIR1Mutation.mutateAsync({
          id: ir1RecordId,
          dto: {
            niveisPartida: [nivelPartidaItem],
          },
        });
      } else {
        // Create new record
        const ir1Dto: IR1Dto = {
          dataReferencia: formData.dataPDP,
          niveisPartida: [nivelPartidaItem],
        };
        await createIR1Mutation.mutateAsync(ir1Dto);
      }

      alert('Nível de Partida salvo com sucesso!');
      // Refetch data to get updated values
      ir1DataQuery.refetch();
    } catch (error) {
      console.error('Erro ao salvar IR1:', error);
      alert('Erro ao salvar Nível de Partida');
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <h2 className={styles.title}>Nível de Partida</h2>
        </div>
      </div>

      {error && (
        <div className={styles.errorMessage}>
          {error}
        </div>
      )}

      {isLoading && (
        <div className={styles.loadingMessage}>
          Carregando...
        </div>
      )}

      <div className={styles.formSection}>
        <div className={styles.formRow}>
          <label>
            <strong>Data PDP:</strong>
          </label>
          <select
            value={formData.dataPDP}
            onChange={(e) => handleDataChange(e.target.value)}
            className={styles.select}
            disabled={isLoading}
          >
            <option value="">Selecione</option>
            {dataOptions.map(data => (
              <option key={data} value={data}>{data}</option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label>
            <strong>Empresa:</strong>
          </label>
          <select
            value={formData.empresa}
            onChange={(e) => handleEmpresaChange(e.target.value)}
            className={styles.select}
            disabled={!formData.dataPDP || loadingEmpresas}
          >
            <option value="">Selecione</option>
            {empresas.map(emp => (
              <option key={emp.id} value={emp.codigo}>{emp.nome}</option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label>
            <strong>Usinas:</strong>
          </label>
          <select
            value={formData.usina}
            onChange={(e) => handleUsinaChange(e.target.value)}
            className={styles.select}
            disabled={!formData.empresa || loadingUsinas}
          >
            <option value="">Selecione</option>
            {usinas.map(usina => (
              <option key={usina.id} value={usina.codigo}>{usina.nome}</option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label>
            <strong>Valor:</strong>
          </label>
          <input
            type="number"
            step="0.01"
            value={formData.nivelPartida}
            onChange={(e) => handleNivelPartidaChange(e.target.value)}
            className={styles.input}
            disabled={!formData.usina || isLoading}
            placeholder="Nível de Partida (m)"
          />
          {showSaveButton && (
            <button
              onClick={handleSave}
              className={styles.saveButton}
              disabled={isLoading}
            >
              {isLoading ? 'Salvando...' : 'Salvar'}
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default IR1;

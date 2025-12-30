import React, { useState, useEffect } from 'react';
import styles from './OutOfMeritGeneration.module.css';
import { OutOfMeritGenerationData, OutOfMeritGenerationTableRow, OutOfMeritGenerationFormData } from '../../../types/outOfMeritGeneration';

/**
 * Componente para Coleta de Geração Fora de Mérito
 * Registra geração despachada fora da ordem de mérito econômico
 * Geração Fora de Mérito = despacho por restrições operacionais, segurança ou confiabilidade
 */
const OutOfMeritGeneration: React.FC = () => {
  const [formData, setFormData] = useState<OutOfMeritGenerationFormData>({
    dataPDP: '',
    empresa: '',
    usina: ''
  });

  const [tableData, setTableData] = useState<OutOfMeritGenerationTableRow[]>([]);
  const [editingValues, setEditingValues] = useState<string>('');
  const [showTextarea, setShowTextarea] = useState(false);
  const [dataOptions, setDataOptions] = useState<string[]>([]);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [usinaOptions, setUsinaOptions] = useState<string[]>([]);

  const getTimeLabel = (intervalo: number): string => {
    const hour = Math.floor((intervalo - 1) / 2);
    const isFirstHalf = (intervalo - 1) % 2 === 0;
    
    if (isFirstHalf) {
      return `${hour.toString().padStart(2, '0')}:00-${hour.toString().padStart(2, '0')}:30`;
    } else {
      return `${hour.toString().padStart(2, '0')}:30-${(hour + 1).toString().padStart(2, '0')}:00`;
    }
  };

  const initializeTable = () => {
    const rows: OutOfMeritGenerationTableRow[] = [];
    for (let i = 1; i <= 48; i++) {
      rows.push({
        intervalo: i,
        horario: getTimeLabel(i),
        valores: {},
        total: 0
      });
    }
    setTableData(rows);
  };

  useEffect(() => {
    initializeTable();
    setDataOptions(['01/01/2024', '02/01/2024', '03/01/2024']);
  }, []);

  const handleFormChange = (field: keyof OutOfMeritGenerationFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));

    if (field === 'dataPDP') {
      setFormData(prev => ({ ...prev, empresa: '', usina: '' }));
      setEmpresaOptions(['Empresa A', 'Empresa B', 'Empresa C']);
    }

    if (field === 'empresa') {
      setFormData(prev => ({ ...prev, usina: '' }));
      setUsinaOptions(['Usina 1', 'Usina 2', 'Usina 3', 'Todas as Usinas']);
    }
  };

  const handleUsinaChange = (value: string) => {
    handleFormChange('usina', value);
    
    if (value && value !== 'Selecione uma Usina') {
      loadOutOfMeritGenerationData(value);
      setShowTextarea(true);
    } else {
      setShowTextarea(false);
    }
  };

  const loadOutOfMeritGenerationData = (usina: string) => {
    const mockData: string[] = [];
    
    if (usina === 'Todas as Usinas') {
      for (let i = 0; i < 48; i++) {
        mockData.push('25\t40\t55');
      }
    } else {
      for (let i = 0; i < 48; i++) {
        mockData.push('25');
      }
    }
    
    setEditingValues(mockData.join('\n'));
    updateTableFromTextarea(mockData.join('\n'));
  };

  const updateTableFromTextarea = (text: string) => {
    const lines = text.split('\n');
    const updatedRows = [...tableData];

    lines.forEach((line, index) => {
      if (index < 48) {
        const values = line.split('\t').map(v => parseFloat(v) || 0);
        
        if (formData.usina === 'Todas as Usinas') {
          const usinas = usinaOptions.filter(u => u !== 'Todas as Usinas' && u !== 'Selecione uma Usina');
          const valoresMap: { [key: string]: number } = {};
          
          usinas.forEach((usina, idx) => {
            valoresMap[usina] = values[idx] || 0;
          });
          
          updatedRows[index].valores = valoresMap;
          updatedRows[index].total = values.reduce((sum, val) => sum + val, 0);
        } else {
          updatedRows[index].valores = { [formData.usina]: values[0] || 0 };
          updatedRows[index].total = values[0] || 0;
        }
      }
    });

    setTableData(updatedRows);
  };

  const handleTextareaChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    const value = e.target.value;
    setEditingValues(value);
    updateTableFromTextarea(value);
  };

  const handleSave = () => {
    if (!formData.dataPDP || !formData.empresa || !formData.usina) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    console.log('Salvando Geração Fora de Mérito:', {
      formData,
      tableData
    });

    alert('Geração Fora de Mérito salva com sucesso!');
  };

  const calculateColumnTotal = (usina: string): number => {
    return tableData.reduce((sum, row) => sum + (row.valores[usina] || 0), 0);
  };

  const calculateColumnAverage = (usina: string): number => {
    const total = calculateColumnTotal(usina);
    return total / 48;
  };

  const usinas = Object.keys(tableData[0]?.valores || {});

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <img src="/images/tit_GerForaMerito.gif" alt="Geração Fora de Mérito" />
        </div>
      </div>

      <div className={styles.formSection}>
        <div className={styles.formRow}>
          <label>
            <strong>Data PDP:</strong>
          </label>
          <select
            value={formData.dataPDP}
            onChange={(e) => handleFormChange('dataPDP', e.target.value)}
            className={styles.select}
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
            onChange={(e) => handleFormChange('empresa', e.target.value)}
            className={styles.select}
            disabled={!formData.dataPDP}
          >
            <option value="">Selecione</option>
            {empresaOptions.map(emp => (
              <option key={emp} value={emp}>{emp}</option>
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
            disabled={!formData.empresa}
          >
            <option value="">Selecione uma Usina</option>
            {usinaOptions.map(usina => (
              <option key={usina} value={usina}>{usina}</option>
            ))}
          </select>
          <button
            onClick={handleSave}
            className={styles.saveButton}
            disabled={!showTextarea}
          >
            Salvar
          </button>
        </div>
      </div>

      <div className={styles.tableContainer}>
        <div className={styles.tableWrapper}>
          <table className={styles.table}>
            <thead>
              <tr>
                <th className={styles.intervalHeader}>Intervalo</th>
                <th className={styles.totalHeader}>Total</th>
                {usinas.map(usina => (
                  <th
                    key={usina}
                    className={usina === formData.usina ? styles.selectedColumn : ''}
                  >
                    {usina}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td className={styles.intervalCell}>{row.horario}</td>
                  <td className={styles.totalCell}>{row.total.toFixed(2)}</td>
                  {usinas.map(usina => (
                    <td key={usina} className={styles.valueCell}>
                      {(row.valores[usina] || 0).toFixed(2)}
                    </td>
                  ))}
                </tr>
              ))}
            </tbody>
            <tfoot>
              <tr>
                <td className={styles.footerLabel}>Total</td>
                <td className={styles.footerValue}>
                  {tableData.reduce((sum, row) => sum + row.total, 0).toFixed(2)}
                </td>
                {usinas.map(usina => (
                  <td key={usina} className={styles.footerValue}>
                    {calculateColumnTotal(usina).toFixed(2)}
                  </td>
                ))}
              </tr>
              <tr>
                <td className={styles.footerLabel}>Média</td>
                <td className={styles.footerValue}>
                  {(tableData.reduce((sum, row) => sum + row.total, 0) / 48).toFixed(2)}
                </td>
                {usinas.map(usina => (
                  <td key={usina} className={styles.footerValue}>
                    {calculateColumnAverage(usina).toFixed(2)}
                  </td>
                ))}
              </tr>
            </tfoot>
          </table>

          {showTextarea && (
            <div className={styles.textareaOverlay}>
              <textarea
                value={editingValues}
                onChange={handleTextareaChange}
                rows={48}
                className={styles.textarea}
                placeholder="Digite valores de Geração Fora de Mérito (MW) - despacho fora da ordem econômica"
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default OutOfMeritGeneration;

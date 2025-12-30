import React, { useState, useEffect } from 'react';
import styles from './EstimatedGeneration.module.css';
import { EstimatedGenerationData, EstimatedGenerationTableRow, EstimatedGenerationFormData } from '../../../types/estimatedGeneration';

/**
 * Componente para Coleta de Geração Estimada
 * Registra previsão de geração por usina
 * 50 intervalos de 30 minutos para planejamento operacional
 */
const EstimatedGeneration: React.FC = () => {
  const [formData, setFormData] = useState<EstimatedGenerationFormData>({
    dataPDP: '',
    empresa: '',
    usina: ''
  });

  const [tableData, setTableData] = useState<EstimatedGenerationTableRow[]>([]);
  const [editingValues, setEditingValues] = useState<string>('');
  const [showTextarea, setShowTextarea] = useState(false);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [usinaOptions, setUsinaOptions] = useState<string[]>([]);
  const [showCalendar, setShowCalendar] = useState(false);

  const getTimeLabel = (intervalo: number): string => {
    const totalMinutes = (intervalo - 1) * 30;
    const hour = Math.floor(totalMinutes / 60);
    const minute = totalMinutes % 60;
    const nextHour = Math.floor((totalMinutes + 30) / 60);
    const nextMinute = (totalMinutes + 30) % 60;
    
    return `${hour.toString().padStart(2, '0')}:${minute.toString().padStart(2, '0')}-${nextHour.toString().padStart(2, '0')}:${nextMinute.toString().padStart(2, '0')}`;
  };

  const initializeTable = () => {
    const rows: EstimatedGenerationTableRow[] = [];
    for (let i = 1; i <= 50; i++) {
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
    setEmpresaOptions(['Empresa A', 'Empresa B', 'Empresa C']);
  }, []);

  const handleDateSelect = (date: string) => {
    setFormData(prev => ({ ...prev, dataPDP: date, usina: '' }));
    setShowCalendar(false);
  };

  const handleFormChange = (field: keyof EstimatedGenerationFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));

    if (field === 'empresa') {
      setFormData(prev => ({ ...prev, usina: '' }));
      setUsinaOptions(['Usina 1', 'Usina 2', 'Usina 3', 'Todas']);
    }
  };

  const handleUsinaChange = (value: string) => {
    handleFormChange('usina', value);
    
    if (value && value !== 'Selecione') {
      loadEstimatedGenerationData(value);
      setShowTextarea(true);
    } else {
      setShowTextarea(false);
    }
  };

  const loadEstimatedGenerationData = (usina: string) => {
    const mockData: string[] = [];
    
    if (usina === 'Todas') {
      for (let i = 0; i < 50; i++) {
        mockData.push('500\t600\t700');
      }
    } else {
      for (let i = 0; i < 50; i++) {
        mockData.push('500');
      }
    }
    
    setEditingValues(mockData.join('\n'));
    updateTableFromTextarea(mockData.join('\n'));
  };

  const updateTableFromTextarea = (text: string) => {
    const lines = text.split('\n');
    const updatedRows = [...tableData];

    lines.forEach((line, index) => {
      if (index < 50) {
        const values = line.split('\t').map(v => parseFloat(v) || 0);
        
        if (formData.usina === 'Todas') {
          const usinas = usinaOptions.filter(u => u !== 'Todas' && u !== 'Selecione');
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

    console.log('Salvando Geração Estimada:', {
      formData,
      tableData
    });

    alert('Geração Estimada salva com sucesso!');
  };

  const calculateColumnTotal = (usina: string): number => {
    return tableData.reduce((sum, row) => sum + (row.valores[usina] || 0), 0);
  };

  const calculateColumnAverage = (usina: string): number => {
    const total = calculateColumnTotal(usina);
    return total / 50;
  };

  const usinas = Object.keys(tableData[0]?.valores || {});

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <img src="/images/tit_ColGeracao.gif" alt="Geração Estimada" />
        </div>
      </div>

      <div className={styles.formSection}>
        <div className={styles.formRow}>
          <label>
            <strong>Data PDP:</strong>
          </label>
          <input
            type="text"
            value={formData.dataPDP}
            readOnly
            className={styles.dateInput}
            placeholder="Selecione a data"
          />
          <button
            onClick={() => setShowCalendar(!showCalendar)}
            className={styles.calendarButton}
          >
            ...
          </button>
          {showCalendar && (
            <div className={styles.calendarPopup}>
              <input
                type="date"
                onChange={(e) => handleDateSelect(e.target.value)}
                className={styles.datePickerInput}
              />
            </div>
          )}
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
            <strong>Usina:</strong>
          </label>
          <select
            value={formData.usina}
            onChange={(e) => handleUsinaChange(e.target.value)}
            className={styles.select}
            disabled={!formData.empresa}
          >
            <option value="">Selecione</option>
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
                  {(tableData.reduce((sum, row) => sum + row.total, 0) / 50).toFixed(2)}
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
                rows={50}
                className={styles.textarea}
                placeholder="Digite valores de Geração Estimada (MW) - 50 intervalos de 30 minutos"
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default EstimatedGeneration;

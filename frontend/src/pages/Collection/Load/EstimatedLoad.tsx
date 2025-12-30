import React, { useState, useEffect } from 'react';
import styles from './EstimatedLoad.module.css';
import { EstimatedLoadData, EstimatedLoadTableRow, EstimatedLoadFormData } from '../../../types/estimatedLoad';

/**
 * Componente para Coleta de Carga Estimada
 * Registra previsão de carga elétrica por submercado
 * 50 intervalos de 30 minutos (25 horas para cobrir mudanças de horário)
 */
const EstimatedLoad: React.FC = () => {
  const [formData, setFormData] = useState<EstimatedLoadFormData>({
    dataPDP: '',
    empresa: '',
    submercado: ''
  });

  const [tableData, setTableData] = useState<EstimatedLoadTableRow[]>([]);
  const [editingValues, setEditingValues] = useState<string>('');
  const [showTextarea, setShowTextarea] = useState(false);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [submercadoOptions, setSubmercadoOptions] = useState<string[]>([]);
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
    const rows: EstimatedLoadTableRow[] = [];
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
    setFormData(prev => ({ ...prev, dataPDP: date, submercado: '' }));
    setShowCalendar(false);
    setSubmercadoOptions(['Norte', 'Nordeste', 'Sudeste', 'Sul', 'Todos']);
  };

  const handleFormChange = (field: keyof EstimatedLoadFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));

    if (field === 'empresa') {
      setFormData(prev => ({ ...prev, submercado: '' }));
    }
  };

  const handleSubmercadoChange = (value: string) => {
    handleFormChange('submercado', value);
    
    if (value && value !== 'Selecione') {
      loadEstimatedLoadData(value);
      setShowTextarea(true);
    } else {
      setShowTextarea(false);
    }
  };

  const loadEstimatedLoadData = (submercado: string) => {
    const mockData: string[] = [];
    
    if (submercado === 'Todos') {
      for (let i = 0; i < 50; i++) {
        mockData.push('1000\t800\t1200\t900');
      }
    } else {
      for (let i = 0; i < 50; i++) {
        mockData.push('1000');
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
        
        if (formData.submercado === 'Todos') {
          const submercados = submercadoOptions.filter(s => s !== 'Todos' && s !== 'Selecione');
          const valoresMap: { [key: string]: number } = {};
          
          submercados.forEach((subm, idx) => {
            valoresMap[subm] = values[idx] || 0;
          });
          
          updatedRows[index].valores = valoresMap;
          updatedRows[index].total = values.reduce((sum, val) => sum + val, 0);
        } else {
          updatedRows[index].valores = { [formData.submercado]: values[0] || 0 };
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
    if (!formData.dataPDP || !formData.empresa || !formData.submercado) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    console.log('Salvando Carga Estimada:', {
      formData,
      tableData
    });

    alert('Carga Estimada salva com sucesso!');
  };

  const calculateColumnTotal = (submercado: string): number => {
    return tableData.reduce((sum, row) => sum + (row.valores[submercado] || 0), 0);
  };

  const calculateColumnAverage = (submercado: string): number => {
    const total = calculateColumnTotal(submercado);
    return total / 50;
  };

  const submercados = Object.keys(tableData[0]?.valores || {});

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <img src="/images/tit_ColCarga.gif" alt="Carga Estimada" />
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
            <strong>Submercado:</strong>
          </label>
          <select
            value={formData.submercado}
            onChange={(e) => handleSubmercadoChange(e.target.value)}
            className={styles.select}
            disabled={!formData.empresa}
          >
            <option value="">Selecione</option>
            {submercadoOptions.map(subm => (
              <option key={subm} value={subm}>{subm}</option>
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
                {submercados.map(subm => (
                  <th
                    key={subm}
                    className={subm === formData.submercado ? styles.selectedColumn : ''}
                  >
                    {subm}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td className={styles.intervalCell}>{row.horario}</td>
                  <td className={styles.totalCell}>{row.total.toFixed(2)}</td>
                  {submercados.map(subm => (
                    <td key={subm} className={styles.valueCell}>
                      {(row.valores[subm] || 0).toFixed(2)}
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
                {submercados.map(subm => (
                  <td key={subm} className={styles.footerValue}>
                    {calculateColumnTotal(subm).toFixed(2)}
                  </td>
                ))}
              </tr>
              <tr>
                <td className={styles.footerLabel}>Média</td>
                <td className={styles.footerValue}>
                  {(tableData.reduce((sum, row) => sum + row.total, 0) / 50).toFixed(2)}
                </td>
                {submercados.map(subm => (
                  <td key={subm} className={styles.footerValue}>
                    {calculateColumnAverage(subm).toFixed(2)}
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
                placeholder="Digite valores de Carga Estimada (MW) - 50 intervalos de 30 minutos"
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default EstimatedLoad;

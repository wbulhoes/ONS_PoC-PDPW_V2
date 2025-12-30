import React, { useState, useEffect } from 'react';
import styles from './EstimatedInterchange.module.css';
import { EstimatedInterchangeData, EstimatedInterchangeTableRow, EstimatedInterchangeFormData } from '../../../types/estimatedInterchange';

/**
 * Componente para Coleta de Intercâmbio Estimado
 * Registra previsão de fluxo de energia entre submercados
 * 50 intervalos de 30 minutos para planejamento de intercâmbios
 */
const EstimatedInterchange: React.FC = () => {
  const [formData, setFormData] = useState<EstimatedInterchangeFormData>({
    dataPDP: '',
    empresa: '',
    intercambio: ''
  });

  const [tableData, setTableData] = useState<EstimatedInterchangeTableRow[]>([]);
  const [editingValues, setEditingValues] = useState<string>('');
  const [showTextarea, setShowTextarea] = useState(false);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [intercambioOptions, setIntercambioOptions] = useState<string[]>([]);
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
    const rows: EstimatedInterchangeTableRow[] = [];
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
    setFormData(prev => ({ ...prev, dataPDP: date, intercambio: '' }));
    setShowCalendar(false);
  };

  const handleFormChange = (field: keyof EstimatedInterchangeFormData, value: string) => {
    setFormData(prev => ({ ...prev, [field]: value }));

    if (field === 'empresa') {
      setFormData(prev => ({ ...prev, intercambio: '' }));
      setIntercambioOptions([
        'Norte -> Nordeste',
        'Sudeste -> Sul',
        'Nordeste -> Sudeste',
        'Todos'
      ]);
    }
  };

  const handleIntercambioChange = (value: string) => {
    handleFormChange('intercambio', value);
    
    if (value && value !== 'Selecione') {
      loadEstimatedInterchangeData(value);
      setShowTextarea(true);
    } else {
      setShowTextarea(false);
    }
  };

  const loadEstimatedInterchangeData = (intercambio: string) => {
    const mockData: string[] = [];
    
    if (intercambio === 'Todos') {
      for (let i = 0; i < 50; i++) {
        mockData.push('200\t150\t300');
      }
    } else {
      for (let i = 0; i < 50; i++) {
        mockData.push('200');
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
        
        if (formData.intercambio === 'Todos') {
          const intercambios = intercambioOptions.filter(i => i !== 'Todos' && i !== 'Selecione');
          const valoresMap: { [key: string]: number } = {};
          
          intercambios.forEach((inter, idx) => {
            valoresMap[inter] = values[idx] || 0;
          });
          
          updatedRows[index].valores = valoresMap;
          updatedRows[index].total = values.reduce((sum, val) => sum + val, 0);
        } else {
          updatedRows[index].valores = { [formData.intercambio]: values[0] || 0 };
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
    if (!formData.dataPDP || !formData.empresa || !formData.intercambio) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    console.log('Salvando Intercâmbio Estimado:', {
      formData,
      tableData
    });

    alert('Intercâmbio Estimado salvo com sucesso!');
  };

  const calculateColumnTotal = (intercambio: string): number => {
    return tableData.reduce((sum, row) => sum + (row.valores[intercambio] || 0), 0);
  };

  const calculateColumnAverage = (intercambio: string): number => {
    const total = calculateColumnTotal(intercambio);
    return total / 50;
  };

  const intercambios = Object.keys(tableData[0]?.valores || {});

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <img src="/images/tit_ColIntercambio.gif" alt="Intercâmbio Estimado" />
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
            <strong>Intercâmbios:</strong>
          </label>
          <select
            value={formData.intercambio}
            onChange={(e) => handleIntercambioChange(e.target.value)}
            className={styles.select}
            disabled={!formData.empresa}
          >
            <option value="">Selecione</option>
            {intercambioOptions.map(inter => (
              <option key={inter} value={inter}>{inter}</option>
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
                {intercambios.map(inter => (
                  <th
                    key={inter}
                    className={inter === formData.intercambio ? styles.selectedColumn : ''}
                  >
                    {inter}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td className={styles.intervalCell}>{row.horario}</td>
                  <td className={styles.totalCell}>{row.total.toFixed(2)}</td>
                  {intercambios.map(inter => (
                    <td key={inter} className={styles.valueCell}>
                      {(row.valores[inter] || 0).toFixed(2)}
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
                {intercambios.map(inter => (
                  <td key={inter} className={styles.footerValue}>
                    {calculateColumnTotal(inter).toFixed(2)}
                  </td>
                ))}
              </tr>
              <tr>
                <td className={styles.footerLabel}>Média</td>
                <td className={styles.footerValue}>
                  {(tableData.reduce((sum, row) => sum + row.total, 0) / 50).toFixed(2)}
                </td>
                {intercambios.map(inter => (
                  <td key={inter} className={styles.footerValue}>
                    {calculateColumnAverage(inter).toFixed(2)}
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
                placeholder="Digite valores de Intercâmbio Estimado (MW) - 50 intervalos de 30 minutos"
              />
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default EstimatedInterchange;

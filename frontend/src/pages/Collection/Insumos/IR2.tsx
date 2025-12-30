import React, { useState, useEffect } from 'react';
import styles from './IR2.module.css';
import { IR2FormData, IR2TableRow } from '../../../types/regulatoryInputs';

/**
 * Componente para Coleta de IR2 - Dia -1
 * Registra valores regulatórios do dia anterior
 * 24 intervalos horários para controle operacional
 */
const IR2: React.FC = () => {
  const [formData, setFormData] = useState<IR2FormData>({
    dataPDP: '',
    empresa: '',
    usina: ''
  });

  const [tableData, setTableData] = useState<IR2TableRow[]>([]);
  const [dataOptions, setDataOptions] = useState<string[]>([]);
  const [empresaOptions, setEmpresaOptions] = useState<string[]>([]);
  const [usinaOptions, setUsinaOptions] = useState<string[]>([]);
  const [showTable, setShowTable] = useState(false);

  useEffect(() => {
    setDataOptions(['2024-01-01', '2024-01-02', '2024-01-03']);
    initializeTable();
  }, []);

  const initializeTable = () => {
    const rows: IR2TableRow[] = [];
    for (let i = 0; i < 24; i++) {
      rows.push({
        hora: `${i.toString().padStart(2, '0')}:00`,
        valor: 0
      });
    }
    setTableData(rows);
  };

  const handleDataChange = (value: string) => {
    setFormData(prev => ({ ...prev, dataPDP: value, empresa: '', usina: '' }));
    
    if (value) {
      setEmpresaOptions(['Empresa A', 'Empresa B', 'Empresa C']);
    } else {
      setEmpresaOptions([]);
    }
    
    setShowTable(false);
  };

  const handleEmpresaChange = (value: string) => {
    setFormData(prev => ({ ...prev, empresa: value, usina: '' }));
    
    if (value) {
      setUsinaOptions(['Usina 1', 'Usina 2', 'Usina 3']);
    } else {
      setUsinaOptions([]);
    }
    
    setShowTable(false);
  };

  const handleUsinaChange = (value: string) => {
    setFormData(prev => ({ ...prev, usina: value }));
    
    if (value) {
      loadMockData();
      setShowTable(true);
    } else {
      setShowTable(false);
    }
  };

  const loadMockData = () => {
    const updatedData = tableData.map((row, index) => ({
      ...row,
      valor: 50 + Math.random() * 50
    }));
    setTableData(updatedData);
  };

  const handleValueChange = (index: number, value: string) => {
    const updatedData = [...tableData];
    updatedData[index].valor = parseFloat(value) || 0;
    setTableData(updatedData);
  };

  const handleSave = () => {
    if (!formData.dataPDP || !formData.empresa || !formData.usina) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    console.log('Salvando IR2 - Dia -1:', { formData, tableData });
    alert('Dados do Dia -1 salvos com sucesso!');
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.titleBar}>
          <img src="/images/tit_sis_guideline.gif" alt="Sistema" />
        </div>
        <div className={styles.pageTitle}>
          <h2 className={styles.title}>Dia -1</h2>
        </div>
      </div>

      <div className={styles.formSection}>
        <div className={styles.formRow}>
          <label>
            <strong>Data PDP:</strong>
          </label>
          <select
            value={formData.dataPDP}
            onChange={(e) => handleDataChange(e.target.value)}
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
            onChange={(e) => handleEmpresaChange(e.target.value)}
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
            <option value="">Selecione</option>
            {usinaOptions.map(usina => (
              <option key={usina} value={usina}>{usina}</option>
            ))}
          </select>
          {showTable && (
            <button
              onClick={handleSave}
              className={styles.saveButton}
            >
              Salvar
            </button>
          )}
        </div>
      </div>

      {showTable && (
        <div className={styles.tableContainer}>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Hora</th>
                <th>Valor</th>
              </tr>
            </thead>
            <tbody>
              {tableData.map((row, index) => (
                <tr key={index}>
                  <td className={styles.horaCell}>{row.hora}</td>
                  <td>
                    <input
                      type="number"
                      step="0.01"
                      value={row.valor.toFixed(2)}
                      onChange={(e) => handleValueChange(index, e.target.value)}
                      className={styles.valueInput}
                    />
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default IR2;

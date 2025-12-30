import React, { useState } from 'react';
import styles from './Generation.module.css';

const Generation: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [selectedPlantType, setSelectedPlantType] = useState<string>('Todos');

  const handleSave = () => {
    console.log('Saving generation data...');
  };

  return (
    <div className={styles.container} data-testid="generation-container">
      <div className={styles.card}>
        <h1 className={styles.title} data-testid="generation-title">
          Coleta de Geração
        </h1>

        <div className={styles.filterSection}>
          <div className={styles.formGroup}>
            <label htmlFor="date" className={styles.label}>
              Data PDP:
            </label>
            <input
              type="date"
              id="date"
              value={selectedDate}
              onChange={(e) => setSelectedDate(e.target.value)}
              className={styles.select}
              data-testid="date-filter"
            />
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="company" className={styles.label}>
              Empresa:
            </label>
            <select
              id="company"
              value={selectedCompany}
              onChange={(e) => setSelectedCompany(e.target.value)}
              className={styles.select}
              data-testid="company-filter"
            >
              <option value="">Selecione...</option>
              <option value="1">Empresa A</option>
              <option value="2">Empresa B</option>
            </select>
          </div>

          <div className={styles.formGroup}>
            <label htmlFor="plantType" className={styles.label}>
              Tipos de Usina:
            </label>
            <select
              id="plantType"
              value={selectedPlantType}
              onChange={(e) => setSelectedPlantType(e.target.value)}
              className={styles.select}
              data-testid="plant-type-filter"
            >
              <option value="Todos">Todas</option>
              <option value="Hidro">Hidráulica</option>
              <option value="Termo">Térmica</option>
            </select>
          </div>

          <button className={styles.button} data-testid="btn-search">
            Pesquisar
          </button>
        </div>

        <div className={styles.tableContainer}>
          <table className={styles.table} data-testid="generation-table">
            <thead>
              <tr>
                <th>Usina</th>
                <th>Tipo</th>
                <th>Geração Programada (MWmed)</th>
                <th>Geração Verificada (MWmed)</th>
                <th>Observações</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Usina Teste 1</td>
                <td>Térmica</td>
                <td>
                  <input
                    type="number"
                    className={styles.input}
                    defaultValue={100}
                    data-testid="input-prog-1"
                  />
                </td>
                <td>
                  <input
                    type="number"
                    className={styles.input}
                    defaultValue={98}
                    data-testid="input-verif-1"
                  />
                </td>
                <td>
                  <input type="text" className={styles.input} data-testid="input-obs-1" />
                </td>
              </tr>
              <tr>
                <td>Usina Teste 2</td>
                <td>Hidráulica</td>
                <td>
                  <input
                    type="number"
                    className={styles.input}
                    defaultValue={200}
                    data-testid="input-prog-2"
                  />
                </td>
                <td>
                  <input
                    type="number"
                    className={styles.input}
                    defaultValue={205}
                    data-testid="input-verif-2"
                  />
                </td>
                <td>
                  <input type="text" className={styles.input} data-testid="input-obs-2" />
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className={styles.actions}>
          <button className={styles.cancelButton} data-testid="btn-cancel">
            Cancelar
          </button>
          <button className={styles.saveButton} onClick={handleSave} data-testid="btn-save">
            Salvar
          </button>
        </div>
      </div>
    </div>
  );
};

export default Generation;

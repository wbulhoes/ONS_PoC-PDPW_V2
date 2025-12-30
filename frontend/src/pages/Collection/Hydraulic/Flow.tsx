import React, { useState, useEffect } from 'react';
import styles from './Flow.module.css';

interface FlowData {
  id: number;
  usina: string;
  turbinada: number;
  vertida: number;
  afluente: number;
  cotaInicial: number;
  cotaFinal: number;
  outrasEstruturas: string;
  vazaoTransferida: number;
  comentario: string;
}

const Flow: React.FC = () => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [data, setData] = useState<FlowData[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  // Mock data for companies
  const companies = [
    { id: '1', name: 'Empresa A' },
    { id: '2', name: 'Empresa B' },
    { id: '3', name: 'Empresa C' },
  ];

  // Mock fetch data
  useEffect(() => {
    if (selectedCompany && selectedDate) {
      setLoading(true);
      // Simulate API call
      setTimeout(() => {
        const mockData: FlowData[] = [
          {
            id: 1,
            usina: 'Usina 1',
            turbinada: 100,
            vertida: 0,
            afluente: 120,
            cotaInicial: 500,
            cotaFinal: 501,
            outrasEstruturas: '',
            vazaoTransferida: 0,
            comentario: '',
          },
          {
            id: 2,
            usina: 'Usina 2',
            turbinada: 200,
            vertida: 50,
            afluente: 250,
            cotaInicial: 300,
            cotaFinal: 299,
            outrasEstruturas: '',
            vazaoTransferida: 10,
            comentario: 'Teste',
          },
        ];
        setData(mockData);
        setLoading(false);
      }, 500);
    } else {
      setData([]);
    }
  }, [selectedCompany, selectedDate]);

  const handleInputChange = (id: number, field: keyof FlowData, value: string | number) => {
    setData((prevData) =>
      prevData.map((item) => (item.id === id ? { ...item, [field]: value } : item))
    );
  };

  const handleSave = () => {
    console.log('Saving data:', data);
    alert('Dados salvos com sucesso!');
  };

  return (
    <div className={styles.container} data-testid="flow-container">
      <div className={styles.header}>
        <h1 className={styles.title}>Coleta de Vazão</h1>
      </div>

      <div className={styles.filterSection}>
        <div className={styles.formGroup}>
          <label htmlFor="date-select" className={styles.label}>
            Data PDP:
          </label>
          <input
            type="date"
            id="date-select"
            className={styles.select}
            value={selectedDate}
            onChange={(e) => setSelectedDate(e.target.value)}
            data-testid="date-select"
          />
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="company-select" className={styles.label}>
            Empresa:
          </label>
          <select
            id="company-select"
            className={styles.select}
            value={selectedCompany}
            onChange={(e) => setSelectedCompany(e.target.value)}
            data-testid="company-select"
          >
            <option value="">Selecione uma empresa</option>
            {companies.map((company) => (
              <option key={company.id} value={company.id}>
                {company.name}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.buttonGroup}>
          <button
            className={styles.saveButton}
            onClick={handleSave}
            disabled={!selectedCompany || loading}
            data-testid="btn-save"
          >
            Salvar
          </button>
        </div>
      </div>

      {loading ? (
        <div data-testid="loading">Carregando...</div>
      ) : (
        data.length > 0 && (
          <div className={styles.tableContainer}>
            <table className={styles.table} data-testid="flow-table">
              <thead>
                <tr>
                  <th>Usina</th>
                  <th>Turbinada</th>
                  <th>Vertida</th>
                  <th>Afluente</th>
                  <th>Cota Inicial</th>
                  <th>Cota Final</th>
                  <th>Outras Estr</th>
                  <th>Vazão Transferida</th>
                  <th>Comentário PDF</th>
                </tr>
              </thead>
              <tbody>
                {data.map((item, index) => (
                  <tr key={item.id} data-testid={`row-${index}`}>
                    <td className={styles.usinaName}>{item.usina}</td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.turbinada}
                        onChange={(e) =>
                          handleInputChange(item.id, 'turbinada', parseFloat(e.target.value))
                        }
                        data-testid={`input-turbinada-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.vertida}
                        onChange={(e) =>
                          handleInputChange(item.id, 'vertida', parseFloat(e.target.value))
                        }
                        data-testid={`input-vertida-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.afluente}
                        onChange={(e) =>
                          handleInputChange(item.id, 'afluente', parseFloat(e.target.value))
                        }
                        data-testid={`input-afluente-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.cotaInicial}
                        onChange={(e) =>
                          handleInputChange(item.id, 'cotaInicial', parseFloat(e.target.value))
                        }
                        data-testid={`input-cotaInicial-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.cotaFinal}
                        onChange={(e) =>
                          handleInputChange(item.id, 'cotaFinal', parseFloat(e.target.value))
                        }
                        data-testid={`input-cotaFinal-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="text"
                        className={styles.input}
                        value={item.outrasEstruturas}
                        onChange={(e) =>
                          handleInputChange(item.id, 'outrasEstruturas', e.target.value)
                        }
                        data-testid={`input-outrasEstruturas-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="number"
                        className={styles.input}
                        value={item.vazaoTransferida}
                        onChange={(e) =>
                          handleInputChange(item.id, 'vazaoTransferida', parseFloat(e.target.value))
                        }
                        data-testid={`input-vazaoTransferida-${index}`}
                      />
                    </td>
                    <td>
                      <input
                        type="text"
                        className={styles.input}
                        value={item.comentario}
                        onChange={(e) => handleInputChange(item.id, 'comentario', e.target.value)}
                        data-testid={`input-comentario-${index}`}
                      />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )
      )}
    </div>
  );
};

export default Flow;

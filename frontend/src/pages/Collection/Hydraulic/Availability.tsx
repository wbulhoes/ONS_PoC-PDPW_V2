import React, { useState, useEffect } from 'react';
import styles from './Availability.module.css';

interface Plant {
  id: string;
  name: string;
}

interface AvailabilityData {
  interval: number; // 1 to 48
  values: Record<string, number>; // plantId -> value
}

interface AvailabilityProps {
  initialType?: 'H' | 'T';
}

const Availability: React.FC<AvailabilityProps> = ({ initialType = 'H' }) => {
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedCompany, setSelectedCompany] = useState<string>('');
  const [selectedType, setSelectedType] = useState<'H' | 'T'>(initialType);
  const [selectedPlant, setSelectedPlant] = useState<string>('all');
  const [plants, setPlants] = useState<Plant[]>([]);
  const [data, setData] = useState<AvailabilityData[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  // Mock data for companies
  const companies = [
    { id: '1', name: 'Empresa A' },
    { id: '2', name: 'Empresa B' },
    { id: '3', name: 'Empresa C' },
  ];

  // Mock fetch plants
  useEffect(() => {
    if (selectedCompany && selectedType) {
      // Simulate API call to get plants
      const mockPlants =
        selectedType === 'H'
          ? [
              { id: 'h1', name: 'Hidrelétrica 1' },
              { id: 'h2', name: 'Hidrelétrica 2' },
              { id: 'h3', name: 'Hidrelétrica 3' },
            ]
          : [
              { id: 't1', name: 'Termelétrica 1' },
              { id: 't2', name: 'Termelétrica 2' },
            ];
      setPlants(mockPlants);
      setSelectedPlant('all');
    } else {
      setPlants([]);
    }
  }, [selectedCompany, selectedType]);

  // Mock fetch data
  useEffect(() => {
    if (selectedCompany && selectedDate && plants.length > 0) {
      setLoading(true);
      // Simulate API call
      setTimeout(() => {
        const mockData: AvailabilityData[] = Array.from({ length: 48 }, (_, i) => ({
          interval: i + 1,
          values: plants.reduce(
            (acc, plant) => ({ ...acc, [plant.id]: Math.floor(Math.random() * 100) }),
            {}
          ),
        }));
        setData(mockData);
        setLoading(false);
      }, 500);
    } else {
      setData([]);
    }
  }, [selectedCompany, selectedDate, plants]);

  const handleInputChange = (interval: number, plantId: string, value: string) => {
    const numValue = parseFloat(value) || 0;
    setData((prevData) =>
      prevData.map((item) =>
        item.interval === interval
          ? { ...item, values: { ...item.values, [plantId]: numValue } }
          : item
      )
    );
  };

  const handleSave = () => {
    console.log('Saving data:', data);
    alert('Dados salvos com sucesso!');
  };

  const getIntervalLabel = (interval: number) => {
    const startHour = Math.floor((interval - 1) / 2);
    const startMin = (interval - 1) % 2 === 0 ? '00' : '30';
    const endHour = Math.floor(interval / 2);
    const endMin = interval % 2 === 0 ? '00' : '30';

    const format = (h: number) => h.toString().padStart(2, '0');
    return `${format(startHour)}:${startMin} - ${format(endHour)}:${endMin}`;
  };

  const displayedPlants =
    selectedPlant === 'all' ? plants : plants.filter((p) => p.id === selectedPlant);

  return (
    <div className={styles.container} data-testid="availability-container">
      <div className={styles.header}>
        <h1 className={styles.title}>Coleta de Disponibilidade</h1>
      </div>

      <div className={styles.filterSection}>
        <div className={styles.formGroup}>
          <label className={styles.label}>Tipo de Usina:</label>
          <div className={styles.radioGroup}>
            <label className={styles.radioLabel}>
              <input
                type="radio"
                name="type"
                value="T"
                checked={selectedType === 'T'}
                onChange={() => setSelectedType('T')}
              />
              Usinas Térmicas
            </label>
            <label className={styles.radioLabel}>
              <input
                type="radio"
                name="type"
                value="H"
                checked={selectedType === 'H'}
                onChange={() => setSelectedType('H')}
              />
              Usinas Hidráulicas
            </label>
          </div>
        </div>

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
          >
            <option value="">Selecione uma empresa</option>
            {companies.map((company) => (
              <option key={company.id} value={company.id}>
                {company.name}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formGroup}>
          <label htmlFor="plant-select" className={styles.label}>
            Usinas:
          </label>
          <select
            id="plant-select"
            className={styles.select}
            value={selectedPlant}
            onChange={(e) => setSelectedPlant(e.target.value)}
            disabled={!selectedCompany}
          >
            <option value="all">Todas as Usinas</option>
            {plants.map((plant) => (
              <option key={plant.id} value={plant.id}>
                {plant.name}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.buttonGroup}>
          <button
            className={styles.saveButton}
            onClick={handleSave}
            disabled={!selectedCompany || loading}
          >
            Salvar
          </button>
        </div>
      </div>

      {loading ? (
        <div>Carregando...</div>
      ) : (
        data.length > 0 && (
          <div className={styles.tableContainer}>
            <table className={styles.table}>
              <thead>
                <tr>
                  <th>Intervalo</th>
                  {displayedPlants.map((plant) => (
                    <th key={plant.id}>{plant.name}</th>
                  ))}
                </tr>
              </thead>
              <tbody>
                {data.map((row) => (
                  <tr key={row.interval}>
                    <td className={styles.intervalCell}>{getIntervalLabel(row.interval)}</td>
                    {displayedPlants.map((plant) => (
                      <td key={plant.id}>
                        <input
                          type="number"
                          className={styles.input}
                          value={row.values[plant.id] || 0}
                          onChange={(e) =>
                            handleInputChange(row.interval, plant.id, e.target.value)
                          }
                        />
                      </td>
                    ))}
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

export default Availability;

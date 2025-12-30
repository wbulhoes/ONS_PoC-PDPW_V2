import React, { useState } from 'react';
import styles from './CompanyRegistry.module.css';

interface Company {
  codempre: number;
  nomempre: string;
  sigempre: string;
  idgtpoempre: number;
  contr: boolean;
  regiao: string;
  sistema: string;
  area_contr: boolean;
  area_nao_contr: string;
  infpdp: boolean;
  empresa_nao_contr: string;
}

// Mock data based on legacy columns
const MOCK_DATA: Company[] = [
  {
    codempre: 1,
    nomempre: 'Empresa Teste 1',
    sigempre: 'EMP1',
    idgtpoempre: 1,
    contr: true,
    regiao: 'SE',
    sistema: 'SIN',
    area_contr: false,
    area_nao_contr: 'Area 1',
    infpdp: true,
    empresa_nao_contr: 'Empresa X',
  },
  {
    codempre: 2,
    nomempre: 'Empresa Teste 2',
    sigempre: 'EMP2',
    idgtpoempre: 2,
    contr: false,
    regiao: 'S',
    sistema: 'SIN',
    area_contr: true,
    area_nao_contr: 'Area 2',
    infpdp: false,
    empresa_nao_contr: 'Empresa Y',
  },
];

const CompanyRegistry: React.FC = () => {
  const [companies] = useState<Company[]>(MOCK_DATA);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 8; // Matching legacy PageSize

  const totalPages = Math.ceil(companies.length / itemsPerPage);
  const startIndex = (currentPage - 1) * itemsPerPage;
  const currentCompanies = companies.slice(startIndex, startIndex + itemsPerPage);

  return (
    <div className={styles.container} data-testid="company-registry-container">
      <div className={styles.card}>
        <h1 className={styles.title} data-testid="company-registry-title">
          Empresas
        </h1>

        <div className={styles.tableContainer}>
          <table className={styles.table} data-testid="company-table">
            <thead>
              <tr>
                <th>Empresa</th>
                <th>Nome</th>
                <th>Sigla</th>
                <th>GTPO</th>
                <th>Controladora de Área</th>
                <th>Região</th>
                <th>Sistema</th>
                <th>Controlada por outra Empresa</th>
                <th>Área</th>
                <th>PDP Informado</th>
                <th>Empresa</th>
              </tr>
            </thead>
            <tbody>
              {currentCompanies.map((company) => (
                <tr key={company.codempre} data-testid={`company-row-${company.codempre}`}>
                  <td>{company.codempre}</td>
                  <td>{company.nomempre}</td>
                  <td>{company.sigempre}</td>
                  <td>{company.idgtpoempre}</td>
                  <td className={styles.checkbox}>
                    <input
                      type="checkbox"
                      checked={company.contr}
                      disabled
                      data-testid={`checkbox-contr-${company.codempre}`}
                    />
                  </td>
                  <td>{company.regiao}</td>
                  <td>{company.sistema}</td>
                  <td className={styles.checkbox}>
                    <input
                      type="checkbox"
                      checked={company.area_contr}
                      disabled
                      data-testid={`checkbox-area-contr-${company.codempre}`}
                    />
                  </td>
                  <td>{company.area_nao_contr}</td>
                  <td className={styles.checkbox}>
                    <input
                      type="checkbox"
                      checked={company.infpdp}
                      disabled
                      data-testid={`checkbox-infpdp-${company.codempre}`}
                    />
                  </td>
                  <td>{company.empresa_nao_contr}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>

        <div className={styles.pagination}>
          <button
            className={styles.pageButton}
            onClick={() => setCurrentPage((p) => Math.max(1, p - 1))}
            disabled={currentPage === 1}
            data-testid="btn-prev-page"
          >
            &lt; Anterior
          </button>
          <span className={styles.pageInfo}>
            Página {currentPage} de {totalPages}
          </span>
          <button
            className={styles.pageButton}
            onClick={() => setCurrentPage((p) => Math.min(totalPages, p + 1))}
            disabled={currentPage === totalPages}
            data-testid="btn-next-page"
          >
            Próxima &gt;
          </button>
        </div>
      </div>
    </div>
  );
};

export default CompanyRegistry;

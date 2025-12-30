import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import CompanyRegistry from '../../../../../src/pages/Admin/Company/CompanyRegistry';

describe('CompanyRegistry', () => {
  it('should render the component correctly', () => {
    render(<CompanyRegistry />);
    
    expect(screen.getByTestId('company-registry-container')).toBeInTheDocument();
    expect(screen.getByTestId('company-registry-title')).toHaveTextContent('Empresas');
    expect(screen.getByTestId('company-table')).toBeInTheDocument();
  });

  it('should render table headers correctly', () => {
    render(<CompanyRegistry />);
    
    const headers = [
      'Empresa', 'Nome', 'Sigla', 'GTPO', 'Controladora de Área',
      'Região', 'Sistema', 'Controlada por outra Empresa', 'Área',
      'PDP Informado', 'Empresa'
    ];

    headers.forEach(header => {
      expect(screen.getByText(header)).toBeInTheDocument();
    });
  });

  it('should render mock data rows', () => {
    render(<CompanyRegistry />);
    
    // Check first row
    expect(screen.getByTestId('company-row-1')).toBeInTheDocument();
    expect(screen.getByText('Empresa Teste 1')).toBeInTheDocument();
    expect(screen.getByText('EMP1')).toBeInTheDocument();
    
    // Check checkboxes state for first row
    const contrCheckbox1 = screen.getByTestId('checkbox-contr-1') as HTMLInputElement;
    expect(contrCheckbox1.checked).toBe(true);
    expect(contrCheckbox1).toBeDisabled();

    const areaContrCheckbox1 = screen.getByTestId('checkbox-area-contr-1') as HTMLInputElement;
    expect(areaContrCheckbox1.checked).toBe(false);
    expect(areaContrCheckbox1).toBeDisabled();

    const infpdpCheckbox1 = screen.getByTestId('checkbox-infpdp-1') as HTMLInputElement;
    expect(infpdpCheckbox1.checked).toBe(true);
    expect(infpdpCheckbox1).toBeDisabled();

    // Check second row
    expect(screen.getByTestId('company-row-2')).toBeInTheDocument();
    expect(screen.getByText('Empresa Teste 2')).toBeInTheDocument();
  });

  it('should handle pagination', () => {
    render(<CompanyRegistry />);
    
    const prevBtn = screen.getByTestId('btn-prev-page');
    const nextBtn = screen.getByTestId('btn-next-page');

    // Initial state
    expect(prevBtn).toBeDisabled();
    // Since we only have 2 items and page size is 8, next should also be disabled
    expect(nextBtn).toBeDisabled();
    
    expect(screen.getByText('Página 1 de 1')).toBeInTheDocument();
  });
});

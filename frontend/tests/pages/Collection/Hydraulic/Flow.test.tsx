import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import Flow from '../../../../src/pages/Collection/Hydraulic/Flow';
import { BrowserRouter } from 'react-router-dom';

describe('Flow Collection Page', () => {
  const renderComponent = () => {
    render(
      <BrowserRouter>
        <Flow />
      </BrowserRouter>
    );
  };

  it('renders the page title', () => {
    renderComponent();
    expect(screen.getByText('Coleta de Vazão')).toBeInTheDocument();
  });

  it('renders the filter section', () => {
    renderComponent();
    expect(screen.getByTestId('date-select')).toBeInTheDocument();
    expect(screen.getByTestId('company-select')).toBeInTheDocument();
    expect(screen.getByTestId('btn-save')).toBeInTheDocument();
  });

  it('loads data when company is selected', async () => {
    renderComponent();
    
    const companySelect = screen.getByTestId('company-select');
    fireEvent.change(companySelect, { target: { value: '1' } });

    await waitFor(() => {
      expect(screen.getByTestId('flow-table')).toBeInTheDocument();
    });

    expect(screen.getByText('Usina 1')).toBeInTheDocument();
    expect(screen.getByText('Usina 2')).toBeInTheDocument();
  });

  it('allows editing values', async () => {
    renderComponent();
    
    const companySelect = screen.getByTestId('company-select');
    fireEvent.change(companySelect, { target: { value: '1' } });

    await waitFor(() => {
      expect(screen.getByTestId('flow-table')).toBeInTheDocument();
    });

    const inputTurbinada = screen.getByTestId('input-turbinada-0');
    fireEvent.change(inputTurbinada, { target: { value: '150' } });

    expect(inputTurbinada).toHaveValue(150);
  });

  it('calls save function when save button is clicked', async () => {
    const consoleSpy = vi.spyOn(console, 'log');
    window.alert = vi.fn();

    renderComponent();
    
    const companySelect = screen.getByTestId('company-select');
    fireEvent.change(companySelect, { target: { value: '1' } });

    await waitFor(() => {
      expect(screen.getByTestId('flow-table')).toBeInTheDocument();
    });

    const saveButton = screen.getByTestId('btn-save');
    fireEvent.click(saveButton);

    expect(consoleSpy).toHaveBeenCalledWith('Saving data:', expect.any(Array));
    expect(window.alert).toHaveBeenCalledWith('Dados salvos com sucesso!');
  });
});

import { render, screen, fireEvent, waitFor, within } from '@testing-library/react';
import { vi, describe, it, expect } from 'vitest';
import Ramp from '../../src/pages/Collection/Other/Ramp';

describe('Ramp Component', () => {
  it('should render the initial state correctly', () => {
    render(<Ramp />);

    expect(screen.getByTestId('ramp-title')).toHaveTextContent('Coleta de Rampas de Geração');
    expect(screen.getByTestId('ramp-subtitle')).toHaveTextContent(
      'Registro da taxa de mudança de potência (MW/min) de usinas por intervalo'
    );
    expect(screen.getByTestId('date-filter')).toBeInTheDocument();
    expect(screen.getByTestId('company-filter')).toBeInTheDocument();
    expect(screen.getByTestId('plant-filter')).toBeInTheDocument();
    expect(screen.getByTestId('btn-search')).toBeDisabled();
  });

  it('should enable search button when all filters are selected', async () => {
    render(<Ramp />);

    // Wait for companies to load
    await waitFor(() => {
      expect(screen.getByTestId('company-filter')).not.toBeDisabled();
    });

    const companySelect = screen.getByTestId('company-filter');
    fireEvent.change(companySelect, { target: { value: '001' } });

    // Wait for plants to load
    await waitFor(() => {
      expect(screen.getByTestId('plant-filter')).not.toBeDisabled();
    });

    const plantSelect = screen.getByTestId('plant-filter');
    fireEvent.change(plantSelect, { target: { value: 'UHE001' } });

    expect(screen.getByTestId('btn-search')).not.toBeDisabled();
  });

  it('should display table when search is clicked', async () => {
    render(<Ramp />);

    // Select filters
    await waitFor(() => expect(screen.getByTestId('company-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('company-filter'), { target: { value: '001' } });

    await waitFor(() => expect(screen.getByTestId('plant-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('plant-filter'), { target: { value: 'UHE001' } });

    // Click search
    fireEvent.click(screen.getByTestId('btn-search'));

    // Check if table is displayed
    await waitFor(() => {
      expect(screen.getByTestId('table-wrapper')).toBeInTheDocument();
      expect(screen.getByTestId('ramp-table')).toBeInTheDocument();
    });

    // Check for 48 intervals (rows)
    // We can check a few rows to be sure
    expect(screen.getByTestId('table-row-1')).toBeInTheDocument();
    expect(screen.getByTestId('table-row-48')).toBeInTheDocument();
  });

  it('should allow editing values and update total/media', async () => {
    render(<Ramp />);

    // Setup and Search
    await waitFor(() => expect(screen.getByTestId('company-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('company-filter'), { target: { value: '001' } });
    await waitFor(() => expect(screen.getByTestId('plant-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('plant-filter'), { target: { value: 'UHE001' } });
    fireEvent.click(screen.getByTestId('btn-search'));
    await waitFor(() => expect(screen.getByTestId('ramp-table')).toBeInTheDocument());

    // Get input for first interval and plant UHE001
    // The plant code in mock data is 'UHE001' (based on selection) or checking column headers
    // In handleSearch mockData: codUsina: selectedPlant (which is UHE001)
    
    const input = screen.getByTestId('input-1-UHE001');
    
    // Change value
    fireEvent.change(input, { target: { value: '50' } });

    // Check if save button is enabled
    expect(screen.getByTestId('btn-save')).toBeEnabled();

    // Verify value update and calculations
    expect(input).toHaveValue(50);
    expect(screen.getByTestId('total-1')).toHaveTextContent('50.00');
    expect(screen.getByTestId('media-1')).toHaveTextContent('50.00');
  });

  it('should clear data when Clear button is clicked', async () => {
    render(<Ramp />);

    // Setup and Search
    await waitFor(() => expect(screen.getByTestId('company-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('company-filter'), { target: { value: '001' } });
    await waitFor(() => expect(screen.getByTestId('plant-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('plant-filter'), { target: { value: 'UHE001' } });
    fireEvent.click(screen.getByTestId('btn-search'));
    await waitFor(() => expect(screen.getByTestId('ramp-table')).toBeInTheDocument());

    // Click Clear
    fireEvent.click(screen.getByTestId('btn-clear'));

    // Verify table is gone
    expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
    expect(screen.queryByTestId('ramp-table')).not.toBeInTheDocument();
  });

  it('should show success message when saved', async () => {
    render(<Ramp />);

    // Setup and Search
    await waitFor(() => expect(screen.getByTestId('company-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('company-filter'), { target: { value: '001' } });
    await waitFor(() => expect(screen.getByTestId('plant-filter')).not.toBeDisabled());
    fireEvent.change(screen.getByTestId('plant-filter'), { target: { value: 'UHE001' } });
    fireEvent.click(screen.getByTestId('btn-search'));
    await waitFor(() => expect(screen.getByTestId('ramp-table')).toBeInTheDocument());

    // Modify data to enable save
    const input = screen.getByTestId('input-1-UHE001');
    fireEvent.change(input, { target: { value: '50' } });

    // Click Save
    fireEvent.click(screen.getByTestId('btn-save'));

    // Check for success message
    await waitFor(() => {
      expect(screen.getByTestId('success-message')).toHaveTextContent('Dados de rampas salvos com sucesso!');
    });
  });
});

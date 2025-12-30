import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ReplacementEnergyPage from '../../../../src/pages/Collection/Other/ReplacementEnergy';
import { replacementEnergyService } from '../../../../src/services/replacementEnergyService';

// Mock the service
vi.mock('../../../../src/services/replacementEnergyService', () => ({
  replacementEnergyService: {
    getByDateAndAgent: vi.fn(),
    save: vi.fn(),
  },
}));

describe('ReplacementEnergyPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should render the page title', () => {
    render(<ReplacementEnergyPage />);
    expect(screen.getByText('Energia de Reposição por Período')).toBeInTheDocument();
  });

  it('should load data when filters are selected', async () => {
    const mockData = [
      {
        id: '1',
        data: '2023-10-27',
        agenteId: '1',
        usinaId: '1',
        valores: Array(48).fill(10)
      }
    ];

    (replacementEnergyService.getByDateAndAgent as any).mockResolvedValue(mockData);

    render(<ReplacementEnergyPage />);

    // Select Agent
    const agentSelect = screen.getByLabelText('Empresa:');
    fireEvent.change(agentSelect, { target: { value: '1' } });

    // Select Plant
    const plantSelect = screen.getByLabelText('Usina:');
    fireEvent.change(plantSelect, { target: { value: '1' } });

    await waitFor(() => {
      expect(replacementEnergyService.getByDateAndAgent).toHaveBeenCalled();
    });

    // Check if table is rendered with data
    const inputs = screen.getAllByRole('spinbutton'); // number inputs
    expect(inputs).toHaveLength(48);
    expect((inputs[0] as HTMLInputElement).value).toBe('10');
  });

  it('should save data when save button is clicked', async () => {
    const mockData = [
      {
        id: '1',
        data: '2023-10-27',
        agenteId: '1',
        usinaId: '1',
        valores: Array(48).fill(10)
      }
    ];

    (replacementEnergyService.getByDateAndAgent as any).mockResolvedValue(mockData);
    (replacementEnergyService.save as any).mockResolvedValue(mockData[0]);

    render(<ReplacementEnergyPage />);

    // Select Agent and Plant to load data
    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: '1' } });
    fireEvent.change(screen.getByLabelText('Usina:'), { target: { value: '1' } });

    await waitFor(() => {
      expect(screen.getAllByRole('spinbutton')).toHaveLength(48);
    });

    // Change a value
    const inputs = screen.getAllByRole('spinbutton');
    fireEvent.change(inputs[0], { target: { value: '20' } });

    // Click Save
    const saveButton = screen.getByText('Salvar');
    fireEvent.click(saveButton);

    await waitFor(() => {
      expect(replacementEnergyService.save).toHaveBeenCalled();
      expect(screen.getByText('Dados salvos com sucesso!')).toBeInTheDocument();
    });
  });

  it('should handle empty data correctly', async () => {
    (replacementEnergyService.getByDateAndAgent as any).mockResolvedValue([]);

    render(<ReplacementEnergyPage />);

    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: '1' } });
    fireEvent.change(screen.getByLabelText('Usina:'), { target: { value: '1' } });

    await waitFor(() => {
      expect(replacementEnergyService.getByDateAndAgent).toHaveBeenCalled();
    });

    // Should render table with zeros
    const inputs = screen.getAllByRole('spinbutton');
    expect(inputs).toHaveLength(48);
    expect((inputs[0] as HTMLInputElement).value).toBe('0');
  });
});

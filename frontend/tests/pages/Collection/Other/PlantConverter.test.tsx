import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor, within } from '@testing-library/react';
import PlantConverterPage from '../../../../src/pages/Collection/Other/PlantConverter';
import { plantConverterService } from '../../../../src/services/plantConverterService';

// Mock the service
vi.mock('../../../../src/services/plantConverterService', () => ({
  plantConverterService: {
    getByAgent: vi.fn(),
    save: vi.fn(),
    delete: vi.fn(),
  },
}));

// Mock window.confirm
const mockConfirm = vi.fn();
window.confirm = mockConfirm;

describe('PlantConverterPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    mockConfirm.mockReturnValue(true);
  });

  it('should render the page title', () => {
    render(<PlantConverterPage />);
    expect(screen.getByText('Usina x Conversora')).toBeInTheDocument();
  });

  it('should load data when agent is selected', async () => {
    const mockData = [
      {
        id: '1',
        usinaId: '1',
        usinaNome: 'Usina A',
        conversoraId: '101',
        conversoraNome: 'Conversora X',
        perda: 2.5,
        prioridade: 1
      }
    ];

    (plantConverterService.getByAgent as any).mockResolvedValue(mockData);

    render(<PlantConverterPage />);

    // Initial load happens on mount with default agent '1'
    await waitFor(() => {
      expect(plantConverterService.getByAgent).toHaveBeenCalledWith('1');
      const table = screen.getByTestId('data-table');
      expect(within(table).getByText('Usina A')).toBeInTheDocument();
      expect(within(table).getByText('Conversora X')).toBeInTheDocument();
    });
  });

  it('should save new association', async () => {
    (plantConverterService.getByAgent as any).mockResolvedValue([]);
    (plantConverterService.save as any).mockResolvedValue({
      id: '2',
      usinaId: '1',
      usinaNome: 'Usina A',
      conversoraId: '101',
      conversoraNome: 'Conversora X',
      perda: 5.0,
      prioridade: 2
    });

    render(<PlantConverterPage />);

    // Fill form
    fireEvent.change(screen.getByLabelText('Usinas:'), { target: { value: '1' } });
    fireEvent.change(screen.getByLabelText('Usina Conversora:'), { target: { value: '101' } });
    fireEvent.change(screen.getByLabelText('Percentual de Perda (%):'), { target: { value: '5.0' } });
    fireEvent.change(screen.getByLabelText('Prioridade:'), { target: { value: '2' } });

    // Click Save
    fireEvent.click(screen.getByText('Salvar'));

    await waitFor(() => {
      expect(plantConverterService.save).toHaveBeenCalledWith({
        usinaId: '1',
        conversoraId: '101',
        perda: 5.0,
        prioridade: 2
      });
      expect(screen.getByText('Associação salva com sucesso!')).toBeInTheDocument();
      // Check if new item appears in table (mocked response)
      const table = screen.getByTestId('data-table');
      expect(within(table).getByText('Usina A')).toBeInTheDocument();
    });
  });

  it('should delete association', async () => {
    const mockData = [
      {
        id: '1',
        usinaId: '1',
        usinaNome: 'Usina A',
        conversoraId: '101',
        conversoraNome: 'Conversora X',
        perda: 2.5,
        prioridade: 1
      }
    ];

    (plantConverterService.getByAgent as any).mockResolvedValue(mockData);
    (plantConverterService.delete as any).mockResolvedValue();

    render(<PlantConverterPage />);

    await waitFor(() => {
      const table = screen.getByTestId('data-table');
      expect(within(table).getByText('Usina A')).toBeInTheDocument();
    });

    // Click Delete
    fireEvent.click(screen.getByText('Excluir'));

    expect(mockConfirm).toHaveBeenCalled();

    await waitFor(() => {
      expect(plantConverterService.delete).toHaveBeenCalledWith('1');
      expect(screen.getByText('Item excluído com sucesso!')).toBeInTheDocument();
      const table = screen.getByTestId('data-table');
      expect(within(table).queryByText('Usina A')).not.toBeInTheDocument();
    });
  });
});

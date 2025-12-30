import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import FuelShortageRestriction from '../../../../src/pages/Collection/Thermal/FuelShortageRestriction';
import { fuelShortageService } from '../../../../src/services/fuelShortageService';

// Mock do service
vi.mock('../../../../src/services/fuelShortageService', () => ({
  fuelShortageService: {
    getData: vi.fn(),
    saveData: vi.fn(),
  },
}));

const mockData = {
  usinas: [
    { codUsina: 'USINA1', nomeUsina: 'Usina A' },
    { codUsina: 'USINA2', nomeUsina: 'Usina B' },
  ],
  intervalos: [
    {
      id: 1,
      hora: '00:00',
      valores: { USINA1: 10, USINA2: 20 },
      total: 30,
    },
    {
      id: 2,
      hora: '00:30',
      valores: { USINA1: 15, USINA2: 25 },
      total: 40,
    },
  ],
};

describe('FuelShortageRestriction Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o título corretamente', () => {
    render(<FuelShortageRestriction />);
    expect(screen.getByText('Restrição por Falta de Combustível')).toBeInTheDocument();
  });

  it('deve carregar dados ao selecionar uma empresa', async () => {
    vi.mocked(fuelShortageService.getData).mockResolvedValue(mockData);

    render(<FuelShortageRestriction />);

    // Seleciona empresa
    const select = screen.getByLabelText('Empresa:');
    fireEvent.change(select, { target: { value: 'EMP001' } });

    // Verifica loading
    expect(screen.getByText('Carregando...')).toBeInTheDocument();

    // Aguarda carregamento
    await waitFor(() => {
      expect(screen.queryByText('Carregando...')).not.toBeInTheDocument();
    });

    // Verifica se dados foram renderizados
    // Usina A aparece no select e no header da tabela
    expect(screen.getAllByText('Usina A').length).toBeGreaterThan(0);
    expect(screen.getAllByText('Usina B').length).toBeGreaterThan(0);
    expect(screen.getByDisplayValue('10')).toBeInTheDocument();
    expect(screen.getByDisplayValue('20')).toBeInTheDocument();
  });

  it('deve filtrar por usina', async () => {
    vi.mocked(fuelShortageService.getData).mockResolvedValue(mockData);

    render(<FuelShortageRestriction />);

    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });
    await waitFor(() => expect(screen.queryByText('Carregando...')).not.toBeInTheDocument());

    // Filtra por Usina A
    const usinaSelect = screen.getByLabelText('Usina:');
    fireEvent.change(usinaSelect, { target: { value: 'USINA1' } });

    // Verifica se apenas Usina A está visível na tabela
    // O select ainda terá a opção Usina B, mas a tabela não deve ter o header Usina B
    const table = screen.getByRole('table');
    expect(table).toHaveTextContent('Usina A');
    expect(table).not.toHaveTextContent('Usina B');
  });

  it('deve atualizar valores e totais', async () => {
    vi.mocked(fuelShortageService.getData).mockResolvedValue(mockData);

    render(<FuelShortageRestriction />);

    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });
    await waitFor(() => expect(screen.queryByText('Carregando...')).not.toBeInTheDocument());

    // Edita valor da Usina A no intervalo 1
    const inputs = screen.getAllByDisplayValue('10'); 
    const input = inputs[0];
    
    fireEvent.change(input, { target: { value: '50' } });

    expect(input).toHaveValue(50);
    
    // Verifica se o total da linha atualizou (50 + 20 = 70)
    const rows = screen.getAllByRole('row');
    // Row 0: Header
    // Row 1: Interval 1
    const interval1Row = rows[1];
    expect(interval1Row).toHaveTextContent('70.00');
  });

  it('deve salvar os dados corretamente', async () => {
    vi.mocked(fuelShortageService.getData).mockResolvedValue(mockData);
    vi.mocked(fuelShortageService.saveData).mockResolvedValue();

    render(<FuelShortageRestriction />);

    fireEvent.change(screen.getByLabelText('Empresa:'), { target: { value: 'EMP001' } });
    await waitFor(() => expect(screen.queryByText('Carregando...')).not.toBeInTheDocument());

    const saveButton = screen.getByText('Salvar');
    fireEvent.click(saveButton);

    await waitFor(() => {
      expect(fuelShortageService.saveData).toHaveBeenCalled();
      expect(screen.getByText('Dados salvos com sucesso!')).toBeInTheDocument();
    });
  });
});

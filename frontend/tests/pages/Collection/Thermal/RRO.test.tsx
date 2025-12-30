import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import RRO from '../../../../src/pages/Collection/Thermal/RRO';
import { rroService } from '../../../../src/services/rroService';

// Mock service
vi.mock('../../../../src/services/rroService', () => ({
  rroService: {
    getOffers: vi.fn(),
    saveOffers: vi.fn(),
  },
}));

describe('RRO Page', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar os filtros iniciais', () => {
    render(<RRO />);
    expect(screen.getByText('RRO - Registro de Restrição Operativa')).toBeInTheDocument();
    expect(screen.getByLabelText('Data PDP:')).toBeInTheDocument();
    expect(screen.getByLabelText('Empresa:')).toBeInTheDocument();
    expect(screen.getByLabelText('Usina:')).toBeInTheDocument();
  });

  it('deve carregar dados ao selecionar filtros', async () => {
    const mockData = {
      dataPdp: '20231010',
      codEmpresa: 'EMP001',
      nomeEmpresa: 'Empresa 1',
      usinas: [
        {
          codUsina: 'US001',
          nomeUsina: 'Usina 1',
          ordem: 1,
          intervalos: Array.from({ length: 48 }, (_, i) => ({ intervalo: i + 1, valor: 10 })),
        },
      ],
    };

    (rroService.getOffers as any).mockResolvedValue(mockData);

    render(<RRO />);

    // Select Date
    const selectData = screen.getByLabelText('Data PDP:');
    await waitFor(() => expect(selectData.children.length).toBeGreaterThan(1));
    const firstDate = (selectData.children[1] as HTMLOptionElement).value;
    fireEvent.change(selectData, { target: { value: firstDate } });

    // Select Company
    const selectEmpresa = screen.getByLabelText('Empresa:');
    fireEvent.change(selectEmpresa, { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(rroService.getOffers).toHaveBeenCalled();
      expect(screen.getByText('Usina 1')).toBeInTheDocument();
    });
  });

  it('deve salvar dados', async () => {
    const mockData = {
      dataPdp: '20231010',
      codEmpresa: 'EMP001',
      nomeEmpresa: 'Empresa 1',
      usinas: [
        {
          codUsina: 'US001',
          nomeUsina: 'Usina 1',
          ordem: 1,
          intervalos: Array.from({ length: 48 }, (_, i) => ({ intervalo: i + 1, valor: 10 })),
        },
      ],
    };

    (rroService.getOffers as any).mockResolvedValue(mockData);
    (rroService.saveOffers as any).mockResolvedValue({});

    render(<RRO />);

    // Select Date & Company
    const selectData = screen.getByLabelText('Data PDP:');
    await waitFor(() => expect(selectData.children.length).toBeGreaterThan(1));
    const firstDate = (selectData.children[1] as HTMLOptionElement).value;
    fireEvent.change(selectData, { target: { value: firstDate } });

    const selectEmpresa = screen.getByLabelText('Empresa:');
    fireEvent.change(selectEmpresa, { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(screen.getByText('Usina 1')).toBeInTheDocument();
    });

    // Save
    const saveBtn = screen.getByText('Salvar');
    fireEvent.click(saveBtn);

    await waitFor(() => {
      expect(rroService.saveOffers).toHaveBeenCalled();
    });
  });
});

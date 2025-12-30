import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ExportOffer from '../../../../src/pages/Collection/Thermal/ExportOffer';
import { exportOfferService } from '../../../../src/services/exportOfferService';

// Mock service
vi.mock('../../../../src/services/exportOfferService', () => ({
  exportOfferService: {
    getOffers: vi.fn(),
    saveOffers: vi.fn(),
  },
}));

describe('ExportOffer Page', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar os filtros iniciais', () => {
    render(<ExportOffer />);
    expect(screen.getByTestId('page-title')).toHaveTextContent('Oferta de Exportação');
    expect(screen.getByTestId('select-data-pdp')).toBeInTheDocument();
    expect(screen.getByTestId('select-empresa')).toBeInTheDocument();
    expect(screen.getByTestId('select-usina')).toBeInTheDocument();
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
          codConversora: 'CV1',
          ordem: 1,
          intervalos: Array.from({ length: 48 }, (_, i) => ({ intervalo: i + 1, valor: 100 })),
        },
      ],
    };

    (exportOfferService.getOffers as any).mockResolvedValue(mockData);

    render(<ExportOffer />);

    // Select Date
    const selectData = screen.getByTestId('select-data-pdp');
    fireEvent.change(selectData, { target: { value: '20231010' } }); // Mock value needs to match options or be accepted

    // Since options are mocked in the component, we need to select a valid option or mock the options generation?
    // The component generates dates dynamically around today.
    // We should probably just select the first available option.
    
    // Wait for options to populate
    await waitFor(() => {
        expect(selectData.children.length).toBeGreaterThan(1);
    });
    const firstDate = (selectData.children[1] as HTMLOptionElement).value;
    fireEvent.change(selectData, { target: { value: firstDate } });

    // Select Company
    const selectEmpresa = screen.getByTestId('select-empresa');
    fireEvent.change(selectEmpresa, { target: { value: 'EMP001' } });

    await waitFor(() => {
      expect(exportOfferService.getOffers).toHaveBeenCalled();
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
          codConversora: 'CV1',
          ordem: 1,
          intervalos: Array.from({ length: 48 }, (_, i) => ({ intervalo: i + 1, valor: 100 })),
        },
      ],
    };

    (exportOfferService.getOffers as any).mockResolvedValue(mockData);
    (exportOfferService.saveOffers as any).mockResolvedValue({});

    render(<ExportOffer />);

    // Select Date & Company
    const selectData = screen.getByTestId('select-data-pdp');
    await waitFor(() => expect(selectData.children.length).toBeGreaterThan(1));
    const firstDate = (selectData.children[1] as HTMLOptionElement).value;
    fireEvent.change(selectData, { target: { value: firstDate } });

    const selectEmpresa = screen.getByTestId('select-empresa');
    fireEvent.change(selectEmpresa, { target: { value: 'EMP001' } });

    // Select Usina
    await waitFor(() => {
        const selectUsina = screen.getByTestId('select-usina');
        expect(selectUsina.children.length).toBeGreaterThan(1);
    });
    const selectUsina = screen.getByTestId('select-usina');
    fireEvent.change(selectUsina, { target: { value: 'US001' } });

    // Save
    const btnSave = screen.getByTestId('btn-save');
    fireEvent.click(btnSave);

    await waitFor(() => {
      expect(exportOfferService.saveOffers).toHaveBeenCalled();
      expect(screen.getByText('Dados salvos com sucesso!')).toBeInTheDocument();
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import ExportOfferAnalysis from '../../../../src/pages/Collection/Thermal/ExportOfferAnalysis';
import { exportOfferService } from '../../../../src/services/exportOfferService';

// Mock service
vi.mock('../../../../src/services/exportOfferService', () => ({
  exportOfferService: {
    getOffers: vi.fn(),
    approveOffers: vi.fn(),
    rejectOffers: vi.fn(),
  },
}));

describe('ExportOfferAnalysis Page', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar título correto para Análise ONS', () => {
    render(
      <MemoryRouter initialEntries={['/analise?AnaliseONS=S']}>
        <ExportOfferAnalysis />
      </MemoryRouter>
    );
    expect(screen.getByText('Análise de Oferta de Exportação')).toBeInTheDocument();
  });

  it('deve renderizar título correto para Análise Agente', () => {
    render(
      <MemoryRouter initialEntries={['/analise?AnaliseONS=N']}>
        <ExportOfferAnalysis />
      </MemoryRouter>
    );
    expect(screen.getByText('Análise de Exportação Agente')).toBeInTheDocument();
  });

  it('deve carregar dados e permitir aprovação', async () => {
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
          intervalos: Array.from({ length: 48 }, (_, i) => ({ intervalo: i + 1, valor: 100, valorOns: 100 })),
          status: 'Pendente',
        },
      ],
    };

    (exportOfferService.getOffers as any).mockResolvedValue(mockData);
    (exportOfferService.approveOffers as any).mockResolvedValue({});

    render(
      <MemoryRouter initialEntries={['/analise?AnaliseONS=S']}>
        <ExportOfferAnalysis />
      </MemoryRouter>
    );

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

    // Select Usina
    // First checkbox is "Select All", second is the usina row
    const checkboxes = screen.getAllByRole('checkbox');
    const usinaCheckbox = checkboxes[1]; 
    fireEvent.click(usinaCheckbox);

    // Approve
    const approveBtn = screen.getByText('Aprovar Selecionados');
    fireEvent.click(approveBtn);

    await waitFor(() => {
      expect(exportOfferService.approveOffers).toHaveBeenCalledWith(firstDate, 'EMP001', ['US001']);
    });
  });
});

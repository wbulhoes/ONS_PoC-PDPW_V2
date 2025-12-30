import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import ContractedInflexibilityPage from '../../src/pages/Administration/ContractedInflexibility';
import { contractedInflexibilityService } from '../../src/services/contractedInflexibilityService';

// Mock service
vi.mock('../../src/services/contractedInflexibilityService', () => ({
  contractedInflexibilityService: {
    getAll: vi.fn(),
    create: vi.fn(),
    update: vi.fn(),
    delete: vi.fn(),
  },
}));

const mockData = [
  {
    id: '1',
    codUsina: 'US001',
    nomeUsina: 'USINA A',
    dataInicio: '2023-01-01',
    dataFim: '2023-12-31',
    valor: 100.5,
    habilitado: true,
    contrato: 'Posterior a 2011'
  }
];

describe('ContractedInflexibilityPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    (contractedInflexibilityService.getAll as any).mockResolvedValue(mockData);
  });

  it('deve renderizar a página e carregar dados', async () => {
    render(<ContractedInflexibilityPage />);
    
    expect(screen.getByText('Inflexibilidade Contratada')).toBeInTheDocument();
    expect(screen.getByText('Carregando...')).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText('USINA A')).toBeInTheDocument();
    });
  });

  it('deve abrir modal de inclusão ao clicar em Incluir', async () => {
    render(<ContractedInflexibilityPage />);
    
    const btnIncluir = screen.getByTestId('btn-incluir');
    fireEvent.click(btnIncluir);

    expect(screen.getByText('Incluir Inflexibilidade')).toBeInTheDocument();
    expect(screen.getByTestId('input-usina')).toHaveValue('');
  });

  it('deve abrir modal de alteração ao selecionar item e clicar em Alterar', async () => {
    render(<ContractedInflexibilityPage />);
    
    await waitFor(() => {
      expect(screen.getByText('USINA A')).toBeInTheDocument();
    });

    const checkbox = screen.getByTestId('checkbox-1');
    fireEvent.click(checkbox);

    const btnAlterar = screen.getByTestId('btn-alterar');
    fireEvent.click(btnAlterar);

    expect(screen.getByText('Alterar Inflexibilidade')).toBeInTheDocument();
    expect(screen.getByTestId('input-usina')).toHaveValue('USINA A');
  });

  it('deve chamar serviço de criação ao salvar novo item', async () => {
    render(<ContractedInflexibilityPage />);
    
    fireEvent.click(screen.getByTestId('btn-incluir'));

    fireEvent.change(screen.getByTestId('input-usina'), { target: { value: 'NOVA USINA' } });
    fireEvent.change(screen.getByTestId('input-valor'), { target: { value: '200' } });
    
    fireEvent.click(screen.getByTestId('btn-salvar'));

    await waitFor(() => {
      expect(contractedInflexibilityService.create).toHaveBeenCalledWith(expect.objectContaining({
        nomeUsina: 'NOVA USINA',
        valor: 200
      }));
    });
  });

  it('deve chamar serviço de exclusão ao clicar em Excluir', async () => {
    // Mock window.confirm
    vi.spyOn(window, 'confirm').mockImplementation(() => true);

    render(<ContractedInflexibilityPage />);
    
    await waitFor(() => {
      expect(screen.getByText('USINA A')).toBeInTheDocument();
    });

    const checkbox = screen.getByTestId('checkbox-1');
    fireEvent.click(checkbox);

    const btnExcluir = screen.getByTestId('btn-excluir');
    fireEvent.click(btnExcluir);

    await waitFor(() => {
      expect(contractedInflexibilityService.delete).toHaveBeenCalledWith('1');
    });
  });
});

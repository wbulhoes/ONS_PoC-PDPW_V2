import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import GEC from '../GEC';

// Mock dos dados estáticos para garantir consistência nos testes
vi.mock('../../../types/gec', () => ({
  // Mantemos as interfaces mas não precisamos mockar os tipos
}));

describe('GEC Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o título da página', () => {
    render(<GEC />);
    expect(screen.getByText('Coleta de Geração de Energia Contratada (GEC)')).toBeInTheDocument();
  });

  it('deve renderizar os filtros iniciais', () => {
    render(<GEC />);
    
    expect(screen.getByLabelText('Data PDP')).toBeInTheDocument();
    expect(screen.getByLabelText('Empresa')).toBeInTheDocument();
    expect(screen.getByLabelText('Usina')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Pesquisar' })).toBeInTheDocument();
  });

  it('deve carregar dados ao clicar em Pesquisar', async () => {
    render(<GEC />);

    // Simula seleção de empresa
    const empresaSelect = screen.getByLabelText('Empresa');
    fireEvent.change(empresaSelect, { target: { value: 'E001' } });

    // Simula seleção de usina (necessário para habilitar o botão)
    const usinaSelect = screen.getByLabelText('Usina');
    fireEvent.change(usinaSelect, { target: { value: 'UGE1' } });

    // Simula clique em pesquisar
    const pesquisarBtn = screen.getByRole('button', { name: 'Pesquisar' });
    expect(pesquisarBtn).not.toBeDisabled();
    fireEvent.click(pesquisarBtn);

    // Verifica se a tabela foi renderizada
    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    // Verifica cabeçalhos da tabela
    expect(screen.getByText('Intervalo')).toBeInTheDocument();
    expect(screen.getByText('Total')).toBeInTheDocument();
  });

  it('deve permitir edição dos valores na tabela', async () => {
    render(<GEC />);

    // Carrega dados
    fireEvent.change(screen.getByLabelText('Empresa'), { target: { value: 'E001' } });
    fireEvent.change(screen.getByLabelText('Usina'), { target: { value: 'UGE1' } });
    fireEvent.click(screen.getByRole('button', { name: 'Pesquisar' }));

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    // Encontra um input de valor
    const inputs = screen.getAllByRole('spinbutton');
    const primeiroInputTabela = inputs.find(input => input.closest('table'));
    
    if (!primeiroInputTabela) throw new Error('Input da tabela não encontrado');

    // Altera valor
    fireEvent.change(primeiroInputTabela, { target: { value: '100' } });
    expect(primeiroInputTabela).toHaveValue(100);
  });

  it('deve exibir mensagem de sucesso ao salvar', async () => {
    render(<GEC />);

    // Carrega dados
    fireEvent.change(screen.getByLabelText('Empresa'), { target: { value: 'E001' } });
    fireEvent.change(screen.getByLabelText('Usina'), { target: { value: 'UGE1' } });
    fireEvent.click(screen.getByRole('button', { name: 'Pesquisar' }));

    await waitFor(() => {
      expect(screen.getByRole('table')).toBeInTheDocument();
    });

    // Modifica um valor para habilitar o botão salvar
    const inputs = screen.getAllByRole('spinbutton');
    const primeiroInputTabela = inputs.find(input => input.closest('table'));
    if (!primeiroInputTabela) throw new Error('Input da tabela não encontrado');
    fireEvent.change(primeiroInputTabela, { target: { value: '100' } });

    // Clica em salvar
    const salvarBtn = screen.getByRole('button', { name: 'Salvar' });
    expect(salvarBtn).not.toBeDisabled();
    fireEvent.click(salvarBtn);

    // Verifica mensagem de sucesso
    await waitFor(() => {
      expect(screen.getByText('Dados de GEC salvos com sucesso')).toBeInTheDocument();
    });
  });
});

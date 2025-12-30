import { describe, it, expect, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import GEC from '../../../../src/pages/Collection/Other/GEC';

const selectBasicFilters = async (plantValue = 'UGE1') => {
  const companyFilter = await screen.findByTestId('company-filter');
  await userEvent.selectOptions(companyFilter, 'E001');

  const plantFilter = await screen.findByTestId('plant-filter');
  await userEvent.selectOptions(plantFilter, plantValue);
};

describe('GEC Component', () => {
  beforeEach(() => {
    // Ensure clean state between tests
    document.body.innerHTML = '';
  });

  it('deve renderizar título e subtítulo', () => {
    render(<GEC />);

    expect(screen.getByTestId('gec-title')).toBeInTheDocument();
    expect(screen.getByTestId('gec-subtitle')).toBeInTheDocument();
  });

  it('deve renderizar filtros e desabilitar pesquisa quando incompleto', () => {
    render(<GEC />);

    expect(screen.getByTestId('date-filter')).toBeInTheDocument();
    expect(screen.getByTestId('company-filter')).toBeInTheDocument();
    const searchButton = screen.getByTestId('btn-search') as HTMLButtonElement;

    expect(searchButton.disabled).toBe(true);
  });

  it('deve carregar dados para uma usina e exibir 48 intervalos', async () => {
    render(<GEC />);

    await selectBasicFilters();

    const searchButton = screen.getByTestId('btn-search');
    await userEvent.click(searchButton);

    const tableWrapper = await screen.findByTestId('table-wrapper');
    expect(tableWrapper).toBeInTheDocument();

    const rows = await screen.findAllByTestId(/row-\d+/);
    expect(rows.length).toBe(48);

    const firstInput = screen.getByTestId('input-1-UGE1') as HTMLInputElement;
    expect(firstInput.value).toBe('1');

    const firstTotal = screen.getByTestId('total-1');
    expect(firstTotal.textContent).toBe('1.00');
  });

  it('deve carregar dados para todas as usinas e somar corretamente', async () => {
    render(<GEC />);

    await selectBasicFilters('all');

    const searchButton = screen.getByTestId('btn-search');
    await userEvent.click(searchButton);

    await waitFor(() => {
      expect(screen.getByTestId('table-wrapper')).toBeInTheDocument();
    });

    const headers = screen.getAllByTestId(/header-/);
    expect(headers.length).toBeGreaterThanOrEqual(3);

    const firstTotal = screen.getByTestId('total-1');
    expect(firstTotal.textContent).toBe('6.00');
  });

  it('deve atualizar totais ao editar valor e habilitar salvar', async () => {
    render(<GEC />);

    await selectBasicFilters();
    await userEvent.click(screen.getByTestId('btn-search'));

    const input = await screen.findByTestId('input-1-UGE1');
    await userEvent.clear(input);
    await userEvent.type(input, '50');

    const firstTotal = screen.getByTestId('total-1');
    expect(firstTotal.textContent).toBe('50.00');

    const saveButton = screen.getByTestId('btn-save') as HTMLButtonElement;
    expect(saveButton.disabled).toBe(false);

    await userEvent.click(saveButton);

    const success = await screen.findByTestId('success-message');
    expect(success.textContent).toContain('salvos');
  });

  it('deve exibir erro para valores negativos', async () => {
    render(<GEC />);

    await selectBasicFilters();
    await userEvent.click(screen.getByTestId('btn-search'));

    const input = await screen.findByTestId('input-1-UGE1');
    await userEvent.clear(input);
    await userEvent.type(input, '-5');

    const error = await screen.findByTestId('error-message');
    expect(error.textContent).toContain('não podem ser negativos');
  });

  it('deve limpar a tabela ao clicar em limpar', async () => {
    render(<GEC />);

    await selectBasicFilters();
    await userEvent.click(screen.getByTestId('btn-search'));

    const clearButton = await screen.findByTestId('btn-clear');
    await userEvent.click(clearButton);

    expect(screen.queryByTestId('table-wrapper')).not.toBeInTheDocument();
  });

  it('deve informar ausência de alterações ao salvar sem mudanças', async () => {
    render(<GEC />);

    await selectBasicFilters();
    await userEvent.click(screen.getByTestId('btn-search'));

    const saveButton = screen.getByTestId('btn-save');
    expect(saveButton).toBeDisabled();

    await userEvent.click(saveButton);

    expect(screen.queryByTestId('success-message')).not.toBeInTheDocument();
  });
});

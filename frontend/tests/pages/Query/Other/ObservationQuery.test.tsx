import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, act } from '@testing-library/react';
import ObservationQuery from '../../../../src/pages/Query/Other/ObservationQuery';

describe('ObservationQuery Component', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('should render the component title', async () => {
    render(<ObservationQuery />);
    expect(screen.getByText('Consulta de Observação Diária')).toBeInTheDocument();
  });

  it('should load available dates on mount', async () => {
    render(<ObservationQuery />);
    
    // Initially shows loading state in dropdown
    expect(screen.getByText('Carregando datas...')).toBeInTheDocument();

    // Fast-forward time to resolve the date fetching
    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    // Should show the mocked dates (formatted)
    expect(screen.getByText('25/10/2023')).toBeInTheDocument();
    expect(screen.getByText('24/10/2023')).toBeInTheDocument();
  });

  it('should fetch and display observation when search button is clicked', async () => {
    render(<ObservationQuery />);

    // Wait for dates to load
    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    const searchButton = screen.getByText('Pesquisar');
    fireEvent.click(searchButton);

    // Should show loading state on button
    expect(screen.getByText('Pesquisando...')).toBeInTheDocument();

    // Fast-forward time to resolve the observation fetching
    await act(async () => {
      vi.advanceTimersByTime(800);
    });

    // Should display the observation text
    const textArea = screen.getByRole('textbox') as HTMLTextAreaElement;
    expect(textArea.value).toContain('Observações do dia 25/10/2023');
    expect(textArea.value).toContain('Operação normal do sistema');
  });

  it('should update selected date when dropdown changes', async () => {
    render(<ObservationQuery />);

    // Wait for dates to load
    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    const select = screen.getByLabelText('Data PDP:');
    fireEvent.change(select, { target: { value: '2023-10-24' } });

    expect(select).toHaveValue('2023-10-24');
  });
});

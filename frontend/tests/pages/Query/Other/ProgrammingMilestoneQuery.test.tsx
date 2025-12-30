import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, act } from '@testing-library/react';
import ProgrammingMilestoneQuery from '../../../../src/pages/Query/Other/ProgrammingMilestoneQuery';

describe('ProgrammingMilestoneQuery Component', () => {
  beforeEach(() => {
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  it('should render the component title', async () => {
    render(<ProgrammingMilestoneQuery />);
    expect(screen.getByText('Consulta de Marcos de Programação')).toBeInTheDocument();
  });

  it('should load available dates on mount', async () => {
    render(<ProgrammingMilestoneQuery />);
    
    expect(screen.getByText('Carregando datas...')).toBeInTheDocument();

    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    expect(screen.getByText('25/10/2023')).toBeInTheDocument();
  });

  it('should fetch and display milestones when search button is clicked', async () => {
    render(<ProgrammingMilestoneQuery />);

    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    const searchButton = screen.getByText('Pesquisar');
    fireEvent.click(searchButton);

    expect(screen.getByText('Pesquisando...')).toBeInTheDocument();

    await act(async () => {
      vi.advanceTimersByTime(800);
    });

    // Check for milestone descriptions
    expect(screen.getByText('Abertura do Dia')).toBeInTheDocument();
    expect(screen.getByText('Coleta de Dados Hidráulicos')).toBeInTheDocument();
    
    // Check for status
    const executedStatuses = screen.getAllByText('Executado');
    expect(executedStatuses.length).toBeGreaterThan(0);
    
    const pendingStatuses = screen.getAllByText('Pendente');
    expect(pendingStatuses.length).toBeGreaterThan(0);
  });

  it('should display milestone details correctly', async () => {
    render(<ProgrammingMilestoneQuery />);

    await act(async () => {
      vi.advanceTimersByTime(500);
    });

    fireEvent.click(screen.getByText('Pesquisar'));

    await act(async () => {
      vi.advanceTimersByTime(800);
    });

    // Check specific details of the first milestone
    expect(screen.getByText('08:00')).toBeInTheDocument();
    
    const operators = screen.getAllByText('Operador A');
    expect(operators.length).toBeGreaterThan(0);
    
    expect(screen.getByText('08:01')).toBeInTheDocument();
  });
});

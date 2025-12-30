import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, waitFor, within, act } from '@testing-library/react';
import Comments from '../../../../src/pages/Query/DESSEM/Comments';

// Mock auth store
vi.mock('../../../../src/store/authStore', () => ({
  useAuthStore: () => ({
    user: { name: 'Test User' },
    isAuthenticated: true
  })
}));

describe('Comments Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    vi.useFakeTimers();
  });

  afterEach(() => {
    vi.useRealTimers();
  });

  const loadData = async () => {
    // Advance timers to trigger useEffect data loading
    await act(async () => {
      vi.advanceTimersByTime(2000);
    });
  };

  it('should render initial state correctly', async () => {
    render(<Comments />);

    // Check header
    expect(screen.getByText('Comentários DESSEM')).toBeInTheDocument();
    // ... (rest is same)
  });

  it('should load mock data after timeout', async () => {
    render(<Comments />);

    await loadData();

    const semSugestaoList = screen.getByTestId('list-sem-sugestao');
    expect(within(semSugestaoList).getByText('Usina A')).toBeInTheDocument();
    
    const listaUsinasList = screen.getByTestId('list-usinas');
    expect(within(listaUsinasList).getByText('Usina C')).toBeInTheDocument();
    
    const comSugestaoList = screen.getByTestId('list-com-sugestao');
    expect(within(comSugestaoList).getByText('Usina E')).toBeInTheDocument();
  });

  it('should handle filter changes', async () => {
    render(<Comments />);
    await loadData();
    // ...
  });

  it('should move items between lists', async () => {
    render(<Comments />);
    await loadData();

    const semSugestaoList = screen.getByTestId('list-sem-sugestao');
    expect(within(semSugestaoList).getByText('Usina A')).toBeInTheDocument();

    // Select item in Sem Sugestão
    const itemA = screen.getByTestId('item-sem-sugestao-1');
    fireEvent.click(itemA);
    expect(itemA).toHaveClass(/selected/);
    // ...
  });

  it('should handle form inputs', async () => {
    render(<Comments />);
    await loadData();

    expect(screen.getByTestId('form-usina')).toBeInTheDocument();
    // ...
  });

  it('should validate form before saving', async () => {
    // ...
  });

  it('should save successfully', async () => {
    render(<Comments />);
    await loadData();

    // Fill form
    fireEvent.change(screen.getByTestId('form-usina'), { target: { value: '1' } });
    fireEvent.change(screen.getByTestId('form-descricao'), { target: { value: 'Desc' } });
    
    // Mock alert
    const alertMock = vi.spyOn(window, 'alert').mockImplementation(() => {});

    // Save
    const saveBtn = screen.getByTestId('btn-save');
    fireEvent.click(saveBtn);

    // Advance timers for save async
    await act(async () => {
      vi.advanceTimersByTime(2000);
    });

    expect(alertMock).toHaveBeenCalledWith('Dados salvos com sucesso!');
  });
});

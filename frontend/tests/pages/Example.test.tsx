import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import Example from '../../src/pages/Example/Example';

describe('Example', () => {
  describe('Rendering', () => {
    it('renders page title', () => {
      render(<Example />);
      expect(screen.getByTestId('example-title')).toHaveTextContent('Página de Exemplo');
    });

    it('renders page subtitle', () => {
      render(<Example />);
      expect(screen.getByTestId('example-subtitle')).toHaveTextContent('Demonstração de funcionalidades e componentes do sistema');
    });

    it('renders all sections', () => {
      render(<Example />);
      expect(screen.getByTestId('example-counter-section')).toBeInTheDocument();
      expect(screen.getByTestId('example-list-section')).toBeInTheDocument();
      expect(screen.getByTestId('example-info-section')).toBeInTheDocument();
    });

    it('renders section titles', () => {
      render(<Example />);
      expect(screen.getByTestId('example-counter-title')).toHaveTextContent('Contador');
      expect(screen.getByTestId('example-list-title')).toHaveTextContent('Lista de Itens');
      expect(screen.getByTestId('example-info-title')).toHaveTextContent('Informações');
    });
  });

  describe('Counter functionality', () => {
    it('displays initial counter value as 0', () => {
      render(<Example />);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('0');
    });

    it('increments counter when + button is clicked', () => {
      render(<Example />);
      const incrementBtn = screen.getByTestId('example-btn-increment');
      fireEvent.click(incrementBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('1');
      fireEvent.click(incrementBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('2');
    });

    it('decrements counter when - button is clicked', () => {
      render(<Example />);
      const decrementBtn = screen.getByTestId('example-btn-decrement');
      fireEvent.click(decrementBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('-1');
      fireEvent.click(decrementBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('-2');
    });

    it('resets counter to 0 when reset button is clicked', () => {
      render(<Example />);
      const incrementBtn = screen.getByTestId('example-btn-increment');
      const resetBtn = screen.getByTestId('example-btn-reset');
      
      fireEvent.click(incrementBtn);
      fireEvent.click(incrementBtn);
      fireEvent.click(incrementBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('3');
      
      fireEvent.click(resetBtn);
      expect(screen.getByTestId('example-counter-display')).toHaveTextContent('0');
    });
  });

  describe('List functionality', () => {
    it('displays empty message when no items', () => {
      render(<Example />);
      expect(screen.getByTestId('example-empty-message')).toHaveTextContent('Nenhum item adicionado');
    });

    it('adds item to list when add button is clicked', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: 'Test Item' } });
      fireEvent.click(addBtn);

      expect(screen.getByTestId('example-item-0')).toBeInTheDocument();
      expect(screen.getByTestId('example-item-text-0')).toHaveTextContent('Test Item');
    });

    it('adds item to list when Enter key is pressed', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');

      fireEvent.change(input, { target: { value: 'Test Item' } });
      fireEvent.keyPress(input, { key: 'Enter', code: 'Enter', charCode: 13 });

      expect(screen.getByTestId('example-item-0')).toBeInTheDocument();
      expect(screen.getByTestId('example-item-text-0')).toHaveTextContent('Test Item');
    });

    it('clears input after adding item', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item') as HTMLInputElement;
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: 'Test Item' } });
      fireEvent.click(addBtn);

      expect(input.value).toBe('');
    });

    it('does not add empty items', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: '   ' } });
      fireEvent.click(addBtn);

      expect(screen.getByTestId('example-empty-message')).toBeInTheDocument();
    });

    it('adds multiple items to list', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: 'Item 1' } });
      fireEvent.click(addBtn);

      fireEvent.change(input, { target: { value: 'Item 2' } });
      fireEvent.click(addBtn);

      fireEvent.change(input, { target: { value: 'Item 3' } });
      fireEvent.click(addBtn);

      expect(screen.getByTestId('example-item-0')).toBeInTheDocument();
      expect(screen.getByTestId('example-item-1')).toBeInTheDocument();
      expect(screen.getByTestId('example-item-2')).toBeInTheDocument();
    });

    it('removes item from list when remove button is clicked', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: 'Item 1' } });
      fireEvent.click(addBtn);

      fireEvent.change(input, { target: { value: 'Item 2' } });
      fireEvent.click(addBtn);

      const removeBtn = screen.getByTestId('example-btn-remove-0');
      fireEvent.click(removeBtn);

      expect(screen.queryByTestId('example-item-text-0')).toHaveTextContent('Item 2');
      expect(screen.queryByTestId('example-item-1')).not.toBeInTheDocument();
    });

    it('shows empty message after removing all items', () => {
      render(<Example />);
      const input = screen.getByTestId('example-input-item');
      const addBtn = screen.getByTestId('example-btn-add-item');

      fireEvent.change(input, { target: { value: 'Test Item' } });
      fireEvent.click(addBtn);

      const removeBtn = screen.getByTestId('example-btn-remove-0');
      fireEvent.click(removeBtn);

      expect(screen.getByTestId('example-empty-message')).toBeInTheDocument();
    });
  });

  describe('Info section', () => {
    it('renders info texts', () => {
      render(<Example />);
      expect(screen.getByTestId('example-info-text-1')).toHaveTextContent('Esta página demonstra componentes básicos do sistema PDPw.');
      expect(screen.getByTestId('example-info-text-2')).toHaveTextContent('Utilize os exemplos acima para testar funcionalidades interativas.');
    });
  });

  describe('Data-testid attributes', () => {
    it('has all required data-testid attributes', () => {
      render(<Example />);
      
      expect(screen.getByTestId('example-container')).toBeInTheDocument();
      expect(screen.getByTestId('example-header')).toBeInTheDocument();
      expect(screen.getByTestId('example-title')).toBeInTheDocument();
      expect(screen.getByTestId('example-subtitle')).toBeInTheDocument();
      expect(screen.getByTestId('example-counter-section')).toBeInTheDocument();
      expect(screen.getByTestId('example-counter-card')).toBeInTheDocument();
      expect(screen.getByTestId('example-counter-display')).toBeInTheDocument();
      expect(screen.getByTestId('example-btn-increment')).toBeInTheDocument();
      expect(screen.getByTestId('example-btn-decrement')).toBeInTheDocument();
      expect(screen.getByTestId('example-btn-reset')).toBeInTheDocument();
      expect(screen.getByTestId('example-list-section')).toBeInTheDocument();
      expect(screen.getByTestId('example-list-card')).toBeInTheDocument();
      expect(screen.getByTestId('example-input-item')).toBeInTheDocument();
      expect(screen.getByTestId('example-btn-add-item')).toBeInTheDocument();
      expect(screen.getByTestId('example-item-list')).toBeInTheDocument();
      expect(screen.getByTestId('example-info-section')).toBeInTheDocument();
      expect(screen.getByTestId('example-info-card')).toBeInTheDocument();
    });
  });
});

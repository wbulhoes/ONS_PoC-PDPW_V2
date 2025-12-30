import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import ProgramacaoEnergetica from '../../src/pages/Collection/Electrical/ProgramacaoEnergetica';

describe('ProgramacaoEnergetica', () => {
  it('renders and allows load/save flow', async () => {
    render(<ProgramacaoEnergetica />);
    expect(screen.getByText('Cadastro Programação Energética')).toBeInTheDocument();

    const dateInput = screen.getByLabelText(/Data PDP/i);
    fireEvent.change(dateInput, { target: { value: '2025-12-25' } });

    const empresaSelect = screen.getByRole('combobox');
    fireEvent.change(empresaSelect, { target: { value: 'EMP001' } });

    const loadBtn = screen.getByText(/Carregar/i);
    expect(loadBtn).toBeInTheDocument();
  });
});

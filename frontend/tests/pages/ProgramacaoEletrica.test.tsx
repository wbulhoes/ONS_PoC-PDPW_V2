import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import ProgramacaoEletrica from '../../src/pages/Collection/Electrical/ProgramacaoEletrica';

describe('ProgramacaoEletrica page', () => {
  it('renders title and controls', () => {
    render(<ProgramacaoEletrica />);
    expect(screen.getByText('Programação Elétrica')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

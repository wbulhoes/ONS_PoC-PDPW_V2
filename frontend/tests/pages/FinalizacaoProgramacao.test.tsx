import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import FinalizacaoProgramacao from '../../src/pages/Finalization/FinalizacaoProgramacao';

describe('FinalizacaoProgramacao page', () => {
  it('renders and shows controls', () => {
    render(<FinalizacaoProgramacao />);
    expect(screen.getByText('Finalização da Programação')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

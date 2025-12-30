import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import PrevisaoEolica from '../../src/pages/Collection/Electrical/PrevisaoEolica';

describe('PrevisaoEolica page', () => {
  it('renders and shows controls', () => {
    render(<PrevisaoEolica />);
    expect(screen.getByText('Previsão Eólica')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

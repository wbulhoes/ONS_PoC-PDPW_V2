import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import Insumos from '../../src/pages/Collection/Insumos/Insumos';

describe('Insumos page', () => {
  it('renders header and controls', () => {
    render(<Insumos />);
    expect(screen.getByText('Coleta de Insumos')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

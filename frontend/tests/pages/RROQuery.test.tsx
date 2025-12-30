import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import RROQuery from '../../src/pages/Query/Other/RROQuery';

describe('RROQuery page', () => {
  it('renders and shows controls', () => {
    render(<RROQuery />);
    expect(screen.getByText('Consulta RRO')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

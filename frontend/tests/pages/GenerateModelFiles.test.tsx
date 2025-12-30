import React from 'react';
import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import GenerateModelFiles from '../../src/pages/ModelFiles/GenerateModelFiles';

describe('GenerateModelFiles page', () => {
  it('renders and displays controls', () => {
    render(<GenerateModelFiles />);
    expect(screen.getByText(/Gerar Arquivo Texto/)).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP/i)).toBeInTheDocument();
  });
});

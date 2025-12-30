import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import Footer from './Footer';

describe('Footer Component', () => {
  it('should render the footer with ONS copyright', () => {
    render(<Footer />);
    expect(screen.getByText(/ONS - Operador Nacional do Sistema Elétrico/i)).toBeInTheDocument();
  });

  it('should render the current year', () => {
    render(<Footer />);
    const currentYear = new Date().getFullYear();
    expect(screen.getByText(new RegExp(currentYear.toString()))).toBeInTheDocument();
  });

  it('should render the PDPw title', () => {
    render(<Footer />);
    expect(screen.getByText(/PDPw - Programação Diária de Produção/i)).toBeInTheDocument();
  });
});

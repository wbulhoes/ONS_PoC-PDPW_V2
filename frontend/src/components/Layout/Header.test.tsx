import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import Header from './Header';

describe('Header Component', () => {
  it('should render the header with default user name', () => {
    render(<Header />);
    expect(screen.getByText(/Bem-vindo, Usuário/i)).toBeInTheDocument();
  });

  it('should render the header with custom user name', () => {
    render(<Header userName="João Silva" />);
    expect(screen.getByText(/Bem-vindo, João Silva/i)).toBeInTheDocument();
  });

  it('should render the logo image', () => {
    render(<Header />);
    const logo = screen.getByAltText('PDPw Logo');
    expect(logo).toBeInTheDocument();
    expect(logo).toHaveAttribute('src', '/images/TituloPDPW.gif');
  });

  it('should render the title', () => {
    render(<Header />);
    expect(screen.getByText(/Programação Diária de Produção/i)).toBeInTheDocument();
  });
});

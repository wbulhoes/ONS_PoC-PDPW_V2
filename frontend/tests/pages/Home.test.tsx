import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import Home from '../../src/pages/Home/Home';

describe('Home', () => {
  it('renders welcome title', () => {
    render(<Home />);
    expect(screen.getByText('Bem-vindo ao PDPw')).toBeInTheDocument();
  });

  it('renders subtitle', () => {
    render(<Home />);
    expect(screen.getByText(/Sistema de Planejamento e Programação da Operação Energética/i)).toBeInTheDocument();
  });

  it('renders description text', () => {
    render(<Home />);
    expect(screen.getByText(/O PDPw é o sistema responsável/i)).toBeInTheDocument();
    expect(screen.getByText(/Utilize o menu de navegação/i)).toBeInTheDocument();
  });

  it('renders welcome card container', () => {
    const { container } = render(<Home />);
    const welcomeCard = container.querySelector('[class*="welcomeCard"]');
    expect(welcomeCard).toBeInTheDocument();
  });
});

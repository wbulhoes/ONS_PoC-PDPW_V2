import { render, screen } from '@testing-library/react';
import { describe, it, expect } from 'vitest';
import App from '../src/App';

describe('App', () => {
  it('renders without crashing', () => {
    render(<App />);
    expect(screen.getByText(/Bem-vindo ao PDPw/i)).toBeInTheDocument();
  });

  it('renders Layout component', () => {
    const { container } = render(<App />);
    const header = container.querySelector('header');
    expect(header).toBeInTheDocument();
  });

  it('renders Home page by default', () => {
    render(<App />);
    expect(screen.getByText(/Sistema de Planejamento e Programação da Operação Energética/i)).toBeInTheDocument();
  });
});

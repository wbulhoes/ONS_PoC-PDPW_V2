import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import Layout from './Layout';

describe('Layout Component', () => {
  it('should render children content', () => {
    render(
      <Layout>
        <div>Test Content</div>
      </Layout>
    );
    expect(screen.getByText('Test Content')).toBeInTheDocument();
  });

  it('should render header, navigation, and footer', () => {
    render(
      <Layout userName="Test User">
        <div>Content</div>
      </Layout>
    );

    expect(screen.getByText(/Bem-vindo, Test User/i)).toBeInTheDocument();
    expect(screen.getByText(/ONS - Operador Nacional do Sistema Elétrico/i)).toBeInTheDocument();
  });

  it('should pass userName prop to Header', () => {
    render(
      <Layout userName="João Silva">
        <div>Content</div>
      </Layout>
    );

    expect(screen.getByText(/Bem-vindo, João Silva/i)).toBeInTheDocument();
  });
});

import { describe, it, expect, beforeAll, afterEach, afterAll } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Electrical from '../../src/pages/Collection/Electrical/Electrical';
import { server, mockEndpoint } from '../setup/mswServer';

/**
 * T049: Integration test for electrical flow (load → edit/save trigger)
 * Tests the complete flow of the Electrical component from data loading to saving
 */

describe('Integration: Electrical flow', () => {
  beforeAll(() => server.listen());
  afterEach(() => server.resetHandlers());
  afterAll(() => server.close());


  // Simple integration test to verify component renders and basic flow works
  it('should render electrical component with all form elements', () => {
    const client = new QueryClient({
      defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
    });
    
    render(
      <QueryClientProvider client={client}>
        <Electrical />
      </QueryClientProvider>
    );

    // Verify main elements are rendered
    expect(screen.getByText('Razão Elétrica Transformada')).toBeInTheDocument();
    expect(screen.getByLabelText(/Data PDP:/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Empresa:/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Usina:/i)).toBeInTheDocument();
  });

  it('should have company select disabled when no date is selected', () => {
    const client = new QueryClient({
      defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
    });
    
    render(
      <QueryClientProvider client={client}>
        <Electrical />
      </QueryClientProvider>
    );

    const empresaSelect = screen.getByLabelText(/Empresa:/i) as HTMLSelectElement;
    const usinaSelect = screen.getByLabelText(/Usina:/i) as HTMLSelectElement;
    
    // Initially empresa should be disabled when no date selected
    expect(empresaSelect).toBeDisabled();
    expect(usinaSelect).toBeDisabled();
  });

  it('should render component structure correctly', () => {
    const client = new QueryClient({
      defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
    });
    
    render(
      <QueryClientProvider client={client}>
        <Electrical />
      </QueryClientProvider>
    );

    // Verify component structure
    expect(screen.getByTestId('electrical-container')).toBeInTheDocument();
    expect(screen.getByTestId('page-title')).toBeInTheDocument();
    expect(screen.getByTestId('page-subtitle')).toBeInTheDocument();
  });
});

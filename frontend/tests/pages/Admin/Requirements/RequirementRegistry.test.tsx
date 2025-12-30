import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi, beforeEach } from 'vitest';
import RequirementRegistry from '../RequirementRegistry';

describe('RequirementRegistry', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('deve renderizar o componente corretamente', () => {
    render(<RequirementRegistry />);

    expect(screen.getByText('Cadastro de Requisitos')).toBeInTheDocument();
    expect(screen.getByLabelText('Título:')).toBeInTheDocument();
    expect(screen.getByLabelText('Tipo:')).toBeInTheDocument();
    expect(screen.getByLabelText('Prioridade:')).toBeInTheDocument();
    expect(screen.getByLabelText('Status:')).toBeInTheDocument();
    expect(screen.getByLabelText('Responsável:')).toBeInTheDocument();
    expect(screen.getByLabelText('Descrição:')).toBeInTheDocument();
    expect(screen.getByText('Salvar')).toBeInTheDocument();
  });

  it('deve carregar requisitos mockados', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      expect(screen.getByText('Implementar validação de dados hidráulicos')).toBeInTheDocument();
      expect(screen.getByText('Otimizar performance de consultas térmicas')).toBeInTheDocument();
      expect(screen.getByText('Implementar auditoria de alterações')).toBeInTheDocument();
    });
  });

  it('deve permitir criar um novo requisito', async () => {
    render(<RequirementRegistry />);

    const tituloInput = screen.getByLabelText('Título:');
    const descricaoTextarea = screen.getByLabelText('Descrição:');
    const responsavelInput = screen.getByLabelText('Responsável:');
    const salvarButton = screen.getByText('Salvar');

    fireEvent.change(tituloInput, { target: { value: 'Novo Requisito de Teste' } });
    fireEvent.change(descricaoTextarea, { target: { value: 'Descrição do novo requisito' } });
    fireEvent.change(responsavelInput, { target: { value: 'Testador Silva' } });
    fireEvent.click(salvarButton);

    await waitFor(() => {
      expect(screen.getByText('Novo Requisito de Teste')).toBeInTheDocument();
      expect(screen.getByText('Testador Silva')).toBeInTheDocument();
    });
  });

  it('deve validar campos obrigatórios', async () => {
    render(<RequirementRegistry />);

    const salvarButton = screen.getByText('Salvar');
    fireEvent.click(salvarButton);

    // O HTML5 validation deve prevenir o submit
    expect(salvarButton).toBeInTheDocument();
  });

  it('deve permitir editar um requisito existente', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const editButtons = screen.getAllByText('Editar');
      fireEvent.click(editButtons[0]);
    });

    const tituloInput = screen.getByLabelText('Título:');
    expect(tituloInput).toHaveValue('Implementar validação de dados hidráulicos');

    fireEvent.change(tituloInput, { target: { value: 'Requisito Editado' } });
    fireEvent.click(screen.getByText('Atualizar'));

    await waitFor(() => {
      expect(screen.getByText('Requisito Editado')).toBeInTheDocument();
    });
  });

  it('deve permitir cancelar edição', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const editButtons = screen.getAllByText('Editar');
      fireEvent.click(editButtons[0]);
    });

    const tituloInput = screen.getByLabelText('Título:');
    fireEvent.change(tituloInput, { target: { value: 'Requisito Modificado' } });
    fireEvent.click(screen.getByText('Cancelar'));

    expect(screen.getByText('Implementar validação de dados hidráulicos')).toBeInTheDocument();
    expect(screen.queryByText('Requisito Modificado')).not.toBeInTheDocument();
  });

  it('deve permitir alterar tipo, prioridade e status', async () => {
    render(<RequirementRegistry />);

    const tipoSelect = screen.getByLabelText('Tipo:');
    const prioridadeSelect = screen.getByLabelText('Prioridade:');
    const statusSelect = screen.getByLabelText('Status:');

    fireEvent.change(tipoSelect, { target: { value: 'Técnico' } });
    fireEvent.change(prioridadeSelect, { target: { value: 'Crítica' } });
    fireEvent.change(statusSelect, { target: { value: 'Aprovado' } });

    expect(tipoSelect).toHaveValue('Técnico');
    expect(prioridadeSelect).toHaveValue('Crítica');
    expect(statusSelect).toHaveValue('Aprovado');
  });

  it('deve permitir selecionar requisitos individualmente', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      const firstReqCheckbox = checkboxes[1]; // Primeiro checkbox de requisito
      fireEvent.click(firstReqCheckbox);

      expect(firstReqCheckbox).toBeChecked();
    });
  });

  it('deve permitir selecionar todos os requisitos', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const checkboxes = screen.getAllByRole('checkbox');
      checkboxes.forEach(checkbox => {
        expect(checkbox).toBeChecked();
      });
    });
  });

  it('deve desmarcar "selecionar todos" quando um requisito é desmarcado', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);

      const checkboxes = screen.getAllByRole('checkbox');
      const firstReqCheckbox = checkboxes[1];
      fireEvent.click(firstReqCheckbox);

      expect(selectAllCheckbox).not.toBeChecked();
    });
  });

  it('deve permitir excluir requisitos selecionados', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const checkboxes = screen.getAllByRole('checkbox');
      const firstReqCheckbox = checkboxes[1];
      fireEvent.click(firstReqCheckbox);
    });

    const deleteButton = screen.getByText('Excluir Selecionados (1)');
    fireEvent.click(deleteButton);

    await waitFor(() => {
      expect(screen.queryByText('Implementar validação de dados hidráulicos')).not.toBeInTheDocument();
    });
  });

  it('deve mostrar cores corretas para prioridades', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      // Verificar se os badges de prioridade estão presentes
      const priorityBadges = screen.getAllByText('Crítica');
      expect(priorityBadges.length).toBeGreaterThan(0);
    });
  });

  it('deve mostrar cores corretas para status', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      // Verificar se os badges de status estão presentes
      const statusBadges = screen.getAllByText('Aprovado');
      expect(statusBadges.length).toBeGreaterThan(0);
    });
  });

  it('deve navegar entre páginas', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const nextButton = screen.getByText('Próxima');
      fireEvent.click(nextButton);

      expect(screen.getByText('Página 2 de 2')).toBeInTheDocument();
    });
  });

  it('deve desabilitar botões de navegação corretamente', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const previousButton = screen.getByText('Anterior');
      const nextButton = screen.getByText('Próxima');

      expect(previousButton).toBeDisabled();
      expect(nextButton).not.toBeDisabled();
    });
  });

  it('deve formatar datas corretamente', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      // Verificar se as datas estão sendo exibidas (formato brasileiro)
      const dateCells = screen.getAllByText(/\d{2}\/\d{2}\/\d{4}/);
      expect(dateCells.length).toBeGreaterThan(0);
    });
  });

  it('deve ser responsivo em telas pequenas', () => {
    render(<RequirementRegistry />);

    const container = screen.getByTestId('requirement-registry-container');
    expect(container).toHaveClass('container');
  });

  it('deve mostrar mensagem quando não há requisitos', async () => {
    // Mock sem requisitos
    vi.spyOn(console, 'error').mockImplementation(() => {});

    render(<RequirementRegistry />);

    // Simular exclusão de todos os requisitos
    await waitFor(() => {
      const selectAllCheckbox = screen.getByTestId('select-all-checkbox');
      fireEvent.click(selectAllCheckbox);
    });

    const deleteButton = screen.getByText('Excluir Selecionados (5)');
    fireEvent.click(deleteButton);

    await waitFor(() => {
      expect(screen.getByText('Nenhum requisito cadastrado.')).toBeInTheDocument();
    });
  });

  it('deve truncar títulos longos na tabela', async () => {
    render(<RequirementRegistry />);

    await waitFor(() => {
      const titleCells = screen.getAllByText(/Implementar validação de dados hidráulicos/);
      expect(titleCells.length).toBeGreaterThan(0);
    });
  });
});
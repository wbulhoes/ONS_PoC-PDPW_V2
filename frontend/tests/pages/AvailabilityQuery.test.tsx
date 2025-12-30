import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent, waitFor, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import AvailabilityQuery from './AvailabilityQuery';
import { AvailabilityQueryService } from '../../../services/availabilityQueryService';
import {
  TipoUsina,
  StatusDisponibilidade,
  TIPO_USINA_LABELS,
  STATUS_LABELS,
} from '../../../types/availabilityQuery';

// Mock do serviço
vi.mock('../../../services/availabilityQueryService');

// Mock dados
const mockEmpresas = [
  { id: 1, codigo: '1-EMP', nome: 'Empresa A' },
  { id: 2, codigo: '2-EMP', nome: 'Empresa B' },
];

const mockUsinas = [
  { id: 1, codigo: 'USN001', nome: 'Itaipu', capacidadeInstalada: 14000, tipoUsina: TipoUsina.HIDRELETRICA },
  { id: 2, codigo: 'USN002', nome: 'Belo Monte', capacidadeInstalada: 11000, tipoUsina: TipoUsina.HIDRELETRICA },
];

const mockDisponibilidadeData = [
  {
    id: 1,
    dataPDP: '2024-12-27',
    codEmpresa: '1-EMP',
    nomeEmpresa: 'Empresa A',
    codUsina: 'USN001',
    nomeUsina: 'Itaipu',
    tipoUsina: TipoUsina.HIDRELETRICA,
    intervalo: 1,
    horario: '00:00 - 00:30',
    capacidadeMaximaDisponivel: 14000,
    capacidadeMinimDisponivel: 10000,
    percentualDisponibilidade: 95.5,
    statusDisponibilidade: StatusDisponibilidade.ATIVA,
    motivoIndisponibilidade: null,
    dataRegistro: '2024-12-27T10:00:00Z',
    usuarioRegistro: 'sistema',
    dataAtualizacao: null,
    usuarioAtualizacao: null,
  },
  {
    id: 2,
    dataPDP: '2024-12-27',
    codEmpresa: '1-EMP',
    nomeEmpresa: 'Empresa A',
    codUsina: 'USN002',
    nomeUsina: 'Belo Monte',
    tipoUsina: TipoUsina.HIDRELETRICA,
    intervalo: 1,
    horario: '00:00 - 00:30',
    capacidadeMaximaDisponivel: 11000,
    capacidadeMinimDisponivel: 8000,
    percentualDisponibilidade: 88.3,
    statusDisponibilidade: StatusDisponibilidade.MANUTENCAO,
    motivoIndisponibilidade: 'Manutenção planejada',
    dataRegistro: '2024-12-27T10:00:00Z',
    usuarioRegistro: 'sistema',
    dataAtualizacao: null,
    usuarioAtualizacao: null,
  },
];

const mockQueryResponse = {
  dados: mockDisponibilidadeData,
  total: 2,
  pagina: 1,
  itensPorPagina: 20,
  totalPaginas: 1,
};

const mockAggregatedData = [
  {
    codUsina: 'USN001',
    nomeUsina: 'Itaipu',
    tipoUsina: TipoUsina.HIDRELETRICA,
    capacidadeMaximaMedia: 14000,
    capacidadeMinimaMedia: 10000,
    percentualDisponibilidadeMedia: 95.5,
    quantidadeRegistros: 48,
    statusPredominante: StatusDisponibilidade.ATIVA,
  },
];

describe('AvailabilityQuery Component', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    // Setup mocks padrão
    (AvailabilityQueryService.getEmpresas as any).mockResolvedValue(mockEmpresas);
    (AvailabilityQueryService.getUsinasByEmpresa as any).mockResolvedValue(mockUsinas);
    (AvailabilityQueryService.query as any).mockResolvedValue(mockQueryResponse);
    (AvailabilityQueryService.getAggregatedData as any).mockResolvedValue(mockAggregatedData);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  // ========== TESTES: RENDERIZAÇÃO ==========
  describe('Renderização', () => {
    it('deve renderizar o componente corretamente', () => {
      render(<AvailabilityQuery />);
      expect(screen.getByText('📊 Consulta de Disponibilidade')).toBeInTheDocument();
      expect(screen.getByText('Consulte dados históricos de disponibilidade de usinas hidráulicas e térmicas')).toBeInTheDocument();
    });

    it('deve carrer empresas ao montar o componente', async () => {
      render(<AvailabilityQuery />);
      await waitFor(() => {
        expect(AvailabilityQueryService.getEmpresas).toHaveBeenCalled();
      });
    });

    it('deve renderizar seção de filtros', () => {
      render(<AvailabilityQuery />);
      expect(screen.getByText('Filtros de Busca')).toBeInTheDocument();
      expect(screen.getByLabelText('Data PDP - Início')).toBeInTheDocument();
      expect(screen.getByLabelText('Data PDP - Fim')).toBeInTheDocument();
      expect(screen.getByLabelText('Empresa')).toBeInTheDocument();
      expect(screen.getByLabelText('Tipo de Usina')).toBeInTheDocument();
    });

    it('deve renderizar botões de ação', () => {
      render(<AvailabilityQuery />);
      expect(screen.getByRole('button', { name: /Buscar/i })).toBeInTheDocument();
      expect(screen.getByRole('button', { name: /Limpar/i })).toBeInTheDocument();
    });

    it('deve mostrar mensagem inicial quando nenhuma busca foi feita', () => {
      render(<AvailabilityQuery />);
      expect(screen.getByText(/Selecione filtros e clique em "Buscar"/i)).toBeInTheDocument();
    });
  });

  // ========== TESTES: CARREGAMENTO DE DADOS ==========
  describe('Carregamento de Dados', () => {
    it('deve carregar empresas ao montar', async () => {
      render(<AvailabilityQuery />);

      await waitFor(() => {
        const empresaSelect = screen.getByLabelText('Empresa') as HTMLSelectElement;
        expect(empresaSelect.options.length).toBeGreaterThan(1);
      });
    });

    it('deve carregar usinas quando empresa é selecionada', async () => {
      render(<AvailabilityQuery />);

      const empresaSelect = screen.getByLabelText('Empresa') as HTMLSelectElement;

      await waitFor(() => {
        expect(empresaSelect.options.length).toBeGreaterThan(1);
      });

      fireEvent.change(empresaSelect, { target: { value: '1-EMP' } });

      await waitFor(() => {
        expect(AvailabilityQueryService.getUsinasByEmpresa).toHaveBeenCalled();
      });
    });
  });

  // ========== TESTES: FILTROS ==========
  describe('Filtros', () => {
    it('deve atualizar filtro de data início', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });

      await waitFor(() => {
        expect(dateInput.value).toBe('2024-12-27');
      });
    });

    it('deve atualizar filtro de data fim', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Fim') as HTMLInputElement;

      fireEvent.change(dateInput, { target: { value: '2024-12-28' } });

      await waitFor(() => {
        expect(dateInput.value).toBe('2024-12-28');
      });
    });

    it('deve atualizar filtro de tipo usina', async () => {
      render(<AvailabilityQuery />);
      const tipoSelect = screen.getByLabelText('Tipo de Usina') as HTMLSelectElement;

      fireEvent.change(tipoSelect, { target: { value: TipoUsina.TERMELETRICA } });

      await waitFor(() => {
        expect(tipoSelect.value).toBe(TipoUsina.TERMELETRICA);
      });
    });

    it('deve atualizar filtro de status', async () => {
      render(<AvailabilityQuery />);
      const statusSelect = screen.getByLabelText('Status') as HTMLSelectElement;

      fireEvent.change(statusSelect, { target: { value: StatusDisponibilidade.ATIVA } });

      await waitFor(() => {
        expect(statusSelect.value).toBe(StatusDisponibilidade.ATIVA);
      });
    });

    it('deve desabilitar select de usina se nenhuma empresa estiver selecionada', () => {
      render(<AvailabilityQuery />);
      const usinaSelect = screen.getByLabelText('Usina') as HTMLSelectElement;

      expect(usinaSelect.disabled).toBe(true);
    });

    it('deve habilitar select de usina quando empresa for selecionada', async () => {
      render(<AvailabilityQuery />);
      const empresaSelect = screen.getByLabelText('Empresa') as HTMLSelectElement;
      const usinaSelect = screen.getByLabelText('Usina') as HTMLSelectElement;

      await waitFor(() => {
        expect(empresaSelect.options.length).toBeGreaterThan(1);
      });

      fireEvent.change(empresaSelect, { target: { value: '1-EMP' } });

      await waitFor(() => {
        expect(usinaSelect.disabled).toBe(false);
      });
    });
  });

  // ========== TESTES: CONSULTA ==========
  describe('Execução de Consulta', () => {
    it('deve exibir erro se nenhum filtro obrigatório for preenchido', async () => {
      render(<AvailabilityQuery />);
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/Preencha pelo menos Data PDP ou Empresa/i)).toBeInTheDocument();
      });
    });

    it('deve executar consulta quando filtros obrigatórios forem preenchidos', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(AvailabilityQueryService.query).toHaveBeenCalled();
      });
    });

    it('deve exibir mensagem de sucesso quando consulta retornar dados', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/2 registros encontrados/i)).toBeInTheDocument();
      });
    });

    it('deve renderizar grid de dados após consulta bem-sucedida', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('Itaipu')).toBeInTheDocument();
        expect(screen.getByText('Belo Monte')).toBeInTheDocument();
      });
    });

    it('deve exibir erro quando consulta falhar', async () => {
      (AvailabilityQueryService.query as any).mockRejectedValueOnce(
        new Error('Erro de conexão')
      );

      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/Erro de conexão/i)).toBeInTheDocument();
      });
    });

    it('deve mostrar estado de carregamento durante consulta', async () => {
      (AvailabilityQueryService.query as any).mockImplementation(
        () => new Promise(resolve => setTimeout(() => resolve(mockQueryResponse), 1000))
      );

      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      expect(screen.getByText(/Buscando/i)).toBeInTheDocument();
    });
  });

  // ========== TESTES: GRID DE DADOS ==========
  describe('Grid de Dados', () => {
    it('deve renderizar todas as colunas esperadas', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('Data PDP')).toBeInTheDocument();
        expect(screen.getByText('Empresa')).toBeInTheDocument();
        expect(screen.getByText('Usina')).toBeInTheDocument();
        expect(screen.getByText('Tipo')).toBeInTheDocument();
      });
    });

    it('deve exibir dados no grid corretamente', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('2024-12-27')).toBeInTheDocument();
        expect(screen.getByText('Empresa A')).toBeInTheDocument();
        expect(screen.getByText('Itaipu')).toBeInTheDocument();
      });
    });

    it('deve exibir status com badge correto', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('Ativa')).toBeInTheDocument();
        expect(screen.getByText('Manutenção')).toBeInTheDocument();
      });
    });

    it('deve exibir capacidades e percentual com formatting correto', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('14000.00')).toBeInTheDocument(); // Cap máxima
        expect(screen.getByText('95.5%')).toBeInTheDocument(); // Percentual
      });
    });
  });

  // ========== TESTES: PAGINAÇÃO ==========
  describe('Paginação', () => {
    it('deve renderizar controles de paginação após consulta', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/« Primeira/)).toBeInTheDocument();
        expect(screen.getByText(/Página 1 de 1/)).toBeInTheDocument();
      });
    });

    it('deve desabilitar botão primeira página quando já está na primeira', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        const primeiraBtn = screen.getByText(/« Primeira/) as HTMLButtonElement;
        expect(primeiraBtn.disabled).toBe(true);
      });
    });
  });

  // ========== TESTES: AGREGAÇÃO ==========
  describe('Agregação de Dados', () => {
    it('deve carregar dados agregados após consulta bem-sucedida', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(AvailabilityQueryService.getAggregatedData).toHaveBeenCalled();
      });
    });

    it('deve renderizar seção de resumo por usina', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('Resumo por Usina')).toBeInTheDocument();
      });
    });

    it('deve exibir dados agregados corretamente', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/Disponibilidade Média:/)).toBeInTheDocument();
        expect(screen.getByText(/95.5%/)).toBeInTheDocument(); // Média
      });
    });
  });

  // ========== TESTES: LIMPEZA DE FILTROS ==========
  describe('Limpeza de Filtros', () => {
    it('deve limpar todos os filtros ao clicar no botão Limpar', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });
      const limparBtn = screen.getByRole('button', { name: /Limpar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText('2 registros encontrados')).toBeInTheDocument();
      });

      fireEvent.click(limparBtn);

      await waitFor(() => {
        expect(dateInput.value).toBe('');
        expect(screen.queryByText('2 registros encontrados')).not.toBeInTheDocument();
      });
    });
  });

  // ========== TESTES: EXPORTAÇÃO ==========
  describe('Exportação de Dados', () => {
    it('deve renderizar botões de exportação após consulta', async () => {
      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/Excel/i)).toBeInTheDocument();
        expect(screen.getByText(/CSV/i)).toBeInTheDocument();
        expect(screen.getByText(/PDF/i)).toBeInTheDocument();
      });
    });

    it('deve chamar exportData com formato correto ao exportar para Excel', async () => {
      (AvailabilityQueryService.exportData as any).mockResolvedValue(new Blob());

      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        const excelBtn = screen.getByRole('button', { name: /Excel/i });
        expect(excelBtn).toBeInTheDocument();
      });

      const excelBtn = screen.getByRole('button', { name: /Excel/i });
      fireEvent.click(excelBtn);

      await waitFor(() => {
        expect(AvailabilityQueryService.exportData).toHaveBeenCalledWith(
          expect.any(Object),
          'excel'
        );
      });
    });

    it('deve desabilitar botões de exportação quando nenhum dado estiver disponível', () => {
      render(<AvailabilityQuery />);
      // Não executar consulta, então nenhum dado estará disponível
      
      expect(screen.queryByText(/Excel/i)).not.toBeInTheDocument();
    });
  });

  // ========== TESTES: TOGGLE FILTROS ==========
  describe('Toggle de Filtros', () => {
    it('deve expandir/colapsar seção de filtros ao clicar no header', async () => {
      render(<AvailabilityQuery />);
      const filtersHeader = screen.getByText('Filtros de Busca').closest('div');

      fireEvent.click(filtersHeader!);

      await waitFor(() => {
        const filterContent = screen.getByLabelText('Data PDP - Início').closest('div')?.parentElement;
        expect(filterContent?.classList.contains('hidden')).toBe(true);
      });

      fireEvent.click(filtersHeader!);

      await waitFor(() => {
        const filterContent = screen.getByLabelText('Data PDP - Início').closest('div')?.parentElement;
        expect(filterContent?.classList.contains('hidden')).toBe(false);
      });
    });
  });

  // ========== TESTES: VALIDAÇÃO DE INTERVALO ==========
  describe('Validação de Intervalo', () => {
    it('deve aceitar valores válidos para intervalo início (1-48)', async () => {
      render(<AvailabilityQuery />);
      const intervaloInicioInput = screen.getByLabelText('Intervalo - Início') as HTMLInputElement;

      fireEvent.change(intervaloInicioInput, { target: { value: '10' } });

      await waitFor(() => {
        expect(intervaloInicioInput.value).toBe('10');
      });
    });

    it('deve aceitar valores válidos para intervalo fim (1-48)', async () => {
      render(<AvailabilityQuery />);
      const intervaloFimInput = screen.getByLabelText('Intervalo - Fim') as HTMLInputElement;

      fireEvent.change(intervaloFimInput, { target: { value: '30' } });

      await waitFor(() => {
        expect(intervaloFimInput.value).toBe('30');
      });
    });
  });

  // ========== TESTES: TRATAMENTO DE ERROS NA AGREGAÇÃO ==========
  describe('Tratamento de Erros', () => {
    it('deve limpar dados agregados se não houver resultados', async () => {
      (AvailabilityQueryService.query as any).mockResolvedValue({
        dados: [],
        total: 0,
        pagina: 1,
        itensPorPagina: 20,
        totalPaginas: 0,
      });

      render(<AvailabilityQuery />);
      const dateInput = screen.getByLabelText('Data PDP - Início') as HTMLInputElement;
      const buscarBtn = screen.getByRole('button', { name: /Buscar/i });

      fireEvent.change(dateInput, { target: { value: '2024-12-27' } });
      fireEvent.click(buscarBtn);

      await waitFor(() => {
        expect(screen.getByText(/Nenhum registro encontrado/i)).toBeInTheDocument();
      });
    });
  });
});

import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Load from '../../src/pages/Collection/Load';
import {
  gerarIntervalos48,
  formatarValoresParaTextarea,
  parseTextareaParaValores,
  calcularTotalEMedia,
  validarValorCarga,
  validarValoresCarga,
  validarSelecao,
  formatarDataPDP,
  converterDataParaPDP
} from '../../src/types/load';

describe('Load Component', () => {
  // Mock functions
  const mockOnLoadDatas = vi.fn();
  const mockOnLoadEmpresas = vi.fn();
  const mockOnLoadCarga = vi.fn();
  const mockOnSave = vi.fn();
  
  // Mock data
  const mockDatas = [
    { datPdp: '20240115', datPdpFormatada: '15/01/2024' },
    { datPdp: '20240116', datPdpFormatada: '16/01/2024' }
  ];
  
  const mockEmpresas = [
    { codEmpre: 'FURNAS', nomeEmpre: 'FURNAS Centrais Elétricas' },
    { codEmpre: 'ELETROBRAS', nomeEmpre: 'Centrais Elétricas Brasileiras' }
  ];
  
  const mockCargaResponse = {
    cargas: Array.from({ length: 48 }, (_, i) => ({
      intCarga: i + 1,
      valCargaTran: 100 + i
    })),
    total: 2520,
    media: 105
  };
  
  beforeEach(() => {
    vi.clearAllMocks();
    mockOnLoadDatas.mockResolvedValue(mockDatas);
    mockOnLoadEmpresas.mockResolvedValue(mockEmpresas);
    mockOnLoadCarga.mockResolvedValue(mockCargaResponse);
    mockOnSave.mockResolvedValue(undefined);
  });
  
  describe('Renderização Inicial', () => {
    it('deve renderizar título corretamente', () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      expect(screen.getByText('Coleta de Carga')).toBeInTheDocument();
    });
    
    it('deve renderizar dropdowns de filtro', () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      expect(screen.getByLabelText('Data PDP:')).toBeInTheDocument();
      expect(screen.getByLabelText('Empresa:')).toBeInTheDocument();
    });
    
    it('deve renderizar grid com 48 linhas de dados + 2 de resumo', () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      const rows = screen.getAllByRole('row');
      expect(rows).toHaveLength(51); // 1 header + 48 data + 2 summary
    });
    
    it('deve renderizar linhas de Total e Média', () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      expect(screen.getByText('Total')).toBeInTheDocument();
      expect(screen.getByText('Média')).toBeInTheDocument();
    });
  });
  
  describe('Carregamento de Dados', () => {
    it('deve carregar datas ao montar componente', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(mockOnLoadDatas).toHaveBeenCalledTimes(1);
      });
    });
    
    it('deve exibir datas carregadas no dropdown', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
        expect(screen.getByText('16/01/2024')).toBeInTheDocument();
      });
    });
    
    it('deve carregar empresas ao selecionar data', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(mockOnLoadEmpresas).toHaveBeenCalledTimes(1);
      });
    });
    
    it('deve carregar dados de carga ao selecionar empresa', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      // Seleciona data
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      // Seleciona empresa
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(mockOnLoadCarga).toHaveBeenCalledWith('20240115', 'FURNAS');
      });
    });
  });
  
  describe('Exibição de Dados', () => {
    it('deve exibir valores de carga no grid', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('100.00')).toBeInTheDocument();
      });
    });
    
    it('deve exibir total calculado', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('2520.00')).toBeInTheDocument();
      });
    });
    
    it('deve exibir média calculada', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        const mediaCells = screen.getAllByText('105');
        expect(mediaCells.length).toBeGreaterThan(0);
      });
    });
  });
  
  describe('Botões de Ação', () => {
    it('deve mostrar botão Alterar após carregar dados', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
    });
    
    it('deve mostrar botão Salvar ao clicar em Alterar', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
    });
  });
  
  describe('Textarea Overlay', () => {
    it('deve mostrar textarea ao clicar em Alterar', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      const textareas = screen.getAllByRole('textbox');
      expect(textareas.length).toBeGreaterThan(0);
    });
    
    it('deve preencher textarea com valores atuais', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      const textareas = screen.getAllByRole('textbox');
      const textarea = textareas.find(t => t.getAttribute('rows') === '48');
      
      expect(textarea).toBeDefined();
      if (textarea) {
        expect((textarea as HTMLTextAreaElement).value).toContain('100');
      }
    });
    
    it('deve remover duplas quebras de linha', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      const textareas = screen.getAllByRole('textbox');
      const textarea = textareas.find(t => t.getAttribute('rows') === '48') as HTMLTextAreaElement;
      
      expect(textarea).toBeDefined();
      
      if (textarea) {
        fireEvent.change(textarea, { target: { value: '100\n\n105' } });
        
        await waitFor(() => {
          expect(textarea.value).toBe('100\n105');
        });
      }
    });
  });
  
  describe('Salvamento de Dados', () => {
    it('deve chamar onSave ao clicar em Salvar', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
      
      const salvarButton = screen.getByText('Salvar');
      fireEvent.click(salvarButton);
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalled();
      });
    });
    
    it('deve passar parâmetros corretos para onSave', async () => {
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
      
      const salvarButton = screen.getByText('Salvar');
      fireEvent.click(salvarButton);
      
      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith({
          datPdp: '20240115',
          codEmpre: 'FURNAS',
          valores: expect.any(Array)
        });
      });
    });
  });
  
  describe('Tratamento de Erros', () => {
    it('deve exibir mensagem de erro ao falhar carregamento de datas', async () => {
      mockOnLoadDatas.mockRejectedValue(new Error('Erro ao carregar'));
      
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText(/Não foi possível acessar a Base de Dados/)).toBeInTheDocument();
      });
    });
    
    it('deve exibir mensagem de erro ao falhar salvamento', async () => {
      mockOnSave.mockRejectedValue(new Error('Erro ao salvar'));
      
      render(
        <Load
          onLoadDatas={mockOnLoadDatas}
          onLoadEmpresas={mockOnLoadEmpresas}
          onLoadCarga={mockOnLoadCarga}
          onSave={mockOnSave}
        />
      );
      
      await waitFor(() => {
        expect(screen.getByText('15/01/2024')).toBeInTheDocument();
      });
      
      const dataSelect = screen.getByLabelText('Data PDP:');
      fireEvent.change(dataSelect, { target: { value: '20240115' } });
      
      await waitFor(() => {
        expect(screen.getByText('FURNAS Centrais Elétricas')).toBeInTheDocument();
      });
      
      const empresaSelect = screen.getByLabelText('Empresa:');
      fireEvent.change(empresaSelect, { target: { value: 'FURNAS' } });
      
      await waitFor(() => {
        expect(screen.getByText('Alterar')).toBeInTheDocument();
      });
      
      const alterarButton = screen.getByText('Alterar');
      fireEvent.click(alterarButton);
      
      await waitFor(() => {
        expect(screen.getByText('Salvar')).toBeInTheDocument();
      });
      
      const salvarButton = screen.getByText('Salvar');
      fireEvent.click(salvarButton);
      
      await waitFor(() => {
        expect(screen.getByText(/Não foi possível gravar os dados/)).toBeInTheDocument();
      });
    });
  });
});

describe('Load Utility Functions', () => {
  describe('gerarIntervalos48', () => {
    it('deve gerar 48 intervalos', () => {
      const intervalos = gerarIntervalos48();
      expect(intervalos).toHaveLength(48);
    });
    
    it('primeiro intervalo deve ser 00:00 - 00:30', () => {
      const intervalos = gerarIntervalos48();
      expect(intervalos[0]).toBe('00:00 - 00:30');
    });
    
    it('último intervalo deve ser 23:30 - 23:59', () => {
      const intervalos = gerarIntervalos48();
      expect(intervalos[47]).toBe('23:30 - 23:59');
    });
  });
  
  describe('formatarValoresParaTextarea', () => {
    it('deve formatar valores com quebra de linha', () => {
      const valores = [100, 105, 110];
      const resultado = formatarValoresParaTextarea(valores);
      expect(resultado).toBe('100\n105\n110');
    });
  });
  
  describe('parseTextareaParaValores', () => {
    it('deve fazer parse de texto para array de valores', () => {
      const texto = '100\n105\n110';
      const resultado = parseTextareaParaValores(texto);
      expect(resultado[0]).toBe(100);
      expect(resultado[1]).toBe(105);
      expect(resultado[2]).toBe(110);
    });
    
    it('deve completar com zeros até 48 valores', () => {
      const texto = '100\n105';
      const resultado = parseTextareaParaValores(texto);
      expect(resultado).toHaveLength(48);
      expect(resultado[47]).toBe(0);
    });
  });
  
  describe('calcularTotalEMedia', () => {
    it('deve calcular total dividindo soma por 2', () => {
      const valores = Array(48).fill(100);
      const { total } = calcularTotalEMedia(valores);
      expect(total).toBe(2400); // (100 * 48) / 2
    });
    
    it('deve calcular média dividindo soma por 48', () => {
      const valores = Array(48).fill(100);
      const { media } = calcularTotalEMedia(valores);
      expect(media).toBe(100); // (100 * 48) / 48
    });
  });
  
  describe('validarValorCarga', () => {
    it('deve aceitar valores positivos', () => {
      expect(validarValorCarga(100)).toBe(true);
    });
    
    it('deve rejeitar valores negativos', () => {
      expect(validarValorCarga(-10)).toBe(false);
    });
    
    it('deve aceitar valores com até 2 casas decimais', () => {
      expect(validarValorCarga(100.50)).toBe(true);
    });
    
    it('deve rejeitar valores com mais de 2 casas decimais', () => {
      expect(validarValorCarga(100.123)).toBe(false);
    });
  });
  
  describe('validarValoresCarga', () => {
    it('deve validar array com 48 valores válidos', () => {
      const valores = Array(48).fill(100);
      const resultado = validarValoresCarga(valores);
      expect(resultado.isValid).toBe(true);
    });
    
    it('deve rejeitar array com menos de 48 valores', () => {
      const valores = Array(40).fill(100);
      const resultado = validarValoresCarga(valores);
      expect(resultado.isValid).toBe(false);
    });
    
    it('deve rejeitar array com valores inválidos', () => {
      const valores = Array(48).fill(100);
      valores[5] = -10;
      const resultado = validarValoresCarga(valores);
      expect(resultado.isValid).toBe(false);
    });
  });
  
  describe('validarSelecao', () => {
    it('deve aceitar data e empresa válidas', () => {
      const resultado = validarSelecao('20240115', 'FURNAS');
      expect(resultado.isValid).toBe(true);
    });
    
    it('deve rejeitar data vazia', () => {
      const resultado = validarSelecao('', 'FURNAS');
      expect(resultado.isValid).toBe(false);
    });
    
    it('deve rejeitar empresa vazia', () => {
      const resultado = validarSelecao('20240115', '');
      expect(resultado.isValid).toBe(false);
    });
  });
  
  describe('formatarDataPDP', () => {
    it('deve formatar data de YYYYMMDD para DD/MM/YYYY', () => {
      const resultado = formatarDataPDP('20240115');
      expect(resultado).toBe('15/01/2024');
    });
    
    it('deve retornar vazio para data inválida', () => {
      const resultado = formatarDataPDP('invalido');
      expect(resultado).toBe('');
    });
  });
  
  describe('converterDataParaPDP', () => {
    it('deve converter data de DD/MM/YYYY para YYYYMMDD', () => {
      const resultado = converterDataParaPDP('15/01/2024');
      expect(resultado).toBe('20240115');
    });
    
    it('deve retornar vazio para data inválida', () => {
      const resultado = converterDataParaPDP('invalido');
      expect(resultado).toBe('');
    });
  });
});

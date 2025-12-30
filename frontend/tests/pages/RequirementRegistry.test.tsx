import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import RequirementRegistry from './RequirementRegistry';
import {
  RequisitoArea,
  DataPDPOption,
  validarValorMW,
  validarHorario,
  validarDataPDP,
  validarFormularioRequisito,
  formatarDataPDP,
  desformatarDataPDP,
  RequirementFormState,
} from '../../../types/requirement';

describe('RequirementRegistry Component', () => {
  const mockDatas: DataPDPOption[] = [
    { datPdp: '20251220', datPdpFormatada: '20/12/2025' },
    { datPdp: '20251221', datPdpFormatada: '21/12/2025' },
    { datPdp: '20251222', datPdpFormatada: '22/12/2025' },
  ];

  const mockRequisito: RequisitoArea = {
    datPdp: '20251220',
    codArea: 'NE',
    valReqMax: 1500,
    hReqMax: '14:30',
    valResReqMax: 300,
    hResReqMax: '14:30',
  };

  const mockOnLoadDatas = jest.fn().mockResolvedValue(mockDatas);
  const mockOnLoadRequisito = jest.fn().mockResolvedValue(mockRequisito);
  const mockOnSave = jest.fn().mockResolvedValue(undefined);

  beforeEach(() => {
    jest.clearAllMocks();
  });

  // Testes de Renderização (5 testes)
  describe('Renderização', () => {
    it('deve renderizar o componente sem erros', () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );
      expect(screen.getByTestId('data-select')).toBeInTheDocument();
    });

    it('deve renderizar select de data PDP', () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );
      expect(screen.getByTestId('data-select')).toBeInTheDocument();
      expect(screen.getByText('Data do PDP:')).toBeInTheDocument();
    });

    it('deve renderizar campos de requisito máximo', () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );
      expect(screen.getByTestId('val-req-max-input')).toBeInTheDocument();
      expect(screen.getByTestId('h-req-max-input')).toBeInTheDocument();
      expect(screen.getByText('Requisito Máximo:')).toBeInTheDocument();
    });

    it('deve renderizar campos de reserva mínima', () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );
      expect(screen.getByTestId('val-res-req-max-input')).toBeInTheDocument();
      expect(screen.getByTestId('h-res-req-max-input')).toBeInTheDocument();
      expect(screen.getByText('Reserva Mín. No Req. Máx.:')).toBeInTheDocument();
    });

    it('deve renderizar botões Salvar e Fechar', () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );
      expect(screen.getByTestId('salvar-btn')).toBeInTheDocument();
      expect(screen.getByTestId('fechar-btn')).toBeInTheDocument();
    });
  });

  // Testes de Carregamento de Dados (4 testes)
  describe('Carregamento de Dados', () => {
    it('deve carregar datas ao montar componente', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        expect(mockOnLoadDatas).toHaveBeenCalled();
      });
    });

    it('deve exibir datas no select', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('20/12/2025')).toBeInTheDocument();
        expect(screen.getByText('21/12/2025')).toBeInTheDocument();
      });
    });

    it('deve carregar requisito ao selecionar data', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        expect(mockOnLoadRequisito).toHaveBeenCalledWith('20251220', 'NE');
      });
    });

    it('deve exibir erro ao falhar no carregamento', async () => {
      const errorLoad = jest.fn().mockRejectedValue(new Error('Erro de rede'));

      render(
        <RequirementRegistry
          onLoadDatas={errorLoad}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        expect(screen.getByText('Não foi possível acessar a Base de Dados.')).toBeInTheDocument();
      });
    });
  });

  // Testes de Preenchimento de Formulário (6 testes)
  describe('Preenchimento de Formulário', () => {
    it('deve preencher campos ao carregar requisito existente', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        const valReqInput = screen.getByTestId('val-req-max-input') as HTMLInputElement;
        const hReqInput = screen.getByTestId('h-req-max-input') as HTMLInputElement;
        const valResInput = screen.getByTestId('val-res-req-max-input') as HTMLInputElement;
        const hResInput = screen.getByTestId('h-res-req-max-input') as HTMLInputElement;

        expect(valReqInput.value).toBe('1500');
        expect(hReqInput.value).toBe('14:30');
        expect(valResInput.value).toBe('300');
        expect(hResInput.value).toBe('14:30');
      });
    });

    it('deve permitir editar valor do requisito', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        const input = screen.getByTestId('val-req-max-input');
        fireEvent.change(input, { target: { value: '2000' } });
        expect((input as HTMLInputElement).value).toBe('2000');
      });
    });

    it('deve permitir editar hora do requisito', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        const input = screen.getByTestId('h-req-max-input');
        fireEvent.change(input, { target: { value: '15:00' } });
        expect((input as HTMLInputElement).value).toBe('15:00');
      });
    });

    it('deve permitir editar valor da reserva', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        const input = screen.getByTestId('val-res-req-max-input');
        fireEvent.change(input, { target: { value: '400' } });
        expect((input as HTMLInputElement).value).toBe('400');
      });
    });

    it('deve permitir editar hora da reserva', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        const input = screen.getByTestId('h-res-req-max-input');
        fireEvent.change(input, { target: { value: '16:00' } });
        expect((input as HTMLInputElement).value).toBe('16:00');
      });
    });

    it('deve limpar campos ao mudar data', async () => {
      const emptyLoad = jest.fn().mockResolvedValue(null);

      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={emptyLoad}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251221' },
        });
      });

      await waitFor(() => {
        const valReqInput = screen.getByTestId('val-req-max-input') as HTMLInputElement;
        expect(valReqInput.value).toBe('');
      });
    });
  });

  // Testes de Validação (8 testes)
  describe('Validação de Formulário', () => {
    it('deve validar data PDP não selecionada', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Selecione uma Data do PDP')).toBeInTheDocument();
      });
    });

    it('deve validar valor do requisito vazio', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Requisito Máximo Requerido')).toBeInTheDocument();
      });
    });

    it('deve validar hora do requisito vazia', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), { target: { value: '' } });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Horários Inválidos.')).toBeInTheDocument();
      });
    });

    it('deve validar formato de hora inválido', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '25:00' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Horários Inválidos.')).toBeInTheDocument();
      });
    });

    it('deve validar valor da reserva vazio', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Valor da Reserva Requerido')).toBeInTheDocument();
      });
    });

    it('deve validar hora da reserva vazia', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), { target: { value: '' } });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Horários Inválidos.')).toBeInTheDocument();
      });
    });

    it('deve validar valor negativo', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '-100' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(
          screen.getByText('Requisito Máximo não pode ser negativo')
        ).toBeInTheDocument();
      });
    });

    it('deve validar valor não numérico', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: 'abc' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(
          screen.getByText('Requisito Máximo deve ser um número válido')
        ).toBeInTheDocument();
      });
    });
  });

  // Testes de Salvamento (4 testes)
  describe('Salvamento de Requisito', () => {
    it('deve chamar onSave com dados corretos', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1800' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '15:00' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '350' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '15:00' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(mockOnSave).toHaveBeenCalledWith({
          datPdp: '20251220',
          codArea: 'NE',
          valReqMax: 1800,
          hReqMax: '15:00',
          valResReqMax: 350,
          hResReqMax: '15:00',
        });
      });
    });

    it('deve exibir mensagem de sucesso após salvar', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Registro gravado com sucesso!')).toBeInTheDocument();
      });
    });

    it('deve limpar formulário após salvar', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        const dataSelect = screen.getByTestId('data-select') as HTMLSelectElement;
        const valReqInput = screen.getByTestId('val-req-max-input') as HTMLInputElement;
        expect(dataSelect.value).toBe('');
        expect(valReqInput.value).toBe('');
      });
    });

    it('deve exibir erro ao falhar no salvamento', async () => {
      const errorSave = jest.fn().mockRejectedValue(new Error('Erro ao gravar'));

      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={errorSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.change(screen.getByTestId('h-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.change(screen.getByTestId('val-res-req-max-input'), {
          target: { value: '300' },
        });
        fireEvent.change(screen.getByTestId('h-res-req-max-input'), {
          target: { value: '14:30' },
        });
        fireEvent.click(screen.getByTestId('salvar-btn'));
      });

      await waitFor(() => {
        expect(screen.getByText('Não foi possível gravar o registro.')).toBeInTheDocument();
      });
    });
  });

  // Testes de Funções Utilitárias (7 testes)
  describe('Funções Utilitárias', () => {
    it('validarValorMW deve validar valor vazio', () => {
      expect(validarValorMW('', 'Campo')).toBe('Campo Requerido');
      expect(validarValorMW('  ', 'Campo')).toBe('Campo Requerido');
    });

    it('validarValorMW deve validar valor não numérico', () => {
      expect(validarValorMW('abc', 'Campo')).toBe('Campo deve ser um número válido');
    });

    it('validarValorMW deve validar valor negativo', () => {
      expect(validarValorMW('-100', 'Campo')).toBe('Campo não pode ser negativo');
    });

    it('validarValorMW deve aceitar valor válido', () => {
      expect(validarValorMW('1500', 'Campo')).toBe(true);
    });

    it('validarHorario deve validar formato correto', () => {
      expect(validarHorario('14:30', 'Campo')).toBe(true);
      expect(validarHorario('00:00', 'Campo')).toBe(true);
      expect(validarHorario('23:59', 'Campo')).toBe(true);
    });

    it('validarHorario deve rejeitar formato incorreto', () => {
      expect(validarHorario('25:00', 'Campo')).toBe('Horários Inválidos.');
      expect(validarHorario('14:60', 'Campo')).toBe('Horários Inválidos.');
      expect(validarHorario('1430', 'Campo')).toBe('Horários Inválidos.');
    });

    it('formatarDataPDP deve formatar corretamente', () => {
      expect(formatarDataPDP('20251220')).toBe('20/12/2025');
      expect(formatarDataPDP('20250101')).toBe('01/01/2025');
    });
  });

  // Testes de Botão Fechar (1 teste)
  describe('Botão Fechar', () => {
    it('deve limpar formulário ao clicar em Fechar', async () => {
      render(
        <RequirementRegistry
          onLoadDatas={mockOnLoadDatas}
          onLoadRequisito={mockOnLoadRequisito}
          onSave={mockOnSave}
        />
      );

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('data-select'), {
          target: { value: '20251220' },
        });
      });

      await waitFor(() => {
        fireEvent.change(screen.getByTestId('val-req-max-input'), {
          target: { value: '1500' },
        });
        fireEvent.click(screen.getByTestId('fechar-btn'));
      });

      const dataSelect = screen.getByTestId('data-select') as HTMLSelectElement;
      const valReqInput = screen.getByTestId('val-req-max-input') as HTMLInputElement;
      expect(dataSelect.value).toBe('');
      expect(valReqInput.value).toBe('');
    });
  });
});

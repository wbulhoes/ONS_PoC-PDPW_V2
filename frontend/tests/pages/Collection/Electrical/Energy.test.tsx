/**
 * Testes para Energy.tsx
 * Migração de: legado/pdpw/frmColEnergetica.aspx
 */

import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import Energy from '../../../../src/pages/Collection/Electrical/Energy';

describe('Energy Component', () => {
  describe('Renderização Inicial', () => {
    it('deve renderizar o título da página', () => {
      render(<Energy />);
      expect(screen.getByTestId('page-title')).toHaveTextContent('Razão Energética Transformada');
    });

    it('deve renderizar o subtítulo da página', () => {
      render(<Energy />);
      expect(screen.getByTestId('page-subtitle')).toHaveTextContent(
        'Coleta de dados energéticos de usinas'
      );
    });

    it('deve renderizar o container principal', () => {
      render(<Energy />);
      expect(screen.getByTestId('energy-container')).toBeInTheDocument();
    });

    it('deve renderizar todos os campos do formulário', () => {
      render(<Energy />);

      expect(screen.getByTestId('label-data-pdp')).toHaveTextContent('Data PDP:');
      expect(screen.getByTestId('select-data-pdp')).toBeInTheDocument();

      expect(screen.getByTestId('label-empresa')).toHaveTextContent('Empresa:');
      expect(screen.getByTestId('select-empresa')).toBeInTheDocument();

      expect(screen.getByTestId('label-usina')).toHaveTextContent('Usina:');
      expect(screen.getByTestId('select-usina')).toBeInTheDocument();
    });

    it('deve desabilitar select de empresa quando data não está selecionada', () => {
      render(<Energy />);
      const selectEmpresa = screen.getByTestId('select-empresa') as HTMLSelectElement;
      expect(selectEmpresa.disabled).toBe(true);
    });

    it('deve desabilitar select de usina quando empresa não está selecionada', () => {
      render(<Energy />);
      const selectUsina = screen.getByTestId('select-usina') as HTMLSelectElement;
      expect(selectUsina.disabled).toBe(true);
    });
  });

  describe('Interação do Usuário', () => {
    it('deve habilitar select de empresa após selecionar data', () => {
      render(<Energy />);
      const selectData = screen.getByTestId('select-data-pdp');
      fireEvent.change(selectData, { target: { value: '20251224' } });
      
      const selectEmpresa = screen.getByTestId('select-empresa') as HTMLSelectElement;
      expect(selectEmpresa.disabled).toBe(false);
    });

    it('deve chamar onLoadData quando data e empresa são selecionadas', async () => {
      const mockOnLoadData = vi.fn().mockResolvedValue({
        dataPdp: '20251224',
        codEmpresa: 'EMP001',
        usinas: [],
        totaisPorIntervalo: []
      });

      render(<Energy onLoadData={mockOnLoadData} />);

      const selectData = screen.getByTestId('select-data-pdp');
      fireEvent.change(selectData, { target: { value: '20251224' } });

      const selectEmpresa = screen.getByTestId('select-empresa');
      fireEvent.change(selectEmpresa, { target: { value: 'EMP001' } });

      await waitFor(() => {
        expect(mockOnLoadData).toHaveBeenCalledWith('20251224', 'EMP001');
      });
    });
  });
});

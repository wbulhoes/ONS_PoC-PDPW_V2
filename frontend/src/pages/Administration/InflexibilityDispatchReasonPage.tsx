import React from 'react';
import InflexibilityDispatchReason from './InflexibilityDispatchReason';
import type { InflexibilityDispatchReason as InflexibilityDispatchReasonType } from '../../types/dispatchReason';

const InflexibilityDispatchReasonPage: React.FC = () => {
  const handleLoadReasons = async (): Promise<InflexibilityDispatchReasonType[]> => {
    // TODO: Implementar chamada à API
    // Por enquanto, retorna dados mockados
    return [
      {
        id: 1,
        codigo: 'INF001',
        descricao: 'Restrição técnica do equipamento',
        tipoInflexibilidade: 'TECNICA',
        ativo: true,
        dataInclusao: '2024-01-15',
      },
      {
        id: 2,
        codigo: 'INF002',
        descricao: 'Contrato de fornecimento mínimo',
        tipoInflexibilidade: 'CONTRATUAL',
        ativo: true,
        dataInclusao: '2024-01-20',
      },
      {
        id: 3,
        codigo: 'INF003',
        descricao: 'Necessidade operacional do sistema',
        tipoInflexibilidade: 'OPERACIONAL',
        ativo: true,
        dataInclusao: '2024-02-05',
      },
      {
        id: 4,
        codigo: 'INF004',
        descricao: 'Restrição ambiental de emissões',
        tipoInflexibilidade: 'AMBIENTAL',
        ativo: true,
        dataInclusao: '2024-02-10',
      },
      {
        id: 5,
        codigo: 'INF005',
        descricao: 'Limitação de estoque de combustível',
        tipoInflexibilidade: 'COMBUSTIVEL',
        ativo: false,
        dataInclusao: '2024-03-01',
      },
    ];
  };

  const handleSave = async (reason: InflexibilityDispatchReasonType): Promise<void> => {
    // TODO: Implementar chamada à API
    console.log('Salvando motivo de inflexibilidade:', reason);
    await new Promise(resolve => setTimeout(resolve, 500));
  };

  const handleDelete = async (id: number): Promise<void> => {
    // TODO: Implementar chamada à API
    console.log('Excluindo motivo de inflexibilidade:', id);
    await new Promise(resolve => setTimeout(resolve, 500));
  };

  return (
    <InflexibilityDispatchReason
      onLoadReasons={handleLoadReasons}
      onSave={handleSave}
      onDelete={handleDelete}
    />
  );
};

export default InflexibilityDispatchReasonPage;

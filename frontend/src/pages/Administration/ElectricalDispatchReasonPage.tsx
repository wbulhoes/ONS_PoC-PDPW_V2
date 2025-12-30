import React from 'react';
import ElectricalDispatchReason from './ElectricalDispatchReason';
import type { ElectricalDispatchReason as ElectricalDispatchReasonType } from '../../types/dispatchReason';

const ElectricalDispatchReasonPage: React.FC = () => {
  const handleLoadReasons = async (): Promise<ElectricalDispatchReasonType[]> => {
    // TODO: Implementar chamada à API
    // Por enquanto, retorna dados mockados
    return [
      {
        id: 1,
        codigo: 'RE001',
        descricao: 'Suprimento de carga',
        ativo: true,
        dataInclusao: '2024-01-15',
      },
      {
        id: 2,
        codigo: 'RE002',
        descricao: 'Estabilidade da rede elétrica',
        ativo: true,
        dataInclusao: '2024-01-15',
      },
      {
        id: 3,
        codigo: 'RE003',
        descricao: 'Regulação de tensão',
        ativo: true,
        dataInclusao: '2024-02-10',
      },
      {
        id: 4,
        codigo: 'RE004',
        descricao: 'Controle de frequência',
        ativo: false,
        dataInclusao: '2024-03-05',
      },
    ];
  };

  const handleSave = async (reason: ElectricalDispatchReasonType): Promise<void> => {
    // TODO: Implementar chamada à API
    console.log('Salvando motivo:', reason);
    await new Promise(resolve => setTimeout(resolve, 500));
  };

  const handleDelete = async (id: number): Promise<void> => {
    // TODO: Implementar chamada à API
    console.log('Excluindo motivo:', id);
    await new Promise(resolve => setTimeout(resolve, 500));
  };

  return (
    <ElectricalDispatchReason
      onLoadReasons={handleLoadReasons}
      onSave={handleSave}
      onDelete={handleDelete}
    />
  );
};

export default ElectricalDispatchReasonPage;

import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface CreditoForaMeritoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  energiaCreditada: number;
  motivoCredito: string;
  valorUnitario: number;
  valorTotal: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'energiaCreditada', label: 'Energia (MWh)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'motivoCredito', label: 'Motivo do Crédito', minWidth: 250 },
  { id: 'valorUnitario', label: 'Valor Unit. (R$/MWh)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'valorTotal', label: 'Valor Total (R$)', minWidth: 160, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const CreditoForaMeritoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<CreditoForaMeritoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<CreditoForaMeritoQueryData[]>(`/consulta/credito-fora-merito?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/credito-fora-merito/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Crédito Fora de Mérito"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default CreditoForaMeritoQuery;

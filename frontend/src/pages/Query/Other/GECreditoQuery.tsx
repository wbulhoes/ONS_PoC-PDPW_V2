import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface GECreditoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  garantiaEnergetica: number;
  creditoConcedido: number;
  saldoCredito: number;
  tipoCredito: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'garantiaEnergetica', label: 'GE (MWmed)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'creditoConcedido', label: 'Crédito Concedido (MWh)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'saldoCredito', label: 'Saldo (MWh)', minWidth: 140, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'tipoCredito', label: 'Tipo de Crédito', minWidth: 180 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const GECreditoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<GECreditoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<GECreditoQueryData[]>(`/consulta/ge-credito?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/ge-credito/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de GE Crédito"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default GECreditoQuery;

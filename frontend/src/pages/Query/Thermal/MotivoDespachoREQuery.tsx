import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface MotivoDespachoREQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  codigoMotivo: string;
  descricaoMotivo: string;
  geracaoDespacho: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'codigoMotivo', label: 'Código', minWidth: 100 },
  { id: 'descricaoMotivo', label: 'Motivo de Despacho RE', minWidth: 300 },
  { id: 'geracaoDespacho', label: 'Geração (MWmed)', minWidth: 160, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const MotivoDespachoREQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<MotivoDespachoREQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<MotivoDespachoREQueryData[]>(`/consulta/motivo-despacho-re?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/motivo-despacho-re/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Motivo de Despacho Razão Elétrica"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default MotivoDespachoREQuery;

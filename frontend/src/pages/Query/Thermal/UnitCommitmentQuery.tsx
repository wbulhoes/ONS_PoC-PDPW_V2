import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface UnitCommitmentQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  statusComprometimento: string;
  potenciaComprometida: number;
  motivoDespacho: string;
  custoOperacional: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'statusComprometimento', label: 'Status', minWidth: 120 },
  { id: 'potenciaComprometida', label: 'Potência (MW)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'motivoDespacho', label: 'Motivo Despacho', minWidth: 250 },
  { id: 'custoOperacional', label: 'Custo (R$/MWh)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const UnitCommitmentQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<UnitCommitmentQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<UnitCommitmentQueryData[]>(`/consulta/unit-commitment?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/unit-commitment/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Unit Commitment"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default UnitCommitmentQuery;

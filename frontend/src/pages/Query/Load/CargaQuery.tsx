import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface CargaQueryData {
  id: number;
  dataPdp: string;
  subsistema: string;
  empresa: string;
  cargaTotal: number;
  cargaMedia: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'subsistema', label: 'Subsistema', minWidth: 100 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'cargaTotal', label: 'Carga Total (MWmed)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'cargaMedia', label: 'Carga Média (MW)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const CargaQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<CargaQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);

    const data = await apiClient.get<CargaQueryData[]>(`/consulta/carga?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);

    const blob = await apiClient.get<Blob>(`/consulta/carga/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Carga"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={false}
      showDateRange={true}
    />
  );
};

export default CargaQuery;

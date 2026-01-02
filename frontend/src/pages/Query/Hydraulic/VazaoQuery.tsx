import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface VazaoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  vazaoAfluente: number;
  vazaoDefluente: number;
  vazaoTurbinada: number;
  vazaoVertida: number;
  volumeUtil: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'vazaoAfluente', label: 'Vazão Afluente (m³/s)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'vazaoDefluente', label: 'Vazão Defluente (m³/s)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'vazaoTurbinada', label: 'Vazão Turbinada (m³/s)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'vazaoVertida', label: 'Vazão Vertida (m³/s)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'volumeUtil', label: 'Volume Útil (%)', minWidth: 140, align: 'right', format: (value) => value.toFixed(1) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const VazaoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<VazaoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<VazaoQueryData[]>(`/consulta/vazao?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/vazao/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Vazão"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default VazaoQuery;

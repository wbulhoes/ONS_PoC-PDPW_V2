import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface SuprimentoOrdemMeritoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  ordemMerito: number;
  custoVariavel: number;
  energiaSuprida: number;
  status: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'ordemMerito', label: 'Ordem', minWidth: 100, align: 'center' },
  { id: 'custoVariavel', label: 'CVU (R$/MWh)', minWidth: 150, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'energiaSuprida', label: 'Energia Suprida (MWh)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'status', label: 'Status', minWidth: 120 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const SuprimentoOrdemMeritoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<SuprimentoOrdemMeritoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<SuprimentoOrdemMeritoQueryData[]>(`/consulta/suprimento-ordem-merito?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/suprimento-ordem-merito/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Suprimento por Ordem de Mérito"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default SuprimentoOrdemMeritoQuery;

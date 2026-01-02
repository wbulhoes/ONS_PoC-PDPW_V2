import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface InflexibilidadeQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  inflexibilidadeDeclarada: number;
  inflexibilidadeVerificada: number;
  motivo: string;
  periodo: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'inflexibilidadeDeclarada', label: 'Inflex. Declarada (MWmed)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'inflexibilidadeVerificada', label: 'Inflex. Verificada (MWmed)', minWidth: 200, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'motivo', label: 'Motivo', minWidth: 250 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const InflexibilidadeQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<InflexibilidadeQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<InflexibilidadeQueryData[]>(`/consulta/inflexibilidade?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/inflexibilidade/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Inflexibilidade"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default InflexibilidadeQuery;

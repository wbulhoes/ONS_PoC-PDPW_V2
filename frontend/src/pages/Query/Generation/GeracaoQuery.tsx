import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface GeracaoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  tipoUsina: string;
  geracaoProgramada: number;
  geracaoVerificada: number;
  disponibilidade: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'tipoUsina', label: 'Tipo', minWidth: 100 },
  { id: 'geracaoProgramada', label: 'Ger. Programada (MWmed)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'geracaoVerificada', label: 'Ger. Verificada (MWmed)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'disponibilidade', label: 'Disponibilidade (%)', minWidth: 150, align: 'right', format: (value) => value.toFixed(1) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const GeracaoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<GeracaoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<GeracaoQueryData[]>(`/consulta/geracao?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/geracao/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Geração"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default GeracaoQuery;

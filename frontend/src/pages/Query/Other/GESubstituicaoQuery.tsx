import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface GESubstituicaoQueryData {
  id: number;
  dataPdp: string;
  usinaOrigem: string;
  usinaDestino: string;
  empresa: string;
  periodo: string;
  garantiaSubstituida: number;
  motivoSubstituicao: string;
  dataInicioSubstituicao: string;
  dataFimSubstituicao: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usinaOrigem', label: 'Usina Origem', minWidth: 180 },
  { id: 'usinaDestino', label: 'Usina Destino', minWidth: 180 },
  { id: 'empresa', label: 'Empresa', minWidth: 180 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'garantiaSubstituida', label: 'GE Substituída (MWmed)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'motivoSubstituicao', label: 'Motivo', minWidth: 250 },
  { id: 'dataInicioSubstituicao', label: 'Início', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'dataFimSubstituicao', label: 'Fim', minWidth: 120, format: (value) => value ? new Date(value).toLocaleDateString('pt-BR') : 'Indeterminado' },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const GESubstituicaoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<GESubstituicaoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<GESubstituicaoQueryData[]>(`/consulta/ge-substituicao?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/ge-substituicao/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de GE Substituição"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default GESubstituicaoQuery;

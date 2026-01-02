import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface MaquinasGerandoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  numeroMaquinasGerando: number;
  potenciaGerando: number;
  geracaoTotal: number;
  periodo: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'numeroMaquinasGerando', label: 'Nº Máq. Gerando', minWidth: 160, align: 'center' },
  { id: 'potenciaGerando', label: 'Potência Unitária (MW)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'geracaoTotal', label: 'Geração Total (MWmed)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'periodo', label: 'Período', minWidth: 120 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const MaquinasGerandoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<MaquinasGerandoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<MaquinasGerandoQueryData[]>(`/consulta/maquinas-gerando?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/maquinas-gerando/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Número de Máquinas Gerando"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default MaquinasGerandoQuery;

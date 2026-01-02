import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface MaquinasParadasQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  numeroMaquinasParadas: number;
  motivoParada: string;
  periodoInicio: string;
  periodoFim: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'numeroMaquinasParadas', label: 'Nº Máquinas Paradas', minWidth: 180, align: 'center' },
  { id: 'motivoParada', label: 'Motivo da Parada', minWidth: 250 },
  { id: 'periodoInicio', label: 'Início', minWidth: 100, format: (value) => value || '-' },
  { id: 'periodoFim', label: 'Fim', minWidth: 100, format: (value) => value || '-' },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const MaquinasParadasQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<MaquinasParadasQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<MaquinasParadasQueryData[]>(`/consulta/maquinas-paradas?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/maquinas-paradas/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Número de Máquinas Paradas por Conveniência Operativa"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default MaquinasParadasQuery;

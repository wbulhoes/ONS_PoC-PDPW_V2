import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface ParadaUGQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  unidadeGeradora: string;
  dataHoraParada: string;
  dataHoraRetorno: string;
  motivoParada: string;
  potenciaNominal: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 180 },
  { id: 'empresa', label: 'Empresa', minWidth: 180 },
  { id: 'unidadeGeradora', label: 'Unidade Geradora', minWidth: 150 },
  { id: 'dataHoraParada', label: 'Data/Hora Parada', minWidth: 160, format: (value) => new Date(value).toLocaleString('pt-BR') },
  { id: 'dataHoraRetorno', label: 'Data/Hora Retorno', minWidth: 170, format: (value) => value ? new Date(value).toLocaleString('pt-BR') : 'Em parada' },
  { id: 'motivoParada', label: 'Motivo da Parada', minWidth: 250 },
  { id: 'potenciaNominal', label: 'Potência (MW)', minWidth: 140, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const ParadaUGQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<ParadaUGQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<ParadaUGQueryData[]>(`/consulta/parada-ug?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/parada-ug/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Parada de Unidade Geradora por Conveniência Operativa"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default ParadaUGQuery;

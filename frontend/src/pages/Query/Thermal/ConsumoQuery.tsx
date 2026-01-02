import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface ConsumoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  consumoInterno: number;
  perdasTransmissao: number;
  compensacao: number;
  consumoLiquido: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'consumoInterno', label: 'Consumo Interno (MWh)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'perdasTransmissao', label: 'Perdas Transmissão (MWh)', minWidth: 200, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'compensacao', label: 'Compensação (MWh)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'consumoLiquido', label: 'Consumo Líquido (MWh)', minWidth: 180, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const ConsumoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<ConsumoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<ConsumoQueryData[]>(`/consulta/consumo?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/consumo/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Perdas, Consumo Interno e Compensação"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default ConsumoQuery;

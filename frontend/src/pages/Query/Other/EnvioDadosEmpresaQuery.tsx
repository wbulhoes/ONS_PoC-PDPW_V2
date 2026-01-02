import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface EnvioDadosEmpresaQueryData {
  id: number;
  dataPdp: string;
  empresa: string;
  dataEnvio: string;
  horaEnvio: string;
  tipoEnvio: string;
  statusEnvio: string;
  quantidadeDados: number;
  usuarioEnvio: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'empresa', label: 'Empresa', minWidth: 250 },
  { id: 'dataEnvio', label: 'Data Envio', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'horaEnvio', label: 'Hora', minWidth: 100 },
  { id: 'tipoEnvio', label: 'Tipo de Envio', minWidth: 150 },
  { id: 'statusEnvio', label: 'Status', minWidth: 130 },
  { id: 'quantidadeDados', label: 'Qtd. Registros', minWidth: 140, align: 'right' },
  { id: 'usuarioEnvio', label: 'Usuário', minWidth: 180 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const EnvioDadosEmpresaQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<EnvioDadosEmpresaQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);

    const data = await apiClient.get<EnvioDadosEmpresaQueryData[]>(`/consulta/envio-dados-empresa?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);

    const blob = await apiClient.get<Blob>(`/consulta/envio-dados-empresa/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Envio de Dados pela Empresa"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={false}
      showDateRange={true}
    />
  );
};

export default EnvioDadosEmpresaQuery;

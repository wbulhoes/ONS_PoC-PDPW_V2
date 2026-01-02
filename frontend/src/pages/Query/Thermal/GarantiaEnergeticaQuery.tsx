import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface GarantiaEnergeticaQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  garantiaFisica: number;
  garantiaContratada: number;
  garantiaDisponivel: number;
  percentualUtilizacao: number;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'garantiaFisica', label: 'Garantia Física (MWmed)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'garantiaContratada', label: 'Garantia Contratada (MWmed)', minWidth: 210, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'garantiaDisponivel', label: 'Garantia Disponível (MWmed)', minWidth: 210, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'percentualUtilizacao', label: 'Utilização (%)', minWidth: 140, align: 'right', format: (value) => value.toFixed(1) },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const GarantiaEnergeticaQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<GarantiaEnergeticaQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<GarantiaEnergeticaQueryData[]>(`/consulta/garantia-energetica?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/garantia-energetica/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Garantia Energética"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default GarantiaEnergeticaQuery;

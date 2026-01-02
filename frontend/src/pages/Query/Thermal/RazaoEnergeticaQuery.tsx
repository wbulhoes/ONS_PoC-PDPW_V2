import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface RazaoEnergeticaQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  geracaoProgramada: number;
  geracaoVerificada: number;
  razaoEnergetica: string;
  motivoDespacho: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'geracaoProgramada', label: 'Ger. Programada (MWmed)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'geracaoVerificada', label: 'Ger. Verificada (MWmed)', minWidth: 190, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'razaoEnergetica', label: 'Razão Energética', minWidth: 200 },
  { id: 'motivoDespacho', label: 'Motivo Despacho', minWidth: 250 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const RazaoEnergeticaQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<RazaoEnergeticaQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<RazaoEnergeticaQueryData[]>(`/consulta/razao-energetica?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/razao-energetica/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Razão Energética"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default RazaoEnergeticaQuery;

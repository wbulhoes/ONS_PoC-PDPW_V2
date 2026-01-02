import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface CompensacaoLastroQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  periodo: string;
  lastroFisico: number;
  energiaCompensada: number;
  saldoCompensacao: number;
  tipoCompensacao: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'periodo', label: 'Período', minWidth: 100 },
  { id: 'lastroFisico', label: 'Lastro Físico (MWh)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'energiaCompensada', label: 'Energia Compensada (MWh)', minWidth: 200, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'saldoCompensacao', label: 'Saldo (MWh)', minWidth: 140, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'tipoCompensacao', label: 'Tipo', minWidth: 150 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const CompensacaoLastroQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<CompensacaoLastroQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<CompensacaoLastroQueryData[]>(`/consulta/compensacao-lastro?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/compensacao-lastro/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Compensação de Lastro Físico"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default CompensacaoLastroQuery;

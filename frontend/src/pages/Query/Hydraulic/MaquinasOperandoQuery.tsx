import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';

interface MaquinasOperandoQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  numeroMaquinasOperando: number;
  potenciaOperando: number;
  tipoOperacao: string;
  periodo: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'numeroMaquinasOperando', label: 'Nº Máq. Operando', minWidth: 160, align: 'center' },
  { id: 'potenciaOperando', label: 'Potência (MW)', minWidth: 140, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'tipoOperacao', label: 'Tipo Operação', minWidth: 150 },
  { id: 'periodo', label: 'Período', minWidth: 120 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const MaquinasOperandoQuery: React.FC = () => {
  const handleSearch = async (filters: BaseQueryFilter): Promise<MaquinasOperandoQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const data = await apiClient.get<MaquinasOperandoQueryData[]>(`/consulta/maquinas-operando?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);

    const blob = await apiClient.get<Blob>(`/consulta/maquinas-operando/export?${params.toString()}`);
    return blob;
  };

  return (
    <BaseQueryPage
      title="Consulta de Número de Máquinas Operando como Síncrono"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
    />
  );
};

export default MaquinasOperandoQuery;

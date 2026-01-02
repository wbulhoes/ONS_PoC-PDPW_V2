import React from 'react';
import BaseQueryPage, { BaseQueryColumn, BaseQueryFilter } from '../../../components/BaseQueryPage';
import { apiClient } from '../../../services/apiClient';
import { Grid, MenuItem, TextField } from '@mui/material';

interface DisponibilidadeQueryData {
  id: number;
  dataPdp: string;
  usina: string;
  empresa: string;
  tipoUsina: string;
  potenciaDisponivel: number;
  potenciaInstalada: number;
  percentualDisponibilidade: number;
  motivoIndisponibilidade?: string;
  observacao?: string;
}

const columns: BaseQueryColumn[] = [
  { id: 'dataPdp', label: 'Data PDP', minWidth: 120, format: (value) => new Date(value).toLocaleDateString('pt-BR') },
  { id: 'usina', label: 'Usina', minWidth: 200 },
  { id: 'empresa', label: 'Empresa', minWidth: 200 },
  { id: 'tipoUsina', label: 'Tipo', minWidth: 100 },
  { id: 'potenciaDisponivel', label: 'Pot. Disponível (MW)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'potenciaInstalada', label: 'Pot. Instalada (MW)', minWidth: 170, align: 'right', format: (value) => value.toFixed(2) },
  { id: 'percentualDisponibilidade', label: 'Disponibilidade (%)', minWidth: 160, align: 'right', format: (value) => value.toFixed(1) },
  { id: 'motivoIndisponibilidade', label: 'Motivo Indisponibilidade', minWidth: 250 },
  { id: 'observacao', label: 'Observação', minWidth: 200 },
];

const DisponibilidadeQuery: React.FC = () => {
  const [tipoUsina, setTipoUsina] = React.useState('');

  const handleSearch = async (filters: BaseQueryFilter): Promise<DisponibilidadeQueryData[]> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);
    if (tipoUsina) params.append('tipoUsina', tipoUsina);

    const data = await apiClient.get<DisponibilidadeQueryData[]>(`/consulta/disponibilidade?${params.toString()}`);
    return data;
  };

  const handleExport = async (filters: BaseQueryFilter): Promise<Blob> => {
    const params = new URLSearchParams();
    if (filters.dataPdpInicio) params.append('dataInicio', filters.dataPdpInicio);
    if (filters.dataPdpFim) params.append('dataFim', filters.dataPdpFim);
    if (filters.empresaId) params.append('empresaId', filters.empresaId);
    if (filters.usinaId) params.append('usinaId', filters.usinaId);
    if (tipoUsina) params.append('tipoUsina', tipoUsina);

    const blob = await apiClient.get<Blob>(`/consulta/disponibilidade/export?${params.toString()}`);
    return blob;
  };

  const renderCustomFilters = () => (
    <Grid item xs={12} md={3}>
      <TextField
        fullWidth
        select
        label="Tipo de Usina"
        value={tipoUsina}
        onChange={(e) => setTipoUsina(e.target.value)}
      >
        <MenuItem value="">Todos</MenuItem>
        <MenuItem value="H">Hidráulica</MenuItem>
        <MenuItem value="T">Térmica</MenuItem>
        <MenuItem value="E">Eólica</MenuItem>
        <MenuItem value="S">Solar</MenuItem>
      </TextField>
    </Grid>
  );

  return (
    <BaseQueryPage
      title="Consulta de Disponibilidade"
      columns={columns}
      onSearch={handleSearch}
      onExport={handleExport}
      showEmpresaFilter={true}
      showUsinaFilter={true}
      showDateRange={true}
      renderCustomFilters={renderCustomFilters}
    />
  );
};

export default DisponibilidadeQuery;

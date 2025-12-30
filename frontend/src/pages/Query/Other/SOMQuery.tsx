import React, { useState, useEffect } from 'react';
import {
  Box,
  Paper,
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Button,
  Alert,
  CircularProgress,
  Grid,
  MenuItem,
  Select,
  FormControl,
  InputLabel
} from '@mui/material';
import { Search as SearchIcon, GetApp as DownloadIcon } from '@mui/icons-material';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers';
import { ptBR } from 'date-fns/locale';

interface SOMData {
  id: number;
  dataReferencia: string;
  revisao: number;
  usina: string;
  subsistema: string;
  periodoInicio: string;
  periodoFim: string;
  potenciaMaxima: number;
  usuario: string;
  dataInclusao: string;
}

const SOMQuery: React.FC = () => {
  const [dataInicio, setDataInicio] = useState<Date | null>(null);
  const [dataFim, setDataFim] = useState<Date | null>(null);
  const [subsistema, setSubsistema] = useState<string>('');
  const [usina, setUsina] = useState<string>('');
  const [subsistemas, setSubsistemas] = useState<string[]>([]);
  const [usinas, setUsinas] = useState<string[]>([]);
  const [dados, setDados] = useState<SOMData[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  useEffect(() => {
    loadSubsistemas();
  }, []);

  useEffect(() => {
    if (subsistema) {
      loadUsinas(subsistema);
    }
  }, [subsistema]);

  const loadSubsistemas = async () => {
    try {
      const response = await fetch('/api/subsistemas');
      if (response.ok) {
        const data = await response.json();
        setSubsistemas(data);
      }
    } catch (err) {
      console.error('Erro ao carregar subsistemas:', err);
    }
  };

  const loadUsinas = async (subsistemaId: string) => {
    try {
      const response = await fetch(`/api/usinas/subsistema/${subsistemaId}`);
      if (response.ok) {
        const data = await response.json();
        setUsinas(data);
      }
    } catch (err) {
      console.error('Erro ao carregar usinas:', err);
    }
  };

  const handleSearch = async () => {
    if (!dataInicio || !dataFim) {
      setError('Informe o período de consulta');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const params = new URLSearchParams();
      params.append('dataInicio', dataInicio.toISOString().split('T')[0]);
      params.append('dataFim', dataFim.toISOString().split('T')[0]);
      if (subsistema) params.append('subsistema', subsistema);
      if (usina) params.append('usina', usina);

      const response = await fetch(`/api/outros-dados/som?${params.toString()}`);
      
      if (!response.ok) {
        throw new Error('Erro ao consultar dados');
      }

      const data = await response.json();
      setDados(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao consultar dados');
    } finally {
      setLoading(false);
    }
  };

  const handleExport = async () => {
    try {
      const params = new URLSearchParams();
      if (dataInicio) params.append('dataInicio', dataInicio.toISOString().split('T')[0]);
      if (dataFim) params.append('dataFim', dataFim.toISOString().split('T')[0]);
      if (subsistema) params.append('subsistema', subsistema);
      if (usina) params.append('usina', usina);

      const response = await fetch(`/api/outros-dados/som/exportar?${params.toString()}`);
      
      if (!response.ok) {
        throw new Error('Erro ao exportar dados');
      }

      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `SOM_${new Date().toISOString().split('T')[0]}.xlsx`;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Erro ao exportar dados');
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" gutterBottom>
        Consulta - SOM (Super Oferta Mensal)
      </Typography>

      <Paper sx={{ p: 3, mb: 3 }}>
        <Typography variant="h6" gutterBottom>
          Filtros de Consulta
        </Typography>

        <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={ptBR}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={3}>
              <DatePicker
                label="Data Início *"
                value={dataInicio}
                onChange={setDataInicio}
                slotProps={{
                  textField: {
                    fullWidth: true,
                    required: true
                  }
                }}
              />
            </Grid>

            <Grid item xs={12} md={3}>
              <DatePicker
                label="Data Fim *"
                value={dataFim}
                onChange={setDataFim}
                slotProps={{
                  textField: {
                    fullWidth: true,
                    required: true
                  }
                }}
              />
            </Grid>

            <Grid item xs={12} md={3}>
              <FormControl fullWidth>
                <InputLabel>Subsistema</InputLabel>
                <Select
                  value={subsistema}
                  label="Subsistema"
                  onChange={(e) => setSubsistema(e.target.value)}
                >
                  <MenuItem value="">Todos</MenuItem>
                  {subsistemas.map((sub) => (
                    <MenuItem key={sub} value={sub}>
                      {sub}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12} md={3}>
              <FormControl fullWidth disabled={!subsistema}>
                <InputLabel>Usina</InputLabel>
                <Select
                  value={usina}
                  label="Usina"
                  onChange={(e) => setUsina(e.target.value)}
                >
                  <MenuItem value="">Todas</MenuItem>
                  {usinas.map((u) => (
                    <MenuItem key={u} value={u}>
                      {u}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12}>
              <Box sx={{ display: 'flex', gap: 2 }}>
                <Button
                  variant="contained"
                  startIcon={<SearchIcon />}
                  onClick={handleSearch}
                  disabled={loading}
                >
                  Consultar
                </Button>
                <Button
                  variant="outlined"
                  startIcon={<DownloadIcon />}
                  onClick={handleExport}
                  disabled={loading || dados.length === 0}
                >
                  Exportar
                </Button>
              </Box>
            </Grid>
          </Grid>
        </LocalizationProvider>
      </Paper>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }} onClose={() => setError('')}>
          {error}
        </Alert>
      )}

      {loading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 3 }}>
          <CircularProgress />
        </Box>
      )}

      {!loading && dados.length > 0 && (
        <Paper>
          <TableContainer>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Data Referência</TableCell>
                  <TableCell>Revisão</TableCell>
                  <TableCell>Usina</TableCell>
                  <TableCell>Subsistema</TableCell>
                  <TableCell>Período Início</TableCell>
                  <TableCell>Período Fim</TableCell>
                  <TableCell align="right">Potência Máxima (MW)</TableCell>
                  <TableCell>Usuário</TableCell>
                  <TableCell>Data Inclusão</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {dados.map((row) => (
                  <TableRow key={row.id}>
                    <TableCell>
                      {new Date(row.dataReferencia).toLocaleDateString('pt-BR')}
                    </TableCell>
                    <TableCell>{row.revisao}</TableCell>
                    <TableCell>{row.usina}</TableCell>
                    <TableCell>{row.subsistema}</TableCell>
                    <TableCell>
                      {new Date(row.periodoInicio).toLocaleString('pt-BR')}
                    </TableCell>
                    <TableCell>
                      {new Date(row.periodoFim).toLocaleString('pt-BR')}
                    </TableCell>
                    <TableCell align="right">
                      {row.potenciaMaxima.toFixed(2)}
                    </TableCell>
                    <TableCell>{row.usuario}</TableCell>
                    <TableCell>
                      {new Date(row.dataInclusao).toLocaleString('pt-BR')}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Paper>
      )}

      {!loading && dados.length === 0 && dataInicio && dataFim && (
        <Alert severity="info">
          Nenhum registro encontrado para os filtros selecionados.
        </Alert>
      )}
    </Box>
  );
};

export default SOMQuery;

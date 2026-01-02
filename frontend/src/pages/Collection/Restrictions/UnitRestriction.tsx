import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  TextField,
  MenuItem,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert,
  Snackbar,
  Chip,
} from '@mui/material';
import { Add, Edit, Delete, Save, Cancel } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

interface UnitRestriction {
  id?: number;
  dataPdp: string;
  usinaId: number;
  usinaNome: string;
  unidadeGeradoraId: number;
  unidadeGeradoraNome: string;
  tipoRestricao: string;
  dataInicio: string;
  dataFim: string;
  potenciaMaxima: number;
  potenciaMinima: number;
  observacao: string;
  status: string;
}

interface Usina {
  id: number;
  nome: string;
  sigla: string;
}

interface UnidadeGeradora {
  id: number;
  nome: string;
  potenciaNominal: number;
}

const UnitRestriction: React.FC = () => {
  const [restrictions, setRestrictions] = useState<UnitRestriction[]>([]);
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [unidades, setUnidades] = useState<UnidadeGeradora[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingRestriction, setEditingRestriction] = useState<UnitRestriction | null>(null);
  const [formData, setFormData] = useState<Partial<UnitRestriction>>({
    dataPdp: '',
    usinaId: 0,
    unidadeGeradoraId: 0,
    tipoRestricao: '',
    dataInicio: '',
    dataFim: '',
    potenciaMaxima: 0,
    potenciaMinima: 0,
    observacao: '',
    status: 'ATIVA',
  });
  const [filters, setFilters] = useState({
    dataPdp: '',
    usinaId: 0,
    status: '',
  });
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  const tiposRestricao = [
    'MANUTENCAO',
    'FALHA_EQUIPAMENTO',
    'RESTRICAO_OPERATIVA',
    'TESTE',
    'OUTROS',
  ];

  const statusOptions = ['ATIVA', 'INATIVA', 'CANCELADA'];

  useEffect(() => {
    loadUsinas();
    loadRestrictions();
  }, []);

  useEffect(() => {
    if (formData.usinaId) {
      loadUnidades(formData.usinaId);
    }
  }, [formData.usinaId]);

  useEffect(() => {
    loadRestrictions();
  }, [filters]);

  const loadUsinas = async () => {
    try {
      const data = await apiClient.get<Usina[]>('/usinas');
      setUsinas(data);
    } catch (error) {
      console.error('Erro ao carregar usinas');
    }
  };

  const loadUnidades = async (usinaId: number) => {
    try {
      const data = await apiClient.get<UnidadeGeradora[]>(`/usinas/${usinaId}/unidades`);
      setUnidades(data);
    } catch (error) {
      console.error('Erro ao carregar unidades geradoras');
      setUnidades([]);
    }
  };

  const loadRestrictions = async () => {
    try {
      const params = new URLSearchParams();
      if (filters.dataPdp) params.append('dataPdp', filters.dataPdp);
      if (filters.usinaId) params.append('usinaId', filters.usinaId.toString());
      if (filters.status) params.append('status', filters.status);

      const data = await apiClient.get<UnitRestriction[]>(`/coleta/restricao-ug?${params.toString()}`);
      setRestrictions(data);
    } catch (error) {
      console.error('Erro ao carregar restrições');
      setRestrictions([]);
    }
  };

  const handleOpenDialog = (restriction?: UnitRestriction) => {
    if (restriction) {
      setEditingRestriction(restriction);
      setFormData(restriction);
    } else {
      setEditingRestriction(null);
      setFormData({
        dataPdp: '',
        usinaId: 0,
        unidadeGeradoraId: 0,
        tipoRestricao: '',
        dataInicio: '',
        dataFim: '',
        potenciaMaxima: 0,
        potenciaMinima: 0,
        observacao: '',
        status: 'ATIVA',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingRestriction(null);
    setFormData({
      dataPdp: '',
      usinaId: 0,
      unidadeGeradoraId: 0,
      tipoRestricao: '',
      dataInicio: '',
      dataFim: '',
      potenciaMaxima: 0,
      potenciaMinima: 0,
      observacao: '',
      status: 'ATIVA',
    });
  };

  const handleSave = async () => {
    try {
      if (editingRestriction?.id) {
        await apiClient.put(`/coleta/restricao-ug/${editingRestriction.id}`, formData);
        setSnackbar({
          open: true,
          message: 'Restrição atualizada com sucesso',
          severity: 'success',
        });
      } else {
        await apiClient.post('/coleta/restricao-ug', formData);
        setSnackbar({
          open: true,
          message: 'Restrição criada com sucesso',
          severity: 'success',
        });
      }
      handleCloseDialog();
      loadRestrictions();
    } catch (error) {
      setSnackbar({
        open: true,
        message: 'Erro ao salvar restrição',
        severity: 'error',
      });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir esta restrição?')) {
      try {
        await apiClient.delete(`/coleta/restricao-ug/${id}`);
        setSnackbar({
          open: true,
          message: 'Restrição excluída com sucesso',
          severity: 'success',
        });
        loadRestrictions();
      } catch (error) {
        setSnackbar({
          open: true,
          message: 'Erro ao excluir restrição',
          severity: 'error',
        });
      }
    }
  };

  const getStatusColor = (status: string) => {
    const colors: any = {
      ATIVA: 'success',
      INATIVA: 'default',
      CANCELADA: 'error',
    };
    return colors[status] || 'default';
  };

  const getTipoRestricaoLabel = (tipo: string) => {
    const labels: any = {
      MANUTENCAO: 'Manutenção',
      FALHA_EQUIPAMENTO: 'Falha de Equipamento',
      RESTRICAO_OPERATIVA: 'Restrição Operativa',
      TESTE: 'Teste',
      OUTROS: 'Outros',
    };
    return labels[tipo] || tipo;
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Restrições de Unidades Geradoras</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Nova Restrição
        </Button>
      </Box>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Filtros
          </Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Data PDP"
                type="date"
                value={filters.dataPdp}
                onChange={(e) => setFilters({ ...filters, dataPdp: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                select
                label="Usina"
                value={filters.usinaId}
                onChange={(e) => setFilters({ ...filters, usinaId: Number(e.target.value) })}
              >
                <MenuItem value={0}>Todas</MenuItem>
                {usinas.map((usina) => (
                  <MenuItem key={usina.id} value={usina.id}>
                    {usina.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                select
                label="Status"
                value={filters.status}
                onChange={(e) => setFilters({ ...filters, status: e.target.value })}
              >
                <MenuItem value="">Todos</MenuItem>
                {statusOptions.map((status) => (
                  <MenuItem key={status} value={status}>
                    {status}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Data PDP</TableCell>
              <TableCell>Usina</TableCell>
              <TableCell>Unidade Geradora</TableCell>
              <TableCell>Tipo</TableCell>
              <TableCell>Período</TableCell>
              <TableCell>Potência (MW)</TableCell>
              <TableCell>Status</TableCell>
              <TableCell align="center">Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {restrictions.map((restriction) => (
              <TableRow key={restriction.id}>
                <TableCell>{new Date(restriction.dataPdp).toLocaleDateString()}</TableCell>
                <TableCell>{restriction.usinaNome}</TableCell>
                <TableCell>{restriction.unidadeGeradoraNome}</TableCell>
                <TableCell>{getTipoRestricaoLabel(restriction.tipoRestricao)}</TableCell>
                <TableCell>
                  {new Date(restriction.dataInicio).toLocaleDateString()} -{' '}
                  {new Date(restriction.dataFim).toLocaleDateString()}
                </TableCell>
                <TableCell>
                  {restriction.potenciaMinima} - {restriction.potenciaMaxima}
                </TableCell>
                <TableCell>
                  <Chip
                    label={restriction.status}
                    color={getStatusColor(restriction.status)}
                    size="small"
                  />
                </TableCell>
                <TableCell align="center">
                  <IconButton
                    size="small"
                    color="primary"
                    onClick={() => handleOpenDialog(restriction)}
                  >
                    <Edit />
                  </IconButton>
                  <IconButton
                    size="small"
                    color="error"
                    onClick={() => handleDelete(restriction.id!)}
                  >
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {restrictions.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            Nenhuma restrição encontrada
          </Typography>
        </Box>
      )}

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>
          {editingRestriction ? 'Editar Restrição' : 'Nova Restrição'}
        </DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Data PDP"
                type="date"
                value={formData.dataPdp}
                onChange={(e) => setFormData({ ...formData, dataPdp: e.target.value })}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Usina"
                value={formData.usinaId}
                onChange={(e) => setFormData({ ...formData, usinaId: Number(e.target.value) })}
                required
              >
                <MenuItem value={0}>Selecione</MenuItem>
                {usinas.map((usina) => (
                  <MenuItem key={usina.id} value={usina.id}>
                    {usina.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Unidade Geradora"
                value={formData.unidadeGeradoraId}
                onChange={(e) =>
                  setFormData({ ...formData, unidadeGeradoraId: Number(e.target.value) })
                }
                required
                disabled={!formData.usinaId}
              >
                <MenuItem value={0}>Selecione</MenuItem>
                {unidades.map((unidade) => (
                  <MenuItem key={unidade.id} value={unidade.id}>
                    {unidade.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Tipo de Restrição"
                value={formData.tipoRestricao}
                onChange={(e) => setFormData({ ...formData, tipoRestricao: e.target.value })}
                required
              >
                {tiposRestricao.map((tipo) => (
                  <MenuItem key={tipo} value={tipo}>
                    {getTipoRestricaoLabel(tipo)}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Data Início"
                type="date"
                value={formData.dataInicio}
                onChange={(e) => setFormData({ ...formData, dataInicio: e.target.value })}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Data Fim"
                type="date"
                value={formData.dataFim}
                onChange={(e) => setFormData({ ...formData, dataFim: e.target.value })}
                InputLabelProps={{ shrink: true }}
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Potência Mínima (MW)"
                type="number"
                value={formData.potenciaMinima}
                onChange={(e) =>
                  setFormData({ ...formData, potenciaMinima: parseFloat(e.target.value) || 0 })
                }
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Potência Máxima (MW)"
                type="number"
                value={formData.potenciaMaxima}
                onChange={(e) =>
                  setFormData({ ...formData, potenciaMaxima: parseFloat(e.target.value) || 0 })
                }
                required
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Status"
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: e.target.value })}
                required
              >
                {statusOptions.map((status) => (
                  <MenuItem key={status} value={status}>
                    {status}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Observação"
                multiline
                rows={3}
                value={formData.observacao}
                onChange={(e) => setFormData({ ...formData, observacao: e.target.value })}
              />
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog} startIcon={<Cancel />}>
            Cancelar
          </Button>
          <Button onClick={handleSave} variant="contained" startIcon={<Save />}>
            Salvar
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar({ ...snackbar, open: false })}
      >
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default UnitRestriction;

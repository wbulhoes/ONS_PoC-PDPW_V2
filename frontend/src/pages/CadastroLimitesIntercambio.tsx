import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
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
  Grid,
  MenuItem,
  Alert,
  Snackbar,
} from '@mui/material';
import { Edit, Delete, Add } from '@mui/icons-material';
import api from '../services/api';

interface LimiteIntercambio {
  id?: number;
  interligacaoId: number;
  interligacaoNome?: string;
  dataInicio: string;
  dataFim: string;
  limiteMinimo: number;
  limiteMaximo: number;
  periodo: string;
}

const CadastroLimitesIntercambio: React.FC = () => {
  const [limites, setLimites] = useState<LimiteIntercambio[]>([]);
  const [interligacoes, setInterligacoes] = useState<any[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingLimite, setEditingLimite] = useState<LimiteIntercambio | null>(null);
  const [formData, setFormData] = useState<LimiteIntercambio>({
    interligacaoId: 0,
    dataInicio: '',
    dataFim: '',
    limiteMinimo: 0,
    limiteMaximo: 0,
    periodo: 'DIARIO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadLimites();
    loadInterligacoes();
  }, []);

  const loadLimites = async () => {
    try {
      const response = await api.get('/limites-intercambio');
      setLimites(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar limites de intercâmbio', severity: 'error' });
    }
  };

  const loadInterligacoes = async () => {
    try {
      const response = await api.get('/interligacoes');
      setInterligacoes(response.data);
    } catch (error) {
      console.error('Erro ao carregar interligações');
    }
  };

  const handleOpenDialog = (limite?: LimiteIntercambio) => {
    if (limite) {
      setEditingLimite(limite);
      setFormData(limite);
    } else {
      setEditingLimite(null);
      setFormData({
        interligacaoId: 0,
        dataInicio: '',
        dataFim: '',
        limiteMinimo: 0,
        limiteMaximo: 0,
        periodo: 'DIARIO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingLimite(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingLimite) {
        await api.put(`/limites-intercambio/${editingLimite.id}`, formData);
        setSnackbar({ open: true, message: 'Limite de intercâmbio atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/limites-intercambio', formData);
        setSnackbar({ open: true, message: 'Limite de intercâmbio criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadLimites();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar limite de intercâmbio', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este limite de intercâmbio?')) {
      try {
        await api.delete(`/limites-intercambio/${id}`);
        setSnackbar({ open: true, message: 'Limite de intercâmbio excluído com sucesso', severity: 'success' });
        loadLimites();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir limite de intercâmbio', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Limites de Intercâmbio</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Limite
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Interligação</TableCell>
              <TableCell>Data Início</TableCell>
              <TableCell>Data Fim</TableCell>
              <TableCell>Limite Mínimo (MW)</TableCell>
              <TableCell>Limite Máximo (MW)</TableCell>
              <TableCell>Período</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {limites.map((limite) => (
              <TableRow key={limite.id}>
                <TableCell>{limite.interligacaoNome}</TableCell>
                <TableCell>{new Date(limite.dataInicio).toLocaleDateString()}</TableCell>
                <TableCell>{new Date(limite.dataFim).toLocaleDateString()}</TableCell>
                <TableCell>{limite.limiteMinimo}</TableCell>
                <TableCell>{limite.limiteMaximo}</TableCell>
                <TableCell>{limite.periodo}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(limite)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(limite.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingLimite ? 'Editar Limite de Intercâmbio' : 'Novo Limite de Intercâmbio'}</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
              <TextField
                fullWidth
                select
                label="Interligação"
                value={formData.interligacaoId}
                onChange={(e) => setFormData({ ...formData, interligacaoId: Number(e.target.value) })}
              >
                {interligacoes.map((interligacao) => (
                  <MenuItem key={interligacao.id} value={interligacao.id}>
                    {interligacao.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="date"
                label="Data Início"
                value={formData.dataInicio}
                onChange={(e) => setFormData({ ...formData, dataInicio: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="date"
                label="Data Fim"
                value={formData.dataFim}
                onChange={(e) => setFormData({ ...formData, dataFim: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Limite Mínimo (MW)"
                value={formData.limiteMinimo}
                onChange={(e) => setFormData({ ...formData, limiteMinimo: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Limite Máximo (MW)"
                value={formData.limiteMaximo}
                onChange={(e) => setFormData({ ...formData, limiteMaximo: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                select
                label="Período"
                value={formData.periodo}
                onChange={(e) => setFormData({ ...formData, periodo: e.target.value })}
              >
                <MenuItem value="DIARIO">Diário</MenuItem>
                <MenuItem value="SEMANAL">Semanal</MenuItem>
                <MenuItem value="MENSAL">Mensal</MenuItem>
                <MenuItem value="ANUAL">Anual</MenuItem>
              </TextField>
            </Grid>
          </Grid>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancelar</Button>
          <Button onClick={handleSubmit} variant="contained">
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

export default CadastroLimitesIntercambio;

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

interface Interligacao {
  id?: number;
  nome: string;
  codigo: string;
  subsistemaOrigemId: number;
  subsistemaDestinoId: number;
  subsistemaOrigemNome?: string;
  subsistemaDestinoNome?: string;
  capacidade: number;
  status: string;
}

const CadastroInterligacoes: React.FC = () => {
  const [interligacoes, setInterligacoes] = useState<Interligacao[]>([]);
  const [subsistemas, setSubsistemas] = useState<any[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingInterligacao, setEditingInterligacao] = useState<Interligacao | null>(null);
  const [formData, setFormData] = useState<Interligacao>({
    nome: '',
    codigo: '',
    subsistemaOrigemId: 0,
    subsistemaDestinoId: 0,
    capacidade: 0,
    status: 'ATIVO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadInterligacoes();
    loadSubsistemas();
  }, []);

  const loadInterligacoes = async () => {
    try {
      const response = await api.get('/interligacoes');
      setInterligacoes(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar interligações', severity: 'error' });
    }
  };

  const loadSubsistemas = async () => {
    try {
      const response = await api.get('/subsistemas');
      setSubsistemas(response.data);
    } catch (error) {
      console.error('Erro ao carregar subsistemas');
    }
  };

  const handleOpenDialog = (interligacao?: Interligacao) => {
    if (interligacao) {
      setEditingInterligacao(interligacao);
      setFormData(interligacao);
    } else {
      setEditingInterligacao(null);
      setFormData({
        nome: '',
        codigo: '',
        subsistemaOrigemId: 0,
        subsistemaDestinoId: 0,
        capacidade: 0,
        status: 'ATIVO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingInterligacao(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingInterligacao) {
        await api.put(`/interligacoes/${editingInterligacao.id}`, formData);
        setSnackbar({ open: true, message: 'Interligação atualizada com sucesso', severity: 'success' });
      } else {
        await api.post('/interligacoes', formData);
        setSnackbar({ open: true, message: 'Interligação criada com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadInterligacoes();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar interligação', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir esta interligação?')) {
      try {
        await api.delete(`/interligacoes/${id}`);
        setSnackbar({ open: true, message: 'Interligação excluída com sucesso', severity: 'success' });
        loadInterligacoes();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir interligação', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Interligações</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Nova Interligação
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Subsistema Origem</TableCell>
              <TableCell>Subsistema Destino</TableCell>
              <TableCell>Capacidade (MW)</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {interligacoes.map((interligacao) => (
              <TableRow key={interligacao.id}>
                <TableCell>{interligacao.codigo}</TableCell>
                <TableCell>{interligacao.nome}</TableCell>
                <TableCell>{interligacao.subsistemaOrigemNome}</TableCell>
                <TableCell>{interligacao.subsistemaDestinoNome}</TableCell>
                <TableCell>{interligacao.capacidade}</TableCell>
                <TableCell>{interligacao.status}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(interligacao)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(interligacao.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingInterligacao ? 'Editar Interligação' : 'Nova Interligação'}</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Código"
                value={formData.codigo}
                onChange={(e) => setFormData({ ...formData, codigo: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Nome"
                value={formData.nome}
                onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Subsistema Origem"
                value={formData.subsistemaOrigemId}
                onChange={(e) => setFormData({ ...formData, subsistemaOrigemId: Number(e.target.value) })}
              >
                {subsistemas.map((subsistema) => (
                  <MenuItem key={subsistema.id} value={subsistema.id}>
                    {subsistema.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Subsistema Destino"
                value={formData.subsistemaDestinoId}
                onChange={(e) => setFormData({ ...formData, subsistemaDestinoId: Number(e.target.value) })}
              >
                {subsistemas.map((subsistema) => (
                  <MenuItem key={subsistema.id} value={subsistema.id}>
                    {subsistema.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Capacidade (MW)"
                value={formData.capacidade}
                onChange={(e) => setFormData({ ...formData, capacidade: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Status"
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: e.target.value })}
              >
                <MenuItem value="ATIVO">Ativo</MenuItem>
                <MenuItem value="INATIVO">Inativo</MenuItem>
                <MenuItem value="MANUTENCAO">Manutenção</MenuItem>
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

export default CadastroInterligacoes;

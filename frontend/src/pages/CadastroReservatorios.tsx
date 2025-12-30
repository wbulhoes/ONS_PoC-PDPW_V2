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

interface Reservatorio {
  id?: number;
  nome: string;
  codigo: string;
  usinaId: number;
  usinaNome?: string;
  volumeUtil: number;
  volumeMinimo: number;
  volumeMaximo: number;
  cotaMinima: number;
  cotaMaxima: number;
}

const CadastroReservatorios: React.FC = () => {
  const [reservatorios, setReservatorios] = useState<Reservatorio[]>([]);
  const [usinas, setUsinas] = useState<any[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingReservatorio, setEditingReservatorio] = useState<Reservatorio | null>(null);
  const [formData, setFormData] = useState<Reservatorio>({
    nome: '',
    codigo: '',
    usinaId: 0,
    volumeUtil: 0,
    volumeMinimo: 0,
    volumeMaximo: 0,
    cotaMinima: 0,
    cotaMaxima: 0,
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadReservatorios();
    loadUsinas();
  }, []);

  const loadReservatorios = async () => {
    try {
      const response = await api.get('/reservatorios');
      setReservatorios(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar reservatórios', severity: 'error' });
    }
  };

  const loadUsinas = async () => {
    try {
      const response = await api.get('/usinas');
      setUsinas(response.data);
    } catch (error) {
      console.error('Erro ao carregar usinas');
    }
  };

  const handleOpenDialog = (reservatorio?: Reservatorio) => {
    if (reservatorio) {
      setEditingReservatorio(reservatorio);
      setFormData(reservatorio);
    } else {
      setEditingReservatorio(null);
      setFormData({
        nome: '',
        codigo: '',
        usinaId: 0,
        volumeUtil: 0,
        volumeMinimo: 0,
        volumeMaximo: 0,
        cotaMinima: 0,
        cotaMaxima: 0,
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingReservatorio(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingReservatorio) {
        await api.put(`/reservatorios/${editingReservatorio.id}`, formData);
        setSnackbar({ open: true, message: 'Reservatório atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/reservatorios', formData);
        setSnackbar({ open: true, message: 'Reservatório criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadReservatorios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar reservatório', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este reservatório?')) {
      try {
        await api.delete(`/reservatorios/${id}`);
        setSnackbar({ open: true, message: 'Reservatório excluído com sucesso', severity: 'success' });
        loadReservatorios();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir reservatório', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Reservatórios</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Reservatório
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Usina</TableCell>
              <TableCell>Volume Útil (hm³)</TableCell>
              <TableCell>Volume Mín (hm³)</TableCell>
              <TableCell>Volume Máx (hm³)</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {reservatorios.map((reservatorio) => (
              <TableRow key={reservatorio.id}>
                <TableCell>{reservatorio.codigo}</TableCell>
                <TableCell>{reservatorio.nome}</TableCell>
                <TableCell>{reservatorio.usinaNome}</TableCell>
                <TableCell>{reservatorio.volumeUtil}</TableCell>
                <TableCell>{reservatorio.volumeMinimo}</TableCell>
                <TableCell>{reservatorio.volumeMaximo}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(reservatorio)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(reservatorio.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingReservatorio ? 'Editar Reservatório' : 'Novo Reservatório'}</DialogTitle>
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
            <Grid item xs={12}>
              <TextField
                fullWidth
                select
                label="Usina"
                value={formData.usinaId}
                onChange={(e) => setFormData({ ...formData, usinaId: Number(e.target.value) })}
              >
                {usinas.map((usina) => (
                  <MenuItem key={usina.id} value={usina.id}>
                    {usina.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Volume Útil (hm³)"
                value={formData.volumeUtil}
                onChange={(e) => setFormData({ ...formData, volumeUtil: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Volume Mínimo (hm³)"
                value={formData.volumeMinimo}
                onChange={(e) => setFormData({ ...formData, volumeMinimo: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Volume Máximo (hm³)"
                value={formData.volumeMaximo}
                onChange={(e) => setFormData({ ...formData, volumeMaximo: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Cota Mínima (m)"
                value={formData.cotaMinima}
                onChange={(e) => setFormData({ ...formData, cotaMinima: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Cota Máxima (m)"
                value={formData.cotaMaxima}
                onChange={(e) => setFormData({ ...formData, cotaMaxima: Number(e.target.value) })}
              />
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

export default CadastroReservatorios;

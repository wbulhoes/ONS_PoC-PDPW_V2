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
  Alert,
  Snackbar,
} from '@mui/material';
import { Edit, Delete, Add } from '@mui/icons-material';
import api from '../services/api';

interface Subsistema {
  id?: number;
  nome: string;
  codigo: string;
  descricao: string;
  capacidadeTotal: number;
}

const CadastroSubsistemas: React.FC = () => {
  const [subsistemas, setSubsistemas] = useState<Subsistema[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingSubsistema, setEditingSubsistema] = useState<Subsistema | null>(null);
  const [formData, setFormData] = useState<Subsistema>({
    nome: '',
    codigo: '',
    descricao: '',
    capacidadeTotal: 0,
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSubsistemas();
  }, []);

  const loadSubsistemas = async () => {
    try {
      const response = await api.get('/subsistemas');
      setSubsistemas(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar subsistemas', severity: 'error' });
    }
  };

  const handleOpenDialog = (subsistema?: Subsistema) => {
    if (subsistema) {
      setEditingSubsistema(subsistema);
      setFormData(subsistema);
    } else {
      setEditingSubsistema(null);
      setFormData({
        nome: '',
        codigo: '',
        descricao: '',
        capacidadeTotal: 0,
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingSubsistema(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingSubsistema) {
        await api.put(`/subsistemas/${editingSubsistema.id}`, formData);
        setSnackbar({ open: true, message: 'Subsistema atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/subsistemas', formData);
        setSnackbar({ open: true, message: 'Subsistema criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadSubsistemas();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar subsistema', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este subsistema?')) {
      try {
        await api.delete(`/subsistemas/${id}`);
        setSnackbar({ open: true, message: 'Subsistema excluído com sucesso', severity: 'success' });
        loadSubsistemas();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir subsistema', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Subsistemas</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Subsistema
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Descrição</TableCell>
              <TableCell>Capacidade Total (MW)</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {subsistemas.map((subsistema) => (
              <TableRow key={subsistema.id}>
                <TableCell>{subsistema.codigo}</TableCell>
                <TableCell>{subsistema.nome}</TableCell>
                <TableCell>{subsistema.descricao}</TableCell>
                <TableCell>{subsistema.capacidadeTotal}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(subsistema)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(subsistema.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingSubsistema ? 'Editar Subsistema' : 'Novo Subsistema'}</DialogTitle>
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
                label="Descrição"
                multiline
                rows={3}
                value={formData.descricao}
                onChange={(e) => setFormData({ ...formData, descricao: e.target.value })}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                type="number"
                label="Capacidade Total (MW)"
                value={formData.capacidadeTotal}
                onChange={(e) => setFormData({ ...formData, capacidadeTotal: Number(e.target.value) })}
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

export default CadastroSubsistemas;

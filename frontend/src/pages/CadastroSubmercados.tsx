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

interface Submercado {
  id?: number;
  nome: string;
  codigo: string;
  descricao: string;
  demandaMedia: number;
}

const CadastroSubmercados: React.FC = () => {
  const [submercados, setSubmercados] = useState<Submercado[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingSubmercado, setEditingSubmercado] = useState<Submercado | null>(null);
  const [formData, setFormData] = useState<Submercado>({
    nome: '',
    codigo: '',
    descricao: '',
    demandaMedia: 0,
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSubmercados();
  }, []);

  const loadSubmercados = async () => {
    try {
      const response = await api.get('/submercados');
      setSubmercados(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar submercados', severity: 'error' });
    }
  };

  const handleOpenDialog = (submercado?: Submercado) => {
    if (submercado) {
      setEditingSubmercado(submercado);
      setFormData(submercado);
    } else {
      setEditingSubmercado(null);
      setFormData({
        nome: '',
        codigo: '',
        descricao: '',
        demandaMedia: 0,
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingSubmercado(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingSubmercado) {
        await api.put(`/submercados/${editingSubmercado.id}`, formData);
        setSnackbar({ open: true, message: 'Submercado atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/submercados', formData);
        setSnackbar({ open: true, message: 'Submercado criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadSubmercados();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar submercado', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este submercado?')) {
      try {
        await api.delete(`/submercados/${id}`);
        setSnackbar({ open: true, message: 'Submercado excluído com sucesso', severity: 'success' });
        loadSubmercados();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir submercado', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Submercados</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Submercado
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Descrição</TableCell>
              <TableCell>Demanda Média (MW)</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {submercados.map((submercado) => (
              <TableRow key={submercado.id}>
                <TableCell>{submercado.codigo}</TableCell>
                <TableCell>{submercado.nome}</TableCell>
                <TableCell>{submercado.descricao}</TableCell>
                <TableCell>{submercado.demandaMedia}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(submercado)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(submercado.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingSubmercado ? 'Editar Submercado' : 'Novo Submercado'}</DialogTitle>
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
                label="Demanda Média (MW)"
                value={formData.demandaMedia}
                onChange={(e) => setFormData({ ...formData, demandaMedia: Number(e.target.value) })}
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

export default CadastroSubmercados;

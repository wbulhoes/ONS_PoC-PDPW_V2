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

interface Usina {
  id?: number;
  nome: string;
  codigo: string;
  tipo: string;
  capacidadeInstalada: number;
  subsistema: string;
  status: string;
}

const CadastroUsinas: React.FC = () => {
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingUsina, setEditingUsina] = useState<Usina | null>(null);
  const [formData, setFormData] = useState<Usina>({
    nome: '',
    codigo: '',
    tipo: 'HIDRO',
    capacidadeInstalada: 0,
    subsistema: 'SUDESTE',
    status: 'ATIVO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadUsinas();
  }, []);

  const loadUsinas = async () => {
    try {
      const response = await api.get('/usinas');
      setUsinas(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar usinas', severity: 'error' });
    }
  };

  const handleOpenDialog = (usina?: Usina) => {
    if (usina) {
      setEditingUsina(usina);
      setFormData(usina);
    } else {
      setEditingUsina(null);
      setFormData({
        nome: '',
        codigo: '',
        tipo: 'HIDRO',
        capacidadeInstalada: 0,
        subsistema: 'SUDESTE',
        status: 'ATIVO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingUsina(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingUsina) {
        await api.put(`/usinas/${editingUsina.id}`, formData);
        setSnackbar({ open: true, message: 'Usina atualizada com sucesso', severity: 'success' });
      } else {
        await api.post('/usinas', formData);
        setSnackbar({ open: true, message: 'Usina criada com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadUsinas();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar usina', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir esta usina?')) {
      try {
        await api.delete(`/usinas/${id}`);
        setSnackbar({ open: true, message: 'Usina excluída com sucesso', severity: 'success' });
        loadUsinas();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir usina', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Usinas</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Nova Usina
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Tipo</TableCell>
              <TableCell>Capacidade (MW)</TableCell>
              <TableCell>Subsistema</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {usinas.map((usina) => (
              <TableRow key={usina.id}>
                <TableCell>{usina.codigo}</TableCell>
                <TableCell>{usina.nome}</TableCell>
                <TableCell>{usina.tipo}</TableCell>
                <TableCell>{usina.capacidadeInstalada}</TableCell>
                <TableCell>{usina.subsistema}</TableCell>
                <TableCell>{usina.status}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(usina)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(usina.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingUsina ? 'Editar Usina' : 'Nova Usina'}</DialogTitle>
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
                label="Tipo"
                value={formData.tipo}
                onChange={(e) => setFormData({ ...formData, tipo: e.target.value })}
              >
                <MenuItem value="HIDRO">Hidrelétrica</MenuItem>
                <MenuItem value="TERMO">Termelétrica</MenuItem>
                <MenuItem value="EOLICA">Eólica</MenuItem>
                <MenuItem value="SOLAR">Solar</MenuItem>
                <MenuItem value="NUCLEAR">Nuclear</MenuItem>
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Capacidade Instalada (MW)"
                value={formData.capacidadeInstalada}
                onChange={(e) => setFormData({ ...formData, capacidadeInstalada: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Subsistema"
                value={formData.subsistema}
                onChange={(e) => setFormData({ ...formData, subsistema: e.target.value })}
              >
                <MenuItem value="SUDESTE">Sudeste</MenuItem>
                <MenuItem value="SUL">Sul</MenuItem>
                <MenuItem value="NORDESTE">Nordeste</MenuItem>
                <MenuItem value="NORTE">Norte</MenuItem>
              </TextField>
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

export default CadastroUsinas;

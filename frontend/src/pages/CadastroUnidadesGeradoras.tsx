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

interface UnidadeGeradora {
  id?: number;
  nome: string;
  codigo: string;
  usinaId: number;
  usinaNome?: string;
  potenciaNominal: number;
  potenciaMinima: number;
  fatorCapacidade: number;
  status: string;
}

const CadastroUnidadesGeradoras: React.FC = () => {
  const [unidades, setUnidades] = useState<UnidadeGeradora[]>([]);
  const [usinas, setUsinas] = useState<any[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingUnidade, setEditingUnidade] = useState<UnidadeGeradora | null>(null);
  const [formData, setFormData] = useState<UnidadeGeradora>({
    nome: '',
    codigo: '',
    usinaId: 0,
    potenciaNominal: 0,
    potenciaMinima: 0,
    fatorCapacidade: 0,
    status: 'ATIVO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadUnidades();
    loadUsinas();
  }, []);

  const loadUnidades = async () => {
    try {
      const response = await api.get('/unidades-geradoras');
      setUnidades(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar unidades geradoras', severity: 'error' });
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

  const handleOpenDialog = (unidade?: UnidadeGeradora) => {
    if (unidade) {
      setEditingUnidade(unidade);
      setFormData(unidade);
    } else {
      setEditingUnidade(null);
      setFormData({
        nome: '',
        codigo: '',
        usinaId: 0,
        potenciaNominal: 0,
        potenciaMinima: 0,
        fatorCapacidade: 0,
        status: 'ATIVO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingUnidade(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingUnidade) {
        await api.put(`/unidades-geradoras/${editingUnidade.id}`, formData);
        setSnackbar({ open: true, message: 'Unidade geradora atualizada com sucesso', severity: 'success' });
      } else {
        await api.post('/unidades-geradoras', formData);
        setSnackbar({ open: true, message: 'Unidade geradora criada com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadUnidades();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar unidade geradora', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir esta unidade geradora?')) {
      try {
        await api.delete(`/unidades-geradoras/${id}`);
        setSnackbar({ open: true, message: 'Unidade geradora excluída com sucesso', severity: 'success' });
        loadUnidades();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir unidade geradora', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Cadastro de Unidades Geradoras</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Nova Unidade
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Código</TableCell>
              <TableCell>Nome</TableCell>
              <TableCell>Usina</TableCell>
              <TableCell>Potência Nominal (MW)</TableCell>
              <TableCell>Potência Mínima (MW)</TableCell>
              <TableCell>Fator Capacidade</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {unidades.map((unidade) => (
              <TableRow key={unidade.id}>
                <TableCell>{unidade.codigo}</TableCell>
                <TableCell>{unidade.nome}</TableCell>
                <TableCell>{unidade.usinaNome}</TableCell>
                <TableCell>{unidade.potenciaNominal}</TableCell>
                <TableCell>{unidade.potenciaMinima}</TableCell>
                <TableCell>{unidade.fatorCapacidade}</TableCell>
                <TableCell>{unidade.status}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(unidade)} color="primary">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(unidade.id!)} color="error">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingUnidade ? 'Editar Unidade Geradora' : 'Nova Unidade Geradora'}</DialogTitle>
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
                label="Potência Nominal (MW)"
                value={formData.potenciaNominal}
                onChange={(e) => setFormData({ ...formData, potenciaNominal: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Potência Mínima (MW)"
                value={formData.potenciaMinima}
                onChange={(e) => setFormData({ ...formData, potenciaMinima: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                type="number"
                label="Fator de Capacidade"
                value={formData.fatorCapacidade}
                onChange={(e) => setFormData({ ...formData, fatorCapacidade: Number(e.target.value) })}
                inputProps={{ step: 0.01, min: 0, max: 1 }}
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

export default CadastroUnidadesGeradoras;

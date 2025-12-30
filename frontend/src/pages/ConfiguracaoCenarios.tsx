import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  MenuItem,
  Alert,
  Snackbar,
  Chip,
  Divider,
} from '@mui/material';
import { Add, Edit, Delete, ContentCopy, PlayArrow } from '@mui/icons-material';
import api from '../services/api';

interface Cenario {
  id?: number;
  nome: string;
  descricao: string;
  dataInicio: string;
  dataFim: string;
  horizonte: number;
  tipoSimulacao: string;
  parametros: any;
  status: string;
}

const ConfiguracaoCenarios: React.FC = () => {
  const [cenarios, setCenarios] = useState<Cenario[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingCenario, setEditingCenario] = useState<Cenario | null>(null);
  const [formData, setFormData] = useState<Cenario>({
    nome: '',
    descricao: '',
    dataInicio: '',
    dataFim: '',
    horizonte: 12,
    tipoSimulacao: 'DETERMINISTICO',
    parametros: {},
    status: 'CONFIGURACAO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadCenarios();
  }, []);

  const loadCenarios = async () => {
    try {
      const response = await api.get('/cenarios');
      setCenarios(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar cenários', severity: 'error' });
    }
  };

  const handleOpenDialog = (cenario?: Cenario) => {
    if (cenario) {
      setEditingCenario(cenario);
      setFormData(cenario);
    } else {
      setEditingCenario(null);
      setFormData({
        nome: '',
        descricao: '',
        dataInicio: '',
        dataFim: '',
        horizonte: 12,
        tipoSimulacao: 'DETERMINISTICO',
        parametros: {},
        status: 'CONFIGURACAO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingCenario(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingCenario) {
        await api.put(`/cenarios/${editingCenario.id}`, formData);
        setSnackbar({ open: true, message: 'Cenário atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/cenarios', formData);
        setSnackbar({ open: true, message: 'Cenário criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadCenarios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar cenário', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este cenário?')) {
      try {
        await api.delete(`/cenarios/${id}`);
        setSnackbar({ open: true, message: 'Cenário excluído com sucesso', severity: 'success' });
        loadCenarios();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir cenário', severity: 'error' });
      }
    }
  };

  const handleDuplicate = async (cenario: Cenario) => {
    try {
      const novoCenario = {
        ...cenario,
        nome: `${cenario.nome} (Cópia)`,
        status: 'CONFIGURACAO',
      };
      delete novoCenario.id;
      await api.post('/cenarios', novoCenario);
      setSnackbar({ open: true, message: 'Cenário duplicado com sucesso', severity: 'success' });
      loadCenarios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao duplicar cenário', severity: 'error' });
    }
  };

  const handleExecute = async (id: number) => {
    try {
      await api.post(`/cenarios/${id}/executar`);
      setSnackbar({ open: true, message: 'Simulação iniciada com sucesso', severity: 'success' });
      loadCenarios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao iniciar simulação', severity: 'error' });
    }
  };

  const getStatusColor = (status: string) => {
    const colors: any = {
      CONFIGURACAO: 'default',
      PRONTO: 'primary',
      EXECUTANDO: 'warning',
      CONCLUIDO: 'success',
      ERRO: 'error',
    };
    return colors[status] || 'default';
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Configuração de Cenários</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Cenário
        </Button>
      </Box>

      <Grid container spacing={3}>
        {cenarios.map((cenario) => (
          <Grid item xs={12} md={6} lg={4} key={cenario.id}>
            <Card>
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'start', mb: 2 }}>
                  <Typography variant="h6">{cenario.nome}</Typography>
                  <Chip label={cenario.status} color={getStatusColor(cenario.status)} size="small" />
                </Box>

                <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                  {cenario.descricao}
                </Typography>

                <Divider sx={{ my: 2 }} />

                <Grid container spacing={1}>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">
                      Tipo
                    </Typography>
                    <Typography variant="body2">{cenario.tipoSimulacao}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">
                      Horizonte
                    </Typography>
                    <Typography variant="body2">{cenario.horizonte} meses</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">
                      Data Início
                    </Typography>
                    <Typography variant="body2">
                      {new Date(cenario.dataInicio).toLocaleDateString()}
                    </Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">
                      Data Fim
                    </Typography>
                    <Typography variant="body2">
                      {new Date(cenario.dataFim).toLocaleDateString()}
                    </Typography>
                  </Grid>
                </Grid>

                <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 2, gap: 1 }}>
                  {cenario.status === 'PRONTO' && (
                    <IconButton
                      size="small"
                      color="success"
                      onClick={() => handleExecute(cenario.id!)}
                      title="Executar"
                    >
                      <PlayArrow />
                    </IconButton>
                  )}
                  <IconButton
                    size="small"
                    color="primary"
                    onClick={() => handleDuplicate(cenario)}
                    title="Duplicar"
                  >
                    <ContentCopy />
                  </IconButton>
                  <IconButton
                    size="small"
                    color="primary"
                    onClick={() => handleOpenDialog(cenario)}
                    title="Editar"
                  >
                    <Edit />
                  </IconButton>
                  <IconButton
                    size="small"
                    color="error"
                    onClick={() => handleDelete(cenario.id!)}
                    title="Excluir"
                  >
                    <Delete />
                  </IconButton>
                </Box>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="md" fullWidth>
        <DialogTitle>{editingCenario ? 'Editar Cenário' : 'Novo Cenário'}</DialogTitle>
        <DialogContent>
          <Grid container spacing={2} sx={{ mt: 1 }}>
            <Grid item xs={12}>
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
                label="Horizonte (meses)"
                value={formData.horizonte}
                onChange={(e) => setFormData({ ...formData, horizonte: Number(e.target.value) })}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                select
                label="Tipo de Simulação"
                value={formData.tipoSimulacao}
                onChange={(e) => setFormData({ ...formData, tipoSimulacao: e.target.value })}
              >
                <MenuItem value="DETERMINISTICO">Determinístico</MenuItem>
                <MenuItem value="ESTOCASTICO">Estocástico</MenuItem>
                <MenuItem value="HIBRIDO">Híbrido</MenuItem>
              </TextField>
            </Grid>
            <Grid item xs={12}>
              <TextField
                fullWidth
                select
                label="Status"
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: e.target.value })}
              >
                <MenuItem value="CONFIGURACAO">Configuração</MenuItem>
                <MenuItem value="PRONTO">Pronto</MenuItem>
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

export default ConfiguracaoCenarios;

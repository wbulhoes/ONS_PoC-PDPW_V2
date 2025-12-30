import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
  IconButton,
  LinearProgress,
  Grid,
  Card,
  CardContent,
  Alert,
  Snackbar,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  List,
  ListItem,
  ListItemText,
  Divider,
} from '@mui/material';
import { Visibility, Stop, Refresh } from '@mui/icons-material';
import api from '../services/api';

interface Simulacao {
  id: number;
  cenarioId: number;
  cenarioNome: string;
  status: string;
  progresso: number;
  mensagem: string;
  dataInicio: string;
  dataFim?: string;
  tempoDecorrido?: string;
  tempoEstimado?: string;
}

const MonitoramentoSimulacoes: React.FC = () => {
  const [simulacoes, setSimulacoes] = useState<Simulacao[]>([]);
  const [selectedSimulacao, setSelectedSimulacao] = useState<Simulacao | null>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });
  const [autoRefresh, setAutoRefresh] = useState(true);

  useEffect(() => {
    loadSimulacoes();
  }, []);

  useEffect(() => {
    let interval: NodeJS.Timeout;
    if (autoRefresh) {
      interval = setInterval(() => {
        loadSimulacoes();
      }, 3000);
    }
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [autoRefresh]);

  const loadSimulacoes = async () => {
    try {
      const response = await api.get('/simulacoes');
      setSimulacoes(response.data);
    } catch (error) {
      console.error('Erro ao carregar simulações');
    }
  };

  const handleViewDetails = async (simulacao: Simulacao) => {
    try {
      const response = await api.get(`/simulacoes/${simulacao.id}`);
      setSelectedSimulacao(response.data);
      setOpenDialog(true);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar detalhes da simulação', severity: 'error' });
    }
  };

  const handleStop = async (id: number) => {
    if (window.confirm('Deseja realmente parar esta simulação?')) {
      try {
        await api.post(`/simulacoes/${id}/parar`);
        setSnackbar({ open: true, message: 'Simulação interrompida', severity: 'success' });
        loadSimulacoes();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao parar simulação', severity: 'error' });
      }
    }
  };

  const getStatusColor = (status: string) => {
    const colors: any = {
      AGUARDANDO: 'default',
      EXECUTANDO: 'warning',
      CONCLUIDA: 'success',
      ERRO: 'error',
      CANCELADA: 'default',
    };
    return colors[status] || 'default';
  };

  const getSummary = () => {
    const executando = simulacoes.filter(s => s.status === 'EXECUTANDO').length;
    const concluidas = simulacoes.filter(s => s.status === 'CONCLUIDA').length;
    const erros = simulacoes.filter(s => s.status === 'ERRO').length;
    const aguardando = simulacoes.filter(s => s.status === 'AGUARDANDO').length;
    return { executando, concluidas, erros, aguardando, total: simulacoes.length };
  };

  const summary = getSummary();

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Monitoramento de Simulações</Typography>
        <IconButton onClick={() => loadSimulacoes()} color="primary">
          <Refresh />
        </IconButton>
      </Box>

      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography variant="h4" color="warning.main">
                {summary.executando}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Em Execução
              </Typography>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography variant="h4" color="success.main">
                {summary.concluidas}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Concluídas
              </Typography>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography variant="h4" color="error.main">
                {summary.erros}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Com Erro
              </Typography>
            </CardContent>
          </Card>
        </Grid>
        <Grid item xs={12} sm={6} md={3}>
          <Card>
            <CardContent>
              <Typography variant="h4" color="primary.main">
                {summary.aguardando}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Aguardando
              </Typography>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Cenário</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Progresso</TableCell>
              <TableCell>Data Início</TableCell>
              <TableCell>Tempo Decorrido</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {simulacoes.map((simulacao) => (
              <TableRow key={simulacao.id}>
                <TableCell>{simulacao.id}</TableCell>
                <TableCell>{simulacao.cenarioNome}</TableCell>
                <TableCell>
                  <Chip label={simulacao.status} color={getStatusColor(simulacao.status)} size="small" />
                </TableCell>
                <TableCell>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Box sx={{ width: '100%', minWidth: 100 }}>
                      <LinearProgress variant="determinate" value={simulacao.progresso} />
                    </Box>
                    <Typography variant="body2">{simulacao.progresso}%</Typography>
                  </Box>
                </TableCell>
                <TableCell>{new Date(simulacao.dataInicio).toLocaleString()}</TableCell>
                <TableCell>{simulacao.tempoDecorrido || '-'}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleViewDetails(simulacao)} color="primary" size="small">
                    <Visibility />
                  </IconButton>
                  {simulacao.status === 'EXECUTANDO' && (
                    <IconButton onClick={() => handleStop(simulacao.id)} color="error" size="small">
                      <Stop />
                    </IconButton>
                  )}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {simulacoes.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            Nenhuma simulação encontrada
          </Typography>
        </Box>
      )}

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)} maxWidth="md" fullWidth>
        <DialogTitle>Detalhes da Simulação</DialogTitle>
        <DialogContent>
          {selectedSimulacao && (
            <List>
              <ListItem>
                <ListItemText primary="ID" secondary={selectedSimulacao.id} />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemText primary="Cenário" secondary={selectedSimulacao.cenarioNome} />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemText
                  primary="Status"
                  secondary={
                    <Chip
                      label={selectedSimulacao.status}
                      color={getStatusColor(selectedSimulacao.status)}
                      size="small"
                    />
                  }
                />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemText primary="Progresso" secondary={`${selectedSimulacao.progresso}%`} />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemText primary="Mensagem" secondary={selectedSimulacao.mensagem} />
              </ListItem>
              <Divider />
              <ListItem>
                <ListItemText
                  primary="Data de Início"
                  secondary={new Date(selectedSimulacao.dataInicio).toLocaleString()}
                />
              </ListItem>
              {selectedSimulacao.dataFim && (
                <>
                  <Divider />
                  <ListItem>
                    <ListItemText
                      primary="Data de Término"
                      secondary={new Date(selectedSimulacao.dataFim).toLocaleString()}
                    />
                  </ListItem>
                </>
              )}
              {selectedSimulacao.tempoDecorrido && (
                <>
                  <Divider />
                  <ListItem>
                    <ListItemText primary="Tempo Decorrido" secondary={selectedSimulacao.tempoDecorrido} />
                  </ListItem>
                </>
              )}
              {selectedSimulacao.tempoEstimado && (
                <>
                  <Divider />
                  <ListItem>
                    <ListItemText primary="Tempo Estimado" secondary={selectedSimulacao.tempoEstimado} />
                  </ListItem>
                </>
              )}
            </List>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)}>Fechar</Button>
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

export default MonitoramentoSimulacoes;

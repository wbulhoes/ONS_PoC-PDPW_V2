import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  MenuItem,
  TextField,
  Alert,
  Snackbar,
  LinearProgress,
  Chip,
  List,
  ListItem,
  ListItemText,
  Divider,
} from '@mui/material';
import { PlayArrow, Stop } from '@mui/icons-material';
import api from '../services/api';

interface Cenario {
  id: number;
  nome: string;
  descricao: string;
  status: string;
}

interface SimulacaoConfig {
  cenarioId: number;
  numeroIteracoes: number;
  numeroThreads: number;
  salvarResultadosIntermediarios: boolean;
  parametrosAdicionais: any;
}

interface SimulacaoStatus {
  id: number;
  cenarioId: number;
  status: string;
  progresso: number;
  mensagem: string;
  dataInicio: string;
  dataFim?: string;
}

const ExecucaoSimulacoes: React.FC = () => {
  const [cenarios, setCenarios] = useState<Cenario[]>([]);
  const [selectedCenario, setSelectedCenario] = useState<number>(0);
  const [config, setConfig] = useState<SimulacaoConfig>({
    cenarioId: 0,
    numeroIteracoes: 2000,
    numeroThreads: 4,
    salvarResultadosIntermediarios: false,
    parametrosAdicionais: {},
  });
  const [simulacaoAtual, setSimulacaoAtual] = useState<SimulacaoStatus | null>(null);
  const [executing, setExecuting] = useState(false);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadCenarios();
  }, []);

  useEffect(() => {
    let interval: NodeJS.Timeout;
    if (executing && simulacaoAtual) {
      interval = setInterval(() => {
        checkSimulacaoStatus(simulacaoAtual.id);
      }, 2000);
    }
    return () => {
      if (interval) clearInterval(interval);
    };
  }, [executing, simulacaoAtual]);

  const loadCenarios = async () => {
    try {
      const response = await api.get('/cenarios?status=PRONTO');
      setCenarios(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar cenários', severity: 'error' });
    }
  };

  const checkSimulacaoStatus = async (simulacaoId: number) => {
    try {
      const response = await api.get(`/simulacoes/${simulacaoId}/status`);
      setSimulacaoAtual(response.data);
      
      if (response.data.status === 'CONCLUIDA' || response.data.status === 'ERRO') {
        setExecuting(false);
        const message = response.data.status === 'CONCLUIDA' 
          ? 'Simulação concluída com sucesso' 
          : 'Simulação finalizada com erro';
        setSnackbar({ 
          open: true, 
          message, 
          severity: response.data.status === 'CONCLUIDA' ? 'success' : 'error' 
        });
      }
    } catch (error) {
      console.error('Erro ao verificar status da simulação');
    }
  };

  const handleExecute = async () => {
    if (!selectedCenario) {
      setSnackbar({ open: true, message: 'Selecione um cenário', severity: 'error' });
      return;
    }

    setExecuting(true);
    const configData = { ...config, cenarioId: selectedCenario };

    try {
      const response = await api.post('/simulacoes/executar', configData);
      setSimulacaoAtual(response.data);
      setSnackbar({ open: true, message: 'Simulação iniciada com sucesso', severity: 'success' });
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Erro ao iniciar simulação';
      setSnackbar({ open: true, message: errorMessage, severity: 'error' });
      setExecuting(false);
    }
  };

  const handleStop = async () => {
    if (!simulacaoAtual) return;

    try {
      await api.post(`/simulacoes/${simulacaoAtual.id}/parar`);
      setSnackbar({ open: true, message: 'Simulação interrompida', severity: 'warning' });
      setExecuting(false);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao parar simulação', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Execução de Simulações
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} md={5}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configuração
              </Typography>

              <TextField
                fullWidth
                select
                label="Cenário"
                value={selectedCenario}
                onChange={(e) => {
                  setSelectedCenario(Number(e.target.value));
                  setConfig({ ...config, cenarioId: Number(e.target.value) });
                }}
                sx={{ mb: 2 }}
                disabled={executing}
              >
                <MenuItem value={0}>Selecione um cenário</MenuItem>
                {cenarios.map((cenario) => (
                  <MenuItem key={cenario.id} value={cenario.id}>
                    {cenario.nome}
                  </MenuItem>
                ))}
              </TextField>

              <TextField
                fullWidth
                type="number"
                label="Número de Iterações"
                value={config.numeroIteracoes}
                onChange={(e) => setConfig({ ...config, numeroIteracoes: Number(e.target.value) })}
                sx={{ mb: 2 }}
                disabled={executing}
              />

              <TextField
                fullWidth
                type="number"
                label="Número de Threads"
                value={config.numeroThreads}
                onChange={(e) => setConfig({ ...config, numeroThreads: Number(e.target.value) })}
                sx={{ mb: 2 }}
                disabled={executing}
                inputProps={{ min: 1, max: 16 }}
              />

              <TextField
                fullWidth
                select
                label="Salvar Resultados Intermediários"
                value={config.salvarResultadosIntermediarios ? 'sim' : 'nao'}
                onChange={(e) => setConfig({ ...config, salvarResultadosIntermediarios: e.target.value === 'sim' })}
                sx={{ mb: 3 }}
                disabled={executing}
              >
                <MenuItem value="sim">Sim</MenuItem>
                <MenuItem value="nao">Não</MenuItem>
              </TextField>

              {!executing ? (
                <Button
                  variant="contained"
                  color="primary"
                  onClick={handleExecute}
                  disabled={!selectedCenario}
                  startIcon={<PlayArrow />}
                  fullWidth
                >
                  Iniciar Simulação
                </Button>
              ) : (
                <Button
                  variant="contained"
                  color="error"
                  onClick={handleStop}
                  startIcon={<Stop />}
                  fullWidth
                >
                  Parar Simulação
                </Button>
              )}
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={7}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Status da Execução
              </Typography>

              {simulacaoAtual ? (
                <>
                  <Box sx={{ mb: 3 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                      <Typography variant="body2">Progresso</Typography>
                      <Typography variant="body2">{simulacaoAtual.progresso}%</Typography>
                    </Box>
                    <LinearProgress variant="determinate" value={simulacaoAtual.progresso} />
                  </Box>

                  <List>
                    <ListItem>
                      <ListItemText
                        primary="Status"
                        secondary={
                          <Chip
                            label={simulacaoAtual.status}
                            color={
                              simulacaoAtual.status === 'EXECUTANDO'
                                ? 'warning'
                                : simulacaoAtual.status === 'CONCLUIDA'
                                ? 'success'
                                : 'error'
                            }
                            size="small"
                          />
                        }
                      />
                    </ListItem>
                    <Divider />
                    <ListItem>
                      <ListItemText primary="Mensagem" secondary={simulacaoAtual.mensagem} />
                    </ListItem>
                    <Divider />
                    <ListItem>
                      <ListItemText
                        primary="Data de Início"
                        secondary={new Date(simulacaoAtual.dataInicio).toLocaleString()}
                      />
                    </ListItem>
                    {simulacaoAtual.dataFim && (
                      <>
                        <Divider />
                        <ListItem>
                          <ListItemText
                            primary="Data de Término"
                            secondary={new Date(simulacaoAtual.dataFim).toLocaleString()}
                          />
                        </ListItem>
                      </>
                    )}
                  </List>

                  {simulacaoAtual.status === 'CONCLUIDA' && (
                    <Alert severity="success" sx={{ mt: 2 }}>
                      Simulação concluída com sucesso! Os resultados estão disponíveis para análise.
                    </Alert>
                  )}

                  {simulacaoAtual.status === 'ERRO' && (
                    <Alert severity="error" sx={{ mt: 2 }}>
                      A simulação foi finalizada com erro. Verifique os logs para mais detalhes.
                    </Alert>
                  )}
                </>
              ) : (
                <Box sx={{ textAlign: 'center', py: 4 }}>
                  <Typography variant="body1" color="text.secondary">
                    Nenhuma simulação em execução
                  </Typography>
                  <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                    Configure os parâmetros e clique em "Iniciar Simulação"
                  </Typography>
                </Box>
              )}
            </CardContent>
          </Card>
        </Grid>
      </Grid>

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

export default ExecucaoSimulacoes;

import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  MenuItem,
  TextField,
  Button,
  Alert,
  Snackbar,
  Tabs,
  Tab,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
} from '@mui/material';
import { Line, Bar, Radar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  RadialLinearScale,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Add, Remove } from '@mui/icons-material';
import api from '../services/api';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  RadialLinearScale,
  Title,
  Tooltip,
  Legend
);

interface Simulacao {
  id: number;
  cenarioNome: string;
  status: string;
}

interface ComparacaoResumo {
  simulacaoId: number;
  cenarioNome: string;
  custoTotal: number;
  custoMedio: number;
  geracaoTotal: number;
  geracaoMedia: number;
  intercambioTotal: number;
}

interface ComparacaoSerie {
  periodo: string;
  valores: { [key: number]: number };
}

const ComparacaoCenarios: React.FC = () => {
  const [simulacoes, setSimulacoes] = useState<Simulacao[]>([]);
  const [selectedSimulacoes, setSelectedSimulacoes] = useState<number[]>([]);
  const [tabValue, setTabValue] = useState(0);
  const [resumos, setResumos] = useState<ComparacaoResumo[]>([]);
  const [serieGeracao, setSerieGeracao] = useState<ComparacaoSerie[]>([]);
  const [serieCusto, setSerieCusto] = useState<ComparacaoSerie[]>([]);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSimulacoes();
  }, []);

  const loadSimulacoes = async () => {
    try {
      const response = await api.get('/simulacoes?status=CONCLUIDA');
      setSimulacoes(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar simulações', severity: 'error' });
    }
  };

  const handleAddSimulacao = (simulacaoId: number) => {
    if (!selectedSimulacoes.includes(simulacaoId) && selectedSimulacoes.length < 5) {
      setSelectedSimulacoes([...selectedSimulacoes, simulacaoId]);
    }
  };

  const handleRemoveSimulacao = (simulacaoId: number) => {
    setSelectedSimulacoes(selectedSimulacoes.filter((id) => id !== simulacaoId));
  };

  const handleCompare = async () => {
    if (selectedSimulacoes.length < 2) {
      setSnackbar({ open: true, message: 'Selecione pelo menos 2 simulações', severity: 'error' });
      return;
    }

    try {
      const response = await api.post('/comparacao/executar', {
        simulacaoIds: selectedSimulacoes,
      });

      setResumos(response.data.resumos);
      setSerieGeracao(response.data.serieGeracao);
      setSerieCusto(response.data.serieCusto);
      setSnackbar({ open: true, message: 'Comparação realizada com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao realizar comparação', severity: 'error' });
    }
  };

  const colors = [
    'rgb(75, 192, 192)',
    'rgb(255, 99, 132)',
    'rgb(54, 162, 235)',
    'rgb(255, 206, 86)',
    'rgb(153, 102, 255)',
  ];

  const geracaoChartData = {
    labels: serieGeracao.map((s) => s.periodo),
    datasets: selectedSimulacoes.map((simId, index) => {
      const simulacao = simulacoes.find((s) => s.id === simId);
      return {
        label: simulacao?.cenarioNome || `Simulação ${simId}`,
        data: serieGeracao.map((s) => s.valores[simId] || 0),
        borderColor: colors[index],
        backgroundColor: colors[index].replace('rgb', 'rgba').replace(')', ', 0.2)'),
        tension: 0.1,
      };
    }),
  };

  const custoChartData = {
    labels: serieCusto.map((s) => s.periodo),
    datasets: selectedSimulacoes.map((simId, index) => {
      const simulacao = simulacoes.find((s) => s.id === simId);
      return {
        label: simulacao?.cenarioNome || `Simulação ${simId}`,
        data: serieCusto.map((s) => s.valores[simId] || 0),
        borderColor: colors[index],
        backgroundColor: colors[index].replace('rgb', 'rgba').replace(')', ', 0.2)'),
        tension: 0.1,
      };
    }),
  };

  const resumoChartData = {
    labels: ['Custo Total', 'Custo Médio', 'Geração Total', 'Geração Média', 'Intercâmbio Total'],
    datasets: resumos.map((resumo, index) => ({
      label: resumo.cenarioNome,
      data: [
        resumo.custoTotal / 1000000,
        resumo.custoMedio / 1000,
        resumo.geracaoTotal / 1000000,
        resumo.geracaoMedia / 1000,
        resumo.intercambioTotal / 1000000,
      ],
      backgroundColor: colors[index].replace('rgb', 'rgba').replace(')', ', 0.2)'),
      borderColor: colors[index],
      borderWidth: 2,
    })),
  };

  const chartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top' as const,
      },
    },
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Comparação de Cenários
      </Typography>

      <Grid container spacing={3} sx={{ mb: 3 }}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Selecionar Simulações
              </Typography>
              <TextField
                fullWidth
                select
                label="Adicionar Simulação"
                value=""
                onChange={(e) => handleAddSimulacao(Number(e.target.value))}
                disabled={selectedSimulacoes.length >= 5}
              >
                <MenuItem value="">Selecione uma simulação</MenuItem>
                {simulacoes
                  .filter((s) => !selectedSimulacoes.includes(s.id))
                  .map((simulacao) => (
                    <MenuItem key={simulacao.id} value={simulacao.id}>
                      {simulacao.cenarioNome} (ID: {simulacao.id})
                    </MenuItem>
                  ))}
              </TextField>
              <Typography variant="caption" color="text.secondary" sx={{ mt: 1, display: 'block' }}>
                Máximo de 5 simulações
              </Typography>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Simulações Selecionadas ({selectedSimulacoes.length})
              </Typography>
              {selectedSimulacoes.map((simId) => {
                const simulacao = simulacoes.find((s) => s.id === simId);
                return (
                  <Chip
                    key={simId}
                    label={simulacao?.cenarioNome || `Simulação ${simId}`}
                    onDelete={() => handleRemoveSimulacao(simId)}
                    sx={{ m: 0.5 }}
                  />
                );
              })}
              {selectedSimulacoes.length === 0 && (
                <Typography variant="body2" color="text.secondary">
                  Nenhuma simulação selecionada
                </Typography>
              )}
              <Button
                fullWidth
                variant="contained"
                onClick={handleCompare}
                disabled={selectedSimulacoes.length < 2}
                sx={{ mt: 2 }}
              >
                Comparar
              </Button>
            </CardContent>
          </Card>
        </Grid>
      </Grid>

      {resumos.length > 0 && (
        <>
          <Paper sx={{ mb: 3 }}>
            <Tabs value={tabValue} onChange={(e, newValue) => setTabValue(newValue)}>
              <Tab label="Resumo Comparativo" />
              <Tab label="Geração" />
              <Tab label="Custo" />
              <Tab label="Tabela Comparativa" />
            </Tabs>
          </Paper>

          {tabValue === 0 && (
            <Paper sx={{ p: 3, height: 500 }}>
              <Radar data={resumoChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 1 && (
            <Paper sx={{ p: 3, height: 400 }}>
              <Line data={geracaoChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 2 && (
            <Paper sx={{ p: 3, height: 400 }}>
              <Line data={custoChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 3 && (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Métrica</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.cenarioNome}
                      </TableCell>
                    ))}
                  </TableRow>
                </TableHead>
                <TableBody>
                  <TableRow>
                    <TableCell>Custo Total (R$)</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.custoTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </TableCell>
                    ))}
                  </TableRow>
                  <TableRow>
                    <TableCell>Custo Médio (R$)</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.custoMedio.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </TableCell>
                    ))}
                  </TableRow>
                  <TableRow>
                    <TableCell>Geração Total (MWh)</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.geracaoTotal.toLocaleString('pt-BR')}
                      </TableCell>
                    ))}
                  </TableRow>
                  <TableRow>
                    <TableCell>Geração Média (MWh)</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.geracaoMedia.toLocaleString('pt-BR')}
                      </TableCell>
                    ))}
                  </TableRow>
                  <TableRow>
                    <TableCell>Intercâmbio Total (MWh)</TableCell>
                    {resumos.map((resumo) => (
                      <TableCell key={resumo.simulacaoId} align="right">
                        {resumo.intercambioTotal.toLocaleString('pt-BR')}
                      </TableCell>
                    ))}
                  </TableRow>
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </>
      )}

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

export default ComparacaoCenarios;

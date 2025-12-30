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
} from '@mui/material';
import { Line, Bar } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import api from '../services/api';

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, BarElement, Title, Tooltip, Legend);

interface Simulacao {
  id: number;
  cenarioNome: string;
  status: string;
}

interface ResultadoResumo {
  custoTotal: number;
  custoMedio: number;
  geracaoTotal: number;
  geracaoMedia: number;
  intercambioTotal: number;
}

interface ResultadoSerie {
  periodo: string;
  valor: number;
}

const AnaliseResultados: React.FC = () => {
  const [simulacoes, setSimulacoes] = useState<Simulacao[]>([]);
  const [selectedSimulacao, setSelectedSimulacao] = useState<number>(0);
  const [tabValue, setTabValue] = useState(0);
  const [resumo, setResumo] = useState<ResultadoResumo | null>(null);
  const [serieGeracao, setSerieGeracao] = useState<ResultadoSerie[]>([]);
  const [serieCusto, setSerieCusto] = useState<ResultadoSerie[]>([]);
  const [serieIntercambio, setSerieIntercambio] = useState<ResultadoSerie[]>([]);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSimulacoes();
  }, []);

  useEffect(() => {
    if (selectedSimulacao) {
      loadResultados();
    }
  }, [selectedSimulacao]);

  const loadSimulacoes = async () => {
    try {
      const response = await api.get('/simulacoes?status=CONCLUIDA');
      setSimulacoes(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar simulações', severity: 'error' });
    }
  };

  const loadResultados = async () => {
    try {
      const [resumoRes, geracaoRes, custoRes, intercambioRes] = await Promise.all([
        api.get(`/resultados/${selectedSimulacao}/resumo`),
        api.get(`/resultados/${selectedSimulacao}/serie-geracao`),
        api.get(`/resultados/${selectedSimulacao}/serie-custo`),
        api.get(`/resultados/${selectedSimulacao}/serie-intercambio`),
      ]);

      setResumo(resumoRes.data);
      setSerieGeracao(geracaoRes.data);
      setSerieCusto(custoRes.data);
      setSerieIntercambio(intercambioRes.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar resultados', severity: 'error' });
    }
  };

  const geracaoChartData = {
    labels: serieGeracao.map((s) => s.periodo),
    datasets: [
      {
        label: 'Geração (MWh)',
        data: serieGeracao.map((s) => s.valor),
        borderColor: 'rgb(75, 192, 192)',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        tension: 0.1,
      },
    ],
  };

  const custoChartData = {
    labels: serieCusto.map((s) => s.periodo),
    datasets: [
      {
        label: 'Custo (R$)',
        data: serieCusto.map((s) => s.valor),
        borderColor: 'rgb(255, 99, 132)',
        backgroundColor: 'rgba(255, 99, 132, 0.2)',
        tension: 0.1,
      },
    ],
  };

  const intercambioChartData = {
    labels: serieIntercambio.map((s) => s.periodo),
    datasets: [
      {
        label: 'Intercâmbio (MWh)',
        data: serieIntercambio.map((s) => s.valor),
        backgroundColor: 'rgba(54, 162, 235, 0.5)',
      },
    ],
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
        Análise de Resultados
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <TextField
            fullWidth
            select
            label="Simulação"
            value={selectedSimulacao}
            onChange={(e) => setSelectedSimulacao(Number(e.target.value))}
          >
            <MenuItem value={0}>Selecione uma simulação</MenuItem>
            {simulacoes.map((simulacao) => (
              <MenuItem key={simulacao.id} value={simulacao.id}>
                {simulacao.cenarioNome} (ID: {simulacao.id})
              </MenuItem>
            ))}
          </TextField>
        </CardContent>
      </Card>

      {selectedSimulacao > 0 && resumo && (
        <>
          <Grid container spacing={3} sx={{ mb: 3 }}>
            <Grid item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    Custo Total
                  </Typography>
                  <Typography variant="h5">
                    R$ {resumo.custoTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    Custo Médio
                  </Typography>
                  <Typography variant="h5">
                    R$ {resumo.custoMedio.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    Geração Total
                  </Typography>
                  <Typography variant="h5">
                    {resumo.geracaoTotal.toLocaleString('pt-BR')} MWh
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    Geração Média
                  </Typography>
                  <Typography variant="h5">
                    {resumo.geracaoMedia.toLocaleString('pt-BR')} MWh
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="body2" color="text.secondary">
                    Intercâmbio Total
                  </Typography>
                  <Typography variant="h5">
                    {resumo.intercambioTotal.toLocaleString('pt-BR')} MWh
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>

          <Paper sx={{ mb: 3 }}>
            <Tabs value={tabValue} onChange={(e, newValue) => setTabValue(newValue)}>
              <Tab label="Geração" />
              <Tab label="Custo" />
              <Tab label="Intercâmbio" />
              <Tab label="Dados Tabulares" />
            </Tabs>
          </Paper>

          {tabValue === 0 && (
            <Paper sx={{ p: 3, height: 400 }}>
              <Line data={geracaoChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 1 && (
            <Paper sx={{ p: 3, height: 400 }}>
              <Line data={custoChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 2 && (
            <Paper sx={{ p: 3, height: 400 }}>
              <Bar data={intercambioChartData} options={chartOptions} />
            </Paper>
          )}

          {tabValue === 3 && (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Período</TableCell>
                    <TableCell align="right">Geração (MWh)</TableCell>
                    <TableCell align="right">Custo (R$)</TableCell>
                    <TableCell align="right">Intercâmbio (MWh)</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {serieGeracao.map((item, index) => (
                    <TableRow key={index}>
                      <TableCell>{item.periodo}</TableCell>
                      <TableCell align="right">{item.valor.toLocaleString('pt-BR')}</TableCell>
                      <TableCell align="right">
                        {serieCusto[index]?.valor.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                      </TableCell>
                      <TableCell align="right">
                        {serieIntercambio[index]?.valor.toLocaleString('pt-BR')}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </>
      )}

      {selectedSimulacao === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            Selecione uma simulação para visualizar os resultados
          </Typography>
        </Box>
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

export default AnaliseResultados;

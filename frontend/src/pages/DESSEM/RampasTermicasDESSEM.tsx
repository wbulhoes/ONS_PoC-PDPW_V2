import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Card,
  CardContent,
  TextField,
  MenuItem,
  Button,
  Alert,
  Snackbar,
  Grid,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
} from '@mui/material';
import { Save, Send } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

interface RampaTermica {
  usinaId: string;
  usinaNome: string;
  rampaSubida: number;
  rampaDescida: number;
  tempoMinimo: number;
}

const RampasTermicasDESSEM: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [rampas, setRampas] = useState<RampaTermica[]>([]);
  const [loading, setLoading] = useState(false);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    if (dataPdp) {
      loadRampas();
    }
  }, [dataPdp]);

  const loadRampas = async () => {
    setLoading(true);
    try {
      const data = await apiClient.get<RampaTermica[]>(`/dessem/rampas-termicas?data=${dataPdp}`);
      setRampas(data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar rampas térmicas', severity: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    try {
      await apiClient.post('/dessem/rampas-termicas', { dataPdp, rampas });
      setSnackbar({ open: true, message: 'Rampas térmicas salvas com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar rampas térmicas', severity: 'error' });
    }
  };

  const handleRampaChange = (index: number, field: keyof RampaTermica, value: number) => {
    const newRampas = [...rampas];
    newRampas[index] = { ...newRampas[index], [field]: value };
    setRampas(newRampas);
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Rampas de Usinas Térmicas - DESSEM
      </Typography>

      <Alert severity="info" sx={{ mb: 3 }}>
        Configure as rampas de subida/descida e tempos mínimos para usinas térmicas no modelo DESSEM
      </Alert>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                label="Data PDP"
                type="date"
                value={dataPdp}
                onChange={(e) => setDataPdp(e.target.value)}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {dataPdp && (
        <Card sx={{ mb: 3 }}>
          <CardContent>
            <Typography variant="h6" sx={{ mb: 2 }}>
              Configuração de Rampas por Usina
            </Typography>

            {loading ? (
              <Alert severity="info">Carregando dados...</Alert>
            ) : rampas.length === 0 ? (
              <Alert severity="warning">Nenhuma usina térmica encontrada para a data selecionada</Alert>
            ) : (
              <TableContainer component={Paper}>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Usina Térmica</TableCell>
                      <TableCell align="right">Rampa Subida (MW/min)</TableCell>
                      <TableCell align="right">Rampa Descida (MW/min)</TableCell>
                      <TableCell align="right">Tempo Mínimo (min)</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {rampas.map((rampa, index) => (
                      <TableRow key={rampa.usinaId}>
                        <TableCell>{rampa.usinaNome}</TableCell>
                        <TableCell align="right">
                          <TextField
                            type="number"
                            size="small"
                            value={rampa.rampaSubida}
                            onChange={(e) => handleRampaChange(index, 'rampaSubida', parseFloat(e.target.value) || 0)}
                            sx={{ width: 120 }}
                            inputProps={{ step: 0.1, min: 0 }}
                          />
                        </TableCell>
                        <TableCell align="right">
                          <TextField
                            type="number"
                            size="small"
                            value={rampa.rampaDescida}
                            onChange={(e) => handleRampaChange(index, 'rampaDescida', parseFloat(e.target.value) || 0)}
                            sx={{ width: 120 }}
                            inputProps={{ step: 0.1, min: 0 }}
                          />
                        </TableCell>
                        <TableCell align="right">
                          <TextField
                            type="number"
                            size="small"
                            value={rampa.tempoMinimo}
                            onChange={(e) => handleRampaChange(index, 'tempoMinimo', parseInt(e.target.value) || 0)}
                            sx={{ width: 120 }}
                            inputProps={{ min: 0 }}
                          />
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            )}
          </CardContent>
        </Card>
      )}

      {rampas.length > 0 && (
        <Box sx={{ display: 'flex', gap: 2 }}>
          <Button variant="contained" startIcon={<Save />} onClick={handleSave}>
            Salvar Configurações
          </Button>
          <Button variant="outlined" onClick={loadRampas}>
            Recarregar
          </Button>
        </Box>
      )}

      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default RampasTermicasDESSEM;

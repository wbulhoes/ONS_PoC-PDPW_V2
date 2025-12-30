import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  TextField,
  MenuItem,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Alert,
  Snackbar,
} from '@mui/material';
import { Save, Refresh } from '@mui/icons-material';
import api from '../../../services/api';

const Consumption: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [subsistema, setSubsistema] = useState('');
  const [empresa, setEmpresa] = useState('');
  const [subsistemas, setSubsistemas] = useState<string[]>([]);
  const [empresas, setEmpresas] = useState<string[]>([]);
  const [consumo, setConsumo] = useState({ previsto: 0, realizado: 0, diferenca: 0 });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSubsistemas();
    loadEmpresas();
  }, []);

  useEffect(() => {
    if (dataPdp && subsistema && empresa) {
      loadData();
    }
  }, [dataPdp, subsistema, empresa]);

  const loadSubsistemas = async () => {
    try {
      const response = await api.get('/subsistemas');
      setSubsistemas(response.data.map((s: any) => s.nome));
    } catch (error) {
      console.error('Erro ao carregar subsistemas');
    }
  };

  const loadEmpresas = async () => {
    try {
      const response = await api.get('/empresas');
      setEmpresas(response.data.map((e: any) => e.nome));
    } catch (error) {
      console.error('Erro ao carregar empresas');
    }
  };

  const loadData = async () => {
    try {
      const response = await api.get('/coleta/consumo', {
        params: { dataPdp, subsistema, empresa },
      });
      if (response.data) {
        setConsumo(response.data);
      }
    } catch (error) {
      setConsumo({ previsto: 0, realizado: 0, diferenca: 0 });
    }
  };

  const handleSave = async () => {
    try {
      await api.post('/coleta/consumo', {
        dataPdp,
        subsistema,
        empresa,
        ...consumo,
      });
      setSnackbar({ open: true, message: 'Dados salvos com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar dados', severity: 'error' });
    }
  };

  const calculateDiferenca = () => {
    const dif = consumo.realizado - consumo.previsto;
    setConsumo({ ...consumo, diferenca: dif });
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Coleta de Consumo
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                label="Data PDP"
                type="date"
                value={dataPdp}
                onChange={(e) => setDataPdp(e.target.value)}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                select
                label="Subsistema"
                value={subsistema}
                onChange={(e) => setSubsistema(e.target.value)}
              >
                <MenuItem value="">Selecione</MenuItem>
                {subsistemas.map((s) => (
                  <MenuItem key={s} value={s}>
                    {s}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField
                fullWidth
                select
                label="Empresa"
                value={empresa}
                onChange={(e) => setEmpresa(e.target.value)}
              >
                <MenuItem value="">Selecione</MenuItem>
                {empresas.map((e) => (
                  <MenuItem key={e} value={e}>
                    {e}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {dataPdp && subsistema && empresa && (
        <>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
            <Typography variant="h6">Dados de Consumo</Typography>
            <Box>
              <Button variant="outlined" startIcon={<Refresh />} onClick={loadData} sx={{ mr: 1 }}>
                Recarregar
              </Button>
              <Button variant="contained" startIcon={<Save />} onClick={handleSave}>
                Salvar
              </Button>
            </Box>
          </Box>

          <Grid container spacing={3}>
            <Grid item xs={12} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="subtitle2" color="text.secondary">
                    Consumo Previsto (MWh)
                  </Typography>
                  <TextField
                    fullWidth
                    type="number"
                    value={consumo.previsto}
                    onChange={(e) => {
                      setConsumo({ ...consumo, previsto: parseFloat(e.target.value) || 0 });
                      calculateDiferenca();
                    }}
                    sx={{ mt: 1 }}
                  />
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="subtitle2" color="text.secondary">
                    Consumo Realizado (MWh)
                  </Typography>
                  <TextField
                    fullWidth
                    type="number"
                    value={consumo.realizado}
                    onChange={(e) => {
                      setConsumo({ ...consumo, realizado: parseFloat(e.target.value) || 0 });
                      calculateDiferenca();
                    }}
                    sx={{ mt: 1 }}
                  />
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="subtitle2" color="text.secondary">
                    Diferença (MWh)
                  </Typography>
                  <Typography variant="h5" sx={{ mt: 1 }} color={consumo.diferenca >= 0 ? 'success.main' : 'error.main'}>
                    {consumo.diferenca.toFixed(2)}
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
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

export default Consumption;

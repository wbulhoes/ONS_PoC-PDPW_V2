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
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { Save, Refresh, ContentCopy } from '@mui/icons-material';
import api from '../../../services/api';

interface LoadData {
  id: number;
  dataPdp: string;
  subsistema: string;
  empresa: string;
  intervalos: number[];
  total: number;
  media: number;
}

const Load: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [subsistema, setSubsistema] = useState('');
  const [empresa, setEmpresa] = useState('');
  const [subsistemas, setSubsistemas] = useState<string[]>([]);
  const [empresas, setEmpresas] = useState<string[]>([]);
  const [intervalos, setIntervalos] = useState<number[]>(Array(48).fill(0));
  const [openTextarea, setOpenTextarea] = useState(false);
  const [textareaValue, setTextareaValue] = useState('');
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
      const response = await api.get('/coleta/carga', {
        params: { dataPdp, subsistema, empresa },
      });
      if (response.data) {
        setIntervalos(response.data.intervalos || Array(48).fill(0));
      }
    } catch (error) {
      setIntervalos(Array(48).fill(0));
    }
  };

  const handleSave = async () => {
    try {
      await api.post('/coleta/carga', {
        dataPdp,
        subsistema,
        empresa,
        intervalos,
      });
      setSnackbar({ open: true, message: 'Dados salvos com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar dados', severity: 'error' });
    }
  };

  const handleIntervalChange = (index: number, value: string) => {
    const newIntervalos = [...intervalos];
    newIntervalos[index] = parseFloat(value) || 0;
    setIntervalos(newIntervalos);
  };

  const handleOpenTextarea = () => {
    setTextareaValue(intervalos.join('\n'));
    setOpenTextarea(true);
  };

  const handleApplyTextarea = () => {
    const values = textareaValue.split('\n').map((v) => parseFloat(v.trim()) || 0);
    if (values.length === 48) {
      setIntervalos(values);
      setOpenTextarea(false);
    } else {
      setSnackbar({ open: true, message: 'Devem ser informados 48 valores', severity: 'error' });
    }
  };

  const calculateTotal = () => intervalos.reduce((sum, val) => sum + val, 0);
  const calculateAverage = () => calculateTotal() / 48;

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Coleta de Carga
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
            <Typography variant="h6">Intervalos de Meia Hora (MW)</Typography>
            <Box>
              <Button variant="outlined" startIcon={<ContentCopy />} onClick={handleOpenTextarea} sx={{ mr: 1 }}>
                Edição em Bloco
              </Button>
              <Button variant="outlined" startIcon={<Refresh />} onClick={loadData} sx={{ mr: 1 }}>
                Recarregar
              </Button>
              <Button variant="contained" startIcon={<Save />} onClick={handleSave}>
                Salvar
              </Button>
            </Box>
          </Box>

          <TableContainer component={Paper}>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Intervalo</TableCell>
                  <TableCell>Hora</TableCell>
                  <TableCell align="right">Carga (MW)</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {intervalos.map((valor, index) => (
                  <TableRow key={index}>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>
                      {Math.floor(index / 2)
                        .toString()
                        .padStart(2, '0')}
                      :{index % 2 === 0 ? '00' : '30'}
                    </TableCell>
                    <TableCell align="right">
                      <TextField
                        type="number"
                        value={valor}
                        onChange={(e) => handleIntervalChange(index, e.target.value)}
                        size="small"
                        sx={{ width: 120 }}
                      />
                    </TableCell>
                  </TableRow>
                ))}
                <TableRow>
                  <TableCell colSpan={2}>
                    <strong>Total</strong>
                  </TableCell>
                  <TableCell align="right">
                    <strong>{calculateTotal().toFixed(2)} MW</strong>
                  </TableCell>
                </TableRow>
                <TableRow>
                  <TableCell colSpan={2}>
                    <strong>Média</strong>
                  </TableCell>
                  <TableCell align="right">
                    <strong>{calculateAverage().toFixed(2)} MW</strong>
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </>
      )}

      <Dialog open={openTextarea} onClose={() => setOpenTextarea(false)} maxWidth="sm" fullWidth>
        <DialogTitle>Edição em Bloco</DialogTitle>
        <DialogContent>
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Informe 48 valores (um por linha)
          </Typography>
          <TextField
            fullWidth
            multiline
            rows={20}
            value={textareaValue}
            onChange={(e) => setTextareaValue(e.target.value)}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenTextarea(false)}>Cancelar</Button>
          <Button onClick={handleApplyTextarea} variant="contained">
            Aplicar
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

export default Load;

import React, { useState } from 'react';
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
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';
import { Save, Send } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

const ProgramacaoDiaria: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [usina, setUsina] = useState('');
  const [intervalos, setIntervalos] = useState<number[]>(Array(48).fill(0));
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const handleSave = async () => {
    try {
      await apiClient.post('/programacao/diaria', { dataPdp, usina, intervalos });
      setSnackbar({ open: true, message: 'Programação diária salva', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>Programação Diária - Gerenciamento da Demanda</Typography>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField fullWidth label="Data PDP" type="date" value={dataPdp} onChange={(e) => setDataPdp(e.target.value)} InputLabelProps={{ shrink: true }} />
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField fullWidth select label="Usina" value={usina} onChange={(e) => setUsina(e.target.value)}>
                <MenuItem value="">Selecione</MenuItem>
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>Redução de Carga por Intervalo (MW)</Typography>
          <TableContainer component={Paper} sx={{ maxHeight: 400 }}>
            <Table stickyHeader size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Intervalo</TableCell>
                  <TableCell>Hora</TableCell>
                  <TableCell align="right">Redução (MW)</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {intervalos.map((valor, index) => (
                  <TableRow key={index}>
                    <TableCell>{index + 1}</TableCell>
                    <TableCell>{Math.floor(index / 2).toString().padStart(2, '0')}:{index % 2 === 0 ? '00' : '30'}</TableCell>
                    <TableCell align="right">
                      <TextField type="number" value={valor} onChange={(e) => {
                        const novos = [...intervalos];
                        novos[index] = parseFloat(e.target.value) || 0;
                        setIntervalos(novos);
                      }} size="small" sx={{ width: 100 }} />
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </CardContent>
      </Card>
      <Box sx={{ display: 'flex', gap: 2 }}>
        <Button variant="contained" startIcon={<Save />} onClick={handleSave}>Salvar</Button>
        <Button variant="contained" color="success" startIcon={<Send />}>Enviar para ONS</Button>
      </Box>
      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default ProgramacaoDiaria;

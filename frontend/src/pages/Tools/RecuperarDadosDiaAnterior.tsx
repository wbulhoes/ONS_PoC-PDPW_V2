import React, { useState } from 'react';
import { Box, Typography, Card, CardContent, TextField, MenuItem, Button, Alert, Snackbar, Grid } from '@mui/material';
import { Restore, Info } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

const RecuperarDadosDiaAnterior: React.FC = () => {
  const [dataOrigem, setDataOrigem] = useState('');
  const [dataDestino, setDataDestino] = useState('');
  const [empresa, setEmpresa] = useState('');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const handleRecuperar = async () => {
    try {
      await apiClient.post('/ferramentas/recuperar-dados', { dataOrigem, dataDestino, empresa });
      setSnackbar({ open: true, message: 'Dados recuperados com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao recuperar dados', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>Recuperar Dados do Dia Anterior</Typography>
      <Alert severity="warning" icon={<Info />} sx={{ mb: 3 }}>
        Esta função copia os dados do dia selecionado (origem) para outro dia (destino). Use com cuidado!
      </Alert>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={4}>
              <TextField fullWidth label="Data Origem" type="date" value={dataOrigem} onChange={(e) => setDataOrigem(e.target.value)} InputLabelProps={{ shrink: true }} />
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField fullWidth label="Data Destino" type="date" value={dataDestino} onChange={(e) => setDataDestino(e.target.value)} InputLabelProps={{ shrink: true }} />
            </Grid>
            <Grid item xs={12} md={4}>
              <TextField fullWidth select label="Empresa" value={empresa} onChange={(e) => setEmpresa(e.target.value)}>
                <MenuItem value="">Todas</MenuItem>
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
      <Button variant="contained" color="warning" startIcon={<Restore />} onClick={handleRecuperar} disabled={!dataOrigem || !dataDestino}>
        Recuperar Dados
      </Button>
      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default RecuperarDadosDiaAnterior;

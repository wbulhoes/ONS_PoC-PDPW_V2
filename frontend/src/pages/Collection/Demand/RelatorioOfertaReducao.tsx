import React, { useState } from 'react';
import { Box, Typography, Card, CardContent, TextField, MenuItem, Button, Alert, Snackbar, Grid } from '@mui/material';
import { FileDownload, Print } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

const RelatorioOfertaReducao: React.FC = () => {
  const [semanaPMO, setSemanaPMO] = useState('');
  const [empresa, setEmpresa] = useState('');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const handleGerar = async () => {
    try {
      const blob = await apiClient.get<Blob>(`/relatorios/oferta-reducao?semana=${semanaPMO}&empresa=${empresa}`);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `Relatorio_Oferta_Reducao_${semanaPMO}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      setSnackbar({ open: true, message: 'Relatório gerado com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao gerar relatório', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>Relatório de Oferta da Redução Semanal</Typography>
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>Parâmetros do Relatório</Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField fullWidth select label="Semana PMO" value={semanaPMO} onChange={(e) => setSemanaPMO(e.target.value)}>
                <MenuItem value="">Selecione</MenuItem>
              </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField fullWidth select label="Empresa" value={empresa} onChange={(e) => setEmpresa(e.target.value)}>
                <MenuItem value="">Todas</MenuItem>
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
      <Alert severity="info" sx={{ mb: 3 }}>
        Este relatório apresenta as ofertas de redução de carga para a semana PMO selecionada
      </Alert>
      <Box sx={{ display: 'flex', gap: 2 }}>
        <Button variant="contained" startIcon={<FileDownload />} onClick={handleGerar} disabled={!semanaPMO}>Gerar PDF</Button>
        <Button variant="outlined" startIcon={<Print />} disabled={!semanaPMO}>Imprimir</Button>
      </Box>
      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default RelatorioOfertaReducao;

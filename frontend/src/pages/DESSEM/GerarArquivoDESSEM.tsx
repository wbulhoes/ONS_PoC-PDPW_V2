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
  FormControlLabel,
  Checkbox,
  LinearProgress,
} from '@mui/material';
import { Build, FileDownload } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

const GerarArquivoDESSEM: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [tiposArquivo, setTiposArquivo] = useState({
    dessem: true,
    hidr: true,
    term: true,
    rede: false,
    cortfcf: false,
  });
  const [generating, setGenerating] = useState(false);
  const [progress, setProgress] = useState(0);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const handleGenerate = async () => {
    if (!dataPdp) {
      setSnackbar({ open: true, message: 'Selecione a data PDP', severity: 'error' });
      return;
    }

    setGenerating(true);
    setProgress(0);

    try {
      const selectedTypes = Object.entries(tiposArquivo)
        .filter(([_, selected]) => selected)
        .map(([type]) => type);

      const response = await apiClient.post(
        '/dessem/gerar-arquivos',
        { dataPdp, tiposArquivo: selectedTypes },
        {
          onUploadProgress: (progressEvent) => {
            const percentCompleted = Math.round((progressEvent.loaded * 100) / (progressEvent.total || 1));
            setProgress(percentCompleted);
          },
        }
      );

      setSnackbar({ open: true, message: 'Arquivos DESSEM gerados com sucesso', severity: 'success' });
    } catch (error: any) {
      setSnackbar({ open: true, message: 'Erro ao gerar arquivos DESSEM', severity: 'error' });
    } finally {
      setGenerating(false);
      setProgress(0);
    }
  };

  const handleDownloadAll = async () => {
    try {
      const blob = await apiClient.get<Blob>(`/dessem/download-lote?data=${dataPdp}`, { responseType: 'blob' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `DESSEM_${dataPdp}.zip`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      setSnackbar({ open: true, message: 'Arquivos baixados com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao baixar arquivos', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Gerar Arquivos DESSEM
      </Typography>

      <Alert severity="info" sx={{ mb: 3 }}>
        Gere os arquivos de entrada do modelo DESSEM a partir dos dados da programação diária
      </Alert>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Parâmetros de Geração
          </Typography>

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

          <Typography variant="subtitle1" sx={{ mt: 3, mb: 1 }}>
            Selecione os arquivos a serem gerados:
          </Typography>

          <Grid container spacing={1}>
            <Grid item xs={12} md={6}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={tiposArquivo.dessem}
                    onChange={(e) => setTiposArquivo({ ...tiposArquivo, dessem: e.target.checked })}
                  />
                }
                label="DESSEM.DAT (Arquivo principal)"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={tiposArquivo.hidr}
                    onChange={(e) => setTiposArquivo({ ...tiposArquivo, hidr: e.target.checked })}
                  />
                }
                label="HIDR.DAT (Dados hidráulicos)"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={tiposArquivo.term}
                    onChange={(e) => setTiposArquivo({ ...tiposArquivo, term: e.target.checked })}
                  />
                }
                label="TERM.DAT (Dados térmicos)"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={tiposArquivo.rede}
                    onChange={(e) => setTiposArquivo({ ...tiposArquivo, rede: e.target.checked })}
                  />
                }
                label="REDE.DAT (Dados de rede)"
              />
            </Grid>
            <Grid item xs={12} md={6}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={tiposArquivo.cortfcf}
                    onChange={(e) => setTiposArquivo({ ...tiposArquivo, cortfcf: e.target.checked })}
                  />
                }
                label="CORTFCF.DAT (Cortes FCF)"
              />
            </Grid>
          </Grid>

          {generating && (
            <Box sx={{ mt: 3 }}>
              <LinearProgress variant="determinate" value={progress} />
              <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                Gerando arquivos... {progress}%
              </Typography>
            </Box>
          )}
        </CardContent>
      </Card>

      <Box sx={{ display: 'flex', gap: 2 }}>
        <Button
          variant="contained"
          startIcon={<Build />}
          onClick={handleGenerate}
          disabled={!dataPdp || generating}
        >
          {generating ? 'Gerando...' : 'Gerar Arquivos'}
        </Button>
        <Button
          variant="outlined"
          startIcon={<FileDownload />}
          onClick={handleDownloadAll}
          disabled={!dataPdp || generating}
        >
          Download em Lote (ZIP)
        </Button>
      </Box>

      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default GerarArquivoDESSEM;

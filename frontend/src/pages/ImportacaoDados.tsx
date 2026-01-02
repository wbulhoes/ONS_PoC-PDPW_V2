import React, { useState } from 'react';
import {
  Box,
  Button,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  LinearProgress,
  Alert,
  Snackbar,
  List,
  ListItem,
  ListItemText,
  ListItemIcon,
  Divider,
  MenuItem,
  TextField,
} from '@mui/material';
import { CloudUpload, CheckCircle, Error as ErrorIcon, Description } from '@mui/icons-material';
import { apiClient } from '../services/apiClient';

interface ImportResult {
  success: boolean;
  message: string;
  recordsImported: number;
  errors: string[];
}

const ImportacaoDados: React.FC = () => {
  const [selectedFile, setSelectedFile] = useState<File | null>(null);
  const [tipoImportacao, setTipoImportacao] = useState('');
  const [uploading, setUploading] = useState(false);
  const [progress, setProgress] = useState(0);
  const [importResult, setImportResult] = useState<ImportResult | null>(null);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const tiposImportacao = [
    { value: 'usinas', label: 'Usinas' },
    { value: 'unidades-geradoras', label: 'Unidades Geradoras' },
    { value: 'subsistemas', label: 'Subsistemas' },
    { value: 'submercados', label: 'Submercados' },
    { value: 'reservatorios', label: 'Reservatórios' },
    { value: 'interligacoes', label: 'Interligações' },
    { value: 'limites-intercambio', label: 'Limites de Intercâmbio' },
  ];

  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      setSelectedFile(event.target.files[0]);
      setImportResult(null);
    }
  };

  const handleImport = async () => {
    if (!selectedFile || !tipoImportacao) {
      setSnackbar({ open: true, message: 'Selecione um arquivo e o tipo de importação', severity: 'error' });
      return;
    }

    setUploading(true);
    setProgress(0);
    setImportResult(null);

    const formData = new FormData();
    formData.append('file', selectedFile);
    formData.append('tipo', tipoImportacao);

    try {
      // Simular progresso (fetch/apiClient não suporta onUploadProgress nativamente)
      const interval = setInterval(() => {
        setProgress((prev) => Math.min(prev + 10, 90));
      }, 200);

      const response = await apiClient.post<ImportResult>('/importacao/upload', formData);
      
      clearInterval(interval);
      setProgress(100);

      setImportResult(response);
      setSnackbar({ open: true, message: 'Importação concluída com sucesso', severity: 'success' });
      setSelectedFile(null);
      setTipoImportacao('');
    } catch (error: any) {
      const errorMessage = error.message || 'Erro ao importar dados';
      setSnackbar({ open: true, message: errorMessage, severity: 'error' });
      setImportResult({
        success: false,
        message: errorMessage,
        recordsImported: 0,
        errors: [errorMessage],
      });
    } finally {
      setUploading(false);
      setProgress(0);
    }
  };

  const handleDownloadTemplate = async () => {
    if (!tipoImportacao) {
      setSnackbar({ open: true, message: 'Selecione o tipo de importação', severity: 'error' });
      return;
    }

    try {
      const blob = await apiClient.get<Blob>(`/importacao/template/${tipoImportacao}`);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `template_${tipoImportacao}.csv`);
      document.body.appendChild(link);
      link.click();
      link.remove();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao baixar template', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Importação de Dados
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Selecionar Arquivo
              </Typography>

              <TextField
                fullWidth
                select
                label="Tipo de Importação"
                value={tipoImportacao}
                onChange={(e) => setTipoImportacao(e.target.value)}
                sx={{ mb: 2 }}
              >
                {tiposImportacao.map((tipo) => (
                  <MenuItem key={tipo.value} value={tipo.value}>
                    {tipo.label}
                  </MenuItem>
                ))}
              </TextField>

              <Button
                variant="outlined"
                onClick={handleDownloadTemplate}
                disabled={!tipoImportacao}
                sx={{ mb: 2, mr: 2 }}
              >
                Baixar Template
              </Button>

              <Box sx={{ mb: 2 }}>
                <input
                  accept=".csv,.xlsx,.xls"
                  style={{ display: 'none' }}
                  id="file-upload"
                  type="file"
                  onChange={handleFileSelect}
                />
                <label htmlFor="file-upload">
                  <Button variant="contained" component="span" startIcon={<CloudUpload />}>
                    Selecionar Arquivo
                  </Button>
                </label>
              </Box>

              {selectedFile && (
                <Alert severity="info" sx={{ mb: 2 }}>
                  Arquivo selecionado: {selectedFile.name}
                </Alert>
              )}

              {uploading && (
                <Box sx={{ mb: 2 }}>
                  <LinearProgress variant="determinate" value={progress} />
                  <Typography variant="body2" sx={{ mt: 1 }}>
                    Progresso: {progress}%
                  </Typography>
                </Box>
              )}

              <Button
                variant="contained"
                color="primary"
                onClick={handleImport}
                disabled={!selectedFile || !tipoImportacao || uploading}
                fullWidth
              >
                Importar Dados
              </Button>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Instruções
              </Typography>

              <List>
                <ListItem>
                  <ListItemIcon>
                    <Description />
                  </ListItemIcon>
                  <ListItemText
                    primary="1. Selecione o tipo de importação"
                    secondary="Escolha qual tipo de dado você deseja importar"
                  />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemIcon>
                    <Description />
                  </ListItemIcon>
                  <ListItemText
                    primary="2. Baixe o template"
                    secondary="Use o template para garantir o formato correto"
                  />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemIcon>
                    <Description />
                  </ListItemIcon>
                  <ListItemText
                    primary="3. Preencha os dados"
                    secondary="Complete o arquivo com os dados a serem importados"
                  />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemIcon>
                    <CloudUpload />
                  </ListItemIcon>
                  <ListItemText
                    primary="4. Faça o upload"
                    secondary="Selecione o arquivo preenchido e clique em Importar"
                  />
                </ListItem>
              </List>
            </CardContent>
          </Card>
        </Grid>

        {importResult && (
          <Grid item xs={12}>
            <Card>
              <CardContent>
                <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                  {importResult.success ? (
                    <CheckCircle color="success" sx={{ mr: 1 }} />
                  ) : (
                    <ErrorIcon color="error" sx={{ mr: 1 }} />
                  )}
                  <Typography variant="h6">Resultado da Importação</Typography>
                </Box>

                <Alert severity={importResult.success ? 'success' : 'error'} sx={{ mb: 2 }}>
                  {importResult.message}
                </Alert>

                {importResult.success && (
                  <Typography variant="body1">
                    Registros importados: {importResult.recordsImported}
                  </Typography>
                )}

                {importResult.errors && importResult.errors.length > 0 && (
                  <Box sx={{ mt: 2 }}>
                    <Typography variant="subtitle1" color="error">
                      Erros encontrados:
                    </Typography>
                    <List>
                      {importResult.errors.map((error, index) => (
                        <ListItem key={index}>
                          <ListItemText primary={error} />
                        </ListItem>
                      ))}
                    </List>
                  </Box>
                )}
              </CardContent>
            </Card>
          </Grid>
        )}
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

export default ImportacaoDados;

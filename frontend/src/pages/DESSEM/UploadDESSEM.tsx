import React, { useState } from 'react';
import {
  Box,
  Typography,
  Paper,
  Card,
  CardContent,
  Button,
  Alert,
  Snackbar,
  LinearProgress,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
  Chip,
} from '@mui/material';
import { CloudUpload, Delete, CheckCircle } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

interface UploadedDESSEMFile {
  id: string;
  nome: string;
  tamanho: number;
  dataUpload: string;
  status: 'pendente' | 'validado' | 'erro';
  mensagem?: string;
}

const UploadDESSEM: React.FC = () => {
  const [files, setFiles] = useState<File[]>([]);
  const [uploadedFiles, setUploadedFiles] = useState<UploadedDESSEMFile[]>([]);
  const [uploading, setUploading] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const selectedFiles = Array.from(event.target.files).filter(
        (file) => file.name.endsWith('.dat') || file.name.endsWith('.txt')
      );
      setFiles([...files, ...selectedFiles]);
    }
  };

  const handleUpload = async () => {
    if (files.length === 0) {
      setSnackbar({ open: true, message: 'Selecione arquivos DESSEM (.dat ou .txt)', severity: 'error' });
      return;
    }

    setUploading(true);
    setUploadProgress(0);

    try {
      const formData = new FormData();
      files.forEach((file) => formData.append('files', file));

      const response = await apiClient.post<UploadedDESSEMFile[]>('/dessem/upload', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
        onUploadProgress: (progressEvent) => {
          const percentCompleted = Math.round((progressEvent.loaded * 100) / (progressEvent.total || 1));
          setUploadProgress(percentCompleted);
        },
      });

      setUploadedFiles([...uploadedFiles, ...response]);
      setFiles([]);
      setSnackbar({ open: true, message: `${response.length} arquivo(s) DESSEM enviado(s)`, severity: 'success' });
    } catch (error: any) {
      setSnackbar({ open: true, message: 'Erro ao fazer upload dos arquivos DESSEM', severity: 'error' });
    } finally {
      setUploading(false);
      setUploadProgress(0);
    }
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  };

  const getStatusChip = (status: UploadedDESSEMFile['status']) => {
    const config = {
      pendente: { label: 'Pendente', color: 'default' as const },
      validado: { label: 'Validado', color: 'success' as const },
      erro: { label: 'Erro', color: 'error' as const },
    };
    return <Chip label={config[status].label} color={config[status].color} size="small" />;
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Upload de Arquivos DESSEM
      </Typography>

      <Alert severity="info" sx={{ mb: 3 }}>
        Faça upload de arquivos DESSEM (.dat ou .txt). Os arquivos serão validados automaticamente.
      </Alert>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
            <Button variant="outlined" component="label" startIcon={<CloudUpload />}>
              Escolher Arquivos DESSEM
              <input type="file" hidden multiple accept=".dat,.txt" onChange={handleFileSelect} />
            </Button>
            <Button
              variant="contained"
              onClick={handleUpload}
              disabled={files.length === 0 || uploading}
              startIcon={<CloudUpload />}
            >
              {uploading ? 'Enviando...' : 'Enviar para Validação'}
            </Button>
          </Box>

          {uploading && (
            <Box sx={{ mb: 2 }}>
              <LinearProgress variant="determinate" value={uploadProgress} />
              <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
                {uploadProgress}% enviado
              </Typography>
            </Box>
          )}

          {files.length > 0 && (
            <Paper sx={{ mt: 2 }}>
              <TableContainer>
                <Table size="small">
                  <TableHead>
                    <TableRow>
                      <TableCell>Arquivo</TableCell>
                      <TableCell>Tamanho</TableCell>
                      <TableCell align="right">Ações</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {files.map((file, index) => (
                      <TableRow key={index}>
                        <TableCell>{file.name}</TableCell>
                        <TableCell>{formatFileSize(file.size)}</TableCell>
                        <TableCell align="right">
                          <IconButton
                            size="small"
                            onClick={() => setFiles(files.filter((_, i) => i !== index))}
                            color="error"
                          >
                            <Delete />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </Paper>
          )}
        </CardContent>
      </Card>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Arquivos DESSEM Enviados
          </Typography>
          {uploadedFiles.length === 0 ? (
            <Alert severity="info">Nenhum arquivo DESSEM enviado ainda</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Nome</TableCell>
                    <TableCell>Tamanho</TableCell>
                    <TableCell>Data Upload</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Mensagem</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {uploadedFiles.map((file) => (
                    <TableRow key={file.id}>
                      <TableCell>{file.nome}</TableCell>
                      <TableCell>{formatFileSize(file.tamanho)}</TableCell>
                      <TableCell>{new Date(file.dataUpload).toLocaleString('pt-BR')}</TableCell>
                      <TableCell>{getStatusChip(file.status)}</TableCell>
                      <TableCell>{file.mensagem || '-'}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </CardContent>
      </Card>

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

export default UploadDESSEM;

import React, { useState } from 'react';
import {
  Box,
  Typography,
  Paper,
  Card,
  CardContent,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Alert,
  Snackbar,
  LinearProgress,
  Chip,
  IconButton,
  Tooltip,
} from '@mui/material';
import { CloudUpload, Delete, CheckCircle, Error as ErrorIcon, Visibility } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface UploadedFile {
  id: string;
  nome: string;
  tamanho: number;
  tipo: string;
  dataUpload: string;
  status: 'pendente' | 'processando' | 'sucesso' | 'erro';
  mensagem?: string;
}

const UploadArquivos: React.FC = () => {
  const [files, setFiles] = useState<File[]>([]);
  const [uploadedFiles, setUploadedFiles] = useState<UploadedFile[]>([]);
  const [uploading, setUploading] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error' | 'info',
  });

  const handleFileSelect = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files) {
      const selectedFiles = Array.from(event.target.files);
      setFiles([...files, ...selectedFiles]);
    }
  };

  const handleRemoveFile = (index: number) => {
    const newFiles = files.filter((_, i) => i !== index);
    setFiles(newFiles);
  };

  const handleUpload = async () => {
    if (files.length === 0) {
      setSnackbar({
        open: true,
        message: 'Selecione pelo menos um arquivo para fazer upload',
        severity: 'error',
      });
      return;
    }

    setUploading(true);
    setUploadProgress(0);

    try {
      const formData = new FormData();
      files.forEach((file) => {
        formData.append('files', file);
      });

      const response = await apiClient.post<UploadedFile[]>('/ferramentas/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
        onUploadProgress: (progressEvent) => {
          const percentCompleted = Math.round((progressEvent.loaded * 100) / (progressEvent.total || 1));
          setUploadProgress(percentCompleted);
        },
      });

      setUploadedFiles([...uploadedFiles, ...response]);
      setFiles([]);
      setSnackbar({
        open: true,
        message: `${response.length} arquivo(s) enviado(s) com sucesso`,
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.message || 'Erro ao fazer upload dos arquivos',
        severity: 'error',
      });
    } finally {
      setUploading(false);
      setUploadProgress(0);
    }
  };

  const handleDeleteUploaded = async (fileId: string) => {
    try {
      await apiClient.delete(`/ferramentas/upload/${fileId}`);
      setUploadedFiles(uploadedFiles.filter((f) => f.id !== fileId));
      setSnackbar({
        open: true,
        message: 'Arquivo removido com sucesso',
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao remover arquivo',
        severity: 'error',
      });
    }
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  const getStatusChip = (status: UploadedFile['status']) => {
    const statusConfig = {
      pendente: { label: 'Pendente', color: 'default' as const },
      processando: { label: 'Processando', color: 'info' as const },
      sucesso: { label: 'Sucesso', color: 'success' as const },
      erro: { label: 'Erro', color: 'error' as const },
    };

    const config = statusConfig[status];
    return <Chip label={config.label} color={config.color} size="small" />;
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Upload de Arquivos
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Selecionar Arquivos
          </Typography>

          <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
            <Button variant="outlined" component="label" startIcon={<CloudUpload />}>
              Escolher Arquivos
              <input type="file" hidden multiple onChange={handleFileSelect} />
            </Button>
            <Button
              variant="contained"
              onClick={handleUpload}
              disabled={files.length === 0 || uploading}
              startIcon={<CloudUpload />}
            >
              {uploading ? 'Enviando...' : 'Enviar Arquivos'}
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
                      <TableCell>Nome do Arquivo</TableCell>
                      <TableCell>Tamanho</TableCell>
                      <TableCell>Tipo</TableCell>
                      <TableCell align="right">Ações</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {files.map((file, index) => (
                      <TableRow key={index}>
                        <TableCell>{file.name}</TableCell>
                        <TableCell>{formatFileSize(file.size)}</TableCell>
                        <TableCell>{file.type || 'Desconhecido'}</TableCell>
                        <TableCell align="right">
                          <IconButton size="small" onClick={() => handleRemoveFile(index)} color="error">
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
            Arquivos Enviados
          </Typography>

          {uploadedFiles.length === 0 ? (
            <Alert severity="info">Nenhum arquivo enviado ainda</Alert>
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
                    <TableCell align="right">Ações</TableCell>
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
                      <TableCell align="right">
                        <Tooltip title="Visualizar">
                          <IconButton size="small" color="primary">
                            <Visibility />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Remover">
                          <IconButton
                            size="small"
                            color="error"
                            onClick={() => handleDeleteUploaded(file.id)}
                          >
                            <Delete />
                          </IconButton>
                        </Tooltip>
                      </TableCell>
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

export default UploadArquivos;

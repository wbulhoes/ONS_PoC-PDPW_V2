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
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  IconButton,
  Chip,
} from '@mui/material';
import { FileDownload, Visibility } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface ArquivoDESSEM {
  id: string;
  nome: string;
  tipo: string;
  tamanho: number;
  dataCriacao: string;
  status: 'disponivel' | 'processando';
}

const DownloadDESSEM: React.FC = () => {
  const [arquivos, setArquivos] = useState<ArquivoDESSEM[]>([]);
  const [filtroTipo, setFiltroTipo] = useState('');
  const [loading, setLoading] = useState(false);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const tiposArquivo = ['Todos', 'DESSEM.DAT', 'HIDR.DAT', 'TERM.DAT', 'REDE.DAT', 'OUTROS'];

  useEffect(() => {
    loadArquivos();
  }, []);

  const loadArquivos = async () => {
    setLoading(true);
    try {
      const data = await apiClient.get<ArquivoDESSEM[]>('/dessem/arquivos-disponiveis');
      setArquivos(data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar arquivos DESSEM', severity: 'error' });
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = async (arquivo: ArquivoDESSEM) => {
    try {
      const blob = await apiClient.get<Blob>(`/dessem/download/${arquivo.id}`, { responseType: 'blob' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', arquivo.nome);
      document.body.appendChild(link);
      link.click();
      link.remove();
      setSnackbar({ open: true, message: `${arquivo.nome} baixado com sucesso`, severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao fazer download', severity: 'error' });
    }
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  };

  const arquivosFiltrados = arquivos.filter(
    (a) => filtroTipo === '' || filtroTipo === 'Todos' || a.tipo === filtroTipo
  );

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Download de Arquivos DESSEM
      </Typography>

      <Alert severity="info" sx={{ mb: 3 }}>
        Faça download dos arquivos DESSEM gerados pelo sistema
      </Alert>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <TextField
            fullWidth
            select
            label="Tipo de Arquivo"
            value={filtroTipo}
            onChange={(e) => setFiltroTipo(e.target.value)}
          >
            {tiposArquivo.map((tipo) => (
              <MenuItem key={tipo} value={tipo}>
                {tipo}
              </MenuItem>
            ))}
          </TextField>
        </CardContent>
      </Card>

      <Card>
        <CardContent>
          {loading ? (
            <Alert severity="info">Carregando arquivos DESSEM...</Alert>
          ) : arquivosFiltrados.length === 0 ? (
            <Alert severity="warning">Nenhum arquivo DESSEM disponível</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Nome do Arquivo</TableCell>
                    <TableCell>Tipo</TableCell>
                    <TableCell>Tamanho</TableCell>
                    <TableCell>Data Criação</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell align="right">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {arquivosFiltrados.map((arquivo) => (
                    <TableRow key={arquivo.id}>
                      <TableCell>{arquivo.nome}</TableCell>
                      <TableCell>{arquivo.tipo}</TableCell>
                      <TableCell>{formatFileSize(arquivo.tamanho)}</TableCell>
                      <TableCell>{new Date(arquivo.dataCriacao).toLocaleString('pt-BR')}</TableCell>
                      <TableCell>
                        <Chip
                          label={arquivo.status === 'disponivel' ? 'Disponível' : 'Processando'}
                          color={arquivo.status === 'disponivel' ? 'success' : 'info'}
                          size="small"
                        />
                      </TableCell>
                      <TableCell align="right">
                        <IconButton
                          size="small"
                          color="success"
                          onClick={() => handleDownload(arquivo)}
                          disabled={arquivo.status !== 'disponivel'}
                        >
                          <FileDownload />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </CardContent>
      </Card>

      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default DownloadDESSEM;

import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
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
  Chip,
  IconButton,
  Tooltip,
} from '@mui/material';
import { Search, FileDownload, Visibility, Refresh } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface ArquivoDisponivel {
  id: string;
  nome: string;
  descricao: string;
  tipo: string;
  tamanho: number;
  dataCriacao: string;
  categoria: string;
  status: 'disponivel' | 'processando' | 'indisponivel';
}

const DownloadArquivos: React.FC = () => {
  const [arquivos, setArquivos] = useState<ArquivoDisponivel[]>([]);
  const [filtroCategoria, setFiltroCategoria] = useState('');
  const [filtroNome, setFiltroNome] = useState('');
  const [loading, setLoading] = useState(false);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  const categorias = [
    'Todos',
    'DESSEM',
    'PMO',
    'Programação',
    'Relatórios',
    'Modelos',
    'Outros',
  ];

  useEffect(() => {
    loadArquivos();
  }, []);

  const loadArquivos = async () => {
    setLoading(true);
    try {
      const data = await apiClient.get<ArquivoDisponivel[]>('/ferramentas/arquivos-disponiveis');
      setArquivos(data);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao carregar arquivos disponíveis',
        severity: 'error',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleDownload = async (arquivo: ArquivoDisponivel) => {
    try {
      const blob = await apiClient.get<Blob>(`/ferramentas/download/${arquivo.id}`, {
        responseType: 'blob',
      });

      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', arquivo.nome);
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSnackbar({
        open: true,
        message: `Arquivo ${arquivo.nome} baixado com sucesso`,
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao fazer download do arquivo',
        severity: 'error',
      });
    }
  };

  const handleVisualizar = async (arquivo: ArquivoDisponivel) => {
    try {
      const blob = await apiClient.get<Blob>(`/ferramentas/visualizar/${arquivo.id}`, {
        responseType: 'blob',
      });

      const url = window.URL.createObjectURL(blob);
      window.open(url, '_blank');
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao visualizar arquivo',
        severity: 'error',
      });
    }
  };

  const formatFileSize = (bytes: number): string => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round((bytes / Math.pow(k, i)) * 100) / 100 + ' ' + sizes[i];
  };

  const getStatusChip = (status: ArquivoDisponivel['status']) => {
    const statusConfig = {
      disponivel: { label: 'Disponível', color: 'success' as const },
      processando: { label: 'Processando', color: 'info' as const },
      indisponivel: { label: 'Indisponível', color: 'error' as const },
    };

    const config = statusConfig[status];
    return <Chip label={config.label} color={config.color} size="small" />;
  };

  const arquivosFiltrados = arquivos.filter((arquivo) => {
    const matchCategoria =
      filtroCategoria === '' || filtroCategoria === 'Todos' || arquivo.categoria === filtroCategoria;
    const matchNome = filtroNome === '' || arquivo.nome.toLowerCase().includes(filtroNome.toLowerCase());
    return matchCategoria && matchNome;
  });

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Download de Arquivos
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Filtros
          </Typography>

          <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
            <TextField
              fullWidth
              label="Nome do Arquivo"
              value={filtroNome}
              onChange={(e) => setFiltroNome(e.target.value)}
              placeholder="Digite para buscar..."
            />

            <TextField
              fullWidth
              select
              label="Categoria"
              value={filtroCategoria}
              onChange={(e) => setFiltroCategoria(e.target.value)}
            >
              {categorias.map((cat) => (
                <MenuItem key={cat} value={cat}>
                  {cat}
                </MenuItem>
              ))}
            </TextField>

            <Button
              variant="outlined"
              startIcon={<Refresh />}
              onClick={loadArquivos}
              disabled={loading}
            >
              Atualizar
            </Button>
          </Box>

          <Alert severity="info">
            {arquivosFiltrados.length} arquivo(s) disponível(is) para download
          </Alert>
        </CardContent>
      </Card>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Arquivos Disponíveis
          </Typography>

          {loading ? (
            <Alert severity="info">Carregando arquivos...</Alert>
          ) : arquivosFiltrados.length === 0 ? (
            <Alert severity="warning">Nenhum arquivo disponível com os filtros selecionados</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Nome</TableCell>
                    <TableCell>Descrição</TableCell>
                    <TableCell>Categoria</TableCell>
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
                      <TableCell>{arquivo.descricao}</TableCell>
                      <TableCell>{arquivo.categoria}</TableCell>
                      <TableCell>{formatFileSize(arquivo.tamanho)}</TableCell>
                      <TableCell>{new Date(arquivo.dataCriacao).toLocaleString('pt-BR')}</TableCell>
                      <TableCell>{getStatusChip(arquivo.status)}</TableCell>
                      <TableCell align="right">
                        <Tooltip title="Visualizar">
                          <IconButton
                            size="small"
                            color="primary"
                            onClick={() => handleVisualizar(arquivo)}
                            disabled={arquivo.status !== 'disponivel'}
                          >
                            <Visibility />
                          </IconButton>
                        </Tooltip>
                        <Tooltip title="Download">
                          <IconButton
                            size="small"
                            color="success"
                            onClick={() => handleDownload(arquivo)}
                            disabled={arquivo.status !== 'disponivel'}
                          >
                            <FileDownload />
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

export default DownloadArquivos;

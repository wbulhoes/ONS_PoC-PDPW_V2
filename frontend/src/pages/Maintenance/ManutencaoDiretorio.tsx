import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Card,
  CardContent,
  TextField,
  Button,
  Alert,
  Snackbar,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  IconButton,
} from '@mui/material';
import { Folder, Delete, Add } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface DiretorioTemp {
  id: string;
  caminho: string;
  tamanho: string;
  dataUltimaModificacao: string;
}

const ManutencaoDiretorio: React.FC = () => {
  const [diretorios, setDiretorios] = useState<DiretorioTemp[]>([]);
  const [novoDiretorio, setNovoDiretorio] = useState('');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadDiretorios();
  }, []);

  const loadDiretorios = async () => {
    try {
      const data = await apiClient.get<DiretorioTemp[]>('/manutencao/diretorios-temporarios');
      setDiretorios(data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar diretórios', severity: 'error' });
    }
  };

  const handleAdd = async () => {
    if (!novoDiretorio) return;
    try {
      await apiClient.post('/manutencao/diretorios-temporarios', { caminho: novoDiretorio });
      setSnackbar({ open: true, message: 'Diretório adicionado', severity: 'success' });
      setNovoDiretorio('');
      loadDiretorios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao adicionar diretório', severity: 'error' });
    }
  };

  const handleDelete = async (id: string) => {
    if (!window.confirm('Confirma a exclusão deste diretório temporário?')) return;
    try {
      await apiClient.delete(`/manutencao/diretorios-temporarios/${id}`);
      setSnackbar({ open: true, message: 'Diretório removido', severity: 'success' });
      loadDiretorios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao remover diretório', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Manutenção de Diretório Temporário
      </Typography>

      <Alert severity="warning" sx={{ mb: 3 }}>
        <strong>Atenção:</strong> Esta funcionalidade é apenas para administradores. Alterações podem afetar o funcionamento do sistema.
      </Alert>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Adicionar Diretório Temporário
          </Typography>
          <Box sx={{ display: 'flex', gap: 2 }}>
            <TextField
              fullWidth
              label="Caminho do Diretório"
              value={novoDiretorio}
              onChange={(e) => setNovoDiretorio(e.target.value)}
              placeholder="C:\Temp\PDPw\"
            />
            <Button variant="contained" startIcon={<Add />} onClick={handleAdd}>
              Adicionar
            </Button>
          </Box>
        </CardContent>
      </Card>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Diretórios Temporários Configurados
          </Typography>
          {diretorios.length === 0 ? (
            <Alert severity="info">Nenhum diretório configurado</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Caminho</TableCell>
                    <TableCell>Tamanho</TableCell>
                    <TableCell>Última Modificação</TableCell>
                    <TableCell align="right">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {diretorios.map((dir) => (
                    <TableRow key={dir.id}>
                      <TableCell>
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                          <Folder color="primary" />
                          {dir.caminho}
                        </Box>
                      '</TableCell>
                      <TableCell>{dir.tamanho}</TableCell>
                      <TableCell>{new Date(dir.dataUltimaModificacao).toLocaleString('pt-BR')}</TableCell>
                      <TableCell align="right">
                        <IconButton size="small" color="error" onClick={() => handleDelete(dir.id)}>
                          <Delete />
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

export default ManutencaoDiretorio;

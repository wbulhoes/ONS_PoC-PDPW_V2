import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Card,
  CardContent,
  TextField,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Alert,
  Snackbar,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Chip,
  IconButton,
  Tooltip,
} from '@mui/material';
import { Search, Visibility, Print, Refresh } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface Recibo {
  id: string;
  numeroRecibo: string;
  dataPdp: string;
  empresa: string;
  dataEnvio: string;
  horaEnvio: string;
  usuario: string;
  tipoEnvio: string;
  quantidadeRegistros: number;
  status: 'sucesso' | 'parcial' | 'erro';
  detalhes?: string;
}

const VisualizacaoRecibos: React.FC = () => {
  const [recibos, setRecibos] = useState<Recibo[]>([]);
  const [filtroData, setFiltroData] = useState('');
  const [filtroEmpresa, setFiltroEmpresa] = useState('');
  const [loading, setLoading] = useState(false);
  const [selectedRecibo, setSelectedRecibo] = useState<Recibo | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  useEffect(() => {
    loadRecibos();
  }, []);

  const loadRecibos = async () => {
    setLoading(true);
    try {
      const params = new URLSearchParams();
      if (filtroData) params.append('data', filtroData);
      if (filtroEmpresa) params.append('empresa', filtroEmpresa);

      const data = await apiClient.get<Recibo[]>(`/ferramentas/recibos?${params.toString()}`);
      setRecibos(data);
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao carregar recibos',
        severity: 'error',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleVisualizar = (recibo: Recibo) => {
    setSelectedRecibo(recibo);
    setDialogOpen(true);
  };

  const handleImprimir = () => {
    if (selectedRecibo) {
      window.print();
    }
  };

  const getStatusChip = (status: Recibo['status']) => {
    const statusConfig = {
      sucesso: { label: 'Sucesso', color: 'success' as const },
      parcial: { label: 'Parcial', color: 'warning' as const },
      erro: { label: 'Erro', color: 'error' as const },
    };

    const config = statusConfig[status];
    return <Chip label={config.label} color={config.color} size="small" />;
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Visualização de Recibos
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Filtros
          </Typography>

          <Box sx={{ display: 'flex', gap: 2, mb: 2 }}>
            <TextField
              fullWidth
              label="Data PDP"
              type="date"
              value={filtroData}
              onChange={(e) => setFiltroData(e.target.value)}
              InputLabelProps={{ shrink: true }}
            />

            <TextField
              fullWidth
              label="Empresa"
              value={filtroEmpresa}
              onChange={(e) => setFiltroEmpresa(e.target.value)}
              placeholder="Digite o nome da empresa..."
            />

            <Button variant="contained" startIcon={<Search />} onClick={loadRecibos} disabled={loading}>
              Buscar
            </Button>

            <Button variant="outlined" startIcon={<Refresh />} onClick={loadRecibos} disabled={loading}>
              Atualizar
            </Button>
          </Box>
        </CardContent>
      </Card>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Recibos Encontrados
          </Typography>

          {loading ? (
            <Alert severity="info">Carregando recibos...</Alert>
          ) : recibos.length === 0 ? (
            <Alert severity="warning">Nenhum recibo encontrado</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Nº Recibo</TableCell>
                    <TableCell>Data PDP</TableCell>
                    <TableCell>Empresa</TableCell>
                    <TableCell>Data/Hora Envio</TableCell>
                    <TableCell>Usuário</TableCell>
                    <TableCell>Tipo Envio</TableCell>
                    <TableCell>Registros</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell align="right">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {recibos.map((recibo) => (
                    <TableRow key={recibo.id}>
                      <TableCell>{recibo.numeroRecibo}</TableCell>
                      <TableCell>{new Date(recibo.dataPdp).toLocaleDateString('pt-BR')}</TableCell>
                      <TableCell>{recibo.empresa}</TableCell>
                      <TableCell>
                        {new Date(recibo.dataEnvio).toLocaleDateString('pt-BR')} {recibo.horaEnvio}
                      </TableCell>
                      <TableCell>{recibo.usuario}</TableCell>
                      <TableCell>{recibo.tipoEnvio}</TableCell>
                      <TableCell align="center">{recibo.quantidadeRegistros}</TableCell>
                      <TableCell>{getStatusChip(recibo.status)}</TableCell>
                      <TableCell align="right">
                        <Tooltip title="Visualizar Recibo">
                          <IconButton
                            size="small"
                            color="primary"
                            onClick={() => handleVisualizar(recibo)}
                          >
                            <Visibility />
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

      {/* Dialog de Visualização do Recibo */}
      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)} maxWidth="md" fullWidth>
        <DialogTitle>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Typography variant="h6">Recibo Nº {selectedRecibo?.numeroRecibo}</Typography>
            <Button startIcon={<Print />} onClick={handleImprimir}>
              Imprimir
            </Button>
          </Box>
        </DialogTitle>
        <DialogContent dividers>
          {selectedRecibo && (
            <Box>
              <Typography variant="subtitle1" sx={{ mb: 2, fontWeight: 'bold' }}>
                Informações do Envio
              </Typography>

              <Table size="small">
                <TableBody>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Data PDP:</TableCell>
                    <TableCell>{new Date(selectedRecibo.dataPdp).toLocaleDateString('pt-BR')}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Empresa:</TableCell>
                    <TableCell>{selectedRecibo.empresa}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Data/Hora Envio:</TableCell>
                    <TableCell>
                      {new Date(selectedRecibo.dataEnvio).toLocaleDateString('pt-BR')}{' '}
                      {selectedRecibo.horaEnvio}
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Usuário:</TableCell>
                    <TableCell>{selectedRecibo.usuario}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Tipo de Envio:</TableCell>
                    <TableCell>{selectedRecibo.tipoEnvio}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Quantidade de Registros:</TableCell>
                    <TableCell>{selectedRecibo.quantidadeRegistros}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell sx={{ fontWeight: 'bold' }}>Status:</TableCell>
                    <TableCell>{getStatusChip(selectedRecibo.status)}</TableCell>
                  </TableRow>
                  {selectedRecibo.detalhes && (
                    <TableRow>
                      <TableCell sx={{ fontWeight: 'bold' }}>Detalhes:</TableCell>
                      <TableCell>{selectedRecibo.detalhes}</TableCell>
                    </TableRow>
                  )}
                </TableBody>
              </Table>

              <Alert severity="info" sx={{ mt: 2 }}>
                Este recibo comprova o envio dos dados para o sistema PDPw
              </Alert>
            </Box>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDialogOpen(false)}>Fechar</Button>
        </DialogActions>
      </Dialog>

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

export default VisualizacaoRecibos;

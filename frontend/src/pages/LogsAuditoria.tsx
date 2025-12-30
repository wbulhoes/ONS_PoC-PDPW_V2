import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
  TextField,
  MenuItem,
  Grid,
  Card,
  CardContent,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TablePagination,
  Alert,
  Snackbar,
} from '@mui/material';
import { Visibility, FilterList, Download } from '@mui/icons-material';
import api from '../services/api';

interface LogAuditoria {
  id: number;
  usuario: string;
  acao: string;
  entidade: string;
  entidadeId?: number;
  detalhes: string;
  ip: string;
  dataHora: string;
  nivel: string;
}

const LogsAuditoria: React.FC = () => {
  const [logs, setLogs] = useState<LogAuditoria[]>([]);
  const [selectedLog, setSelectedLog] = useState<LogAuditoria | null>(null);
  const [openDialog, setOpenDialog] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(25);
  const [totalLogs, setTotalLogs] = useState(0);
  const [filters, setFilters] = useState({
    usuario: '',
    acao: '',
    entidade: '',
    nivel: '',
    dataInicio: '',
    dataFim: '',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadLogs();
  }, [page, rowsPerPage, filters]);

  const loadLogs = async () => {
    try {
      const params = {
        page,
        size: rowsPerPage,
        ...filters,
      };
      const response = await api.get('/auditoria/logs', { params });
      setLogs(response.data.content);
      setTotalLogs(response.data.totalElements);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar logs', severity: 'error' });
    }
  };

  const handleViewDetails = (log: LogAuditoria) => {
    setSelectedLog(log);
    setOpenDialog(true);
  };

  const handleExport = async () => {
    try {
      const response = await api.get('/auditoria/logs/exportar', {
        params: filters,
        responseType: 'blob',
      });

      const fileName = `logs_auditoria_${new Date().getTime()}.csv`;
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSnackbar({ open: true, message: 'Logs exportados com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao exportar logs', severity: 'error' });
    }
  };

  const getNivelColor = (nivel: string) => {
    const colors: any = {
      INFO: 'info',
      WARNING: 'warning',
      ERROR: 'error',
      CRITICAL: 'error',
    };
    return colors[nivel] || 'default';
  };

  const getAcaoColor = (acao: string) => {
    const colors: any = {
      CREATE: 'success',
      UPDATE: 'warning',
      DELETE: 'error',
      READ: 'info',
      LOGIN: 'primary',
      LOGOUT: 'default',
    };
    return colors[acao] || 'default';
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Logs de Auditoria</Typography>
        <Button variant="contained" startIcon={<Download />} onClick={handleExport}>
          Exportar
        </Button>
      </Box>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Filtros
          </Typography>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                label="Usuário"
                value={filters.usuario}
                onChange={(e) => setFilters({ ...filters, usuario: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                select
                label="Ação"
                value={filters.acao}
                onChange={(e) => setFilters({ ...filters, acao: e.target.value })}
              >
                <MenuItem value="">Todas</MenuItem>
                <MenuItem value="CREATE">Criar</MenuItem>
                <MenuItem value="UPDATE">Atualizar</MenuItem>
                <MenuItem value="DELETE">Excluir</MenuItem>
                <MenuItem value="READ">Ler</MenuItem>
                <MenuItem value="LOGIN">Login</MenuItem>
                <MenuItem value="LOGOUT">Logout</MenuItem>
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                label="Entidade"
                value={filters.entidade}
                onChange={(e) => setFilters({ ...filters, entidade: e.target.value })}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                select
                label="Nível"
                value={filters.nivel}
                onChange={(e) => setFilters({ ...filters, nivel: e.target.value })}
              >
                <MenuItem value="">Todos</MenuItem>
                <MenuItem value="INFO">Info</MenuItem>
                <MenuItem value="WARNING">Warning</MenuItem>
                <MenuItem value="ERROR">Error</MenuItem>
                <MenuItem value="CRITICAL">Critical</MenuItem>
              </TextField>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                label="Data Início"
                type="date"
                value={filters.dataInicio}
                onChange={(e) => setFilters({ ...filters, dataInicio: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <TextField
                fullWidth
                label="Data Fim"
                type="date"
                value={filters.dataFim}
                onChange={(e) => setFilters({ ...filters, dataFim: e.target.value })}
                InputLabelProps={{ shrink: true }}
              />
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Data/Hora</TableCell>
              <TableCell>Usuário</TableCell>
              <TableCell>Ação</TableCell>
              <TableCell>Entidade</TableCell>
              <TableCell>Nível</TableCell>
              <TableCell>IP</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {logs.map((log) => (
              <TableRow key={log.id}>
                <TableCell>{new Date(log.dataHora).toLocaleString()}</TableCell>
                <TableCell>{log.usuario}</TableCell>
                <TableCell>
                  <Chip label={log.acao} color={getAcaoColor(log.acao)} size="small" />
                </TableCell>
                <TableCell>
                  {log.entidade}
                  {log.entidadeId && ` #${log.entidadeId}`}
                </TableCell>
                <TableCell>
                  <Chip label={log.nivel} color={getNivelColor(log.nivel)} size="small" />
                </TableCell>
                <TableCell>{log.ip}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleViewDetails(log)} color="primary" size="small">
                    <Visibility />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <TablePagination
          component="div"
          count={totalLogs}
          page={page}
          onPageChange={(e, newPage) => setPage(newPage)}
          rowsPerPage={rowsPerPage}
          onRowsPerPageChange={(e) => {
            setRowsPerPage(parseInt(e.target.value, 10));
            setPage(0);
          }}
          rowsPerPageOptions={[10, 25, 50, 100]}
          labelRowsPerPage="Registros por página"
        />
      </TableContainer>

      <Dialog open={openDialog} onClose={() => setOpenDialog(false)} maxWidth="md" fullWidth>
        <DialogTitle>Detalhes do Log</DialogTitle>
        <DialogContent>
          {selectedLog && (
            <Box>
              <Grid container spacing={2} sx={{ mt: 1 }}>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Data/Hora
                  </Typography>
                  <Typography variant="body1">{new Date(selectedLog.dataHora).toLocaleString()}</Typography>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Usuário
                  </Typography>
                  <Typography variant="body1">{selectedLog.usuario}</Typography>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Ação
                  </Typography>
                  <Chip label={selectedLog.acao} color={getAcaoColor(selectedLog.acao)} size="small" />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Nível
                  </Typography>
                  <Chip label={selectedLog.nivel} color={getNivelColor(selectedLog.nivel)} size="small" />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Entidade
                  </Typography>
                  <Typography variant="body1">
                    {selectedLog.entidade}
                    {selectedLog.entidadeId && ` #${selectedLog.entidadeId}`}
                  </Typography>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <Typography variant="subtitle2" color="text.secondary">
                    IP
                  </Typography>
                  <Typography variant="body1">{selectedLog.ip}</Typography>
                </Grid>
                <Grid item xs={12}>
                  <Typography variant="subtitle2" color="text.secondary">
                    Detalhes
                  </Typography>
                  <Paper sx={{ p: 2, mt: 1, bgcolor: 'grey.100' }}>
                    <Typography variant="body2" component="pre" sx={{ whiteSpace: 'pre-wrap', wordBreak: 'break-word' }}>
                      {selectedLog.detalhes}
                    </Typography>
                  </Paper>
                </Grid>
              </Grid>
            </Box>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenDialog(false)}>Fechar</Button>
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

export default LogsAuditoria;

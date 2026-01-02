import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
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
  TablePagination,
  CircularProgress,
  Alert,
  Snackbar,
  IconButton,
} from '@mui/material';
import { Search, Refresh, FileDownload } from '@mui/icons-material';

export interface BaseQueryFilter {
  dataPdpInicio?: string;
  dataPdpFim?: string;
  empresaId?: string;
  usinaId?: string;
}

export interface BaseQueryColumn {
  id: string;
  label: string;
  minWidth?: number;
  align?: 'left' | 'right' | 'center';
  format?: (value: any) => string;
}

interface BaseQueryPageProps<T> {
  title: string;
  columns: BaseQueryColumn[];
  onSearch: (filters: BaseQueryFilter) => Promise<T[]>;
  onExport?: (filters: BaseQueryFilter) => Promise<Blob>;
  renderCustomFilters?: () => React.ReactNode;
  showEmpresaFilter?: boolean;
  showUsinaFilter?: boolean;
  showDateRange?: boolean;
}

function BaseQueryPage<T extends Record<string, any>>({
  title,
  columns,
  onSearch,
  onExport,
  renderCustomFilters,
  showEmpresaFilter = true,
  showUsinaFilter = false,
  showDateRange = true,
}: BaseQueryPageProps<T>) {
  const [filters, setFilters] = useState<BaseQueryFilter>({
    dataPdpInicio: '',
    dataPdpFim: '',
    empresaId: '',
    usinaId: '',
  });

  const [data, setData] = useState<T[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(10);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  const [empresas, setEmpresas] = useState<any[]>([]);
  const [usinas, setUsinas] = useState<any[]>([]);

  useEffect(() => {
    loadEmpresas();
    if (showUsinaFilter) {
      loadUsinas();
    }
  }, [showUsinaFilter]);

  const loadEmpresas = async () => {
    try {
      // TODO: Implementar carregamento de empresas
      setEmpresas([]);
    } catch (error) {
      console.error('Erro ao carregar empresas');
    }
  };

  const loadUsinas = async () => {
    try {
      // TODO: Implementar carregamento de usinas
      setUsinas([]);
    } catch (error) {
      console.error('Erro ao carregar usinas');
    }
  };

  const handleSearch = async () => {
    setLoading(true);
    try {
      const result = await onSearch(filters);
      setData(result);
      setSnackbar({
        open: true,
        message: `${result.length} registro(s) encontrado(s)`,
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: error.message || 'Erro ao buscar dados',
        severity: 'error',
      });
      setData([]);
    } finally {
      setLoading(false);
    }
  };

  const handleExport = async () => {
    if (!onExport) return;

    setLoading(true);
    try {
      const blob = await onExport(filters);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `${title.replace(/\s/g, '_')}_${new Date().getTime()}.xlsx`);
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSnackbar({
        open: true,
        message: 'Arquivo exportado com sucesso',
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao exportar dados',
        severity: 'error',
      });
    } finally {
      setLoading(false);
    }
  };

  const handleChangePage = (event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(+event.target.value);
    setPage(0);
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        {title}
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Filtros
          </Typography>
          <Grid container spacing={2}>
            {showDateRange && (
              <>
                <Grid item xs={12} md={3}>
                  <TextField
                    fullWidth
                    label="Data Início"
                    type="date"
                    value={filters.dataPdpInicio}
                    onChange={(e) => setFilters({ ...filters, dataPdpInicio: e.target.value })}
                    InputLabelProps={{ shrink: true }}
                  />
                </Grid>
                <Grid item xs={12} md={3}>
                  <TextField
                    fullWidth
                    label="Data Fim"
                    type="date"
                    value={filters.dataPdpFim}
                    onChange={(e) => setFilters({ ...filters, dataPdpFim: e.target.value })}
                    InputLabelProps={{ shrink: true }}
                  />
                </Grid>
              </>
            )}

            {showEmpresaFilter && (
              <Grid item xs={12} md={showDateRange ? 3 : 6}>
                <TextField
                  fullWidth
                  select
                  label="Empresa"
                  value={filters.empresaId}
                  onChange={(e) => setFilters({ ...filters, empresaId: e.target.value })}
                >
                  <MenuItem value="">Todas</MenuItem>
                  {empresas.map((empresa) => (
                    <MenuItem key={empresa.id} value={empresa.id}>
                      {empresa.nome}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
            )}

            {showUsinaFilter && (
              <Grid item xs={12} md={showDateRange ? 3 : 6}>
                <TextField
                  fullWidth
                  select
                  label="Usina"
                  value={filters.usinaId}
                  onChange={(e) => setFilters({ ...filters, usinaId: e.target.value })}
                >
                  <MenuItem value="">Todas</MenuItem>
                  {usinas.map((usina) => (
                    <MenuItem key={usina.id} value={usina.id}>
                      {usina.nome}
                    </MenuItem>
                  ))}
                </TextField>
              </Grid>
            )}

            {renderCustomFilters && renderCustomFilters()}
          </Grid>

          <Box sx={{ mt: 2, display: 'flex', gap: 1 }}>
            <Button
              variant="contained"
              startIcon={<Search />}
              onClick={handleSearch}
              disabled={loading}
            >
              Pesquisar
            </Button>
            <Button
              variant="outlined"
              startIcon={<Refresh />}
              onClick={() => {
                setFilters({
                  dataPdpInicio: '',
                  dataPdpFim: '',
                  empresaId: '',
                  usinaId: '',
                });
                setData([]);
              }}
              disabled={loading}
            >
              Limpar
            </Button>
            {onExport && (
              <Button
                variant="outlined"
                startIcon={<FileDownload />}
                onClick={handleExport}
                disabled={loading || data.length === 0}
              >
                Exportar
              </Button>
            )}
          </Box>
        </CardContent>
      </Card>

      {loading && (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <CircularProgress />
        </Box>
      )}

      {!loading && data.length > 0 && (
        <Paper>
          <TableContainer sx={{ maxHeight: 600 }}>
            <Table stickyHeader>
              <TableHead>
                <TableRow>
                  {columns.map((column) => (
                    <TableCell
                      key={column.id}
                      align={column.align}
                      style={{ minWidth: column.minWidth }}
                    >
                      {column.label}
                    </TableCell>
                  ))}
                </TableRow>
              </TableHead>
              <TableBody>
                {data
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((row, index) => (
                    <TableRow hover key={index}>
                      {columns.map((column) => {
                        const value = row[column.id];
                        return (
                          <TableCell key={column.id} align={column.align}>
                            {column.format ? column.format(value) : value}
                          </TableCell>
                        );
                      })}
                    </TableRow>
                  ))}
              </TableBody>
            </Table>
          </TableContainer>
          <TablePagination
            rowsPerPageOptions={[10, 25, 50, 100]}
            component="div"
            count={data.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            labelRowsPerPage="Linhas por página:"
          />
        </Paper>
      )}

      {!loading && data.length === 0 && (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            Nenhum registro encontrado. Use os filtros acima para buscar dados.
          </Typography>
        </Box>
      )}

      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar({ ...snackbar, open: false })}
      >
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
}

export default BaseQueryPage;

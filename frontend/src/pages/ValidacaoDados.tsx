import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  Alert,
  Snackbar,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
  IconButton,
  Collapse,
  MenuItem,
  TextField,
} from '@mui/material';
import { PlayArrow, CheckCircle, Error as ErrorIcon, Warning, ExpandMore, ExpandLess } from '@mui/icons-material';
import api from '../services/api';

interface ValidationRule {
  id: string;
  name: string;
  description: string;
  severity: 'error' | 'warning' | 'info';
}

interface ValidationResult {
  ruleId: string;
  ruleName: string;
  status: 'passed' | 'failed' | 'warning';
  message: string;
  details: string[];
  affectedRecords: number;
}

const ValidacaoDados: React.FC = () => {
  const [tipoValidacao, setTipoValidacao] = useState('');
  const [validating, setValidating] = useState(false);
  const [validationResults, setValidationResults] = useState<ValidationResult[]>([]);
  const [expandedRows, setExpandedRows] = useState<Set<string>>(new Set());
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const tiposValidacao = [
    { value: 'usinas', label: 'Usinas' },
    { value: 'unidades-geradoras', label: 'Unidades Geradoras' },
    { value: 'subsistemas', label: 'Subsistemas' },
    { value: 'submercados', label: 'Submercados' },
    { value: 'reservatorios', label: 'Reservatórios' },
    { value: 'interligacoes', label: 'Interligações' },
    { value: 'limites-intercambio', label: 'Limites de Intercâmbio' },
    { value: 'todos', label: 'Todos os Dados' },
  ];

  const handleValidate = async () => {
    if (!tipoValidacao) {
      setSnackbar({ open: true, message: 'Selecione o tipo de validação', severity: 'error' });
      return;
    }

    setValidating(true);
    setValidationResults([]);

    try {
      const response = await api.post('/validacao/executar', { tipo: tipoValidacao });
      setValidationResults(response.data.results);
      
      const hasErrors = response.data.results.some((r: ValidationResult) => r.status === 'failed');
      const message = hasErrors 
        ? 'Validação concluída com erros' 
        : 'Validação concluída com sucesso';
      
      setSnackbar({ 
        open: true, 
        message, 
        severity: hasErrors ? 'warning' : 'success' 
      });
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Erro ao executar validação';
      setSnackbar({ open: true, message: errorMessage, severity: 'error' });
    } finally {
      setValidating(false);
    }
  };

  const toggleRowExpansion = (ruleId: string) => {
    const newExpanded = new Set(expandedRows);
    if (newExpanded.has(ruleId)) {
      newExpanded.delete(ruleId);
    } else {
      newExpanded.add(ruleId);
    }
    setExpandedRows(newExpanded);
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'passed':
        return <CheckCircle color="success" />;
      case 'failed':
        return <ErrorIcon color="error" />;
      case 'warning':
        return <Warning color="warning" />;
      default:
        return null;
    }
  };

  const getStatusChip = (status: string) => {
    const colors: any = {
      passed: 'success',
      failed: 'error',
      warning: 'warning',
    };
    const labels: any = {
      passed: 'Aprovado',
      failed: 'Falhou',
      warning: 'Aviso',
    };
    return <Chip label={labels[status]} color={colors[status]} size="small" />;
  };

  const getSummary = () => {
    const passed = validationResults.filter(r => r.status === 'passed').length;
    const failed = validationResults.filter(r => r.status === 'failed').length;
    const warnings = validationResults.filter(r => r.status === 'warning').length;
    return { passed, failed, warnings, total: validationResults.length };
  };

  const summary = getSummary();

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Validação de Dados
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} md={4}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configuração
              </Typography>

              <TextField
                fullWidth
                select
                label="Tipo de Validação"
                value={tipoValidacao}
                onChange={(e) => setTipoValidacao(e.target.value)}
                sx={{ mb: 2 }}
              >
                {tiposValidacao.map((tipo) => (
                  <MenuItem key={tipo.value} value={tipo.value}>
                    {tipo.label}
                  </MenuItem>
                ))}
              </TextField>

              <Button
                variant="contained"
                color="primary"
                onClick={handleValidate}
                disabled={!tipoValidacao || validating}
                startIcon={<PlayArrow />}
                fullWidth
              >
                {validating ? 'Validando...' : 'Executar Validação'}
              </Button>
            </CardContent>
          </Card>

          {validationResults.length > 0 && (
            <Card sx={{ mt: 2 }}>
              <CardContent>
                <Typography variant="h6" sx={{ mb: 2 }}>
                  Resumo
                </Typography>

                <Grid container spacing={2}>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center' }}>
                      <Typography variant="h4" color="success.main">
                        {summary.passed}
                      </Typography>
                      <Typography variant="body2">Aprovados</Typography>
                    </Box>
                  </Grid>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center' }}>
                      <Typography variant="h4" color="error.main">
                        {summary.failed}
                      </Typography>
                      <Typography variant="body2">Falhas</Typography>
                    </Box>
                  </Grid>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center' }}>
                      <Typography variant="h4" color="warning.main">
                        {summary.warnings}
                      </Typography>
                      <Typography variant="body2">Avisos</Typography>
                    </Box>
                  </Grid>
                  <Grid item xs={6}>
                    <Box sx={{ textAlign: 'center' }}>
                      <Typography variant="h4" color="primary.main">
                        {summary.total}
                      </Typography>
                      <Typography variant="body2">Total</Typography>
                    </Box>
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          )}
        </Grid>

        <Grid item xs={12} md={8}>
          {validationResults.length > 0 ? (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell width="50px"></TableCell>
                    <TableCell>Regra</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Registros Afetados</TableCell>
                    <TableCell width="50px"></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {validationResults.map((result) => (
                    <React.Fragment key={result.ruleId}>
                      <TableRow>
                        <TableCell>{getStatusIcon(result.status)}</TableCell>
                        <TableCell>
                          <Typography variant="body1">{result.ruleName}</Typography>
                          <Typography variant="body2" color="text.secondary">
                            {result.message}
                          </Typography>
                        </TableCell>
                        <TableCell>{getStatusChip(result.status)}</TableCell>
                        <TableCell>{result.affectedRecords}</TableCell>
                        <TableCell>
                          {result.details && result.details.length > 0 && (
                            <IconButton
                              size="small"
                              onClick={() => toggleRowExpansion(result.ruleId)}
                            >
                              {expandedRows.has(result.ruleId) ? <ExpandLess /> : <ExpandMore />}
                            </IconButton>
                          )}
                        </TableCell>
                      </TableRow>
                      {result.details && result.details.length > 0 && (
                        <TableRow>
                          <TableCell colSpan={5} sx={{ py: 0 }}>
                            <Collapse in={expandedRows.has(result.ruleId)} timeout="auto" unmountOnExit>
                              <Box sx={{ p: 2, bgcolor: 'background.default' }}>
                                <Typography variant="subtitle2" sx={{ mb: 1 }}>
                                  Detalhes:
                                </Typography>
                                {result.details.map((detail, index) => (
                                  <Typography key={index} variant="body2" sx={{ ml: 2 }}>
                                    • {detail}
                                  </Typography>
                                ))}
                              </Box>
                            </Collapse>
                          </TableCell>
                        </TableRow>
                      )}
                    </React.Fragment>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          ) : (
            <Card>
              <CardContent>
                <Box sx={{ textAlign: 'center', py: 4 }}>
                  <Typography variant="h6" color="text.secondary">
                    Selecione um tipo de validação e clique em "Executar Validação"
                  </Typography>
                </Box>
              </CardContent>
            </Card>
          )}
        </Grid>
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

export default ValidacaoDados;

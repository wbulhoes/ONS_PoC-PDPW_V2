import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  MenuItem,
  TextField,
  Button,
  Alert,
  Snackbar,
  FormControlLabel,
  Checkbox,
  List,
  ListItem,
  ListItemText,
  Divider,
  LinearProgress,
} from '@mui/material';
import { Download, Description } from '@mui/icons-material';
import api from '../services/api';

interface Simulacao {
  id: number;
  cenarioNome: string;
  status: string;
}

interface ExportConfig {
  simulacaoId: number;
  formato: string;
  incluirResumo: boolean;
  incluirSerieGeracao: boolean;
  incluirSerieCusto: boolean;
  incluirSerieIntercambio: boolean;
  incluirGraficos: boolean;
}

const ExportacaoRelatorios: React.FC = () => {
  const [simulacoes, setSimulacoes] = useState<Simulacao[]>([]);
  const [config, setConfig] = useState<ExportConfig>({
    simulacaoId: 0,
    formato: 'PDF',
    incluirResumo: true,
    incluirSerieGeracao: true,
    incluirSerieCusto: true,
    incluirSerieIntercambio: true,
    incluirGraficos: true,
  });
  const [exporting, setExporting] = useState(false);
  const [progress, setProgress] = useState(0);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadSimulacoes();
  }, []);

  const loadSimulacoes = async () => {
    try {
      const response = await api.get('/simulacoes?status=CONCLUIDA');
      setSimulacoes(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar simulações', severity: 'error' });
    }
  };

  const handleExport = async () => {
    if (config.simulacaoId === 0) {
      setSnackbar({ open: true, message: 'Selecione uma simulação', severity: 'error' });
      return;
    }

    setExporting(true);
    setProgress(0);

    try {
      const response = await api.post(
        '/relatorios/exportar',
        config,
        {
          responseType: 'blob',
          onDownloadProgress: (progressEvent) => {
            const percentCompleted = Math.round((progressEvent.loaded * 100) / (progressEvent.total || 1));
            setProgress(percentCompleted);
          },
        }
      );

      const simulacao = simulacoes.find((s) => s.id === config.simulacaoId);
      const fileName = `relatorio_${simulacao?.cenarioNome}_${new Date().getTime()}.${config.formato.toLowerCase()}`;

      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSnackbar({ open: true, message: 'Relatório exportado com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao exportar relatório', severity: 'error' });
    } finally {
      setExporting(false);
      setProgress(0);
    }
  };

  const handleExportTemplate = async () => {
    try {
      const response = await api.get('/relatorios/template', {
        responseType: 'blob',
      });

      const fileName = `template_relatorio.docx`;
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();

      setSnackbar({ open: true, message: 'Template baixado com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao baixar template', severity: 'error' });
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Exportação de Relatórios
      </Typography>

      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configuração do Relatório
              </Typography>

              <TextField
                fullWidth
                select
                label="Simulação"
                value={config.simulacaoId}
                onChange={(e) => setConfig({ ...config, simulacaoId: Number(e.target.value) })}
                sx={{ mb: 2 }}
              >
                <MenuItem value={0}>Selecione uma simulação</MenuItem>
                {simulacoes.map((simulacao) => (
                  <MenuItem key={simulacao.id} value={simulacao.id}>
                    {simulacao.cenarioNome} (ID: {simulacao.id})
                  </MenuItem>
                ))}
              </TextField>

              <TextField
                fullWidth
                select
                label="Formato"
                value={config.formato}
                onChange={(e) => setConfig({ ...config, formato: e.target.value })}
                sx={{ mb: 2 }}
              >
                <MenuItem value="PDF">PDF</MenuItem>
                <MenuItem value="EXCEL">Excel</MenuItem>
                <MenuItem value="CSV">CSV</MenuItem>
                <MenuItem value="JSON">JSON</MenuItem>
              </TextField>

              <Typography variant="subtitle2" sx={{ mb: 1 }}>
                Conteúdo do Relatório
              </Typography>

              <FormControlLabel
                control={
                  <Checkbox
                    checked={config.incluirResumo}
                    onChange={(e) => setConfig({ ...config, incluirResumo: e.target.checked })}
                  />
                }
                label="Incluir Resumo Executivo"
              />

              <FormControlLabel
                control={
                  <Checkbox
                    checked={config.incluirSerieGeracao}
                    onChange={(e) => setConfig({ ...config, incluirSerieGeracao: e.target.checked })}
                  />
                }
                label="Incluir Série de Geração"
              />

              <FormControlLabel
                control={
                  <Checkbox
                    checked={config.incluirSerieCusto}
                    onChange={(e) => setConfig({ ...config, incluirSerieCusto: e.target.checked })}
                  />
                }
                label="Incluir Série de Custo"
              />

              <FormControlLabel
                control={
                  <Checkbox
                    checked={config.incluirSerieIntercambio}
                    onChange={(e) => setConfig({ ...config, incluirSerieIntercambio: e.target.checked })}
                  />
                }
                label="Incluir Série de Intercâmbio"
              />

              <FormControlLabel
                control={
                  <Checkbox
                    checked={config.incluirGraficos}
                    onChange={(e) => setConfig({ ...config, incluirGraficos: e.target.checked })}
                    disabled={config.formato === 'CSV' || config.formato === 'JSON'}
                  />
                }
                label="Incluir Gráficos"
              />

              {exporting && (
                <Box sx={{ mt: 2 }}>
                  <LinearProgress variant="determinate" value={progress} />
                  <Typography variant="caption" color="text.secondary" sx={{ mt: 1 }}>
                    Exportando... {progress}%
                  </Typography>
                </Box>
              )}

              <Button
                fullWidth
                variant="contained"
                startIcon={<Download />}
                onClick={handleExport}
                disabled={exporting || config.simulacaoId === 0}
                sx={{ mt: 2 }}
              >
                Exportar Relatório
              </Button>
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card sx={{ mb: 3 }}>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Formatos Disponíveis
              </Typography>
              <List>
                <ListItem>
                  <ListItemText
                    primary="PDF"
                    secondary="Relatório completo com gráficos e formatação profissional"
                  />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemText
                    primary="Excel"
                    secondary="Planilha com dados tabulares e gráficos integrados"
                  />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemText primary="CSV" secondary="Dados brutos em formato de texto separado por vírgulas" />
                </ListItem>
                <Divider />
                <ListItem>
                  <ListItemText primary="JSON" secondary="Dados estruturados em formato JSON para integração" />
                </ListItem>
              </List>
            </CardContent>
          </Card>

          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Templates
              </Typography>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
                Baixe o template padrão de relatório para personalização
              </Typography>
              <Button
                fullWidth
                variant="outlined"
                startIcon={<Description />}
                onClick={handleExportTemplate}
              >
                Baixar Template
              </Button>
            </CardContent>
          </Card>
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

export default ExportacaoRelatorios;

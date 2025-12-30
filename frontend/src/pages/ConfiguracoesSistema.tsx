import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Paper,
  Grid,
  Card,
  CardContent,
  TextField,
  Button,
  Switch,
  FormControlLabel,
  Divider,
  Alert,
  Snackbar,
  List,
  ListItem,
  ListItemText,
  MenuItem,
} from '@mui/material';
import { Save, RestartAlt } from '@mui/icons-material';
import api from '../services/api';

interface ConfiguracaoSistema {
  geral: {
    nomeAplicacao: string;
    versao: string;
    ambiente: string;
    manutencao: boolean;
  };
  email: {
    servidor: string;
    porta: number;
    usuario: string;
    ssl: boolean;
    remetente: string;
  };
  seguranca: {
    tempoSessao: number;
    tentativasLogin: number;
    senhaMinLength: number;
    senhaRequireSpecial: boolean;
    senhaRequireNumber: boolean;
    senhaRequireUpper: boolean;
  };
  simulacao: {
    maxIteracoes: number;
    maxThreads: number;
    timeoutMinutos: number;
    salvarIntermediarios: boolean;
  };
  backup: {
    automatico: boolean;
    frequenciaHoras: number;
    retencaoDias: number;
    caminho: string;
  };
}

const ConfiguracoesSistema: React.FC = () => {
  const [config, setConfig] = useState<ConfiguracaoSistema>({
    geral: {
      nomeAplicacao: 'PDPw',
      versao: '1.0.0',
      ambiente: 'PRODUCAO',
      manutencao: false,
    },
    email: {
      servidor: '',
      porta: 587,
      usuario: '',
      ssl: true,
      remetente: '',
    },
    seguranca: {
      tempoSessao: 60,
      tentativasLogin: 3,
      senhaMinLength: 8,
      senhaRequireSpecial: true,
      senhaRequireNumber: true,
      senhaRequireUpper: true,
    },
    simulacao: {
      maxIteracoes: 1000,
      maxThreads: 4,
      timeoutMinutos: 120,
      salvarIntermediarios: false,
    },
    backup: {
      automatico: true,
      frequenciaHoras: 24,
      retencaoDias: 30,
      caminho: '/backup',
    },
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadConfiguracoes();
  }, []);

  const loadConfiguracoes = async () => {
    try {
      const response = await api.get('/configuracoes');
      setConfig(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar configurações', severity: 'error' });
    }
  };

  const handleSave = async () => {
    try {
      await api.put('/configuracoes', config);
      setSnackbar({ open: true, message: 'Configurações salvas com sucesso', severity: 'success' });
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar configurações', severity: 'error' });
    }
  };

  const handleReset = async () => {
    if (window.confirm('Deseja realmente restaurar as configurações padrão?')) {
      try {
        await api.post('/configuracoes/reset');
        loadConfiguracoes();
        setSnackbar({ open: true, message: 'Configurações restauradas', severity: 'success' });
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao restaurar configurações', severity: 'error' });
      }
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Configurações do Sistema</Typography>
        <Box>
          <Button variant="outlined" startIcon={<RestartAlt />} onClick={handleReset} sx={{ mr: 1 }}>
            Restaurar Padrão
          </Button>
          <Button variant="contained" startIcon={<Save />} onClick={handleSave}>
            Salvar
          </Button>
        </Box>
      </Box>

      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configurações Gerais
              </Typography>
              <TextField
                fullWidth
                label="Nome da Aplicação"
                value={config.geral.nomeAplicacao}
                onChange={(e) => setConfig({ ...config, geral: { ...config.geral, nomeAplicacao: e.target.value } })}
                margin="normal"
              />
              <TextField
                fullWidth
                label="Versão"
                value={config.geral.versao}
                disabled
                margin="normal"
              />
              <TextField
                fullWidth
                select
                label="Ambiente"
                value={config.geral.ambiente}
                onChange={(e) => setConfig({ ...config, geral: { ...config.geral, ambiente: e.target.value } })}
                margin="normal"
              >
                <MenuItem value="DESENVOLVIMENTO">Desenvolvimento</MenuItem>
                <MenuItem value="HOMOLOGACAO">Homologação</MenuItem>
                <MenuItem value="PRODUCAO">Produção</MenuItem>
              </TextField>
              <FormControlLabel
                control={
                  <Switch
                    checked={config.geral.manutencao}
                    onChange={(e) =>
                      setConfig({ ...config, geral: { ...config.geral, manutencao: e.target.checked } })
                    }
                  />
                }
                label="Modo Manutenção"
              />
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configurações de Email
              </Typography>
              <TextField
                fullWidth
                label="Servidor SMTP"
                value={config.email.servidor}
                onChange={(e) => setConfig({ ...config, email: { ...config.email, servidor: e.target.value } })}
                margin="normal"
              />
              <TextField
                fullWidth
                label="Porta"
                type="number"
                value={config.email.porta}
                onChange={(e) => setConfig({ ...config, email: { ...config.email, porta: Number(e.target.value) } })}
                margin="normal"
              />
              <TextField
                fullWidth
                label="Usuário"
                value={config.email.usuario}
                onChange={(e) => setConfig({ ...config, email: { ...config.email, usuario: e.target.value } })}
                margin="normal"
              />
              <TextField
                fullWidth
                label="Remetente"
                value={config.email.remetente}
                onChange={(e) => setConfig({ ...config, email: { ...config.email, remetente: e.target.value } })}
                margin="normal"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={config.email.ssl}
                    onChange={(e) => setConfig({ ...config, email: { ...config.email, ssl: e.target.checked } })}
                  />
                }
                label="Usar SSL"
              />
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configurações de Segurança
              </Typography>
              <TextField
                fullWidth
                label="Tempo de Sessão (minutos)"
                type="number"
                value={config.seguranca.tempoSessao}
                onChange={(e) =>
                  setConfig({ ...config, seguranca: { ...config.seguranca, tempoSessao: Number(e.target.value) } })
                }
                margin="normal"
              />
              <TextField
                fullWidth
                label="Tentativas de Login"
                type="number"
                value={config.seguranca.tentativasLogin}
                onChange={(e) =>
                  setConfig({ ...config, seguranca: { ...config.seguranca, tentativasLogin: Number(e.target.value) } })
                }
                margin="normal"
              />
              <TextField
                fullWidth
                label="Tamanho Mínimo da Senha"
                type="number"
                value={config.seguranca.senhaMinLength}
                onChange={(e) =>
                  setConfig({ ...config, seguranca: { ...config.seguranca, senhaMinLength: Number(e.target.value) } })
                }
                margin="normal"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={config.seguranca.senhaRequireSpecial}
                    onChange={(e) =>
                      setConfig({
                        ...config,
                        seguranca: { ...config.seguranca, senhaRequireSpecial: e.target.checked },
                      })
                    }
                  />
                }
                label="Exigir Caracteres Especiais"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={config.seguranca.senhaRequireNumber}
                    onChange={(e) =>
                      setConfig({
                        ...config,
                        seguranca: { ...config.seguranca, senhaRequireNumber: e.target.checked },
                      })
                    }
                  />
                }
                label="Exigir Números"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={config.seguranca.senhaRequireUpper}
                    onChange={(e) =>
                      setConfig({
                        ...config,
                        seguranca: { ...config.seguranca, senhaRequireUpper: e.target.checked },
                      })
                    }
                  />
                }
                label="Exigir Maiúsculas"
              />
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12} md={6}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configurações de Simulação
              </Typography>
              <TextField
                fullWidth
                label="Máximo de Iterações"
                type="number"
                value={config.simulacao.maxIteracoes}
                onChange={(e) =>
                  setConfig({ ...config, simulacao: { ...config.simulacao, maxIteracoes: Number(e.target.value) } })
                }
                margin="normal"
              />
              <TextField
                fullWidth
                label="Máximo de Threads"
                type="number"
                value={config.simulacao.maxThreads}
                onChange={(e) =>
                  setConfig({ ...config, simulacao: { ...config.simulacao, maxThreads: Number(e.target.value) } })
                }
                margin="normal"
              />
              <TextField
                fullWidth
                label="Timeout (minutos)"
                type="number"
                value={config.simulacao.timeoutMinutos}
                onChange={(e) =>
                  setConfig({ ...config, simulacao: { ...config.simulacao, timeoutMinutos: Number(e.target.value) } })
                }
                margin="normal"
              />
              <FormControlLabel
                control={
                  <Switch
                    checked={config.simulacao.salvarIntermediarios}
                    onChange={(e) =>
                      setConfig({
                        ...config,
                        simulacao: { ...config.simulacao, salvarIntermediarios: e.target.checked },
                      })
                    }
                  />
                }
                label="Salvar Resultados Intermediários"
              />
            </CardContent>
          </Card>
        </Grid>

        <Grid item xs={12}>
          <Card>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Configurações de Backup
              </Typography>
              <Grid container spacing={2}>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="Frequência (horas)"
                    type="number"
                    value={config.backup.frequenciaHoras}
                    onChange={(e) =>
                      setConfig({ ...config, backup: { ...config.backup, frequenciaHoras: Number(e.target.value) } })
                    }
                  />
                </Grid>
                <Grid item xs={12} md={6}>
                  <TextField
                    fullWidth
                    label="Retenção (dias)"
                    type="number"
                    value={config.backup.retencaoDias}
                    onChange={(e) =>
                      setConfig({ ...config, backup: { ...config.backup, retencaoDias: Number(e.target.value) } })
                    }
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Caminho do Backup"
                    value={config.backup.caminho}
                    onChange={(e) => setConfig({ ...config, backup: { ...config.backup, caminho: e.target.value } })}
                  />
                </Grid>
                <Grid item xs={12}>
                  <FormControlLabel
                    control={
                      <Switch
                        checked={config.backup.automatico}
                        onChange={(e) =>
                          setConfig({ ...config, backup: { ...config.backup, automatico: e.target.checked } })
                        }
                      />
                    }
                    label="Backup Automático"
                  />
                </Grid>
              </Grid>
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

export default ConfiguracoesSistema;

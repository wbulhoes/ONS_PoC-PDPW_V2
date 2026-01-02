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
  Grid,
} from '@mui/material';
import { Save, Refresh, Send } from '@mui/icons-material';
import { apiClient } from '../../../services/apiClient';

interface ProgramacaoSemanal {
  id?: string;
  semanaPMO: string;
  usina: string;
  empresa: string;
  dias: {
    [key: string]: {
      reducaoCarga: number;
      horarioInicio: string;
      horarioFim: string;
    };
  };
}

const ProgramacaoSemanal: React.FC = () => {
  const [semanaPMO, setSemanaPMO] = useState('');
  const [semanasPMO, setSemanasPMO] = useState<string[]>([]);
  const [usina, setUsina] = useState('');
  const [usinas, setUsinas] = useState<any[]>([]);
  const [programacao, setProgramacao] = useState<ProgramacaoSemanal | null>(null);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: '',
    severity: 'success' as 'success' | 'error',
  });

  const diasSemana = ['Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'];

  useEffect(() => {
    loadSemanasPMO();
    loadUsinas();
  }, []);

  useEffect(() => {
    if (semanaPMO && usina) {
      loadProgramacao();
    }
  }, [semanaPMO, usina]);

  const loadSemanasPMO = async () => {
    try {
      const data = await apiClient.get<any[]>('/programacao/semanas-pmo');
      setSemanasPMO(data.map((s: any) => s.semana));
    } catch (error) {
      console.error('Erro ao carregar semanas PMO');
    }
  };

  const loadUsinas = async () => {
    try {
      const data = await apiClient.get<any[]>('/usinas');
      setUsinas(data);
    } catch (error) {
      console.error('Erro ao carregar usinas');
    }
  };

  const loadProgramacao = async () => {
    try {
      const params = new URLSearchParams({ semanaPMO, usina });
      const data = await apiClient.get<ProgramacaoSemanal>(
        `/programacao/semanal?${params.toString()}`
      );
      setProgramacao(data);
    } catch (error) {
      setProgramacao({
        semanaPMO,
        usina,
        empresa: '',
        dias: diasSemana.reduce((acc, dia) => ({
          ...acc,
          [dia]: { reducaoCarga: 0, horarioInicio: '00:00', horarioFim: '00:00' },
        }), {}),
      });
    }
  };

  const handleSave = async () => {
    if (!programacao) return;

    try {
      await apiClient.post('/programacao/semanal', programacao);
      setSnackbar({
        open: true,
        message: 'Programação semanal salva com sucesso',
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao salvar programação semanal',
        severity: 'error',
      });
    }
  };

  const handleSend = async () => {
    try {
      await apiClient.post('/programacao/semanal/enviar', { semanaPMO, usina });
      setSnackbar({
        open: true,
        message: 'Programação semanal enviada com sucesso',
        severity: 'success',
      });
    } catch (error: any) {
      setSnackbar({
        open: true,
        message: 'Erro ao enviar programação semanal',
        severity: 'error',
      });
    }
  };

  const handleDiaChange = (dia: string, field: string, value: any) => {
    if (!programacao) return;

    setProgramacao({
      ...programacao,
      dias: {
        ...programacao.dias,
        [dia]: {
          ...programacao.dias[dia],
          [field]: value,
        },
      },
    });
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Programação Semanal - Gerenciamento da Demanda
      </Typography>

      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Semana PMO"
                value={semanaPMO}
                onChange={(e) => setSemanaPMO(e.target.value)}
              >
                <MenuItem value="">Selecione</MenuItem>
                {semanasPMO.map((s) => (
                  <MenuItem key={s} value={s}>
                    {s}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                select
                label="Usina"
                value={usina}
                onChange={(e) => setUsina(e.target.value)}
              >
                <MenuItem value="">Selecione</MenuItem>
                {usinas.map((u) => (
                  <MenuItem key={u.id} value={u.id}>
                    {u.nome}
                  </MenuItem>
                ))}
              </TextField>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {programacao && (
        <>
          <Card sx={{ mb: 3 }}>
            <CardContent>
              <Typography variant="h6" sx={{ mb: 2 }}>
                Programação de Redução de Carga
              </Typography>

              <TableContainer component={Paper}>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell>Dia da Semana</TableCell>
                      <TableCell align="right">Redução de Carga (MW)</TableCell>
                      <TableCell>Horário Início</TableCell>
                      <TableCell>Horário Fim</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {diasSemana.map((dia) => (
                      <TableRow key={dia}>
                        <TableCell>{dia}</TableCell>
                        <TableCell align="right">
                          <TextField
                            type="number"
                            size="small"
                            value={programacao.dias[dia]?.reducaoCarga || 0}
                            onChange={(e) =>
                              handleDiaChange(dia, 'reducaoCarga', parseFloat(e.target.value))
                            }
                            sx={{ width: 120 }}
                          />
                        </TableCell>
                        <TableCell>
                          <TextField
                            type="time"
                            size="small"
                            value={programacao.dias[dia]?.horarioInicio || '00:00'}
                            onChange={(e) => handleDiaChange(dia, 'horarioInicio', e.target.value)}
                          />
                        </TableCell>
                        <TableCell>
                          <TextField
                            type="time"
                            size="small"
                            value={programacao.dias[dia]?.horarioFim || '00:00'}
                            onChange={(e) => handleDiaChange(dia, 'horarioFim', e.target.value)}
                          />
                        </TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </CardContent>
          </Card>

          <Box sx={{ display: 'flex', gap: 2 }}>
            <Button variant="outlined" startIcon={<Refresh />} onClick={loadProgramacao}>
              Recarregar
            </Button>
            <Button variant="contained" startIcon={<Save />} onClick={handleSave}>
              Salvar
            </Button>
            <Button variant="contained" color="success" startIcon={<Send />} onClick={handleSend}>
              Enviar para ONS
            </Button>
          </Box>
        </>
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
};

export default ProgramacaoSemanal;

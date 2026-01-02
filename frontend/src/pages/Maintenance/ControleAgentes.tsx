import React, { useState, useEffect } from 'react';
import {
  Box,
  Typography,
  Card,
  CardContent,
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
  Chip,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { Block, CheckCircle, Visibility } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

interface Agente {
  id: string;
  codigo: string;
  nome: string;
  ativo: boolean;
  bloqueado: boolean;
  ultimoAcesso: string;
  motivoBloqueio?: string;
}

const ControleAgentes: React.FC = () => {
  const [agentes, setAgentes] = useState<Agente[]>([]);
  const [selectedAgente, setSelectedAgente] = useState<Agente | null>(null);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [acao, setAcao] = useState<'bloquear' | 'desbloquear'>('bloquear');
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadAgentes();
  }, []);

  const loadAgentes = async () => {
    try {
      const data = await apiClient.get<Agente[]>('/manutencao/controle-agentes');
      setAgentes(data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar agentes', severity: 'error' });
    }
  };

  const handleOpenDialog = (agente: Agente, acao: 'bloquear' | 'desbloquear') => {
    setSelectedAgente(agente);
    setAcao(acao);
    setDialogOpen(true);
  };

  const handleConfirm = async () => {
    if (!selectedAgente) return;

    try {
      await apiClient.post(`/manutencao/controle-agentes/${acao}`, {
        agenteId: selectedAgente.id,
        motivo: acao === 'bloquear' ? 'Bloqueio administrativo' : '',
      });
      setSnackbar({
        open: true,
        message: `Agente ${acao === 'bloquear' ? 'bloqueado' : 'desbloqueado'} com sucesso`,
        severity: 'success',
      });
      setDialogOpen(false);
      loadAgentes();
    } catch (error) {
      setSnackbar({ open: true, message: `Erro ao ${acao} agente`, severity: 'error' });
    }
  };

  const getStatusChip = (agente: Agente) => {
    if (agente.bloqueado) {
      return <Chip label="Bloqueado" color="error" size="small" />;
    }
    if (agente.ativo) {
      return <Chip label="Ativo" color="success" size="small" />;
    }
    return <Chip label="Inativo" color="default" size="small" />;
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Controle de Agentes do Setor Elétrico
      </Typography>

      <Alert severity="warning" sx={{ mb: 3 }}>
        <strong>Atenção:</strong> Bloqueios de agentes impedem o envio de dados ao sistema. Use com cautela!
      </Alert>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Agentes Cadastrados
          </Typography>
          {agentes.length === 0 ? (
            <Alert severity="info">Nenhum agente cadastrado</Alert>
          ) : (
            <TableContainer component={Paper}>
              <Table>
                <TableHead>
                  <TableRow>
                    <TableCell>Código</TableCell>
                    <TableCell>Nome do Agente</TableCell>
                    <TableCell>Status</TableCell>
                    <TableCell>Último Acesso</TableCell>
                    <TableCell>Motivo Bloqueio</TableCell>
                    <TableCell align="right">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {agentes.map((agente) => (
                    <TableRow key={agente.id}>
                      <TableCell>{agente.codigo}</TableCell>
                      <TableCell>{agente.nome}</TableCell>
                      <TableCell>{getStatusChip(agente)}</TableCell>
                      <TableCell>
                        {agente.ultimoAcesso
                          ? new Date(agente.ultimoAcesso).toLocaleString('pt-BR')
                          : 'Nunca'}
                      </TableCell>
                      <TableCell>{agente.motivoBloqueio || '-'}</TableCell>
                      <TableCell align="right">
                        {agente.bloqueado ? (
                          <IconButton
                            size="small"
                            color="success"
                            onClick={() => handleOpenDialog(agente, 'desbloquear')}
                          >
                            <CheckCircle />
                          </IconButton>
                        ) : (
                          <IconButton
                            size="small"
                            color="error"
                            onClick={() => handleOpenDialog(agente, 'bloquear')}
                          >
                            <Block />
                          </IconButton>
                        )}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </CardContent>
      </Card>

      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
        <DialogTitle>
          {acao === 'bloquear' ? 'Bloquear Agente' : 'Desbloquear Agente'}
        </DialogTitle>
        <DialogContent>
          {selectedAgente && (
            <Typography>
              Confirma {acao === 'bloquear' ? 'o bloqueio' : 'o desbloqueio'} do agente{' '}
              <strong>{selectedAgente.nome}</strong>?
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDialogOpen(false)}>Cancelar</Button>
          <Button onClick={handleConfirm} color={acao === 'bloquear' ? 'error' : 'success'} variant="contained">
            Confirmar
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default ControleAgentes;

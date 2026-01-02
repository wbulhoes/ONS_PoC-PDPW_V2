import React, { useState } from 'react';
import {
  Box,
  Typography,
  Card,
  CardContent,
  TextField,
  Button,
  Alert,
  Snackbar,
  FormControlLabel,
  Checkbox,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from '@mui/material';
import { LockOpen, Warning } from '@mui/icons-material';
import { apiClient } from '../../services/apiClient';

const AberturaDia: React.FC = () => {
  const [dataPdp, setDataPdp] = useState('');
  const [confirmarAbertura, setConfirmarAbertura] = useState(false);
  const [dialogOpen, setDialogOpen] = useState(false);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  const handleOpenDialog = () => {
    if (!dataPdp) {
      setSnackbar({ open: true, message: 'Selecione a data PDP', severity: 'error' });
      return;
    }
    setDialogOpen(true);
  };

  const handleConfirmAbertura = async () => {
    try {
      await apiClient.post('/manutencao/abrir-dia', { dataPdp, confirmar: confirmarAbertura });
      setSnackbar({ open: true, message: 'Dia aberto com sucesso', severity: 'success' });
      setDialogOpen(false);
      setDataPdp('');
      setConfirmarAbertura(false);
    } catch (error: any) {
      setSnackbar({ open: true, message: error.message || 'Erro ao abrir dia', severity: 'error' });
      setDialogOpen(false);
    }
  };

  return (
    <Box sx={{ p: 3 }}>
      <Typography variant="h4" sx={{ mb: 3 }}>
        Abertura de Dia para Coleta
      </Typography>

      <Alert severity="error" icon={<Warning />} sx={{ mb: 3 }}>
        <strong>ATENÇÃO:</strong> Esta operação abre um dia para coleta de dados. Use apenas sob orientação do ONS ou em casos de emergência.
        Esta ação não pode ser desfeita!
      </Alert>

      <Card>
        <CardContent>
          <Typography variant="h6" sx={{ mb: 2 }}>
            Selecione o Dia para Abertura
          </Typography>

          <TextField
            fullWidth
            label="Data PDP"
            type="date"
            value={dataPdp}
            onChange={(e) => setDataPdp(e.target.value)}
            InputLabelProps={{ shrink: true }}
            sx={{ mb: 3 }}
          />

          <FormControlLabel
            control={
              <Checkbox
                checked={confirmarAbertura}
                onChange={(e) => setConfirmarAbertura(e.target.checked)}
              />
            }
            label="Confirmo que desejo abrir este dia para coleta de dados"
          />

          <Box sx={{ mt: 3 }}>
            <Button
              variant="contained"
              color="warning"
              startIcon={<LockOpen />}
              onClick={handleOpenDialog}
              disabled={!dataPdp || !confirmarAbertura}
              fullWidth
            >
              Abrir Dia para Coleta
            </Button>
          </Box>
        </CardContent>
      </Card>

      <Dialog open={dialogOpen} onClose={() => setDialogOpen(false)}>
        <DialogTitle>Confirmar Abertura de Dia</DialogTitle>
        <DialogContent>
          <Typography>
            Você está prestes a abrir o dia <strong>{new Date(dataPdp).toLocaleDateString('pt-BR')}</strong> para coleta de dados.
          </Typography>
          <Typography sx={{ mt: 2, color: 'error.main' }}>
            Esta ação permitirá que empresas enviem dados para este dia. Confirma?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDialogOpen(false)}>Cancelar</Button>
          <Button onClick={handleConfirmAbertura} color="warning" variant="contained">
            Sim, Abrir Dia
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar open={snackbar.open} autoHideDuration={6000} onClose={() => setSnackbar({ ...snackbar, open: false })}>
        <Alert severity={snackbar.severity}>{snackbar.message}</Alert>
      </Snackbar>
    </Box>
  );
};

export default AberturaDia;

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
  Button,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  TextField,
  MenuItem,
  Chip,
  Alert,
  Snackbar,
} from '@mui/material';
import { Add, Edit, Delete, Lock } from '@mui/icons-material';
import api from '../services/api';

interface Usuario {
  id: number;
  nome: string;
  email: string;
  perfil: string;
  status: string;
  ultimoAcesso?: string;
}

const GerenciamentoUsuarios: React.FC = () => {
  const [usuarios, setUsuarios] = useState<Usuario[]>([]);
  const [openDialog, setOpenDialog] = useState(false);
  const [editingUsuario, setEditingUsuario] = useState<Usuario | null>(null);
  const [formData, setFormData] = useState({
    nome: '',
    email: '',
    senha: '',
    perfil: 'USUARIO',
    status: 'ATIVO',
  });
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' as 'success' | 'error' });

  useEffect(() => {
    loadUsuarios();
  }, []);

  const loadUsuarios = async () => {
    try {
      const response = await api.get('/usuarios');
      setUsuarios(response.data);
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao carregar usuários', severity: 'error' });
    }
  };

  const handleOpenDialog = (usuario?: Usuario) => {
    if (usuario) {
      setEditingUsuario(usuario);
      setFormData({
        nome: usuario.nome,
        email: usuario.email,
        senha: '',
        perfil: usuario.perfil,
        status: usuario.status,
      });
    } else {
      setEditingUsuario(null);
      setFormData({
        nome: '',
        email: '',
        senha: '',
        perfil: 'USUARIO',
        status: 'ATIVO',
      });
    }
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingUsuario(null);
  };

  const handleSubmit = async () => {
    try {
      if (editingUsuario) {
        await api.put(`/usuarios/${editingUsuario.id}`, formData);
        setSnackbar({ open: true, message: 'Usuário atualizado com sucesso', severity: 'success' });
      } else {
        await api.post('/usuarios', formData);
        setSnackbar({ open: true, message: 'Usuário criado com sucesso', severity: 'success' });
      }
      handleCloseDialog();
      loadUsuarios();
    } catch (error) {
      setSnackbar({ open: true, message: 'Erro ao salvar usuário', severity: 'error' });
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm('Deseja realmente excluir este usuário?')) {
      try {
        await api.delete(`/usuarios/${id}`);
        setSnackbar({ open: true, message: 'Usuário excluído com sucesso', severity: 'success' });
        loadUsuarios();
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao excluir usuário', severity: 'error' });
      }
    }
  };

  const handleResetPassword = async (id: number) => {
    if (window.confirm('Deseja realmente resetar a senha deste usuário?')) {
      try {
        await api.post(`/usuarios/${id}/reset-senha`);
        setSnackbar({ open: true, message: 'Senha resetada com sucesso', severity: 'success' });
      } catch (error) {
        setSnackbar({ open: true, message: 'Erro ao resetar senha', severity: 'error' });
      }
    }
  };

  const getPerfilColor = (perfil: string) => {
    const colors: any = {
      ADMIN: 'error',
      GESTOR: 'warning',
      USUARIO: 'primary',
      VISUALIZADOR: 'default',
    };
    return colors[perfil] || 'default';
  };

  const getStatusColor = (status: string) => {
    return status === 'ATIVO' ? 'success' : 'default';
  };

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 3 }}>
        <Typography variant="h4">Gerenciamento de Usuários</Typography>
        <Button variant="contained" startIcon={<Add />} onClick={() => handleOpenDialog()}>
          Novo Usuário
        </Button>
      </Box>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Nome</TableCell>
              <TableCell>Email</TableCell>
              <TableCell>Perfil</TableCell>
              <TableCell>Status</TableCell>
              <TableCell>Último Acesso</TableCell>
              <TableCell>Ações</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {usuarios.map((usuario) => (
              <TableRow key={usuario.id}>
                <TableCell>{usuario.nome}</TableCell>
                <TableCell>{usuario.email}</TableCell>
                <TableCell>
                  <Chip label={usuario.perfil} color={getPerfilColor(usuario.perfil)} size="small" />
                </TableCell>
                <TableCell>
                  <Chip label={usuario.status} color={getStatusColor(usuario.status)} size="small" />
                </TableCell>
                <TableCell>
                  {usuario.ultimoAcesso ? new Date(usuario.ultimoAcesso).toLocaleString() : 'Nunca'}
                </TableCell>
                <TableCell>
                  <IconButton onClick={() => handleOpenDialog(usuario)} color="primary" size="small">
                    <Edit />
                  </IconButton>
                  <IconButton onClick={() => handleResetPassword(usuario.id)} color="warning" size="small">
                    <Lock />
                  </IconButton>
                  <IconButton onClick={() => handleDelete(usuario.id)} color="error" size="small">
                    <Delete />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog open={openDialog} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
        <DialogTitle>{editingUsuario ? 'Editar Usuário' : 'Novo Usuário'}</DialogTitle>
        <DialogContent>
          <TextField
            fullWidth
            label="Nome"
            value={formData.nome}
            onChange={(e) => setFormData({ ...formData, nome: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            label="Email"
            type="email"
            value={formData.email}
            onChange={(e) => setFormData({ ...formData, email: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            label={editingUsuario ? 'Nova Senha (deixe em branco para manter)' : 'Senha'}
            type="password"
            value={formData.senha}
            onChange={(e) => setFormData({ ...formData, senha: e.target.value })}
            margin="normal"
          />
          <TextField
            fullWidth
            select
            label="Perfil"
            value={formData.perfil}
            onChange={(e) => setFormData({ ...formData, perfil: e.target.value })}
            margin="normal"
          >
            <MenuItem value="ADMIN">Administrador</MenuItem>
            <MenuItem value="GESTOR">Gestor</MenuItem>
            <MenuItem value="USUARIO">Usuário</MenuItem>
            <MenuItem value="VISUALIZADOR">Visualizador</MenuItem>
          </TextField>
          <TextField
            fullWidth
            select
            label="Status"
            value={formData.status}
            onChange={(e) => setFormData({ ...formData, status: e.target.value })}
            margin="normal"
          >
            <MenuItem value="ATIVO">Ativo</MenuItem>
            <MenuItem value="INATIVO">Inativo</MenuItem>
          </TextField>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog}>Cancelar</Button>
          <Button onClick={handleSubmit} variant="contained">
            Salvar
          </Button>
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

export default GerenciamentoUsuarios;

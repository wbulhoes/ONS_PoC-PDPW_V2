import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { dadosEnergeticosApi, CriarDadoEnergeticoDto } from '../services/api';

const DadosEnergeticosForm = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formData, setFormData] = useState<CriarDadoEnergeticoDto>({
    dataReferencia: new Date().toISOString().split('T')[0],
    codigoUsina: '',
    producaoMWh: 0,
    capacidadeDisponivel: 0,
    status: 'Ativo',
    observacoes: ''
  });

  useEffect(() => {
    if (id) {
      carregarDado(parseInt(id));
    }
  }, [id]);

  const carregarDado = async (dadoId: number) => {
    try {
      setLoading(true);
      const dado = await dadosEnergeticosApi.obterPorId(dadoId);
      setFormData({
        dataReferencia: dado.dataReferencia.split('T')[0],
        codigoUsina: dado.codigoUsina,
        producaoMWh: dado.producaoMWh,
        capacidadeDisponivel: dado.capacidadeDisponivel,
        status: dado.status,
        observacoes: dado.observacoes || ''
      });
    } catch (err) {
      setError('Erro ao carregar dado');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (id) {
        await dadosEnergeticosApi.atualizar(parseInt(id), formData);
      } else {
        await dadosEnergeticosApi.criar(formData);
      }
      navigate('/');
    } catch (err) {
      setError('Erro ao salvar dado');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'producaoMWh' || name === 'capacidadeDisponivel' 
        ? parseFloat(value) || 0 
        : value
    }));
  };

  return (
    <div>
      <h2>{id ? 'Editar' : 'Novo'} Dado Energético</h2>
      {error && <div className="error">{error}</div>}
      
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="dataReferencia">Data de Referência:</label>
          <input
            type="date"
            id="dataReferencia"
            name="dataReferencia"
            value={formData.dataReferencia}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="codigoUsina">Código da Usina:</label>
          <input
            type="text"
            id="codigoUsina"
            name="codigoUsina"
            value={formData.codigoUsina}
            onChange={handleChange}
            required
            maxLength={50}
          />
        </div>

        <div className="form-group">
          <label htmlFor="producaoMWh">Produção (MWh):</label>
          <input
            type="number"
            id="producaoMWh"
            name="producaoMWh"
            value={formData.producaoMWh}
            onChange={handleChange}
            required
            step="0.01"
            min="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="capacidadeDisponivel">Capacidade Disponível:</label>
          <input
            type="number"
            id="capacidadeDisponivel"
            name="capacidadeDisponivel"
            value={formData.capacidadeDisponivel}
            onChange={handleChange}
            required
            step="0.01"
            min="0"
          />
        </div>

        <div className="form-group">
          <label htmlFor="status">Status:</label>
          <input
            type="text"
            id="status"
            name="status"
            value={formData.status}
            onChange={handleChange}
            required
            maxLength={50}
          />
        </div>

        <div className="form-group">
          <label htmlFor="observacoes">Observações:</label>
          <textarea
            id="observacoes"
            name="observacoes"
            value={formData.observacoes}
            onChange={handleChange}
            rows={4}
          />
        </div>

        <div className="form-actions">
          <button type="submit" className="btn-primary" disabled={loading}>
            {loading ? 'Salvando...' : 'Salvar'}
          </button>
          <button 
            type="button" 
            className="btn-secondary" 
            onClick={() => navigate('/')}
            disabled={loading}
          >
            Cancelar
          </button>
        </div>
      </form>
    </div>
  );
};

export default DadosEnergeticosForm;

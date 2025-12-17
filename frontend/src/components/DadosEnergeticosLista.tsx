import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { dadosEnergeticosApi, DadoEnergeticoDto } from '../services/api';

const DadosEnergeticosLista = () => {
  const [dados, setDados] = useState<DadoEnergeticoDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const resultado = await dadosEnergeticosApi.obterTodos();
      setDados(resultado);
      setError(null);
    } catch (err) {
      setError('Erro ao carregar dados energéticos');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleRemover = async (id: number) => {
    if (!confirm('Deseja realmente remover este registro?')) return;

    try {
      await dadosEnergeticosApi.remover(id);
      await carregarDados();
    } catch (err) {
      setError('Erro ao remover registro');
      console.error(err);
    }
  };

  const formatarData = (data: string) => {
    return new Date(data).toLocaleDateString('pt-BR');
  };

  if (loading) {
    return <div className="loading">Carregando dados...</div>;
  }

  return (
    <div>
      <h2>Dados Energéticos</h2>
      {error && <div className="error">{error}</div>}
      
      <table>
        <thead>
          <tr>
            <th>Data Referência</th>
            <th>Código Usina</th>
            <th>Produção (MWh)</th>
            <th>Capacidade</th>
            <th>Status</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {dados.map((dado) => (
            <tr key={dado.id}>
              <td>{formatarData(dado.dataReferencia)}</td>
              <td>{dado.codigoUsina}</td>
              <td>{dado.producaoMWh.toFixed(2)}</td>
              <td>{dado.capacidadeDisponivel.toFixed(2)}</td>
              <td>{dado.status}</td>
              <td>
                <button 
                  onClick={() => navigate(`/editar/${dado.id}`)}
                  className="btn-secondary"
                  style={{ marginRight: '0.5rem' }}
                >
                  Editar
                </button>
                <button 
                  onClick={() => handleRemover(dado.id)}
                  className="btn-danger"
                >
                  Remover
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {dados.length === 0 && (
        <p style={{ textAlign: 'center', marginTop: '2rem', color: '#888' }}>
          Nenhum dado cadastrado
        </p>
      )}
    </div>
  );
};

export default DadosEnergeticosLista;

import React, { useState, useEffect, useMemo } from 'react';
import styles from './Inflexibility.module.css';
import {
  InflexibilidadeData,
  InflexibilidadeForm,
  InflexibilidadeIntervalo,
} from '../../../types/inflexibility';

/**
 * Página de Coleta de Inflexibilidade Térmica
 *
 * Funcionalidades:
 * - Seleção de data PDP
 * - Seleção de empresa
 * - Seleção de usina térmica
 * - Coleta de dados de inflexibilidade por intervalo de 30 min (48 intervalos)
 * - Visualização de dados em tabela dinâmica
 * - Cálculo automático de totais e médias
 * - Validações de limite de envio
 */
const Inflexibility: React.FC = () => {
  // Estados
  const [formData, setFormData] = useState<InflexibilidadeForm>({
    dataPdp: '',
    codEmpresa: '',
    codUsina: '',
    valores: Array(48).fill(0),
  });

  const [datasPdp, setDatasPdp] = useState<string[]>([]);
  const [empresas, setEmpresas] = useState<{ codigo: string; nome: string }[]>([]);
  const [usinas, setUsinas] = useState<string[]>([]);
  const [dadosInflexibilidade, setDadosInflexibilidade] = useState<InflexibilidadeData[]>([]);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);
  const [showTextArea, setShowTextArea] = useState(false);

  // Gerar intervalos de 30 minutos (48 intervalos)
  const intervalos = useMemo(() => {
    const result: string[] = [];
    for (let i = 0; i < 24; i++) {
      const hora = i.toString().padStart(2, '0');
      result.push(`${hora}:00-${hora}:30`);
      result.push(`${hora}:30-${i === 23 ? '00' : (i + 1).toString().padStart(2, '0')}:00`);
    }
    return result;
  }, []);

  // Carregar datas PDP disponíveis
  useEffect(() => {
    const carregarDatasPdp = async () => {
      try {
        setLoading(true);
        // Mock de datas - substituir por chamada real à API
        const hoje = new Date();
        const datas: string[] = [];
        for (let i = -5; i <= 5; i++) {
          const data = new Date(hoje);
          data.setDate(data.getDate() + i);
          datas.push(data.toISOString().split('T')[0]);
        }
        setDatasPdp(datas);
      } catch (error) {
        setMessage({ text: 'Erro ao carregar datas PDP', type: 'error' });
      } finally {
        setLoading(false);
      }
    };

    carregarDatasPdp();
  }, []);

  // Carregar empresas quando data PDP é selecionada
  useEffect(() => {
    if (!formData.dataPdp) return;

    const carregarEmpresas = async () => {
      try {
        setLoading(true);
        // Mock de empresas - substituir por chamada real à API
        const empresasMock = [
          { codigo: 'EMP001', nome: 'Empresa Térmica 1' },
          { codigo: 'EMP002', nome: 'Empresa Térmica 2' },
          { codigo: 'EMP003', nome: 'Empresa Térmica 3' },
        ];
        setEmpresas(empresasMock);
      } catch (error) {
        setMessage({ text: 'Erro ao carregar empresas', type: 'error' });
      } finally {
        setLoading(false);
      }
    };

    carregarEmpresas();
  }, [formData.dataPdp]);

  // Carregar usinas quando empresa é selecionada
  useEffect(() => {
    if (!formData.codEmpresa || !formData.dataPdp) return;

    const carregarUsinas = async () => {
      try {
        setLoading(true);
        // Mock de usinas - substituir por chamada real à API
        const usinasMock = ['USINA001', 'USINA002', 'USINA003', 'USINA004'];
        setUsinas(usinasMock);

        // Carregar dados de inflexibilidade existentes
        await carregarDadosInflexibilidade();
      } catch (error) {
        setMessage({ text: 'Erro ao carregar usinas', type: 'error' });
      } finally {
        setLoading(false);
      }
    };

    carregarUsinas();
  }, [formData.codEmpresa, formData.dataPdp]);

  // Carregar dados de inflexibilidade da empresa/usina
  const carregarDadosInflexibilidade = async () => {
    try {
      setLoading(true);
      // Mock de dados - substituir por chamada real à API
      const dadosMock: InflexibilidadeData[] = [];
      usinas.forEach((usina) => {
        for (let i = 1; i <= 48; i++) {
          dadosMock.push({
            codusina: usina,
            intflexi: i,
            valflexitran: Math.floor(Math.random() * 500),
            datpdp: formData.dataPdp,
          });
        }
      });
      setDadosInflexibilidade(dadosMock);
    } catch (error) {
      setMessage({ text: 'Erro ao carregar dados de inflexibilidade', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  // Handler para mudança de usina
  const handleUsinaChange = (codUsina: string) => {
    setFormData({ ...formData, codUsina });
    setShowTextArea(codUsina !== '');

    if (codUsina === 'TODAS') {
      // Mostrar todas as usinas
      const valoresTodas: number[] = [];
      for (let i = 1; i <= 48; i++) {
        valoresTodas.push(0); // Será preenchido com os dados reais
      }
      setFormData({ ...formData, codUsina, valores: valoresTodas });
    } else if (codUsina !== '') {
      // Carregar valores da usina específica
      const valoresUsina = dadosInflexibilidade
        .filter((d) => d.codusina === codUsina)
        .sort((a, b) => a.intflexi - b.intflexi)
        .map((d) => d.valflexitran);

      setFormData({ ...formData, codUsina, valores: valoresUsina });
    }
  };

  // Handler para mudança de valores no textarea
  const handleValoresChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const texto = event.target.value;
    const linhas = texto.split('\n');
    const valores: number[] = [];

    linhas.forEach((linha) => {
      const valoresLinha = linha.split('\t').filter((v) => v.trim() !== '');
      valoresLinha.forEach((valor) => {
        const num = parseFloat(valor);
        valores.push(isNaN(num) ? 0 : num);
      });
    });

    setFormData({ ...formData, valores });
  };

  // Calcular totais e médias por usina
  const calcularEstatisticas = (codUsina: string) => {
    const dados = dadosInflexibilidade.filter((d) => d.codusina === codUsina);
    const total = dados.reduce((acc, d) => acc + d.valflexitran, 0);
    const media = dados.length > 0 ? Math.floor(total / dados.length) : 0;
    return { total, media };
  };

  // Handler para salvar dados
  const handleSalvar = async () => {
    if (!formData.dataPdp || !formData.codEmpresa || !formData.codUsina) {
      setMessage({ text: 'Preencha todos os campos obrigatórios', type: 'error' });
      return;
    }

    try {
      setLoading(true);

      const intervalos: InflexibilidadeIntervalo[] = formData.valores.map((valor, index) => ({
        intflexi: index + 1,
        valflexitran: valor,
      }));

      // Mock de salvamento - substituir por chamada real à API
      // const response = await api.post('/api/inflexibilidade', {
      //   dataPdp: formData.dataPdp,
      //   codEmpresa: formData.codEmpresa,
      //   codUsina: formData.codUsina,
      //   dados: intervalos,
      // });

      setMessage({ text: 'Dados salvos com sucesso!', type: 'success' });

      // Recarregar dados
      await carregarDadosInflexibilidade();
    } catch (error) {
      setMessage({ text: 'Erro ao salvar dados', type: 'error' });
    } finally {
      setLoading(false);
    }
  };

  // Obter usinas únicas dos dados
  const usinasUnicas = useMemo(() => {
    const codigos = new Set(dadosInflexibilidade.map((d) => d.codusina));
    return Array.from(codigos).sort();
  }, [dadosInflexibilidade]);

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.guideline}>
          <img src="/images/tit_sis_guideline.gif" alt="Guideline" />
        </div>
        <div className={styles.title}>
          <img src="/images/tit_ColInflexibilidade.gif" alt="Coleta de Inflexibilidade" />
        </div>
      </div>

      {message && (
        <div className={`${styles.message} ${styles[message.type]}`} data-testid="message">
          {message.text}
        </div>
      )}

      <form className={styles.form} data-testid="inflexibility-form">
        <div className={styles.formRow}>
          <label htmlFor="dataPdp">
            <strong>Data PDP:</strong>
          </label>
          <select
            id="dataPdp"
            value={formData.dataPdp}
            onChange={(e) => setFormData({ ...formData, dataPdp: e.target.value })}
            className={styles.select}
            data-testid="data-pdp-select"
          >
            <option value="">Selecione</option>
            {datasPdp.map((data) => (
              <option key={data} value={data}>
                {new Date(data).toLocaleDateString('pt-BR')}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="empresa">
            <strong>Empresa:</strong>
          </label>
          <select
            id="empresa"
            value={formData.codEmpresa}
            onChange={(e) => setFormData({ ...formData, codEmpresa: e.target.value })}
            className={styles.select}
            disabled={!formData.dataPdp}
            data-testid="empresa-select"
          >
            <option value="">Selecione</option>
            {empresas.map((empresa) => (
              <option key={empresa.codigo} value={empresa.codigo}>
                {empresa.nome}
              </option>
            ))}
          </select>
        </div>

        <div className={styles.formRow}>
          <label htmlFor="usina">
            <strong>Usinas:</strong>
          </label>
          <select
            id="usina"
            value={formData.codUsina}
            onChange={(e) => handleUsinaChange(e.target.value)}
            className={styles.select}
            disabled={!formData.codEmpresa}
            data-testid="usina-select"
          >
            <option value="">Selecione uma Usina</option>
            {usinas.map((usina) => (
              <option key={usina} value={usina}>
                {usina}
              </option>
            ))}
            {usinas.length > 0 && <option value="TODAS">Todas as Usinas</option>}
          </select>
          {showTextArea && (
            <button
              type="button"
              onClick={handleSalvar}
              className={styles.btnSalvar}
              disabled={loading}
              data-testid="salvar-button"
            >
              <img src="/images/bt_salvar.gif" alt="Salvar" />
            </button>
          )}
        </div>
      </form>

      <div className={styles.tableContainer}>
        <table className={styles.inflexibilityTable} data-testid="inflexibility-table">
          <thead>
            <tr>
              <th className={styles.headerCell}>Intervalo</th>
              <th className={styles.headerCell}>Total</th>
              {usinasUnicas.map((usina) => (
                <th
                  key={usina}
                  className={`${styles.headerCell} ${
                    usina === formData.codUsina ? styles.selectedColumn : ''
                  }`}
                >
                  {usina}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {intervalos.map((intervalo, index) => {
              const totalIntervalo = dadosInflexibilidade
                .filter((d) => d.intflexi === index + 1)
                .reduce((acc, d) => acc + d.valflexitran, 0);

              return (
                <tr key={index}>
                  <td className={styles.intervalCell}>{intervalo}</td>
                  <td className={styles.totalCell}>{totalIntervalo}</td>
                  {usinasUnicas.map((usina) => {
                    const dado = dadosInflexibilidade.find(
                      (d) => d.codusina === usina && d.intflexi === index + 1
                    );
                    return (
                      <td
                        key={usina}
                        className={`${styles.valueCell} ${
                          usina === formData.codUsina ? styles.selectedColumn : ''
                        }`}
                      >
                        {dado?.valflexitran || 0}
                      </td>
                    );
                  })}
                </tr>
              );
            })}
            <tr className={styles.totalRow}>
              <td className={styles.totalLabelCell}>
                <strong>Total</strong>
              </td>
              <td className={styles.totalCell}>
                <strong>{dadosInflexibilidade.reduce((acc, d) => acc + d.valflexitran, 0)}</strong>
              </td>
              {usinasUnicas.map((usina) => {
                const stats = calcularEstatisticas(usina);
                return (
                  <td key={usina} className={styles.totalCell}>
                    <strong>{stats.total}</strong>
                  </td>
                );
              })}
            </tr>
            <tr className={styles.mediaRow}>
              <td className={styles.totalLabelCell}>
                <strong>Média</strong>
              </td>
              <td className={styles.totalCell}>
                <strong>
                  {dadosInflexibilidade.length > 0
                    ? Math.floor(
                        dadosInflexibilidade.reduce((acc, d) => acc + d.valflexitran, 0) / 48
                      )
                    : 0}
                </strong>
              </td>
              {usinasUnicas.map((usina) => {
                const stats = calcularEstatisticas(usina);
                return (
                  <td key={usina} className={styles.totalCell}>
                    <strong>{stats.media}</strong>
                  </td>
                );
              })}
            </tr>
          </tbody>
        </table>

        {showTextArea && formData.codUsina !== '' && (
          <div className={styles.textAreaContainer} data-testid="valores-textarea-container">
            <textarea
              id="txtValor"
              value={formData.valores.join('\n')}
              onChange={handleValoresChange}
              className={styles.valoresTextArea}
              rows={48}
              data-testid="valores-textarea"
            />
          </div>
        )}
      </div>

      {loading && (
        <div className={styles.loading} data-testid="loading">
          Carregando...
        </div>
      )}
    </div>
  );
};

export default Inflexibility;

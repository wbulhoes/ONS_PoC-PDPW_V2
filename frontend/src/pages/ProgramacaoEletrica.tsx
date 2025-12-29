import React, { useState, useEffect } from 'react';
import { cargasService, intercambiosService, balancosService, semanasPmoService } from '../services';
import { CargaDto, IntercambioDto, BalancoDto, SemanaPmoDto } from '../types';
import styles from './ProgramacaoEletrica.module.css';

const ProgramacaoEletrica: React.FC = () => {
  const [semanaSelecionada, setSemanaSelecionada] = useState<number | null>(null);
  const [semanas, setSemanas] = useState<SemanaPmoDto[]>([]);
  const [abaSelecionada, setAbaSelecionada] = useState<'cargas' | 'intercambios' | 'balancos'>('cargas');

  // Cargas
  const [cargas, setCargas] = useState<CargaDto[]>([]);
  const [formCarga, setFormCarga] = useState({
    semanaPmoId: 0,
    subsistema: 'SE',
    dataReferencia: '',
    cargaMWMed: 0,
    cargaMWMax: 0,
    observacoes: '',
  });

  // Intercâmbios
  const [intercambios, setIntercambios] = useState<IntercambioDto[]>([]);
  const [formIntercambio, setFormIntercambio] = useState({
    semanaPmoId: 0,
    subsistemaOrigem: 'SE',
    subsistemaDestino: 'S',
    limiteMaximoMW: 0,
    limiteOperativoMW: 0,
    observacoes: '',
  });

  // Balanços
  const [balancos, setBalancos] = useState<BalancoDto[]>([]);

  useEffect(() => {
    carregarSemanas();
  }, []);

  useEffect(() => {
    if (semanaSelecionada) {
      carregarDados();
    }
  }, [semanaSelecionada, abaSelecionada]);

  const carregarSemanas = async () => {
    try {
      const resultado = await semanasPmoService.obterProximas(8);
      setSemanas(resultado);
      if (resultado.length > 0) {
        setSemanaSelecionada(resultado[0].id);
      }
    } catch (err) {
      console.error('Erro ao carregar semanas:', err);
    }
  };

  const carregarDados = async () => {
    if (!semanaSelecionada) return;

    try {
      switch (abaSelecionada) {
        case 'cargas':
          const cargasData = await cargasService.obterPorSemana(semanaSelecionada);
          setCargas(cargasData);
          break;
        case 'intercambios':
          const intercambiosData = await intercambiosService.obterTodos();
          setIntercambios(intercambiosData.filter((i) => i.semanaPmoId === semanaSelecionada));
          break;
        case 'balancos':
          const balancosData = await balancosService.obterTodos();
          setBalancos(balancosData.filter((b) => b.semanaPmoId === semanaSelecionada));
          break;
      }
    } catch (err) {
      console.error('Erro ao carregar dados:', err);
    }
  };

  const handleSubmitCarga = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await cargasService.criar({ ...formCarga, semanaPmoId: semanaSelecionada! });
      carregarDados();
      resetFormCarga();
    } catch (err) {
      console.error('Erro ao salvar carga:', err);
    }
  };

  const handleSubmitIntercambio = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await intercambiosService.criar({ ...formIntercambio, semanaPmoId: semanaSelecionada! });
      carregarDados();
      resetFormIntercambio();
    } catch (err) {
      console.error('Erro ao salvar intercâmbio:', err);
    }
  };

  const resetFormCarga = () => {
    setFormCarga({
      semanaPmoId: 0,
      subsistema: 'SE',
      dataReferencia: '',
      cargaMWMed: 0,
      cargaMWMax: 0,
      observacoes: '',
    });
  };

  const resetFormIntercambio = () => {
    setFormIntercambio({
      semanaPmoId: 0,
      subsistemaOrigem: 'SE',
      subsistemaDestino: 'S',
      limiteMaximoMW: 0,
      limiteOperativoMW: 0,
      observacoes: '',
    });
  };

  const subsistemas = ['SE', 'S', 'NE', 'N'];

  return (
    <div className={styles.container}>
      <h2>2. Programação Elétrica</h2>

      <div className={styles.semanaSelector}>
        <label>Semana PMO:</label>
        <select value={semanaSelecionada || ''} onChange={(e) => setSemanaSelecionada(Number(e.target.value))}>
          {semanas.map((s) => (
            <option key={s.id} value={s.id}>
              Semana {s.numero}/{s.ano} ({new Date(s.dataInicio).toLocaleDateString()} -{' '}
              {new Date(s.dataFim).toLocaleDateString()})
            </option>
          ))}
        </select>
      </div>

      <div className={styles.tabs}>
        <button
          className={abaSelecionada === 'cargas' ? styles.tabActive : ''}
          onClick={() => setAbaSelecionada('cargas')}
        >
          Cargas
        </button>
        <button
          className={abaSelecionada === 'intercambios' ? styles.tabActive : ''}
          onClick={() => setAbaSelecionada('intercambios')}
        >
          Intercâmbios
        </button>
        <button
          className={abaSelecionada === 'balancos' ? styles.tabActive : ''}
          onClick={() => setAbaSelecionada('balancos')}
        >
          Balanços
        </button>
      </div>

      {abaSelecionada === 'cargas' && (
        <div className={styles.content}>
          <form onSubmit={handleSubmitCarga} className={styles.form}>
            <div className={styles.formRow}>
              <div className={styles.formGroup}>
                <label>Subsistema:</label>
                <select
                  value={formCarga.subsistema}
                  onChange={(e) => setFormCarga({ ...formCarga, subsistema: e.target.value })}
                >
                  {subsistemas.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Data Referência:</label>
                <input
                  type="date"
                  value={formCarga.dataReferencia}
                  onChange={(e) => setFormCarga({ ...formCarga, dataReferencia: e.target.value })}
                  required
                />
              </div>

              <div className={styles.formGroup}>
                <label>Carga Média (MW):</label>
                <input
                  type="number"
                  value={formCarga.cargaMWMed}
                  onChange={(e) => setFormCarga({ ...formCarga, cargaMWMed: parseFloat(e.target.value) })}
                  required
                />
              </div>

              <div className={styles.formGroup}>
                <label>Carga Máxima (MW):</label>
                <input
                  type="number"
                  value={formCarga.cargaMWMax || ''}
                  onChange={(e) => setFormCarga({ ...formCarga, cargaMWMax: parseFloat(e.target.value) || 0 })}
                />
              </div>
            </div>

            <button type="submit" className={styles.btnSalvar}>
              Adicionar Carga
            </button>
          </form>

          <table className={styles.table}>
            <thead>
              <tr>
                <th>Subsistema</th>
                <th>Data</th>
                <th>Carga Média (MW)</th>
                <th>Carga Máxima (MW)</th>
                <th>Observações</th>
              </tr>
            </thead>
            <tbody>
              {cargas.map((carga) => (
                <tr key={carga.id}>
                  <td>{carga.subsistema}</td>
                  <td>{new Date(carga.dataReferencia).toLocaleDateString()}</td>
                  <td>{carga.cargaMWMed.toLocaleString()}</td>
                  <td>{carga.cargaMWMax?.toLocaleString() || '-'}</td>
                  <td>{carga.observacoes}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {abaSelecionada === 'intercambios' && (
        <div className={styles.content}>
          <form onSubmit={handleSubmitIntercambio} className={styles.form}>
            <div className={styles.formRow}>
              <div className={styles.formGroup}>
                <label>Subsistema Origem:</label>
                <select
                  value={formIntercambio.subsistemaOrigem}
                  onChange={(e) => setFormIntercambio({ ...formIntercambio, subsistemaOrigem: e.target.value })}
                >
                  {subsistemas.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Subsistema Destino:</label>
                <select
                  value={formIntercambio.subsistemaDestino}
                  onChange={(e) => setFormIntercambio({ ...formIntercambio, subsistemaDestino: e.target.value })}
                >
                  {subsistemas.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>

              <div className={styles.formGroup}>
                <label>Limite Máximo (MW):</label>
                <input
                  type="number"
                  value={formIntercambio.limiteMaximoMW}
                  onChange={(e) => setFormIntercambio({ ...formIntercambio, limiteMaximoMW: parseFloat(e.target.value) })}
                  required
                />
              </div>

              <div className={styles.formGroup}>
                <label>Limite Operativo (MW):</label>
                <input
                  type="number"
                  value={formIntercambio.limiteOperativoMW}
                  onChange={(e) =>
                    setFormIntercambio({ ...formIntercambio, limiteOperativoMW: parseFloat(e.target.value) })
                  }
                  required
                />
              </div>
            </div>

            <button type="submit" className={styles.btnSalvar}>
              Adicionar Intercâmbio
            </button>
          </form>

          <table className={styles.table}>
            <thead>
              <tr>
                <th>Origem</th>
                <th>Destino</th>
                <th>Limite Máximo (MW)</th>
                <th>Limite Operativo (MW)</th>
                <th>Observações</th>
              </tr>
            </thead>
            <tbody>
              {intercambios.map((inter) => (
                <tr key={inter.id}>
                  <td>{inter.subsistemaOrigem}</td>
                  <td>{inter.subsistemaDestino}</td>
                  <td>{inter.limiteMaximoMW.toLocaleString()}</td>
                  <td>{inter.limiteOperativoMW.toLocaleString()}</td>
                  <td>{inter.observacoes}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {abaSelecionada === 'balancos' && (
        <div className={styles.content}>
          <table className={styles.table}>
            <thead>
              <tr>
                <th>Subsistema</th>
                <th>Geração (MW médio)</th>
                <th>Carga (MW médio)</th>
                <th>Intercâmbio Líquido (MW)</th>
                <th>Observações</th>
              </tr>
            </thead>
            <tbody>
              {balancos.map((balanco) => (
                <tr key={balanco.id}>
                  <td>{balanco.subsistema}</td>
                  <td>{balanco.geracaoMWMed.toLocaleString()}</td>
                  <td>{balanco.cargaMWMed.toLocaleString()}</td>
                  <td>{balanco.intercambioLiquidoMW.toLocaleString()}</td>
                  <td>{balanco.observacoes}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ProgramacaoEletrica;

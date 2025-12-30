import React, { useState, useEffect } from 'react';
import {
  RequisitoArea,
  DataPDPOption,
  RequirementFormState,
  validarFormularioRequisito,
  requisitoParaForm,
  formParaDTO,
  formatarDataPDP,
} from '../../types/requirement';
import styles from './RequirementRegistry.module.css';

interface RequirementRegistryProps {
  onLoadDatas: () => Promise<DataPDPOption[]>;
  onLoadRequisito: (datPdp: string, codArea: string) => Promise<RequisitoArea | null>;
  onSave: (requisito: RequisitoArea) => Promise<void>;
}

const RequirementRegistry: React.FC<RequirementRegistryProps> = ({
  onLoadDatas,
  onLoadRequisito,
  onSave,
}) => {
  // Estados principais
  const [datas, setDatas] = useState<DataPDPOption[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');

  // Estados do formulário
  const [formState, setFormState] = useState<RequirementFormState>({
    datPdp: '',
    valReqMax: '',
    hReqMax: '',
    valResReqMax: '',
    hResReqMax: '',
    errors: {},
  });

  // Carregar datas ao montar
  useEffect(() => {
    loadDatas();
  }, []);

  // Limpar mensagens após 5 segundos
  useEffect(() => {
    if (error || successMessage) {
      const timer = setTimeout(() => {
        setError('');
        setSuccessMessage('');
      }, 5000);
      return () => clearTimeout(timer);
    }
  }, [error, successMessage]);

  /**
   * Carrega datas disponíveis do PDP
   */
  const loadDatas = async () => {
    try {
      setLoading(true);
      setError('');
      const datasCarregadas = await onLoadDatas();
      setDatas(datasCarregadas);
    } catch (err) {
      setError('Não foi possível acessar a Base de Dados.');
      console.error('Erro ao carregar datas:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Carrega requisito quando data é selecionada
   */
  const handleDataChange = async (datPdp: string) => {
    setFormState((prev) => ({
      ...prev,
      datPdp,
      valReqMax: '',
      hReqMax: '',
      valResReqMax: '',
      hResReqMax: '',
      errors: {},
    }));

    if (!datPdp || datPdp === '0') {
      return;
    }

    try {
      setLoading(true);
      setError('');
      const requisito = await onLoadRequisito(datPdp, 'NE');
      setFormState((prev) => ({
        ...prev,
        ...requisitoParaForm(requisito),
        datPdp,
      }));
    } catch (err) {
      setError('Não foi possível acessar a Base de Dados.');
      console.error('Erro ao carregar requisito:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Atualiza campo do formulário
   */
  const updateField = (field: keyof RequirementFormState, value: string) => {
    setFormState((prev) => ({
      ...prev,
      [field]: value,
      errors: {
        ...prev.errors,
        [field]: '',
      },
    }));
  };

  /**
   * Salva requisito
   */
  const handleSalvar = async () => {
    // Validar formulário
    const validacao = validarFormularioRequisito(formState);

    if (!validacao.valido) {
      setFormState((prev) => ({
        ...prev,
        errors: validacao.erros,
      }));

      // Exibir primeira mensagem de erro
      const primeiroErro = Object.values(validacao.erros)[0];
      setError(primeiroErro);
      return;
    }

    try {
      setLoading(true);
      setError('');

      const dto = formParaDTO(formState, 'NE');
      await onSave(dto as RequisitoArea);

      setSuccessMessage('Registro gravado com sucesso!');

      // Limpar formulário
      setFormState({
        datPdp: '',
        valReqMax: '',
        hReqMax: '',
        valResReqMax: '',
        hResReqMax: '',
        errors: {},
      });
    } catch (err: any) {
      setError('Não foi possível gravar o registro.');
      console.error('Erro ao salvar:', err);
    } finally {
      setLoading(false);
    }
  };

  /**
   * Fecha janela (simulação - na web abre em modal/popup)
   */
  const handleFechar = () => {
    if (window.opener) {
      window.close();
    } else {
      // Se não for popup, limpar formulário
      setFormState({
        datPdp: '',
        valReqMax: '',
        hReqMax: '',
        valResReqMax: '',
        hResReqMax: '',
        errors: {},
      });
      setError('');
      setSuccessMessage('');
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.systemTitle}>
          <img
            src="/images/tit_sis_guideline.gif"
            alt="Sistema"
            className={styles.systemImage}
          />
        </div>
        <div className={styles.pageTitle}>
          <img
            src="/images/tit_CadRequisito.gif"
            alt="Cadastro de Requisito"
            className={styles.pageTitleImage}
          />
        </div>
      </div>

      <div className={styles.formContainer}>
        {/* Mensagens */}
        {error && (
          <div className={styles.errorMessage} data-testid="error-message">
            {error}
          </div>
        )}
        {successMessage && (
          <div className={styles.successMessage} data-testid="success-message">
            {successMessage}
          </div>
        )}

        <form className={styles.form} onSubmit={(e) => e.preventDefault()}>
          <table className={styles.formTable}>
            <tbody>
              {/* Data do PDP */}
              <tr>
                <td className={styles.labelCell}>Data do PDP:&nbsp;</td>
                <td className={styles.inputCell}>
                  <select
                    value={formState.datPdp}
                    onChange={(e) => handleDataChange(e.target.value)}
                    disabled={loading}
                    className={styles.selectData}
                    data-testid="data-select"
                  >
                    <option value="0"></option>
                    {datas.map((data) => (
                      <option key={data.datPdp} value={data.datPdp}>
                        {formatarDataPDP(data.datPdp)}
                      </option>
                    ))}
                  </select>
                </td>
              </tr>

              {/* Requisito Máximo */}
              <tr>
                <td className={styles.labelCell}>Requisito Máximo:&nbsp;</td>
                <td className={styles.inputCell}>
                  <input
                    type="text"
                    value={formState.valReqMax}
                    onChange={(e) => updateField('valReqMax', e.target.value)}
                    disabled={loading}
                    className={styles.inputValor}
                    data-testid="val-req-max-input"
                  />
                  &nbsp;MW
                  {formState.errors.valReqMax && (
                    <span className={styles.fieldError}>*</span>
                  )}
                  &nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;
                  <input
                    type="text"
                    value={formState.hReqMax}
                    onChange={(e) => updateField('hReqMax', e.target.value)}
                    placeholder="HH:mm"
                    disabled={loading}
                    className={styles.inputHora}
                    data-testid="h-req-max-input"
                  />
                  &nbsp;HS
                  {formState.errors.hReqMax && <span className={styles.fieldError}>*</span>}
                </td>
              </tr>

              {/* Reserva Mínima */}
              <tr>
                <td className={styles.labelCell}>Reserva Mín. No Req. Máx.:&nbsp;</td>
                <td className={styles.inputCell}>
                  <input
                    type="text"
                    value={formState.valResReqMax}
                    onChange={(e) => updateField('valResReqMax', e.target.value)}
                    disabled={loading}
                    className={styles.inputValor}
                    data-testid="val-res-req-max-input"
                  />
                  &nbsp;MW
                  {formState.errors.valResReqMax && (
                    <span className={styles.fieldError}>*</span>
                  )}
                  &nbsp;&nbsp;/&nbsp;&nbsp;&nbsp;
                  <input
                    type="text"
                    value={formState.hResReqMax}
                    onChange={(e) => updateField('hResReqMax', e.target.value)}
                    placeholder="HH:mm"
                    disabled={loading}
                    className={styles.inputHora}
                    data-testid="h-res-req-max-input"
                  />
                  &nbsp;HS
                  {formState.errors.hResReqMax && <span className={styles.fieldError}>*</span>}
                </td>
              </tr>

              {/* Botões */}
              <tr>
                <td colSpan={2} className={styles.buttonCell}>
                  <button
                    type="button"
                    onClick={handleSalvar}
                    disabled={loading}
                    className={styles.btnSalvar}
                    data-testid="salvar-btn"
                  >
                    <img src="/images/bt_salvar.gif" alt="Salvar" />
                  </button>
                  &nbsp;&nbsp;&nbsp;
                  <button
                    type="button"
                    onClick={handleFechar}
                    disabled={loading}
                    className={styles.btnFechar}
                    data-testid="fechar-btn"
                  >
                    <img src="/images/bt_fechar.gif" alt="Fechar" />
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </form>
      </div>
    </div>
  );
};

export default RequirementRegistry;

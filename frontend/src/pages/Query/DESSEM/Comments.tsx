import React, { useState, useEffect } from 'react';
import styles from './Comments.module.css';
import { useAuthStore } from '../../../store/authStore';

interface Usina {
  id: number;
  nome: string;
}

interface Empresa {
  id: number;
  nome: string;
}

interface Insumo {
  id: number;
  nome: string;
}

interface ComentarioData {
  usinaId: number;
  descricao: string;
  comentarioONS: string;
  energiaVertidaTurbinavel: boolean;
  temSugestao: boolean; // true = Com Sugestão, false = Sem Sugestão
}

const Comments: React.FC = () => {
  const { user } = useAuthStore();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Filters
  const [selectedDate, setSelectedDate] = useState<string>(new Date().toISOString().split('T')[0]);
  const [selectedEmpresa, setSelectedEmpresa] = useState<string>('');
  const [selectedInsumo, setSelectedInsumo] = useState<string>('');

  // Lists Data
  const [semSugestaoList, setSemSugestaoList] = useState<Usina[]>([]);
  const [listaUsinasList, setListaUsinasList] = useState<Usina[]>([]);
  const [comSugestaoList, setComSugestaoList] = useState<Usina[]>([]);

  // Selections
  const [selectedSemSugestao, setSelectedSemSugestao] = useState<number[]>([]);
  const [selectedListaUsinas, setSelectedListaUsinas] = useState<number[]>([]);
  const [selectedComSugestao, setSelectedComSugestao] = useState<number[]>([]);

  // Form Data
  const [formData, setFormData] = useState<ComentarioData>({
    usinaId: 0,
    descricao: '',
    comentarioONS: '',
    energiaVertidaTurbinavel: false,
    temSugestao: false
  });

  // Mock Data Loading
  useEffect(() => {
    // Simulate API call to load initial data
    setLoading(true);
    setTimeout(() => {
      setSemSugestaoList([
        { id: 1, nome: 'Usina A' },
        { id: 2, nome: 'Usina B' }
      ]);
      setListaUsinasList([
        { id: 3, nome: 'Usina C' },
        { id: 4, nome: 'Usina D' }
      ]);
      setComSugestaoList([
        { id: 5, nome: 'Usina E' }
      ]);
      setLoading(false);
    }, 1000);
  }, [selectedDate, selectedEmpresa, selectedInsumo]);

  // Handlers for List Selection
  const handleSelect = (
    id: number, 
    selection: number[], 
    setSelection: React.Dispatch<React.SetStateAction<number[]>>
  ) => {
    if (selection.includes(id)) {
      setSelection(selection.filter(item => item !== id));
    } else {
      setSelection([...selection, id]);
    }
  };

  // Handlers for Moving Items
  const moveItems = (
    sourceList: Usina[],
    setSourceList: React.Dispatch<React.SetStateAction<Usina[]>>,
    targetList: Usina[],
    setTargetList: React.Dispatch<React.SetStateAction<Usina[]>>,
    selection: number[],
    setSelection: React.Dispatch<React.SetStateAction<number[]>>
  ) => {
    const itemsToMove = sourceList.filter(item => selection.includes(item.id));
    const newSourceList = sourceList.filter(item => !selection.includes(item.id));
    const newTargetList = [...targetList, ...itemsToMove];

    setSourceList(newSourceList);
    setTargetList(newTargetList);
    setSelection([]); // Clear selection after move
  };

  // Form Handlers
  const handleFormChange = (field: keyof ComentarioData, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleSave = async () => {
    if (!formData.usinaId) {
      setError('Selecione uma usina.');
      return;
    }
    if (!formData.descricao) {
      setError('Preencha a descrição.');
      return;
    }

    setLoading(true);
    setError(null);
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      alert('Dados salvos com sucesso!');
      // Reset form or refresh data
    } catch (err) {
      setError('Erro ao salvar dados.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.container} data-testid="comments-container">
      {loading && (
        <div className={styles.loadingOverlay} data-testid="loading-overlay">
          Carregando...
        </div>
      )}

      <div className={styles.header}>
        <h1 className={styles.title}>
          <span role="img" aria-label="icon">💬</span> Comentários DESSEM
        </h1>
      </div>

      {error && (
        <div className={styles.errorMessage} data-testid="error-message">
          {error}
        </div>
      )}

      <div className={styles.filters} data-testid="filters-section">
        <div className={styles.formGroup}>
          <label className={styles.label} htmlFor="filter-date">
            <span className={styles.required}>*</span>Data:
          </label>
          <input
            type="date"
            id="filter-date"
            className={styles.select}
            value={selectedDate}
            onChange={(e) => setSelectedDate(e.target.value)}
            data-testid="filter-date"
          />
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label} htmlFor="filter-empresa">
            <span className={styles.required}>*</span>Empresa:
          </label>
          <select
            id="filter-empresa"
            className={styles.select}
            value={selectedEmpresa}
            onChange={(e) => setSelectedEmpresa(e.target.value)}
            data-testid="filter-empresa"
          >
            <option value="">Selecione...</option>
            <option value="1">Empresa A</option>
            <option value="2">Empresa B</option>
          </select>
        </div>

        <div className={styles.formGroup}>
          <label className={styles.label} htmlFor="filter-insumo">
            <span className={styles.required}>*</span>Insumo:
          </label>
          <select
            id="filter-insumo"
            className={styles.select}
            value={selectedInsumo}
            onChange={(e) => setSelectedInsumo(e.target.value)}
            data-testid="filter-insumo"
          >
            <option value="">Selecione...</option>
            <option value="1">Insumo X</option>
            <option value="2">Insumo Y</option>
          </select>
        </div>
      </div>

      <div className={styles.content}>
        {/* Left Column: Analysis Lists */}
        <div className={styles.leftColumn}>
          <h3 className={styles.listTitle}>Análise DESSEM</h3>
          <div className={styles.listsContainer}>
            
            {/* Sem Sugestão */}
            <div className={styles.listColumn}>
              <span className={styles.listTitle}>Sem Sugestão</span>
              <div className={styles.listBox} data-testid="list-sem-sugestao">
                {semSugestaoList.map(item => (
                  <div
                    key={item.id}
                    className={`${styles.listItem} ${selectedSemSugestao.includes(item.id) ? styles.selected : ''}`}
                    onClick={() => handleSelect(item.id, selectedSemSugestao, setSelectedSemSugestao)}
                    data-testid={`item-sem-sugestao-${item.id}`}
                  >
                    {item.nome}
                  </div>
                ))}
              </div>
            </div>

            {/* Buttons 1 */}
            <div className={styles.buttonsColumn}>
              <button
                className={styles.moveButton}
                onClick={() => moveItems(listaUsinasList, setListaUsinasList, semSugestaoList, setSemSugestaoList, selectedListaUsinas, setSelectedListaUsinas)}
                disabled={selectedListaUsinas.length === 0}
                data-testid="btn-move-left-1"
              >
                &lt;&lt;
              </button>
              <button
                className={styles.moveButton}
                onClick={() => moveItems(semSugestaoList, setSemSugestaoList, listaUsinasList, setListaUsinasList, selectedSemSugestao, setSelectedSemSugestao)}
                disabled={selectedSemSugestao.length === 0}
                data-testid="btn-move-right-1"
              >
                &gt;&gt;
              </button>
            </div>

            {/* Lista de Usinas */}
            <div className={styles.listColumn}>
              <span className={styles.listTitle}>Lista de Usinas</span>
              <div className={styles.listBox} data-testid="list-usinas">
                {listaUsinasList.map(item => (
                  <div
                    key={item.id}
                    className={`${styles.listItem} ${selectedListaUsinas.includes(item.id) ? styles.selected : ''}`}
                    onClick={() => handleSelect(item.id, selectedListaUsinas, setSelectedListaUsinas)}
                    data-testid={`item-lista-usinas-${item.id}`}
                  >
                    {item.nome}
                  </div>
                ))}
              </div>
            </div>

            {/* Buttons 2 */}
            <div className={styles.buttonsColumn}>
              <button
                className={styles.moveButton}
                onClick={() => moveItems(listaUsinasList, setListaUsinasList, comSugestaoList, setComSugestaoList, selectedListaUsinas, setSelectedListaUsinas)}
                disabled={selectedListaUsinas.length === 0}
                data-testid="btn-move-right-2"
              >
                &gt;&gt;
              </button>
              <button
                className={styles.moveButton}
                onClick={() => moveItems(comSugestaoList, setComSugestaoList, listaUsinasList, setListaUsinasList, selectedComSugestao, setSelectedComSugestao)}
                disabled={selectedComSugestao.length === 0}
                data-testid="btn-move-left-2"
              >
                &lt;&lt;
              </button>
            </div>

            {/* Com Sugestão */}
            <div className={styles.listColumn}>
              <span className={styles.listTitle}>Com Sugestão</span>
              <div className={styles.listBox} data-testid="list-com-sugestao">
                {comSugestaoList.map(item => (
                  <div
                    key={item.id}
                    className={`${styles.listItem} ${selectedComSugestao.includes(item.id) ? styles.selected : ''}`}
                    onClick={() => handleSelect(item.id, selectedComSugestao, setSelectedComSugestao)}
                    data-testid={`item-com-sugestao-${item.id}`}
                  >
                    {item.nome}
                  </div>
                ))}
              </div>
            </div>

          </div>
        </div>

        {/* Right Column: Details Form */}
        <div className={styles.rightColumn}>
          <div className={styles.formField}>
            <label className={styles.label} htmlFor="form-usina">
              <span className={styles.required}>*</span>Usina:
            </label>
            <select
              id="form-usina"
              className={styles.select}
              value={formData.usinaId}
              onChange={(e) => handleFormChange('usinaId', Number(e.target.value))}
              data-testid="form-usina"
            >
              <option value="0">Selecione...</option>
              {/* Combine all lists for dropdown options or fetch separately */}
              {[...semSugestaoList, ...listaUsinasList, ...comSugestaoList].map(u => (
                <option key={u.id} value={u.id}>{u.nome}</option>
              ))}
            </select>
          </div>

          <div className={styles.checkboxField}>
            <input
              type="checkbox"
              id="form-evt"
              checked={formData.energiaVertidaTurbinavel}
              onChange={(e) => handleFormChange('energiaVertidaTurbinavel', e.target.checked)}
              data-testid="form-evt"
            />
            <label htmlFor="form-evt">Energia Vertida Turbinável</label>
          </div>

          <div className={styles.formField}>
            <label className={styles.label} htmlFor="form-descricao">
              <span className={styles.required}>*</span>Descrição:
            </label>
            <textarea
              id="form-descricao"
              className={styles.textArea}
              rows={4}
              maxLength={200}
              value={formData.descricao}
              onChange={(e) => handleFormChange('descricao', e.target.value)}
              data-testid="form-descricao"
            />
            <span className={styles.charCount}>
              (Até 200 caracteres) {formData.descricao.length}/200
            </span>
          </div>

          <div className={styles.formField}>
            <label className={styles.label} htmlFor="form-comentario-ons">
              <span className={styles.required}>*</span>Comentário ONS:
            </label>
            <textarea
              id="form-comentario-ons"
              className={styles.textArea}
              rows={4}
              maxLength={200}
              value={formData.comentarioONS}
              onChange={(e) => handleFormChange('comentarioONS', e.target.value)}
              data-testid="form-comentario-ons"
            />
            <span className={styles.charCount}>
              (Até 200 caracteres) {formData.comentarioONS.length}/200
            </span>
          </div>

          <div className={styles.actions}>
            <button
              className={styles.saveButton}
              onClick={handleSave}
              disabled={loading}
              data-testid="btn-save"
            >
              💾 Salvar
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Comments;

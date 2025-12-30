import { useState } from 'react';
import styles from './Example.module.css';

export default function Example() {
  const [counter, setCounter] = useState(0);
  const [inputValue, setInputValue] = useState('');
  const [items, setItems] = useState<string[]>([]);

  const handleIncrement = () => {
    setCounter((prev) => prev + 1);
  };

  const handleDecrement = () => {
    setCounter((prev) => prev - 1);
  };

  const handleReset = () => {
    setCounter(0);
  };

  const handleAddItem = () => {
    if (inputValue.trim()) {
      setItems((prev) => [...prev, inputValue]);
      setInputValue('');
    }
  };

  const handleRemoveItem = (index: number) => {
    setItems((prev) => prev.filter((_, i) => i !== index));
  };

  return (
    <div className={styles.container} data-testid="example-container">
      <div className={styles.header} data-testid="example-header">
        <h1 className={styles.title} data-testid="example-title">
          Página de Exemplo
        </h1>
        <p className={styles.subtitle} data-testid="example-subtitle">
          Demonstração de funcionalidades e componentes do sistema
        </p>
      </div>

      <div className={styles.content}>
        <section className={styles.section} data-testid="example-counter-section">
          <h2 className={styles.sectionTitle} data-testid="example-counter-title">
            Contador
          </h2>
          <div className={styles.counterCard} data-testid="example-counter-card">
            <div className={styles.counterDisplay} data-testid="example-counter-display">
              {counter}
            </div>
            <div className={styles.counterButtons}>
              <button
                className={styles.button}
                onClick={handleDecrement}
                data-testid="example-btn-decrement"
              >
                -
              </button>
              <button
                className={styles.button}
                onClick={handleReset}
                data-testid="example-btn-reset"
              >
                Resetar
              </button>
              <button
                className={styles.button}
                onClick={handleIncrement}
                data-testid="example-btn-increment"
              >
                +
              </button>
            </div>
          </div>
        </section>

        <section className={styles.section} data-testid="example-list-section">
          <h2 className={styles.sectionTitle} data-testid="example-list-title">
            Lista de Itens
          </h2>
          <div className={styles.listCard} data-testid="example-list-card">
            <div className={styles.inputGroup}>
              <input
                type="text"
                className={styles.input}
                value={inputValue}
                onChange={(e) => setInputValue(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleAddItem()}
                placeholder="Digite um item..."
                data-testid="example-input-item"
              />
              <button
                className={styles.button}
                onClick={handleAddItem}
                data-testid="example-btn-add-item"
              >
                Adicionar
              </button>
            </div>
            <ul className={styles.itemList} data-testid="example-item-list">
              {items.length === 0 ? (
                <li className={styles.emptyMessage} data-testid="example-empty-message">
                  Nenhum item adicionado
                </li>
              ) : (
                items.map((item, index) => (
                  <li key={index} className={styles.item} data-testid={`example-item-${index}`}>
                    <span data-testid={`example-item-text-${index}`}>{item}</span>
                    <button
                      className={styles.removeButton}
                      onClick={() => handleRemoveItem(index)}
                      data-testid={`example-btn-remove-${index}`}
                    >
                      ×
                    </button>
                  </li>
                ))
              )}
            </ul>
          </div>
        </section>

        <section className={styles.section} data-testid="example-info-section">
          <h2 className={styles.sectionTitle} data-testid="example-info-title">
            Informações
          </h2>
          <div className={styles.infoCard} data-testid="example-info-card">
            <p data-testid="example-info-text-1">
              Esta página demonstra componentes básicos do sistema PDPw.
            </p>
            <p data-testid="example-info-text-2">
              Utilize os exemplos acima para testar funcionalidades interativas.
            </p>
          </div>
        </section>
      </div>
    </div>
  );
}

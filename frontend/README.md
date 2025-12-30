# Frontend

Interface React para o sistema PDPw.

## Estrutura

```
frontend/
├── src/
│   ├── components/     # Componentes reutilizáveis
│   ├── pages/         # Páginas principais
│   ├── services/      # Serviços de API
│   ├── App.tsx        # Componente raiz
│   └── index.tsx      # Entry point
├── public/            # Arquivos estáticos
├── tests/             # Testes
├── package.json       # Dependências
└── tsconfig.json      # Configuração TypeScript
```

## Instalar e Executar

```bash
cd frontend
npm install
npm start
```

A aplicação estará disponível em `http://localhost:3000`

## Executar Testes

**⚠️ OBRIGATÓRIO**: Use sempre o comando abaixo para executar os testes:

```bash
npm test
# OU explicitamente:
npx vitest run tests
```

**Modo watch (desenvolvimento):**
```bash
npm run test:watch
```

## Exemplo de Componente

```typescript
// src/components/DadosHidraulicosTable.tsx
interface DadosHidraulicosTableProps {
  dados: DadosHidraulicos[];
  onSave: (dados: DadosHidraulicos) => Promise<void>;
  onDelete: (id: number) => Promise<void>;
}

export const DadosHidraulicosTable: React.FC<DadosHidraulicosTableProps> = ({
  dados,
  onSave,
  onDelete
}) => {
  return (
    <table>
      {/* renderizar dados */}
    </table>
  );
};
```

## Exemplo de Service API

```typescript
// src/services/dadosHidraulicosService.ts
import { apiClient } from './apiClient';

export const dadosHidraulicosService = {
  async getAll(): Promise<DadosHidraulicos[]> {
    return apiClient.get<DadosHidraulicos[]>('/dados-hidraulicos');
  },

  async getById(id: number): Promise<DadosHidraulicos> {
    return apiClient.get<DadosHidraulicos>(`/dados-hidraulicos/${id}`);
  },

  async create(data: CreateDadosHidraulicosDto): Promise<DadosHidraulicos> {
    return apiClient.post<DadosHidraulicos>('/dados-hidraulicos', data);
  },
};
```

## Convenções

- **Componentes**: PascalCase (ex: `DadosHidraulicosTable.tsx`)
- **Hooks**: Prefixo `use` (ex: `useDadosHidraulicos.ts`)
- **Services**: camelCase (ex: `dadosHidraulicosService.ts`)
- **Props**: Interface com sufixo `Props`
- **Tipos**: TypeScript para tipagem completa

## Estilo Visual

Manter o visual próximo ao das telas WebForms originais:
- Usar tabelas para listagens
- Inputs simples e diretos
- Botões claros de ação
- Feedback visual para ações (loading, sucesso, erro)

## Testes

```bash
npm test
```

Usar Jest + Testing Library para testes unitários e de componentes.

## Build para Produção

```bash
npm run build
```

Gera build otimizado na pasta `build/`

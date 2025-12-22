# ??? GUIA COMPLETO - DEV 3 (Frontend) DIA 1

**Data:** 19/12/2024  
**Respons�vel:** DEV 3  
**Branch:** `feature/frontend-usinas`

---

## ?? �NDICE

1. ? [PARTE 1: Setup e Valida��o (09:00-10:00)](DEV3_CHECKLIST_SETUP.md)
2. ? [PARTE 2: An�lise Tela Legada (10:00-12:00)](DEV3_PARTE2_ANALISE_LEGADO.md)
3. ?? [PARTE 3: Estrutura de Componentes (12:00-13:00)](#parte-3)
4. ??? [ALMO�O (13:00-14:00)](#almoco)
5. ?? [PARTE 4: Componente Listagem (14:00-16:00)](#parte-4)
6. ?? [PARTE 5: Componente Formul�rio (16:00-18:00)](#parte-5)
7. ?? [PARTE 6: Commit e Push (18:00-18:15)](#parte-6)

---

<a name="parte-3"></a>
## ??? PARTE 3: ESTRUTURA DE COMPONENTES (12:00 - 13:00)

**Objetivo:** Criar estrutura de pastas e arquivos base

### PASSO 1: Criar Estrutura de Pastas (10 min)

```powershell
cd C:\temp\_ONS_PoC-PDPW\frontend\src

# Criar estrutura de pastas
mkdir pages\Usinas
mkdir components\Usinas
mkdir components\common
mkdir services\api
mkdir types
mkdir hooks
mkdir utils
```

**Estrutura final:**
```
src/
??? pages/
?   ??? Usinas/
?       ??? UsinasPage.tsx        (criar)
?       ??? UsinasList.tsx        (criar)
?       ??? UsinaForm.tsx         (criar)
??? components/
?   ??? Usinas/
?   ?   ??? UsinaCard.tsx         (criar)
?   ?   ??? UsinaFilters.tsx      (criar)
?   ??? common/
?       ??? Button.tsx            (criar)
?       ??? Input.tsx             (criar)
?       ??? Loading.tsx           (criar)
?       ??? ErrorMessage.tsx      (criar)
??? services/
?   ??? api/
?       ??? usinaService.ts       (criar)
??? types/
?   ??? usina.ts                  (criar)
??? hooks/
?   ??? useUsinas.ts              (criar)
??? utils/
    ??? api.ts                    (configura��o axios)
```

---

### PASSO 2: Criar Types TypeScript (15 min)

**Criar:** `src/types/usina.ts`

```typescript
// src/types/usina.ts

/**
 * Representa��o completa de uma Usina
 */
export interface Usina {
  id: number;
  codigo: string;
  nome: string;
  tipoUsina: string;          // Nome do tipo (ex: "Hidrel�trica")
  tipoUsinaId: number;
  empresa: string;             // Nome da empresa
  empresaId: number;
  capacidadeInstalada: number; // Em MW
  localizacao?: string;
  dataOperacao: string;        // ISO date string
  ativa: boolean;
}

/**
 * DTO para cria��o de nova Usina
 */
export interface CreateUsinaDto {
  codigo: string;
  nome: string;
  tipoUsinaId: number;
  empresaId: number;
  capacidadeInstalada: number;
  localizacao?: string;
  dataOperacao: string;
  ativa: boolean;
}

/**
 * DTO para atualiza��o de Usina
 * (mesmo que Create por enquanto)
 */
export type UpdateUsinaDto = CreateUsinaDto;

/**
 * Filtros para listagem de Usinas
 */
export interface UsinaFilters {
  search?: string;        // Busca em c�digo, nome
  tipoUsinaId?: number;   // Filtro por tipo
  empresaId?: number;     // Filtro por empresa
  ativa?: boolean;        // Filtro por status
}

/**
 * Tipo Usina (para dropdown)
 */
export interface TipoUsina {
  id: number;
  nome: string;
  descricao?: string;
}

/**
 * Empresa (para dropdown)
 */
export interface Empresa {
  id: number;
  nome: string;
  cnpj?: string;
}
```

---

### PASSO 3: Configurar Axios (15 min)

**Criar:** `src/utils/api.ts`

```typescript
// src/utils/api.ts
import axios from 'axios';

/**
 * URL base da API
 * Em desenvolvimento: backend local
 * Em produ��o: vari�vel de ambiente
 */
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

/**
 * Inst�ncia do Axios configurada
 */
export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  timeout: 10000, // 10 segundos
});

/**
 * Interceptor para adicionar token (se houver autentica��o)
 */
api.interceptors.request.use(
  (config) => {
    // Aqui voc� pode adicionar token de autentica��o se necess�rio
    // const token = localStorage.getItem('token');
    // if (token) {
    //   config.headers.Authorization = `Bearer ${token}`;
    // }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

/**
 * Interceptor para tratamento global de erros
 */
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      // Servidor respondeu com erro
      console.error('API Error:', error.response.status, error.response.data);
    } else if (error.request) {
      // Requisi��o foi feita mas sem resposta
      console.error('Network Error:', error.message);
    } else {
      // Erro na configura��o da requisi��o
      console.error('Request Error:', error.message);
    }
    return Promise.reject(error);
  }
);

export default api;
```

---

### PASSO 4: Criar Service de Usinas (20 min)

**Criar:** `src/services/api/usinaService.ts`

```typescript
// src/services/api/usinaService.ts
import { api } from '../../utils/api';
import type { 
  Usina, 
  CreateUsinaDto, 
  UpdateUsinaDto,
  UsinaFilters,
  TipoUsina,
  Empresa
} from '../../types/usina';

/**
 * Service para opera��es de Usinas
 */
export const usinaService = {
  /**
   * Obter todas as usinas
   * @param filters Filtros opcionais
   */
  async getAll(filters?: UsinaFilters): Promise<Usina[]> {
    const params = new URLSearchParams();
    
    if (filters?.search) params.append('search', filters.search);
    if (filters?.tipoUsinaId) params.append('tipoUsinaId', filters.tipoUsinaId.toString());
    if (filters?.empresaId) params.append('empresaId', filters.empresaId.toString());
    if (filters?.ativa !== undefined) params.append('ativa', filters.ativa.toString());

    const response = await api.get<Usina[]>('/usinas', { params });
    return response.data;
  },

  /**
   * Obter usina por ID
   */
  async getById(id: number): Promise<Usina> {
    const response = await api.get<Usina>(`/usinas/${id}`);
    return response.data;
  },

  /**
   * Obter usina por c�digo
   */
  async getByCodigo(codigo: string): Promise<Usina> {
    const response = await api.get<Usina>(`/usinas/codigo/${codigo}`);
    return response.data;
  },

  /**
   * Criar nova usina
   */
  async create(data: CreateUsinaDto): Promise<Usina> {
    const response = await api.post<Usina>('/usinas', data);
    return response.data;
  },

  /**
   * Atualizar usina existente
   */
  async update(id: number, data: UpdateUsinaDto): Promise<void> {
    await api.put(`/usinas/${id}`, data);
  },

  /**
   * Remover usina
   */
  async delete(id: number): Promise<void> {
    await api.delete(`/usinas/${id}`);
  },

  /**
   * Obter tipos de usina (para dropdown)
   */
  async getTipos(): Promise<TipoUsina[]> {
    const response = await api.get<TipoUsina[]>('/tiposusina');
    return response.data;
  },

  /**
   * Obter empresas (para dropdown)
   */
  async getEmpresas(): Promise<Empresa[]> {
    const response = await api.get<Empresa[]>('/empresas');
    return response.data;
  },
};

export default usinaService;
```

---

## ? RESULTADO DA PARTE 3

Ao final de 1 hora, voc� deve ter:

```
? Estrutura de pastas criada
? Types TypeScript definidos (usina.ts)
? Axios configurado (api.ts)
? Service de Usinas criado (usinaService.ts)
? Base pronta para criar componentes
```

---

<a name="almoco"></a>
## ??? ALMO�O (13:00 - 14:00)

**1 hora de pausa**

---

<a name="parte-4"></a>
## ?? PARTE 4: COMPONENTE LISTAGEM (14:00 - 16:00)

**Objetivo:** Criar listagem funcional de usinas

### PASSO 1: Componente de Loading (15 min)

**Criar:** `src/components/common/Loading.tsx`

```typescript
// src/components/common/Loading.tsx
import React from 'react';

interface LoadingProps {
  message?: string;
}

export const Loading: React.FC<LoadingProps> = ({ message = 'Carregando...' }) => {
  return (
    <div className="flex flex-col items-center justify-center p-8">
      <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      <p className="mt-4 text-gray-600">{message}</p>
    </div>
  );
};

export default Loading;
```

---

### PASSO 2: Componente de Card de Usina (30 min)

**Criar:** `src/components/Usinas/UsinaCard.tsx`

```typescript
// src/components/Usinas/UsinaCard.tsx
import React from 'react';
import type { Usina } from '../../types/usina';

interface UsinaCardProps {
  usina: Usina;
  onEdit: (usina: Usina) => void;
  onDelete: (id: number) => void;
}

export const UsinaCard: React.FC<UsinaCardProps> = ({ usina, onEdit, onDelete }) => {
  const handleDelete = () => {
    if (window.confirm(`Deseja realmente excluir a usina "${usina.nome}"?`)) {
      onDelete(usina.id);
    }
  };

  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      {/* Header com c�digo e status */}
      <div className="flex justify-between items-start mb-4">
        <div>
          <h3 className="text-xl font-bold text-gray-800">{usina.nome}</h3>
          <p className="text-sm text-gray-500">C�digo: {usina.codigo}</p>
        </div>
        <span 
          className={`px-3 py-1 rounded-full text-sm font-semibold ${
            usina.ativa 
              ? 'bg-green-100 text-green-800' 
              : 'bg-red-100 text-red-800'
          }`}
        >
          {usina.ativa ? 'Ativa' : 'Inativa'}
        </span>
      </div>

      {/* Informa��es principais */}
      <div className="space-y-2 mb-4">
        <div className="flex justify-between">
          <span className="text-gray-600">Tipo:</span>
          <span className="font-medium">{usina.tipoUsina}</span>
        </div>
        <div className="flex justify-between">
          <span className="text-gray-600">Empresa:</span>
          <span className="font-medium">{usina.empresa}</span>
        </div>
        <div className="flex justify-between">
          <span className="text-gray-600">Capacidade:</span>
          <span className="font-medium">{usina.capacidadeInstalada.toFixed(2)} MW</span>
        </div>
        {usina.localizacao && (
          <div className="flex justify-between">
            <span className="text-gray-600">Localiza��o:</span>
            <span className="font-medium">{usina.localizacao}</span>
          </div>
        )}
      </div>

      {/* A��es */}
      <div className="flex gap-2 pt-4 border-t">
        <button
          onClick={() => onEdit(usina)}
          className="flex-1 bg-blue-500 hover:bg-blue-600 text-white py-2 px-4 rounded transition-colors"
        >
          Editar
        </button>
        <button
          onClick={handleDelete}
          className="flex-1 bg-red-500 hover:bg-red-600 text-white py-2 px-4 rounded transition-colors"
        >
          Excluir
        </button>
      </div>
    </div>
  );
};

export default UsinaCard;
```

---

### PASSO 3: Hook Customizado useUsinas (20 min)

**Criar:** `src/hooks/useUsinas.ts`

```typescript
// src/hooks/useUsinas.ts
import { useState, useEffect } from 'react';
import { usinaService } from '../services/api/usinaService';
import type { Usina, UsinaFilters } from '../types/usina';

export const useUsinas = (initialFilters?: UsinaFilters) => {
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<UsinaFilters>(initialFilters || {});

  const loadUsinas = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const data = await usinaService.getAll(filters);
      setUsinas(data);
    } catch (err) {
      console.error('Erro ao carregar usinas:', err);
      setError('Erro ao carregar usinas. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const deleteUsina = async (id: number) => {
    try {
      await usinaService.delete(id);
      await loadUsinas(); // Recarregar lista ap�s deletar
    } catch (err) {
      console.error('Erro ao deletar usina:', err);
      setError('Erro ao deletar usina. Tente novamente.');
      throw err;
    }
  };

  useEffect(() => {
    loadUsinas();
  }, [filters]); // Recarregar quando filtros mudarem

  return {
    usinas,
    loading,
    error,
    filters,
    setFilters,
    loadUsinas,
    deleteUsina,
  };
};

export default useUsinas;
```

---

### PASSO 4: Componente de Listagem (35 min)

**Criar:** `src/pages/Usinas/UsinasList.tsx`

```typescript
// src/pages/Usinas/UsinasList.tsx
import React from 'react';
import { useUsinas } from '../../hooks/useUsinas';
import { UsinaCard } from '../../components/Usinas/UsinaCard';
import { Loading } from '../../components/common/Loading';
import type { Usina } from '../../types/usina';

interface UsinasListProps {
  onEdit: (usina: Usina) => void;
  onCreate: () => void;
}

export const UsinasList: React.FC<UsinasListProps> = ({ onEdit, onCreate }) => {
  const { usinas, loading, error, deleteUsina } = useUsinas();

  if (loading) {
    return <Loading message="Carregando usinas..." />;
  }

  if (error) {
    return (
      <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
        <p className="font-bold">Erro</p>
        <p>{error}</p>
        <button 
          onClick={() => window.location.reload()}
          className="mt-2 text-sm underline"
        >
          Tentar novamente
        </button>
      </div>
    );
  }

  if (usinas.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-gray-500 text-lg mb-4">Nenhuma usina cadastrada</p>
        <button
          onClick={onCreate}
          className="bg-blue-500 hover:bg-blue-600 text-white py-2 px-6 rounded"
        >
          Cadastrar Primeira Usina
        </button>
      </div>
    );
  }

  return (
    <div>
      {/* Header */}
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold text-gray-800">
          Usinas Cadastradas ({usinas.length})
        </h2>
        <button
          onClick={onCreate}
          className="bg-blue-500 hover:bg-blue-600 text-white py-2 px-6 rounded flex items-center gap-2"
        >
          <span className="text-xl">+</span>
          Nova Usina
        </button>
      </div>

      {/* Grid de Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {usinas.map((usina) => (
          <UsinaCard
            key={usina.id}
            usina={usina}
            onEdit={onEdit}
            onDelete={deleteUsina}
          />
        ))}
      </div>
    </div>
  );
};

export default UsinasList;
```

---

### PASSO 5: P�gina Principal (20 min)

**Criar:** `src/pages/Usinas/UsinasPage.tsx`

```typescript
// src/pages/Usinas/UsinasPage.tsx
import React, { useState } from 'react';
import { UsinasList } from './UsinasList';
import type { Usina } from '../../types/usina';

type PageMode = 'list' | 'create' | 'edit';

export const UsinasPage: React.FC = () => {
  const [mode, setMode] = useState<PageMode>('list');
  const [selectedUsina, setSelectedUsina] = useState<Usina | null>(null);

  const handleCreate = () => {
    setSelectedUsina(null);
    setMode('create');
  };

  const handleEdit = (usina: Usina) => {
    setSelectedUsina(usina);
    setMode('edit');
  };

  const handleBackToList = () => {
    setSelectedUsina(null);
    setMode('list');
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <header className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">
          Gest�o de Usinas
        </h1>
        <p className="text-gray-600 mt-2">
          Cadastro e gerenciamento de usinas geradoras
        </p>
      </header>

      <main>
        {mode === 'list' && (
          <UsinasList 
            onEdit={handleEdit}
            onCreate={handleCreate}
          />
        )}

        {(mode === 'create' || mode === 'edit') && (
          <div className="bg-white rounded-lg shadow-md p-6">
            <div className="mb-4">
              <button
                onClick={handleBackToList}
                className="text-blue-500 hover:text-blue-700 flex items-center gap-2"
              >
                ? Voltar para lista
              </button>
            </div>
            
            <h2 className="text-2xl font-bold mb-6">
              {mode === 'create' ? 'Nova Usina' : 'Editar Usina'}
            </h2>
            
            <p className="text-gray-500">
              Formul�rio ser� implementado na pr�xima etapa (16:00-18:00)
            </p>
          </div>
        )}
      </main>
    </div>
  );
};

export default UsinasPage;
```

---

### PASSO 6: Adicionar Rota (10 min)

**Editar:** `src/App.tsx`

```typescript
// src/App.tsx
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom'
import DadosEnergeticosLista from './components/DadosEnergeticosLista'
import DadosEnergeticosForm from './components/DadosEnergeticosForm'
import { UsinasPage } from './pages/Usinas/UsinasPage' // NOVO
import './App.css'

function App() {
  return (
    <Router>
      <div className="App">
        <header className="App-header">
          <h1>PDPW - Programa��o Di�ria da Produ��o</h1>
          <nav>
            <Link to="/">Dados Energ�ticos</Link>
            <Link to="/usinas">Usinas</Link> {/* NOVO */}
            <Link to="/novo">Novo Registro</Link>
          </nav>
        </header>
        <main className="App-main">
          <Routes>
            <Route path="/" element={<DadosEnergeticosLista />} />
            <Route path="/usinas" element={<UsinasPage />} /> {/* NOVO */}
            <Route path="/novo" element={<DadosEnergeticosForm />} />
            <Route path="/editar/:id" element={<DadosEnergeticosForm />} />
          </Routes>
        </main>
      </div>
    </Router>
  )
}

export default App
```

---

## ? RESULTADO DA PARTE 4

```
? Componente Loading criado
? Componente UsinaCard criado
? Hook useUsinas criado
? Componente UsinasList criado
? P�gina UsinasPage criada
? Rota adicionada no App.tsx
? Listagem funcionando (80% completa)
```

**Testar:**
```powershell
npm run dev
# Acessar: http://localhost:5173/usinas
```

---

<a name="parte-5"></a>
## ?? PARTE 5: COMPONENTE FORMUL�RIO (16:00 - 18:00)

**Ser� implementado se houver tempo, caso contr�rio deixar para DIA 2**

**Por enquanto, formul�rio mostra mensagem:**
> "Formul�rio ser� implementado na pr�xima etapa"

---

<a name="parte-6"></a>
## ?? PARTE 6: COMMIT E PUSH (18:00 - 18:15)

```powershell
cd C:\temp\_ONS_PoC-PDPW

# Ver mudan�as
git status

# Adicionar tudo
git add .

# Commit
git commit -m "[FRONTEND] feat: estrutura inicial tela de Usinas

Implementa��es DIA 1:
- Estrutura de pastas e componentes
- Types TypeScript (Usina, CreateUsinaDto, etc.)
- Configura��o Axios
- Service de Usinas (API calls)
- Hook useUsinas (gerenciamento de estado)
- Componente UsinaCard (card individual)
- Componente UsinasList (listagem completa)
- Componente UsinasPage (p�gina principal)
- Rota /usinas adicionada

Funcionalidades:
? Listagem de usinas
? Loading state
? Error handling
? Empty state
? Navega��o criar/editar (estrutura)
? Exclus�o com confirma��o

Status: Listagem 80% completa
Pr�ximo: Formul�rio (DIA 2)"

# Push
git push origin feature/frontend-usinas
```

---

## ?? RESULTADO FINAL DIA 1 - DEV 3

```
???????????????????????????????????????????
? ENTREGAS DO DIA 1 - DEV 3               ?
???????????????????????????????????????????
? ? Ambiente configurado                  ?
? ? Documenta��o de an�lise completa      ?
? ? Estrutura de componentes criada       ?
? ? Types TypeScript definidos            ?
? ? Service de API implementado           ?
? ? Hook customizado criado               ?
? ? Componente de listagem funcionando    ?
? ? Rota configurada                      ?
? ? Commit e push realizados              ?
?                                         ?
? ?? PROGRESSO: 60% da tela completa      ?
???????????????????????????????????????????
```

---

**Guia criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Status:** ? COMPLETO E PRONTO PARA USO

**DEV 3: Siga este guia passo a passo! ??**

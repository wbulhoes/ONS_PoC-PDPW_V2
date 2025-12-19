# ?? PARTE 2: ANÁLISE DA TELA LEGADA (10:00 - 12:00)

**Tempo:** 2 horas  
**Objetivo:** Entender funcionalidades da tela antiga para replicar no React

---

## ?? OBJETIVO

Analisar a tela antiga de cadastro de usinas (`pdpw_act/pdpw/frmCadUsina.aspx`) para:
1. Mapear todos os campos
2. Entender validações
3. Identificar funcionalidades principais
4. Definir estrutura dos componentes React

---

## ?? LOCALIZAR ARQUIVOS LEGADOS

```powershell
cd C:\temp\_ONS_PoC-PDPW

# Verificar se pasta legada existe
dir pdpw_act\pdpw

# Arquivos principais:
# - frmCadUsina.aspx (interface)
# - frmCadUsina.aspx.cs (código behind - se existir)
```

---

## ?? PASSO 1: MAPEAR CAMPOS DO FORMULÁRIO (30 min)

### Abrir e Analisar frmCadUsina.aspx

**Buscar por:**
- `<asp:TextBox>` ? Campos de texto
- `<asp:DropDownList>` ? Seletores
- `<asp:CheckBox>` ? Checkboxes
- `<asp:Button>` ? Botões
- `<asp:GridView>` ? Tabelas/Listas

### Criar Documento de Mapeamento

**Criar:** `docs/ANALISE_TELA_USINAS_LEGADA.md`

```markdown
# Análise: Tela de Cadastro de Usinas (Legado)

## Campos do Formulário

| Campo | Tipo | Obrigatório | Validação | Observações |
|-------|------|-------------|-----------|-------------|
| Código | TextBox | ? Sim | Alfanumérico, max 10 | Único |
| Nome | TextBox | ? Sim | Texto, max 100 | - |
| Tipo Usina | DropDown | ? Sim | FK TipoUsina | Lista fixa |
| Empresa | DropDown | ? Sim | FK Empresa | Lista do BD |
| Capacidade (MW) | TextBox | ? Sim | Numérico, > 0 | Decimal |
| Localização | TextBox | ? Não | Texto, max 200 | Opcional |
| Data Operação | DatePicker | ? Sim | Data válida | Não pode ser futura |
| Ativa | CheckBox | ? Sim | Boolean | Default: true |

## Funcionalidades

### Listagem
- [ ] Tabela com todas as usinas
- [ ] Colunas: Código, Nome, Tipo, Empresa, Capacidade, Ativa
- [ ] Paginação (10, 25, 50 por página)
- [ ] Ordenação por coluna
- [ ] Filtros: Nome, Tipo, Empresa, Status (Ativa/Inativa)
- [ ] Busca rápida (pesquisa em múltiplos campos)

### Ações
- [ ] Novo: Abre formulário em branco
- [ ] Editar: Abre formulário preenchido
- [ ] Excluir: Confirma e remove (soft delete?)
- [ ] Visualizar: Modo leitura

### Validações
1. Código não pode ser duplicado
2. Nome é obrigatório (min 3 caracteres)
3. Capacidade deve ser > 0
4. Data operação não pode ser futura
5. Ao editar, campos obrigatórios devem estar preenchidos

### Mensagens
- Sucesso: "Usina cadastrada com sucesso!"
- Erro: "Erro ao cadastrar usina: [mensagem]"
- Confirmação exclusão: "Deseja realmente excluir esta usina?"
- Validação: "Campo [X] é obrigatório"

## Fluxo de Navegação

```
Lista de Usinas
    |
    ?? [Novo] ? Formulário Vazio ? [Salvar] ? Lista
    |               |
    |               ?? [Cancelar] ? Lista
    |
    ?? [Editar] ? Formulário Preenchido ? [Salvar] ? Lista
    |               |
    |               ?? [Cancelar] ? Lista
    |
    ?? [Excluir] ? Confirmação ? Lista
```

## Notas Técnicas

- ASP.NET WebForms (arquitetura antiga)
- ViewState usado (não aplicável no React)
- Postback completo (React usa SPA)
- Validação mista: client-side (JS) + server-side (C#)

## Decisões para React

1. **Single Page Application (SPA)**
   - Sem postbacks
   - Navegação via React Router

2. **Componentes Separados**
   - UsinasList (listagem)
   - UsinaForm (formulário)
   - UsinaFilters (filtros)

3. **Estado**
   - React useState para formulário
   - React Query ou Context para cache de dados

4. **Validação**
   - Client-side com Zod ou Yup
   - Feedback em tempo real
   - Server-side via API (backend valida)

5. **UI/UX Melhorias**
   - Design moderno
   - Responsivo (mobile-friendly)
   - Mensagens mais claras
   - Loading states
   - Error boundaries
```

---

## ?? PASSO 2: IDENTIFICAR DEPENDÊNCIAS (30 min)

### APIs Necessárias

**Anotar quais endpoints serão usados:**

1. **Usinas**
   - `GET /api/usinas` - Listar todas
   - `GET /api/usinas/{id}` - Obter por ID
   - `GET /api/usinas/codigo/{codigo}` - Buscar por código
   - `POST /api/usinas` - Criar nova
   - `PUT /api/usinas/{id}` - Atualizar
   - `DELETE /api/usinas/{id}` - Remover

2. **Tipos de Usina** (para dropdown)
   - `GET /api/tiposusina` - Listar todos

3. **Empresas** (para dropdown)
   - `GET /api/empresas` - Listar todas

**Criar:** `docs/APIS_NECESSARIAS_FRONTEND.md`

```markdown
# APIs Necessárias para Tela de Usinas

## Status de Implementação

| Endpoint | Método | Status | Dev Responsável | ETA |
|----------|--------|--------|-----------------|-----|
| `/api/usinas` | GET | ?? Em andamento | DEV 1 | Hoje 13h |
| `/api/usinas/{id}` | GET | ?? Em andamento | DEV 1 | Hoje 13h |
| `/api/usinas` | POST | ?? Em andamento | DEV 1 | Hoje 13h |
| `/api/usinas/{id}` | PUT | ?? Em andamento | DEV 1 | Hoje 13h |
| `/api/usinas/{id}` | DELETE | ?? Em andamento | DEV 1 | Hoje 13h |
| `/api/tiposusina` | GET | ?? Em andamento | DEV 1 | Hoje 16h |
| `/api/empresas` | GET | ?? Em andamento | DEV 1 | Hoje 16h |

## Mocks para Desenvolvimento

Enquanto APIs não estão prontas, usar dados mockados:

```typescript
// src/mocks/usinasMock.ts
export const usinasMock = [
  {
    id: 1,
    codigo: 'ITU001',
    nome: 'Usina de Itaipu',
    tipoUsina: 'Hidrelétrica',
    empresa: 'Itaipu Binacional',
    capacidadeInstalada: 14000,
    ativa: true
  },
  // ... mais dados
];
```

## Plano B

Se APIs não estiverem prontas:
1. Desenvolver com mocks
2. Criar interface (TypeScript) compatível com API esperada
3. Quando API estiver pronta, trocar mock por chamada real
```

---

## ?? PASSO 3: DEFINIR COMPONENTES REACT (30 min)

### Estrutura de Componentes

**Criar diagrama mental/documento:**

```
pages/Usinas/
??? UsinasPage.tsx          (Página principal - container)
?   ??? UsinasList.tsx      (Listagem de usinas)
?   ?   ??? UsinaCard.tsx   (Card individual na lista)
?   ?   ??? UsinaFilters.tsx (Filtros de busca)
?   ??? UsinaForm.tsx       (Formulário criar/editar)
?       ??? UsinaFormFields.tsx (Campos do formulário)

components/common/
??? Button.tsx              (Botão reutilizável)
??? Input.tsx               (Input reutilizável)
??? Select.tsx              (Select reutilizável)
??? Modal.tsx               (Modal genérico)
??? Loading.tsx             (Spinner de loading)
??? ErrorMessage.tsx        (Mensagem de erro)
??? SuccessMessage.tsx      (Mensagem de sucesso)

services/
??? usinaService.ts         (API calls para usinas)
??? tipoUsinaService.ts     (API calls para tipos)
??? empresaService.ts       (API calls para empresas)

types/
??? usina.ts                (TypeScript types)
??? tipoUsina.ts
??? empresa.ts

hooks/
??? useUsinas.ts            (Hook customizado para lógica)
??? useForm.ts              (Hook para formulário)
??? useFilters.ts           (Hook para filtros)
```

### Criar Documento de Estrutura

**Criar:** `docs/ESTRUTURA_COMPONENTES_USINAS.md`

```markdown
# Estrutura de Componentes - Tela de Usinas

## Hierarquia

```
UsinasPage (Container Principal)
?
?? useState: mode (list | create | edit)
?? useState: selectedUsina
?
?? {mode === 'list' && (
?   ?? UsinasList
?       ?? useUsinas() hook
?       ?? useState: filters
?       ?
?       ?? UsinaFilters
?       ?   ?? onChange ? setFilters
?       ?
?       ?? List
?           ?? UsinaCard (cada usina)
?               ?? onEdit ? setMode('edit')
?               ?? onDelete ? confirm ? delete
?  )}
?
?? {(mode === 'create' || mode === 'edit') && (
    ?? UsinaForm
        ?? useForm() hook
        ?? initialValues (se edit)
        ?
        ?? UsinaFormFields
        ?   ?? Input: codigo
        ?   ?? Input: nome
        ?   ?? Select: tipoUsina
        ?   ?? Select: empresa
        ?   ?? Input: capacidade
        ?   ?? Input: localizacao
        ?   ?? DatePicker: dataOperacao
        ?   ?? Checkbox: ativa
        ?
        ?? onSubmit ? save
        ?? onCancel ? setMode('list')
   )}
```

## Props dos Componentes

### UsinasPage
```typescript
// Sem props, é o container principal
```

### UsinasList
```typescript
interface UsinasListProps {
  onEdit: (usina: Usina) => void;
  onDelete: (id: number) => void;
  onCreate: () => void;
}
```

### UsinaCard
```typescript
interface UsinaCardProps {
  usina: Usina;
  onEdit: () => void;
  onDelete: () => void;
}
```

### UsinaForm
```typescript
interface UsinaFormProps {
  mode: 'create' | 'edit';
  initialValues?: Usina;
  onSubmit: (data: CreateUsinaDto) => Promise<void>;
  onCancel: () => void;
}
```

### UsinaFilters
```typescript
interface UsinaFiltersProps {
  filters: UsinaFilters;
  onChange: (filters: UsinaFilters) => void;
}
```

## Estados

### UsinasPage
```typescript
const [mode, setMode] = useState<'list' | 'create' | 'edit'>('list');
const [selectedUsina, setSelectedUsina] = useState<Usina | null>(null);
```

### UsinasList
```typescript
const [usinas, setUsinas] = useState<Usina[]>([]);
const [loading, setLoading] = useState(false);
const [filters, setFilters] = useState<UsinaFilters>({
  search: '',
  tipoUsinaId: null,
  empresaId: null,
  ativa: null
});
```

### UsinaForm
```typescript
const [formData, setFormData] = useState<CreateUsinaDto>({
  codigo: '',
  nome: '',
  tipoUsinaId: 0,
  empresaId: 0,
  capacidadeInstalada: 0,
  localizacao: '',
  dataOperacao: new Date().toISOString(),
  ativa: true
});
const [errors, setErrors] = useState<Record<string, string>>({});
const [submitting, setSubmitting] = useState(false);
```
```

---

## ?? PASSO 4: DEFINIR FLUXO DE DADOS (30 min)

### Criar Diagrama de Fluxo

**Criar:** `docs/FLUXO_DADOS_USINAS.md`

```markdown
# Fluxo de Dados - Tela de Usinas

## Carregamento Inicial

```
UsinasPage montado
    ?
UsinasList montado
    ?
useEffect(() => loadUsinas(), [])
    ?
usinaService.getAll()
    ?
GET /api/usinas
    ?
Backend retorna dados
    ?
setUsinas(data)
    ?
Renderiza lista de UsinaCard
```

## Criação de Nova Usina

```
Usuário clica [Novo]
    ?
setMode('create')
    ?
UsinaForm renderizado (modo create)
    ?
Usuário preenche campos
    ?
Usuário clica [Salvar]
    ?
handleSubmit(formData)
    ?
Validações client-side
    ?
usinaService.create(formData)
    ?
POST /api/usinas
    ?
Backend valida e salva
    ?
Backend retorna usina criada
    ?
Mostra mensagem sucesso
    ?
setMode('list')
    ?
loadUsinas() (atualiza lista)
```

## Edição de Usina

```
Usuário clica [Editar] em UsinaCard
    ?
onEdit(usina)
    ?
setSelectedUsina(usina)
setMode('edit')
    ?
UsinaForm renderizado (modo edit, initialValues=usina)
    ?
Usuário edita campos
    ?
Usuário clica [Salvar]
    ?
handleSubmit(formData)
    ?
Validações
    ?
usinaService.update(usina.id, formData)
    ?
PUT /api/usinas/{id}
    ?
Backend valida e atualiza
    ?
Mostra mensagem sucesso
    ?
setMode('list')
    ?
loadUsinas()
```

## Exclusão de Usina

```
Usuário clica [Excluir] em UsinaCard
    ?
Mostra confirmação
    ?
Usuário confirma
    ?
usinaService.delete(usina.id)
    ?
DELETE /api/usinas/{id}
    ?
Backend remove (soft delete?)
    ?
Mostra mensagem sucesso
    ?
loadUsinas() (atualiza lista)
```

## Filtros

```
Usuário digita em filtro de busca
    ?
onChange ? setFilters({ ...filters, search: value })
    ?
useEffect(() => loadUsinas(), [filters])
    ?
usinaService.getAll(filters)
    ?
GET /api/usinas?search=X&tipoUsinaId=Y
    ?
Backend filtra e retorna
    ?
setUsinas(data)
    ?
Lista atualizada
```

## Tratamento de Erros

```
Erro em requisição
    ?
catch (error)
    ?
Identificar tipo de erro:
    ?? 400: Validação ? Mostrar erros nos campos
    ?? 401: Não autorizado ? Redirecionar login
    ?? 404: Não encontrado ? Mensagem específica
    ?? 500: Erro servidor ? Mensagem genérica
    ?? Network: Sem conexão ? Mensagem de rede
    ?
Mostrar ErrorMessage
```
```

---

## ?? DELIVERABLE DA PARTE 2

Ao final das 2 horas, você deve ter:

### Documentos Criados:
- [ ] `docs/ANALISE_TELA_USINAS_LEGADA.md`
- [ ] `docs/APIS_NECESSARIAS_FRONTEND.md`
- [ ] `docs/ESTRUTURA_COMPONENTES_USINAS.md`
- [ ] `docs/FLUXO_DADOS_USINAS.md`

### Conhecimento Adquirido:
- [ ] Entende todos os campos do formulário
- [ ] Conhece as validações necessárias
- [ ] Sabe quais APIs serão usadas
- [ ] Tem estrutura de componentes definida
- [ ] Entende fluxo de dados completo

---

## ?? PRÓXIMO PASSO

**Após completar análise:**

? **??? ALMOÇO (12:00 - 13:00)**

? **Depois: PARTE 3 - Estrutura de Componentes**

---

**Tempo estimado:** 2 horas  
**Status:** ? Preparado para execução  
**Resultado:** Documentação completa para guiar desenvolvimento

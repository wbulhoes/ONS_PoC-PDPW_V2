# Integração Backend - Rota /admin/usuarios

## Visão Geral

A rota `/admin/usuarios` foi conectada ao backend rodando em `localhost:5001/api`.

## Arquivos Modificados

### 1. `/src/services/userService.ts`
- **Alteração**: Substituído implementação mock por chamadas reais à API
- **Endpoints utilizados**:
  - `GET /api/usuarios` - Lista todos os usuários
  - `GET /api/usuarios?page={page}&pageSize={pageSize}&login={login}&nome={nome}&email={email}&telefone={telefone}` - Lista com paginação e filtros
  - `GET /api/usuarios/{id}` - Busca usuário por ID
  - `POST /api/usuarios` - Cria novo usuário
  - `PUT /api/usuarios/{id}` - Atualiza usuário
  - `DELETE /api/usuarios/{id}` - Exclui usuário
  - `POST /api/usuarios/delete-multiple` - Exclui múltiplos usuários

### 2. `/src/pages/Administration/UserRegistryContainer.tsx`
- **Criado**: Container para conectar o componente UserRegistry com o userService
- **Responsabilidade**: Gerenciar as chamadas ao backend e passar para o componente de apresentação

### 3. `/src/App.tsx`
- **Alteração**: Rota `/admin/usuarios` agora usa `UserRegistryContainer` em vez de `UserRegistry`

## Estrutura de Dados

### Request - Criar Usuário (POST /api/usuarios)
```json
{
  "usuar_id": "string",      // Login (max 8 caracteres)
  "usuar_nome": "string",    // Nome (max 40 caracteres)
  "usuar_email": "string",   // E-mail (max 40 caracteres)
  "usuar_telefone": "string" // Telefone (max 20 caracteres)
}
```

### Request - Atualizar Usuário (PUT /api/usuarios/{id})
```json
{
  "usuar_id": "string",
  "usuar_nome": "string",
  "usuar_email": "string",
  "usuar_telefone": "string"
}
```

### Request - Excluir Múltiplos (POST /api/usuarios/delete-multiple)
```json
{
  "ids": ["string", "string", ...]
}
```

### Response - Lista de Usuários
```json
{
  "sucesso": true,
  "mensagem": "string (opcional)",
  "usuarios": [
    {
      "usuar_id": "string",
      "usuar_nome": "string",
      "usuar_email": "string",
      "usuar_telefone": "string"
    }
  ],
  "total": 0
}
```

### Response - Operação de Usuário
```json
{
  "sucesso": true,
  "mensagem": "string",
  "usuario": {
    "usuar_id": "string",
    "usuar_nome": "string",
    "usuar_email": "string",
    "usuar_telefone": "string"
  }
}
```

## Configuração

A URL base da API é configurada em `.env.development`:

```env
VITE_API_BASE_URL=http://localhost:5001/api
```

## Cliente HTTP

O serviço utiliza o `apiClient` (`/src/services/apiClient.ts`) que:
- Adiciona headers automaticamente (`Content-Type: application/json`)
- Trata erros de forma consistente
- Retorna promises tipadas

## Tratamento de Erros

Todos os métodos do `userService` tratam erros e retornam objetos com `sucesso: false` em caso de falha:

```typescript
{
  sucesso: false,
  mensagem: "Mensagem de erro",
  usuarios: [], // no caso de listagem
  total: 0      // no caso de listagem
}
```

## Funcionalidades da Tela

1. **Listagem**: Paginada (4 registros por página)
2. **Filtros**: Por login, nome, email e telefone
3. **Criação**: Formulário com validação
4. **Edição**: Seleção única para edição
5. **Exclusão**: Seleção múltipla com confirmação

## Testando a Integração

### Pré-requisitos
1. Backend rodando em `localhost:5001`
2. Endpoint `/api/usuarios` implementado

### Como testar
1. Acessar `http://localhost:5173/admin/usuarios`
2. Clicar em "Pesquisar" para carregar usuários
3. Testar criação, edição e exclusão

### Verificação de Requisições
Abrir Developer Tools (F12) > Network para ver as requisições HTTP sendo feitas.

## Próximos Passos

### Backend Necessário
O backend deve implementar os seguintes endpoints:

1. **GET /api/usuarios**
   - Query params: `page`, `pageSize`, `login`, `nome`, `email`, `telefone`
   - Response: `UserListResponse`

2. **GET /api/usuarios/{id}**
   - Response: `User`

3. **POST /api/usuarios**
   - Body: `UserFormData`
   - Response: `User`

4. **PUT /api/usuarios/{id}**
   - Body: `UserFormData`
   - Response: `User`

5. **DELETE /api/usuarios/{id}**
   - Response: 204 No Content

6. **POST /api/usuarios/delete-multiple**
   - Body: `{ ids: string[] }`
   - Response: 204 No Content

### Autenticação
Adicionar token JWT no header quando implementado:
```typescript
headers: {
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
}
```

## Compatibilidade

- ✅ Compatível com a tela legado `frmCadUsuario.aspx`
- ✅ Mantém comportamento de paginação (4 registros por página)
- ✅ Mantém validações do formulário
- ✅ Mantém mensagens de sucesso/erro

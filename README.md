# PDPW - Programação Diária da Produção

## 🎯 Sobre o Projeto

PoC de modernização do sistema legado PDPW para o ONS (Operador Nacional do Sistema Elétrico), migrando de .NET Framework/WebForms/VB.NET para uma arquitetura moderna com .NET 8, React e containerização.

---

## ✅ Decisões da Reunião (17/12/2025)

- Escopo: migrar para .NET 8 (backend) e React (frontend).
- Foco: Backend primeiro; tentar 2 fluxos completos se houver tempo.
- Legado: receberemos repositório em VB.NET (WebForms) e um backup (dump) do banco.
- Autenticação: fora do escopo da PoC (sem login por ora).
- Banco de dados: SQL Server (usar backup/dump legado; sem EF migrations iniciais).
- Contêineres: priorizar backend em contêiner Windows; banco ficará externo (local/VM) devido à indisponibilidade de imagem oficial do SQL Server para contêiner Windows.
- IaC: Docker Compose como diferencial (frontend e backend); SQL Server fora do compose no Windows.
- Repositório: manteremos TFS no cliente durante a PoC; entrega final no GitHub.
- Entregáveis: código, compose, documentação e demonstração funcional até 26/12.

## 🏗️ Arquitetura

### Backend (.NET 8)
- **Arquitetura Limpa (Clean Architecture)** com separação de camadas:
  - **PDPW.Domain**: Entidades e interfaces do domínio
  - **PDPW.Application**: Casos de uso, serviços e DTOs
  - **PDPW.Infrastructure**: Implementação de repositórios e Entity Framework Core
  - **PDPW.API**: Controllers e configuração da API REST

### Frontend (React + TypeScript)
- React 18 com TypeScript
- Vite como bundler
- Axios para comunicação com API
- React Router para navegação

### Containerização
- **Docker Compose** orquestrando:
  - SQL Server 2022
  - Backend API (.NET 8)
  - Frontend (React + Nginx)
- Contêineres Windows para backend (compatibilidade com legado)

## 📁 Estrutura do Projeto

```
_ONS_PoC-PDPW/
├── src/
│   ├── PDPW.Domain/           # Camada de domínio
│   │   ├── Entities/          # Entidades de negócio
│   │   └── Interfaces/        # Contratos de repositórios
│   ├── PDPW.Application/      # Camada de aplicação
│   │   ├── DTOs/              # Data Transfer Objects
│   │   ├── Interfaces/        # Contratos de serviços
│   │   └── Services/          # Lógica de negócio
│   ├── PDPW.Infrastructure/   # Camada de infraestrutura
│   │   ├── Data/              # Contexto do EF Core
│   │   └── Repositories/      # Implementações
│   └── PDPW.API/              # API REST
│       └── Controllers/       # Endpoints
├── frontend/                  # Aplicação React
│   ├── src/
│   │   ├── components/        # Componentes React
│   │   └── services/          # Integração com API
│   └── package.json
├── docker-compose.yml         # Orquestração dos containers
├── Dockerfile.backend         # Build do backend
├── Dockerfile.frontend        # Build do frontend
└── PDPW.sln                   # Solução Visual Studio
```

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- Node.js 20+
- Docker Desktop
- SQL Server (ou usar o container)

### Opção 1: Executar com Docker (Recomendado)

```powershell
# Na raiz do projeto
docker-compose up --build
```

Acessar:
- Frontend: http://localhost:3000
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

### Opção 2: Executar Localmente

#### Backend
```powershell
cd src\PDPW.API

# Restaurar pacotes
dotnet restore

# Criar banco de dados
dotnet ef database update --project ..\PDPW.Infrastructure

# Executar
dotnet run
```

#### Frontend
```powershell
cd frontend

# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm run dev
```

## 🗄️ Banco de Dados

### Criar Migração Inicial
```powershell
cd src\PDPW.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ..\PDPW.API
dotnet ef database update --startup-project ..\PDPW.API
```

### Connection String
Padrão em `appsettings.json`:
```json
"DefaultConnection": "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True"
```

## 📡 API Endpoints

### Dados Energéticos
- `GET /api/dadosenergeticos` - Lista todos
- `GET /api/dadosenergeticos/{id}` - Busca por ID
- `GET /api/dadosenergeticos/periodo?dataInicio=&dataFim=` - Busca por período
- `POST /api/dadosenergeticos` - Criar novo
- `PUT /api/dadosenergeticos/{id}` - Atualizar
- `DELETE /api/dadosenergeticos/{id}` - Remover (soft delete)

### Exemplo de Request (POST)
```json
{
  "dataReferencia": "2025-12-17T00:00:00",
  "codigoUsina": "UHE-001",
  "producaoMWh": 1500.50,
  "capacidadeDisponivel": 2000.00,
  "status": "Ativo",
  "observacoes": "Produção normal"
}
```

## 🎨 Funcionalidades Implementadas

### ✅ Backend
- [x] API REST com .NET 8
- [x] Clean Architecture
- [x] Entity Framework Core com SQL Server
- [x] Repository Pattern
- [x] DTOs e Validações
- [x] Swagger/OpenAPI
- [x] CORS configurado para React

### ✅ Frontend
- [x] Interface de listagem de dados
- [x] Formulário de criação/edição
- [x] Integração com API
- [x] Roteamento com React Router
- [x] Responsividade básica

### ✅ DevOps
- [x] Dockerfile para backend (Windows Container)
- [x] Dockerfile para frontend (Nginx)
- [x] Docker Compose
- [x] .gitignore configurado

## 🔧 Próximos Passos

### Antes da Entrega (26/12/2025)
1. **Integração com sistema legado**
   - Analisar código VB.NET original
   - Migrar lógica de negócio específica
   - Validar cálculos e regras

2. **Melhorias de UI**
   - Adicionar filtros e busca
   - Gráficos de produção
   - Dashboard resumido

3. **Testes**
   - Testes unitários (xUnit)
   - Testes de integração
   - Testes E2E com Playwright

4. **Documentação**
   - Documentar decisões arquiteturais
   - Criar guia de migração completo
   - Preparar apresentação

## 📊 Tecnologias Utilizadas

### Backend
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server 2022
- Swashbuckle (Swagger)

### Frontend
- React 18
- TypeScript 5
- Vite 5
- Axios
- React Router 6

### DevOps
- Docker e Docker Compose
- Contêineres Windows
- Nginx

---

## � Glossário de Termos

Para consultar definições e conceitos técnicos utilizados neste projeto, consulte o [GLOSSARIO.md](GLOSSARIO.md).
Ele contém explicações de termos como PoC, Arquitetura Limpa, DTO, Contêiner, Docker Compose, Vertical Slice e muito mais.

---

## �🔄 Integração com TFS (PoC)

- Fonte legado: será recebido via TFS/TFVC (ou ZIP exportado).
- Estratégia na PoC:
   - Manter TFS como origem do legado para consulta.
   - Migrar o código funcional para esta nova solução .NET 8 (sem tentar converter projeto VB/WebForms in-place).
   - Entrega final: espelhar a PoC no GitHub (repositório público/privado a combinar).
- Sugestão de fluxo:
   1) Baixar o repositório VB/WebForms do TFS em uma pasta separada (`_legado/` fora da solução).
   2) Mapear entidades, regras e consultas SQL necessárias para os 2 fluxos prioritários.
   3) Reimplementar no backend .NET 8 (camadas Domain/Application/Infrastructure).
   4) Replicar telas no React conforme fidelidade necessária.

## 📝 Observações Importantes

- Este é um projeto de **Proof of Concept (PoC)**
- Foco em **vertical slice**: um fluxo completo e funcional
- Prazo de entrega: **26/12/2025**
- Apresentação: **05/01/2026**

## 🤝 Contribuindo

1. Analise o código legado em VB.NET
2. Identifique funcionalidades críticas
3. Implemente usando Clean Architecture
4. Documente decisões técnicas
5. Teste extensivamente

## 📞 Suporte

Para dúvidas sobre o projeto, consulte:
- Documentação do código (comentários inline)
- Swagger da API: http://localhost:5000/swagger
- Issues do repositório

---

**Desenvolvido para ONS - Operador Nacional do Sistema Elétrico**  
**PoC de Modernização PDPW - Dezembro/2025**

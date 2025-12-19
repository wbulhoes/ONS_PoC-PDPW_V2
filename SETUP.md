# Guia de Setup - PDPW PoC

## ⏱️ Início Rápido (10 minutos)

### 1️⃣ Verificar Pré-requisitos
```powershell
# Verificar .NET 8
dotnet --version  # Deve ser 8.x.x

# Verificar Node.js
node --version    # Deve ser 20.x ou superior

# Verificar Docker
docker --version
```

### 2️⃣ Clonar e Navegar
```powershell
cd c:\temp\_ONS_PoC-PDPW
```

### 3️⃣ Executar com Docker (Mais Rápido)
```powershell
# Build e start de todos os serviços
docker-compose up --build

# Aguardar ~2 minutos para inicialização
# Frontend: http://localhost:3000
# Backend: http://localhost:5000/swagger
```

---

## 🔧 Configuração Detalhada (Desenvolvimento Local)

### Backend .NET 8

#### 1. Restaurar Dependências
```powershell
cd src\PDPW.API
dotnet restore
```

#### 2. Configurar Banco de Dados

**Opção A: SQL Server Local**
```powershell
# Atualizar appsettings.json com sua connection string
# Criar banco:
cd ..\PDPW.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ..\PDPW.API
dotnet ef database update --startup-project ..\PDPW.API
```

**Opção B: SQL Server via Docker**
```powershell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Password123" `
  -p 1433:1433 --name pdpw-sql `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Aguardar 30 segundos, depois executar migrations
cd src\PDPW.Infrastructure
dotnet ef database update --startup-project ..\PDPW.API
```

#### 3. Executar API
```powershell
cd ..\PDPW.API
dotnet run

# API rodando em: http://localhost:5000
# Swagger UI: http://localhost:5000/swagger
```

### Frontend React

#### 1. Instalar Dependências
```powershell
cd frontend
npm install
```

#### 2. Configurar Variáveis de Ambiente (Opcional)
```powershell
# Criar arquivo .env na pasta frontend
# VITE_API_URL=http://localhost:5000/api
```

#### 3. Executar em Modo Dev
```powershell
npm run dev

# App rodando em: http://localhost:5173
```

---

## 🧪 Testar a Aplicação

### 1. Acessar Swagger
Abrir: http://localhost:5000/swagger

### 2. Criar Primeiro Registro
```json
POST /api/dadosenergeticos
{
  "dataReferencia": "2025-12-17T00:00:00",
  "codigoUsina": "UHE-001",
  "producaoMWh": 1500.50,
  "capacidadeDisponivel": 2000.00,
  "status": "Ativo",
  "observacoes": "Teste inicial"
}
```

### 3. Verificar no Frontend
Acessar: http://localhost:3000 (ou :5173 se local)

---

## 🐛 Solução de Problemas

### Erro: "Porta já em uso"
```powershell
# Verificar processos na porta 5000
netstat -ano | findstr :5000

# Matar processo (substituir PID)
taskkill /PID <numero_pid> /F
```

### Erro: "Connection to SQL Server failed"
```powershell
# Verificar se SQL Server está rodando
docker ps  # Se usando container

# Testar connection string
sqlcmd -S localhost -U sa -P YourStrong@Password123
```

### Erro: "npm install falhou"
```powershell
# Limpar cache e reinstalar
cd frontend
rm -rf node_modules
rm package-lock.json
npm cache clean --force
npm install
```

### Erro: "dotnet ef não encontrado"
```powershell
# Instalar ferramenta EF Core globalmente
dotnet tool install --global dotnet-ef

# Ou atualizar
dotnet tool update --global dotnet-ef
```

---

## 📦 Build para Produção

### Backend
```powershell
cd src\PDPW.API
dotnet publish -c Release -o .\publish
```

### Frontend
```powershell
cd frontend
npm run build
# Arquivos em: dist/
```

---

## 🎯 Próximos Passos Após Setup

1. ✅ Executar aplicação e validar funcionamento
2. 📖 Estudar código legado VB.NET (quando disponível)
3. 🔄 Identificar funcionalidades para migrar
4. 💻 Implementar novos endpoints conforme necessário
5. 🎨 Ajustar UI para se parecer com sistema legado
6. 🧪 Adicionar testes

---

## 📞 Comandos Úteis

```powershell
# Listar containers rodando
docker ps

# Ver logs de um container
docker logs pdpw-backend -f

# Parar todos os containers
docker-compose down

# Rebuild completo (se houver mudanças)
docker-compose up --build --force-recreate

# Acessar SQL Server no container
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Password123
```

---

**Tempo estimado de setup completo: 15-20 minutos**

---

## 🧰 Visual Studio Community + Olá Mundo (C#)

Para validar o ambiente Windows/VS enquanto aguardamos o repositório legado (VB/WebForms), use este passo-a-passo rápido.

### 1) Instalar Visual Studio Community 2022
- Baixe: https://visualstudio.microsoft.com/pt-br/vs/community/
- Selecione as cargas de trabalho:
  - “Desenvolvimento para desktop com .NET”
  - “Desenvolvimento ASP.NET e web”

### 2) Abrir a solução da PoC no VS
1. Abrir `PDPW.sln` na raiz do projeto
2. Restaurar pacotes automaticamente (VS faz isso ao abrir)

### 3) Rodar o Olá Mundo (Console .NET)
O projeto `PDPW.Tools.HelloWorld` foi criado para validar o runtime do .NET no ambiente.

No Visual Studio:
- Defina como projeto de inicialização: `PDPW.Tools.HelloWorld`
- Pressione F5 (ou Ctrl+F5)

Ou via PowerShell:
```powershell
cd src\PDPW.Tools.HelloWorld
dotnet run
```

Saída esperada:
```
Olá, PDPW PoC! .NET em funcionamento.
Runtime: .NET 8.0.0 (ou similar)
```

Observação: se sua máquina tiver apenas runtime .NET 10 instalado, o HelloWorld já está configurado para rodar também em .NET 10 (net10.0). Para executar a API da PoC em .NET 8, instale o runtime 8.x a partir de https://dotnet.microsoft.com/pt-br/download/dotnet/8.0.

### 4) Próximo passo no VS
- Troque o startup para `PDPW.API` e execute para abrir o Swagger.
```powershell
cd src\PDPW.API
dotnet run
```

---

## 🗃️ Banco de Dados (Backup/Dump do Cliente)

Quando recebermos o backup (dump) do SQL Server:
- Restaurar no SQL Server local (ou instância designada pelo ONS)
- Atualizar a connection string em `src/PDPW.API/appsettings.Development.json`
- Pular migrations do EF Core (usaremos o schema do legado nesta fase)

Exemplo de connection string:
```json
"DefaultConnection": "Server=localhost;Database=PDPW_DB_Legado;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Observação: imagens oficiais do SQL Server para contêiner Windows não estão disponíveis atualmente. Para a PoC, utilize SQL Server local/VM ou contêiner Linux separado do backend Windows (não funciona no mesmo daemon simultaneamente no Windows). Para desenvolvimento local, manter o SQL Server fora do compose.

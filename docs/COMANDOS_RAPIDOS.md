# ⚡ COMANDOS RÁPIDOS - POC PDPw

**Referência rápida de comandos para desenvolvimento e testes**

---

## 🐳 DOCKER

### **Iniciar Ambiente**
```powershell
docker-compose up -d
```

### **Parar Ambiente**
```powershell
docker-compose down
```

### **Rebuild Completo**
```powershell
docker-compose down -v
docker-compose up -d --build
```

### **Ver Logs**
```powershell
# Todos os containers
docker-compose logs -f

# Apenas API
docker-compose logs -f api

# Apenas SQL Server
docker-compose logs -f sqlserver
```

### **Status dos Containers**
```powershell
docker ps
```

### **Entrar no SQL Server**
```powershell
docker exec -it pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C
```

---

## 🔨 BUILD E TESTES

### **Build**
```powershell
# Build da solução
dotnet build

# Build de projeto específico
dotnet build src/PDPW.API/PDPW.API.csproj

# Build sem cache
dotnet build --no-incremental

# Build em Release
dotnet build -c Release
```

### **Testes Unitários**
```powershell
# Todos os testes
dotnet test

# Com verbosidade
dotnet test --logger "console;verbosity=detailed"

# Apenas um projeto
dotnet test tests/PDPW.UnitTests/PDPW.UnitTests.csproj

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

### **Build + Testes**
```powershell
dotnet build && dotnet test
```

---

## 🌐 VALIDAÇÃO DE APIS

### **Health Check**
```powershell
curl http://localhost:5001/health
```

### **Swagger UI**
```powershell
# Abrir no navegador
start http://localhost:5001/swagger
```

### **Validação Completa**
```powershell
.\scripts\powershell\validar-todas-apis.ps1
```

### **Testar Endpoint Específico**
```powershell
# TiposUsina - Listar
Invoke-RestMethod -Uri "http://localhost:5001/api/tiposusina" | ConvertTo-Json

# Empresas - Buscar
Invoke-RestMethod -Uri "http://localhost:5001/api/empresas/buscar?termo=Itaipu" | ConvertTo-Json

# Intercambios - Filtrar
Invoke-RestMethod -Uri "http://localhost:5001/api/intercambios/subsistema?origem=SE&destino=S" | ConvertTo-Json

# SemanasPMO - Próximas
Invoke-RestMethod -Uri "http://localhost:5001/api/semanaspmo/proximas?quantidade=4" | ConvertTo-Json
```

---

## 💾 BANCO DE DADOS

### **Verificar Dados**
```powershell
# Total de registros por tabela
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "
SELECT 'TiposUsina' AS Tabela, COUNT(*) AS Total FROM TiposUsina
UNION ALL SELECT 'Empresas', COUNT(*) FROM Empresas
UNION ALL SELECT 'Usinas', COUNT(*) FROM Usinas
UNION ALL SELECT 'UnidadesGeradoras', COUNT(*) FROM UnidadesGeradoras
UNION ALL SELECT 'Cargas', COUNT(*) FROM Cargas
UNION ALL SELECT 'Intercambios', COUNT(*) FROM Intercambios
UNION ALL SELECT 'Balancos', COUNT(*) FROM Balancos
UNION ALL SELECT 'Usuarios', COUNT(*) FROM Usuarios
"
```

### **Consultas Específicas**
```powershell
# Listar usinas
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Nome, Codigo, PotenciaInstalada FROM Usinas"

# Listar UGs da Itaipu
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Codigo, Nome, PotenciaNominal FROM UnidadesGeradoras WHERE UsinaId = 1"
```

### **Migrations**
```powershell
# Criar nova migration
dotnet ef migrations add NomeDaMigration --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Aplicar migrations
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Remover última migration
dotnet ef migrations remove --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Ver migrations aplicadas
dotnet ef migrations list --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

---

## 🔍 DIAGNÓSTICO

### **Verificar Versões**
```powershell
# .NET SDK
dotnet --version

# Docker
docker --version

# Docker Compose
docker-compose --version

# PowerShell
$PSVersionTable.PSVersion
```

### **Verificar Processos**
```powershell
# Processos usando porta 5001
netstat -ano | findstr :5001

# Processos Docker
docker ps -a
```

### **Limpar Ambiente**
```powershell
# Limpar builds
dotnet clean

# Remover containers parados
docker container prune -f

# Remover volumes não usados
docker volume prune -f

# Remover imagens não usadas
docker image prune -f
```

---

## 📝 GIT

### **Status**
```powershell
git status
git log --oneline -10
```

### **Branches**
```powershell
# Ver branches
git branch -a

# Criar branch
git checkout -b feature/nova-funcionalidade

# Trocar branch
git checkout main
git checkout feature/backend
```

### **Commit e Push**
```powershell
# Adicionar arquivos
git add .

# Commit
git commit -m "feat(api): adicionar endpoints de busca"

# Push
git push origin feature/backend

# Pull
git pull origin feature/backend
```

### **Sincronizar com Remotes**
```powershell
# Ver remotes
git remote -v

# Adicionar remote
git remote add squad https://github.com/RafaelSuzanoACT/POCMigracaoPDPw

# Fetch de todos os remotes
git fetch --all

# Merge de outro remote
git merge squad/main
```

---

## 🚀 DESENVOLVIMENTO

### **Adicionar Novo Controller**
```powershell
# Criar arquivo
New-Item -Path "src/PDPW.API/Controllers/NovoController.cs" -ItemType File

# Template básico já criado automaticamente pelo Copilot
```

### **Adicionar Nova Entidade**
```powershell
# 1. Criar Entity em Domain
New-Item -Path "src/PDPW.Domain/Entities/NovaEntidade.cs"

# 2. Criar Repository Interface em Domain
New-Item -Path "src/PDPW.Domain/Interfaces/INovaEntidadeRepository.cs"

# 3. Criar Repository em Infrastructure
New-Item -Path "src/PDPW.Infrastructure/Repositories/NovaEntidadeRepository.cs"

# 4. Criar Service Interface em Application
New-Item -Path "src/PDPW.Application/Interfaces/INovaEntidadeService.cs"

# 5. Criar Service em Application
New-Item -Path "src/PDPW.Application/Services/NovaEntidadeService.cs"

# 6. Criar DTOs em Application
New-Item -Path "src/PDPW.Application/DTOs/NovaEntidade/NovaEntidadeDto.cs"

# 7. Criar Migration
dotnet ef migrations add AdicionarNovaEntidade --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

### **Executar API Localmente**
```powershell
# Com hot reload
dotnet watch run --project src/PDPW.API

# Sem hot reload
dotnet run --project src/PDPW.API

# Em modo Release
dotnet run --project src/PDPW.API -c Release
```

---

## 📊 RELATÓRIOS

### **Gerar Relatório de Validação**
```powershell
.\scripts\powershell\validar-todas-apis.ps1
# Salva em: C:\temp\_ONS_PoC-PDPW\relatorio-validacao-apis.json
```

### **Ver Último Relatório**
```powershell
Get-Content C:\temp\_ONS_PoC-PDPW\relatorio-validacao-apis.json | ConvertFrom-Json | Format-Table
```

---

## 🔐 SEGURANÇA

### **Verificar Senhas**
```powershell
# SQL Server (Docker)
# Senha: Pdpw@2024!Strong

# Appsettings
Get-Content src/PDPW.API/appsettings.Development.json
```

### **Trocar Senha SQL Server**
```powershell
# Editar docker-compose.yml
# Trocar SA_PASSWORD

# Recriar container
docker-compose down -v
docker-compose up -d
```

---

## 📦 PACOTES NUGET

### **Adicionar Pacote**
```powershell
dotnet add src/PDPW.API package NomeDoPacote
```

### **Remover Pacote**
```powershell
dotnet remove src/PDPW.API package NomeDoPacote
```

### **Atualizar Pacotes**
```powershell
dotnet restore
```

### **Listar Pacotes**
```powershell
dotnet list src/PDPW.API package
```

---

## 🎨 FORMATAÇÃO DE CÓDIGO

### **Formatar Código**
```powershell
dotnet format
```

### **Verificar Formatação**
```powershell
dotnet format --verify-no-changes
```

---

## 📱 ATALHOS ÚTEIS

### **Abrir Solution no Visual Studio**
```powershell
start PDPW.sln
```

### **Abrir VS Code**
```powershell
code .
```

### **Abrir Explorer na pasta atual**
```powershell
explorer .
```

---

## 🆘 TROUBLESHOOTING

### **API não sobe**
```powershell
# 1. Verificar porta ocupada
netstat -ano | findstr :5001

# 2. Matar processo
taskkill /PID <PID> /F

# 3. Limpar e rebuild
dotnet clean
dotnet build
```

### **Docker não conecta**
```powershell
# 1. Reiniciar Docker Desktop
Restart-Service docker

# 2. Recriar containers
docker-compose down -v
docker-compose up -d --build

# 3. Verificar logs
docker-compose logs -f sqlserver
```

### **Banco de dados vazio**
```powershell
# Recriar banco com seed
docker-compose down -v
docker-compose up -d --build
Start-Sleep -Seconds 30
```

### **Testes falhando**
```powershell
# Limpar e testar novamente
dotnet clean
dotnet build
dotnet test --logger "console;verbosity=detailed"
```

---

## 📖 REFERÊNCIAS

- **Swagger**: http://localhost:5001/swagger
- **Health**: http://localhost:5001/health
- **GitHub**: https://github.com/wbulhoes/ONS_PoC-PDPW_V2
- **Documentos**: `docs/` folder

---

## ✅ CHECKLIST DIÁRIO

```powershell
# 1. Subir ambiente
docker-compose up -d

# 2. Verificar health
curl http://localhost:5001/health

# 3. Pull de atualizações
git pull origin feature/backend

# 4. Build
dotnet build

# 5. Testes
dotnet test

# 6. Desenvolvimento
code .
```

---

**Atualizado**: 27/12/2024  
**Versão**: 1.0  
**Autor**: Willian Bulhões

# ?? GUIA R�PIDO - MIGRA��O PARA SQL SERVER

## ? **PR�-REQUISITOS VERIFICADOS**

- ? **Espa�o em disco:** 191 GB livres (necess�rio: 15-20 GB)
- ? **Backup do cliente:** `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak` (43.2 GB)
- ? **SQL Server:** Express instalado (localhost\SQLEXPRESS)
- ? **Script de migra��o:** Criado e pronto

---

## ?? **OP��ES DE IMPLEMENTA��O**

### **OP��O 1: SQL Server via Docker (Recomendado)** ?

? **Vantagens:**
- Isolado em container
- F�cil de resetar
- N�o afeta SQL Server local
- Configura��o via docker-compose

**Como fazer:**

```powershell
# 1. Parar InMemory atual
cd C:\temp\_ONS_PoC-PDPW_V2
docker-compose down

# 2. Iniciar SQL Server em Docker
docker-compose -f docker-compose.full.yml up -d

# 3. Aguardar SQL Server inicializar (1-2 minutos)
timeout /t 90

# 4. Aplicar migrations EF Core
cd src\PDPW.API
dotnet ef database update

# 5. Executar script de migra��o
cd ..\..
.\scripts\migration\Migrate-Legacy-To-POC.ps1 -SqlServer "localhost" -SqlInstance "" -TargetDatabase "PDPW_DB"

# 6. Testar no Swagger
start http://localhost:5001/swagger
```

**Tempo estimado:** 20-30 minutos

---

### **OP��O 2: SQL Server Express Local**

? **Vantagens:**
- Mais performance
- Acesso via SSMS
- Sem overhead de Docker

**Como fazer:**

```powershell
# 1. Parar Docker
cd C:\temp\_ONS_PoC-PDPW_V2
docker-compose down

# 2. Atualizar appsettings.json
# Editar: src\PDPW.API\appsettings.json
# Alterar: "UseInMemoryDatabase": false

# 3. Aplicar migrations
cd src\PDPW.API
dotnet ef database update

# 4. Executar script de migra��o
cd ..\..
.\scripts\migration\Migrate-Legacy-To-POC.ps1

# 5. Rodar aplica��o
cd src\PDPW.API
dotnet run

# 6. Testar no Swagger
start http://localhost:5001/swagger
```

**Tempo estimado:** 15-25 minutos

---

## ?? **PASSO A PASSO DETALHADO - OP��O 1 (Docker)**

### **1. Preparar Ambiente**

```powershell
# Parar container atual
docker-compose down

# Verificar se SQL Server est� no Docker
docker images | findstr mssql
```

### **2. Configurar docker-compose.full.yml**

Verificar se o arquivo existe:

```powershell
Get-Content docker-compose.full.yml
```

Se n�o existir, criar com este conte�do m�nimo:

```yaml
version: '3.8'

services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: pdpw-sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Pdpw@2024!Strong
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - pdpw-network

  backend:
    build:
      context: .
      dockerfile: src/PDPW.API/Dockerfile
    container_name: pdpw-backend
    ports:
      - "5001:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - UseInMemoryDatabase=false
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=true;
    depends_on:
      - sqlserver
    networks:
      - pdpw-network

volumes:
  sqlserver-data:

networks:
  pdpw-network:
```

### **3. Iniciar SQL Server**

```powershell
# Iniciar apenas SQL Server primeiro
docker-compose -f docker-compose.full.yml up -d sqlserver

# Aguardar inicializa��o
timeout /t 60

# Verificar se est� rodando
docker ps
docker logs pdpw-sqlserver --tail 20
```

### **4. Aplicar Migrations**

```powershell
cd src\PDPW.API

# Configurar connection string tempor�ria
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=true;"

# Aplicar migrations
dotnet ef database update

# Verificar banco criado
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -Q "SELECT name FROM sys.databases WHERE name = 'PDPW_DB'"
```

### **5. Executar Migra��o de Dados**

```powershell
cd ..\..

# Executar script de migra��o
.\scripts\migration\Migrate-Legacy-To-POC.ps1 `
    -SqlServer "localhost,1433" `
    -SqlInstance "" `
    -TargetDatabase "PDPW_DB" `
    -TopEmpresas 30 `
    -TopUsinas 50 `
    -SemanasPMO 26
```

**Durante a execu��o:**
- Responda **"S"** quando perguntado se deseja aplicar os dados
- Responda **"S"** quando perguntado se deseja remover banco tempor�rio

### **6. Iniciar Backend**

```powershell
# Iniciar backend conectado ao SQL Server
docker-compose -f docker-compose.full.yml up -d backend

# Verificar logs
docker logs pdpw-backend --tail 50

# Aguardar inicializa��o
timeout /t 10
```

### **7. Testar Swagger**

```powershell
# Abrir Swagger no navegador
start http://localhost:5001/swagger
```

**Valida��es:**
- ? GET /api/empresas deve retornar ~30 empresas
- ? GET /api/usinas deve retornar ~50 usinas
- ? Dados devem ser diferentes do InMemory
- ? Relacionamentos devem funcionar

---

## ?? **TROUBLESHOOTING**

### **Problema: SQL Server n�o inicia**

```powershell
# Ver logs detalhados
docker logs pdpw-sqlserver

# Reiniciar container
docker-compose -f docker-compose.full.yml restart sqlserver

# Verificar porta 1433 livre
netstat -ano | findstr 1433
```

### **Problema: Migrations falham**

```powershell
# Verificar connection string
echo $env:ConnectionStrings__DefaultConnection

# Testar conex�o manualmente
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong"

# Dropar e recriar banco
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -Q "DROP DATABASE PDPW_DB"
dotnet ef database update
```

### **Problema: Script de migra��o falha**

```powershell
# Verificar backup existe
Test-Path "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"

# Executar com verbose
.\scripts\migration\Migrate-Legacy-To-POC.ps1 -Verbose

# Aplicar dados manualmente
sqlcmd -S localhost,1433 -U sa -P "Pdpw@2024!Strong" -d PDPW_DB -i ".\scripts\migration\output\migrate-all.sql"
```

### **Problema: Backend n�o conecta ao SQL Server**

```powershell
# Verificar vari�veis de ambiente
docker inspect pdpw-backend | findstr ConnectionStrings

# Reiniciar backend
docker-compose -f docker-compose.full.yml restart backend

# Ver logs de erro
docker logs pdpw-backend --tail 100
```

---

## ? **VALIDA��O FINAL**

### **Checklist P�s-Migra��o:**

```powershell
# 1. Verificar containers rodando
docker ps
# Deve mostrar: pdpw-sqlserver e pdpw-backend

# 2. Testar conex�o SQL Server
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -Q "USE PDPW_DB; SELECT COUNT(*) FROM Empresas; SELECT COUNT(*) FROM Usinas;"

# 3. Testar APIs
curl http://localhost:5001/api/empresas | ConvertFrom-Json | Measure-Object
curl http://localhost:5001/api/usinas | ConvertFrom-Json | Measure-Object

# 4. Verificar Swagger
start http://localhost:5001/swagger
```

**Resultados esperados:**
- ? Empresas: ~30 registros
- ? Usinas: ~50 registros
- ? Swagger carrega sem erros
- ? CRUD funciona normalmente

---

## ?? **PR�XIMOS PASSOS**

Ap�s migra��o bem-sucedida:

1. ? **Atualizar QA** - Informar sobre nova base de dados
2. ? **Executar testes** - Validar todos os endpoints
3. ? **Documentar diferen�as** - InMemory vs SQL Server
4. ? **Backup da base** - Guardar estado atual
5. ? **Planejar pr�ximas migra��es** - Mais tabelas se necess�rio

---

## ?? **COMPARA��O: InMemory vs SQL Server**

| Caracter�stica | InMemory | SQL Server |
|----------------|----------|------------|
| **Dados** | 69 registros | ~150 registros |
| **Origem** | Seed manual | Backup real do cliente |
| **Persist�ncia** | Perdidos ao reiniciar | Mantidos em volumes |
| **Performance** | Muito r�pida | R�pida |
| **Relacionamentos** | Simulados | Reais (FKs) |
| **Valida��es** | Limitadas | Completas |
| **Realismo** | M�dio | Alto |
| **Uso recomendado** | Desenvolvimento r�pido | Testes QA |

---

## ?? **SUPORTE**

**D�vidas ou problemas?**
- Consulte: `docs/PLANO_MIGRACAO_SQL_SERVER.md`
- Verifique logs: `docker logs pdpw-backend`
- Scripts em: `.\scripts\migration\output\`

---

**Boa sorte com a migra��o! ??**

**Tempo total estimado:** 20-30 minutos  
**Espa�o necess�rio:** ~15 GB  
**Dados extra�dos:** ~150 registros reais  


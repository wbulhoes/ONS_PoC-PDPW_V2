# ?? GUIA DE IN�CIO R�PIDO

## Op��o 1: Docker (Recomendado)

### Requisitos
- Docker Desktop instalado

### Passos
\\\ash
# 1. Clonar reposit�rio
git clone https://github.com/wbulhoes/ONS_PoC-PDPW.git
cd ONS_PoC-PDPW

# 2. Iniciar ambiente
docker-compose up -d

# 3. Acessar aplica��o
# Backend: http://localhost:5001/swagger
# Frontend: http://localhost:3000
\\\

## Op��o 2: Local

### Requisitos
- .NET 8 SDK
- Node.js 18+
- SQL Server

### Backend
\\\ash
cd src/PDPW.API
dotnet restore
dotnet ef database update --project ../PDPW.Infrastructure
dotnet run
\\\

### Frontend
\\\ash
cd frontend
npm install
npm start
\\\

## Testar APIs

Acesse: http://localhost:5001/swagger

APIs dispon�veis:
- GET /api/usinas - Listar usinas
- GET /api/empresas - Listar empresas
- GET /api/tiposusina - Listar tipos
- GET /api/semanaspmo - Listar semanas PMO
- GET /api/equipespdp - Listar equipes

## Credenciais

### Banco de Dados
- Server: localhost
- Database: PDPW_PoC
- User: sa
- Password: Pdpw@2024!

## Problemas Comuns

### Porta 5001 em uso
\\\ash
# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F
\\\

### Migrations n�o aplicadas
\\\ash
cd src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
\\\

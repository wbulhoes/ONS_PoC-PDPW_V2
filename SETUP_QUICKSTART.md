# ?? Setup Rápido para Colegas

## ? Começar em 5 Minutos

### 1?? **Clonar o Projeto**

```powershell
git clone https://github.com/seu-usuario/PDPW.git
cd PDPW
```

### 2?? **Verificar Pré-requisitos**

```powershell
# .NET 8 instalado?
dotnet --version
# Deve retornar: 8.0.x

# Se não tiver, baixe:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### 3?? **Configurar (ESCOLHA UMA OPÇÃO)**

#### ? Opção A: InMemory (Mais Rápido - SEM SQL Server)

**Edite:** `src\PDPW.API\appsettings.Development.json`

Altere para:
```json
{
  "UseInMemoryDatabase": true
}
```

#### ??? Opção B: SQL Server LocalDB (Dados Persistem)

```powershell
# Instalar EF Tools
dotnet tool install --global dotnet-ef

# Criar banco de dados
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

### 4?? **Executar**

```powershell
# API Principal
dotnet run --project src\PDPW.API

# Acessar:
# Swagger: https://localhost:65417/swagger
# API: https://localhost:65417
```

```powershell
# Hello World (Dashboard de Validação)
dotnet run --project HelloWorld

# Acessar:
# Dashboard: http://localhost:5555
```

---

## ?? Documentação Completa

Leia os guias na pasta raiz:

| Guia | Descrição |
|------|-----------|
| `DATABASE_SETUP.md` | Como configurar banco de dados |
| `TROUBLESHOOTING.md` | Resolver problemas |
| `QUICK_START_INMEMORY.md` | Início rápido sem SQL |
| `SHARING_GUIDE.md` | Como compartilhar o projeto |

---

## ? Problemas Comuns

### ".NET não encontrado"
Instale: https://dotnet.microsoft.com/download/dotnet/8.0

### "Erro de conexão com banco"
Configure InMemory: `"UseInMemoryDatabase": true`

### "Porta já em uso"
Mude em `launchSettings.json`

---

## ?? Precisa de Ajuda?

1. Leia: `TROUBLESHOOTING.md`
2. Veja issues no GitHub
3. Entre em contato com o autor

---

**? Tudo funcionando? Bom desenvolvimento!** ??

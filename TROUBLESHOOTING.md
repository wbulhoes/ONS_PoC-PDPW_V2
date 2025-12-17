# Guia de Solução de Problemas - PDPW API

## ? Preciso instalar SQL Server?

**NÃO!** Você tem 3 opções:

1. **InMemory Database** ? Mais rápido (zero configuração)
2. **SQL Server LocalDB** ??? Recomendado (já vem com VS)
3. **SQL Server Completo** - Para produção

**Veja o guia completo:** `DATABASE_SETUP.md`

**Início rápido SEM SQL Server:** `QUICK_START_INMEMORY.md`

---

## Erro: Aplicação encerra com código 0xffffffff

### Causa
Este erro geralmente ocorre quando há problemas de conexão com o banco de dados SQL Server.

### ? Solução Rápida (SEM instalar SQL Server)

**Use banco InMemory:**

1. Edite `src/PDPW.API/appsettings.Development.json`:
```json
{
  "UseInMemoryDatabase": true
}
```

2. Execute:
```powershell
dotnet run --project src\PDPW.API
```

**Pronto!** A aplicação funcionará sem SQL Server.

?? **Atenção:** Dados são temporários (perdidos ao fechar).

---

## Soluções com SQL Server

### 1. Verificar se o SQL Server está rodando

**Windows:**
```powershell
# Verificar status do serviço
Get-Service -Name "MSSQL*"

# Iniciar o SQL Server (como Administrador)
Start-Service -Name "MSSQLSERVER"
```

**SQL Server Express:**
```powershell
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### 2. Usar LocalDB (Recomendado)

**Verificar se já está instalado:**
```powershell
sqllocaldb info
```

**Configurar:**

Edite `src/PDPW.API/appsettings.Development.json`:

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Criar o banco de dados

Execute os seguintes comandos na raiz do projeto:

```bash
# Instalar ferramenta EF Core (se ainda não instalada)
dotnet tool install --global dotnet-ef

# Criar uma nova migração
dotnet ef migrations add InitialCreate --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Aplicar a migração e criar o banco de dados
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

### 4. Verificar a string de conexão

Edite o arquivo `src/PDPW.API/appsettings.Development.json`:

**Para SQL Server Express:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PDPW_DB_Dev;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Para SQL Server com usuário/senha:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB_Dev;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Para LocalDB:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 5. Testar a conexão

Após iniciar a aplicação, acesse:

- **Health Check:** http://localhost:65418/health ou https://localhost:65417/health
- **Swagger:** https://localhost:65417/swagger
- **Status:** https://localhost:65417/

Os logs agora mostrarão detalhes sobre a conexão com o banco de dados:
- ? Se a conexão foi estabelecida com sucesso
- ? Se há migrações pendentes
- ? Se houve erro ao conectar
- ??? Se está usando InMemory Database

O endpoint `/` também mostra qual tipo de banco está sendo usado:
```json
{
  "databaseType": "InMemory (temporário)"
  // ou
  "databaseType": "SQL Server"
}
```

---

## Comparação das Opções

| Opção | Configuração | Persistência | Quando Usar |
|-------|-------------|--------------|-------------|
| **InMemory** | Zero | ? Temporária | Testes rápidos, desenvolvimento inicial |
| **LocalDB** | Mínima | ? Persistente | Desenvolvimento local |
| **SQL Server** | Completa | ? Persistente | Produção, staging |

---

## Verificar Logs

Os logs detalhados agora mostrarão:
- Tipo de banco de dados em uso
- Tentativa de conexão com banco de dados
- Migrações pendentes
- Erros específicos de conexão

**Exemplo com InMemory:**
```
??? Banco de dados InMemory inicializado (dados temporários)
```

**Exemplo com SQL Server:**
```
? Conexão com banco de dados estabelecida com sucesso!
```

### Comandos Úteis

```bash
# Verificar qual banco está configurado
dotnet run --project src\PDPW.API
# Veja os logs ou acesse https://localhost:65417/

# Verificar migrações (apenas SQL Server/LocalDB)
dotnet ef migrations list --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Remover última migração (se necessário)
dotnet ef migrations remove --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Criar script SQL das migrações
dotnet ef migrations script --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Ver informações do banco
dotnet ef dbcontext info --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

## Outros Problemas Comuns

### Porta já em uso

Se as portas 65417/65418 estiverem em uso, modifique em `launchSettings.json`:

```json
"applicationUrl": "https://localhost:7000;http://localhost:7001"
```

### HTTPS Development Certificate

Se houver problemas com certificado HTTPS:

```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

---

## ?? Documentação Adicional

- **`DATABASE_SETUP.md`** - Guia completo de configuração de banco de dados
- **`QUICK_START_INMEMORY.md`** - Início rápido sem SQL Server
- **`IMPROVEMENTS.md`** - Melhorias implementadas
- **`HelloWorld\README.md`** - Teste de ambiente C#

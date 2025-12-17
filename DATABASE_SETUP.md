# ??? Guia de Configuração de Banco de Dados - PDPW API

## ? Preciso instalar SQL Server?

**Resposta curta: NÃO!** Você tem 3 opções:

---

## ?? Comparação das Opções

| Opção | Instalação | Persistência | Ideal Para | Recomendação |
|-------|-----------|--------------|------------|--------------|
| **InMemory** | ? Nenhuma | ? Dados temporários | Testes rápidos | ??? Mais fácil |
| **LocalDB** | ? Mínima | ? Dados salvos | Desenvolvimento | ????? Recomendado |
| **SQL Server** | ?? Completa | ? Dados salvos | Produção | ??? Produção |

---

## ?? Opção 1: InMemory Database (MAIS RÁPIDO)

### Vantagens
? **Zero instalação** - Não precisa instalar nada  
? **Funciona imediatamente** - Sem configuração  
? **Leve e rápido** - Perfeito para desenvolvimento inicial  

### Desvantagens
?? **Dados temporários** - Perdidos ao fechar a aplicação  
?? **Não é produção** - Apenas para desenvolvimento/testes  

### Como Ativar

**Edite `appsettings.Development.json`:**

```json
{
  "UseInMemoryDatabase": true,
  "ConnectionStrings": {
    "DefaultConnection": "não será usado"
  }
}
```

**Pronto!** Execute a aplicação:

```powershell
dotnet run --project src\PDPW.API
```

A aplicação irá mostrar:
```
??? Banco de dados InMemory inicializado (dados temporários)
```

### Testar

```powershell
# Status da API
curl https://localhost:65417/

# Resposta incluirá:
# "databaseType": "InMemory (temporário)"
```

---

## ? Opção 2: SQL Server LocalDB (RECOMENDADO)

### Vantagens
? **Leve** - Menor que SQL Server completo  
? **Já vem com Visual Studio** - Provavelmente já instalado  
? **Dados persistem** - Salvos entre execuções  
? **Suporta migrations** - Ambiente real de desenvolvimento  

### Verificar se já está instalado

```powershell
sqllocaldb info
```

**Se retornar uma lista:** ? Já está instalado!  
**Se retornar erro:** ?? Precisa instalar

### Instalar LocalDB (se necessário)

**Via Visual Studio Installer:**
1. Abra **Visual Studio Installer**
2. Clique em **Modificar**
3. Selecione: **"Desenvolvimento de dados e armazenamento"**
4. Marque: **"SQL Server Express LocalDB"**
5. Clique em **Modificar**

**Ou baixe standalone:**
[SQL Server Express LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb)

### Configurar

**Edite `appsettings.Development.json`:**

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Criar o banco de dados

```powershell
# Instalar ferramenta EF Core (primeira vez)
dotnet tool install --global dotnet-ef

# Criar migração inicial
dotnet ef migrations add InitialCreate --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Aplicar migração (cria o banco)
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

### Executar

```powershell
dotnet run --project src\PDPW.API
```

Logs mostrarão:
```
? Conexão com banco de dados estabelecida com sucesso!
```

---

## ?? Opção 3: SQL Server Completo

### Quando usar
- Produção
- Testes de performance
- Recursos avançados do SQL Server

### Instalar

**SQL Server Express (Gratuito):**
```powershell
# Download
https://www.microsoft.com/sql-server/sql-server-downloads

# Escolha: SQL Server 2022 Express
```

### Configurar

**Para SQL Server padrão:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB_Dev;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Para SQL Server Express:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=PDPW_DB_Dev;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

**Com usuário/senha:**
```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB_Dev;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### Iniciar SQL Server

```powershell
# Verificar status
Get-Service -Name "MSSQL*"

# Iniciar SQL Server
Start-Service -Name "MSSQLSERVER"

# OU SQL Express
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Criar banco de dados

```powershell
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

---

## ?? Recomendação por Cenário

### ?? **Estou apenas testando a aplicação**
? Use **InMemory Database** (Opção 1)
```json
"UseInMemoryDatabase": true
```

### ?? **Vou desenvolver features**
? Use **LocalDB** (Opção 2)
```json
"UseInMemoryDatabase": false,
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;..."
```

### ?? **Ambiente de produção**
? Use **SQL Server** completo (Opção 3)

---

## ?? Comandos Úteis

### Verificar configuração atual

```powershell
# Ver qual banco está configurado
dotnet run --project src\PDPW.API

# Abrir a API
start https://localhost:65417/

# O campo "databaseType" mostrará qual banco está ativo
```

### Alternar entre bancos

**Para InMemory:**
```json
"UseInMemoryDatabase": true
```

**Para SQL Server/LocalDB:**
```json
"UseInMemoryDatabase": false
```

### Migrations (apenas SQL Server/LocalDB)

```powershell
# Listar migrações
dotnet ef migrations list --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Adicionar migração
dotnet ef migrations add NomeDaMigracao --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Aplicar migrações
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Remover última migração
dotnet ef migrations remove --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

---

## ?? Dicas

### Começar rápido
1. Configure `"UseInMemoryDatabase": true`
2. Execute `dotnet run --project src\PDPW.API`
3. Teste a API no Swagger
4. Depois migre para LocalDB quando quiser persistência

### Desenvolvimento em equipe
- Use LocalDB ou SQL Server
- Compartilhe migrations no Git
- Cada dev aplica migrations localmente

### CI/CD
- Testes: Use InMemory
- Staging/Produção: Use SQL Server

---

## ? Perguntas Frequentes

**Q: Posso usar PostgreSQL ou MySQL?**  
A: Sim! Basta trocar o provider do Entity Framework Core.

**Q: InMemory suporta todas as features do SQL?**  
A: Não. Algumas queries complexas podem não funcionar.

**Q: LocalDB vs SQL Express, qual a diferença?**  
A: LocalDB é mais leve, apenas para desenvolvimento. Express é completo.

**Q: Como sei qual banco estou usando?**  
A: Acesse `https://localhost:65417/` e veja o campo `databaseType`.

---

## ? Checklist de Setup

- [ ] Escolhi qual opção usar (InMemory, LocalDB ou SQL Server)
- [ ] Configurei `appsettings.Development.json`
- [ ] Se SQL/LocalDB: Executei migrations
- [ ] Testei a aplicação
- [ ] Verificei os logs de conexão
- [ ] Acessei `/health` para confirmar status

**Tudo OK? Sua API está pronta para uso!** ??

# ?? Guia Rápido - Restauração de Banco de Dados

## ?? Onde Colocar o Backup

1. **Copie** o arquivo de backup fornecido pelo cliente para:
   ```
   C:\temp\_ONS_PoC-PDPW\database\backups\working\
   ```

2. **Mantenha** o backup original em local seguro

---

## ? Restauração Rápida

### Opção 1: Script Automatizado (Recomendado)

```powershell
# Navegar para a pasta do projeto
cd C:\temp\_ONS_PoC-PDPW

# Executar script de restauração
.\database\restore-database.ps1
```

**O script irá:**
- ? Detectar automaticamente o backup
- ? Restaurar o banco de dados
- ? Verificar integridade
- ? Atualizar estatísticas
- ? Exibir informações do banco

### Opção 2: Com Parâmetros Customizados

```powershell
# Especificar caminho do backup
.\database\restore-database.ps1 -BackupPath "C:\caminho\do\backup.bak"

# Especificar servidor e nome do banco
.\database\restore-database.ps1 -ServerInstance "localhost\SQLEXPRESS" -DatabaseName "PDPW_DB"

# Apenas analisar (não restaurar)
.\database\restore-database.ps1 -AnalyzeOnly

# Restaurar e gerar scripts do schema
.\database\restore-database.ps1 -GenerateScripts
```

### Opção 3: Manual (SQL Server Management Studio)

1. Abrir SSMS
2. Conectar ao servidor
3. Botão direito em `Databases` > `Restore Database...`
4. Selecionar `Device` > `Add...`
5. Navegar até o backup
6. Clicar em `OK`

---

## ?? Verificar Restauração

### SQL Server Management Studio (SSMS)

```sql
-- Verificar se banco existe
SELECT name, state_desc, recovery_model_desc 
FROM sys.databases 
WHERE name = 'PDPW_DB';

-- Listar tabelas
SELECT 
    s.name AS SchemaName,
    t.name AS TableName,
    p.rows AS RowCount
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0, 1)
ORDER BY s.name, t.name;

-- Ver primeiros registros de uma tabela
SELECT TOP 10 * FROM [SchemaName].[TableName];
```

### PowerShell

```powershell
# Verificar bancos disponíveis
Invoke-Sqlcmd -ServerInstance "localhost" -Query "SELECT name FROM sys.databases" -TrustServerCertificate

# Verificar tabelas do banco
Invoke-Sqlcmd -ServerInstance "localhost" -Database "PDPW_DB" -Query "SELECT s.name + '.' + t.name AS TableName FROM sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id" -TrustServerCertificate
```

---

## ??? Próximos Passos

### 1. Explorar o Banco

```powershell
# Gerar scripts do schema
.\database\restore-database.ps1 -GenerateScripts

# Resultado: scripts em database/scripts/schema/
```

### 2. Scaffold do EF Core

```powershell
# Navegar para o projeto Infrastructure
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure

# Instalar ferramenta EF (se necessário)
dotnet tool install --global dotnet-ef

# Scaffold das entidades do banco legado
dotnet ef dbcontext scaffold "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir LegacyModels --context-dir Data --context PDPWLegacyContext --force --no-onconfiguring
```

### 3. Atualizar Connection String

**appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "UseInMemoryDatabase": false
}
```

### 4. Documentar o Schema

Preencher os templates em:
```
database/docs/
??? schema.md           ? Visão geral
??? tables.md           ? Descrição das tabelas
??? relationships.md    ? Relacionamentos
```

---

## ?? Troubleshooting

### Erro: "Cannot open backup device"

**Causa:** Arquivo não encontrado ou sem permissão

**Solução:**
```powershell
# Verificar se arquivo existe
Test-Path "C:\caminho\do\backup.bak"

# Verificar permissões
icacls "C:\caminho\do\backup.bak"

# Dar permissão ao SQL Server
icacls "C:\caminho\do\backup.bak" /grant "NT SERVICE\MSSQLSERVER:F"
```

### Erro: "Database already exists"

**Solução:** O script já trata isso, mas se necessário:
```sql
-- Dropar banco manualmente
ALTER DATABASE [PDPW_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [PDPW_DB];
```

### Erro: "Exclusive access could not be obtained"

**Causa:** Banco em uso

**Solução:**
```sql
-- Fechar todas as conexões
ALTER DATABASE [PDPW_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
```

### Erro: "The backup set holds a backup of a database other than the existing"

**Causa:** Nome do banco no backup diferente

**Solução:** Usar RESTORE com MOVE (script já faz isso automaticamente)

---

## ?? Referências

### Documentação

- [SQL Server RESTORE](https://docs.microsoft.com/sql/t-sql/statements/restore-statements-transact-sql)
- [EF Core Scaffold](https://docs.microsoft.com/ef/core/managing-schemas/scaffolding)
- [SQL Server Management Studio](https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms)

### Ferramentas

- **SSMS:** SQL Server Management Studio
- **Azure Data Studio:** Alternativa moderna ao SSMS
- **dotnet-ef:** Ferramenta CLI do Entity Framework Core

---

## ?? Suporte

**Problemas?** Entre em contato:

- **GitHub Issues:** https://github.com/wbulhoes/ONS_PoC-PDPW/issues
- **Desenvolvedor:** Willian Charantola Bulhoes

---

**Última atualização:** 17/12/2025

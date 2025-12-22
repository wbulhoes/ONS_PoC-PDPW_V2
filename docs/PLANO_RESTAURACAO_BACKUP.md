# ?? PLANO DE RESTAURA��O DO BACKUP DO CLIENTE

## ?? INFORMA��ES DO BACKUP

```
Arquivo: Backup_PDP_TST.bak
Tamanho: 43.2 GB (46,424,975,872 bytes)
Data: 18/12/2024 10:32:38
Localiza��o: C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak
```

---

## ?? ESTRAT�GIA RECOMENDADA

### Op��o 1: RESTAURA��O COMPLETA (Recomendada para An�lise)
Restaurar o backup em uma inst�ncia SQL Server separada para:
- ? Analisar a estrutura real do banco legado
- ? Entender os dados existentes
- ? Mapear relacionamentos
- ? Extrair dados para popular a POC

### Op��o 2: EXTRA��O SELETIVA (Recomendada para POC)
Restaurar, extrair dados relevantes e popular o banco da POC:
- ? Manter a estrutura da POC (EF Core)
- ? Popular com dados reais do cliente
- ? Preservar integridade referencial
- ? Manter seed data + dados reais

---

## ?? PASSOS PARA RESTAURA��O

### Passo 1: Verificar Informa��es do Backup

```powershell
# Conectar ao SQL Server e verificar o backup
sqlcmd -S localhost -Q "RESTORE FILELISTONLY FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'"
```

### Passo 2: Restaurar em Banco Separado

```sql
-- Restaurar backup em novo banco
RESTORE DATABASE PDPW_Legacy
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP_TST' TO 'C:\SQL\Data\PDPW_Legacy.mdf',
    MOVE 'PDP_TST_log' TO 'C:\SQL\Data\PDPW_Legacy_log.ldf',
    REPLACE,
    RECOVERY;
```

### Passo 3: Analisar Estrutura

```sql
-- Listar todas as tabelas
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Ver contagem de registros por tabela
SELECT 
    t.NAME AS TableName,
    p.rows AS RowCounts
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE p.index_id < 2
ORDER BY p.rows DESC;
```

---

## ?? MAPEAMENTO DE DADOS

### Tabelas Priorit�rias para Extra��o

Com base nas 5 APIs j� implementadas, precisamos extrair dados de:

1. **TiposUsina** / **TB_TIPO_USINA** (ou similar)
   - Mapear tipos de usina do legado
   - Complementar nossos 5 tipos seed

2. **Empresas** / **TB_EMPRESA** (ou similar)
   - Extrair empresas reais do setor el�trico
   - Manter CNPJ, contatos, etc.

3. **Usinas** / **TB_USINA** (ou similar)
   - Todas as usinas do SIN (Sistema Interligado Nacional)
   - Capacidade instalada, localiza��o, etc.

4. **SemanasPMO** / **TB_SEMANA_PMO** (ou similar)
   - Hist�rico de semanas PMO
   - Datas de refer�ncia

5. **EquipesPDP** / **TB_EQUIPE** (ou similar)
   - Equipes de opera��o reais
   - Coordenadores, contatos

---

## ?? SCRIPT DE MIGRA��O DE DADOS

### Abordagem: SQL Server ? SQL Server (Mesmo Servidor)

```sql
USE PDPW_PoC;
GO

-- ============================================
-- MIGRA��O DE DADOS DO LEGADO PARA POC
-- ============================================

-- 1. TIPOS DE USINA
INSERT INTO TiposUsina (Nome, Descricao, FonteEnergia, DataCriacao, Ativo)
SELECT DISTINCT
    TipoUsina,
    'Migrado do sistema legado',
    FonteEnergia,
    GETDATE(),
    1
FROM PDPW_Legacy.dbo.TB_USINA
WHERE TipoUsina NOT IN (SELECT Nome FROM TiposUsina);

-- 2. EMPRESAS
INSERT INTO Empresas (Nome, CNPJ, Email, Telefone, Endereco, DataCriacao, Ativo)
SELECT DISTINCT
    NomeEmpresa,
    CNPJ,
    Email,
    Telefone,
    Endereco,
    GETDATE(),
    1
FROM PDPW_Legacy.dbo.TB_EMPRESA
WHERE CNPJ NOT IN (SELECT CNPJ FROM Empresas WHERE CNPJ IS NOT NULL);

-- 3. USINAS
INSERT INTO Usinas (Codigo, Nome, TipoUsinaId, EmpresaId, CapacidadeInstalada, 
                    Localizacao, DataOperacao, DataCriacao, Ativo)
SELECT 
    l.CodigoUsina,
    l.NomeUsina,
    t.Id, -- FK para TipoUsina
    e.Id, -- FK para Empresa
    l.CapacidadeInstalada,
    l.Localizacao,
    l.DataOperacao,
    GETDATE(),
    1
FROM PDPW_Legacy.dbo.TB_USINA l
LEFT JOIN TiposUsina t ON l.TipoUsina = t.Nome
LEFT JOIN Empresas e ON l.CNPJEmpresa = e.CNPJ
WHERE l.CodigoUsina NOT IN (SELECT Codigo FROM Usinas);

-- 4. SEMANAS PMO (�ltimos 2 anos)
INSERT INTO SemanasPMO (Numero, DataInicio, DataFim, Ano, DataCriacao, Ativo)
SELECT 
    NumeroSemana,
    DataInicio,
    DataFim,
    YEAR(DataInicio),
    GETDATE(),
    1
FROM PDPW_Legacy.dbo.TB_SEMANA_PMO
WHERE DataInicio >= DATEADD(YEAR, -2, GETDATE())
AND NOT EXISTS (
    SELECT 1 FROM SemanasPMO 
    WHERE Numero = NumeroSemana AND Ano = YEAR(DataInicio)
);

-- 5. EQUIPES PDP
INSERT INTO EquipesPDP (Nome, Descricao, Coordenador, Email, Telefone, DataCriacao, Ativo)
SELECT 
    NomeEquipe,
    Descricao,
    Coordenador,
    Email,
    Telefone,
    GETDATE(),
    1
FROM PDPW_Legacy.dbo.TB_EQUIPE
WHERE NomeEquipe NOT IN (SELECT Nome FROM EquipesPDP);
```

---

## ?? SCRIPT DE AN�LISE DO LEGADO

```sql
-- ============================================
-- AN�LISE DO BANCO LEGADO
-- ============================================

USE PDPW_Legacy;
GO

-- Contagem de registros por tabela
SELECT 
    OBJECT_SCHEMA_NAME(object_id) AS SchemaName,
    OBJECT_NAME(object_id) AS TableName,
    SUM(row_count) AS TotalRows
FROM sys.dm_db_partition_stats
WHERE index_id IN (0,1)
    AND OBJECT_NAME(object_id) NOT LIKE 'sys%'
GROUP BY OBJECT_SCHEMA_NAME(object_id), OBJECT_NAME(object_id)
ORDER BY TotalRows DESC;

-- Estrutura das principais tabelas
EXEC sp_help 'TB_USINA';
EXEC sp_help 'TB_EMPRESA';
EXEC sp_help 'TB_TIPO_USINA';
EXEC sp_help 'TB_SEMANA_PMO';
EXEC sp_help 'TB_EQUIPE';

-- Amostras de dados
SELECT TOP 10 * FROM TB_USINA;
SELECT TOP 10 * FROM TB_EMPRESA;
SELECT TOP 10 * FROM TB_TIPO_USINA;
SELECT TOP 10 * FROM TB_SEMANA_PMO;
SELECT TOP 10 * FROM TB_EQUIPE;
```

---

## ?? CONSIDERA��ES IMPORTANTES

### 1. Compatibilidade de Schema
- ? **Nomes de colunas diferentes** - Precisamos mapear nomes legado ? POC
- ? **Tipos de dados diferentes** - Validar convers�es
- ? **Constraints diferentes** - Adaptar FKs e Unique constraints
- ? **Dados faltantes** - Tratar NULLs e valores padr�o

### 2. Integridade Referencial
- ? Inserir dados na ordem correta (respeitando FKs)
- ? Validar IDs referenciados
- ? Tratar registros �rf�os

### 3. Performance
- ?? Backup de 43 GB pode ter milh�es de registros
- ?? Inser��o em massa pode demorar
- ?? Considerar inser��o em lotes (batches)

### 4. Limpeza de Dados
- ?? Remover dados de teste do legado
- ?? Validar qualidade dos dados
- ?? Normalizar formatos (CNPJ, telefones, emails)

---

## ?? PLANO DE A��O RECOMENDADO

### Fase 1: AN�LISE (30 min)
1. ? Restaurar backup em banco separado `PDPW_Legacy`
2. ? Executar scripts de an�lise
3. ? Documentar estrutura encontrada
4. ? Identificar tabelas relevantes

### Fase 2: MAPEAMENTO (1 hora)
1. ? Criar documento de mapeamento Legado ? POC
2. ? Identificar transforma��es necess�rias
3. ? Validar compatibilidade de dados
4. ? Criar queries de extra��o

### Fase 3: EXTRA��O (2 horas)
1. ? Executar scripts de migra��o
2. ? Validar integridade referencial
3. ? Conferir contagens de registros
4. ? Testar APIs com dados reais

### Fase 4: VALIDA��O (1 hora)
1. ? Testar todos os endpoints
2. ? Validar relacionamentos
3. ? Conferir dados no Swagger
4. ? Documentar discrep�ncias

---

## ?? COMANDOS PARA COME�AR

### 1. Verificar SQL Server Local

```powershell
# Verificar se SQL Server est� rodando
Get-Service -Name "MSSQL*" | Select-Object Name, Status, DisplayName

# Testar conex�o
sqlcmd -S localhost -Q "SELECT @@VERSION"
```

### 2. Restaurar Backup

```powershell
# Via PowerShell (usando SMO)
$backupFile = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
$server = "localhost"
$newDbName = "PDPW_Legacy"

# Comando sqlcmd
sqlcmd -S $server -Q "
RESTORE DATABASE [$newDbName]
FROM DISK = '$backupFile'
WITH 
    FILE = 1,
    MOVE 'PDP_TST' TO 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PDPW_Legacy.mdf',
    MOVE 'PDP_TST_log' TO 'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\PDPW_Legacy_log.ldf',
    NOUNLOAD,
    REPLACE,
    STATS = 10
"
```

### 3. Analisar Estrutura

```powershell
# Listar tabelas
sqlcmd -S localhost -d PDPW_Legacy -Q "
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME
" -o "C:\temp\_ONS_PoC-PDPW\docs\legacy_tables.txt"

# Contar registros
sqlcmd -S localhost -d PDPW_Legacy -Q "
SELECT 
    t.NAME AS TableName,
    SUM(p.rows) AS RowCounts
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE p.index_id < 2
GROUP BY t.NAME
ORDER BY RowCounts DESC
" -o "C:\temp\_ONS_PoC-PDPW\docs\legacy_row_counts.txt"
```

---

## ?? ALTERNATIVAS SE N�O HOUVER SQL SERVER LOCAL

### Op��o A: Usar SQL Server Express LocalDB
```powershell
# J� est� configurado no projeto
# Connection String: Server=(localdb)\\mssqllocaldb;Database=PDPW_PoC;...
```

### Op��o B: Usar SQL Server em Container Docker
```powershell
# Subir SQL Server 2022 em Docker
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Pdpw@2024!" `
  -p 1433:1433 --name pdpw-sqlserver `
  -v C:\temp\_ONS_PoC-PDPW\pdpw_act:/backups `
  -d mcr.microsoft.com/mssql/server:2022-latest

# Aguardar 30 segundos
Start-Sleep -Seconds 30

# Restaurar backup dentro do container
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd `
  -S localhost -U SA -P "Pdpw@2024!" `
  -Q "RESTORE DATABASE PDPW_Legacy FROM DISK = '/backups/Backup_PDP_TST.bak' WITH MOVE 'PDP_TST' TO '/var/opt/mssql/data/PDPW_Legacy.mdf', MOVE 'PDP_TST_log' TO '/var/opt/mssql/data/PDPW_Legacy_log.ldf', REPLACE"
```

### Op��o C: Usar Azure SQL Database
```powershell
# Fazer upload do backup para Azure Storage
# Restaurar usando Azure Portal ou PowerShell
```

---

## ?? PR�XIMOS PASSOS

1. **IMEDIATO**: Verificar se SQL Server est� dispon�vel
2. **CURTO PRAZO**: Restaurar backup e analisar estrutura
3. **M�DIO PRAZO**: Criar scripts de migra��o de dados
4. **LONGO PRAZO**: Popular POC com dados reais

---

## ? DECIS�O R�PIDA

**Gostaria que eu:**

**A)** Tente restaurar o backup agora e analisar a estrutura?  
**B)** Crie apenas os scripts SQL para voc� executar manualmente?  
**C)** Configure SQL Server em Docker e restaure l�?  
**D)** Mantenha apenas os seed data e ignore o backup por enquanto?

---

**Criado em**: 19/12/2024  
**Arquivo de Backup**: Backup_PDP_TST.bak (43.2 GB)  
**Status**: ? Aguardando decis�o

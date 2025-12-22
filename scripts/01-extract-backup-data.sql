-- =============================================
-- Script: Extrair Dados do Backup do Cliente
-- Objetivo: Copiar dados específicos do backup para o banco atual
-- Método: Restaurar com nome temporário e copiar dados
-- =============================================

USE master;
GO

-- =============================================
-- PASSO 1: Restaurar backup em banco temporário
-- =============================================
PRINT '========================================='
PRINT 'PASSO 1: Restaurando backup temporário...'
PRINT '========================================='

-- Verificar e dropar banco temporário se existir
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_TEMP_BACKUP')
BEGIN
    ALTER DATABASE PDPW_TEMP_BACKUP SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PDPW_TEMP_BACKUP;
    PRINT '? Banco temporário anterior removido'
END
GO

-- Restaurar backup em banco temporário (somente leitura, sem logs grandes)
PRINT 'Restaurando backup do cliente em banco temporário...'
PRINT 'Isso pode levar alguns minutos...'

RESTORE DATABASE PDPW_TEMP_BACKUP
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP' TO 'C:\temp\PDPW_TEMP.mdf',
    MOVE 'PDP_log' TO 'C:\temp\PDPW_TEMP_log.ldf',
    MOVE 'PDP_log2' TO 'C:\temp\PDPW_TEMP_log2.ldf',
    MOVE 'PDP_log3' TO 'C:\temp\PDPW_TEMP_log3.ldf',
    REPLACE,
    RECOVERY,
    STATS = 10;
GO

PRINT '? Backup restaurado em PDPW_TEMP_BACKUP'
GO

-- Colocar em modo somente leitura para economizar espaço
ALTER DATABASE PDPW_TEMP_BACKUP SET READ_ONLY;
GO

-- =============================================
-- PASSO 2: Verificar estrutura do backup
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'PASSO 2: Verificando estrutura do backup'
PRINT '========================================='

USE PDPW_TEMP_BACKUP;
GO

-- Listar tabelas disponíveis
SELECT 
    t.name AS Tabela,
    (SELECT COUNT(*) FROM sys.columns c WHERE c.object_id = t.object_id) AS Colunas,
    p.rows AS Registros
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
ORDER BY p.rows DESC;
GO

-- =============================================
-- PASSO 3: Verificar dados específicos
-- =============================================
PRINT ''
PRINT '========================================='
PRINT 'PASSO 3: Analisando dados do backup'
PRINT '========================================='

-- Empresas
IF OBJECT_ID('Empresas', 'U') IS NOT NULL
BEGIN
    PRINT 'EMPRESAS:'
    SELECT TOP 5 
        Id, 
        CNPJ, 
        RazaoSocial, 
        NomeFantasia,
        CASE WHEN Ativo = 1 THEN 'Sim' ELSE 'Não' END AS Ativo
    FROM Empresas
    ORDER BY Id;
END
GO

-- Usinas
IF OBJECT_ID('Usinas', 'U') IS NOT NULL
BEGIN
    PRINT ''
    PRINT 'USINAS:'
    SELECT TOP 5
        Id,
        Codigo,
        Nome,
        TipoUsinaId,
        EmpresaId,
        CASE WHEN Ativo = 1 THEN 'Sim' ELSE 'Não' END AS Ativo
    FROM Usinas
    ORDER BY Id;
END
GO

-- UnidadesGeradoras
IF OBJECT_ID('UnidadesGeradoras', 'U') IS NOT NULL
BEGIN
    PRINT ''
    PRINT 'UNIDADES GERADORAS:'
    SELECT TOP 5
        Id,
        Codigo,
        Nome,
        UsinaId,
        PotenciaNominal,
        CASE WHEN Ativo = 1 THEN 'Sim' ELSE 'Não' END AS Ativo
    FROM UnidadesGeradoras
    ORDER BY Id;
END
GO

PRINT ''
PRINT '========================================='
PRINT 'Verificação concluída!'
PRINT '========================================='
PRINT ''
PRINT 'PRÓXIMOS PASSOS:'
PRINT '1. Revisar estrutura das tabelas'
PRINT '2. Executar script de merge de dados'
PRINT '3. Validar dados copiados'
PRINT '========================================='

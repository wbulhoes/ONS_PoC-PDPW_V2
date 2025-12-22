-- Restaurar Backup do Cliente PDPW
-- SQL Server Express

USE master;
GO

-- Fechar conexões existentes se houver
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_DB')
BEGIN
    ALTER DATABASE PDPW_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PDPW_DB;
END
GO

-- Restaurar o backup
RESTORE DATABASE PDPW_DB
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP' TO 'C:\temp\PDPW_DB.mdf',
    MOVE 'PDP_log' TO 'C:\temp\PDPW_DB_log.ldf',
    MOVE 'PDP_log2' TO 'C:\temp\PDPW_DB_log2.ldf',
    MOVE 'PDP_log3' TO 'C:\temp\PDPW_DB_log3.ldf',
    REPLACE,
    RECOVERY,
    STATS = 10;
GO

-- Configurar banco
ALTER DATABASE PDPW_DB SET MULTI_USER;
GO

ALTER DATABASE PDPW_DB SET RECOVERY SIMPLE;
GO

-- Verificar dados restaurados
USE PDPW_DB;
GO

PRINT '========================================='
PRINT 'DADOS RESTAURADOS:'
PRINT '========================================='

SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas WHERE Ativo = 1
UNION ALL
SELECT 'Usinas', COUNT(*) FROM Usinas WHERE Ativo = 1
UNION ALL
SELECT 'TiposUsina', COUNT(*) FROM TiposUsina WHERE Ativo = 1
UNION ALL
SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO WHERE Ativo = 1
UNION ALL
SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP WHERE Ativo = 1
UNION ALL
SELECT 'Cargas', COUNT(*) FROM Cargas WHERE Ativo = 1
UNION ALL
SELECT 'ArquivosDadger', COUNT(*) FROM ArquivosDadger WHERE Ativo = 1
UNION ALL
SELECT 'UnidadesGeradoras', COUNT(*) FROM UnidadesGeradoras WHERE Ativo = 1
UNION ALL
SELECT 'MotivosRestricao', COUNT(*) FROM MotivosRestricao WHERE Ativo = 1
UNION ALL
SELECT 'RestricoesUG', COUNT(*) FROM RestricoesUG WHERE Ativo = 1
ORDER BY Tabela;
GO

PRINT '========================================='
PRINT 'RESTAURAÇÃO CONCLUÍDA!'
PRINT '========================================='

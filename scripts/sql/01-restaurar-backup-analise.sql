-- =====================================================
-- SCRIPT 1: RESTAURAR BACKUP DO CLIENTE
-- Backup: Backup_PDP_TST.bak
-- Database: PDPW_BACKUP_ANALISE
-- Data: 26/12/2024
-- =====================================================

USE master;
GO

-- Verificar conteúdo do backup
PRINT '========================================';
PRINT 'ANALISANDO BACKUP...';
PRINT '========================================';

RESTORE FILELISTONLY 
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak';
GO

-- Fechar conexões existentes se banco já existir
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_BACKUP_ANALISE')
BEGIN
    PRINT 'Fechando conexões existentes...';
    ALTER DATABASE PDPW_BACKUP_ANALISE SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PDPW_BACKUP_ANALISE;
    PRINT 'Banco anterior removido.';
END
GO

-- Restaurar banco
PRINT '';
PRINT 'Restaurando backup...';

RESTORE DATABASE PDPW_BACKUP_ANALISE
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP' TO 'C:\temp\PDPW_BACKUP_ANALISE.mdf',
    MOVE 'PDP_log' TO 'C:\temp\PDPW_BACKUP_ANALISE_log.ldf',
    MOVE 'PDP_log2' TO 'C:\temp\PDPW_BACKUP_ANALISE_log2.ldf',
    MOVE 'PDP_log3' TO 'C:\temp\PDPW_BACKUP_ANALISE_log3.ldf',
    REPLACE,
    RECOVERY;
GO

PRINT '✅ Backup restaurado com sucesso!';
PRINT '';

USE PDPW_BACKUP_ANALISE;
GO

-- Análise rápida de tabelas
PRINT '========================================';
PRINT 'ANÁLISE DE DADOS DISPONÍVEIS';
PRINT '========================================';
PRINT '';

SELECT 
    t.NAME AS Tabela,
    SUM(p.rows) AS TotalRegistros
FROM 
    sys.tables t
INNER JOIN      
    sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE 
    t.is_ms_shipped = 0
    AND p.index_id IN (0,1)
GROUP BY 
    t.NAME
ORDER BY 
    SUM(p.rows) DESC;
GO

PRINT '';
PRINT '========================================';
PRINT 'BANCO RESTAURADO E PRONTO PARA ANÁLISE!';
PRINT '========================================';

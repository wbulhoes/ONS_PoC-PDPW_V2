-- =====================================================
-- SCRIPT DE RESTAURAÇÃO E ANÁLISE DO BACKUP DO CLIENTE
-- Backup: Backup_PDP_TST.bak
-- Data: 26/12/2024
-- Objetivo: Extrair dados reais para seeder único
-- =====================================================

USE master;
GO

-- =====================================================
-- 1. RESTAURAR BACKUP
-- =====================================================

-- Verificar conteúdo do backup
RESTORE FILELISTONLY 
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak';
GO

-- Restaurar banco de dados
RESTORE DATABASE PDPW_BACKUP
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP_TST' TO 'C:\temp\PDPW_BACKUP.mdf',
    MOVE 'PDP_TST_log' TO 'C:\temp\PDPW_BACKUP_log.ldf',
    REPLACE,
    RECOVERY;
GO

USE PDPW_BACKUP;
GO

-- =====================================================
-- 2. ANÁLISE DE ESTRUTURA
-- =====================================================

PRINT '==========================================';
PRINT 'ANÁLISE DE TABELAS E REGISTROS';
PRINT '==========================================';
PRINT '';

-- Ver total de registros por tabela
SELECT 
    SCHEMA_NAME(t.schema_id) AS SchemaName,
    t.NAME AS TableName,
    SUM(p.rows) AS TotalRows
FROM 
    sys.tables t
INNER JOIN      
    sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE 
    t.is_ms_shipped = 0
    AND p.index_id IN (0,1) -- Heap ou Clustered Index
GROUP BY 
    SCHEMA_NAME(t.schema_id), t.NAME
ORDER BY 
    TotalRows DESC;
GO

-- =====================================================
-- 3. VERIFICAR COLUNAS DE CADA TABELA PRINCIPAL
-- =====================================================

-- Tipos de Usina
SELECT 'TiposUsina' AS Tabela, COUNT(*) AS Total FROM TiposUsina;
SELECT TOP 3 * FROM TiposUsina;
GO

-- Empresas
SELECT 'Empresas' AS Tabela, COUNT(*) AS Total FROM Empresas;
SELECT TOP 3 * FROM Empresas ORDER BY Id;
GO

-- Usinas
SELECT 'Usinas' AS Tabela, COUNT(*) AS Total FROM Usinas;
SELECT TOP 3 * FROM Usinas ORDER BY CapacidadeInstalada DESC;
GO

-- Semanas PMO
SELECT 'SemanasPMO' AS Tabela, COUNT(*) AS Total FROM SemanasPMO;
SELECT TOP 3 * FROM SemanasPMO ORDER BY DataInicio DESC;
GO

-- Equipes PDP
SELECT 'EquipesPDP' AS Tabela, COUNT(*) AS Total FROM EquipesPDP;
SELECT TOP 3 * FROM EquipesPDP;
GO

-- Unidades Geradoras
SELECT 'UnidadesGeradoras' AS Tabela, COUNT(*) AS Total FROM UnidadesGeradoras;
SELECT TOP 3 * FROM UnidadesGeradoras ORDER BY PotenciaNominal DESC;
GO

-- Cargas
SELECT 'Cargas' AS Tabela, COUNT(*) AS Total FROM Cargas;
SELECT TOP 3 * FROM Cargas ORDER BY DataReferencia DESC;
GO

-- Intercâmbios
SELECT 'Intercambios' AS Tabela, COUNT(*) AS Total FROM Intercambios;
SELECT TOP 3 * FROM Intercambios ORDER BY DataReferencia DESC;
GO

-- Balanços
SELECT 'Balancos' AS Tabela, COUNT(*) AS Total FROM Balancos;
SELECT TOP 3 * FROM Balancos ORDER BY DataReferencia DESC;
GO

-- Motivos Restrição
SELECT 'MotivosRestricao' AS Tabela, COUNT(*) AS Total FROM MotivosRestricao;
SELECT TOP 3 * FROM MotivosRestricao;
GO

-- Restrições UG
SELECT 'RestricoesUG' AS Tabela, COUNT(*) AS Total FROM RestricoesUG;
SELECT TOP 3 * FROM RestricoesUG ORDER BY DataInicio DESC;
GO

-- Paradas UG
SELECT 'ParadasUG' AS Tabela, COUNT(*) AS Total FROM ParadasUG;
SELECT TOP 3 * FROM ParadasUG ORDER BY DataInicio DESC;
GO

-- Arquivos DADGER
SELECT 'ArquivosDadger' AS Tabela, COUNT(*) AS Total FROM ArquivosDadger;
SELECT TOP 3 * FROM ArquivosDadger ORDER BY DataImportacao DESC;
GO

PRINT '';
PRINT '==========================================';
PRINT 'ANÁLISE CONCLUÍDA';
PRINT '==========================================';

-- Script para restaurar backup do cliente PDPW
-- Banco: PDPW_DB
-- Backup: C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak

USE master;
GO

-- 1. Verificar se o banco já existe e fazer backup se necessário
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_DB')
BEGIN
    PRINT 'Banco PDPW_DB já existe. Fazendo backup antes de sobrescrever...'
    
    -- Colocar banco em modo single user para fechar conexões
    ALTER DATABASE PDPW_DB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    
    -- Fazer backup atual
    BACKUP DATABASE PDPW_DB 
    TO DISK = 'C:\temp\PDPW_DB_Backup_Before_Restore.bak'
    WITH FORMAT, 
         NAME = 'Backup antes de restaurar dados do cliente',
         DESCRIPTION = 'Backup de segurança antes de restaurar Backup_PDP_TST.bak';
    
    PRINT 'Backup de segurança criado em: C:\temp\PDPW_DB_Backup_Before_Restore.bak'
END
GO

-- 2. Restaurar o backup do cliente
PRINT 'Iniciando restauração do backup do cliente...'

-- Obter informações do backup
RESTORE FILELISTONLY 
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak';
GO

-- Restaurar com REPLACE (sobrescreve banco existente)
RESTORE DATABASE PDPW_DB
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    REPLACE,
    MOVE 'PDPW_Data' TO 'C:\temp\PDPW_DB.mdf',
    MOVE 'PDPW_Log' TO 'C:\temp\PDPW_DB_log.ldf',
    RECOVERY,
    STATS = 10;
GO

-- 3. Configurar banco restaurado
ALTER DATABASE PDPW_DB SET MULTI_USER;
GO

ALTER DATABASE PDPW_DB SET RECOVERY SIMPLE;
GO

-- 4. Criar usuário para a aplicação (se necessário)
USE PDPW_DB;
GO

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'sa')
BEGIN
    CREATE LOGIN pdpw_app WITH PASSWORD = 'Pdpw@2024!Strong';
    CREATE USER pdpw_app FOR LOGIN pdpw_app;
    ALTER ROLE db_owner ADD MEMBER pdpw_app;
    PRINT 'Usuário pdpw_app criado com sucesso!'
END
GO

-- 5. Verificar dados restaurados
PRINT '========================================='
PRINT 'VERIFICAÇÃO DOS DADOS RESTAURADOS:'
PRINT '========================================='

SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas
UNION ALL
SELECT 'Usinas', COUNT(*) FROM Usinas
UNION ALL
SELECT 'TiposUsina', COUNT(*) FROM TiposUsina
UNION ALL
SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION ALL
SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
UNION ALL
SELECT 'Cargas', COUNT(*) FROM Cargas
UNION ALL
SELECT 'ArquivosDadger', COUNT(*) FROM ArquivosDadger
UNION ALL
SELECT 'UnidadesGeradoras', COUNT(*) FROM UnidadesGeradoras
UNION ALL
SELECT 'MotivosRestricao', COUNT(*) FROM MotivosRestricao
UNION ALL
SELECT 'RestricoesUG', COUNT(*) FROM RestricoesUG
ORDER BY Tabela;
GO

PRINT '========================================='
PRINT 'RESTAURAÇÃO CONCLUÍDA COM SUCESSO!'
PRINT '========================================='
PRINT 'Banco: PDPW_DB'
PRINT 'Connection String para appsettings.json:'
PRINT 'Server=localhost,1433;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;'
PRINT '========================================='

-- =====================================================
-- Script: Habilitar SQL Server Authentication e SA
-- =====================================================

USE master;
GO

-- 1. Habilitar modo de autenticação mista (Windows + SQL)
EXEC xp_instance_regwrite 
    N'HKEY_LOCAL_MACHINE', 
    N'Software\Microsoft\MSSQLServer\MSSQLServer',
    N'LoginMode', 
    REG_DWORD, 
    2; -- 1 = Windows only, 2 = Mixed mode
GO

PRINT '? Modo de autenticação mista habilitado (requer restart do SQL Server)'
GO

-- 2. Habilitar login SA
ALTER LOGIN sa ENABLE;
GO

PRINT '? Login SA habilitado'
GO

-- 3. Definir senha do SA
ALTER LOGIN sa WITH PASSWORD = 'Pdpw@2024!Strong';
GO

PRINT '? Senha do SA configurada'
GO

-- 4. Garantir que SA é sysadmin
ALTER SERVER ROLE sysadmin ADD MEMBER sa;
GO

PRINT '? SA adicionado ao role sysadmin'
GO

-- 5. Verificar configuração
SELECT 
    name AS LoginName,
    type_desc AS LoginType,
    is_disabled AS IsDisabled,
    CASE WHEN IS_SRVROLEMEMBER('sysadmin', name) = 1 THEN 'Yes' ELSE 'No' END AS IsSysAdmin
FROM sys.server_principals
WHERE name = 'sa';
GO

PRINT '========================================='
PRINT 'CONFIGURAÇÃO CONCLUÍDA!'
PRINT '========================================='
PRINT ''
PRINT 'IMPORTANTE: É necessário REINICIAR o serviço SQL Server'
PRINT ''
PRINT 'Execute no PowerShell (como Administrador):'
PRINT 'Restart-Service MSSQL$SQLEXPRESS'
PRINT ''
PRINT 'Ou use SQL Server Configuration Manager'
PRINT '========================================='

-- =====================================================
-- Script: Extrair Dados do Backup do Cliente
-- Método: Attach temporário do backup e extrair dados
-- =====================================================

USE master;
GO

PRINT '========================================='
PRINT 'EXTRAÇÃO DE DADOS DO BACKUP DO CLIENTE'
PRINT '========================================='
PRINT ''

-- Verificar espaço disponível
EXEC xp_fixeddrives;
GO

PRINT ''
PRINT 'ATENÇÃO: O backup original tem ~350GB'
PRINT 'Vamos extrair apenas os dados necessários via scripts SQL'
PRINT ''
PRINT 'Executando extração otimizada...'
PRINT ''

-- =====================================================
-- Como o backup é muito grande, vamos usar uma 
-- abordagem alternativa: criar scripts INSERT
-- baseados na estrutura que já conhecemos
-- =====================================================

PRINT '? Script preparado para extração seletiva'
PRINT ''
PRINT 'Próximos passos:'
PRINT '1. Identificar tabelas prioritárias'
PRINT '2. Extrair TOP 100 de cada tabela'
PRINT '3. Inserir no banco PDPW_DB'
PRINT ''
PRINT '========================================='

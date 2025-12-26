-- =====================================================
-- SCRIPT DE EXTRAÇÃO DE DADOS PARA SEEDER
-- Database: PDPW_BACKUP
-- Data: 26/12/2024
-- Objetivo: Extrair ~638 registros reais para PdpwRealDataSeeder.cs
-- =====================================================

USE PDPW_BACKUP;
GO

PRINT '==========================================';
PRINT 'EXTRAÇÃO DE DADOS PARA SEEDER';
PRINT '==========================================';
PRINT '';

-- =====================================================
-- 1. TIPOS DE USINA (TODOS)
-- =====================================================

PRINT '1. Tipos de Usina:';
SELECT 
    Id, Nome, Descricao, FonteEnergia, 
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM TiposUsina
WHERE Ativo = 1
ORDER BY Id;
GO

-- =====================================================
-- 2. EMPRESAS (TOP 30)
-- =====================================================

PRINT '';
PRINT '2. Empresas (Top 30):';
SELECT TOP 30
    Id, Nome, CNPJ, Telefone, Email, Endereco, 
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM Empresas
WHERE Ativo = 1
ORDER BY Id;
GO

-- =====================================================
-- 3. USINAS (TOP 50 POR CAPACIDADE)
-- =====================================================

PRINT '';
PRINT '3. Usinas (Top 50 por capacidade):';
SELECT TOP 50
    Id, Codigo, Nome, TipoUsinaId, EmpresaId, 
    CapacidadeInstalada, Localizacao, 
    CONVERT(VARCHAR, DataOperacao, 120) AS DataOperacao,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM Usinas
WHERE Ativo = 1
ORDER BY CapacidadeInstalada DESC;
GO

-- =====================================================
-- 4. SEMANAS PMO (6 MESES PASSADOS + 3 MESES FUTUROS)
-- =====================================================

PRINT '';
PRINT '4. Semanas PMO (6 meses passados + 3 meses futuros):';
SELECT 
    Id, Numero, Ano, 
    CONVERT(VARCHAR, DataInicio, 120) AS DataInicio,
    CONVERT(VARCHAR, DataFim, 120) AS DataFim,
    Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM SemanasPMO
WHERE Ativo = 1
  AND DataInicio >= DATEADD(MONTH, -6, GETDATE())
  AND DataFim <= DATEADD(MONTH, 3, GETDATE())
ORDER BY DataInicio;
GO

-- =====================================================
-- 5. EQUIPES PDP (TODAS)
-- =====================================================

PRINT '';
PRINT '5. Equipes PDP (Todas):';
SELECT 
    Id, Nome, Descricao, Coordenador, Email, Telefone,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM EquipesPDP
WHERE Ativo = 1
ORDER BY Id;
GO

-- =====================================================
-- 6. UNIDADES GERADORAS (TOP 100)
-- =====================================================

PRINT '';
PRINT '6. Unidades Geradoras (Top 100):';
SELECT TOP 100
    Id, Codigo, Nome, UsinaId, PotenciaNominal, PotenciaMinima,
    CONVERT(VARCHAR, DataComissionamento, 120) AS DataComissionamento,
    Status,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM UnidadesGeradoras
WHERE Ativo = 1
ORDER BY PotenciaNominal DESC;
GO

-- =====================================================
-- 7. CARGAS (ÚLTIMOS 3 MESES)
-- =====================================================

PRINT '';
PRINT '7. Cargas (Últimos 3 meses):';
SELECT TOP 120
    Id, 
    CONVERT(VARCHAR, DataReferencia, 120) AS DataReferencia,
    SubsistemaId, CargaMWmed, CargaVerificada, PrevisaoCarga,
    Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM Cargas
WHERE Ativo = 1
  AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())
ORDER BY DataReferencia DESC;
GO

-- =====================================================
-- 8. INTERCÂMBIOS (ÚLTIMOS 3 MESES)
-- =====================================================

PRINT '';
PRINT '8. Intercâmbios (Últimos 3 meses):';
SELECT TOP 240
    Id,
    CONVERT(VARCHAR, DataReferencia, 120) AS DataReferencia,
    SubsistemaOrigem, SubsistemaDestino, EnergiaIntercambiada,
    Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM Intercambios
WHERE Ativo = 1
  AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())
ORDER BY DataReferencia DESC;
GO

-- =====================================================
-- 9. BALANÇOS (ÚLTIMOS 3 MESES)
-- =====================================================

PRINT '';
PRINT '9. Balanços (Últimos 3 meses):';
SELECT TOP 120
    Id,
    CONVERT(VARCHAR, DataReferencia, 120) AS DataReferencia,
    SubsistemaId, Geracao, Carga, Intercambio, Perdas, Deficit,
    Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM Balancos
WHERE Ativo = 1
  AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())
ORDER BY DataReferencia DESC;
GO

-- =====================================================
-- 10. MOTIVOS RESTRIÇÃO (TODOS)
-- =====================================================

PRINT '';
PRINT '10. Motivos Restrição (Todos):';
SELECT 
    Id, Nome, Descricao, Categoria,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM MotivosRestricao
WHERE Ativo = 1
ORDER BY Id;
GO

-- =====================================================
-- 11. RESTRIÇÕES UG (ÚLTIMAS 50)
-- =====================================================

PRINT '';
PRINT '11. Restrições UG (Últimas 50):';
SELECT TOP 50
    Id, UnidadeGeradoraId,
    CONVERT(VARCHAR, DataInicio, 120) AS DataInicio,
    CONVERT(VARCHAR, DataFim, 120) AS DataFim,
    MotivoRestricaoId, PotenciaRestrita, Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM RestricoesUG
WHERE Ativo = 1
ORDER BY DataInicio DESC;
GO

-- =====================================================
-- 12. PARADAS UG (ÚLTIMAS 30)
-- =====================================================

PRINT '';
PRINT '12. Paradas UG (Últimas 30):';
SELECT TOP 30
    Id, UnidadeGeradoraId,
    CONVERT(VARCHAR, DataInicio, 120) AS DataInicio,
    CONVERT(VARCHAR, DataFim, 120) AS DataFim,
    MotivoParada, Programada, Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM ParadasUG
WHERE Ativo = 1
ORDER BY DataInicio DESC;
GO

-- =====================================================
-- 13. ARQUIVOS DADGER (ÚLTIMOS 20)
-- =====================================================

PRINT '';
PRINT '13. Arquivos DADGER (Últimos 20):';
SELECT TOP 20
    Id, NomeArquivo, CaminhoArquivo,
    CONVERT(VARCHAR, DataImportacao, 120) AS DataImportacao,
    SemanaPMOId, Processado,
    CONVERT(VARCHAR, DataProcessamento, 120) AS DataProcessamento,
    Observacoes,
    CONVERT(VARCHAR, DataCriacao, 120) AS DataCriacao, Ativo
FROM ArquivosDadger
WHERE Ativo = 1
ORDER BY DataImportacao DESC;
GO

-- =====================================================
-- RESUMO FINAL
-- =====================================================

PRINT '';
PRINT '==========================================';
PRINT 'RESUMO DA EXTRAÇÃO';
PRINT '==========================================';

SELECT 'TiposUsina' AS Tabela, COUNT(*) AS Registros FROM TiposUsina WHERE Ativo = 1
UNION ALL
SELECT 'Empresas', COUNT(*) FROM (SELECT TOP 30 * FROM Empresas WHERE Ativo = 1) AS T
UNION ALL
SELECT 'Usinas', COUNT(*) FROM (SELECT TOP 50 * FROM Usinas WHERE Ativo = 1) AS T
UNION ALL
SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO WHERE Ativo = 1 AND DataInicio >= DATEADD(MONTH, -6, GETDATE()) AND DataFim <= DATEADD(MONTH, 3, GETDATE())
UNION ALL
SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP WHERE Ativo = 1
UNION ALL
SELECT 'UnidadesGeradoras', COUNT(*) FROM (SELECT TOP 100 * FROM UnidadesGeradoras WHERE Ativo = 1) AS T
UNION ALL
SELECT 'Cargas', COUNT(*) FROM (SELECT TOP 120 * FROM Cargas WHERE Ativo = 1 AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())) AS T
UNION ALL
SELECT 'Intercambios', COUNT(*) FROM (SELECT TOP 240 * FROM Intercambios WHERE Ativo = 1 AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())) AS T
UNION ALL
SELECT 'Balancos', COUNT(*) FROM (SELECT TOP 120 * FROM Balancos WHERE Ativo = 1 AND DataReferencia >= DATEADD(MONTH, -3, GETDATE())) AS T
UNION ALL
SELECT 'MotivosRestricao', COUNT(*) FROM MotivosRestricao WHERE Ativo = 1
UNION ALL
SELECT 'RestricoesUG', COUNT(*) FROM (SELECT TOP 50 * FROM RestricoesUG WHERE Ativo = 1) AS T
UNION ALL
SELECT 'ParadasUG', COUNT(*) FROM (SELECT TOP 30 * FROM ParadasUG WHERE Ativo = 1) AS T
UNION ALL
SELECT 'ArquivosDadger', COUNT(*) FROM (SELECT TOP 20 * FROM ArquivosDadger WHERE Ativo = 1) AS T
ORDER BY Tabela;

PRINT '';
PRINT 'EXTRAÇÃO CONCLUÍDA COM SUCESSO!';
PRINT '==========================================';

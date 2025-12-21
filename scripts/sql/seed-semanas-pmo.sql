-- ============================================
-- POPULAR TABELA SemanasPMO
-- ============================================
-- Data: 20/12/2024
-- Descrição: Adiciona 10 semanas PMO reais (Nov-Dez 2024 + Jan 2025 sem conflito)
-- ============================================

USE PDPW_DB;
GO

-- Verificar se já existem dados
IF EXISTS (SELECT 1 FROM SemanasPMO WHERE Id >= 50)
BEGIN
    PRINT 'Dados reais já existem. Limpando dados antigos das semanas reais...'
    DELETE FROM SemanasPMO WHERE Id >= 50;
END

-- Habilitar IDENTITY_INSERT
SET IDENTITY_INSERT SemanasPMO ON;

-- Semanas Reais (Novembro e Dezembro 2024 + Janeiro 2025)
INSERT INTO SemanasPMO (Id, Numero, DataInicio, DataFim, Ano, Observacoes, DataCriacao, Ativo)
VALUES 
    -- Novembro 2024
    (50, 44, '2024-11-02', '2024-11-08', 2024, 'Semana Operativa 44/2024', '2024-10-25', 1),
    (51, 45, '2024-11-09', '2024-11-15', 2024, 'Semana Operativa 45/2024', '2024-11-01', 1),
    (52, 46, '2024-11-16', '2024-11-22', 2024, 'Semana Operativa 46/2024', '2024-11-08', 1),
    (53, 47, '2024-11-23', '2024-11-29', 2024, 'Semana Operativa 47/2024', '2024-11-15', 1),
    (54, 48, '2024-11-30', '2024-12-06', 2024, 'Semana Operativa 48/2024', '2024-11-22', 1),
    
    -- Dezembro 2024
    (55, 49, '2024-12-07', '2024-12-13', 2024, 'Semana Operativa 49/2024', '2024-11-29', 1),
    (56, 50, '2024-12-14', '2024-12-20', 2024, 'Semana Operativa 50/2024', '2024-12-06', 1),
    (57, 51, '2024-12-21', '2024-12-27', 2024, 'Semana Operativa 51/2024', '2024-12-13', 1),
    (58, 52, '2024-12-28', '2025-01-03', 2024, 'Semana Operativa 52/2024', '2024-12-20', 1),
    
    -- Janeiro 2025 (apenas semana 4 para não conflitar com seed IDs 1,2,3)
    (62, 4, '2025-01-25', '2025-01-31', 2025, 'Semana Operativa 4/2025', '2025-01-17', 1),
    (63, 5, '2025-02-01', '2025-02-07', 2025, 'Semana Operativa 5/2025', '2025-01-24', 1);

-- Desabilitar IDENTITY_INSERT
SET IDENTITY_INSERT SemanasPMO OFF;

PRINT ''
PRINT '? Dados inseridos com sucesso!'
PRINT ''

-- Verificar dados inseridos
SELECT 
    Id,
    Numero,
    Ano,
    CONVERT(VARCHAR(10), DataInicio, 103) as DataInicio,
    CONVERT(VARCHAR(10), DataFim, 103) as DataFim,
    Ativo
FROM SemanasPMO
ORDER BY Ano DESC, Numero ASC;

-- Mostrar estatísticas
PRINT ''
PRINT '?? Estatísticas:'
SELECT 
    'Total de Semanas PMO' as Metrica,
    COUNT(*) as Valor
FROM SemanasPMO
UNION ALL
SELECT 
    'Semanas de 2024',
    COUNT(*)
FROM SemanasPMO
WHERE Ano = 2024
UNION ALL
SELECT 
    'Semanas de 2025',
    COUNT(*)
FROM SemanasPMO
WHERE Ano = 2025
UNION ALL
SELECT 
    'Semanas Ativas',
    COUNT(*)
FROM SemanasPMO
WHERE Ativo = 1;

GO

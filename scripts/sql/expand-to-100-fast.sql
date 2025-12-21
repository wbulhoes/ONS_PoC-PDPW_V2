-- ============================================
-- EXPANSÃO RÁPIDA - 100 REGISTROS
-- ============================================
-- Insere dados adicionais diretamente no banco Docker
-- Sem necessidade de restaurar backup completo
-- ============================================

USE PDPW_DB;
GO

-- Desabilitar constraints temporariamente
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- ============================================
-- EMPRESAS ADICIONAIS (17 empresas reais)
-- ============================================

SET IDENTITY_INSERT Empresas ON;

INSERT INTO Empresas (Id, Nome, CNPJ, Telefone, Email, DataCriacao, Ativo) VALUES
(101, 'Empresa de Energia do Amazonas', '02341467000120', NULL, NULL, GETDATE(), 1),
(102, 'Companhia Energética de Pernambuco', '10835932000108', NULL, NULL, GETDATE(), 1),
(103, 'Companhia de Eletricidade do Estado da Bahia', '15139629000194', NULL, NULL, GETDATE(), 1),
(104, 'Companhia Energética do Rio Grande do Norte', '08324196000181', NULL, NULL, GETDATE(), 1),
(105, 'CPFL Paulista', '02429144000193', NULL, NULL, GETDATE(), 1),
(106, 'Enel Distribuição São Paulo', '61695227000193', NULL, NULL, GETDATE(), 1),
(107, 'Light Serviços de Eletricidade S.A.', '60444437000171', NULL, NULL, GETDATE(), 1),
(108, 'Companhia Energética de Goiás', '01543032000165', NULL, NULL, GETDATE(), 1),
(109, 'Centrais Elétricas de Santa Catarina', '83878892000156', NULL, NULL, GETDATE(), 1),
(110, 'Enel Distribuição Ceará', '07047251000170', NULL, NULL, GETDATE(), 1),
(111, 'Energisa Mato Grosso do Sul', '02016440000162', NULL, NULL, GETDATE(), 1),
(112, 'Energisa Sergipe', '13092313000105', NULL, NULL, GETDATE(), 1),
(113, 'EDP Espírito Santo Distribuição', '28152650000174', NULL, NULL, GETDATE(), 1),
(114, 'Rio Grande Energia', '02016440000162', NULL, NULL, GETDATE(), 1),
(115, 'Enel Distribuição Rio', '33050196000188', NULL, NULL, GETDATE(), 1),
(116, 'EDP São Paulo Distribuição', '61695731000193', NULL, NULL, GETDATE(), 1),
(117, 'Companhia Energética do Maranhão', '06272793000184', NULL, NULL, GETDATE(), 1);

SET IDENTITY_INSERT Empresas OFF;

PRINT '? 17 empresas adicionadas'
GO

-- ============================================
-- SEMANAS PMO ADICIONAIS (6 semanas)
-- ============================================

SET IDENTITY_INSERT SemanasPMO ON;

INSERT INTO SemanasPMO (Id, Numero, DataInicio, DataFim, Ano, Observacoes, DataCriacao, Ativo) VALUES
(64, 6, '2025-02-08', '2025-02-14', 2025, 'Semana Operativa 6/2025', GETDATE(), 1),
(65, 7, '2025-02-15', '2025-02-21', 2025, 'Semana Operativa 7/2025', GETDATE(), 1),
(66, 8, '2025-02-22', '2025-02-28', 2025, 'Semana Operativa 8/2025', GETDATE(), 1),
(67, 9, '2025-03-01', '2025-03-07', 2025, 'Semana Operativa 9/2025', GETDATE(), 1),
(68, 10, '2025-03-08', '2025-03-14', 2025, 'Semana Operativa 10/2025', GETDATE(), 1),
(69, 11, '2025-03-15', '2025-03-21', 2025, 'Semana Operativa 11/2025', GETDATE(), 1);

SET IDENTITY_INSERT SemanasPMO OFF;

PRINT '? 6 semanas PMO adicionadas'
GO

-- ============================================
-- TIPOS DE USINA ADICIONAIS (3 tipos)
-- ============================================

SET IDENTITY_INSERT TiposUsina ON;

INSERT INTO TiposUsina (Id, Nome, Descricao, FonteEnergia, DataCriacao, Ativo) VALUES
(6, 'Pequena Central Hidrelétrica', 'Usinas hidrelétricas com potência entre 1 e 30 MW', 'Hidráulica', GETDATE(), 1),
(7, 'Central Geradora Hidrelétrica', 'Usinas hidrelétricas com potência até 1 MW', 'Hidráulica', GETDATE(), 1),
(8, 'Biomassa', 'Usinas termelétricas movidas a biomassa', 'Biomassa', GETDATE(), 1);

SET IDENTITY_INSERT TiposUsina OFF;

PRINT '? 3 tipos de usina adicionados'
GO

-- Re-habilitar constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
GO

-- ============================================
-- VERIFICAÇÃO FINAL
-- ============================================

PRINT ''
PRINT '============================================'
PRINT '  EXPANSÃO CONCLUÍDA COM SUCESSO!'
PRINT '============================================'
PRINT ''

SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas
UNION SELECT 'Usinas', COUNT(*) FROM Usinas
UNION SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
UNION SELECT 'TiposUsina', COUNT(*) FROM TiposUsina
UNION SELECT 'TOTAL GERAL', 
    (SELECT COUNT(*) FROM Empresas) +
    (SELECT COUNT(*) FROM Usinas) +
    (SELECT COUNT(*) FROM SemanasPMO) +
    (SELECT COUNT(*) FROM EquipesPDP) +
    (SELECT COUNT(*) FROM TiposUsina)
ORDER BY Tabela;

PRINT ''
PRINT '?? Base POC expandida com dados reais!'
PRINT ''

GO

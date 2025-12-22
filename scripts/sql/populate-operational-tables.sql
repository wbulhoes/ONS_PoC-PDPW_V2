-- ============================================
-- POPULAR TABELAS OPERACIONAIS
-- ============================================
-- Cargas, ArquivosDadger, RestricoesUG
-- Data: 20/12/2024
-- ============================================

USE PDPW_DB;
GO

-- ============================================
-- 1. CARGAS ELÉTRICAS (20 registros)
-- ============================================

PRINT 'Inserindo Cargas Elétricas...'

SET IDENTITY_INSERT Cargas ON;

INSERT INTO Cargas (Id, DataReferencia, SubsistemaId, CargaMWmed, CargaVerificada, PrevisaoCarga, Observacoes, DataCriacao, Ativo)
VALUES
-- Janeiro 2025 - Sudeste
(1, '2025-01-15', 'SE', 45678.50, 45234.20, 46000.00, 'Carga elevada devido a temperatura', GETDATE(), 1),
(2, '2025-01-16', 'SE', 46123.30, 45890.10, 46200.00, 'Pico de verão', GETDATE(), 1),
(3, '2025-01-17', 'SE', 44890.20, 44567.80, 45100.00, 'Quinta-feira normal', GETDATE(), 1),
(4, '2025-01-18', 'SE', 43210.40, 42980.30, 43500.00, 'Sexta-feira', GETDATE(), 1),

-- Janeiro 2025 - Sul
(5, '2025-01-15', 'S', 12345.60, 12210.30, 12500.00, 'Carga normal de verão', GETDATE(), 1),
(6, '2025-01-16', 'S', 12890.40, 12745.20, 13000.00, 'Leve aumento', GETDATE(), 1),
(7, '2025-01-17', 'S', 12567.80, 12430.50, 12700.00, 'Quinta-feira', GETDATE(), 1),
(8, '2025-01-18', 'S', 11980.30, 11850.60, 12200.00, 'Sexta-feira', GETDATE(), 1),

-- Janeiro 2025 - Nordeste
(9, '2025-01-15', 'NE', 18765.40, 18590.20, 19000.00, 'Temperatura elevada', GETDATE(), 1),
(10, '2025-01-16', 'NE', 19234.50, 19080.30, 19400.00, 'Pico de verão', GETDATE(), 1),
(11, '2025-01-17', 'NE', 18890.60, 18720.40, 19100.00, 'Quinta-feira', GETDATE(), 1),
(12, '2025-01-18', 'NE', 18456.70, 18310.20, 18700.00, 'Sexta-feira', GETDATE(), 1),

-- Janeiro 2025 - Norte
(13, '2025-01-15', 'N', 8234.50, 8120.30, 8400.00, 'Carga estável', GETDATE(), 1),
(14, '2025-01-16', 'N', 8456.20, 8340.50, 8600.00, 'Leve aumento', GETDATE(), 1),
(15, '2025-01-17', 'N', 8367.80, 8250.40, 8500.00, 'Quinta-feira', GETDATE(), 1),
(16, '2025-01-18', 'N', 8190.40, 8080.20, 8350.00, 'Sexta-feira', GETDATE(), 1),

-- Dezembro 2024 - Exemplo histórico
(17, '2024-12-20', 'SE', 48567.30, 48234.50, 49000.00, 'Pico anual - final de ano', GETDATE(), 1),
(18, '2024-12-20', 'S', 13456.40, 13280.60, 13700.00, 'Dezembro - alta demanda', GETDATE(), 1),
(19, '2024-12-20', 'NE', 19890.50, 19680.20, 20100.00, 'Final de ano', GETDATE(), 1),
(20, '2024-12-20', 'N', 8890.30, 8760.40, 9000.00, 'Dezembro estável', GETDATE(), 1);

SET IDENTITY_INSERT Cargas OFF;

PRINT '? 20 cargas elétricas inseridas'
GO

-- ============================================
-- 2. ARQUIVOS DADGER (15 registros)
-- ============================================

PRINT 'Inserindo Arquivos DADGER...'

SET IDENTITY_INSERT ArquivosDadger ON;

INSERT INTO ArquivosDadger (Id, NomeArquivo, CaminhoArquivo, DataImportacao, SemanaPMOId, Processado, DataProcessamento, Observacoes, DataCriacao, Ativo)
VALUES
-- Semana 1/2025 (Id = 1)
(1, 'dadger_202501_sem01.dat', '/uploads/2025/01/dadger_202501_sem01.dat', '2025-01-03 08:00:00', 1, 1, '2025-01-03 08:15:00', 'Importado automaticamente do DESSEM', GETDATE(), 1),
(2, 'dadger_202501_sem01_rev1.dat', '/uploads/2025/01/dadger_202501_sem01_rev1.dat', '2025-01-04 10:30:00', 1, 1, '2025-01-04 10:45:00', 'Revisão 1 - Ajustes de UHE Itaipu', GETDATE(), 1),

-- Semana 2/2025 (Id = 2)
(3, 'dadger_202501_sem02.dat', '/uploads/2025/01/dadger_202501_sem02.dat', '2025-01-10 08:00:00', 2, 1, '2025-01-10 08:20:00', 'Importado automaticamente do DESSEM', GETDATE(), 1),
(4, 'dadger_202501_sem02_rev1.dat', '/uploads/2025/01/dadger_202501_sem02_rev1.dat', '2025-01-11 14:00:00', 2, 1, '2025-01-11 14:15:00', 'Revisão 1 - Manutenção UTE Norte Fluminense', GETDATE(), 1),

-- Semana 3/2025 (Id = 3)
(5, 'dadger_202501_sem03.dat', '/uploads/2025/01/dadger_202501_sem03.dat', '2025-01-17 08:00:00', 3, 1, '2025-01-17 08:18:00', 'Importado automaticamente do DESSEM', GETDATE(), 1),
(6, 'dadger_202501_sem03_rev1.dat', '/uploads/2025/01/dadger_202501_sem03_rev1.dat', '2025-01-18 09:30:00', 3, 0, NULL, 'Aguardando processamento - revisão de restrições', GETDATE(), 1),

-- Semana 50/2024 (Id = 56)
(7, 'dadger_202412_sem50.dat', '/uploads/2024/12/dadger_202412_sem50.dat', '2024-12-13 08:00:00', 56, 1, '2024-12-13 08:25:00', 'Importado automaticamente do DESSEM', GETDATE(), 1),
(8, 'dadger_202412_sem50_rev1.dat', '/uploads/2024/12/dadger_202412_sem50_rev1.dat', '2024-12-14 11:00:00', 56, 1, '2024-12-14 11:20:00', 'Revisão 1 - Ajuste de intercâmbios', GETDATE(), 1),
(9, 'dadger_202412_sem50_rev2.dat', '/uploads/2024/12/dadger_202412_sem50_rev2.dat', '2024-12-15 15:45:00', 56, 1, '2024-12-15 16:00:00', 'Revisão 2 - Chuvas no Sul', GETDATE(), 1),

-- Semana 51/2024 (Id = 57)
(10, 'dadger_202412_sem51.dat', '/uploads/2024/12/dadger_202412_sem51.dat', '2024-12-20 08:00:00', 57, 1, '2024-12-20 08:30:00', 'Importado automaticamente do DESSEM', GETDATE(), 1),
(11, 'dadger_202412_sem51_rev1.dat', '/uploads/2024/12/dadger_202412_sem51_rev1.dat', '2024-12-21 10:00:00', 57, 1, '2024-12-21 10:18:00', 'Revisão 1 - Final de ano', GETDATE(), 1),

-- Semana 52/2024 (Id = 58)
(12, 'dadger_202412_sem52.dat', '/uploads/2024/12/dadger_202412_sem52.dat', '2024-12-27 08:00:00', 58, 1, '2024-12-27 08:22:00', 'Última semana de 2024', GETDATE(), 1),

-- Semana 44/2024 (Id = 50)
(13, 'dadger_202411_sem44.dat', '/uploads/2024/11/dadger_202411_sem44.dat', '2024-11-01 08:00:00', 50, 1, '2024-11-01 08:35:00', 'Início de novembro', GETDATE(), 1),

-- Semana 45/2024 (Id = 51)
(14, 'dadger_202411_sem45.dat', '/uploads/2024/11/dadger_202411_sem45.dat', '2024-11-08 08:00:00', 51, 1, '2024-11-08 08:28:00', 'Segunda semana de novembro', GETDATE(), 1),

-- Semana 46/2024 (Id = 52)
(15, 'dadger_202411_sem46.dat', '/uploads/2024/11/dadger_202411_sem46.dat', '2024-11-15 08:00:00', 52, 1, '2024-11-15 08:19:00', 'Terceira semana de novembro', GETDATE(), 1);

SET IDENTITY_INSERT ArquivosDadger OFF;

PRINT '? 15 arquivos DADGER inseridos'
GO

-- ============================================
-- NOTA: RestricoesUG precisa de UnidadeGeradora e MotivoRestricao
-- Essas tabelas ainda não existem no banco, então vamos pular por enquanto
-- ============================================

PRINT ''
PRINT '? AVISO: RestricoesUG não foi populada pois depende de:'
PRINT '  - Tabela UnidadeGeradora (não existe)'
PRINT '  - Tabela MotivoRestricao (não existe)'
PRINT ''

-- ============================================
-- VERIFICAÇÃO FINAL
-- ============================================

PRINT ''
PRINT '============================================'
PRINT '  DADOS OPERACIONAIS INSERIDOS COM SUCESSO'
PRINT '============================================'
PRINT ''

SELECT 'Cargas' as Tabela, COUNT(*) as Total FROM Cargas
UNION SELECT 'ArquivosDadger', COUNT(*) FROM ArquivosDadger
UNION SELECT 'TOTAL', 
    (SELECT COUNT(*) FROM Cargas) +
    (SELECT COUNT(*) FROM ArquivosDadger)
ORDER BY Tabela;

PRINT ''
PRINT '?? Tabelas operacionais populadas!'
PRINT ''
PRINT 'Distribuição:'
PRINT '  • Cargas: 20 registros (4 subsistemas x 5 períodos)'
PRINT '  • ArquivosDadger: 15 arquivos (várias semanas PMO)'
PRINT ''
PRINT 'TOTAL: 35 novos registros adicionados'
PRINT ''

GO

-- ============================================
-- POPULAR TABELAS AUXILIARES + RESTRIÇÕES UG
-- ============================================
-- UnidadesGeradoras, MotivosRestricao, RestricoesUG
-- Data: 20/12/2024
-- ============================================

USE PDPW_DB;
GO

-- ============================================
-- 1. POPULAR MOTIVOS RESTRIÇÃO (10 registros)
-- ============================================

PRINT 'Populando MotivosRestricao...'

SET IDENTITY_INSERT MotivosRestricao ON;

INSERT INTO MotivosRestricao (Id, Nome, Categoria, Descricao, DataCriacao, Ativo)
VALUES
(1, 'Manutenção Preventiva', 'PROGRAMADA', 'Manutenção programada conforme plano anual', GETDATE(), 1),
(2, 'Manutenção Corretiva', 'NAO_PROGRAMADA', 'Reparo emergencial de equipamento', GETDATE(), 1),
(3, 'Falha de Equipamento', 'NAO_PROGRAMADA', 'Falha inesperada em componente crítico', GETDATE(), 1),
(4, 'Restrição Hidráulica', 'OPERACIONAL', 'Limitação por condições hidrológicas', GETDATE(), 1),
(5, 'Restrição de Transmissão', 'OPERACIONAL', 'Limitação por capacidade de escoamento', GETDATE(), 1),
(6, 'Restrição Ambiental', 'REGULATORIA', 'Limitação por questões ambientais/licenciamento', GETDATE(), 1),
(7, 'Restrição de Combustível', 'OPERACIONAL', 'Disponibilidade limitada de combustível', GETDATE(), 1),
(8, 'Teste/Comissionamento', 'PROGRAMADA', 'Testes pós-manutenção ou comissionamento', GETDATE(), 1),
(9, 'Desligamento Total', 'PROGRAMADA', 'Unidade totalmente fora de operação', GETDATE(), 1),
(10, 'Restrição Climática', 'NAO_PROGRAMADA', 'Limitação por condições meteorológicas', GETDATE(), 1);

SET IDENTITY_INSERT MotivosRestricao OFF;

PRINT '? 10 motivos de restrição inseridos'
GO

-- ============================================
-- 2. POPULAR UNIDADES GERADORAS (30 unidades)
-- ============================================

PRINT 'Populando UnidadesGeradoras...'

SET IDENTITY_INSERT UnidadesGeradoras ON;

INSERT INTO UnidadesGeradoras (Id, Codigo, Nome, UsinaId, PotenciaNominal, PotenciaMinima, DataComissionamento, Status, DataCriacao, Ativo)
VALUES
-- Itaipu (UsinaId = 1) - 700 MW cada
(1, 'ITU-G01', 'Itaipu - Unidade Geradora 1', 1, 700.00, 350.00, '1984-05-05', 'OPERANDO', GETDATE(), 1),
(2, 'ITU-G02', 'Itaipu - Unidade Geradora 2', 1, 700.00, 350.00, '1984-06-30', 'OPERANDO', GETDATE(), 1),
(3, 'ITU-G03', 'Itaipu - Unidade Geradora 3', 1, 700.00, 350.00, '1984-09-01', 'OPERANDO', GETDATE(), 1),

-- Belo Monte (UsinaId = 2)
(4, 'BEM-G01', 'Belo Monte - Unidade Geradora 1', 2, 611.11, 300.00, '2016-04-22', 'OPERANDO', GETDATE(), 1),
(5, 'BEM-G02', 'Belo Monte - Unidade Geradora 2', 2, 611.11, 300.00, '2016-05-10', 'OPERANDO', GETDATE(), 1),

-- Tucuruí (UsinaId = 201)
(6, 'TUC-G01', 'Tucuruí - Unidade Geradora 1', 201, 350.00, 175.00, '1984-11-22', 'OPERANDO', GETDATE(), 1),
(7, 'TUC-G02', 'Tucuruí - Unidade Geradora 2', 201, 350.00, 175.00, '1984-12-15', 'OPERANDO', GETDATE(), 1),
(8, 'TUC-G03', 'Tucuruí - Unidade Geradora 3', 201, 350.00, 175.00, '1985-01-20', 'OPERANDO', GETDATE(), 1),

-- Jirau (UsinaId = 202)
(9, 'JIR-G01', 'Jirau - Unidade Geradora 1', 202, 75.00, 37.50, '2013-09-03', 'OPERANDO', GETDATE(), 1),
(10, 'JIR-G02', 'Jirau - Unidade Geradora 2', 202, 75.00, 37.50, '2013-10-15', 'OPERANDO', GETDATE(), 1),

-- Santo Antônio (UsinaId = 203)
(11, 'SAN-G01', 'Santo Antônio - Unidade Geradora 1', 203, 71.40, 35.70, '2012-03-30', 'OPERANDO', GETDATE(), 1),
(12, 'SAN-G02', 'Santo Antônio - Unidade Geradora 2', 203, 71.40, 35.70, '2012-04-25', 'OPERANDO', GETDATE(), 1),

-- Itá (UsinaId = 204)
(13, 'ITA-G01', 'Itá - Unidade Geradora 1', 204, 181.25, 90.60, '2000-09-29', 'OPERANDO', GETDATE(), 1),
(14, 'ITA-G02', 'Itá - Unidade Geradora 2', 204, 181.25, 90.60, '2001-03-15', 'OPERANDO', GETDATE(), 1),

-- Machadinho (UsinaId = 205)
(15, 'MAC-G01', 'Machadinho - Unidade Geradora 1', 205, 142.50, 71.25, '2002-04-26', 'OPERANDO', GETDATE(), 1),
(16, 'MAC-G02', 'Machadinho - Unidade Geradora 2', 205, 142.50, 71.25, '2002-06-10', 'OPERANDO', GETDATE(), 1),

-- Angra 1 (UsinaId = 214)
(17, 'ANG1-G01', 'Angra 1 - Unidade Única', 214, 640.00, 320.00, '1985-04-01', 'OPERANDO', GETDATE(), 1),

-- Angra 2 (UsinaId = 215)
(18, 'ANG2-G01', 'Angra 2 - Unidade Única', 215, 1350.00, 675.00, '2001-02-01', 'OPERANDO', GETDATE(), 1),

-- Piratininga (UsinaId = 216)
(19, 'PIR-G01', 'Piratininga - Unidade Geradora 1', 216, 121.00, 60.50, '1999-12-15', 'OPERANDO', GETDATE(), 1),
(20, 'PIR-G02', 'Piratininga - Unidade Geradora 2', 216, 121.00, 60.50, '2000-03-20', 'OPERANDO', GETDATE(), 1),

-- Santa Cruz (UsinaId = 217)
(21, 'SCR-G01', 'Santa Cruz - Unidade Geradora 1', 217, 250.00, 125.00, '2010-06-01', 'OPERANDO', GETDATE(), 1),
(22, 'SCR-G02', 'Santa Cruz - Unidade Geradora 2', 217, 250.00, 125.00, '2010-08-15', 'OPERANDO', GETDATE(), 1),

-- TermoRio (UsinaId = 218)
(23, 'TER-G01', 'TermoRio - Unidade Geradora 1', 218, 529.00, 264.50, '2010-12-01', 'OPERANDO', GETDATE(), 1),
(24, 'TER-G02', 'TermoRio - Unidade Geradora 2', 218, 529.00, 264.50, '2011-02-10', 'OPERANDO', GETDATE(), 1),

-- Norte Fluminense (UsinaId = 219)
(25, 'NOR-G01', 'Norte Fluminense - Unidade Geradora 1', 219, 434.00, 217.00, '2005-06-01', 'OPERANDO', GETDATE(), 1),
(26, 'NOR-G02', 'Norte Fluminense - Unidade Geradora 2', 219, 434.00, 217.00, '2005-09-15', 'OPERANDO', GETDATE(), 1),

-- Fortaleza (UsinaId = 220)
(27, 'FOR-G01', 'Fortaleza - Unidade Única', 220, 347.00, 173.50, '2008-12-01', 'OPERANDO', GETDATE(), 1),

-- Pecém I (UsinaId = 221)
(28, 'PEC-G01', 'Pecém I - Unidade Geradora 1', 221, 360.00, 180.00, '2012-03-01', 'OPERANDO', GETDATE(), 1),
(29, 'PEC-G02', 'Pecém I - Unidade Geradora 2', 221, 360.00, 180.00, '2012-05-20', 'OPERANDO', GETDATE(), 1),

-- Suape II (UsinaId = 222)
(30, 'SUA-G01', 'Suape II - Unidade Única', 222, 381.00, 190.50, '2013-06-01', 'OPERANDO', GETDATE(), 1);

SET IDENTITY_INSERT UnidadesGeradoras OFF;

PRINT '? 30 unidades geradoras inseridas'
GO

-- ============================================
-- 3. POPULAR RESTRIÇÕES UG (25 registros)
-- ============================================

PRINT 'Populando RestricoesUG...'

SET IDENTITY_INSERT RestricoesUG ON;

INSERT INTO RestricoesUG (Id, UnidadeGeradoraId, DataInicio, DataFim, MotivoRestricaoId, PotenciaRestrita, Observacoes, DataCriacao, Ativo)
VALUES
-- Janeiro 2025 - Manutenções Programadas
(1, 1, '2025-01-10', '2025-01-17', 1, 700.00, 'Manutenção preventiva anual - Revisão completa do gerador', GETDATE(), 1),
(2, 2, '2025-01-24', '2025-01-31', 1, 700.00, 'Manutenção preventiva anual - Troca de rolamentos', GETDATE(), 1),
(3, 6, '2025-01-05', '2025-01-12', 1, 350.00, 'Manutenção preventiva - Revisão do sistema hidráulico', GETDATE(), 1),

-- Janeiro 2025 - Restrições Operativas Hidráulicas
(4, 9, '2025-01-15', '2025-01-20', 4, 55.00, 'Nível do reservatório baixo - Afluência reduzida', GETDATE(), 1),
(5, 11, '2025-01-16', '2025-01-22', 4, 68.00, 'Vazão reduzida - Período de seca', GETDATE(), 1),

-- Janeiro 2025 - Falhas/Emergências
(6, 17, '2025-01-12', '2025-01-15', 3, 640.00, 'Falha no sistema de refrigeração - Reparo emergencial', GETDATE(), 1),
(7, 25, '2025-01-08', '2025-01-10', 3, 289.00, 'Problema na turbina - Manutenção corretiva', GETDATE(), 1),

-- Dezembro 2024 - Histórico
(8, 13, '2024-12-10', '2024-12-20', 1, 181.25, 'Manutenção anual - Revisão completa', GETDATE(), 1),
(9, 15, '2024-12-15', '2024-12-28', 1, 142.50, 'Manutenção de final de ano - Troca de componentes', GETDATE(), 1),

-- Novembro 2024 - Histórico
(10, 14, '2024-11-10', '2024-11-20', 1, 181.25, 'Manutenção programada - Revisão de 10 anos', GETDATE(), 1),
(11, 16, '2024-11-05', '2024-11-15', 1, 142.50, 'Manutenção preventiva - Troca de óleo', GETDATE(), 1),

-- Restrições por Transmissão
(12, 28, '2025-01-05', '2025-01-12', 5, 240.00, 'Manutenção em LT 500kV - Restrição de escoamento', GETDATE(), 1),
(13, 30, '2025-01-18', '2025-01-25', 5, 127.00, 'Contingência no sistema - Limitação de geração', GETDATE(), 1),

-- Restrições Ambientais
(14, 13, '2024-12-01', '2024-12-31', 6, 98.00, 'Vazão ecológica - Preservação ambiental', GETDATE(), 1),

-- Restrições por Combustível (Termelétricas)
(15, 19, '2025-01-20', '2025-01-27', 7, 121.00, 'Fornecimento de gás reduzido - Manutenção no gasoduto', GETDATE(), 1),
(16, 23, '2025-01-08', '2025-01-14', 7, 529.00, 'Entrega de GNL atrasada - Logística de combustível', GETDATE(), 1),

-- Teste/Comissionamento
(17, 4, '2025-01-15', '2025-01-18', 8, 611.11, 'Testes pós-manutenção - Fase de comissionamento', GETDATE(), 1),

-- Desligamento Total
(18, 21, '2024-12-25', '2025-01-05', 9, 250.00, 'Revisão geral completa - Overhaul programado', GETDATE(), 1),
(19, 27, '2024-11-15', '2024-11-30', 9, 347.00, 'Troca de equipamento principal - Substituição de turbina', GETDATE(), 1),

-- Restrições Climáticas (Hidráulicas)
(20, 10, '2025-01-10', '2025-01-12', 10, 45.00, 'Nível d''água insuficiente - Estiagem', GETDATE(), 1),
(21, 12, '2025-01-14', '2025-01-16', 10, 52.00, 'Vazão reduzida por seca - Restrição temporária', GETDATE(), 1),

-- Manutenções Corretivas
(22, 20, '2025-01-06', '2025-01-09', 2, 121.00, 'Reparo de válvula - Vazamento detectado', GETDATE(), 1),
(23, 22, '2024-12-18', '2024-12-22', 2, 250.00, 'Troca emergencial de componente eletrônico', GETDATE(), 1),

-- Restrições Adicionais
(24, 24, '2025-01-11', '2025-01-13', 3, 529.00, 'Falha no sistema de controle - Reparo urgente', GETDATE(), 1),
(25, 26, '2024-11-20', '2024-11-25', 2, 434.00, 'Manutenção corretiva do gerador - Troca de componente', GETDATE(), 1);

SET IDENTITY_INSERT RestricoesUG OFF;

PRINT '? 25 restrições UG inseridas'
GO

-- ============================================
-- VERIFICAÇÃO FINAL
-- ============================================

PRINT ''
PRINT '============================================'
PRINT '  TABELAS AUXILIARES POPULADAS COM SUCESSO'
PRINT '============================================'
PRINT ''

SELECT 'MotivosRestricao' as Tabela, COUNT(*) as Total FROM MotivosRestricao
UNION SELECT 'UnidadesGeradoras', COUNT(*) FROM UnidadesGeradoras
UNION SELECT 'RestricoesUG', COUNT(*) FROM RestricoesUG
UNION SELECT 'TOTAL NOVOS', 
    (SELECT COUNT(*) FROM MotivosRestricao) +
    (SELECT COUNT(*) FROM UnidadesGeradoras) +
    (SELECT COUNT(*) FROM RestricoesUG)
ORDER BY Tabela;

PRINT ''
PRINT '?? Sistema completo com todas as tabelas!'
PRINT ''
PRINT 'Distribuição:'
PRINT '  • MotivosRestricao: 10 registros (categorias de restrição)'
PRINT '  • UnidadesGeradoras: 30 unidades (de 15 usinas)'
PRINT '  • RestricoesUG: 25 restrições (várias datas e motivos)'
PRINT ''
PRINT 'TOTAL: 65 novos registros adicionados'
PRINT ''

-- Contagem total geral
SELECT 'TOTAL GERAL NO BANCO' as Metrica,
    (SELECT COUNT(*) FROM Empresas) +
    (SELECT COUNT(*) FROM Usinas) +
    (SELECT COUNT(*) FROM SemanasPMO) +
    (SELECT COUNT(*) FROM EquipesPDP) +
    (SELECT COUNT(*) FROM TiposUsina) +
    (SELECT COUNT(*) FROM Cargas) +
    (SELECT COUNT(*) FROM ArquivosDadger) +
    (SELECT COUNT(*) FROM MotivosRestricao) +
    (SELECT COUNT(*) FROM UnidadesGeradoras) +
    (SELECT COUNT(*) FROM RestricoesUG) as Total;

GO

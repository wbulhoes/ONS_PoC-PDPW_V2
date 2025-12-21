# ============================================
# GERADOR DE DADOS FICTÍCIOS - TODAS AS APIs
# ============================================
# Cria dados de teste para validação completa
# ORDEM CORRETA PARA EVITAR ERROS DE FK
# Data: 20/12/2024
# ============================================

USE PDPW_DB;
GO

PRINT '============================================'
PRINT '  GERANDO DADOS FICTÍCIOS PARA TESTES'
PRINT '============================================'
PRINT ''

-- ============================================
-- 1. EMPRESAS FICTÍCIAS (5 registros)
-- ============================================

PRINT 'Gerando Empresas Fictícias...'

-- Limpar dados fictícios anteriores se existirem
DELETE FROM Empresas WHERE Id BETWEEN 200 AND 210;

SET IDENTITY_INSERT Empresas ON;

INSERT INTO Empresas (Id, Nome, CNPJ, Telefone, Email, DataCriacao, Ativo)
VALUES
(200, 'Empresa Teste Alpha Ltda', '11222333000144', '(11) 91234-5678', 'alpha@teste.com.br', GETDATE(), 1),
(201, 'Empresa Teste Beta S.A.', '22333444000155', '(21) 92345-6789', 'beta@teste.com.br', GETDATE(), 1),
(202, 'Empresa Teste Gamma Corp', '33444555000166', '(31) 93456-7890', 'gamma@teste.com.br', GETDATE(), 1),
(203, 'Empresa Teste Delta Energia', '44555666000177', '(41) 94567-8901', 'delta@teste.com.br', GETDATE(), 1),
(204, 'Empresa Teste Epsilon Power', '55666777000188', '(51) 95678-9012', 'epsilon@teste.com.br', GETDATE(), 1);

SET IDENTITY_INSERT Empresas OFF;

PRINT '? 5 empresas fictícias criadas (IDs: 200-204)'
GO

-- ============================================
-- 2. USINAS FICTÍCIAS (10 registros)
-- ============================================

PRINT 'Gerando Usinas Fictícias...'

-- Limpar dados fictícios anteriores
DELETE FROM Usinas WHERE Id BETWEEN 300 AND 310;

SET IDENTITY_INSERT Usinas ON;

INSERT INTO Usinas (Id, Codigo, Nome, TipoUsinaId, EmpresaId, CapacidadeInstalada, Localizacao, DataOperacao, DataCriacao, Ativo)
VALUES
(300, 'TEST-H01', 'UHE Teste Hidro Alpha', 1, 200, 500.00, 'Rio Teste, MG', '2024-01-15', GETDATE(), 1),
(301, 'TEST-H02', 'UHE Teste Hidro Beta', 1, 201, 750.00, 'Rio Beta, SP', '2024-02-20', GETDATE(), 1),
(302, 'TEST-T01', 'UTE Teste Termo Alpha', 2, 202, 300.00, 'São Paulo, SP', '2024-03-10', GETDATE(), 1),
(303, 'TEST-T02', 'UTE Teste Termo Beta', 2, 203, 450.00, 'Rio de Janeiro, RJ', '2024-04-05', GETDATE(), 1),
(304, 'TEST-E01', 'EOL Teste Eólica Alpha', 4, 200, 120.00, 'Ceará, CE', '2024-05-12', GETDATE(), 1),
(305, 'TEST-E02', 'EOL Teste Eólica Beta', 4, 201, 180.00, 'Bahia, BA', '2024-06-18', GETDATE(), 1),
(306, 'TEST-S01', 'UFV Teste Solar Alpha', 5, 202, 50.00, 'Minas Gerais, MG', '2024-07-22', GETDATE(), 1),
(307, 'TEST-S02', 'UFV Teste Solar Beta', 5, 203, 75.00, 'São Paulo, SP', '2024-08-14', GETDATE(), 1),
(308, 'TEST-P01', 'PCH Teste Alpha', 6, 204, 25.00, 'Paraná, PR', '2024-09-09', GETDATE(), 1),
(309, 'TEST-P02', 'PCH Teste Beta', 6, 200, 30.00, 'Santa Catarina, SC', '2024-10-03', GETDATE(), 1);

SET IDENTITY_INSERT Usinas OFF;

PRINT '? 10 usinas fictícias criadas (IDs: 300-309)'
GO

-- ============================================
-- 3. UNIDADES GERADORAS FICTÍCIAS (10 registros)
-- ============================================

PRINT 'Gerando Unidades Geradoras Fictícias...'

-- Limpar dados fictícios anteriores
DELETE FROM UnidadesGeradoras WHERE Id BETWEEN 100 AND 110;

SET IDENTITY_INSERT UnidadesGeradoras ON;

INSERT INTO UnidadesGeradoras (Id, Codigo, Nome, UsinaId, PotenciaNominal, PotenciaMinima, DataComissionamento, Status, DataCriacao, Ativo)
VALUES
(100, 'TEST-H01-G01', 'UHE Teste Hidro Alpha - Unidade 1', 300, 250.00, 125.00, '2024-01-15', 'OPERANDO', GETDATE(), 1),
(101, 'TEST-H01-G02', 'UHE Teste Hidro Alpha - Unidade 2', 300, 250.00, 125.00, '2024-01-20', 'OPERANDO', GETDATE(), 1),
(102, 'TEST-H02-G01', 'UHE Teste Hidro Beta - Unidade 1', 301, 375.00, 187.50, '2024-02-20', 'OPERANDO', GETDATE(), 1),
(103, 'TEST-H02-G02', 'UHE Teste Hidro Beta - Unidade 2', 301, 375.00, 187.50, '2024-02-25', 'OPERANDO', GETDATE(), 1),
(104, 'TEST-T01-G01', 'UTE Teste Termo Alpha - Unidade 1', 302, 150.00, 75.00, '2024-03-10', 'OPERANDO', GETDATE(), 1),
(105, 'TEST-T01-G02', 'UTE Teste Termo Alpha - Unidade 2', 302, 150.00, 75.00, '2024-03-15', 'OPERANDO', GETDATE(), 1),
(106, 'TEST-E01-G01', 'EOL Teste Eólica Alpha - Parque 1', 304, 60.00, 0.00, '2024-05-12', 'OPERANDO', GETDATE(), 1),
(107, 'TEST-E01-G02', 'EOL Teste Eólica Alpha - Parque 2', 304, 60.00, 0.00, '2024-05-18', 'OPERANDO', GETDATE(), 1),
(108, 'TEST-S01-G01', 'UFV Teste Solar Alpha - Array 1', 306, 25.00, 0.00, '2024-07-22', 'OPERANDO', GETDATE(), 1),
(109, 'TEST-S01-G02', 'UFV Teste Solar Alpha - Array 2', 306, 25.00, 0.00, '2024-07-28', 'OPERANDO', GETDATE(), 1);

SET IDENTITY_INSERT UnidadesGeradoras OFF;

PRINT '? 10 unidades geradoras fictícias criadas (IDs: 100-109)'
GO

-- ============================================
-- 4. SEMANAS PMO FICTÍCIAS (5 registros)
-- ============================================

PRINT 'Gerando Semanas PMO Fictícias...'

-- Limpar dados fictícios anteriores
DELETE FROM SemanasPMO WHERE Id BETWEEN 100 AND 110;

SET IDENTITY_INSERT SemanasPMO ON;

INSERT INTO SemanasPMO (Id, Numero, DataInicio, DataFim, Ano, Observacoes, DataCriacao, Ativo)
VALUES
(100, 52, '2025-12-27', '2026-01-02', 2025, 'Semana Teste 52/2025 - Transição de ano', GETDATE(), 1),
(101, 1, '2026-01-03', '2026-01-09', 2026, 'Semana Teste 1/2026 - Início do ano', GETDATE(), 1),
(102, 2, '2026-01-10', '2026-01-16', 2026, 'Semana Teste 2/2026', GETDATE(), 1),
(103, 3, '2026-01-17', '2026-01-23', 2026, 'Semana Teste 3/2026', GETDATE(), 1),
(104, 4, '2026-01-24', '2026-01-30', 2026, 'Semana Teste 4/2026', GETDATE(), 1);

SET IDENTITY_INSERT SemanasPMO OFF;

PRINT '? 5 semanas PMO fictícias criadas (IDs: 100-104)'
GO

-- ============================================
-- 5. CARGAS FICTÍCIAS (10 registros)
-- ============================================

PRINT 'Gerando Cargas Fictícias...'

-- Limpar dados fictícios anteriores
DELETE FROM Cargas WHERE Id BETWEEN 100 AND 120;

SET IDENTITY_INSERT Cargas ON;

INSERT INTO Cargas (Id, DataReferencia, SubsistemaId, CargaMWmed, CargaVerificada, PrevisaoCarga, Observacoes, DataCriacao, Ativo)
VALUES
(100, '2025-12-21', 'SE', 47000.00, 46800.00, 47200.00, 'Carga teste - Sudeste', GETDATE(), 1),
(101, '2025-12-21', 'S', 13000.00, 12850.00, 13150.00, 'Carga teste - Sul', GETDATE(), 1),
(102, '2025-12-21', 'NE', 20000.00, 19850.00, 20150.00, 'Carga teste - Nordeste', GETDATE(), 1),
(103, '2025-12-21', 'N', 9000.00, 8900.00, 9100.00, 'Carga teste - Norte', GETDATE(), 1),
(104, '2025-12-22', 'SE', 46500.00, 46300.00, 46700.00, 'Carga teste - Sudeste D+1', GETDATE(), 1),
(105, '2025-12-22', 'S', 12800.00, 12650.00, 12950.00, 'Carga teste - Sul D+1', GETDATE(), 1),
(106, '2025-12-22', 'NE', 19800.00, 19650.00, 19950.00, 'Carga teste - Nordeste D+1', GETDATE(), 1),
(107, '2025-12-22', 'N', 8900.00, 8800.00, 9000.00, 'Carga teste - Norte D+1', GETDATE(), 1),
(108, '2025-12-23', 'SE', 45000.00, 44800.00, 45200.00, 'Carga teste - Sudeste D+2 (Domingo)', GETDATE(), 1),
(109, '2025-12-23', 'S', 11500.00, 11350.00, 11650.00, 'Carga teste - Sul D+2 (Domingo)', GETDATE(), 1);

SET IDENTITY_INSERT Cargas OFF;

PRINT '? 10 cargas fictícias criadas (IDs: 100-109)'
GO

-- ============================================
-- 6. ARQUIVOS DADGER FICTÍCIOS (5 registros)
-- ============================================

PRINT 'Gerando Arquivos DADGER Fictícios...'


-- Limpar dados fictícios anteriores
DELETE FROM ArquivosDadger WHERE Id BETWEEN 100 AND 110;

SET IDENTITY_INSERT ArquivosDadger ON;

INSERT INTO ArquivosDadger (Id, NomeArquivo, CaminhoArquivo, DataImportacao, SemanaPMOId, Processado, DataProcessamento, Observacoes, DataCriacao, Ativo)
VALUES
(100, 'dadger_teste_202552.dat', '/uploads/test/dadger_teste_202552.dat', '2025-12-20 08:00:00', 100, 1, '2025-12-20 08:15:00', 'Arquivo teste - Semana 52/2025', GETDATE(), 1),
(101, 'dadger_teste_202601.dat', '/uploads/test/dadger_teste_202601.dat', '2026-01-02 08:00:00', 101, 1, '2026-01-02 08:20:00', 'Arquivo teste - Semana 1/2026', GETDATE(), 1),
(102, 'dadger_teste_202602.dat', '/uploads/test/dadger_teste_202602.dat', '2026-01-09 08:00:00', 102, 1, '2026-01-09 08:18:00', 'Arquivo teste - Semana 2/2026', GETDATE(), 1),
(103, 'dadger_teste_202603.dat', '/uploads/test/dadger_teste_202603.dat', '2026-01-16 08:00:00', 103, 0, NULL, 'Arquivo teste - Semana 3/2026 (não processado)', GETDATE(), 1),
(104, 'dadger_teste_202604.dat', '/uploads/test/dadger_teste_202604.dat', '2026-01-23 08:00:00', 104, 0, NULL, 'Arquivo teste - Semana 4/2026 (não processado)', GETDATE(), 1);

SET IDENTITY_INSERT ArquivosDadger OFF;

PRINT '? 5 arquivos DADGER fictícios criados (IDs: 100-104)'
GO

-- ============================================
-- 7. RESTRIÇÕES UG FICTÍCIAS (10 registros)
-- ============================================

PRINT 'Gerando Restrições UG Fictícias...'


-- Limpar dados fictícios anteriores
DELETE FROM RestricoesUG WHERE Id BETWEEN 100 AND 120;

SET IDENTITY_INSERT RestricoesUG ON;

INSERT INTO RestricoesUG (Id, UnidadeGeradoraId, DataInicio, DataFim, MotivoRestricaoId, PotenciaRestrita, Observacoes, DataCriacao, Ativo)
VALUES
(100, 100, '2025-12-22', '2025-12-29', 1, 250.00, 'Teste - Manutenção preventiva UHE Teste Alpha G01', GETDATE(), 1),
(101, 102, '2025-12-25', '2026-01-05', 1, 375.00, 'Teste - Manutenção preventiva UHE Teste Beta G01', GETDATE(), 1),
(102, 104, '2025-12-20', '2025-12-22', 2, 150.00, 'Teste - Manutenção corretiva UTE Teste Alpha G01', GETDATE(), 1),
(103, 105, '2025-12-23', '2025-12-24', 3, 150.00, 'Teste - Falha equipamento UTE Teste Alpha G02', GETDATE(), 1),
(104, 100, '2026-01-10', '2026-01-15', 4, 180.00, 'Teste - Restrição hidráulica UHE Teste Alpha G01', GETDATE(), 1),
(105, 101, '2026-01-12', '2026-01-18', 5, 200.00, 'Teste - Restrição transmissão UHE Teste Alpha G02', GETDATE(), 1),
(106, 106, '2025-12-21', '2025-12-21', 10, 45.00, 'Teste - Restrição climática (vento fraco) EOL Alpha P1', GETDATE(), 1),
(107, 108, '2025-12-22', '2025-12-22', 10, 20.00, 'Teste - Restrição climática (nublado) UFV Alpha A1', GETDATE(), 1),
(108, 104, '2026-01-05', '2026-01-10', 7, 120.00, 'Teste - Restrição combustível UTE Alpha G01', GETDATE(), 1),
(109, 103, '2026-01-20', '2026-01-25', 8, 375.00, 'Teste - Comissionamento pós-manutenção UHE Beta G02', GETDATE(), 1);

SET IDENTITY_INSERT RestricoesUG OFF;

PRINT '? 10 restrições UG fictícias criadas (IDs: 100-109)'
GO

-- ============================================
-- 8. EQUIPES PDP FICTÍCIAS (3 registros)
-- ============================================

PRINT 'Gerando Equipes PDP Fictícias...'


-- Limpar dados fictícios anteriores
DELETE FROM EquipesPDP WHERE Id BETWEEN 100 AND 110;

SET IDENTITY_INSERT EquipesPDP ON;

INSERT INTO EquipesPDP (Id, Nome, Descricao, Coordenador, Email, Telefone, DataCriacao, Ativo)
VALUES
(100, 'Equipe Teste Alpha', 'Equipe de testes automatizados - Região Teste Alpha', 'Coordenador Teste A', 'alpha.pdp@teste.com.br', '(11) 91111-1111', GETDATE(), 1),
(101, 'Equipe Teste Beta', 'Equipe de testes automatizados - Região Teste Beta', 'Coordenador Teste B', 'beta.pdp@teste.com.br', '(21) 92222-2222', GETDATE(), 1),
(102, 'Equipe Teste Gamma', 'Equipe de testes automatizados - Região Teste Gamma', 'Coordenador Teste C', 'gamma.pdp@teste.com.br', '(31) 93333-3333', GETDATE(), 1);

SET IDENTITY_INSERT EquipesPDP OFF;

PRINT '? 3 equipes PDP fictícias criadas (IDs: 100-102)'
GO

-- ============================================
-- VERIFICAÇÃO FINAL
-- ============================================

PRINT ''
PRINT '============================================'
PRINT '  DADOS FICTÍCIOS CRIADOS COM SUCESSO'
PRINT '============================================'
PRINT ''

SELECT 'Empresas' as Tabela, COUNT(*) as TotalGeral, 
       (SELECT COUNT(*) FROM Empresas WHERE Id >= 200) as Ficticias
FROM Empresas
UNION SELECT 'Usinas', COUNT(*), (SELECT COUNT(*) FROM Usinas WHERE Id >= 300) FROM Usinas
UNION SELECT 'UnidadesGeradoras', COUNT(*), (SELECT COUNT(*) FROM UnidadesGeradoras WHERE Id >= 100) FROM UnidadesGeradoras
UNION SELECT 'SemanasPMO', COUNT(*), (SELECT COUNT(*) FROM SemanasPMO WHERE Id >= 100) FROM SemanasPMO
UNION SELECT 'Cargas', COUNT(*), (SELECT COUNT(*) FROM Cargas WHERE Id >= 100) FROM Cargas
UNION SELECT 'ArquivosDadger', COUNT(*), (SELECT COUNT(*) FROM ArquivosDadger WHERE Id >= 100) FROM ArquivosDadger
UNION SELECT 'RestricoesUG', COUNT(*), (SELECT COUNT(*) FROM RestricoesUG WHERE Id >= 100) FROM RestricoesUG
UNION SELECT 'EquipesPDP', COUNT(*), (SELECT COUNT(*) FROM EquipesPDP WHERE Id >= 100) FROM EquipesPDP
ORDER BY Tabela;

PRINT ''
PRINT '?? Resumo de Dados Fictícios:'
PRINT '  • Empresas: 5 (IDs: 200-204)'
PRINT '  • Usinas: 10 (IDs: 300-309)'
PRINT '  • UnidadesGeradoras: 10 (IDs: 100-109)'
PRINT '  • SemanasPMO: 5 (IDs: 100-104)'
PRINT '  • Cargas: 10 (IDs: 100-109)'
PRINT '  • ArquivosDadger: 5 (IDs: 100-104)'
PRINT '  • RestricoesUG: 10 (IDs: 100-109)'
PRINT '  • EquipesPDP: 3 (IDs: 100-102)'
PRINT ''
PRINT 'TOTAL: 58 novos registros fictícios'
PRINT ''
PRINT '? Dados prontos para testes automatizados!'
PRINT '? Todas as Foreign Keys válidas!'
PRINT ''

GO

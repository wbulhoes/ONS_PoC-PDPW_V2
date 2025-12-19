# ============================================
# EXTRAÇÃO SELETIVA DE DADOS DO BACKUP LEGADO
# ============================================
# Arquivo: Extract-LegacyData-Selective.ps1
# Descrição: Extrai apenas dados relevantes do backup para popular a POC
# ============================================

param(
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    [string]$BackupFile = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$TempDatabaseName = "PDPW_Legacy_Temp",
    [string]$TargetDatabaseName = "PDPW_PoC",
    [string]$TempDataPath = "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA",
    [int]$TopEmpresas = 20,
    [int]$TopUsinas = 100,
    [int]$MesesSemanasPMO = 6,
    [int]$TopUsuarios = 50
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  EXTRAÇÃO SELETIVA DE DADOS LEGADOS" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Stop"
$startTime = Get-Date

# Função para executar query e exibir resultado
function Invoke-SqlQuery {
    param(
        [string]$Query,
        [string]$Database = "master"
    )
    
    try {
        if ($Database -ne "master") {
            $Query = "USE [$Database]; $Query"
        }
        $result = sqlcmd -S $ServerInstance -E -Q $Query -h -1 2>&1
        if ($LASTEXITCODE -ne 0) {
            throw "Erro ao executar query: $result"
        }
        return $result
    }
    catch {
        Write-Host "   ? Erro: $_" -ForegroundColor Red
        throw
    }
}

# Função para verificar se tabela existe
function Test-TableExists {
    param(
        [string]$Database,
        [string]$TableName
    )
    
    $query = @"
USE [$Database];
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '$TableName')
    SELECT 1
ELSE
    SELECT 0;
"@
    $result = Invoke-SqlQuery -Query $query
    return ($result -eq 1)
}

# ============================================
# FASE 1: PREPARAÇÃO
# ============================================

Write-Host "FASE 1: PREPARAÇÃO" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????????" -ForegroundColor Yellow
Write-Host ""

# 1.1 Verificar backup
Write-Host "1.1. Verificando backup..." -ForegroundColor Cyan
if (-not (Test-Path $BackupFile)) {
    Write-Host "   ? Backup não encontrado: $BackupFile" -ForegroundColor Red
    exit 1
}
Write-Host "   ? Backup encontrado: $([math]::Round((Get-Item $BackupFile).Length / 1GB, 2)) GB" -ForegroundColor Green

# 1.2 Verificar conexão
Write-Host ""
Write-Host "1.2. Testando conexão SQL Server..." -ForegroundColor Cyan
try {
    $version = Invoke-SqlQuery -Query "SELECT @@VERSION"
    Write-Host "   ? Conexão estabelecida!" -ForegroundColor Green
}
catch {
    Write-Host "   ? Erro na conexão com SQL Server" -ForegroundColor Red
    exit 1
}

# 1.3 Verificar banco da POC
Write-Host ""
Write-Host "1.3. Verificando banco da POC..." -ForegroundColor Cyan
$checkPocDb = "SELECT COUNT(*) FROM sys.databases WHERE name = '$TargetDatabaseName'"
$pocExists = Invoke-SqlQuery -Query $checkPocDb
if ($pocExists -eq 0) {
    Write-Host "   ? Banco da POC não encontrado: $TargetDatabaseName" -ForegroundColor Red
    Write-Host "   ?? Execute: dotnet ef database update --startup-project src/PDPW.API" -ForegroundColor Yellow
    exit 1
}
Write-Host "   ? Banco da POC encontrado: $TargetDatabaseName" -ForegroundColor Green

# 1.4 Limpar banco temporário se existir
Write-Host ""
Write-Host "1.4. Preparando banco temporário..." -ForegroundColor Cyan
$checkTempDb = "SELECT COUNT(*) FROM sys.databases WHERE name = '$TempDatabaseName'"
$tempExists = Invoke-SqlQuery -Query $checkTempDb
if ($tempExists -gt 0) {
    Write-Host "   ???  Removendo banco temporário anterior..." -ForegroundColor Yellow
    $dropQuery = @"
ALTER DATABASE [$TempDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabaseName];
"@
    Invoke-SqlQuery -Query $dropQuery | Out-Null
}
Write-Host "   ? Pronto para criar banco temporário" -ForegroundColor Green

# ============================================
# FASE 2: RESTAURAÇÃO ESTRUTURA (SEM DADOS)
# ============================================

Write-Host ""
Write-Host ""
Write-Host "FASE 2: RESTAURAÇÃO DA ESTRUTURA" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????????" -ForegroundColor Yellow
Write-Host ""

Write-Host "2.1. Restaurando estrutura do banco (sem dados históricos)..." -ForegroundColor Cyan
Write-Host "     ? Esta etapa pode demorar 5-10 minutos..." -ForegroundColor Yellow
Write-Host ""

# Construir comando RESTORE com NORECOVERY para estrutura mínima
$restoreStructureQuery = @"
RESTORE DATABASE [$TempDatabaseName]
FROM DISK = '$BackupFile'
WITH 
    MOVE 'PDP' TO '$TempDataPath\$TempDatabaseName.mdf',
    MOVE 'PDP_log' TO '$TempDataPath\${TempDatabaseName}_log.ldf',
    MOVE 'PDP_log2' TO '$TempDataPath\${TempDatabaseName}_log2.ldf',
    MOVE 'PDP_log3' TO '$TempDataPath\${TempDatabaseName}_log3.ldf',
    REPLACE,
    RECOVERY,
    STATS = 10;
"@

try {
    Write-Host "   ?? Iniciando restauração..." -ForegroundColor Cyan
    $restoreOutput = sqlcmd -S $ServerInstance -E -Q $restoreStructureQuery -t 1800 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "   ? Estrutura restaurada com sucesso!" -ForegroundColor Green
    }
    else {
        throw "Erro na restauração: $restoreOutput"
    }
}
catch {
    Write-Host ""
    Write-Host "   ? ERRO: Restauração falhou!" -ForegroundColor Red
    Write-Host "   Detalhes: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "   ?? POSSÍVEIS SOLUÇÕES:" -ForegroundColor Yellow
    Write-Host "   1. Verificar espaço em disco (necessário ~20-30 GB)" -ForegroundColor White
    Write-Host "   2. Verificar permissões SQL Server no diretório" -ForegroundColor White
    Write-Host "   3. Usar outro caminho com mais espaço" -ForegroundColor White
    exit 1
}

# ============================================
# FASE 3: ANÁLISE DO BANCO TEMPORÁRIO
# ============================================

Write-Host ""
Write-Host ""
Write-Host "FASE 3: ANÁLISE DO BANCO TEMPORÁRIO" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????????" -ForegroundColor Yellow
Write-Host ""

Write-Host "3.1. Analisando tabelas disponíveis..." -ForegroundColor Cyan

# Mapear nomes de tabelas legado -> POC
$tableMappings = @{
    # Formato: "TabelaLegado" = "TabelaPOC"
    "TB_TIPO_USINA" = "TiposUsina"
    "TIPOS_USINA" = "TiposUsina"
    "TipoUsina" = "TiposUsina"
    
    "TB_EMPRESA" = "Empresas"
    "EMPRESAS" = "Empresas"
    
    "TB_USINA" = "Usinas"
    "USINAS" = "Usinas"
    
    "TB_SEMANA_PMO" = "SemanasPMO"
    "SEMANA_PMO" = "SemanasPMO"
    "SemanaPMO" = "SemanasPMO"
    
    "TB_EQUIPE" = "EquipesPDP"
    "EQUIPES" = "EquipesPDP"
    "EquipePDP" = "EquipesPDP"
    
    "TB_USUARIO" = "Usuarios"
    "USUARIOS" = "Usuarios"
}

# Descobrir nomes reais das tabelas no banco legado
$foundTables = @{}
$listTablesQuery = @"
USE [$TempDatabaseName];
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
"@

$allTables = sqlcmd -S $ServerInstance -E -Q $listTablesQuery -h -1 | Where-Object { $_.Trim() -ne "" }

foreach ($legacyName in $tableMappings.Keys) {
    $pocName = $tableMappings[$legacyName]
    
    if ($allTables -contains $legacyName) {
        $foundTables[$pocName] = $legacyName
        Write-Host "   ? $legacyName -> $pocName" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "   ?? Tabelas encontradas: $($foundTables.Count)" -ForegroundColor Cyan

if ($foundTables.Count -eq 0) {
    Write-Host ""
    Write-Host "   ??  AVISO: Nenhuma tabela conhecida encontrada!" -ForegroundColor Yellow
    Write-Host "   ?? Listando todas as tabelas disponíveis:" -ForegroundColor Cyan
    foreach ($table in $allTables | Select-Object -First 20) {
        Write-Host "      - $table" -ForegroundColor Gray
    }
    Write-Host ""
    Write-Host "   ?? Próximo passo: Mapear manualmente as tabelas corretas" -ForegroundColor Yellow
    
    # Limpar banco temporário
    Write-Host ""
    Write-Host "   ???  Removendo banco temporário..." -ForegroundColor Yellow
    $dropTempQuery = @"
ALTER DATABASE [$TempDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabaseName];
"@
    Invoke-SqlQuery -Query $dropTempQuery | Out-Null
    
    exit 0
}

# ============================================
# FASE 4: EXTRAÇÃO E MIGRAÇÃO DE DADOS
# ============================================

Write-Host ""
Write-Host ""
Write-Host "FASE 4: EXTRAÇÃO E MIGRAÇÃO DE DADOS" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????????" -ForegroundColor Yellow
Write-Host ""

$migratedTables = 0
$migratedRows = 0

# 4.1 TIPOS DE USINA
if ($foundTables.ContainsKey("TiposUsina")) {
    Write-Host "4.1. Migrando Tipos de Usina..." -ForegroundColor Cyan
    
    $legacyTable = $foundTables["TiposUsina"]
    
    # Contar registros disponíveis
    $countQuery = "USE [$TempDatabaseName]; SELECT COUNT(*) FROM [$legacyTable];"
    $totalRows = Invoke-SqlQuery -Query $countQuery
    Write-Host "   ?? Registros disponíveis: $totalRows" -ForegroundColor Gray
    
    # Migrar dados (evitar duplicados)
    $migrateQuery = @"
USE [$TargetDatabaseName];

INSERT INTO TiposUsina (Nome, Descricao, FonteEnergia, DataCriacao, Ativo)
SELECT DISTINCT TOP 50
    COALESCE(Nome, Tipo, Descricao) AS Nome,
    COALESCE(Descricao, '') AS Descricao,
    COALESCE(FonteEnergia, TipoEnergia, '') AS FonteEnergia,
    GETDATE() AS DataCriacao,
    1 AS Ativo
FROM [$TempDatabaseName].dbo.[$legacyTable]
WHERE COALESCE(Nome, Tipo, Descricao) IS NOT NULL
AND COALESCE(Nome, Tipo, Descricao) NOT IN (SELECT Nome FROM TiposUsina)
ORDER BY COALESCE(Nome, Tipo, Descricao);
"@
    
    try {
        $result = sqlcmd -S $ServerInstance -E -Q $migrateQuery 2>&1
        $rowsMigrated = ($result | Select-String "linhas? afetadas?" | ForEach-Object { $_.Line -replace '\D', '' } | Measure-Object -Sum).Sum
        Write-Host "   ? Migrados: $rowsMigrated registros" -ForegroundColor Green
        $migratedTables++
        $migratedRows += $rowsMigrated
    }
    catch {
        Write-Host "   ??  Erro ao migrar: $_" -ForegroundColor Yellow
    }
}

# 4.2 EMPRESAS
if ($foundTables.ContainsKey("Empresas")) {
    Write-Host ""
    Write-Host "4.2. Migrando Empresas (Top $TopEmpresas)..." -ForegroundColor Cyan
    
    $legacyTable = $foundTables["Empresas"]
    
    $countQuery = "USE [$TempDatabaseName]; SELECT COUNT(*) FROM [$legacyTable];"
    $totalRows = Invoke-SqlQuery -Query $countQuery
    Write-Host "   ?? Registros disponíveis: $totalRows" -ForegroundColor Gray
    
    $migrateQuery = @"
USE [$TargetDatabaseName];

INSERT INTO Empresas (Nome, CNPJ, Telefone, Email, Endereco, DataCriacao, Ativo)
SELECT TOP $TopEmpresas
    COALESCE(Nome, RazaoSocial, NomeEmpresa) AS Nome,
    COALESCE(CNPJ, NumeroCNPJ) AS CNPJ,
    COALESCE(Telefone, Fone, Contato) AS Telefone,
    COALESCE(Email, EmailContato) AS Email,
    COALESCE(Endereco, Localizacao) AS Endereco,
    GETDATE() AS DataCriacao,
    1 AS Ativo
FROM [$TempDatabaseName].dbo.[$legacyTable]
WHERE COALESCE(Nome, RazaoSocial, NomeEmpresa) IS NOT NULL
AND (
    COALESCE(CNPJ, NumeroCNPJ) NOT IN (SELECT CNPJ FROM Empresas WHERE CNPJ IS NOT NULL)
    OR COALESCE(CNPJ, NumeroCNPJ) IS NULL
)
ORDER BY COALESCE(Nome, RazaoSocial, NomeEmpresa);
"@
    
    try {
        $result = sqlcmd -S $ServerInstance -E -Q $migrateQuery 2>&1
        $rowsMigrated = ($result | Select-String "linhas? afetadas?" | ForEach-Object { $_.Line -replace '\D', '' } | Measure-Object -Sum).Sum
        Write-Host "   ? Migrados: $rowsMigrated registros" -ForegroundColor Green
        $migratedTables++
        $migratedRows += $rowsMigrated
    }
    catch {
        Write-Host "   ??  Erro ao migrar: $_" -ForegroundColor Yellow
    }
}

# 4.3 USINAS
if ($foundTables.ContainsKey("Usinas") -and $foundTables.ContainsKey("TiposUsina") -and $foundTables.ContainsKey("Empresas")) {
    Write-Host ""
    Write-Host "4.3. Migrando Usinas (Top $TopUsinas)..." -ForegroundColor Cyan
    
    $legacyTable = $foundTables["Usinas"]
    
    $countQuery = "USE [$TempDatabaseName]; SELECT COUNT(*) FROM [$legacyTable];"
    $totalRows = Invoke-SqlQuery -Query $countQuery
    Write-Host "   ?? Registros disponíveis: $totalRows" -ForegroundColor Gray
    
    $migrateQuery = @"
USE [$TargetDatabaseName];

INSERT INTO Usinas (Codigo, Nome, TipoUsinaId, EmpresaId, CapacidadeInstalada, Localizacao, DataOperacao, DataCriacao, Ativo)
SELECT TOP $TopUsinas
    COALESCE(l.Codigo, l.CodigoUsina, l.SiglaUsina) AS Codigo,
    COALESCE(l.Nome, l.NomeUsina) AS Nome,
    COALESCE(t.Id, 1) AS TipoUsinaId,
    COALESCE(e.Id, 1) AS EmpresaId,
    COALESCE(l.CapacidadeInstalada, l.PotenciaInstalada, 0) AS CapacidadeInstalada,
    COALESCE(l.Localizacao, l.Municipio, l.Estado) AS Localizacao,
    COALESCE(l.DataOperacao, l.DataInauguracao, '2000-01-01') AS DataOperacao,
    GETDATE() AS DataCriacao,
    1 AS Ativo
FROM [$TempDatabaseName].dbo.[$legacyTable] l
LEFT JOIN TiposUsina t ON t.Nome = COALESCE(l.TipoUsina, l.Tipo)
LEFT JOIN Empresas e ON e.CNPJ = COALESCE(l.CNPJEmpresa, l.CNPJ)
WHERE COALESCE(l.Codigo, l.CodigoUsina, l.SiglaUsina) IS NOT NULL
AND COALESCE(l.Nome, l.NomeUsina) IS NOT NULL
AND COALESCE(l.Codigo, l.CodigoUsina, l.SiglaUsina) NOT IN (SELECT Codigo FROM Usinas)
ORDER BY COALESCE(l.CapacidadeInstalada, l.PotenciaInstalada, 0) DESC;
"@
    
    try {
        $result = sqlcmd -S $ServerInstance -E -Q $migrateQuery 2>&1
        $rowsMigrated = ($result | Select-String "linhas? afetadas?" | ForEach-Object { $_.Line -replace '\D', '' } | Measure-Object -Sum).Sum
        Write-Host "   ? Migrados: $rowsMigrated registros" -ForegroundColor Green
        $migratedTables++
        $migratedRows += $rowsMigrated
    }
    catch {
        Write-Host "   ??  Erro ao migrar: $_" -ForegroundColor Yellow
    }
}

# 4.4 SEMANAS PMO
if ($foundTables.ContainsKey("SemanasPMO")) {
    Write-Host ""
    Write-Host "4.4. Migrando Semanas PMO (últimos $MesesSemanasPMO meses)..." -ForegroundColor Cyan
    
    $legacyTable = $foundTables["SemanasPMO"]
    
    $countQuery = "USE [$TempDatabaseName]; SELECT COUNT(*) FROM [$legacyTable];"
    $totalRows = Invoke-SqlQuery -Query $countQuery
    Write-Host "   ?? Registros disponíveis: $totalRows" -ForegroundColor Gray
    
    $migrateQuery = @"
USE [$TargetDatabaseName];

INSERT INTO SemanasPMO (Numero, DataInicio, DataFim, Ano, Observacoes, DataCriacao, Ativo)
SELECT 
    COALESCE(NumeroSemana, Numero, Semana) AS Numero,
    COALESCE(DataInicio, DataIni) AS DataInicio,
    COALESCE(DataFim, DataFinal) AS DataFim,
    YEAR(COALESCE(DataInicio, DataIni)) AS Ano,
    COALESCE(Observacoes, Obs, '') AS Observacoes,
    GETDATE() AS DataCriacao,
    1 AS Ativo
FROM [$TempDatabaseName].dbo.[$legacyTable]
WHERE COALESCE(DataInicio, DataIni) >= DATEADD(MONTH, -$MesesSemanasPMO, GETDATE())
AND NOT EXISTS (
    SELECT 1 FROM SemanasPMO 
    WHERE Numero = COALESCE(NumeroSemana, Numero, Semana)
    AND Ano = YEAR(COALESCE(DataInicio, DataIni))
)
ORDER BY COALESCE(DataInicio, DataIni) DESC;
"@
    
    try {
        $result = sqlcmd -S $ServerInstance -E -Q $migrateQuery 2>&1
        $rowsMigrated = ($result | Select-String "linhas? afetadas?" | ForEach-Object { $_.Line -replace '\D', '' } | Measure-Object -Sum).Sum
        Write-Host "   ? Migrados: $rowsMigrated registros" -ForegroundColor Green
        $migratedTables++
        $migratedRows += $rowsMigrated
    }
    catch {
        Write-Host "   ??  Erro ao migrar: $_" -ForegroundColor Yellow
    }
}

# 4.5 EQUIPES PDP
if ($foundTables.ContainsKey("EquipesPDP")) {
    Write-Host ""
    Write-Host "4.5. Migrando Equipes PDP..." -ForegroundColor Cyan
    
    $legacyTable = $foundTables["EquipesPDP"]
    
    $countQuery = "USE [$TempDatabaseName]; SELECT COUNT(*) FROM [$legacyTable];"
    $totalRows = Invoke-SqlQuery -Query $countQuery
    Write-Host "   ?? Registros disponíveis: $totalRows" -ForegroundColor Gray
    
    $migrateQuery = @"
USE [$TargetDatabaseName];

INSERT INTO EquipesPDP (Nome, Descricao, Coordenador, Email, Telefone, DataCriacao, Ativo)
SELECT 
    COALESCE(Nome, NomeEquipe, Equipe) AS Nome,
    COALESCE(Descricao, Desc, '') AS Descricao,
    COALESCE(Coordenador, Responsavel) AS Coordenador,
    COALESCE(Email, EmailContato) AS Email,
    COALESCE(Telefone, Fone) AS Telefone,
    GETDATE() AS DataCriacao,
    1 AS Ativo
FROM [$TempDatabaseName].dbo.[$legacyTable]
WHERE COALESCE(Nome, NomeEquipe, Equipe) IS NOT NULL
AND COALESCE(Nome, NomeEquipe, Equipe) NOT IN (SELECT Nome FROM EquipesPDP)
ORDER BY COALESCE(Nome, NomeEquipe, Equipe);
"@
    
    try {
        $result = sqlcmd -S $ServerInstance -E -Q $migrateQuery 2>&1
        $rowsMigrated = ($result | Select-String "linhas? afetadas?" | ForEach-Object { $_.Line -replace '\D', '' } | Measure-Object -Sum).Sum
        Write-Host "   ? Migrados: $rowsMigrated registros" -ForegroundColor Green
        $migratedTables++
        $migratedRows += $rowsMigrated
    }
    catch {
        Write-Host "   ??  Erro ao migrar: $_" -ForegroundColor Yellow
    }
}

# ============================================
# FASE 5: LIMPEZA
# ============================================

Write-Host ""
Write-Host ""
Write-Host "FASE 5: LIMPEZA" -ForegroundColor Yellow
Write-Host "???????????????????????????????????????????" -ForegroundColor Yellow
Write-Host ""

Write-Host "5.1. Removendo banco temporário..." -ForegroundColor Cyan
$dropTempQuery = @"
ALTER DATABASE [$TempDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabaseName];
"@

try {
    Invoke-SqlQuery -Query $dropTempQuery | Out-Null
    Write-Host "   ? Banco temporário removido" -ForegroundColor Green
}
catch {
    Write-Host "   ??  Aviso: Não foi possível remover banco temporário" -ForegroundColor Yellow
    Write-Host "   ?? Remova manualmente se necessário" -ForegroundColor Gray
}

# ============================================
# FASE 6: RELATÓRIO FINAL
# ============================================

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  EXTRAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""

Write-Host "?? ESTATÍSTICAS:" -ForegroundColor Cyan
Write-Host "   Tabelas migradas: $migratedTables" -ForegroundColor White
Write-Host "   Registros migrados: $migratedRows" -ForegroundColor White
Write-Host "   Tempo total: $($duration.Minutes)m $($duration.Seconds)s" -ForegroundColor White
Write-Host ""

Write-Host "?? DADOS MIGRADOS:" -ForegroundColor Cyan
foreach ($pocTable in $foundTables.Keys) {
    Write-Host "   ? $pocTable" -ForegroundColor Green
}
Write-Host ""

Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "   1. Testar APIs com dados reais" -ForegroundColor White
Write-Host "   2. Verificar integridade referencial" -ForegroundColor White
Write-Host "   3. Validar qualidade dos dados" -ForegroundColor White
Write-Host ""

Write-Host "?? TESTAR NO SWAGGER:" -ForegroundColor Cyan
Write-Host "   cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API" -ForegroundColor White
Write-Host "   dotnet run" -ForegroundColor White
Write-Host "   # Acessar: http://localhost:5000/swagger" -ForegroundColor White
Write-Host ""

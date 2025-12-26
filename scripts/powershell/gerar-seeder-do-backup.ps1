# =====================================================
# SCRIPT POWERSHELL - GERADOR DE SEEDER A PARTIR DO BACKUP
# Data: 26/12/2024
# Objetivo: Automatizar extração e geração do PdpwRealDataSeeder.cs
# =====================================================

param(
    [string]$ServerInstance = "localhost",
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$OutputPath = "C:\temp\_ONS_PoC-PDPW_V2\src\PDPW.Infrastructure\Data\Seed\PdpwRealDataSeeder.cs"
)

$ErrorActionPreference = "Stop"

Write-Host "`n=========================================="  -ForegroundColor Cyan
Write-Host "GERADOR DE SEEDER A PARTIR DO BACKUP REAL" -ForegroundColor Cyan
Write-Host "==========================================`n" -ForegroundColor Cyan

# =====================================================
# 1. VERIFICAR PRÉ-REQUISITOS
# =====================================================

Write-Host "1. Verificando pré-requisitos..." -ForegroundColor Yellow

# Verificar se SqlServer module está instalado
if (-not (Get-Module -ListAvailable -Name SqlServer)) {
    Write-Host "  ⚠️  Instalando módulo SqlServer..." -ForegroundColor Yellow
    Install-Module -Name SqlServer -Force -AllowClobber -Scope CurrentUser
}

Import-Module SqlServer -ErrorAction Stop
Write-Host "  ✅ Módulo SqlServer OK" -ForegroundColor Green

# Verificar se backup existe
if (-not (Test-Path $BackupPath)) {
    Write-Host "  ❌ Backup não encontrado: $BackupPath" -ForegroundColor Red
    exit 1
}
Write-Host "  ✅ Backup encontrado: $BackupPath" -ForegroundColor Green

# Testar conexão SQL Server
try {
    $testQuery = "SELECT @@VERSION"
    Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $testQuery -QueryTimeout 5 -TrustServerCertificate | Out-Null
    Write-Host "  ✅ Conexão SQL Server OK: $ServerInstance" -ForegroundColor Green
}
catch {
    Write-Host "  ❌ Não foi possível conectar ao SQL Server: $ServerInstance" -ForegroundColor Red
    Write-Host "     Erro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# =====================================================
# 2. RESTAURAR BACKUP
# =====================================================

Write-Host "`n2. Restaurando backup..." -ForegroundColor Yellow

$restoreScript = @"
USE master;

-- Fechar conexões existentes
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_BACKUP')
BEGIN
    ALTER DATABASE PDPW_BACKUP SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PDPW_BACKUP;
END

-- Verificar arquivos lógicos do backup
RESTORE FILELISTONLY FROM DISK = '$BackupPath';
"@

try {
    $fileList = Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $restoreScript -QueryTimeout 30
    
    if ($fileList) {
        $dataFile = $fileList[0].LogicalName
        $logFile = $fileList[1].LogicalName
        
        Write-Host "  Arquivos lógicos encontrados:" -ForegroundColor Gray
        Write-Host "    - Data: $dataFile" -ForegroundColor Gray
        Write-Host "    - Log: $logFile" -ForegroundColor Gray
        
        # Restaurar banco
        $restoreSql = @"
RESTORE DATABASE PDPW_BACKUP
FROM DISK = '$BackupPath'
WITH 
    MOVE '$dataFile' TO 'C:\temp\PDPW_BACKUP.mdf',
    MOVE '$logFile' TO 'C:\temp\PDPW_BACKUP_log.ldf',
    REPLACE,
    RECOVERY;
"@
        
        Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $restoreSql -QueryTimeout 120
        Write-Host "  ✅ Backup restaurado com sucesso: PDPW_BACKUP" -ForegroundColor Green
    }
    else {
        Write-Host "  ⚠️  Usando banco existente PDPW_BACKUP" -ForegroundColor Yellow
    }
}
catch {
    Write-Host "  ⚠️  Erro ao restaurar backup, tentando usar banco existente..." -ForegroundColor Yellow
    Write-Host "     $($_.Exception.Message)" -ForegroundColor Gray
}

# =====================================================
# 3. ANALISAR DADOS
# =====================================================

Write-Host "`n3. Analisando estrutura de dados..." -ForegroundColor Yellow

$analysisQuery = @"
USE PDPW_BACKUP;

SELECT 
    t.NAME AS TableName,
    SUM(p.rows) AS TotalRows
FROM 
    sys.tables t
INNER JOIN      
    sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE 
    t.is_ms_shipped = 0
    AND p.index_id IN (0,1)
    AND t.NAME IN ('TiposUsina', 'Empresas', 'Usinas', 'SemanasPMO', 
                   'EquipesPDP', 'UnidadesGeradoras', 'Cargas', 
                   'Intercambios', 'Balancos', 'MotivosRestricao',
                   'RestricoesUG', 'ParadasUG', 'ArquivosDadger')
GROUP BY 
    t.NAME
ORDER BY 
    TotalRows DESC;
"@

$tableStats = Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $analysisQuery

Write-Host "`n  Tabelas encontradas:" -ForegroundColor Gray
$tableStats | Format-Table -AutoSize

# =====================================================
# 4. EXTRAIR DADOS E GERAR SEEDER
# =====================================================

Write-Host "`n4. Extraindo dados e gerando seeder C#..." -ForegroundColor Yellow

# Função auxiliar para converter valor SQL em C#
function ConvertTo-CSharpValue {
    param($Value, $Type)
    
    if ($null -eq $Value -or $Value -is [DBNull]) {
        return "null"
    }
    
    switch ($Type) {
        "System.DateTime" {
            return "DateTime.Parse(`"$($Value.ToString('yyyy-MM-dd HH:mm:ss'))`")"
        }
        "System.Decimal" {
            return "${Value}m"
        }
        "System.Double" {
            return "${Value}d"
        }
        "System.Boolean" {
            return $Value.ToString().ToLower()
        }
        "System.String" {
            $escaped = $Value.Replace("`"", "\`"").Replace("`n", "\n").Replace("`r", "")
            return "`"$escaped`""
        }
        default {
            return $Value
        }
    }
}

# Função para gerar código C# de uma tabela
function Export-TableToCSharp {
    param(
        [string]$EntityName,
        [string]$Query
    )
    
    Write-Host "    Exportando $EntityName..." -ForegroundColor Gray
    
    $data = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database "PDPW_BACKUP" -Query $Query
    
    if (-not $data -or $data.Count -eq 0) {
        Write-Host "      ⚠️  Nenhum registro encontrado" -ForegroundColor Yellow
        return ""
    }
    
    $count = if ($data -is [Array]) { $data.Count } else { 1 }
    Write-Host "      ✅ $count registros" -ForegroundColor Green
    
    $code = @"

        // $EntityName ($count registros)
        modelBuilder.Entity<$EntityName>().HasData(

"@
    
    $dataArray = if ($data -is [Array]) { $data } else { @($data) }
    
    foreach ($row in $dataArray) {
        $code += "            new $EntityName { "
        
        foreach ($prop in $row.PSObject.Properties) {
            if ($prop.Name -ne "RowError" -and $prop.Name -ne "RowState" -and $prop.Name -ne "Table" -and $prop.Name -ne "ItemArray" -and $prop.Name -ne "HasErrors") {
                $csharpValue = ConvertTo-CSharpValue -Value $prop.Value -Type $prop.TypeNameOfValue
                $code += "$($prop.Name) = $csharpValue, "
            }
        }
        
        $code = $code.TrimEnd(", ")
        $code += " },`n"
    }
    
    $code = $code.TrimEnd(",`n")
    $code += @"

        );

"@
    
    return $code
}

# Iniciar geração do arquivo
$seederCode = @"
using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seeder ÚNICO com dados REAIS do cliente
/// Fonte: Backup_PDP_TST.bak
/// Gerado automaticamente em: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
/// Total: ~638 registros
/// </summary>
public static class PdpwRealDataSeeder
{
    /// <summary>
    /// Aplica todos os dados reais no ModelBuilder
    /// </summary>
    public static void SeedRealData(this ModelBuilder modelBuilder)
    {
"@

# Extrair cada tabela
$seederCode += Export-TableToCSharp -EntityName "TipoUsina" -Query @"
SELECT Id, Nome, Descricao, FonteEnergia, DataCriacao, Ativo 
FROM TiposUsina WHERE Ativo = 1 ORDER BY Id
"@

$seederCode += Export-TableToCSharp -EntityName "Empresa" -Query @"
SELECT TOP 30 Id, Nome, CNPJ, Telefone, Email, Endereco, DataCriacao, Ativo 
FROM Empresas WHERE Ativo = 1 ORDER BY Id
"@

$seederCode += Export-TableToCSharp -EntityName "Usina" -Query @"
SELECT TOP 50 Id, Codigo, Nome, TipoUsinaId, EmpresaId, CapacidadeInstalada, 
       Localizacao, DataOperacao, DataCriacao, Ativo 
FROM Usinas WHERE Ativo = 1 ORDER BY CapacidadeInstalada DESC
"@

$seederCode += Export-TableToCSharp -EntityName "SemanaPMO" -Query @"
SELECT Id, Numero, Ano, DataInicio, DataFim, Observacoes, DataCriacao, Ativo
FROM SemanasPMO 
WHERE Ativo = 1 
  AND DataInicio >= DATEADD(MONTH, -6, GETDATE())
  AND DataFim <= DATEADD(MONTH, 3, GETDATE())
ORDER BY DataInicio
"@

$seederCode += Export-TableToCSharp -EntityName "EquipePDP" -Query @"
SELECT Id, Nome, Descricao, Coordenador, Email, Telefone, DataCriacao, Ativo
FROM EquipesPDP WHERE Ativo = 1 ORDER BY Id
"@

$seederCode += Export-TableToCSharp -EntityName "MotivoRestricao" -Query @"
SELECT Id, Nome, Descricao, Categoria, DataCriacao, Ativo
FROM MotivosRestricao WHERE Ativo = 1 ORDER BY Id
"@

# Fechar classe
$seederCode += @"
    }
}
"@

# =====================================================
# 5. SALVAR ARQUIVO
# =====================================================

Write-Host "`n5. Salvando arquivo..." -ForegroundColor Yellow

$outputDir = Split-Path -Parent $OutputPath
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

$seederCode | Out-File -FilePath $OutputPath -Encoding UTF8 -Force

Write-Host "  ✅ Seeder gerado com sucesso!" -ForegroundColor Green
Write-Host "     Arquivo: $OutputPath" -ForegroundColor Gray
Write-Host "     Tamanho: $((Get-Item $OutputPath).Length) bytes" -ForegroundColor Gray

# =====================================================
# 6. RESUMO FINAL
# =====================================================

Write-Host "`n=========================================="  -ForegroundColor Cyan
Write-Host "GERAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "==========================================`n" -ForegroundColor Cyan

Write-Host "Próximos passos:" -ForegroundColor Yellow
Write-Host "  1. Revisar arquivo gerado: $OutputPath" -ForegroundColor White
Write-Host "  2. Atualizar DbSeeder.cs para chamar SeedRealData()" -ForegroundColor White
Write-Host "  3. Remover seeders antigos" -ForegroundColor White
Write-Host "  4. Criar nova migration" -ForegroundColor White
Write-Host "  5. Aplicar migration e testar`n" -ForegroundColor White

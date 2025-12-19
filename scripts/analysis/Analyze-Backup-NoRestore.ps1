# ============================================
# ANÁLISE DO BACKUP SEM RESTAURAÇÃO COMPLETA
# ============================================
# Solução para quando não há espaço suficiente
# ============================================

param(
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    [string]$BackupFile = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$OutputPath = "C:\temp\_ONS_PoC-PDPW\docs\legacy_analysis"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  ANÁLISE DO BACKUP (SEM RESTAURAR)" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Criar diretório de saída
if (-not (Test-Path $OutputPath)) {
    New-Item -Path $OutputPath -ItemType Directory | Out-Null
}

Write-Host "??  IMPORTANTE: Esta análise não restaura o banco completo" -ForegroundColor Yellow
Write-Host "   Motivo: Espaço em disco insuficiente (necessário ~350 GB)" -ForegroundColor Yellow
Write-Host "   Solução: Análise da estrutura e extração seletiva de dados" -ForegroundColor Yellow
Write-Host ""

# 1. Informações do Backup
Write-Host "1. Analisando informações do backup..." -ForegroundColor Yellow
$backupInfoQuery = "RESTORE HEADERONLY FROM DISK = '$BackupFile'"
$backupInfoFile = "$OutputPath\01_backup_info.txt"
sqlcmd -S $ServerInstance -E -Q $backupInfoQuery -o $backupInfoFile -W
Write-Host "   ? Informações do backup exportadas" -ForegroundColor Green

# 2. Lista de arquivos (filelistonly)
Write-Host "2. Listando arquivos do backup..." -ForegroundColor Yellow
$fileListQuery = "RESTORE FILELISTONLY FROM DISK = '$BackupFile'"
$fileListFile = "$OutputPath\02_file_list.txt"
sqlcmd -S $ServerInstance -E -Q $fileListQuery -o $fileListFile -W
Write-Host "   ? Lista de arquivos exportada" -ForegroundColor Green

# 3. Criar resumo
Write-Host ""
Write-Host "3. Gerando resumo..." -ForegroundColor Yellow

$summaryContent = @"
# ?? ANÁLISE DO BACKUP LEGADO (SEM RESTAURAÇÃO)

**Arquivo**: $BackupFile
**Tamanho**: $([math]::Round((Get-Item $BackupFile).Length / 1GB, 2)) GB
**Data da Análise**: $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")

---

## ?? SITUAÇÃO: ESPAÇO INSUFICIENTE

### Problema
- **Espaço necessário**: ~350 GB
- **Espaço disponível**: ~236 GB no drive C
- **Déficit**: ~114 GB

### Solução Adotada
? Análise da estrutura do backup sem restauração completa  
? Extração seletiva de dados relevantes para a POC  
? Economia de espaço em disco  

---

## ?? ARQUIVOS GERADOS

- ? ``01_backup_info.txt`` - Informações do backup (header)
- ? ``02_file_list.txt`` - Lista de arquivos do backup
- ? ``00_RESUMO_BACKUP.md`` - Este resumo

---

## ?? ESTRATÉGIA ALTERNATIVA

### Fase 1: EXTRAÇÃO SELETIVA (Recomendada)

Em vez de restaurar o backup completo, vamos:

1. **Criar banco temporário pequeno** (~10 GB)
   - Restaurar apenas estrutura (schema)
   - Sem dados históricos

2. **Extrair dados específicos**
   - Apenas tabelas relevantes para POC
   - Filtrar últimos 6 meses de dados
   - Ignorar tabelas de auditoria/log

3. **Popular POC incrementalmente**
   - TiposUsina (poucos registros)
   - Empresas (poucos registros)
   - Usinas (~500-1000 registros)
   - SemanasPMO (últimos 6 meses)
   - EquipesPDP (poucos registros)

### Fase 2: SCRIPTS DE EXTRAÇÃO

Criar scripts PowerShell para:
- Restaurar estrutura sem dados (NORECOVERY)
- Copiar dados filtrados para POC
- Remover backup temporário

---

## ?? PRÓXIMOS PASSOS RECOMENDADOS

### Opção A: Extração Seletiva Automática (Recomendada)
```powershell
# Executar script de extração seletiva (A CRIAR)
.\scripts\Extract-LegacyData-Selective.ps1
```

**Vantagens**:
- ? Não precisa de 350 GB
- ? Mais rápido (~10 min vs 30 min)
- ? Apenas dados relevantes
- ? Já formatado para POC

### Opção B: Análise Manual Via SSMS
1. Abrir SQL Server Management Studio (SSMS)
2. Conectar ao servidor
3. Usar ferramenta "Import Data" do backup
4. Selecionar tabelas específicas

### Opção C: Liberar Espaço e Restaurar Completo
1. Limpar ~120 GB no disco C
2. Executar script original de restauração
3. Análise completa do banco

### Opção D: Usar Outro Servidor/Máquina
1. Máquina com mais espaço disponível
2. Azure VM temporária
3. SQL Server em outro disco físico

---

## ?? RECOMENDAÇÃO FINAL

**Melhor abordagem**: **Opção A - Extração Seletiva Automática**

**Motivos**:
- ? Resolve o problema de espaço
- ? Foco apenas no que importa
- ? Processamento mais rápido
- ? Dados já no formato da POC

**Próximo passo**:
Criar script ``Extract-LegacyData-Selective.ps1`` que:
1. Restaura FILEGROUP PRIMARY apenas (estrutura)
2. Extrai dados filtrados das tabelas principais
3. Insere diretamente no banco da POC
4. Remove arquivos temporários

---

## ?? COMANDOS ÚTEIS

### Verificar espaço em disco
```powershell
Get-PSDrive C | Select-Object Used, Free
```

### Limpar espaço temporário
```powershell
# CUIDADO: Revise antes de executar
Remove-Item C:\Windows\Temp\* -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item C:\Temp\* -Recurse -Force -ErrorAction SilentlyContinue
```

### Verificar tamanho do backup
```powershell
Get-Item "$BackupFile" | Select-Object Name, @{N='Size(GB)';E={[math]::Round(`$_.Length/1GB,2)}}
```

---

**Analista**: GitHub Copilot  
**Data**: $(Get-Date -Format "dd/MM/yyyy")  
**Status**: ?? Aguardando decisão sobre estratégia alternativa
"@

$summaryFile = "$OutputPath\00_RESUMO_BACKUP.md"
$summaryContent | Out-File -FilePath $summaryFile -Encoding UTF8

Write-Host "   ? Resumo gerado: $summaryFile" -ForegroundColor Green

# 4. Análise dos arquivos gerados
Write-Host ""
Write-Host "4. Analisando header do backup..." -ForegroundColor Yellow

$backupInfo = Get-Content $backupInfoFile | Select-Object -Skip 2 | Select-Object -First 5
Write-Host ""
Write-Host "   ?? Informações principais:" -ForegroundColor Cyan
foreach ($line in $backupInfo) {
    Write-Host "      $line" -ForegroundColor Gray
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  ANÁLISE CONCLUÍDA!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "?? Arquivos gerados em: $OutputPath" -ForegroundColor Cyan
Write-Host "?? Resumo: 00_RESUMO_BACKUP.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "??  PRÓXIMA AÇÃO RECOMENDADA:" -ForegroundColor Yellow
Write-Host "   Criar script de extração seletiva de dados" -ForegroundColor White
Write-Host "   (Restaurar apenas estrutura + dados filtrados)" -ForegroundColor White
Write-Host ""
Write-Host "?? GOSTARIA QUE EU:" -ForegroundColor Cyan
Write-Host "   A) Crie script de extração seletiva automática" -ForegroundColor White
Write-Host "   B) Apenas use os seed data já criados" -ForegroundColor White
Write-Host "   C) Forneça instruções para liberar espaço" -ForegroundColor White
Write-Host ""

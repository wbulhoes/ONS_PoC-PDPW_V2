# ?? GUIA DE USO - EXTRAÇÃO SELETIVA DE DADOS

## ? EXECUÇÃO RÁPIDA

```powershell
cd C:\temp\_ONS_PoC-PDPW
.\scripts\Extract-LegacyData-Selective.ps1
```

---

## ?? O QUE O SCRIPT FAZ

### Fase 1: PREPARAÇÃO (1 min)
- ? Verifica backup do cliente
- ? Testa conexão SQL Server
- ? Valida banco da POC
- ? Remove bancos temporários anteriores

### Fase 2: RESTAURAÇÃO ESTRUTURA (5-10 min)
- ? Restaura apenas estrutura do banco legado
- ? Espaço necessário: ~20-30 GB (vs 350 GB completo)
- ? Mantém em banco temporário isolado

### Fase 3: ANÁLISE (30 seg)
- ? Identifica tabelas relevantes automaticamente
- ? Mapeia nomes Legado ? POC
- ? Exibe estatísticas de dados disponíveis

### Fase 4: EXTRAÇÃO E MIGRAÇÃO (2-5 min)
Migra dados filtrados:
- ? **TiposUsina**: Todos os tipos
- ? **Empresas**: Top 20 principais
- ? **Usinas**: Top 100 por capacidade
- ? **SemanasPMO**: Últimos 6 meses
- ? **EquipesPDP**: Todas as equipes

### Fase 5: LIMPEZA (30 seg)
- ? Remove banco temporário
- ? Libera espaço em disco
- ? Mantém apenas dados na POC

### Fase 6: RELATÓRIO
- ? Estatísticas de migração
- ? Tabelas e registros migrados
- ? Tempo total de execução

---

## ?? TEMPO TOTAL ESTIMADO

```
Preparação:        ~1 min
Restauração:       ~5-10 min
Análise:           ~30 seg
Migração:          ~2-5 min
Limpeza:           ~30 seg
?????????????????????????
TOTAL:             ~9-17 min
```

---

## ?? ESPAÇO EM DISCO

```
Backup original:        43.2 GB (compactado)
Banco temporário:       ~20-30 GB (estrutura + dados filtrados)
Dados na POC:           ~500 MB - 1 GB (apenas dados migrados)
Espaço livre mínimo:    ~30 GB recomendado
```

---

## ?? PARÂMETROS OPCIONAIS

### Personalizar quantidades:
```powershell
.\scripts\Extract-LegacyData-Selective.ps1 `
    -TopEmpresas 50 `
    -TopUsinas 200 `
    -MesesSemanasPMO 12 `
    -TopUsuarios 100
```

### Usar outro servidor:
```powershell
.\scripts\Extract-LegacyData-Selective.ps1 `
    -ServerInstance "localhost\SQLEXPRESS"
```

### Banco POC diferente:
```powershell
.\scripts\Extract-LegacyData-Selective.ps1 `
    -TargetDatabaseName "MeuBancoPOC"
```

---

## ? PRÉ-REQUISITOS

1. ? SQL Server Express rodando
2. ? Backup do cliente disponível (43.2 GB)
3. ? Espaço livre: ~30 GB no disco C
4. ? Banco da POC criado (migrations aplicadas)
5. ? PowerShell 5.1+ e sqlcmd instalado

---

## ?? VERIFICAÇÕES PRÉ-EXECUÇÃO

### 1. Verificar SQL Server:
```powershell
Get-Service MSSQL* | Select-Object Name, Status
```

### 2. Verificar espaço em disco:
```powershell
Get-PSDrive C | Select-Object @{N='Free(GB)';E={[math]::Round($_.Free/1GB,2)}}
```

### 3. Verificar banco da POC:
```powershell
sqlcmd -S localhost\SQLEXPRESS -E -Q "SELECT name FROM sys.databases WHERE name = 'PDPW_PoC'"
```

### 4. Verificar backup:
```powershell
Test-Path "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
```

---

## ?? PROBLEMAS COMUNS

### Problema 1: Espaço insuficiente
```
Erro: O espaço livre no volume do disco 'C:\' é insuficiente
```
**Solução**:
```powershell
# Liberar espaço temporário
Remove-Item C:\Windows\Temp\* -Recurse -Force -ErrorAction SilentlyContinue

# Verificar espaço novamente
Get-PSDrive C | Select-Object Used, Free
```

### Problema 2: Banco POC não encontrado
```
Erro: Banco da POC não encontrado: PDPW_PoC
```
**Solução**:
```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure
dotnet ef database update --startup-project ..\PDPW.API\PDPW.API.csproj
```

### Problema 3: Permissões SQL Server
```
Erro: Login failed for user
```
**Solução**:
- Execute PowerShell como Administrador
- Verifique Windows Authentication habilitado

### Problema 4: Timeout na restauração
```
Erro: Timeout expired
```
**Solução**: Script já configurado com timeout de 30 minutos (1800 seg)

---

## ?? RESULTADO ESPERADO

Após execução bem-sucedida:

```
============================================
  EXTRAÇÃO CONCLUÍDA COM SUCESSO!
============================================

?? ESTATÍSTICAS:
   Tabelas migradas: 5
   Registros migrados: 250-500
   Tempo total: 12m 34s

?? DADOS MIGRADOS:
   ? TiposUsina
   ? Empresas
   ? Usinas
   ? SemanasPMO
   ? EquipesPDP
```

---

## ?? APÓS A MIGRAÇÃO

### 1. Testar APIs:
```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run

# Acessar: http://localhost:5000/swagger
```

### 2. Verificar dados migrados:
```powershell
# Contar registros na POC
sqlcmd -S localhost\SQLEXPRESS -d PDPW_PoC -Q "
SELECT 'TiposUsina' AS Tabela, COUNT(*) AS Registros FROM TiposUsina
UNION ALL SELECT 'Empresas', COUNT(*) FROM Empresas
UNION ALL SELECT 'Usinas', COUNT(*) FROM Usinas
UNION ALL SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION ALL SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
"
```

### 3. Testar endpoints no Swagger:
- `GET /api/tiposusina` - Ver todos os tipos
- `GET /api/empresas` - Ver empresas migradas
- `GET /api/usinas` - Ver usinas migradas
- `GET /api/semanaspmo` - Ver semanas PMO
- `GET /api/equipespdp` - Ver equipes

---

## ?? EXECUTAR NOVAMENTE

Para re-executar a migração:

```powershell
# Limpar dados anteriores (CUIDADO!)
sqlcmd -S localhost\SQLEXPRESS -d PDPW_PoC -Q "
DELETE FROM Usinas WHERE Id > 10;
DELETE FROM Empresas WHERE Id > 8;
DELETE FROM TiposUsina WHERE Id > 5;
-- etc...
"

# Re-executar script
.\scripts\Extract-LegacyData-Selective.ps1
```

---

## ?? LOGS E DEBUG

### Ver mensagens detalhadas:
```powershell
.\scripts\Extract-LegacyData-Selective.ps1 -Verbose
```

### Salvar log em arquivo:
```powershell
.\scripts\Extract-LegacyData-Selective.ps1 | Tee-Object -FilePath "migration_log.txt"
```

---

## ?? DICAS

1. **Execute em horário de baixo uso** do servidor
2. **Backup do banco POC** antes da migração
3. **Monitore o espaço em disco** durante execução
4. **Não interrompa** durante a restauração
5. **Valide os dados** após migração

---

## ?? SUPORTE

Em caso de problemas:
1. Consulte os logs de erro exibidos
2. Verifique pré-requisitos acima
3. Revise `docs/SITUACAO_BACKUP_CLIENTE.md`
4. Tente executar novamente

---

**Criado em**: 19/12/2024  
**Versão**: 1.0.0  
**Status**: ? Pronto para uso

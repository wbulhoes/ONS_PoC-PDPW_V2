# ?? GUIA DE USO - EXTRA��O SELETIVA DE DADOS

## ? EXECU��O R�PIDA

```powershell
cd C:\temp\_ONS_PoC-PDPW
.\scripts\Extract-LegacyData-Selective.ps1
```

---

## ?? O QUE O SCRIPT FAZ

### Fase 1: PREPARA��O (1 min)
- ? Verifica backup do cliente
- ? Testa conex�o SQL Server
- ? Valida banco da POC
- ? Remove bancos tempor�rios anteriores

### Fase 2: RESTAURA��O ESTRUTURA (5-10 min)
- ? Restaura apenas estrutura do banco legado
- ? Espa�o necess�rio: ~20-30 GB (vs 350 GB completo)
- ? Mant�m em banco tempor�rio isolado

### Fase 3: AN�LISE (30 seg)
- ? Identifica tabelas relevantes automaticamente
- ? Mapeia nomes Legado ? POC
- ? Exibe estat�sticas de dados dispon�veis

### Fase 4: EXTRA��O E MIGRA��O (2-5 min)
Migra dados filtrados:
- ? **TiposUsina**: Todos os tipos
- ? **Empresas**: Top 20 principais
- ? **Usinas**: Top 100 por capacidade
- ? **SemanasPMO**: �ltimos 6 meses
- ? **EquipesPDP**: Todas as equipes

### Fase 5: LIMPEZA (30 seg)
- ? Remove banco tempor�rio
- ? Libera espa�o em disco
- ? Mant�m apenas dados na POC

### Fase 6: RELAT�RIO
- ? Estat�sticas de migra��o
- ? Tabelas e registros migrados
- ? Tempo total de execu��o

---

## ?? TEMPO TOTAL ESTIMADO

```
Prepara��o:        ~1 min
Restaura��o:       ~5-10 min
An�lise:           ~30 seg
Migra��o:          ~2-5 min
Limpeza:           ~30 seg
?????????????????????????
TOTAL:             ~9-17 min
```

---

## ?? ESPA�O EM DISCO

```
Backup original:        43.2 GB (compactado)
Banco tempor�rio:       ~20-30 GB (estrutura + dados filtrados)
Dados na POC:           ~500 MB - 1 GB (apenas dados migrados)
Espa�o livre m�nimo:    ~30 GB recomendado
```

---

## ?? PAR�METROS OPCIONAIS

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

## ? PR�-REQUISITOS

1. ? SQL Server Express rodando
2. ? Backup do cliente dispon�vel (43.2 GB)
3. ? Espa�o livre: ~30 GB no disco C
4. ? Banco da POC criado (migrations aplicadas)
5. ? PowerShell 5.1+ e sqlcmd instalado

---

## ?? VERIFICA��ES PR�-EXECU��O

### 1. Verificar SQL Server:
```powershell
Get-Service MSSQL* | Select-Object Name, Status
```

### 2. Verificar espa�o em disco:
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

### Problema 1: Espa�o insuficiente
```
Erro: O espa�o livre no volume do disco 'C:\' � insuficiente
```
**Solu��o**:
```powershell
# Liberar espa�o tempor�rio
Remove-Item C:\Windows\Temp\* -Recurse -Force -ErrorAction SilentlyContinue

# Verificar espa�o novamente
Get-PSDrive C | Select-Object Used, Free
```

### Problema 2: Banco POC n�o encontrado
```
Erro: Banco da POC n�o encontrado: PDPW_PoC
```
**Solu��o**:
```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure
dotnet ef database update --startup-project ..\PDPW.API\PDPW.API.csproj
```

### Problema 3: Permiss�es SQL Server
```
Erro: Login failed for user
```
**Solu��o**:
- Execute PowerShell como Administrador
- Verifique Windows Authentication habilitado

### Problema 4: Timeout na restaura��o
```
Erro: Timeout expired
```
**Solu��o**: Script j� configurado com timeout de 30 minutos (1800 seg)

---

## ?? RESULTADO ESPERADO

Ap�s execu��o bem-sucedida:

```
============================================
  EXTRA��O CONCLU�DA COM SUCESSO!
============================================

?? ESTAT�STICAS:
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

## ?? AP�S A MIGRA��O

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

Para re-executar a migra��o:

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

1. **Execute em hor�rio de baixo uso** do servidor
2. **Backup do banco POC** antes da migra��o
3. **Monitore o espa�o em disco** durante execu��o
4. **N�o interrompa** durante a restaura��o
5. **Valide os dados** ap�s migra��o

---

## ?? SUPORTE

Em caso de problemas:
1. Consulte os logs de erro exibidos
2. Verifique pr�-requisitos acima
3. Revise `docs/SITUACAO_BACKUP_CLIENTE.md`
4. Tente executar novamente

---

**Criado em**: 19/12/2024  
**Vers�o**: 1.0.0  
**Status**: ? Pronto para uso

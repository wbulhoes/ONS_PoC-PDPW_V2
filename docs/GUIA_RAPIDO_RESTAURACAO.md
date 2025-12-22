# ?? GUIA R�PIDO - RESTAURA��O DO BACKUP LEGADO

## ? EXECU��O R�PIDA (2 comandos)

### 1?? Restaurar Backup (10-30 minutos)
```powershell
cd C:\temp\_ONS_PoC-PDPW
.\scripts\Restore-LegacyBackup.ps1
```

### 2?? Analisar Estrutura (2-5 minutos)
```powershell
.\scripts\Analyze-LegacyDatabase.ps1
```

---

## ?? RESULTADO ESPERADO

Ap�s a execu��o, voc� ter�:

1. ? **Banco `PDPW_Legacy` restaurado** no SQL Server Express
2. ? **Pasta `docs/legacy_analysis/`** com an�lise completa:
   - Lista de todas as tabelas
   - Contagem de registros
   - Estrutura das principais tabelas
   - Amostras de dados
   - Relacionamentos (FKs)
   - �ndices e constraints
   - Resumo em Markdown

---

## ?? PR�XIMAS A��ES

### Imediato (Ap�s An�lise)
1. Revisar arquivo `docs/legacy_analysis/00_RESUMO_ANALISE.md`
2. Identificar tabelas correspondentes � POC
3. Planejar migra��o de dados

### Scripts Adicionais (A criar conforme necess�rio)
- `Migrate-TiposUsina.ps1` - Migrar tipos de usina
- `Migrate-Empresas.ps1` - Migrar empresas
- `Migrate-Usinas.ps1` - Migrar usinas
- `Migrate-SemanasPMO.ps1` - Migrar semanas PMO
- `Migrate-EquipesPDP.ps1` - Migrar equipes

---

## ?? IMPORTANTE

### Tempo Estimado
- **Restaura��o**: 10-30 minutos (depende do hardware)
- **An�lise**: 2-5 minutos
- **Total**: ~15-35 minutos

### Requisitos
- ? SQL Server Express rodando
- ? ~50 GB de espa�o livre em disco
- ? Permiss�es de administrador
- ? PowerShell 5.1+

### Em Caso de Erro
- Verifique se SQL Server est� rodando: `Get-Service MSSQL*`
- Verifique espa�o em disco: `Get-PSDrive C`
- Execute PowerShell como Administrador
- Consulte logs em `docs/legacy_analysis/`

---

## ?? PAR�METROS OPCIONAIS

### Restaurar em servidor diferente:
```powershell
.\scripts\Restore-LegacyBackup.ps1 -ServerInstance "seu_servidor\instancia"
```

### Usar nome de banco diferente:
```powershell
.\scripts\Restore-LegacyBackup.ps1 -NewDatabaseName "OutroNome"
```

### Salvar an�lise em outro local:
```powershell
.\scripts\Analyze-LegacyDatabase.ps1 -OutputPath "C:\MeuCaminho"
```

---

## ?? SUPORTE

Em caso de d�vidas:
1. Consulte `docs/PLANO_RESTAURACAO_BACKUP.md`
2. Revise logs de erro no PowerShell
3. Verifique o SQL Server Error Log

---

**Preparado por**: GitHub Copilot  
**Data**: 19/12/2024  
**Vers�o**: 1.0.0

# ?? GUIA RÁPIDO - RESTAURAÇÃO DO BACKUP LEGADO

## ? EXECUÇÃO RÁPIDA (2 comandos)

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

Após a execução, você terá:

1. ? **Banco `PDPW_Legacy` restaurado** no SQL Server Express
2. ? **Pasta `docs/legacy_analysis/`** com análise completa:
   - Lista de todas as tabelas
   - Contagem de registros
   - Estrutura das principais tabelas
   - Amostras de dados
   - Relacionamentos (FKs)
   - Índices e constraints
   - Resumo em Markdown

---

## ?? PRÓXIMAS AÇÕES

### Imediato (Após Análise)
1. Revisar arquivo `docs/legacy_analysis/00_RESUMO_ANALISE.md`
2. Identificar tabelas correspondentes à POC
3. Planejar migração de dados

### Scripts Adicionais (A criar conforme necessário)
- `Migrate-TiposUsina.ps1` - Migrar tipos de usina
- `Migrate-Empresas.ps1` - Migrar empresas
- `Migrate-Usinas.ps1` - Migrar usinas
- `Migrate-SemanasPMO.ps1` - Migrar semanas PMO
- `Migrate-EquipesPDP.ps1` - Migrar equipes

---

## ?? IMPORTANTE

### Tempo Estimado
- **Restauração**: 10-30 minutos (depende do hardware)
- **Análise**: 2-5 minutos
- **Total**: ~15-35 minutos

### Requisitos
- ? SQL Server Express rodando
- ? ~50 GB de espaço livre em disco
- ? Permissões de administrador
- ? PowerShell 5.1+

### Em Caso de Erro
- Verifique se SQL Server está rodando: `Get-Service MSSQL*`
- Verifique espaço em disco: `Get-PSDrive C`
- Execute PowerShell como Administrador
- Consulte logs em `docs/legacy_analysis/`

---

## ?? PARÂMETROS OPCIONAIS

### Restaurar em servidor diferente:
```powershell
.\scripts\Restore-LegacyBackup.ps1 -ServerInstance "seu_servidor\instancia"
```

### Usar nome de banco diferente:
```powershell
.\scripts\Restore-LegacyBackup.ps1 -NewDatabaseName "OutroNome"
```

### Salvar análise em outro local:
```powershell
.\scripts\Analyze-LegacyDatabase.ps1 -OutputPath "C:\MeuCaminho"
```

---

## ?? SUPORTE

Em caso de dúvidas:
1. Consulte `docs/PLANO_RESTAURACAO_BACKUP.md`
2. Revise logs de erro no PowerShell
3. Verifique o SQL Server Error Log

---

**Preparado por**: GitHub Copilot  
**Data**: 19/12/2024  
**Versão**: 1.0.0

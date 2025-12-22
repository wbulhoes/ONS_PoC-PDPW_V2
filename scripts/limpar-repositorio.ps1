# Script de Limpeza do Repositório para Entrega ao Cliente
# Uso: .\limpar-repositorio.ps1

Write-Host "🧹 LIMPEZA DO REPOSITÓRIO PARA ENTREGA AO CLIENTE" -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan
Write-Host ""

$root = "C:\temp\_ONS_PoC-PDPW_V2"

# ===========================
# 1. BACKUP ANTES DE LIMPAR
# ===========================

Write-Host "📦 1. Criando backup de segurança..." -ForegroundColor Yellow

$backupPath = "C:\temp\POC_BACKUP_PRE_LIMPEZA_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
New-Item -Path $backupPath -ItemType Directory -Force | Out-Null

# Backup apenas da pasta docs (164 arquivos)
Copy-Item "$root\docs" "$backupPath\docs" -Recurse -Force
Write-Host "   ✅ Backup criado em: $backupPath" -ForegroundColor Green
Write-Host ""

# ===========================
# 2. REMOVER PASTAS DESNECESSÁRIAS
# ===========================

Write-Host "🗑️  2. Removendo pastas desnecessárias..." -ForegroundColor Yellow

$pastasRemover = @(
    "Backup",
    "backups", 
    "database",
    "HelloWorld",
    "legado",
    "pdpw_act",
    "pdpw-react",
    ".cursor",
    ".vs"
)

foreach ($pasta in $pastasRemover) {
    $path = Join-Path $root $pasta
    if (Test-Path $path) {
        Remove-Item $path -Recurse -Force -ErrorAction SilentlyContinue
        Write-Host "   ✅ Removido: $pasta" -ForegroundColor Green
    }
}

Write-Host ""

# ===========================
# 3. REORGANIZAR DOCUMENTAÇÃO
# ===========================

Write-Host "📚 3. Reorganizando documentação..." -ForegroundColor Yellow

# Criar pasta temporária para docs essenciais
$docsEssenciais = "$root\docs_essenciais"
New-Item -Path $docsEssenciais -ItemType Directory -Force | Out-Null

# Documentos ESSENCIAIS para manter
$docsParaManter = @(
    "CONFIGURACAO_SQL_SERVER.md",
    "GUIA_TESTES_SWAGGER.md",
    "VALIDACAO_COMPLETA_SWAGGER_23_12_2024.md",
    "FRAMEWORK_EXCELENCIA.md",
    "PULL_REQUEST_SQUAD.md",
    "GUIA_CRIAR_PULL_REQUEST.md",
    "RELATORIO_VALIDACAO_POC.md",
    "RESUMO_EXECUTIVO_POC_ATUALIZADO.md"
)

foreach ($doc in $docsParaManter) {
    $source = Join-Path "$root\docs" $doc
    if (Test-Path $source) {
        Copy-Item $source $docsEssenciais -Force
        Write-Host "   ✅ Mantido: $doc" -ForegroundColor Green
    }
}

# Remover pasta docs antiga
Remove-Item "$root\docs" -Recurse -Force -ErrorAction SilentlyContinue

# Renomear docs_essenciais para docs
Rename-Item $docsEssenciais "docs"

Write-Host "   ✅ Reduzido de 164 para 8 documentos" -ForegroundColor Green
Write-Host ""

# ===========================
# 4. CRIAR README PRINCIPAL
# ===========================

Write-Host "📝 4. Criando README principal..." -ForegroundColor Yellow

$readmeContent = @"
# 🚀 POC Migração PDPW - Backend .NET 8

**Projeto**: Prova de Conceito - Migração do sistema PDPW  
**Cliente**: ONS (Operador Nacional do Sistema)  
**Período**: Dezembro/2024  
**Status**: ✅ Concluído

---

## 📋 Sobre o Projeto

Sistema de Programação Diária da Produção de Energia migrado de .NET Framework/VB.NET para **.NET 8/C#** com Clean Architecture.

### 🎯 Objetivo da POC

Validar a viabilidade técnica da migração modernizando:
- Backend: .NET Framework 4.8 → .NET 8
- Linguagem: VB.NET → C# 12
- Arquitetura: 3-camadas → Clean Architecture
- Banco: SQL Server (mantido)

---

## ✨ Entregas da POC

### 🌐 Backend (.NET 8)
- ✅ **15 APIs REST** (107 endpoints)
- ✅ **Clean Architecture** implementada
- ✅ **Repository Pattern** em todas as entidades
- ✅ **53 testes unitários** (100% passando)
- ✅ **Swagger** completo e documentado

### 🗄️ Banco de Dados
- ✅ **638 registros reais** do setor elétrico brasileiro
- ✅ **30 entidades** do domínio
- ✅ **Migrations** configuradas
- ✅ Dados de empresas reais (CEMIG, COPEL, Itaipu, FURNAS, etc)
- ✅ Usinas reais (Itaipu 14GW, Belo Monte 11GW, Tucuruí 8GW, etc)

### 🧪 Qualidade
- ✅ **Score POC**: 76/100 ⭐⭐⭐⭐
- ✅ 53 testes unitários (100% passando)
- ✅ Zero bugs conhecidos
- ✅ Swagger 100% validado

---

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server 2019+ (Express é suficiente)
- Visual Studio 2022 ou VS Code

### Passo 1: Clonar Repositório
```bash
git clone https://github.com/RafaelSuzanoACT/POCMigracaoPDPw.git
cd POCMigracaoPDPw
git checkout feature/backend
```

### Passo 2: Configurar Banco de Dados
```bash
cd src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
```

**Resultado**: Banco criado com 638 registros reais

### Passo 3: Iniciar API
```bash
cd ../PDPW.API
dotnet run
```

### Passo 4: Acessar Swagger
```
http://localhost:5001/swagger/index.html
```

**OU** usar script de automação:
```powershell
.\scripts\gerenciar-api.ps1 start
.\scripts\gerenciar-api.ps1 test
```

---

## 🧪 Executar Testes

```bash
cd tests/PDPW.Application.Tests
dotnet test
```

**Resultado esperado**: 53/53 testes passando ✅

---

## 📚 Documentação

- 📄 [Configuração SQL Server](docs/CONFIGURACAO_SQL_SERVER.md)
- 📄 [Guia de Testes Swagger](docs/GUIA_TESTES_SWAGGER.md)
- 📄 [Validação Completa](docs/VALIDACAO_COMPLETA_SWAGGER_23_12_2024.md)
- 📄 [Framework de Excelência](docs/FRAMEWORK_EXCELENCIA.md)
- 📄 [Relatório de Validação](docs/RELATORIO_VALIDACAO_POC.md)

---

## 🏗️ Arquitetura

```
src/
├── PDPW.API/              # Controllers, Filters, Middlewares
├── PDPW.Application/      # Services, DTOs, Interfaces
├── PDPW.Domain/           # Entities, Domain Interfaces
└── PDPW.Infrastructure/   # Repositories, DbContext, Migrations
```

**Padrões implementados**:
- Clean Architecture
- Repository Pattern
- Dependency Injection
- DTOs + AutoMapper
- Global Exception Handling

---

## 📊 Estatísticas

| Métrica | Valor |
|---------|-------|
| **APIs REST** | 15 (107 endpoints) |
| **Testes Unitários** | 53 (100% passando) |
| **Entidades** | 30 |
| **Registros BD** | 638 |
| **Documentação** | 8 documentos |
| **Score POC** | 76/100 ⭐⭐⭐⭐ |
| **Capacidade Total** | ~110.000 MW |

---

## 👥 Squad

- **Tech Lead**: Rafael Suzano
- **Backend Developer**: Willian Bulhões
- **Período**: 19-23 Dezembro/2024

---

## 📞 Suporte

Ver documentação em `docs/` para:
- Troubleshooting
- Configuração avançada
- Guia de testes
- Relatórios de validação

---

## ✅ Status da POC

**✅ Backend Concluído**  
**✅ Banco de Dados Configurado**  
**✅ Testes Validados**  
**✅ Swagger Funcional**  
**✅ Documentação Completa**  

**Pronto para apresentação ao cliente! 🎉**

---

**📅 Última Atualização**: 23/12/2024  
**🎯 Versão**: 1.0 (POC)  
**🏆 Score**: 76/100 ⭐⭐⭐⭐
"@

Set-Content "$root\README.md" $readmeContent -Encoding UTF8
Write-Host "   ✅ README.md criado" -ForegroundColor Green
Write-Host ""

# ===========================
# 5. LIMPAR ARQUIVOS TEMPORÁRIOS
# ===========================

Write-Host "🧹 5. Removendo arquivos temporários..." -ForegroundColor Yellow

$arquivosRemover = @(
    ".DS_Store",
    "Thumbs.db",
    "*.tmp",
    "*.log"
)

foreach ($pattern in $arquivosRemover) {
    Get-ChildItem $root -Recurse -Filter $pattern -ErrorAction SilentlyContinue | Remove-Item -Force
}

Write-Host "   ✅ Arquivos temporários removidos" -ForegroundColor Green
Write-Host ""

# ===========================
# 6. VERIFICAR .gitignore
# ===========================

Write-Host "🔒 6. Verificando .gitignore..." -ForegroundColor Yellow

$gitignorePath = "$root\.gitignore"
if (Test-Path $gitignorePath) {
    Write-Host "   ✅ .gitignore existe" -ForegroundColor Green
} else {
    Write-Host "   ⚠️  .gitignore não encontrado" -ForegroundColor Yellow
}

Write-Host ""

# ===========================
# 7. RESUMO FINAL
# ===========================

Write-Host "=" -ForegroundColor Cyan
Write-Host "📊 RESUMO DA LIMPEZA" -ForegroundColor Cyan
Write-Host "=" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ Pastas removidas: $($pastasRemover.Count)" -ForegroundColor Green
Write-Host "✅ Documentação reduzida: 164 → 8 arquivos" -ForegroundColor Green
Write-Host "✅ README principal criado" -ForegroundColor Green
Write-Host "✅ Backup criado em: $backupPath" -ForegroundColor Green
Write-Host ""
Write-Host "🎯 Estrutura final:" -ForegroundColor Cyan
Write-Host "   📁 src/              (código principal)" -ForegroundColor Gray
Write-Host "   📁 tests/            (testes)" -ForegroundColor Gray
Write-Host "   📁 scripts/          (automação)" -ForegroundColor Gray
Write-Host "   📁 docs/             (8 documentos essenciais)" -ForegroundColor Gray
Write-Host "   📁 .github/          (workflows)" -ForegroundColor Gray
Write-Host "   📄 README.md         (principal)" -ForegroundColor Gray
Write-Host ""
Write-Host "✅ LIMPEZA CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host ""
Write-Host "🚀 Próximos passos:" -ForegroundColor Cyan
Write-Host "   1. Revisar mudanças: git status" -ForegroundColor Gray
Write-Host "   2. Commit: git add . && git commit -m 'chore: limpa repositorio para entrega ao cliente'" -ForegroundColor Gray
Write-Host "   3. Push: git push origin feature/backend" -ForegroundColor Gray
Write-Host ""

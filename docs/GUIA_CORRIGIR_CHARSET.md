# 🔧 GUIA: CORRIGIR PROBLEMAS DE CHARSET/ENCODING

**Problema**: Caracteres inv�lidos (�, �§, �£o, etc.) no c�digo  
**Causa**: Inconsistência de encoding (UTF-8 vs ISO-8859-1 vs Windows-1252)  
**Solu��o**: Padronizar tudo em UTF-8

---

## 🎯 SOLU��o R�PIDA (5 MINUTOS)

### **1. Verificar Encoding Atual**

#### **PowerShell - Verificar arquivos problem�ticos**
```powershell
# Navegar para o reposit�rio
cd C:\temp\_ONS_PoC-PDPW_V2

# Verificar encoding de arquivos .md
Get-ChildItem -Recurse -Filter *.md | ForEach-Object {
    $encoding = [System.IO.File]::ReadAllText($_.FullName, [System.Text.Encoding]::Default)
    if ($encoding -match '[�]') {
        Write-Host "❌ Encoding problem�tico: $($_.FullName)" -ForegroundColor Red
    }
}

# Verificar encoding de arquivos .cs
Get-ChildItem -Recurse -Filter *.cs | Select-Object -First 10 | ForEach-Object {
    Write-Host "Arquivo: $($_.Name)"
    $content = Get-Content $_.FullName -Raw -Encoding Default
    if ($content -match '[�]') {
        Write-Host "  ❌ Cont�m caracteres inv�lidos" -ForegroundColor Red
    } else {
        Write-Host "  ✅ Encoding OK" -ForegroundColor Green
    }
}
```

---

## 🔧 SOLU��o PERMANENTE

### **PASSO 1: Configurar Visual Studio Code**

#### **1.1. Settings do VS Code (Ctrl+,)**

Adicione/verifique estas configura�ões:

```json
{
  // Encoding padr�o
  "files.encoding": "utf8",
  
  // Auto-detectar encoding
  "files.autoGuessEncoding": true,
  
  // End of Line (Windows)
  "files.eol": "\r\n",
  
  // Inserir final newline
  "files.insertFinalNewline": true,
  
  // Trimmar espa�os no final
  "files.trimTrailingWhitespace": true,
  
  // Mostrar encoding na barra de status
  "workbench.statusBar.visible": true
}
```

#### **1.2. Salvar configura��o no workspace**

Crie `.vscode/settings.json` no reposit�rio:

```json
{
  "files.encoding": "utf8",
  "files.autoGuessEncoding": false,
  "files.eol": "\r\n",
  "[markdown]": {
    "files.encoding": "utf8"
  },
  "[csharp]": {
    "files.encoding": "utf8"
  }
}
```

---

### **PASSO 2: Configurar Git**

#### **2.1. Configurar Git para UTF-8**

```powershell
# Configurar encoding UTF-8
git config --global core.quotepath false
git config --global gui.encoding utf-8
git config --global i18n.commit.encoding utf-8
git config --global i18n.logoutputencoding utf-8

# Verificar configura�ões
git config --global --list | Select-String "encoding"
```

#### **2.2. Configurar .gitattributes**

Crie/atualize `.gitattributes` na raiz do reposit�rio:

```
# Auto detect text files and perform LF normalization
* text=auto

# Arquivos de c�digo sempre UTF-8
*.cs     text eol=crlf encoding=utf-8
*.csproj text eol=crlf encoding=utf-8
*.sln    text eol=crlf encoding=utf-8
*.json   text eol=crlf encoding=utf-8
*.xml    text eol=crlf encoding=utf-8

# Documenta��o sempre UTF-8
*.md     text eol=crlf encoding=utf-8
*.txt    text eol=crlf encoding=utf-8

# Scripts PowerShell
*.ps1    text eol=crlf encoding=utf-8

# Scripts Bash
*.sh     text eol=lf encoding=utf-8

# Arquivos bin�rios
*.dll    binary
*.exe    binary
*.png    binary
*.jpg    binary
*.gif    binary
*.pdf    binary
```

---

### **PASSO 3: Converter Arquivos Existentes para UTF-8**

#### **3.1. Script PowerShell - Converter Arquivos Markdown**

Crie `scripts/fix-encoding.ps1`:

```powershell
# Script para converter arquivos para UTF-8
param(
    [string]$Path = ".",
    [string]$Extension = "*.md"
)

Write-Host "🔧 Convertendo arquivos $Extension para UTF-8..." -ForegroundColor Cyan

$files = Get-ChildItem -Path $Path -Filter $Extension -Recurse -File

$count = 0
foreach ($file in $files) {
    try {
        # Ler conte�do com encoding atual
        $content = Get-Content -Path $file.FullName -Raw -Encoding Default
        
        # Verificar se tem caracteres problem�ticos
        if ($content -match '[�]' -or $content -match '�§' -or $content -match '�£o') {
            Write-Host "  ⚠️  Corrigindo: $($file.Name)" -ForegroundColor Yellow
            
            # Salvar como UTF-8 com BOM
            [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
            $count++
        } else {
            # Ainda assim, garantir UTF-8
            [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
        }
    }
    catch {
        Write-Host "  ❌ Erro em $($file.Name): $_" -ForegroundColor Red
    }
}

Write-Host "`n✅ Convers�o conclu�da! $count arquivos corrigidos." -ForegroundColor Green
```

#### **3.2. Executar convers�o**

```powershell
# Navegar para o reposit�rio
cd C:\temp\_ONS_PoC-PDPW_V2

# Converter arquivos Markdown
.\scripts\fix-encoding.ps1 -Path "docs" -Extension "*.md"

# Converter arquivos README
.\scripts\fix-encoding.ps1 -Path "." -Extension "README*.md"

# Converter arquivos C# (se necess�rio)
.\scripts\fix-encoding.ps1 -Path "src" -Extension "*.cs"
```

---

### **PASSO 4: Configurar Visual Studio 2022**

#### **4.1. Configura�ões Globais**

1. Abrir Visual Studio
2. **Tools** → **Options**
3. Navegar: **Environment** → **Documents**
4. Configurar:
   - ✅ **Save documents as Unicode (UTF-8 with signature) - Codepage 65001**
   - ✅ **Auto-detect UTF-8 encoding without signature**

#### **4.2. Configurar .editorconfig**

Crie `.editorconfig` na raiz:

```ini
# EditorConfig is awesome: https://EditorConfig.org

root = true

# Padrões gerais
[*]
charset = utf-8
end_of_line = crlf
insert_final_newline = true
trim_trailing_whitespace = true

# Arquivos C#
[*.cs]
charset = utf-8-bom
indent_style = space
indent_size = 4

# Arquivos de projeto
[*.{csproj,vbproj,vcxproj,vcxproj.filters,proj,projitems,shproj}]
charset = utf-8-bom
indent_size = 2

# JSON
[*.json]
charset = utf-8
indent_size = 2

# Markdown
[*.md]
charset = utf-8
trim_trailing_whitespace = false

# PowerShell
[*.ps1]
charset = utf-8-bom
```

---

### **PASSO 5: Limpar e Reconverter Git**

#### **5.1. Renormalizar reposit�rio**

```powershell
# Fazer backup primeiro!
git add --all
git commit -m "chore: backup antes de renormalizar encoding"

# Remover cache do git
git rm --cached -r .

# Re-adicionar com novo encoding
git reset --hard

# Normalizar line endings
git add --renormalize .

# Verificar diferen�as
git status
git diff --cached --stat

# Commit das mudan�as
git commit -m "chore: normalizar encoding para UTF-8

- Configura .gitattributes para UTF-8
- Adiciona .editorconfig
- Converte arquivos para UTF-8 com BOM
- Corrige caracteres inv�lidos"
```

---

## 🔍 DIAGN�STICO DETALHADO

### **Script de Diagn�stico Completo**

Crie `scripts/check-encoding.ps1`:

```powershell
# Script de diagn�stico de encoding
param(
    [string]$Path = "."
)

Write-Host "🔍 Diagn�stico de Encoding" -ForegroundColor Cyan
Write-Host "==============================`n" -ForegroundColor Cyan

# Fun��o para detectar encoding
function Get-FileEncoding {
    param([string]$FilePath)
    
    $bytes = [System.IO.File]::ReadAllBytes($FilePath)
    
    # UTF-8 BOM
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
        return "UTF-8 BOM"
    }
    
    # UTF-16 LE BOM
    if ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFF -and $bytes[1] -eq 0xFE) {
        return "UTF-16 LE"
    }
    
    # UTF-16 BE BOM
    if ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFE -and $bytes[1] -eq 0xFF) {
        return "UTF-16 BE"
    }
    
    # ASCII/UTF-8 sem BOM
    $allAscii = $true
    foreach ($byte in $bytes) {
        if ($byte -gt 127) {
            $allAscii = $false
            break
        }
    }
    
    if ($allAscii) {
        return "ASCII"
    } else {
        return "UTF-8 sem BOM (ou outro)"
    }
}

# Analisar arquivos
$extensions = @("*.cs", "*.md", "*.json", "*.xml", "*.txt")
$results = @{}

foreach ($ext in $extensions) {
    Write-Host "Analisando $ext..." -ForegroundColor Yellow
    $files = Get-ChildItem -Path $Path -Filter $ext -Recurse -File | Select-Object -First 20
    
    foreach ($file in $files) {
        $encoding = Get-FileEncoding -FilePath $file.FullName
        
        if (-not $results.ContainsKey($encoding)) {
            $results[$encoding] = @()
        }
        
        $results[$encoding] += $file.FullName
    }
}

# Mostrar resultados
Write-Host "`n📊 Resumo:" -ForegroundColor Cyan
foreach ($encoding in $results.Keys) {
    $count = $results[$encoding].Count
    Write-Host "`n$encoding ($count arquivos):" -ForegroundColor Green
    $results[$encoding] | Select-Object -First 5 | ForEach-Object {
        Write-Host "  - $_" -ForegroundColor Gray
    }
    if ($count -gt 5) {
        Write-Host "  ... e mais $($count - 5) arquivos" -ForegroundColor Gray
    }
}

# Procurar caracteres problem�ticos
Write-Host "`n🔍 Procurando caracteres inv�lidos..." -ForegroundColor Cyan
$problematicFiles = @()

Get-ChildItem -Path $Path -Filter "*.md" -Recurse -File | ForEach-Object {
    $content = Get-Content $_.FullName -Raw -Encoding Default
    if ($content -match '[�]|�§|�£o|�¡|�©|�­|�³|�º') {
        $problematicFiles += $_.FullName
    }
}

if ($problematicFiles.Count -gt 0) {
    Write-Host "`n❌ Arquivos com caracteres problem�ticos:" -ForegroundColor Red
    $problematicFiles | ForEach-Object {
        Write-Host "  - $_" -ForegroundColor Yellow
    }
} else {
    Write-Host "`n✅ Nenhum arquivo com caracteres problem�ticos encontrado!" -ForegroundColor Green
}
```

**Executar:**
```powershell
.\scripts\check-encoding.ps1 -Path "."
```

---

## 🎯 PREVEN��o FUTURA

### **1. Configurar novo commit hook**

Crie `.git/hooks/pre-commit`:

```bash
#!/bin/sh
# Pre-commit hook para verificar encoding

echo "Verificando encoding dos arquivos..."

# Verificar arquivos staged
git diff --cached --name-only --diff-filter=ACM | while read file; do
    if [ -f "$file" ]; then
        # Verificar se � arquivo de texto
        if file "$file" | grep -q "text"; then
            # Verificar encoding
            encoding=$(file -bi "$file" | sed -n 's/.*charset=\(.*\)/\1/p')
            
            if [ "$encoding" != "utf-8" ] && [ "$encoding" != "us-ascii" ]; then
                echo "❌ ERRO: $file n�o est� em UTF-8 (encoding: $encoding)"
                echo "   Execute: scripts/fix-encoding.ps1"
                exit 1
            fi
        fi
    fi
done

echo "✅ Encoding verificado!"
exit 0
```

Tornar execut�vel (no Git Bash):
```bash
chmod +x .git/hooks/pre-commit
```

---

### **2. Adicionar CI/CD check**

Crie `.github/workflows/encoding-check.yml`:

```yaml
name: Encoding Check

on:
  pull_request:
    branches: [develop, main]

jobs:
  check-encoding:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Check UTF-8 encoding
        run: |
          echo "Verificando encoding UTF-8..."
          
          # Verificar arquivos
          find . -type f \( -name "*.cs" -o -name "*.md" -o -name "*.json" \) | while read file; do
            if ! file -bi "$file" | grep -q "charset=utf-8\|charset=us-ascii"; then
              echo "❌ ERRO: $file n�o est� em UTF-8"
              exit 1
            fi
          done
          
          echo "✅ Todos os arquivos est�o em UTF-8"
```

---

## 📋 CHECKLIST DE CORRE��o

```
┌─────────────────────────────────────────────────┐
│  CHECKLIST: CORRIGIR ENCODING                   │
├─────────────────────────────────────────────────┤
│  □ 1. Configurar VS Code (settings.json)       │
│  □ 2. Configurar Git (core.quotepath, etc.)    │
│  □ 3. Criar .gitattributes                     │
│  □ 4. Criar .editorconfig                      │
│  □ 5. Executar diagn�stico (check-encoding)    │
│  □ 6. Converter arquivos (fix-encoding)        │
│  □ 7. Configurar Visual Studio 2022            │
│  □ 8. Renormalizar Git (git add --renormalize) │
│  □ 9. Commit das mudan�as                      │
│  □ 10. Configurar pre-commit hook              │
│  □ 11. Adicionar CI/CD check (opcional)        │
│  □ 12. Verificar no GitHub/GitLab              │
└─────────────────────────────────────────────────┘
```

---

## 🚨 PROBLEMAS COMUNS E SOLU�ÕES

### **Problema 1: "Ainda vejo caracteres estranhos depois de converter"**

**Solu��o**:
```powershell
# For�a convers�o com detec��o de encoding original
$file = "caminho/do/arquivo.md"
$content = Get-Content $file -Raw -Encoding Default
$content = $content -replace '�§', '�'
$content = $content -replace '�£o', '�o'
$content = $content -replace '�¡', '�'
[System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8)
```

### **Problema 2: "Git mostra diferen�as mesmo sem mudan�as"**

**Solu��o**:
```powershell
# Normalizar line endings
git config core.autocrlf true
git add --renormalize .
git commit -m "chore: normalizar line endings"
```

### **Problema 3: "Erro ao fazer push: invalid UTF-8"**

**Solu��o**:
```powershell
# Verificar e corrigir antes de commit
git diff --check
git add --renormalize .
```

---

## 📧 RESPOSTA PARA O GEORGE

**Copie e cole:**

```
George,

Sobre o charset: � problema de encoding UTF-8 vs ISO-8859-1/Windows-1252.

SOLU��o R�PIDA:
1. Configurar VS Code para UTF-8:
   - Ctrl+, → buscar "files.encoding"
   - Mudar para "utf8"

2. Configurar Git:
   git config --global core.quotepath false
   git config --global gui.encoding utf-8

3. Converter arquivos problem�ticos:
   Criei script em: scripts/fix-encoding.ps1
   Execute: .\scripts\fix-encoding.ps1 -Path "docs"

DOCUMENTA��o COMPLETA:
docs/GUIA_CORRIGIR_CHARSET.md

Resumo do problema:
- Arquivo criado em Windows-1252
- Git esperando UTF-8
- Resultado: caracteres � e �§

Quer que eu rode o script para você e fa�a commit da corre��o?

Willian
```

---

**✅ Guia completo criado! Agora � s� seguir os passos! 🚀**

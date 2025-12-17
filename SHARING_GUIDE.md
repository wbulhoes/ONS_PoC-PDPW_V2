# ?? Guia de Compartilhamento do Projeto PDPW

## ?? Como Compartilhar com Seu Colega

Este guia explica como compartilhar o projeto **PDPW** com outros desenvolvedores para que consigam executá-lo em suas máquinas.

---

## ?? Opções de Compartilhamento

### ? Opção 1: Git/GitHub (RECOMENDADO)

**Vantagens:**
- ? Controle de versão
- ? Histórico de mudanças
- ? Colaboração em equipe
- ? Backup automático
- ? Fácil atualização

#### ?? Passo a Passo

**1. Criar repositório Git (se ainda não tiver)**

```powershell
# Na pasta raiz do projeto
cd C:\temp\_ONS_PoC-PDPW

# Inicializar Git
git init

# Adicionar todos os arquivos
git add .

# Fazer o primeiro commit
git commit -m "Initial commit - PDPW Project"
```

**2. Criar .gitignore (importante!)**

Já vou criar o arquivo para você ignorar arquivos desnecessários:

**3. Enviar para GitHub**

```powershell
# Criar repositório no GitHub (via website)
# https://github.com/new

# Conectar ao repositório remoto
git remote add origin https://github.com/seu-usuario/PDPW.git

# Enviar código
git branch -M main
git push -u origin main
```

**4. Seu colega clona o repositório**

```powershell
# Seu colega executa:
git clone https://github.com/seu-usuario/PDPW.git
cd PDPW
```

---

### ?? Opção 2: Arquivo ZIP

**Quando usar:**
- ?? Compartilhamento único/rápido
- ?? Sem necessidade de versionamento

#### ?? Passo a Passo

**1. Limpar arquivos temporários**

```powershell
# Remover pastas bin e obj (não devem ser compartilhadas)
Get-ChildItem -Path . -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
```

**2. Criar arquivo ZIP**

```powershell
# Comprimir projeto
Compress-Archive -Path "C:\temp\_ONS_PoC-PDPW\*" -DestinationPath "C:\temp\PDPW-Project.zip"
```

**3. Enviar para seu colega**
- Email (se < 25MB)
- Google Drive / OneDrive
- Compartilhamento de rede

**4. Seu colega descompacta**

```powershell
# Descompactar
Expand-Archive -Path "PDPW-Project.zip" -DestinationPath "C:\Projetos\PDPW"
```

---

### ?? Opção 3: Azure DevOps / GitLab

Similar ao GitHub, mas com mais recursos corporativos.

---

## ?? Checklist para Seu Colega

Após receber o projeto, seu colega deve seguir estes passos:

### 1?? **Pré-requisitos**

```powershell
# Verificar .NET 8 instalado
dotnet --version
# Deve mostrar: 8.0.x

# Se não tiver, baixar de:
# https://dotnet.microsoft.com/download/dotnet/8.0
```

### 2?? **Restaurar Dependências**

```powershell
# Na pasta raiz do projeto
cd PDPW

# Restaurar pacotes NuGet
dotnet restore
```

### 3?? **Compilar Projeto**

```powershell
# Build de todos os projetos
dotnet build
```

### 4?? **Configurar Banco de Dados**

**Opção A: InMemory (Mais Rápido)**

Editar `src\PDPW.API\appsettings.Development.json`:
```json
{
  "UseInMemoryDatabase": true
}
```

**Opção B: SQL Server LocalDB**

```powershell
# Instalar EF Core Tools
dotnet tool install --global dotnet-ef

# Criar banco de dados
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

### 5?? **Executar Aplicação**

**API Principal:**
```powershell
dotnet run --project src\PDPW.API\PDPW.API.csproj
```

**Hello World Dashboard:**
```powershell
dotnet run --project HelloWorld\HelloWorld.csproj
```

### 6?? **Testar**

- **API:** https://localhost:65417/swagger
- **Hello World:** http://localhost:5555

---

## ?? Arquivos para Compartilhar

### ? **Incluir:**

```
? src/ (código fonte)
? HelloWorld/ (projeto de teste)
? *.md (documentação)
? *.csproj (arquivos de projeto)
? *.sln (solution)
? appsettings.json
? .gitignore
```

### ? **NÃO Incluir:**

```
? bin/ (binários compilados)
? obj/ (arquivos temporários)
? .vs/ (configurações do Visual Studio)
? *.user (configurações pessoais)
? Migrations/ (se usando banco local)
```

---

## ?? Template de Email para Seu Colega

```
Assunto: Projeto PDPW - Setup

Olá [Nome],

Segue o projeto PDPW para você configurar em sua máquina.

?? Repositório: [link do GitHub]
OU
?? Arquivo ZIP: [link do arquivo]

?? Documentação de Setup:
1. README.md - Visão geral
2. DATABASE_SETUP.md - Configuração de banco
3. QUICK_START_INMEMORY.md - Início rápido (sem SQL)
4. TROUBLESHOOTING.md - Solução de problemas

? Setup Rápido (5 minutos):
1. Clonar/Extrair projeto
2. Abrir em Visual Studio 2022
3. Configurar: "UseInMemoryDatabase": true
4. Executar (F5)
5. Acessar: https://localhost:65417/swagger

?? Pré-requisitos:
- .NET 8 SDK
- Visual Studio 2022 (ou VS Code)
- Git (se clonar do GitHub)

? Problemas?
Veja: TROUBLESHOOTING.md ou me avise!

Abraços,
[Seu Nome]
```

---

## ?? Configuração Recomendada no GitHub

### Estrutura do Repositório

```
seu-usuario/PDPW/
??? .gitignore          ? Criado automaticamente
??? README.md           ? Documentação principal
??? src/
?   ??? PDPW.API/
?   ??? PDPW.Application/
?   ??? PDPW.Domain/
?   ??? PDPW.Infrastructure/
??? HelloWorld/
??? DATABASE_SETUP.md
??? TROUBLESHOOTING.md
??? [outros .md]
```

### README.md do Repositório

Já criarei um README.md adequado para o repositório.

---

## ?? Automação (Avançado)

### Script de Setup Automático

Crie `setup.ps1` para seu colega:

```powershell
# setup.ps1 - Script de setup automático
Write-Host "?? Configurando projeto PDPW..." -ForegroundColor Cyan

# 1. Verificar .NET
Write-Host "`n? Verificando .NET..." -ForegroundColor Yellow
dotnet --version

# 2. Restaurar dependências
Write-Host "`n? Restaurando pacotes..." -ForegroundColor Yellow
dotnet restore

# 3. Build
Write-Host "`n? Compilando projeto..." -ForegroundColor Yellow
dotnet build

# 4. Configurar InMemory
Write-Host "`n? Configurando InMemory Database..." -ForegroundColor Yellow
$appsettings = Get-Content "src\PDPW.API\appsettings.Development.json" | ConvertFrom-Json
$appsettings.UseInMemoryDatabase = $true
$appsettings | ConvertTo-Json -Depth 10 | Set-Content "src\PDPW.API\appsettings.Development.json"

Write-Host "`n? Setup concluído!" -ForegroundColor Green
Write-Host "Execute: dotnet run --project src\PDPW.API" -ForegroundColor Cyan
```

---

## ?? Compartilhamento via Azure DevOps

Se sua empresa usa Azure DevOps:

### 1. Criar Repositório
```
https://dev.azure.com/sua-empresa
? Repos ? New Repository
```

### 2. Push do Código
```powershell
git remote add origin https://sua-empresa@dev.azure.com/sua-empresa/PDPW/_git/PDPW
git push -u origin main
```

### 3. Seu Colega Clona
```powershell
git clone https://sua-empresa@dev.azure.com/sua-empresa/PDPW/_git/PDPW
```

---

## ? Checklist Final

Antes de compartilhar, certifique-se:

- [ ] Código compila sem erros
- [ ] Documentação está atualizada
- [ ] .gitignore está configurado
- [ ] Senhas/secrets removidos
- [ ] appsettings.json está genérico
- [ ] README.md está claro
- [ ] Testou o setup do zero

---

## ?? Suporte para Seu Colega

Se seu colega tiver problemas:

### Problema: ".NET não encontrado"
**Solução:** Instalar .NET 8 SDK
```
https://dotnet.microsoft.com/download/dotnet/8.0
```

### Problema: "Erro de compilação"
**Solução:** Restaurar pacotes
```powershell
dotnet restore
dotnet clean
dotnet build
```

### Problema: "Erro de banco de dados"
**Solução:** Usar InMemory
```json
"UseInMemoryDatabase": true
```

### Problema: "Porta em uso"
**Solução:** Mudar porta em `launchSettings.json`

---

## ?? Contato

Deixe claro para seu colega como te contatar:

- ?? Email
- ?? Teams/Slack
- ?? Telefone
- ?? Issues no GitHub

---

## ?? Recursos de Aprendizado

Para ajudar seu colega:

- [.NET 8 Docs](https://learn.microsoft.com/dotnet/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [ASP.NET Core](https://learn.microsoft.com/aspnet/core/)

---

## ? Melhorias Futuras

- [ ] Configurar CI/CD
- [ ] Docker containerization
- [ ] Documentação da API (Swagger)
- [ ] Testes unitários
- [ ] Code coverage

---

**?? Projeto pronto para compartilhar!**

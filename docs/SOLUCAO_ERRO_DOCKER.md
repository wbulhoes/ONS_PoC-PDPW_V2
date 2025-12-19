# ?? SOLUÇÃO: Configuração para Windows Containers

**Erro inicial:** Docker estava tentando usar imagem inexistente  
**Solicitação cliente:** Usar Windows containers (não Linux)  
**Status:** ? **CORRIGIDO E CONFIGURADO PARA WINDOWS**

---

## ? CORREÇÕES APLICADAS

### 1. Dockerfile.backend ? Windows Nano Server

**Antes:** Linux containers  
**Depois:** Windows Nano Server LTSC 2022

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-ltsc2022 AS final
```

### 2. Dockerfile.frontend ? Windows IIS

**Antes:** Linux + Nginx  
**Depois:** Windows Server Core + IIS

```dockerfile
FROM mcr.microsoft.com/windows/servercore:ltsc2022 AS build
FROM mcr.microsoft.com/windows/servercore/iis:windowsservercore-ltsc2022 AS final
```

### 3. docker-compose.yml ? Windows Network

**Alterações:**
- ? `isolation: process` (Windows process isolation)
- ? `driver: nat` (Windows network driver)
- ? Paths de volume Windows

---

## ?? PRÉ-REQUISITOS CRÍTICOS

### 1. Sistema Operacional

**? Compatível:**
- Windows 10 Pro (version 1809+)
- Windows 11 Pro
- Windows Server 2019+

**? NÃO compatível:**
- Windows 10/11 Home
- Linux
- macOS

### 2. Docker Desktop em Modo Windows

**IMPORTANTE:** Docker DEVE estar em **Windows containers mode**

**Como verificar:**
```powershell
docker version
```

**Deve mostrar:**
```
Server:
 OS/Arch: windows/amd64  ? Deve ser WINDOWS
```

**Se mostrar `linux/amd64`, trocar:**
1. Clicar botão direito no Docker Desktop (system tray)
2. Selecionar "Switch to Windows containers..."
3. Aguardar reinicialização (~30 seg)
4. Verificar novamente

### 3. Hyper-V Habilitado

```powershell
# Verificar (PowerShell Admin)
Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V

# Se não estiver habilitado:
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
Enable-WindowsOptionalFeature -Online -FeatureName Containers -All

# Reiniciar após habilitar
Restart-Computer
```

---

## ?? PASSOS PARA EXECUTAR AGORA

### PASSO 1: Trocar para Windows Containers (2 min)

```powershell
# Verificar modo atual
docker version

# Se aparecer "linux/amd64":
# 1. Botão direito no Docker Desktop (system tray)
# 2. "Switch to Windows containers..."
# 3. Aguardar reinicialização

# Verificar novamente
docker version
# Deve mostrar: OS/Arch: windows/amd64
```

### PASSO 2: Limpar Ambiente (2 min)

```powershell
cd C:\temp\_ONS_PoC-PDPW

# Parar tudo
docker-compose down -v

# Limpar cache (recomendado)
docker system prune -a --volumes -f
```

### PASSO 3: Build (15-30 min primeira vez) ??

```powershell
# Build SEM cache
docker-compose build --no-cache

# Ou com logs detalhados:
docker-compose build --no-cache --progress=plain
```

**?? TEMPO ESPERADO:**
- SQL Server: ~2 min
- Backend (.NET): ~5-10 min
- Frontend (Node + IIS): ~10-20 min
- **TOTAL: 15-35 minutos**

? **Aproveite para um café!**

### PASSO 4: Iniciar (3-5 min)

```powershell
# Iniciar em foreground (ver logs)
docker-compose up

# Aguardar até ver:
# ? pdpw-sqlserver started
# ? pdpw-backend started
# ? pdpw-frontend started
```

### PASSO 5: Validar

**Abrir navegador:**
- Backend: http://localhost:5000/swagger
- Frontend: http://localhost:3000

**Se funcionar:** ? **SUCESSO!**

---

## ?? TROUBLESHOOTING

### ? Erro: "This version of Windows doesn't support..."

**Causa:** Está tentando rodar Linux containers no modo Windows ou vice-versa

**Solução:**
```powershell
# Trocar para Windows containers
# Docker Desktop ? Botão direito ? "Switch to Windows containers..."
```

---

### ? Erro: "Hyper-V is not enabled"

**Causa:** Hyper-V não está habilitado

**Solução:**
```powershell
# PowerShell Admin
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
Enable-WindowsOptionalFeature -Online -FeatureName Containers -All

# Reiniciar
Restart-Computer
```

---

### ? Erro: "insufficient memory"

**Causa:** Windows containers precisam de MAIS memória que Linux

**Solução:**
1. Docker Desktop ? Settings ? Resources
2. Memory: Aumentar para **8 GB** ou mais
3. Apply & Restart

**Requisitos:**
- Mínimo: 8 GB RAM
- Recomendado: 16 GB RAM

---

### ? Erro: "SQL Server keeps restarting"

**Causa:** SQL Server demora 2-3 minutos para iniciar

**Solução:**
```powershell
# Ver logs
docker-compose logs sqlserver

# Aguardar até ver:
# "SQL Server is now ready for client connections"

# Se continuar falhando, verificar memória (precisa 2 GB+)
```

---

### ? Erro: "Build failed on frontend"

**Causa:** Download do Node.js pode falhar em Windows containers

**Solução 1: Usar imagem com Node pré-instalado**
```dockerfile
# Trocar primeira linha do Dockerfile.frontend:
FROM stefanscherer/node-windows:20-nanoserver-ltsc2022 AS build
```

**Solução 2: Simplificar frontend (desenvolvimento local)**
```powershell
# Usar frontend local para desenvolvimento
cd frontend
npm install
npm run dev

# Docker só para backend + SQL
```
---

### ? Erro: "Port already in use"

**Causa:** Porta 5000, 3000 ou 1433 em uso

**Solução:**
```powershell
# Ver processo
netstat -ano | findstr :5000

# Matar processo
taskkill /PID <numero> /F

# Ou trocar porta no docker-compose.yml:
ports:
  - "5001:80"  # Usar 5001 em vez de 5000
```
---

### ? Erro: "Network timeout" durante build

**Causa:** Firewall ou proxy bloqueando download

**Solução:**
```powershell
# Criar/editar: C:\ProgramData\Docker\config\daemon.json

{
  "max-concurrent-downloads": 3,
  "max-concurrent-uploads": 3,
  "dns": ["8.8.8.8", "8.8.4.4"]
}

# Reiniciar Docker Desktop
```
---

## ?? RECOMENDAÇÃO PRÁTICA

### Para a PoC (6 dias):

**? OPÇÃO A: Desenvolvimento Local + Docker Final (RECOMENDADO)**

```
Dias 1-5: Desenvolvimento local
?? Backend: dotnet run (mais rápido)
?? Frontend: npm run dev (mais rápido)
?? SQL: InMemory ou local

Dia 6: Validar Docker Windows
?? Garantir que funciona para entrega
```

**Vantagens:**
- ? Desenvolvimento MUITO mais rápido
- ?? Menos problemas de ambiente
- ?? Foco na entrega das 29 APIs
- ?? Economiza 15-30 min a cada rebuild

**? OPÇÃO B: Docker desde início**

```
Todos os dias: docker-compose up
```

**Vantagens:**
- Ambiente consistente
- Problemas detectados cedo

**Desvantagens:**
- ?? Build demora 15-30 min
- ?? Troubleshooting mais complexo
- ?? Precisa 16 GB RAM

---

## ?? COMPARAÇÃO: Linux vs Windows Containers

| Aspecto | Linux Containers | Windows Containers |
|---------|-----------------|-------------------|
| **Compatibilidade** | Win/Mac/Linux | **Só Windows Pro+** |
| **Tamanho imagens** | ~100-200 MB | **~500 MB - 5 GB** |
| **Tempo build** | 5-10 min | **15-30 min** |
| **Tempo startup** | 10-30 seg | **2-3 min** |
| **RAM necessária** | 4 GB | **8-16 GB** |
| **Performance** | Melhor | Boa |
| **Mercado** | Padrão | Nicho MS |

**Por que o cliente quer Windows containers?**
- Ambiente MS puro (Windows Server)
- Políticas corporativas
- Integração com infraestrutura existente
- Suporte Microsoft

---

## ? VALIDAÇÃO FINAL

### Checklist para confirmar que está funcionando:

#### 1. Docker Desktop
- [ ] Docker Desktop rodando
- [ ] Modo: **Windows containers** (`docker version` mostra `windows/amd64`)
- [ ] Memória alocada: 8 GB+

#### 2. Build
- [ ] `docker-compose build` completa sem erros
- [ ] `docker images` lista 3 imagens
- [ ] Imagens tem "windows" no nome/plataforma

#### 3. Containers
- [ ] `docker-compose up` inicia 3 containers
- [ ] `docker ps` mostra 3 containers (Status: Up)
- [ ] Nenhum container reiniciando constantemente

#### 4. Aplicação
- [ ] http://localhost:5000/swagger abre e mostra endpoints
- [ ] http://localhost:3000 abre frontend React
- [ ] Swagger consegue executar GET /api/dadosenergeticos
- [ ] Frontend chama API sem erro CORS

#### 5. Logs
- [ ] `docker-compose logs` sem erros críticos
- [ ] SQL Server: "ready for client connections"
- [ ] Backend: "Now listening on: http://[::]:80"
- [ ] Frontend: IIS servindo arquivos

---

## ?? PRÓXIMOS PASSOS

### Se Docker Funcionar ?

```powershell
# 1. Commit das mudanças
git add .
git commit -m "[DOCKER] Configura Windows containers

- Atualiza Dockerfile.backend (nanoserver)
- Atualiza Dockerfile.frontend (IIS)
- Configura docker-compose para Windows
- Adiciona documentação Windows containers"
git push origin develop

# 2. Criar branches de desenvolvimento
git checkout -b feature/gestao-ativos
git push -u origin feature/gestao-ativos

# 3. Começar desenvolvimento das APIs
cd src\PDPW.API
# ...
```

### Se Docker Der Problemas ??

```powershell
# 1. Documentar erro específico
docker-compose logs > docker-error.log

# 2. Usar desenvolvimento local
cd src\PDPW.API
dotnet run

cd frontend
npm run dev

# 3. Validar Docker no final (Dia 6)
```
---

## ?? DOCUMENTAÇÃO COMPLETA

Ver guia detalhado: **[`docs/GUIA_WINDOWS_CONTAINERS.md`](GUIA_WINDOWS_CONTAINERS.md)**

**Inclui:**
- ? Pré-requisitos detalhados
- ? Troubleshooting completo
- ? Comandos úteis
- ? Alternativas e workarounds
- ? Boas práticas Windows containers

---

## ?? RESUMO

**O QUE FOI FEITO:**
1. ? Dockerfile.backend ? Windows Nano Server
2. ? Dockerfile.frontend ? Windows IIS
3. ? docker-compose.yml ? Configuração Windows
4. ? Documentação completa criada

**O QUE VOCÊ PRECISA FAZER:**
1. ?? Trocar Docker para modo Windows
2. ?? Executar `docker-compose build`
3. ?? Executar `docker-compose up`
4. ? Validar funcionamento

**TEMPO TOTAL:** ~30-40 minutos na primeira vez

---

**Documento atualizado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 2.0 (Windows Containers)  
**Status:** ? CONFIGURADO PARA WINDOWS

**Execute os passos e me avise o resultado! ????**

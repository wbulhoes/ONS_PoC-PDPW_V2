# ?? GUIA COMPLETO - Windows Containers

**Data:** 19/12/2024  
**Solicitação:** Cliente requer Windows containers  
**Status:** ? Configuração atualizada

---

## ?? PRÉ-REQUISITOS IMPORTANTES

### 1. Windows 10/11 Pro/Enterprise ou Windows Server

Windows containers **NÃO funcionam** em:
- ? Windows 10/11 Home
- ? Linux
- ? macOS

**Versão mínima:**
- Windows 10 Pro version 1809+
- Windows 11 Pro
- Windows Server 2019+

### 2. Docker Desktop Configurado para Windows Containers

**Verificar modo atual:**
```powershell
docker version
```

**Deve aparecer:**
```
Server:
 OS/Arch: windows/amd64
```

**Se aparecer `linux/amd64`, precisa trocar:**

#### Como Trocar para Windows Containers:

**Método 1: Via Interface Gráfica**
1. Abrir Docker Desktop
2. Clicar com botão direito no ícone do Docker (system tray)
3. Clicar em "Switch to Windows containers..."
4. Aguardar reinicialização (~30 segundos)
5. Verificar novamente: `docker version`

**Método 2: Via PowerShell (Admin)**
```powershell
& $Env:ProgramFiles\Docker\Docker\DockerCli.exe -SwitchDaemon
```

### 3. Hyper-V e Containers Features Habilitados

```powershell
# Verificar se está habilitado
Get-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V
Get-WindowsOptionalFeature -Online -FeatureName Containers

# Se não estiver habilitado, executar (requer Admin):
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
Enable-WindowsOptionalFeature -Online -FeatureName Containers -All

# Reiniciar computador após habilitar
Restart-Computer
```

---

## ?? PASSOS PARA EXECUTAR

### PASSO 1: Trocar para Windows Containers (5 min)

```powershell
# Verificar modo atual
docker version

# Se for Linux, trocar:
# - Clicar botão direito no Docker Desktop
# - "Switch to Windows containers..."
# - Aguardar

# Verificar novamente
docker version
# Deve mostrar: OS/Arch: windows/amd64
```

### PASSO 2: Limpar Ambiente (2 min)

```powershell
# Navegar para o projeto
cd C:\temp\_ONS_PoC-PDPW

# Parar containers existentes
docker-compose down -v

# Limpar imagens antigas (opcional)
docker system prune -a --volumes -f
```

### PASSO 3: Build das Imagens (15-30 min primeira vez)

```powershell
# Build SEM cache (recomendado)
docker-compose build --no-cache

# Ou com logs detalhados:
docker-compose build --no-cache --progress=plain
```

**?? TEMPO ESPERADO:**
- SQL Server: ~2 min (download de imagem)
- Backend: ~5-10 min (build .NET)
- Frontend: ~10-15 min (Node.js + IIS)

**TOTAL: 15-30 minutos na primeira vez**

### PASSO 4: Iniciar Serviços (5 min)

```powershell
# Iniciar em foreground (ver logs)
docker-compose up

# Ou em background:
# docker-compose up -d

# Aguardar até ver:
# ? pdpw-sqlserver started
# ? pdpw-backend started
# ? pdpw-frontend started
```

### PASSO 5: Validar (2 min)

**Abrir navegador:**
- Backend: http://localhost:5000/swagger
- Frontend: http://localhost:3000

**Verificar containers:**
```powershell
docker ps

# Deve listar 3 containers rodando
```

---

## ?? ARQUIVOS ATUALIZADOS

### 1. Dockerfile.backend (Nano Server)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022 AS build
# ... build stages ...
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-ltsc2022 AS final
```

**Características:**
- ? Windows Nano Server LTSC 2022
- ? .NET 8.0
- ? Imagem leve (~200 MB)
- ? Multi-stage build (otimizado)

### 2. Dockerfile.frontend (IIS)

```dockerfile
FROM mcr.microsoft.com/windows/servercore:ltsc2022 AS build
# ... Node.js install + build ...
FROM mcr.microsoft.com/windows/servercore/iis:windowsservercore-ltsc2022 AS final
```

**Características:**
- ? Windows Server Core + IIS
- ? Node.js 20.x para build
- ? IIS como servidor web
- ? Configuração para SPA (React)

### 3. docker-compose.yml

```yaml
services:
  sqlserver:
    isolation: process
  backend:
    isolation: process
  frontend:
    isolation: process

networks:
  pdpw-network:
    driver: nat  # Windows network driver
```

**Alterações:**
- ? `isolation: process` (melhor performance Windows)
- ? `driver: nat` (driver de rede Windows)
- ? Paths Windows nos volumes

---

## ?? DIFERENÇAS: Linux vs Windows Containers

| Aspecto | Linux Containers | Windows Containers |
|---------|-----------------|-------------------|
| **OS Host** | Qualquer (Win/Mac/Linux) | Só Windows Pro+ |
| **Tamanho Imagens** | ~100-200 MB | ~500 MB - 5 GB |
| **Tempo de Build** | 5-10 min | 15-30 min |
| **Performance** | Melhor | Boa (process isolation) |
| **Compatibilidade** | Universal | Só Windows |
| **Produção** | Mais comum | Comum em ambientes MS |

---

## ?? TROUBLESHOOTING

### Erro: "This version of Windows doesn't support Linux containers"

```powershell
# SOLUÇÃO: Trocar para Windows containers
# Docker Desktop ? Botão direito ? "Switch to Windows containers..."
```

### Erro: "Hyper-V is not enabled"

```powershell
# Executar como Admin:
Enable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V -All
Enable-WindowsOptionalFeature -Online -FeatureName Containers -All

# Reiniciar
Restart-Computer
```

### Erro: "Access denied" ou "Permission denied"

```powershell
# Executar PowerShell como Administrador
# Tentar novamente
```

### Erro: "Insufficient memory"

Windows containers precisam de **MAIS memória** que Linux:

**Mínimo:** 8 GB RAM  
**Recomendado:** 16 GB RAM

**Configurar no Docker Desktop:**
1. Settings ? Resources
2. Memory: Aumentar para 8 GB+
3. Apply & Restart

### Erro: "SQL Server container keeps restarting"

```powershell
# Ver logs
docker-compose logs sqlserver

# SQL Server demora 2-3 minutos na primeira vez
# Aguardar até ver: "SQL Server is now ready for client connections"

# Se continuar falhando, aumentar timeout:
# Editar docker-compose.yml:
services:
  sqlserver:
    healthcheck:
      test: ["CMD", "sqlcmd", "-Q", "select 1"]
      interval: 10s
      timeout: 3s
      retries: 30
```

### Erro: "Build failed on frontend (Node.js)"

O download do Node.js pode falhar. Alternativas:

**Opção 1: Usar imagem pré-construída**
```dockerfile
# Trocar de servercore para imagem com Node
FROM stefanscherer/node-windows:20-nanoserver-ltsc2022
```

**Opção 2: Usar PowerShell Gallery**
```dockerfile
RUN powershell -Command `
    Install-PackageProvider -Name NuGet -Force; `
    Install-Module -Name NodeJs -Force
```

### Erro: "Network timeout" durante build

```powershell
# Aumentar timeout do Docker
# Criar/editar: C:\ProgramData\Docker\config\daemon.json

{
  "max-concurrent-downloads": 3,
  "max-concurrent-uploads": 3
}

# Reiniciar Docker Desktop
```

### Erro: "Port 5000 already in use"

```powershell
# Verificar processos
netstat -ano | findstr :5000

# Matar processo
taskkill /PID <numero> /F

# Ou trocar porta no docker-compose.yml:
ports:
  - "5001:80"
```

---

## ?? VALIDAÇÃO DE FUNCIONAMENTO

### Checklist Completo

#### 1. Docker Desktop
- [ ] Docker Desktop rodando
- [ ] Modo: Windows containers
- [ ] `docker version` mostra `windows/amd64`
- [ ] Memória: 8 GB+ alocados

#### 2. Build
- [ ] `docker-compose build` completa sem erros
- [ ] 3 imagens criadas (sqlserver, backend, frontend)
- [ ] `docker images` lista as 3 imagens

#### 3. Containers
- [ ] `docker-compose up` inicia 3 containers
- [ ] `docker ps` mostra 3 containers rodando (Up)
- [ ] Nenhum container em estado "Restarting"

#### 4. Aplicação
- [ ] http://localhost:5000/swagger abre Swagger UI
- [ ] http://localhost:3000 abre frontend React
- [ ] Swagger consegue executar endpoints (GET /api/dadosenergeticos)
- [ ] Frontend consegue chamar API

#### 5. Logs
- [ ] `docker-compose logs backend` sem erros críticos
- [ ] `docker-compose logs frontend` sem erros críticos
- [ ] `docker-compose logs sqlserver` mostra "ready for connections"

---

## ?? COMANDOS ÚTEIS

### Gestão de Containers

```powershell
# Ver containers rodando
docker ps

# Ver todos (incluindo parados)
docker ps -a

# Ver logs de um container
docker logs pdpw-backend
docker logs pdpw-frontend
docker logs pdpw-sqlserver

# Seguir logs em tempo real
docker logs -f pdpw-backend

# Parar containers
docker-compose down

# Parar e remover volumes
docker-compose down -v

# Reiniciar um container específico
docker restart pdpw-backend
```

### Build e Limpeza

```powershell
# Build de um serviço específico
docker-compose build backend
docker-compose build frontend

# Rebuild sem cache
docker-compose build --no-cache backend

# Ver imagens
docker images

# Remover imagem
docker rmi <image-id>

# Limpar tudo (CUIDADO!)
docker system prune -a --volumes -f
```

### Debug

```powershell
# Entrar em um container (Windows CMD)
docker exec -it pdpw-backend cmd

# Entrar com PowerShell
docker exec -it pdpw-backend powershell

# Ver uso de recursos
docker stats

# Inspecionar container
docker inspect pdpw-backend

# Ver logs de build
docker-compose build --progress=plain --no-cache
```

---

## ?? RECOMENDAÇÕES

### Para Desenvolvimento (6 dias de PoC)

**? OPÇÃO 1: Desenvolvimento Local + Docker Final (RECOMENDADO)**

```
Dias 1-5: Desenvolvimento local
?? Backend: dotnet run
?? Frontend: npm run dev
?? SQL Server: Local ou InMemory

Dia 6: Validar Docker
?? Garantir que funciona em Windows containers
```

**Vantagens:**
- Desenvolvimento mais rápido
- Menos problemas de ambiente
- Docker só para validação final

**? OPÇÃO 2: Docker desde início**

```
Todos os dias: Docker
?? docker-compose up
?? Testar no container
```

**Vantagens:**
- Ambiente consistente desde início
- Problemas de container detectados cedo

**Desvantagens:**
- Build demora 15-30 min
- Troubleshooting mais complexo

### Para Produção

Se o cliente exige Windows containers, considere:

1. **Azure App Service (Windows)**
   - Gerenciado pela Microsoft
   - Não precisa gerenciar containers
   - Suporta .NET 8 nativamente

2. **Azure Container Instances (Windows)**
   - Containers Windows gerenciados
   - Mais simples que AKS

3. **Azure Kubernetes Service (AKS) com Windows nodes**
   - Kubernetes com nodes Windows
   - Mais complexo, mas escalável

4. **Windows Server VMs com Docker**
   - Controle total
   - Mais trabalho de manutenção

---

## ?? PRÓXIMOS PASSOS

### 1. Validar Windows Containers (30 min)

```powershell
# 1. Trocar para Windows containers
# 2. Executar build
docker-compose build --no-cache

# 3. Iniciar
docker-compose up

# 4. Validar
# - http://localhost:5000/swagger
# - http://localhost:3000

# 5. Se funcionar, commit:
git add .
git commit -m "[DOCKER] Configura Windows containers

- Atualiza Dockerfile.backend para nanoserver
- Atualiza Dockerfile.frontend para IIS
- Configura docker-compose para Windows
- Adiciona guia completo Windows containers"
git push origin develop
```

### 2. Decidir Estratégia de Desenvolvimento

**Se Windows containers funcionarem bem:**
```
? Usar Docker durante desenvolvimento
```

**Se Windows containers derem problemas:**
```
? Desenvolver local
? Validar Docker no final
```

### 3. Começar Desenvolvimento

```powershell
# Criar branches
git checkout -b feature/gestao-ativos
git push -u origin feature/gestao-ativos

# Começar primeira API
cd src\PDPW.API
# ...
```

---

## ?? EXPECTATIVAS DE TEMPO

### Build Inicial (Primeira Vez)

```
SQL Server: ~2-3 min   (download imagem)
Backend:    ~5-10 min  (build .NET + restore)
Frontend:   ~10-20 min (Node.js install + npm build)
????????????????????????????????????????????
TOTAL:      ~15-35 min
```

### Builds Subsequentes (Com Cache)

```
Backend:  ~2-3 min
Frontend: ~3-5 min
????????????????????
TOTAL:    ~5-8 min
```

### Startup (docker-compose up)

```
SQL Server: ~2 min   (inicialização)
Backend:    ~30 seg  (startup .NET)
Frontend:   ~10 seg  (IIS startup)
?????????????????????????????????
TOTAL:      ~3 min
```

---

## ? CHECKLIST DE ENTREGA

### Requisitos do Cliente: Windows Containers

- [ ] Dockerfile.backend usando nanoserver ?
- [ ] Dockerfile.frontend usando IIS ?
- [ ] docker-compose.yml com isolation: process ?
- [ ] docker-compose.yml com driver: nat ?
- [ ] Documentação completa ?
- [ ] Testado em ambiente Windows ?
- [ ] Funcional (backend + frontend + SQL) ?

---

## ?? DOCUMENTAÇÃO ADICIONAL

### Microsoft Docs

- [Windows Containers Overview](https://learn.microsoft.com/en-us/virtualization/windowscontainers/)
- [.NET on Windows Containers](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/container-docker-introduction/)
- [IIS on Windows Containers](https://learn.microsoft.com/en-us/iis/)

### Docker Docs

- [Windows Containers on Docker](https://docs.docker.com/desktop/windows/wsl/)
- [Isolation Modes](https://docs.docker.com/engine/reference/commandline/run/#isolation)

---

**Guia criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? CONFIGURADO PARA WINDOWS CONTAINERS

**Teste e me avise o resultado! ????**

# 🔧 Troubleshooting - PDPw Docker

## 🚨 Problemas Comuns e Soluções

### 1. Containers não iniciam

#### Sintomas:
- `docker-compose up` falha
- Containers em estado `Exit` ou `Restarting`

#### Soluções:

**Opção 1: Limpar tudo e recomeçar**
```bash
# Windows
.\scripts\prepare-demo.bat

# Linux/Mac
bash scripts/prepare-demo.sh
```

**Opção 2: Verificar logs**
```bash
docker-compose logs
docker-compose logs backend
docker-compose logs frontend
docker-compose logs sqlserver
```

**Opção 3: Rebuild forçado**
```bash
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
```

---

### 2. Frontend retorna 404 ou tela branca

#### Sintomas:
- http://localhost:5173 mostra página vazia
- Console do navegador mostra erros 404

#### Soluções:

**Verificar se build foi bem-sucedido:**
```bash
docker-compose logs frontend | grep -i error
```

**Rebuild do frontend:**
```bash
docker-compose stop frontend
docker-compose rm -f frontend
docker-compose up --build frontend -d
```

**Verificar arquivos dentro do container:**
```bash
docker exec pdpw-frontend ls -la /usr/share/nginx/html/
```

**Resultado esperado:**
```
index.html
assets/
vite.svg
```

---

### 3. Backend retorna 500 ou não conecta ao banco

#### Sintomas:
- http://localhost:5001/health retorna erro
- Swagger não carrega
- Logs mostram erro de conexão

#### Soluções:

**Verificar se SQL Server está saudável:**
```bash
docker-compose ps sqlserver
```

**Testar conexão manualmente:**
```bash
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "SELECT 1"
```

**Verificar variáveis de ambiente:**
```bash
docker exec pdpw-backend printenv | grep ConnectionStrings
```

**Recriar banco:**
```bash
docker-compose down -v
docker-compose up sqlserver -d
# Aguardar 2 minutos
docker-compose up backend -d
```

---

### 4. SQL Server não inicia (healthcheck failing)

#### Sintomas:
- Container sqlserver em estado `unhealthy`
- Logs mostram erros de permissão ou memória

#### Soluções:

**Aumentar memória do Docker Desktop:**
1. Docker Desktop → Settings → Resources
2. Aumentar Memory para pelo menos 4GB
3. Apply & Restart

**Verificar logs detalhados:**
```bash
docker-compose logs sqlserver | tail -100
```

**Remover volume corrompido:**
```bash
docker-compose down -v
docker volume rm pdpw_sqldata
docker-compose up sqlserver -d
```

**Verificar porta 1433:**
```bash
# Windows
netstat -ano | findstr :1433

# Linux/Mac
lsof -i :1433
```

---

### 5. Portas em uso

#### Sintomas:
- Erro: "port is already allocated"

#### Soluções:

**Windows:**
```cmd
# Descobrir qual processo está usando a porta
netstat -ano | findstr :5173

# Matar processo (substitua <PID>)
taskkill /PID <PID> /F
```

**Linux/Mac:**
```bash
# Descobrir processo
lsof -i :5173

# Matar processo
kill -9 <PID>
```

**Alternativa: Mudar portas**

Edite `docker-compose.yml`:
```yaml
services:
  frontend:
    ports:
      - "5174:80"  # Mude 5173 para 5174
```

---

### 6. Erro de permissão no volume do SQL Server

#### Sintomas:
- SQL Server não inicia
- Logs mostram "Permission denied"

#### Soluções:

**Linux/Mac:**
```bash
# Dar permissões ao volume
sudo chown -R 10001:10001 ./docker/volumes/sqldata

# Ou remover e recriar
docker-compose down -v
docker volume rm pdpw_sqldata
docker-compose up -d
```

**Windows:**
Não deve ocorrer, mas se ocorrer:
```cmd
docker-compose down -v
docker volume prune -f
docker-compose up -d
```

---

### 7. Frontend não se comunica com Backend

#### Sintomas:
- Frontend carrega, mas consultas falham
- Console mostra erro de CORS ou Network Error

#### Soluções:

**Verificar configuração do nginx:**
```bash
docker exec pdpw-frontend cat /etc/nginx/conf.d/default.conf
```

**Deve conter:**
```nginx
location /api/ {
    proxy_pass http://backend:80/api/;
    ...
}
```

**Verificar se backend está acessível da rede Docker:**
```bash
docker exec pdpw-frontend wget -O- http://backend:80/health
```

**Reiniciar frontend:**
```bash
docker-compose restart frontend
```

---

### 8. Build falha por falta de memória

#### Sintomas:
- Build é interrompido
- Docker Desktop mostra "Out of memory"

#### Soluções:

**Aumentar memória:**
1. Docker Desktop → Settings → Resources
2. Memory: 6GB ou mais
3. Swap: 2GB
4. Apply & Restart

**Build em etapas:**
```bash
# Build apenas backend
docker-compose build backend

# Build apenas frontend
docker-compose build frontend

# Depois inicie tudo
docker-compose up -d
```

---

### 9. Imagens Docker desatualizadas

#### Sintomas:
- Mudanças no código não aparecem
- Comportamento antigo persiste

#### Soluções:

**Rebuild sem cache:**
```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

**Limpar imagens antigas:**
```bash
docker image prune -a
```

---

### 10. Logs muito grandes/performance ruim

#### Sintomas:
- Docker Desktop lento
- Disco cheio

#### Soluções:

**Limitar tamanho de logs:**

Edite `docker-compose.yml`:
```yaml
services:
  backend:
    logging:
      driver: "json-file"
      options:
        max-size: "10m"
        max-file: "3"
```

**Limpar logs manualmente:**
```bash
# Linux/Mac
sudo sh -c "truncate -s 0 /var/lib/docker/containers/*/*-json.log"

# Windows
docker-compose down
docker system prune --volumes
docker-compose up -d
```

---

## 🔍 Comandos de Diagnóstico Avançado

### Verificar uso de recursos
```bash
docker stats
```

### Verificar rede Docker
```bash
docker network inspect pdpw_network
```

### Verificar volumes
```bash
docker volume inspect pdpw_sqldata
```

### Verificar espaço em disco
```bash
docker system df
```

### Limpar tudo (CUIDADO!)
```bash
docker system prune -af --volumes
```

---

## 📞 Checklist de Emergência (Durante Apresentação)

Se algo der errado DURANTE a demo:

1. ✅ **Mantenha a calma**
2. ✅ Mostre slides enquanto investiga
3. ✅ Execute: `docker-compose restart <serviço>`
4. ✅ Verifique logs: `docker-compose logs -f`
5. ✅ Tenha um **plano B**: vídeo gravado ou prints

---

## 🚑 Recuperação Rápida (< 5 minutos)

```bash
# 1. Parar tudo
docker-compose down

# 2. Iniciar apenas o que funciona
docker-compose up -d sqlserver
# Aguardar 1 minuto
docker-compose up -d backend
# Aguardar 30 segundos
docker-compose up -d frontend

# 3. Verificar
docker-compose ps
```

---

## 📝 Logs Úteis para Debug

### Backend
```bash
docker-compose logs backend | grep -i error
docker-compose logs backend | grep -i exception
docker-compose logs backend | tail -50
```

### Frontend
```bash
docker-compose logs frontend | grep -i error
docker-compose logs frontend | tail -50
```

### SQL Server
```bash
docker-compose logs sqlserver | grep -i error
docker-compose logs sqlserver | tail -100
```

---

**Dica:** Mantenha este guia aberto durante a apresentação!

# ?? DOCKER - GUIA RÁPIDO

## ?? INÍCIO RÁPIDO

```powershell
# 1. Subir aplicação
.\docker-start.ps1

# 2. Testar
.\docker-test.ps1

# 3. Abrir Swagger
start http://localhost:5000/swagger

# 4. Parar
.\docker-stop.ps1
```

---

## ?? PRÉ-REQUISITOS

- ? Docker Desktop instalado e rodando
- ? Porta 5000 livre
- ? Porta 1433 livre
- ? 4 GB RAM disponível

---

## ?? PARA A DAILY

### 5 minutos antes:
```powershell
.\docker-start.ps1
```

### Durante apresentação:
```
http://localhost:5000/swagger
```

Demonstrar:
- GET /api/usinas (10 usinas)
- Dados reais (Itaipu, Belo Monte)
- CRUD completo

---

## ?? PROBLEMAS?

### Erro ao subir:
```powershell
docker-compose down -v
.\docker-start.ps1
```

### Ver logs:
```powershell
docker-compose logs -f api
```

### Porta ocupada:
```powershell
netstat -ano | findstr :5000
taskkill /F /PID <PID>
```

---

## ?? COMANDOS ÚTEIS

```powershell
# Status
docker ps

# Logs
docker-compose logs -f

# Restart
docker-compose restart api

# Parar tudo
docker-compose down -v
```

---

## ?? O QUE TEM DENTRO?

```
?? pdpw-api
   ?? .NET 8
   ?? API Usina (8 endpoints)
   ?? Swagger UI
   ?? 10 usinas reais

?? pdpw-sqlserver
   ?? SQL Server 2022
   ?? Database: PDPW_DB_Dev
   ?? Dados: 23 registros
```

---

## ?? URLs

| Serviço | URL |
|---------|-----|
| **Swagger** | http://localhost:5000/swagger |
| **API** | http://localhost:5000/api |
| **Health** | http://localhost:5000/health |
| **SQL Server** | localhost,1433 |

---

## ?? CREDENCIAIS SQL

```
Server: localhost,1433
Database: PDPW_DB_Dev
User: sa
Password: Pdpw@2024!Strong
```

---

## ? CHECKLIST

### Antes da Daily
- [ ] Docker Desktop rodando
- [ ] Executar `.\docker-start.ps1`
- [ ] Aguardar "? APLICAÇÃO RODANDO"
- [ ] Testar `.\docker-test.ps1`
- [ ] Abrir Swagger

### Durante Daily
- [ ] Compartilhar tela
- [ ] Mostrar Swagger
- [ ] GET /api/usinas
- [ ] Destacar dados reais

---

## ?? DOCUMENTAÇÃO COMPLETA

Ver: `docs/GUIA_DOCKER_DAILY.md`

---

**BOA SORTE NA DAILY! ??**

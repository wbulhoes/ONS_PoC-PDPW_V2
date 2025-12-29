# ⚡ INÍCIO RÁPIDO - PDPw v2.0

## 🎯 SETUP EM 1 COMANDO

```powershell
.\setup-ambiente.ps1
```

**Isso irá:**
1. ✅ Verificar todas as dependências
2. ✅ Instalar pacotes do frontend
3. ✅ Compilar backend e frontend
4. ✅ Subir containers Docker
5. ✅ Abrir aplicação no navegador

**Tempo:** 3-5 minutos

---

## 📋 VERIFICAÇÃO MANUAL DO AMBIENTE

Se preferir fazer passo a passo:

### 1️⃣ Instalar Dependências do Frontend

```bash
cd C:\temp\_ONS_PoC-PDPW_V2\frontend
npm install
```

### 2️⃣ Iniciar com Docker

```bash
cd C:\temp\_ONS_PoC-PDPW_V2
docker-compose up -d
```

### 3️⃣ Acessar Aplicação

- **Frontend:** http://localhost:5173
- **Swagger:** http://localhost:5001/swagger

---

## ✅ CHECKLIST DE REQUISITOS

Seu ambiente possui:

| Requisito | Status | Versão |
|-----------|--------|--------|
| Git | ✅ | v2.51.2 |
| .NET 8 Runtime | ✅ | v8.0.22 |
| Node.js | ✅ | v24.12.0 |
| npm | ✅ | v11.6.2 |
| Docker | ✅ | v28.5.1 |
| Docker Compose | ✅ | v2.40.3 |
| Portas 5001, 5173, 1433 | ✅ | Disponíveis |

**Status:** ✅ PRONTO PARA USO

**Ação necessária:** Apenas `npm install` no frontend

---

## 🚀 COMANDOS ÚTEIS

### Docker

```bash
# Iniciar
docker-compose up -d

# Parar
docker-compose down

# Ver logs
docker-compose logs -f

# Status
docker ps
```

### Desenvolvimento Manual

**Backend:**
```bash
cd src\PDPW.API
dotnet run
```

**Frontend:**
```bash
cd frontend
npm run dev
```

---

## 📚 DOCUMENTAÇÃO

- **[RELATORIO_AMBIENTE.md](RELATORIO_AMBIENTE.md)** - Relatório completo da verificação
- **[INDEX.md](INDEX.md)** - Índice de toda documentação
- **[COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md)** - Comandos úteis
- **[FRONTEND_COMPLETO_9_ETAPAS.md](FRONTEND_COMPLETO_9_ETAPAS.md)** - Documentação técnica

---

## 🎯 PRONTO PARA USAR!

Execute o script de setup e em 5 minutos você terá o sistema rodando! 🚀

```powershell
.\setup-ambiente.ps1
```

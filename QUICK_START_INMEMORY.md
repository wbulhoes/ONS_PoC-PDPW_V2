# ?? Início Rápido - PDPW API (SEM SQL Server)

## Executar em 30 segundos

### 1?? Ativar Banco InMemory

Abra: `src\PDPW.API\appsettings.Development.json`

Altere para:
```json
{
  "UseInMemoryDatabase": true
}
```

### 2?? Executar

```powershell
dotnet run --project src\PDPW.API
```

### 3?? Testar

Abra no navegador:
- **Swagger:** https://localhost:65417/swagger
- **Status:** https://localhost:65417/

**Pronto!** A API está funcionando sem SQL Server! ??

---

## O que é InMemory Database?

- ? Banco de dados na memória RAM
- ? Zero configuração
- ? Perfeito para testes e desenvolvimento inicial
- ?? Dados são perdidos ao fechar a aplicação

---

## Testar os Endpoints

### Via Swagger
1. Acesse: https://localhost:65417/swagger
2. Teste os endpoints:
   - `GET /api/DadosEnergeticos` - Listar dados
   - `POST /api/DadosEnergeticos` - Criar novo dado
   - `GET /api/DadosEnergeticos/{id}` - Buscar por ID

### Via PowerShell

```powershell
# Status
curl https://localhost:65417/

# Health Check
curl https://localhost:65417/health

# Listar dados (vazio inicialmente)
curl https://localhost:65417/api/DadosEnergeticos

# Criar um dado
curl -X POST https://localhost:65417/api/DadosEnergeticos `
  -H "Content-Type: application/json" `
  -d '{
    "dataReferencia": "2025-01-17T00:00:00",
    "codigoUsina": "UHE-001",
    "producaoMWh": 1500.50,
    "capacidadeDisponivel": 2000.00,
    "status": "Operacional",
    "observacoes": "Teste inicial"
  }'
```

---

## Quando migrar para SQL Server?

**Continue com InMemory se:**
- Está apenas testando
- Desenvolvimento inicial rápido
- Não precisa salvar dados entre execuções

**Migre para SQL Server/LocalDB quando:**
- Precisa persistir dados
- Vai desenvolver features reais
- Quer testar migrations

**Como migrar:** Veja `DATABASE_SETUP.md`

---

## Verificar se está usando InMemory

Acesse: https://localhost:65417/

Resposta deve incluir:
```json
{
  "status": "running",
  "databaseType": "InMemory (temporário)",
  "version": "v1"
}
```

---

## Comandos Úteis

```powershell
# Executar
dotnet run --project src\PDPW.API

# Build
dotnet build

# Limpar e rebuild
dotnet clean
dotnet build
```

---

## Próximos Passos

1. ? Testou a API com InMemory? Funcionou!
2. ?? Quer persistência? Veja `DATABASE_SETUP.md`
3. ??? Problemas? Veja `TROUBLESHOOTING.md`

**Documentação completa:**
- `DATABASE_SETUP.md` - Guia completo de banco de dados
- `TROUBLESHOOTING.md` - Solução de problemas
- `IMPROVEMENTS.md` - Melhorias implementadas
- `QUICK_START.md` - Guia geral

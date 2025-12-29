# 🎯 ROTEIRO DE VALIDAÇÃO DOCKER + SWAGGER - POC 100%

**Data**: 26/12/2025  
**Objetivo**: Validar POC 100% via Docker e Swagger  
**Tempo Total Estimado**: 30-45 minutos

---

## 📋 CHECKLIST PRÉ-VALIDAÇÃO

- [ ] Docker Desktop instalado e rodando
- [ ] PowerShell 5.1+ disponível
- [ ] Porta 5001 livre
- [ ] Porta 1433 livre (SQL Server)
- [ ] Mínimo 4GB RAM disponível

---

## 🚀 PASSO A PASSO

### **ETAPA 1: Executar Validação Automática** (15 min)

```powershell
# Navegar para o diretório do projeto
cd C:\temp\_ONS_PoC-PDPW_V2

# Executar validação completa
.\scripts\validacao-completa.ps1
```

**O que o script faz**:
1. ✅ Verifica Docker
2. ✅ Para containers antigos
3. ✅ Limpa imagens
4. ✅ Builda containers (API + SQL Server)
5. ✅ Inicia containers
6. ✅ Aguarda SQL Server ficar healthy
7. ✅ Aguarda Backend ficar ready
8. ✅ Testa Swagger UI
9. ✅ Executa 25+ testes automatizados
10. ✅ Gera relatório

**Resultado Esperado**:
```
✅ VALIDAÇÃO COMPLETA: SUCESSO!
📊 Total de Testes:  28
✅ Testes Passaram:  28
❌ Testes Falharam:  0
📈 Taxa de Sucesso:  100%
```

---

### **ETAPA 2: Validação Manual via Swagger** (15-20 min)

#### **2.1 Acessar Swagger**
```
URL: http://localhost:5001/swagger
```

✅ **Verificar**:
- Interface Swagger carrega
- 17 controllers visíveis
- Todos os endpoints listados

---

#### **2.2 Testar Dashboard**

**Teste A: Resumo Geral**
```
GET /api/dashboard/resumo
```
Clicar em "Try it out" → "Execute"

**Verificar resposta**:
```json
{
  "dataHoraAtualizacao": "2024-12-27T...",
  "totalOfertasExportacao": 0,
  "totalUsinas": 15,
  "totalEmpresas": 8,
  ...
}
```

**Teste B: Alertas**
```
GET /api/dashboard/alertas
```

---

#### **2.3 Criar Oferta de Exportação**

**Passo 1**: Obter ID de usina
```
GET /api/usinas
```
Copiar o `id` da primeira usina (ex: 1)

**Passo 2**: Criar oferta
```
POST /api/ofertas-exportacao
```

**Request Body**:
```json
{
  "usinaId": 1,
  "dataOferta": "2025-12-27",
  "dataPDP": "2025-12-29",
  "valorMW": 150.5,
  "precoMWh": 250.75,
  "horaInicial": "08:00:00",
  "horaFinal": "18:00:00",
  "observacoes": "Teste via Swagger - Validação POC 100%"
}
```

**Verificar**:
- Status: 201 Created
- Response com ID gerado
- flgAprovadoONS = null (pendente)

**Passo 3**: Copiar o ID da oferta criada

**Passo 4**: Aprovar oferta
```
POST /api/ofertas-exportacao/{id}/aprovar
```

**Request Body**:
```json
{
  "usuarioONS": "validador@ons.org.br",
  "observacao": "Aprovada na validação POC 100%"
}
```

**Verificar**:
- Status: 200 OK

**Passo 5**: Confirmar aprovação
```
GET /api/ofertas-exportacao/{id}
```

**Verificar**:
- flgAprovadoONS = true
- dataAnaliseONS preenchida
- usuarioAnaliseONS = "validador@ons.org.br"

---

#### **2.4 Fluxo Completo: Programação Energética**

**A. Obter arquivo DADGER aberto**
```
GET /api/arquivosdadger/status/Aberto
```

Copiar ID do primeiro arquivo (ou criar um novo)

**B. Finalizar programação**
```
POST /api/arquivosdadger/{id}/finalizar
```

**Request Body**:
```json
{
  "usuarioFinalizacao": "operador@ons.org.br",
  "observacaoFinalizacao": "Finalizado para validação POC"
}
```

**Verificar**:
- Status = "EmAnalise"

**C. Aprovar programação**
```
POST /api/arquivosdadger/{id}/aprovar
```

**Request Body**:
```json
{
  "usuarioAprovacao": "gerente@ons.org.br",
  "observacaoAprovacao": "Aprovado na validação"
}
```

**Verificar**:
- Status = "Aprovado"
- dataAprovacao preenchida

---

#### **2.5 Criar Previsão Eólica**

**A. Criar previsão**
```
POST /api/previsoes-eolicas
```

**Request Body**:
```json
{
  "usinaId": 1,
  "dataHoraReferencia": "2024-12-27T08:00:00",
  "dataHoraPrevista": "2024-12-28T08:00:00",
  "geracaoPrevistaMWmed": 85.5,
  "velocidadeVentoMS": 12.5,
  "modeloPrevisao": "WRF",
  "horizontePrevisaoHoras": 24,
  "tipoPrevisao": "Curto Prazo"
}
```

Copiar ID da previsão

**B. Atualizar geração real**
```
PATCH /api/previsoes-eolicas/{id}/geracao-real
```

**Request Body**:
```json
{
  "geracaoRealMWmed": 82.3
}
```

**C. Verificar erro calculado**
```
GET /api/previsoes-eolicas/{id}
```

**Verificar**:
- erroAbsolutoMW calculado (82.3 - 85.5 = -3.2)
- erroPercentual calculado

---

### **ETAPA 3: Validação de Logs e Métricas** (5 min)

#### **3.1 Ver logs do backend**
```powershell
docker logs pdpw-backend --tail 100
```

**Verificar**:
- ✅ Migrations aplicadas
- ✅ Seed executado
- ✅ Requests HTTP logados
- ❌ Sem erros críticos

---

#### **3.2 Ver logs do SQL Server**
```powershell
docker logs pdpw-sqlserver --tail 50
```

**Verificar**:
- ✅ SQL Server pronto
- ✅ Database criado
- ❌ Sem erros de conexão

---

#### **3.3 Verificar containers rodando**
```powershell
docker-compose ps
```

**Verificar**:
```
NAME              STATUS          PORTS
pdpw-backend      Up (healthy)    0.0.0.0:5001->80/tcp
pdpw-sqlserver    Up (healthy)    0.0.0.0:1433->1433/tcp
```

---

### **ETAPA 4: Testes Finais de Performance** (5 min)

#### **4.1 Teste de Carga Leve**
```powershell
# Executar 10 requests simultâneos
1..10 | ForEach-Object -Parallel {
    Invoke-RestMethod -Uri "http://localhost:5001/api/dashboard/resumo"
}
```

**Verificar**:
- Todas as requisições retornam 200 OK
- Tempo de resposta < 2s

---

#### **4.2 Dashboard em Tempo Real**

Abrir navegador em:
```
http://localhost:5001/api/dashboard/resumo
```

Atualizar página (F5) várias vezes

**Verificar**:
- Dados consistentes
- Sem erros
- Resposta rápida

---

## ✅ CHECKLIST DE VALIDAÇÃO FINAL

### **Docker**
- [ ] Containers iniciados sem erros
- [ ] SQL Server healthy
- [ ] Backend healthy
- [ ] Migrations aplicadas automaticamente
- [ ] Seed executado com sucesso

### **API**
- [ ] Health endpoint responde
- [ ] Swagger UI acessível
- [ ] Todos os 87 endpoints visíveis
- [ ] Schemas corretos

### **Funcionalidades Críticas**
- [ ] Dashboard retorna métricas
- [ ] Criar oferta de exportação funciona
- [ ] Aprovar/Rejeitar ofertas funciona
- [ ] Finalização de programação funciona
- [ ] Workflow de aprovação completo
- [ ] Previsão eólica com cálculo de erro
- [ ] Dados energéticos com vertimento

### **Qualidade**
- [ ] Logs sem erros críticos
- [ ] Validações de negócio funcionando
- [ ] Dados persistidos no banco
- [ ] Performance aceitável (<2s)

---

## 🎯 RESULTADO ESPERADO

Ao final da validação:

✅ **Docker**: 2 containers rodando (healthy)  
✅ **API**: 87 endpoints funcionais  
✅ **Dashboard**: Métricas em tempo real  
✅ **CRUD**: Todas operações funcionando  
✅ **Workflows**: Aprovação/Finalização OK  
✅ **Validações**: Regras de negócio aplicadas  
✅ **Performance**: < 2s por request  

---

## 📊 RELATÓRIO DE VALIDAÇÃO

Criar arquivo `VALIDACAO_DOCKER_SWAGGER_27-12-2024.md` com:

```markdown
# ✅ Relatório de Validação - POC PDPw 100%

**Data**: 26/12/2025
**Validador**: [Seu Nome]
**Duração**: XX minutos

## Resultados

### Docker
- ✅ Build sem erros
- ✅ Containers iniciados
- ✅ Health checks OK

### Testes Automatizados
- Total: 28 testes
- Passou: 28
- Falhou: 0
- Taxa: 100%

### Testes Manuais (Swagger)
- [ ] Dashboard OK
- [ ] Ofertas Exportação OK
- [ ] Programação OK
- [ ] Previsões Eólicas OK

## Observações
[Adicionar observações]

## Conclusão
✅ POC 100% validada e funcional via Docker
```

---

## 🚀 COMANDOS ÚTEIS

### **Parar tudo**
```powershell
docker-compose down
```

### **Parar e limpar volumes**
```powershell
docker-compose down -v
```

### **Rebuild forçado**
```powershell
docker-compose build --no-cache
docker-compose up -d
```

### **Ver logs em tempo real**
```powershell
docker-compose logs -f
```

---

**📝 Roteiro criado por**: Willian Bulhões + GitHub Copilot  
**Data**: 26/12/2025  
**Status**: ✅ Pronto para execução

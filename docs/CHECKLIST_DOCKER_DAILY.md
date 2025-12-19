# ? CHECKLIST FINAL - DOCKER PARA DAILY

**Data:** 19/12/2024  
**Hora da Daily:** [PREENCHER]  
**Duração:** 10 minutos

---

## ?? OBJETIVO

Demonstrar API PDPW funcionando via Docker com Swagger UI interativo.

---

## ? TIMELINE

### 30 MINUTOS ANTES DA DAILY

#### 1. Preparar Ambiente (5 min)
- [ ] Abrir Docker Desktop
- [ ] Aguardar Docker inicializar completamente
- [ ] Verificar se está rodando: `docker info`

#### 2. Fechar Portas (2 min)
- [ ] Verificar porta 5000: `netstat -ano | findstr :5000`
- [ ] Verificar porta 1433: `netstat -ano | findstr :1433`
- [ ] Matar processos se necessário

#### 3. Subir Aplicação (3 min)
```powershell
cd C:\temp\_ONS_PoC-PDPW
.\docker-start.ps1
```

- [ ] Aguardar mensagem: "? APLICAÇÃO RODANDO COM SUCESSO!"
- [ ] Tempo esperado: ~3-5 minutos (primeira vez)

#### 4. Testar Aplicação (2 min)
```powershell
.\docker-test.ps1
```

- [ ] ? Health Check: OK
- [ ] ? GET /api/usinas: 10 usinas
- [ ] ? GET /api/usinas/1: Itaipu

#### 5. Abrir Swagger (1 min)
```
http://localhost:5000/swagger
```

- [ ] Swagger UI carregou
- [ ] Endpoints visíveis
- [ ] "Try it out" disponível

---

### 10 MINUTOS ANTES DA DAILY

#### 1. Teste Rápido (2 min)
- [ ] Abrir Swagger
- [ ] Testar GET /api/usinas
- [ ] Verificar 10 usinas retornadas
- [ ] Testar GET /api/usinas/1 (Itaipu)

#### 2. Preparar Tela (2 min)
- [ ] Fechar abas desnecessárias
- [ ] Deixar apenas Swagger aberto
- [ ] Ajustar zoom para apresentação
- [ ] Testar compartilhamento de tela

#### 3. Preparar Roteiro (2 min)
- [ ] Revisar `APRESENTACAO_DAILY_DIA1_RESUMO.md`
- [ ] Memorizar pontos principais
- [ ] Ter checklist à mão

---

### DURANTE A DAILY (5 MIN)

#### Parte 1: Introdução (30 seg)
- [ ] "Vou mostrar a API rodando via Docker"
- [ ] Compartilhar tela
- [ ] Mostrar URL: localhost:5000/swagger

#### Parte 2: Swagger UI (1 min)
- [ ] Explicar: "Documentação automática"
- [ ] Mostrar lista de endpoints
- [ ] Destacar: 8 endpoints disponíveis

#### Parte 3: Demo GET Lista (2 min)
- [ ] Expandir `GET /api/usinas`
- [ ] Clicar "Try it out"
- [ ] Clicar "Execute"
- [ ] Mostrar Response:
  - [ ] Status 200 OK
  - [ ] 10 usinas retornadas
  - [ ] Scroll no JSON

#### Parte 4: Destacar Dados (1 min)
- [ ] Apontar: Itaipu (14.000 MW)
- [ ] Apontar: Belo Monte (11.233 MW)
- [ ] Dizer: "Dados reais do SIN"

#### Parte 5: Mostrar CRUD (30 seg)
- [ ] Scroll para baixo
- [ ] Mostrar POST, PUT, DELETE
- [ ] Dizer: "CRUD completo implementado"

#### Parte 6: Encerramento (30 seg)
- [ ] "Tudo rodando via Docker"
- [ ] "Reproduzível em qualquer máquina"
- [ ] Parar compartilhamento

---

### APÓS A DAILY

#### Se precisa manter rodando:
- [ ] Deixar containers ativos
- [ ] Disponível para mais testes

#### Se vai parar:
```powershell
.\docker-stop.ps1
```

---

## ?? PLANO B - SE ALGO DER ERRADO

### Problema 1: Docker não sobe
**Solução:**
```powershell
docker-compose down -v
.\docker-start.ps1
```

### Problema 2: Swagger não abre
**Plano B:** Usar apresentação preparada
- Ir para: `APRESENTACAO_DAILY_DIA1_RESUMO.md`
- Explicar com slides ao invés de demo

### Problema 3: API não responde
**Verificar:**
```powershell
docker-compose logs -f api
```

**Restart:**
```powershell
docker-compose restart api
```

### Problema 4: Sem tempo para Docker
**Alternativa:**
- Usar local: `dotnet run` na API
- Ou pular demo e focar em slides

---

## ?? NÚMEROS PARA MENCIONAR

Durante a demo, cite:
- ? **10 usinas** cadastradas
- ? **41.493 MW** de capacidade total
- ? **8 endpoints** funcionais
- ? **CRUD completo** implementado
- ? **Dados reais** do SIN (ONS)
- ? **Swagger** documentação automática

---

## ?? FRASES PREPARADAS

### Início:
> "Agora vou mostrar a API funcionando via Docker com dados reais do setor elétrico."

### Swagger:
> "Este é o Swagger UI, que documenta automaticamente todos os endpoints da nossa API."

### Executando:
> "Vou buscar todas as usinas cadastradas. Execute..."

### Resultado:
> "Veja, retornou 10 usinas com dados reais: Itaipu com 14.000 MW, Belo Monte com 11.233 MW..."

### CRUD:
> "Temos o CRUD completo: GET para buscar, POST para criar, PUT para atualizar e DELETE para remover."

### Docker:
> "Tudo isso está rodando via Docker, então é 100% reproduzível em qualquer máquina."

---

## ?? O QUE NÃO FAZER

? Não falar muito rápido  
? Não entrar em detalhes técnicos demais  
? Não testar endpoints sem preparar antes  
? Não esquecer de compartilhar tela  
? Não prometer features não implementadas  

---

## ? O QUE FAZER

? Falar devagar e claro  
? Focar nos resultados (10 usinas, dados reais)  
? Preparar e testar antes  
? Compartilhar tela cedo  
? Ser honesto sobre o que está pronto  

---

## ?? OBJETIVO DA DEMO

**Provar que:**
1. ? API está funcionando
2. ? Dados reais estão cadastrados
3. ? CRUD completo implementado
4. ? Documentação automática (Swagger)
5. ? Reproduzível via Docker

**Não precisa:**
- ? Testar todos os endpoints
- ? Explicar código
- ? Fazer POST/PUT/DELETE ao vivo
- ? Entrar em detalhes de infraestrutura

---

## ?? NOTAS DE ÚLTIMA HORA

**Escrever aqui qualquer ajuste:**

```
____________________________________________
____________________________________________
____________________________________________
```

---

## ?? ROTEIRO VISUAL

```
1. Compartilhar Tela
   ?
2. Mostrar URL: localhost:5000/swagger
   ?
3. Explicar Swagger UI
   ?
4. Expandir GET /api/usinas
   ?
5. Try it out ? Execute
   ?
6. Mostrar 10 usinas
   ?
7. Destacar dados reais
   ?
8. Scroll para POST/PUT/DELETE
   ?
9. Dizer: "CRUD completo"
   ?
10. Parar compartilhamento
```

---

## ?? TEMPO POR SEÇÃO

```
Introdução:        30 seg
Swagger UI:         1 min
Demo GET:           2 min
Dados reais:        1 min
Mostrar CRUD:      30 seg
Encerramento:      30 seg
????????????????????????
TOTAL:             5 min 30 seg
```

**Margem:** 30 seg para perguntas

---

## ? VERIFICAÇÃO FINAL

### 5 minutos antes:

```powershell
# 1. Containers rodando?
docker ps

# Deve mostrar:
# pdpw-api        Up
# pdpw-sqlserver  Up
```

```powershell
# 2. API respondendo?
curl http://localhost:5000/health

# Deve retornar:
# {"status":"Healthy"}
```

```powershell
# 3. Swagger acessível?
start http://localhost:5000/swagger

# Deve abrir navegador com Swagger UI
```

```powershell
# 4. Dados presentes?
curl http://localhost:5000/api/usinas

# Deve retornar array com 10 usinas
```

**Se todos ? ? PRONTO!**

---

## ?? MENSAGEM FINAL

```
?????????????????????????????????????????
?                                       ?
?   VOCÊ ESTÁ PREPARADO! ??             ?
?                                       ?
?   • Docker configurado ?             ?
?   • Scripts prontos ?                ?
?   • Roteiro definido ?               ?
?   • Demo testada ?                   ?
?                                       ?
?   CONFIANÇA: 100%                     ?
?                                       ?
?   BOA SORTE NA DAILY! ??              ?
?                                       ?
?   MOSTRE O PODER DO DOCKER! ??        ?
?                                       ?
?????????????????????????????????????????
```

---

**RESPIRE FUNDO E VAMOS LÁ! ????**

**VOCÊ TEM TUDO PRONTO!**

**SUCESSO! ??**

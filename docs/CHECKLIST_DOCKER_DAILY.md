# ? CHECKLIST FINAL - DOCKER PARA DAILY

**Data:** 19/12/2024  
**Hora da Daily:** [PREENCHER]  
**Dura��o:** 10 minutos

---

## ?? OBJETIVO

Demonstrar API PDPW funcionando via Docker com Swagger UI interativo.

---

## ? TIMELINE

### 30 MINUTOS ANTES DA DAILY

#### 1. Preparar Ambiente (5 min)
- [ ] Abrir Docker Desktop
- [ ] Aguardar Docker inicializar completamente
- [ ] Verificar se est� rodando: `docker info`

#### 2. Fechar Portas (2 min)
- [ ] Verificar porta 5000: `netstat -ano | findstr :5000`
- [ ] Verificar porta 1433: `netstat -ano | findstr :1433`
- [ ] Matar processos se necess�rio

#### 3. Subir Aplica��o (3 min)
```powershell
cd C:\temp\_ONS_PoC-PDPW
.\docker-start.ps1
```

- [ ] Aguardar mensagem: "? APLICA��O RODANDO COM SUCESSO!"
- [ ] Tempo esperado: ~3-5 minutos (primeira vez)

#### 4. Testar Aplica��o (2 min)
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
- [ ] Endpoints vis�veis
- [ ] "Try it out" dispon�vel

---

### 10 MINUTOS ANTES DA DAILY

#### 1. Teste R�pido (2 min)
- [ ] Abrir Swagger
- [ ] Testar GET /api/usinas
- [ ] Verificar 10 usinas retornadas
- [ ] Testar GET /api/usinas/1 (Itaipu)

#### 2. Preparar Tela (2 min)
- [ ] Fechar abas desnecess�rias
- [ ] Deixar apenas Swagger aberto
- [ ] Ajustar zoom para apresenta��o
- [ ] Testar compartilhamento de tela

#### 3. Preparar Roteiro (2 min)
- [ ] Revisar `APRESENTACAO_DAILY_DIA1_RESUMO.md`
- [ ] Memorizar pontos principais
- [ ] Ter checklist � m�o

---

### DURANTE A DAILY (5 MIN)

#### Parte 1: Introdu��o (30 seg)
- [ ] "Vou mostrar a API rodando via Docker"
- [ ] Compartilhar tela
- [ ] Mostrar URL: localhost:5000/swagger

#### Parte 2: Swagger UI (1 min)
- [ ] Explicar: "Documenta��o autom�tica"
- [ ] Mostrar lista de endpoints
- [ ] Destacar: 8 endpoints dispon�veis

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
- [ ] "Reproduz�vel em qualquer m�quina"
- [ ] Parar compartilhamento

---

### AP�S A DAILY

#### Se precisa manter rodando:
- [ ] Deixar containers ativos
- [ ] Dispon�vel para mais testes

#### Se vai parar:
```powershell
.\docker-stop.ps1
```

---

## ?? PLANO B - SE ALGO DER ERRADO

### Problema 1: Docker n�o sobe
**Solu��o:**
```powershell
docker-compose down -v
.\docker-start.ps1
```

### Problema 2: Swagger n�o abre
**Plano B:** Usar apresenta��o preparada
- Ir para: `APRESENTACAO_DAILY_DIA1_RESUMO.md`
- Explicar com slides ao inv�s de demo

### Problema 3: API n�o responde
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

## ?? N�MEROS PARA MENCIONAR

Durante a demo, cite:
- ? **10 usinas** cadastradas
- ? **41.493 MW** de capacidade total
- ? **8 endpoints** funcionais
- ? **CRUD completo** implementado
- ? **Dados reais** do SIN (ONS)
- ? **Swagger** documenta��o autom�tica

---

## ?? FRASES PREPARADAS

### In�cio:
> "Agora vou mostrar a API funcionando via Docker com dados reais do setor el�trico."

### Swagger:
> "Este � o Swagger UI, que documenta automaticamente todos os endpoints da nossa API."

### Executando:
> "Vou buscar todas as usinas cadastradas. Execute..."

### Resultado:
> "Veja, retornou 10 usinas com dados reais: Itaipu com 14.000 MW, Belo Monte com 11.233 MW..."

### CRUD:
> "Temos o CRUD completo: GET para buscar, POST para criar, PUT para atualizar e DELETE para remover."

### Docker:
> "Tudo isso est� rodando via Docker, ent�o � 100% reproduz�vel em qualquer m�quina."

---

## ?? O QUE N�O FAZER

? N�o falar muito r�pido  
? N�o entrar em detalhes t�cnicos demais  
? N�o testar endpoints sem preparar antes  
? N�o esquecer de compartilhar tela  
? N�o prometer features n�o implementadas  

---

## ? O QUE FAZER

? Falar devagar e claro  
? Focar nos resultados (10 usinas, dados reais)  
? Preparar e testar antes  
? Compartilhar tela cedo  
? Ser honesto sobre o que est� pronto  

---

## ?? OBJETIVO DA DEMO

**Provar que:**
1. ? API est� funcionando
2. ? Dados reais est�o cadastrados
3. ? CRUD completo implementado
4. ? Documenta��o autom�tica (Swagger)
5. ? Reproduz�vel via Docker

**N�o precisa:**
- ? Testar todos os endpoints
- ? Explicar c�digo
- ? Fazer POST/PUT/DELETE ao vivo
- ? Entrar em detalhes de infraestrutura

---

## ?? NOTAS DE �LTIMA HORA

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

## ?? TEMPO POR SE��O

```
Introdu��o:        30 seg
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

## ? VERIFICA��O FINAL

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
# 3. Swagger acess�vel?
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
?   VOC� EST� PREPARADO! ??             ?
?                                       ?
?   � Docker configurado ?             ?
?   � Scripts prontos ?                ?
?   � Roteiro definido ?               ?
?   � Demo testada ?                   ?
?                                       ?
?   CONFIAN�A: 100%                     ?
?                                       ?
?   BOA SORTE NA DAILY! ??              ?
?                                       ?
?   MOSTRE O PODER DO DOCKER! ??        ?
?                                       ?
?????????????????????????????????????????
```

---

**RESPIRE FUNDO E VAMOS L�! ????**

**VOC� TEM TUDO PRONTO!**

**SUCESSO! ??**

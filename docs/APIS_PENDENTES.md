# ?? 24 APIs PENDENTES - Detalhamento Completo

**Status:** ? N�o iniciado  
**Branch:** `feature/backend`  
**Objetivo:** Implementar backend completo do PDPw

---

## ?? VIS�O GERAL

| Categoria | Quantidade | Status |
|-----------|------------|--------|
| **Opera��o Energ�tica** | 5 APIs | ? |
| **Cargas e Balan�o** | 3 APIs | ? |
| **Consolidados** | 3 APIs | ? |
| **T�rmicas e Contratos** | 4 APIs | ? |
| **Paradas e Motivos** | 2 APIs | ? |
| **Documentos** | 4 APIs | ? |
| **Administrativo** | 2 APIs | ? |
| **Legado (Refactor)** | 1 API | ? |
| **TOTAL** | **24 APIs** | **0%** |

---

## ?? GRUPO A: OPERA��O ENERG�TICA (Prioridade ALTA)

### 1. **ArquivoDadger API** ???
**Arquivo:** `ArquivosDadgerController.cs`  
**Prioridade:** ALTA (SLICE 2)  
**Complexidade:** ALTA  
**Estimativa:** 1.5 dias

**Entidades:**
- `ArquivoDadger`
- `ArquivoDadgerValor`

**Endpoints (5):**
```
GET    /api/arquivos-dadger              # Listar todos
GET    /api/arquivos-dadger/{id}         # Buscar por ID
GET    /api/arquivos-dadger/semana/{id}  # Por semana PMO
POST   /api/arquivos-dadger              # Upload de arquivo
DELETE /api/arquivos-dadger/{id}         # Remover
```

**Legado:** `ArquivoDadgerValorDAO.vb`

**Complexidade:**
- Relacionamento com SemanaPMO
- Parsing de arquivo DADGER
- M�ltiplos valores por arquivo
- C�lculos de inflexibilidade

---

### 2. **UnidadeGeradora API** ???
**Arquivo:** `UnidadesGeradorasController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `UnidadeGeradora`

**Endpoints (8):**
```
GET    /api/unidades-geradoras                    # Listar todas
GET    /api/unidades-geradoras/{id}               # Buscar por ID
GET    /api/unidades-geradoras/codigo/{codigo}    # Por c�digo
GET    /api/unidades-geradoras/usina/{usinaId}    # Por usina
POST   /api/unidades-geradoras                    # Criar
PUT    /api/unidades-geradoras/{id}               # Atualizar
DELETE /api/unidades-geradoras/{id}               # Remover
GET    /api/unidades-geradoras/verificar-codigo/{codigo} # Validar c�digo
```

**Legado:** `UnidadeGeradoraDAO.vb`

**Relacionamentos:**
- N:1 com UsinaGeradora
- 1:N com ParadaUG
- 1:N com RestricaoUG

---

### 3. **RestricaoUG API** ???
**Arquivo:** `RestricoesUGController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `RestricaoUG`

**Endpoints (6):**
```
GET    /api/restricoes-ug                     # Listar todas
GET    /api/restricoes-ug/{id}                # Buscar por ID
GET    /api/restricoes-ug/unidade/{ugId}      # Por unidade geradora
GET    /api/restricoes-ug/periodo             # Por per�odo (query params)
POST   /api/restricoes-ug                     # Criar
DELETE /api/restricoes-ug/{id}                # Remover
```

**Legado:** `RestricaoUGDAO.vb`

**Relacionamentos:**
- N:1 com UnidadeGeradora
- N:1 com MotivoRestricao

---

### 4. **RestricaoUS API** ???
**Arquivo:** `RestricoesUSController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `RestricaoUS`

**Endpoints (6):**
```
GET    /api/restricoes-us                 # Listar todas
GET    /api/restricoes-us/{id}            # Buscar por ID
GET    /api/restricoes-us/usina/{usinaId} # Por usina
GET    /api/restricoes-us/periodo         # Por per�odo
POST   /api/restricoes-us                 # Criar
DELETE /api/restricoes-us/{id}            # Remover
```

**Legado:** `RestricaoUSDAO.vb`

**Relacionamentos:**
- N:1 com UsinaGeradora
- N:1 com MotivoRestricao

---

### 5. **GerForaMerito API** ???
**Arquivo:** `GeracoesForaMeritoController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `GerForaMerito`

**Endpoints (6):**
```
GET    /api/geracoes-fora-merito                # Listar todas
GET    /api/geracoes-fora-merito/{id}           # Buscar por ID
GET    /api/geracoes-fora-merito/usina/{usinaId} # Por usina
GET    /api/geracoes-fora-merito/periodo        # Por per�odo
POST   /api/geracoes-fora-merito                # Criar
DELETE /api/geracoes-fora-merito/{id}           # Remover
```

**Legado:** `GerForaMeritoDAO.vb`

**Relacionamento:**
- N:1 com UsinaGeradora

---

## ? GRUPO B: CARGAS E BALAN�O (Prioridade ALTA)

### 6. **Carga API** ???
**Arquivo:** `CargasController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `Carga`

**Endpoints (8):**
```
GET    /api/cargas                      # Listar todas
GET    /api/cargas/{id}                 # Buscar por ID
GET    /api/cargas/subsistema/{id}      # Por subsistema
GET    /api/cargas/periodo              # Por per�odo
GET    /api/cargas/verificada           # Carga verificada
GET    /api/cargas/previsao             # Previs�o de carga
POST   /api/cargas                      # Criar
PUT    /api/cargas/{id}                 # Atualizar
```

**Legado:** `CargaDAO.vb`

**Campos importantes:**
- SubsistemaId (SE, SUL, NE, N)
- CargaMWmed
- CargaVerificada
- PrevisaoCarga

---

### 7. **Intercambio API** ???
**Arquivo:** `IntercambiosController.cs`  
**Prioridade:** ALTA  
**Complexidade:** M�DIA  
**Estimativa:** 1 dia

**Entidade:** `Intercambio`

**Endpoints (8):**
```
GET    /api/intercambios                # Listar todos
GET    /api/intercambios/{id}           # Buscar por ID
GET    /api/intercambios/origem/{id}    # Por subsistema origem
GET    /api/intercambios/destino/{id}   # Por subsistema destino
GET    /api/intercambios/periodo        # Por per�odo
GET    /api/intercambios/fluxo          # Fluxo entre subsistemas
POST   /api/intercambios                # Criar
PUT    /api/intercambios/{id}           # Atualizar
```

**Legado:** `IntercambioDAO.vb`

**L�gica especial:**
- Fluxo bidirecional entre subsistemas
- C�lculo de perdas

---

### 8. **Balanco API** ???
**Arquivo:** `BalancosController.cs`  
**Prioridade:** ALTA  
**Complexidade:** ALTA  
**Estimativa:** 1 dia

**Entidade:** `Balanco`

**Endpoints (8):**
```
GET    /api/balancos                    # Listar todos
GET    /api/balancos/{id}               # Buscar por ID
GET    /api/balancos/subsistema/{id}    # Por subsistema
GET    /api/balancos/periodo            # Por per�odo
GET    /api/balancos/calcular           # Calcular balan�o
GET    /api/balancos/deficit            # An�lise de d�ficit
POST   /api/balancos                    # Criar
PUT    /api/balancos/{id}               # Atualizar
```

**Legado:** `BalancoDAO.vb`

**C�lculos:**
- Gera��o - Carga + Interc�mbio - Perdas = D�ficit
- Agrega��o de dados de m�ltiplas fontes

---

## ?? GRUPO C: CONSOLIDADOS (Prioridade M�DIA)

### 9. **DCA API** ??
**Arquivo:** `DCAsController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `DCA` (Declara��o de Carga Agregada)

**Endpoints (8):**
```
GET    /api/dcas                        # Listar todos
GET    /api/dcas/{id}                   # Buscar por ID
GET    /api/dcas/semana/{id}            # Por semana PMO
GET    /api/dcas/agente/{id}            # Por agente
GET    /api/dcas/status/{status}        # Por status
POST   /api/dcas                        # Criar
PUT    /api/dcas/{id}                   # Atualizar
DELETE /api/dcas/{id}                   # Remover
```

**Legado:** `DCADAO.vb`

**Relacionamentos:**
- N:1 com SemanaPMO
- 1:N com DCR

---

### 10. **DCR API** ??
**Arquivo:** `DCRsController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `DCR` (Declara��o de Carga Revisada)

**Endpoints (8):**
```
GET    /api/dcrs                        # Listar todos
GET    /api/dcrs/{id}                   # Buscar por ID
GET    /api/dcrs/dca/{dcaId}            # Por DCA
GET    /api/dcrs/semana/{id}            # Por semana PMO
GET    /api/dcrs/revisoes               # Hist�rico de revis�es
POST   /api/dcrs                        # Criar
PUT    /api/dcrs/{id}                   # Atualizar
DELETE /api/dcrs/{id}                   # Remover
```

**Legado:** `DCRDAO.vb`

**Relacionamentos:**
- N:1 com DCA (opcional)
- N:1 com SemanaPMO

---

### 11. **Responsavel API** ??
**Arquivo:** `ResponsaveisController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `Responsavel`

**Endpoints (6):**
```
GET    /api/responsaveis                # Listar todos
GET    /api/responsaveis/{id}           # Buscar por ID
GET    /api/responsaveis/email/{email}  # Por email
POST   /api/responsaveis                # Criar
PUT    /api/responsaveis/{id}           # Atualizar
DELETE /api/responsaveis/{id}           # Remover
```

**Legado:** `ResponsavelDAO.vb`

---

## ?? GRUPO D: T�RMICAS E CONTRATOS (Prioridade M�DIA)

### 12. **ModalidadeOpTermica API** ??
**Arquivo:** `ModalidadesOpTermicaController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `ModalidadeOpTermica`

**Endpoints (8):**
```
GET    /api/modalidades-op-termica      # Listar todas
GET    /api/modalidades-op-termica/{id} # Buscar por ID
GET    /api/modalidades-op-termica/nome/{nome} # Por nome
POST   /api/modalidades-op-termica      # Criar
PUT    /api/modalidades-op-termica/{id} # Atualizar
DELETE /api/modalidades-op-termica/{id} # Remover
GET    /api/modalidades-op-termica/custos # An�lise de custos
GET    /api/modalidades-op-termica/potencias # Faixas de pot�ncia
```

**Legado:** `ModalidadeOpTermicaDAO.vb`

---

### 13. **InflexibilidadeContratada API** ??
**Arquivo:** `InflexibilidadesContratadasController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `InflexibilidadeContratada`

**Endpoints (8):**
```
GET    /api/inflexibilidades-contratadas           # Listar todas
GET    /api/inflexibilidades-contratadas/{id}      # Buscar por ID
GET    /api/inflexibilidades-contratadas/usina/{usinaId} # Por usina
GET    /api/inflexibilidades-contratadas/periodo   # Por per�odo
GET    /api/inflexibilidades-contratadas/ativas    # Contratos ativos
POST   /api/inflexibilidades-contratadas           # Criar
PUT    /api/inflexibilidades-contratadas/{id}      # Atualizar
DELETE /api/inflexibilidades-contratadas/{id}      # Remover
```

**Legado:** `InflexibilidadeContratadaDAO.vb`

---

### 14. **RampasUsinaTermica API** ??
**Arquivo:** `RampasUsinasTermicasController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `RampasUsinaTermica`

**Endpoints (8):**
```
GET    /api/rampas-usinas-termicas           # Listar todas
GET    /api/rampas-usinas-termicas/{id}      # Buscar por ID
GET    /api/rampas-usinas-termicas/usina/{usinaId} # Por usina
GET    /api/rampas-usinas-termicas/validar   # Validar rampas
POST   /api/rampas-usinas-termicas           # Criar
PUT    /api/rampas-usinas-termicas/{id}      # Atualizar
DELETE /api/rampas-usinas-termicas/{id}      # Remover
GET    /api/rampas-usinas-termicas/tempos    # An�lise de tempos
```

**Legado:** `RampasUsinaTermicaDAO.vb`

**Campos:**
- RampaSubida (MW/min)
- RampaDescida (MW/min)
- TempoPartida (horas)
- TempoParada (horas)

---

### 15. **UsinaConversora API** ??
**Arquivo:** `UsinasConversorasController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `UsinaConversora`

**Endpoints (6):**
```
GET    /api/usinas-conversoras              # Listar todas
GET    /api/usinas-conversoras/{id}         # Buscar por ID
GET    /api/usinas-conversoras/usina/{usinaId} # Por usina
POST   /api/usinas-conversoras              # Criar
PUT    /api/usinas-conversoras/{id}         # Atualizar
DELETE /api/usinas-conversoras/{id}         # Remover
```

**Legado:** `UsinaConversoraDAO.vb`

---

## ?? GRUPO E: PARADAS E MOTIVOS (Prioridade M�DIA)

### 16. **ParadaUG API** ??
**Arquivo:** `ParadasUGController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `ParadaUG`

**Endpoints (8):**
```
GET    /api/paradas-ug                      # Listar todas
GET    /api/paradas-ug/{id}                 # Buscar por ID
GET    /api/paradas-ug/unidade/{ugId}       # Por unidade geradora
GET    /api/paradas-ug/periodo              # Por per�odo
GET    /api/paradas-ug/programadas          # Paradas programadas
GET    /api/paradas-ug/forcadas             # Paradas for�adas
POST   /api/paradas-ug                      # Criar
PUT    /api/paradas-ug/{id}                 # Atualizar
```

**Legado:** `ParadaUGDAO.vb`

---

### 17. **MotivoRestricao API** ??
**Arquivo:** `MotivosRestricaoController.cs`  
**Prioridade:** M�DIA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `MotivoRestricao`

**Endpoints (6):**
```
GET    /api/motivos-restricao               # Listar todos
GET    /api/motivos-restricao/{id}          # Buscar por ID
GET    /api/motivos-restricao/categoria/{cat} # Por categoria
POST   /api/motivos-restricao               # Criar
PUT    /api/motivos-restricao/{id}          # Atualizar
DELETE /api/motivos-restricao/{id}          # Remover
```

**Legado:** `MotivoRestricaoDAO.vb`

---

## ?? GRUPO F: DOCUMENTOS (Prioridade BAIXA)

### 18. **Upload API** ?
**Arquivo:** `UploadsController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `Upload`

**Endpoints (6):**
```
GET    /api/uploads                 # Listar todos
GET    /api/uploads/{id}            # Buscar por ID
GET    /api/uploads/{id}/download   # Download do arquivo
POST   /api/uploads                 # Upload de arquivo
DELETE /api/uploads/{id}            # Remover
GET    /api/uploads/tipos/{tipo}    # Por tipo de arquivo
```

**Legado:** `UploadDAO.vb`

---

### 19. **Relatorio API** ?
**Arquivo:** `RelatoriosController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `Relatorio`

**Endpoints (8):**
```
GET    /api/relatorios                  # Listar todos
GET    /api/relatorios/{id}             # Buscar por ID
GET    /api/relatorios/tipo/{tipo}      # Por tipo
GET    /api/relatorios/gerar            # Gerar relat�rio
GET    /api/relatorios/{id}/exportar    # Exportar (PDF/Excel)
POST   /api/relatorios                  # Criar
PUT    /api/relatorios/{id}             # Atualizar
DELETE /api/relatorios/{id}             # Remover
```

**Legado:** `RelatorioDAO.vb`

---

### 20. **Arquivo API** ?
**Arquivo:** `ArquivosController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `Arquivo`

**Endpoints (8):**
```
GET    /api/arquivos                    # Listar todos
GET    /api/arquivos/{id}               # Buscar por ID
GET    /api/arquivos/diretorio/{dirId}  # Por diret�rio
GET    /api/arquivos/{id}/download      # Download
POST   /api/arquivos                    # Upload
PUT    /api/arquivos/{id}               # Atualizar metadados
DELETE /api/arquivos/{id}               # Remover
GET    /api/arquivos/buscar             # Busca por nome/extens�o
```

**Legado:** `ArquivoDAO.vb`

---

### 21. **Diretorio API** ?
**Arquivo:** `DiretoriosController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `Diretorio`

**Endpoints (8):**
```
GET    /api/diretorios                  # Listar todos
GET    /api/diretorios/{id}             # Buscar por ID
GET    /api/diretorios/{id}/subdiretorios # Subdiretorios
GET    /api/diretorios/{id}/arquivos    # Arquivos do diret�rio
GET    /api/diretorios/raiz             # Diret�rio raiz
POST   /api/diretorios                  # Criar
PUT    /api/diretorios/{id}             # Atualizar
DELETE /api/diretorios/{id}             # Remover
```

**Legado:** `DiretorioDAO.vb`

---

## ?? GRUPO G: ADMINISTRATIVO (Prioridade BAIXA)

### 22. **Usuario API** ?
**Arquivo:** `UsuariosController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** M�DIA  
**Estimativa:** 0.5 dia

**Entidade:** `Usuario`

**Endpoints (8):**
```
GET    /api/usuarios                    # Listar todos
GET    /api/usuarios/{id}               # Buscar por ID
GET    /api/usuarios/email/{email}      # Por email
GET    /api/usuarios/equipe/{equipeId}  # Por equipe
GET    /api/usuarios/perfil/{perfil}    # Por perfil
POST   /api/usuarios                    # Criar
PUT    /api/usuarios/{id}               # Atualizar
DELETE /api/usuarios/{id}               # Remover
```

**Legado:** `UsuarioDAO.vb`

**Nota:** Sem autentica��o completa na POC

---

### 23. **Observacao API** ?
**Arquivo:** `ObservacoesController.cs`  
**Prioridade:** BAIXA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**Entidade:** `Observacao`

**Endpoints (8):**
```
GET    /api/observacoes                 # Listar todas
GET    /api/observacoes/{id}            # Buscar por ID
GET    /api/observacoes/buscar          # Busca por t�tulo/conte�do
GET    /api/observacoes/periodo         # Por per�odo
GET    /api/observacoes/autor/{autor}   # Por autor
POST   /api/observacoes                 # Criar
PUT    /api/observacoes/{id}            # Atualizar
DELETE /api/observacoes/{id}            # Remover
```

**Legado:** `ObservacaoDAO.vb`

---

## ?? GRUPO H: LEGADO (Refactor)

### 24. **DadoEnergetico API** ?
**Arquivo:** `DadosEnergeticosController.cs` (existente)  
**Prioridade:** BAIXA  
**Complexidade:** BAIXA  
**Estimativa:** 0.5 dia

**A��o:** Refatorar para usar Result<T> e linguagem ub�qua

---

## ?? RESUMO EXECUTIVO

### **Total de Trabalho:**
- **24 APIs** a implementar
- **~170 endpoints** a criar
- **Estimativa total:** ~20 dias �teis
- **Com testes:** ~30 dias �teis

### **Distribui��o por Prioridade:**
- ??? **ALTA:** 8 APIs (33%)
- ?? **M�DIA:** 11 APIs (46%)
- ? **BAIXA:** 5 APIs (21%)

### **Distribui��o por Complexidade:**
- **ALTA:** 3 APIs (12%)
- **M�DIA:** 13 APIs (54%)
- **BAIXA:** 8 APIs (34%)

---

## ?? ESTRAT�GIA DE IMPLEMENTA��O

### **Abordagem Recomendada:**

1. **Semana 1-2:** Grupo A + B (8 APIs cr�ticas)
2. **Semana 3:** Grupo C + D (7 APIs de neg�cio)
3. **Semana 4:** Grupo E + F + G + H (9 APIs complementares)
4. **Semana 5:** Testes, refatora��o e documenta��o

### **Velocidade Esperada:**
- **Com foco:** 2-3 APIs/dia (APIs simples)
- **Com qualidade:** 1-2 APIs/dia (com testes)
- **Realista:** 1.5 APIs/dia (m�dia ponderada)

### **Marco de Sucesso:**
? 29 APIs implementadas  
? ~200 endpoints funcionando  
? >80% cobertura de testes  
? Documenta��o completa  
? Backend pronto para produ��o  

---

**Criado em:** 20/12/2024  
**Branch:** `feature/backend`  
**Pr�ximo passo:** Implementar Fase 1 (Funda��o)

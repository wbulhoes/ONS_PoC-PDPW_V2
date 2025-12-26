# 🎯 TESTE MASTER COMPLETO - POC PDPW

## 📋 Descrição

Script PowerShell **DEFINITIVO** para validação **completa** de todas as APIs do sistema PDPw. Testa **todos os métodos HTTP** em **todas as 17 APIs**, criando recursos de teste e gerando relatório detalhado em JSON.

## 🚀 Como Usar

### **Execução Padrão**
```powershell
.\scripts\TESTE-MASTER-COMPLETO.ps1
```

### **Com Modo Verbose**
```powershell
.\scripts\TESTE-MASTER-COMPLETO.ps1 -Verbose
```

### **Parar no Primeiro Erro**
```powershell
.\scripts\TESTE-MASTER-COMPLETO.ps1 -StopOnError
```

### **Incluir Testes de DELETE**
```powershell
.\scripts\TESTE-MASTER-COMPLETO.ps1 -IncludeDelete
```

### **URL Customizada**
```powershell
.\scripts\TESTE-MASTER-COMPLETO.ps1 -BaseUrl "http://seu-servidor:porta"
```

## 📊 O Que é Testado

### **17 APIs Completas**

| # | API | Endpoints | Métodos Testados |
|---|-----|-----------|------------------|
| 1 | Dashboard | 3 | GET |
| 2 | Usinas | 15+ | GET, POST, PUT, DELETE |
| 3 | Empresas | 12+ | GET, POST, PUT, DELETE |
| 4 | Tipos de Usina | 8+ | GET, POST, PUT, DELETE |
| 5 | Semanas PMO | 12+ | GET, POST, PUT, DELETE |
| 6 | Equipes PDP | 10+ | GET, POST, PUT, DELETE |
| 7 | Usuários | 9+ | GET, POST, PUT, DELETE |
| 8 | Ofertas Exportação | 15+ | GET, POST, PUT, PATCH |
| 9 | Ofertas Resposta Voluntária | 15+ | GET, POST, PUT, PATCH |
| 10 | Previsões Eólicas | 12+ | GET, POST, PUT |
| 11 | Arquivos DADGER | 15+ | GET, POST, PUT, PATCH |
| 12 | Cargas | 10+ | GET, POST, PUT, DELETE |
| 13 | Intercâmbios | 10+ | GET, POST, PUT, DELETE |
| 14 | Balanços | 10+ | GET, POST, PUT, DELETE |
| 15 | Unidades Geradoras | 10+ | GET, POST, PUT, DELETE |
| 16 | Paradas UG | 10+ | GET, POST, PUT, DELETE |
| 17 | Dados Energéticos | 10+ | GET, POST, PUT, DELETE |

**Total**: ~200 testes individuais

## ✅ Funcionalidades

### **Testes Realizados**
- ✅ GET - Listar recursos
- ✅ GET por ID - Buscar recurso específico
- ✅ GET com filtros - Buscar por parâmetros
- ✅ POST - Criar novos recursos
- ✅ PUT - Atualizar recursos existentes
- ✅ PATCH - Atualização parcial
- ✅ DELETE - Remover recursos (opcional)

### **Validações**
- ✅ Status codes HTTP corretos
- ✅ Formato JSON de resposta
- ✅ Criação bem-sucedida de recursos
- ✅ Relacionamentos entre entidades
- ✅ Regras de negócio básicas

### **Relatórios**
- ✅ Console colorido em tempo real
- ✅ Estatísticas detalhadas
- ✅ Lista de endpoints com falha
- ✅ IDs de recursos criados
- ✅ Arquivo JSON completo

## 📄 Relatório JSON

O script gera um arquivo JSON detalhado com:

```json
{
  "Timestamp": "2024-12-26T18:00:00",
  "BaseUrl": "http://localhost:5001",
  "Statistics": {
    "TotalTests": 200,
    "PassedTests": 195,
    "FailedTests": 5,
    "SuccessRate": 97.5,
    "Duration": "02:15"
  },
  "CreatedResources": {
    "Usina": 1001,
    "Empresa": 2001,
    "Usuario": 3001
  },
  "Results": [...],
  "FailedEndpoints": [...]
}
```

**Localização**: `./relatorio-teste-master-YYYYMMDD-HHmmss.json`

## 🎨 Saída no Console

```
============================================================================
 🚀 TESTE MASTER COMPLETO - POC PDPW 100%
============================================================================

🎯 [1/17] API DASHBOARD
────────────────────────────────────────────────────────────────────────
  ✅ GET Resumo do Dashboard
  ✅ GET Métricas de Ofertas
  ✅ GET Alertas do Sistema

🎯 [2/17] API USINAS
────────────────────────────────────────────────────────────────────────
  ✅ GET Listar todas as usinas
  ✅ GET Buscar usina por ID=1
  ✅ POST Criar nova usina
  ✅ PUT Atualizar usina criada
  
...

============================================================================
 📊 RELATÓRIO FINAL - TESTE MASTER COMPLETO
============================================================================

📈 ESTATÍSTICAS GERAIS
────────────────────────────────────────────────────────────────────────
Total de Testes:       200
Testes Passaram:       195 ✅
Testes Falharam:       5 ❌
Taxa de Sucesso:       97.5%
Duração Total:         02:15

🎯 APIs TESTADAS (17)
────────────────────────────────────────────────────────────────────────
  01. Dashboard ✅
  02. Usinas ✅
  ...
  17. Dados Energéticos ✅

🎉 PARABÉNS! TODOS OS TESTES PASSARAM COM 100% DE SUCESSO!
    POC PDPw está 100% funcional e pronta para apresentação!
```

## 🔧 Pré-requisitos

1. ✅ Docker containers rodando (`docker-compose up -d`)
2. ✅ API disponível em `http://localhost:5001`
3. ✅ PowerShell 5.1 ou superior
4. ✅ Seed data aplicado no banco

## 💡 Dicas de Uso

### **Para CI/CD**
```powershell
$result = .\scripts\TESTE-MASTER-COMPLETO.ps1
if ($LASTEXITCODE -ne 0) {
    Write-Error "Testes falharam!"
    exit 1
}
```

### **Testar Apenas Leitura (sem POST/PUT/DELETE)**
Comente as seções de POST/PUT/DELETE no script.

### **Validar Antes de Deploy**
```powershell
# 1. Subir containers
docker-compose up -d

# 2. Aguardar API inicializar
Start-Sleep -Seconds 30

# 3. Executar testes
.\scripts\TESTE-MASTER-COMPLETO.ps1 -StopOnError

# 4. Se passar, fazer deploy
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Tudo OK! Pode fazer deploy!"
}
```

## 📈 Métricas de Sucesso

| Métrica | Valor Esperado | Ação se Falhar |
|---------|----------------|----------------|
| Taxa de Sucesso | ≥ 95% | Revisar logs de erro |
| Endpoints GET | 100% | Verificar banco de dados |
| Endpoints POST | ≥ 90% | Validar DTOs e regras |
| Endpoints PUT | ≥ 90% | Verificar mapeamentos |
| Tempo Total | < 5 min | Otimizar queries |

## 🐛 Troubleshooting

### **Erro: Connection refused**
```
Solução: Verificar se containers estão rodando
docker ps
docker-compose up -d
```

### **Erro: 400 Bad Request em POST**
```
Solução: Verificar validações de DTO e AutoMapper
- Campos obrigatórios
- Formatos de data
- Foreign keys válidas
```

### **Erro: Timeout**
```
Solução: Aumentar timeout no script
$params.TimeoutSec = 30
```

## 📝 Exemplos de Recursos Criados

### **Usina**
```json
{
  "codigo": "TESTE-UHE-180530",
  "nome": "Usina Teste Master Script",
  "tipoUsinaId": 1,
  "empresaId": 1,
  "capacidadeInstalada": 500.00,
  "localizacao": "Local Teste"
}
```

### **Usuário**
```json
{
  "nome": "Usuario Teste Master 180530",
  "email": "usuario.teste.180530@ons.org.br",
  "telefone": "(21) 3444-5555",
  "equipePDPId": 1,
  "perfil": "Operador"
}
```

### **Oferta Exportação**
```json
{
  "usinaId": 2,
  "dataPDP": "2024-12-27",
  "valorMW": 150.5,
  "precoMWh": 250.75,
  "observacoes": "Oferta teste script master"
}
```

## 🎯 Casos de Uso

### **1. Validação Pré-Release**
Executar antes de criar tag de versão para garantir que não há regressões.

### **2. Testes de Smoke**
Validar que sistema está funcionando após deploy em novo ambiente.

### **3. Demonstração ao Cliente**
Mostrar que todas as APIs estão funcionais durante apresentação.

### **4. Debugging de Issues**
Identificar rapidamente quais endpoints estão com problema.

### **5. Documentação Viva**
Script serve como exemplo de uso de todas as APIs.

## 🔗 Integrações

### **Swagger UI**
Após executar testes, acesse: http://localhost:5001/swagger

### **Relatório HTML**
Converter JSON para HTML:
```powershell
$json = Get-Content .\relatorio-teste-master-*.json | ConvertFrom-Json
# Processar e gerar HTML
```

### **CI/CD Pipeline**
```yaml
test:
  script:
    - docker-compose up -d
    - pwsh .\scripts\TESTE-MASTER-COMPLETO.ps1
  artifacts:
    paths:
      - relatorio-teste-master-*.json
```

## 📚 Documentação Relacionada

- [GUIA_TESTES_SWAGGER.md](../docs/GUIA_TESTES_SWAGGER.md)
- [ROTEIRO_VALIDACAO_DOCKER_SWAGGER.md](../docs/ROTEIRO_VALIDACAO_DOCKER_SWAGGER.md)
- [RELATORIO_FINAL_100_PORCENTO.md](../docs/RELATORIO_FINAL_100_PORCENTO.md)

## 👥 Autores

- **Willian Bulhões** - Product Owner
- **GitHub Copilot** - AI Assistant

## 📅 Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0.0 | 26/12/2024 | Versão inicial - Grand Finale da POC |

## 🎉 Conclusão

Este script representa o **TESTE MAIS COMPLETO** da POC PDPw, validando **TODAS as funcionalidades** implementadas. É a **chave de ouro** para garantir que o sistema está **100% funcional e pronto para apresentação ao ONS**!

---

**Status**: ✅ **PRONTO PARA USO**  
**Última Atualização**: 26/12/2024  
**Licença**: Proprietário - ONS

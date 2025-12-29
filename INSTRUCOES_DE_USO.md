# 🚀 INSTRUÇÕES DE USO - Frontend PDPw

## ✅ PASSO A PASSO PARA EXECUTAR

### OPÇÃO 1: Setup Automático (Recomendado)

#### Windows:
```cmd
# Execute o script de setup
.\setup-frontend.bat
```

#### Linux/Mac:
```bash
# Dar permissão de execução
chmod +x setup-frontend.sh

# Execute o script
./setup-frontend.sh
```

O script irá:
1. Verificar se Node.js está instalado
2. Instalar todas as dependências (npm install)
3. Criar arquivo `.env` a partir do `.env.example`
4. Verificar erros de TypeScript

---

### OPÇÃO 2: Setup Manual

#### 1. Instalar Dependências

```bash
# Navegar para o diretório frontend
cd frontend

# Instalar dependências do npm
npm install
```

#### 2. Configurar Variáveis de Ambiente

```bash
# Copiar arquivo de exemplo
cp .env.example .env

# Ou manualmente criar .env com:
VITE_API_URL=http://localhost:5001/api
VITE_ENV=development
VITE_ENABLE_DEBUG=true
VITE_ENABLE_MOCK_DATA=false
```

#### 3. Verificar TypeScript (opcional)

```bash
npm run type-check
```

---

## 🏃 EXECUTAR O SISTEMA

### 1. Iniciar Backend (.NET 8)

**Terminal 1:**
```bash
cd src/PDPW.API
dotnet run
```

✅ Backend rodando em: http://localhost:5001  
✅ Swagger disponível em: http://localhost:5001/swagger

### 2. Iniciar Frontend (React)

**Terminal 2:**
```bash
cd frontend
npm run dev
```

✅ Frontend rodando em: http://localhost:5173

---

## 🧪 TESTAR O SISTEMA

### 1. Acessar Dashboard
```
http://localhost:5173
```

Você verá:
- Cards com métricas do sistema
- Workflow das 9 etapas
- Menu lateral com navegação

### 2. Testar Dados Energéticos
1. Click em "1. Dados Energéticos" no menu
2. Preencha o formulário:
   - Data Referência: Hoje
   - Código Usina: ITB001
   - Produção: 14000
   - Capacidade: 14000
   - Status: PLANEJADO
3. Click em "Salvar"
4. Veja o registro na tabela abaixo

### 3. Testar Programação Elétrica
1. Click em "2. Programação Elétrica"
2. Selecione uma Semana PMO
3. Na aba "Cargas":
   - Subsistema: SE
   - Data: Hoje
   - Carga Média: 50000
4. Click em "Adicionar Carga"
5. Navegue pelas abas (Cargas, Intercâmbios, Balanços)

### 4. Testar Previsão Eólica
1. Click em "3. Previsão Eólica"
2. Selecione um Parque Eólico
3. Preencha a previsão
4. Veja o cálculo automático do fator de capacidade

### 5. Testar Geração de Arquivos
1. Click em "4. Geração de Arquivos"
2. Selecione uma Semana PMO
3. Click em "Gerar Novo Arquivo DADGER"
4. Aguarde a geração
5. Teste: Download, Aprovar, Rejeitar

---

## 📊 DADOS DISPONÍVEIS

O backend já possui **857 registros** prontos:

### Usinas Disponíveis
- **ITB001** - Itaipu Binacional (14.000 MW)
- **BLM001** - Belo Monte (11.233 MW)
- **TCR001** - Tucuruí (8.370 MW)
- **ITU001** - Itumbiara (2.082 MW)
- **TRS001** - Três Marias (396 MW)

### Semanas PMO
- **108 semanas** cadastradas (2024-2026)
- Use as primeiras semanas para testes

### Subsistemas
- **SE** - Sudeste
- **S** - Sul
- **NE** - Nordeste
- **N** - Norte

---

## 🔧 COMANDOS ÚTEIS

### Frontend

```bash
# Desenvolvimento
npm run dev              # Iniciar dev server (porta 5173)
npm run build            # Build de produção
npm run preview          # Preview do build
npm run type-check       # Verificar tipos TypeScript
npm run lint             # Verificar código

# Limpeza
npm run clean            # Limpar arquivos
rm -rf node_modules && npm install  # Reinstalar
```

### Backend

```bash
# Desenvolvimento
dotnet run               # Iniciar API (porta 5001)
dotnet build             # Compilar
dotnet test              # Executar testes

# Banco de Dados
dotnet ef database update  # Aplicar migrations
dotnet ef migrations add NomeMigracao  # Criar migration
```

---

## 🐛 SOLUÇÃO DE PROBLEMAS

### Erro: "Module not found"
```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
```

### Erro: "CORS"
1. Verificar se backend está rodando (http://localhost:5001)
2. Verificar CORS no `Program.cs` do backend
3. Conferir `VITE_API_URL` no arquivo `.env`

### Erro: "Port 5173 already in use"
```bash
# Matar processo
npx kill-port 5173

# Ou usar outra porta
npm run dev -- --port 3000
```

### Backend não responde
```bash
# Verificar saúde
curl http://localhost:5001/health

# Resposta esperada: "Healthy"
```

### Dados não carregam
1. Verificar se backend está rodando
2. Abrir DevTools (F12) → Console
3. Verificar erros de rede (Network tab)
4. Testar endpoint diretamente no Swagger

---

## 📱 NAVEGAÇÃO DO SISTEMA

### Menu Lateral

**Principal**
- 🏠 Dashboard

**Programação**
- ⚡ 1. Dados Energéticos
- 🔌 2. Programação Elétrica
- 💨 3. Previsão Eólica
- 📁 4. Geração de Arquivos

**Workflow**
- ✅ 5. Finalização (em desenvolvimento)

**Recebimentos**
- 📥 6. Insumos Agentes (em desenvolvimento)
- 🔥 7. Ofertas Térmicas (em desenvolvimento)
- 📉 8. Ofertas RV (em desenvolvimento)
- 💧 9. Energia Vertida (em desenvolvimento)

---

## 🎯 FLUXO DE TRABALHO TÍPICO

### Criar uma Programação Completa

1. **Dashboard** - Verificar métricas
2. **Dados Energéticos** - Cadastrar produção das usinas
3. **Programação Elétrica**:
   - Adicionar Cargas dos subsistemas
   - Configurar Intercâmbios
   - Verificar Balanços
4. **Previsão Eólica** - Adicionar previsões de parques eólicos
5. **Geração de Arquivos**:
   - Gerar arquivo DADGER
   - Revisar dados
   - Aprovar arquivo
   - Fazer download
6. **Finalização** - (próxima etapa) Publicar programação

---

## 📚 DOCUMENTAÇÃO ADICIONAL

### Arquivos de Referência
- **`frontend/README.md`** - Documentação técnica completa
- **`frontend/GUIA_RAPIDO.md`** - Quick start guide
- **`frontend/ESTRUTURA_COMPLETA.md`** - Visão end-to-end
- **`RESUMO_FRONTEND_COMPLETO.md`** - Resumo executivo

### APIs (Swagger)
http://localhost:5001/swagger

### Exemplos de Código
Veja os arquivos em `frontend/src/pages/` para exemplos de:
- Formulários
- Tabelas
- Chamadas de API
- Gestão de estado
- Validações

---

## ✅ CHECKLIST DE VALIDAÇÃO

Antes de considerar concluído, verificar:

- [ ] Frontend rodando em http://localhost:5173
- [ ] Backend rodando em http://localhost:5001
- [ ] Dashboard carrega métricas
- [ ] Consegue criar Dados Energéticos
- [ ] Consegue adicionar Cargas
- [ ] Consegue cadastrar Previsão Eólica
- [ ] Consegue gerar Arquivo DADGER
- [ ] Consegue aprovar arquivo
- [ ] Consegue fazer download
- [ ] Menu lateral funciona
- [ ] Responsividade em mobile funciona

---

## 🎉 TUDO PRONTO!

Se todos os passos acima funcionaram, você tem:

✅ Frontend React + TypeScript funcionando  
✅ Backend .NET 8 integrado  
✅ 4 etapas completamente funcionais  
✅ 857 registros de teste disponíveis  
✅ Sistema end-to-end operacional  

### Próximos Passos:
1. Testar todas as funcionalidades
2. Implementar etapas 5-9
3. Adicionar testes automatizados
4. Preparar para deploy

---

## 📞 SUPORTE

### Dúvidas?
1. Consulte a documentação (`README.md`)
2. Verifique o Swagger (`/swagger`)
3. Abra issue no GitHub
4. Contate a equipe

---

**PDPw v2.0** - Sistema pronto para uso!  
**Data:** Janeiro 2025  
**Status:** ✅ Funcional (Etapas 1-4)

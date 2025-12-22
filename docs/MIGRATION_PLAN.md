# ?? Plano de Migra��o PDPW

## ?? Projeto: PDPW - Programa��o Di�ria da Produ��o Web

### Informa��es do Projeto

| Item | Detalhes |
|------|----------|
| **Reposit�rio Legado** | https://github.com/ONSArquitetura/pdpw_act |
| **Reposit�rio Novo** | https://github.com/wbulhoes/ONS_PoC-PDPW |
| **Status** | PoC - Prova de Conceito |
| **Cliente** | ONS (Operador Nacional do Sistema El�trico) |

---

## ?? Cen�rio Atual (Legado)

### Tecnologias do Sistema Legado

| Tecnologia | Vers�o/Tipo | Status |
|------------|-------------|--------|
| Framework | .NET Framework | ? Obsoleto |
| Interface | WebForms | ? Obsoleto |
| Linguagem | VB.NET | ? Obsoleto |
| Arquitetura | Monol�tica | ? Antiga |
| Containeriza��o | Nenhuma | ? |

**Problemas identificados:**
- ? Stack tecnol�gica considerada antiga pelo ONS
- ? Manuten��o dif�cil
- ? Baixa produtividade
- ? Integra��o complicada
- ? Dif�cil de testar
- ? Sem suporte a containers

---

## ?? Objetivo da PoC

### Objetivos Principais

1. **Migrar a aplica��o** para stack moderna:
   - Backend: .NET 8
   - Frontend: React (�ltima vers�o)
   - Containeriza��o: Windows Container (compatibilidade com legado)
   - Arquitetura: Clean Architecture (modular e atualizada)

2. **Manter fidelidade m�xima**:
   - ? Funcionalidades preservadas
   - ? UI modernizada mas familiar
   - ? Experi�ncia de usu�rio aprimorada

3. **Migra��o flex�vel**:
   - Vertical slice: fluxos completos end-to-end
   - Horizontal slice: camadas espec�ficas (backend/frontend separados)

---

## ?? Entreg�veis da PoC

### 1. C�digo Migrado
- ? Reposit�rio GitHub atualizado
- ? Commits organizados e documentados
- ? Hist�rico de migra��o rastre�vel

### 2. Documenta��o
- ? README completo
- ? Guias de setup
- ? Troubleshooting
- ? Documenta��o de APIs (Swagger)
- ? Decis�es arquiteturais (ADRs)

### 3. Apresenta��o
- Demonstra��o da solu��o
- Comparativo legado vs novo
- ROI e benef�cios

### 4. Automa��o e IA
- Uso de ferramentas de acelera��o
- GitHub Copilot
- Automa��o de testes

---

## ??? Arquitetura Proposta

### Stack Tecnol�gica Nova

```
???????????????????????????????????????????
?           Frontend (React)              ?
?    - React 18+ (�ltima vers�o)          ?
?    - TypeScript                         ?
?    - Vite/Next.js                       ?
?    - Material-UI/Ant Design             ?
???????????????????????????????????????????
                    ? REST API
???????????????????????????????????????????
?         Backend (.NET 8)                ?
?    ???????????????????????????????      ?
?    ?   PDPW.API                  ?      ?
?    ?   (Controllers, Swagger)    ?      ?
?    ???????????????????????????????      ?
?    ???????????????????????????????      ?
?    ?   PDPW.Application          ?      ?
?    ?   (Services, DTOs)          ?      ?
?    ???????????????????????????????      ?
?    ???????????????????????????????      ?
?    ?   PDPW.Domain               ?      ?
?    ?   (Entities, Business)      ?      ?
?    ???????????????????????????????      ?
?    ???????????????????????????????      ?
?    ?   PDPW.Infrastructure       ?      ?
?    ?   (Data Access, EF Core)    ?      ?
?    ???????????????????????????????      ?
???????????????????????????????????????????
                    ?
???????????????????????????????????????????
?           Banco de Dados                ?
?    - SQL Server / LocalDB               ?
?    - Entity Framework Core 8            ?
???????????????????????????????????????????
```

### Containeriza��o

```dockerfile
# Windows Container (compatibilidade com legado)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022

# React App pode ser servido via nginx ou IIS
```

---

## ?? An�lise do Sistema Legado

### Funcionalidades Principais (a identificar)

Com base na descri��o, o PDPW realiza:

1. **Coleta de dados energ�ticos**
   - Entrada manual/autom�tica
   - Valida��es de dados
   - Armazenamento

2. **Envio de insumos para modelos matem�ticos**
   - Processamento de dados
   - Formata��o para modelos
   - Integra��o com sistemas externos

3. **Apoio a previs�es e fluxos de produ��o**
   - C�lculos e simula��es
   - Gera��o de relat�rios
   - Visualiza��o de dados

### Fluxos Cr�ticos (a mapear)

```
???????????????      ???????????????      ???????????????
?   Entrada   ? ???> ? Processamento? ???> ?   Sa�da     ?
?   de Dados  ?      ?   e Valida��o?      ? (Modelos)   ?
???????????????      ???????????????      ???????????????
```

---

## ??? Plano de Migra��o

### Fase 1: An�lise e Prepara��o (1-2 semanas)

#### 1.1. An�lise do C�digo Legado
- [ ] Clonar reposit�rio legado
- [ ] Mapear estrutura de pastas
- [ ] Identificar WebForms (.aspx)
- [ ] Mapear Models (VB.NET)
- [ ] Identificar DAL (Data Access Layer)
- [ ] Mapear regras de neg�cio
- [ ] Documentar fluxos principais

#### 1.2. Mapeamento de Banco de Dados
- [ ] Extrair schema do banco
- [ ] Identificar tabelas principais
- [ ] Mapear relacionamentos
- [ ] Documentar stored procedures
- [ ] Identificar dados de migra��o

#### 1.3. Defini��o de Prioridades
- [ ] Selecionar fluxo para vertical slice
- [ ] Definir camadas para horizontal slice
- [ ] Estabelecer crit�rios de sucesso

### Fase 2: Setup Inicial (1 semana)

#### 2.1. Backend (.NET 8)
- [x] Criar solution .NET 8
- [x] Configurar Clean Architecture
- [x] Setup Entity Framework Core
- [x] Configurar Swagger
- [ ] Setup de testes unit�rios
- [ ] Configurar CI/CD b�sico

#### 2.2. Frontend (React)
- [ ] Inicializar projeto React
- [ ] Configurar TypeScript
- [ ] Setup de roteamento
- [ ] Configurar biblioteca de componentes
- [ ] Setup de testes (Jest/React Testing Library)

#### 2.3. Infraestrutura
- [ ] Dockerfile para backend
- [ ] Dockerfile para frontend
- [ ] docker-compose.yml
- [ ] Scripts de deploy

### Fase 3: Migra��o - Vertical Slice (2-3 semanas)

**Fluxo Selecionado:** [A definir - ex: Cadastro de Dados Energ�ticos]

#### 3.1. Backend
- [ ] Criar entidades de dom�nio
- [ ] Implementar reposit�rios
- [ ] Desenvolver services
- [ ] Criar DTOs
- [ ] Implementar controllers
- [ ] Adicionar valida��es
- [ ] Testes unit�rios

#### 3.2. Frontend
- [ ] Criar p�ginas/componentes
- [ ] Implementar formul�rios
- [ ] Integrar com API
- [ ] Valida��es no cliente
- [ ] Feedback visual
- [ ] Testes de componentes

#### 3.3. Integra��o
- [ ] Testes de integra��o E2E
- [ ] Valida��o com usu�rios
- [ ] Ajustes de UX

### Fase 4: Migra��o - Horizontal Slice (2-3 semanas)

#### 4.1. Migra��o de Mais Entidades
- [ ] Mapear entidades restantes
- [ ] Implementar CRUD completo
- [ ] APIs documentadas
- [ ] Testes de unidade

#### 4.2. Frontend Adicional
- [ ] Telas secund�rias
- [ ] Dashboards
- [ ] Relat�rios
- [ ] Componentes reutiliz�veis

### Fase 5: Refinamento e Otimiza��o (1-2 semanas)

#### 5.1. Qualidade de C�digo
- [ ] Code review completo
- [ ] Refatora��o
- [ ] Otimiza��es de performance
- [ ] Seguran�a (OWASP Top 10)

#### 5.2. Documenta��o
- [ ] README completo
- [ ] Swagger/OpenAPI atualizado
- [ ] Guias de setup
- [ ] Troubleshooting
- [ ] ADRs (Architecture Decision Records)

#### 5.3. Deploy e Containeriza��o
- [ ] Build de containers
- [ ] Testes de container
- [ ] Pipeline CI/CD
- [ ] Deploy em ambiente de homologa��o

### Fase 6: Apresenta��o (1 semana)

#### 6.1. Prepara��o
- [ ] Slides da apresenta��o
- [ ] Demonstra��o ao vivo
- [ ] Comparativo legado vs novo
- [ ] ROI e benef�cios

#### 6.2. Entrega
- [ ] Apresenta��o ao cliente
- [ ] Coleta de feedback
- [ ] Documenta��o final
- [ ] Handover (se aplic�vel)

---

## ?? Estrat�gias de Migra��o

### Op��o 1: Vertical Slice (Recomendado para PoC)

**Vantagens:**
- ? Entrega r�pida de valor
- ? Feedback r�pido do cliente
- ? Demonstra��o de fluxo completo
- ? Menos risco

**Exemplo de Fluxo:**
```
Cadastro de Dados Energ�ticos
??? Backend
?   ??? API REST
?   ??? Valida��es
?   ??? Persist�ncia
?   ??? L�gica de neg�cio
??? Frontend
    ??? Formul�rio de entrada
    ??? Valida��es
    ??? Feedback visual
    ??? Integra��o com API
```

### Op��o 2: Horizontal Slice

**Vantagens:**
- ? Migra��o completa de uma camada
- ? Pode ser dividido entre equipes
- ? Backend independente de frontend

**Exemplo:**
```
Backend Completo
??? Todas as APIs
??? Todas as entidades
??? Todos os services
??? Testes completos

Frontend B�sico (MVP)
??? Telas principais
??? Navega��o b�sica
??? Integra��o com APIs principais
```

---

## ?? Processo de Convers�o

### De VB.NET para C# (.NET 8)

#### Exemplo de Convers�o

**Legado (VB.NET):**
```vb
Public Class DadoEnergetico
    Public Property Id As Integer
    Public Property Data As DateTime
    Public Property Valor As Decimal
    
    Public Function Validar() As Boolean
        Return Valor > 0
    End Function
End Class
```

**Novo (C# .NET 8):**
```csharp
public class DadoEnergetico
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    
    public bool Validar()
    {
        return Valor > 0;
    }
}
```

### De WebForms para React

#### Exemplo de Convers�o

**Legado (WebForms .aspx):**
```aspx
<asp:GridView ID="GridView1" runat="server" />
<asp:Button ID="BtnSalvar" runat="server" Text="Salvar" />
```

**Novo (React + TypeScript):**
```tsx
import { Table, Button } from 'antd';

const DadosEnergeticosPage: React.FC = () => {
  const [dados, setDados] = useState([]);
  
  const handleSalvar = async () => {
    await api.post('/dadosenergeticos', dados);
  };
  
  return (
    <>
      <Table dataSource={dados} />
      <Button onClick={handleSalvar}>Salvar</Button>
    </>
  );
};
```

---

## ??? Ferramentas e Tecnologias

### Backend

| Ferramenta | Vers�o | Uso |
|------------|--------|-----|
| .NET SDK | 8.0 | Runtime e compila��o |
| Entity Framework Core | 8.0 | ORM |
| SQL Server | 2019+ / LocalDB | Banco de dados |
| Swagger/OpenAPI | 3.0 | Documenta��o de API |
| xUnit | �ltima | Testes unit�rios |
| FluentValidation | �ltima | Valida��es |

### Frontend

| Ferramenta | Vers�o | Uso |
|------------|--------|-----|
| React | 18+ | Framework UI |
| TypeScript | 5.x | Linguagem |
| Vite/Next.js | �ltima | Build/SSR |
| Ant Design / Material-UI | �ltima | Componentes |
| Axios | �ltima | HTTP Client |
| React Query | �ltima | State management |
| Jest + Testing Library | �ltima | Testes |

### DevOps

| Ferramenta | Uso |
|------------|-----|
| Docker | Containeriza��o |
| GitHub Actions | CI/CD |
| Azure DevOps | (Opcional) Pipeline |
| SonarQube | Qualidade de c�digo |

### Ferramentas de IA e Acelera��o

| Ferramenta | Uso |
|------------|-----|
| GitHub Copilot | Assist�ncia de c�digo |
| ChatGPT | Consultoria e revis�o |
| Tabnine | Autocomplete inteligente |

---

## ? Crit�rios de Sucesso

### T�cnicos

- [ ] C�digo compilando sem erros
- [ ] Testes unit�rios > 70% cobertura
- [ ] Testes de integra��o passando
- [ ] Performance similar ou melhor que legado
- [ ] Sem regress�es de funcionalidade
- [ ] Documenta��o completa
- [ ] Swagger 100% documentado

### Neg�cio

- [ ] Funcionalidades principais migradas
- [ ] UI moderna e responsiva
- [ ] Experi�ncia de usu�rio aprovada
- [ ] Feedback do cliente positivo
- [ ] Demonstra��o bem-sucedida

### Qualidade

- [ ] Clean Code
- [ ] SOLID principles
- [ ] Seguran�a (OWASP)
- [ ] Acessibilidade (WCAG)
- [ ] Performance otimizada

---

## ?? Cronograma Estimado

| Fase | Dura��o | Marco |
|------|---------|-------|
| An�lise e Prepara��o | 1-2 semanas | Setup completo |
| Setup Inicial | 1 semana | Projetos criados |
| Vertical Slice | 2-3 semanas | Fluxo completo |
| Horizontal Slice | 2-3 semanas | APIs completas |
| Refinamento | 1-2 semanas | Qualidade |
| Apresenta��o | 1 semana | Entrega |
| **TOTAL** | **8-12 semanas** | **PoC Conclu�da** |

---

## ?? Pr�ximos Passos

### Imediatos (Esta Semana)

1. **An�lise do Reposit�rio Legado**
   ```bash
   git clone https://github.com/ONSArquitetura/pdpw_act
   cd pdpw_act
   # Explorar estrutura
   ```

2. **Mapear Funcionalidades**
   - Documentar fluxos principais
   - Identificar telas cr�ticas
   - Mapear modelos de dados

3. **Definir Prioridades**
   - Escolher fluxo para vertical slice
   - Validar com cliente/stakeholder

### Curto Prazo (Pr�ximas 2 Semanas)

1. **Iniciar Frontend React**
   ```bash
   npx create-react-app pdpw-frontend --template typescript
   # ou
   npx create-next-app@latest pdpw-frontend --typescript
   ```

2. **Implementar Primeiro Fluxo**
   - Backend API
   - Frontend componentes
   - Integra��o

3. **Configurar CI/CD B�sico**
   - GitHub Actions
   - Build autom�tico
   - Testes autom�ticos

---

## ?? Recursos e Refer�ncias

### Documenta��o Oficial

- [.NET 8 Documentation](https://learn.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)

### Guias de Migra��o

- [Migrating from .NET Framework to .NET 8](https://learn.microsoft.com/dotnet/core/porting/)
- [WebForms to Blazor/React](https://learn.microsoft.com/aspnet/core/migration/)

### Clean Architecture

- [Microsoft Clean Architecture](https://github.com/dotnet-architecture/eShopOnWeb)
- [Jason Taylor Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

---

## ?? Considera��es de Seguran�a

### Backend

- [ ] Autentica��o JWT
- [ ] Autoriza��o baseada em roles
- [ ] Valida��o de entrada
- [ ] Prote��o contra SQL Injection (EF Core)
- [ ] Rate limiting
- [ ] CORS configurado corretamente
- [ ] HTTPS obrigat�rio

### Frontend

- [ ] XSS protection
- [ ] CSRF tokens
- [ ] Sanitiza��o de inputs
- [ ] Valida��o no cliente
- [ ] Secrets n�o expostos

---

## ?? Contatos e Suporte

| Papel | Nome | Contato |
|-------|------|---------|
| Desenvolvedor | Willian Charantola Bulhoes | [GitHub](https://github.com/wbulhoes) |
| Cliente | ONS | [A definir] |
| Stakeholder | [A definir] | [A definir] |

---

## ?? Notas Importantes

### Decis�es Arquiteturais

1. **Clean Architecture**: Escolhida para garantir manutenibilidade e testabilidade
2. **Entity Framework Core**: Para facilitar migra��es e manuten��o
3. **React + TypeScript**: Stack moderna e demandada pelo mercado
4. **Windows Container**: Compatibilidade com sistemas legados do ONS

### Riscos Identificados

| Risco | Impacto | Mitiga��o |
|-------|---------|-----------|
| Complexidade do legado | Alto | An�lise detalhada pr�via |
| Integra��o com sistemas externos | M�dio | Mapeamento de APIs |
| Curva de aprendizado React | Baixo | Documenta��o e treinamento |
| Performance | M�dio | Testes de carga |

---

**�ltima atualiza��o:** 17/12/2025  
**Vers�o:** 1.0  
**Status:** ?? Pronto para Iniciar

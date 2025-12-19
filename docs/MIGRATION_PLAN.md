# ?? Plano de Migração PDPW

## ?? Projeto: PDPW - Programação Diária da Produção Web

### Informações do Projeto

| Item | Detalhes |
|------|----------|
| **Repositório Legado** | https://github.com/ONSArquitetura/pdpw_act |
| **Repositório Novo** | https://github.com/wbulhoes/ONS_PoC-PDPW |
| **Status** | PoC - Prova de Conceito |
| **Cliente** | ONS (Operador Nacional do Sistema Elétrico) |

---

## ?? Cenário Atual (Legado)

### Tecnologias do Sistema Legado

| Tecnologia | Versão/Tipo | Status |
|------------|-------------|--------|
| Framework | .NET Framework | ? Obsoleto |
| Interface | WebForms | ? Obsoleto |
| Linguagem | VB.NET | ? Obsoleto |
| Arquitetura | Monolítica | ? Antiga |
| Containerização | Nenhuma | ? |

**Problemas identificados:**
- ? Stack tecnológica considerada antiga pelo ONS
- ? Manutenção difícil
- ? Baixa produtividade
- ? Integração complicada
- ? Difícil de testar
- ? Sem suporte a containers

---

## ?? Objetivo da PoC

### Objetivos Principais

1. **Migrar a aplicação** para stack moderna:
   - Backend: .NET 8
   - Frontend: React (última versão)
   - Containerização: Windows Container (compatibilidade com legado)
   - Arquitetura: Clean Architecture (modular e atualizada)

2. **Manter fidelidade máxima**:
   - ? Funcionalidades preservadas
   - ? UI modernizada mas familiar
   - ? Experiência de usuário aprimorada

3. **Migração flexível**:
   - Vertical slice: fluxos completos end-to-end
   - Horizontal slice: camadas específicas (backend/frontend separados)

---

## ?? Entregáveis da PoC

### 1. Código Migrado
- ? Repositório GitHub atualizado
- ? Commits organizados e documentados
- ? Histórico de migração rastreável

### 2. Documentação
- ? README completo
- ? Guias de setup
- ? Troubleshooting
- ? Documentação de APIs (Swagger)
- ? Decisões arquiteturais (ADRs)

### 3. Apresentação
- Demonstração da solução
- Comparativo legado vs novo
- ROI e benefícios

### 4. Automação e IA
- Uso de ferramentas de aceleração
- GitHub Copilot
- Automação de testes

---

## ??? Arquitetura Proposta

### Stack Tecnológica Nova

```
???????????????????????????????????????????
?           Frontend (React)              ?
?    - React 18+ (última versão)          ?
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

### Containerização

```dockerfile
# Windows Container (compatibilidade com legado)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022

# React App pode ser servido via nginx ou IIS
```

---

## ?? Análise do Sistema Legado

### Funcionalidades Principais (a identificar)

Com base na descrição, o PDPW realiza:

1. **Coleta de dados energéticos**
   - Entrada manual/automática
   - Validações de dados
   - Armazenamento

2. **Envio de insumos para modelos matemáticos**
   - Processamento de dados
   - Formatação para modelos
   - Integração com sistemas externos

3. **Apoio a previsões e fluxos de produção**
   - Cálculos e simulações
   - Geração de relatórios
   - Visualização de dados

### Fluxos Críticos (a mapear)

```
???????????????      ???????????????      ???????????????
?   Entrada   ? ???> ? Processamento? ???> ?   Saída     ?
?   de Dados  ?      ?   e Validação?      ? (Modelos)   ?
???????????????      ???????????????      ???????????????
```

---

## ??? Plano de Migração

### Fase 1: Análise e Preparação (1-2 semanas)

#### 1.1. Análise do Código Legado
- [ ] Clonar repositório legado
- [ ] Mapear estrutura de pastas
- [ ] Identificar WebForms (.aspx)
- [ ] Mapear Models (VB.NET)
- [ ] Identificar DAL (Data Access Layer)
- [ ] Mapear regras de negócio
- [ ] Documentar fluxos principais

#### 1.2. Mapeamento de Banco de Dados
- [ ] Extrair schema do banco
- [ ] Identificar tabelas principais
- [ ] Mapear relacionamentos
- [ ] Documentar stored procedures
- [ ] Identificar dados de migração

#### 1.3. Definição de Prioridades
- [ ] Selecionar fluxo para vertical slice
- [ ] Definir camadas para horizontal slice
- [ ] Estabelecer critérios de sucesso

### Fase 2: Setup Inicial (1 semana)

#### 2.1. Backend (.NET 8)
- [x] Criar solution .NET 8
- [x] Configurar Clean Architecture
- [x] Setup Entity Framework Core
- [x] Configurar Swagger
- [ ] Setup de testes unitários
- [ ] Configurar CI/CD básico

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

### Fase 3: Migração - Vertical Slice (2-3 semanas)

**Fluxo Selecionado:** [A definir - ex: Cadastro de Dados Energéticos]

#### 3.1. Backend
- [ ] Criar entidades de domínio
- [ ] Implementar repositórios
- [ ] Desenvolver services
- [ ] Criar DTOs
- [ ] Implementar controllers
- [ ] Adicionar validações
- [ ] Testes unitários

#### 3.2. Frontend
- [ ] Criar páginas/componentes
- [ ] Implementar formulários
- [ ] Integrar com API
- [ ] Validações no cliente
- [ ] Feedback visual
- [ ] Testes de componentes

#### 3.3. Integração
- [ ] Testes de integração E2E
- [ ] Validação com usuários
- [ ] Ajustes de UX

### Fase 4: Migração - Horizontal Slice (2-3 semanas)

#### 4.1. Migração de Mais Entidades
- [ ] Mapear entidades restantes
- [ ] Implementar CRUD completo
- [ ] APIs documentadas
- [ ] Testes de unidade

#### 4.2. Frontend Adicional
- [ ] Telas secundárias
- [ ] Dashboards
- [ ] Relatórios
- [ ] Componentes reutilizáveis

### Fase 5: Refinamento e Otimização (1-2 semanas)

#### 5.1. Qualidade de Código
- [ ] Code review completo
- [ ] Refatoração
- [ ] Otimizações de performance
- [ ] Segurança (OWASP Top 10)

#### 5.2. Documentação
- [ ] README completo
- [ ] Swagger/OpenAPI atualizado
- [ ] Guias de setup
- [ ] Troubleshooting
- [ ] ADRs (Architecture Decision Records)

#### 5.3. Deploy e Containerização
- [ ] Build de containers
- [ ] Testes de container
- [ ] Pipeline CI/CD
- [ ] Deploy em ambiente de homologação

### Fase 6: Apresentação (1 semana)

#### 6.1. Preparação
- [ ] Slides da apresentação
- [ ] Demonstração ao vivo
- [ ] Comparativo legado vs novo
- [ ] ROI e benefícios

#### 6.2. Entrega
- [ ] Apresentação ao cliente
- [ ] Coleta de feedback
- [ ] Documentação final
- [ ] Handover (se aplicável)

---

## ?? Estratégias de Migração

### Opção 1: Vertical Slice (Recomendado para PoC)

**Vantagens:**
- ? Entrega rápida de valor
- ? Feedback rápido do cliente
- ? Demonstração de fluxo completo
- ? Menos risco

**Exemplo de Fluxo:**
```
Cadastro de Dados Energéticos
??? Backend
?   ??? API REST
?   ??? Validações
?   ??? Persistência
?   ??? Lógica de negócio
??? Frontend
    ??? Formulário de entrada
    ??? Validações
    ??? Feedback visual
    ??? Integração com API
```

### Opção 2: Horizontal Slice

**Vantagens:**
- ? Migração completa de uma camada
- ? Pode ser dividido entre equipes
- ? Backend independente de frontend

**Exemplo:**
```
Backend Completo
??? Todas as APIs
??? Todas as entidades
??? Todos os services
??? Testes completos

Frontend Básico (MVP)
??? Telas principais
??? Navegação básica
??? Integração com APIs principais
```

---

## ?? Processo de Conversão

### De VB.NET para C# (.NET 8)

#### Exemplo de Conversão

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

#### Exemplo de Conversão

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

| Ferramenta | Versão | Uso |
|------------|--------|-----|
| .NET SDK | 8.0 | Runtime e compilação |
| Entity Framework Core | 8.0 | ORM |
| SQL Server | 2019+ / LocalDB | Banco de dados |
| Swagger/OpenAPI | 3.0 | Documentação de API |
| xUnit | Última | Testes unitários |
| FluentValidation | Última | Validações |

### Frontend

| Ferramenta | Versão | Uso |
|------------|--------|-----|
| React | 18+ | Framework UI |
| TypeScript | 5.x | Linguagem |
| Vite/Next.js | Última | Build/SSR |
| Ant Design / Material-UI | Última | Componentes |
| Axios | Última | HTTP Client |
| React Query | Última | State management |
| Jest + Testing Library | Última | Testes |

### DevOps

| Ferramenta | Uso |
|------------|-----|
| Docker | Containerização |
| GitHub Actions | CI/CD |
| Azure DevOps | (Opcional) Pipeline |
| SonarQube | Qualidade de código |

### Ferramentas de IA e Aceleração

| Ferramenta | Uso |
|------------|-----|
| GitHub Copilot | Assistência de código |
| ChatGPT | Consultoria e revisão |
| Tabnine | Autocomplete inteligente |

---

## ? Critérios de Sucesso

### Técnicos

- [ ] Código compilando sem erros
- [ ] Testes unitários > 70% cobertura
- [ ] Testes de integração passando
- [ ] Performance similar ou melhor que legado
- [ ] Sem regressões de funcionalidade
- [ ] Documentação completa
- [ ] Swagger 100% documentado

### Negócio

- [ ] Funcionalidades principais migradas
- [ ] UI moderna e responsiva
- [ ] Experiência de usuário aprovada
- [ ] Feedback do cliente positivo
- [ ] Demonstração bem-sucedida

### Qualidade

- [ ] Clean Code
- [ ] SOLID principles
- [ ] Segurança (OWASP)
- [ ] Acessibilidade (WCAG)
- [ ] Performance otimizada

---

## ?? Cronograma Estimado

| Fase | Duração | Marco |
|------|---------|-------|
| Análise e Preparação | 1-2 semanas | Setup completo |
| Setup Inicial | 1 semana | Projetos criados |
| Vertical Slice | 2-3 semanas | Fluxo completo |
| Horizontal Slice | 2-3 semanas | APIs completas |
| Refinamento | 1-2 semanas | Qualidade |
| Apresentação | 1 semana | Entrega |
| **TOTAL** | **8-12 semanas** | **PoC Concluída** |

---

## ?? Próximos Passos

### Imediatos (Esta Semana)

1. **Análise do Repositório Legado**
   ```bash
   git clone https://github.com/ONSArquitetura/pdpw_act
   cd pdpw_act
   # Explorar estrutura
   ```

2. **Mapear Funcionalidades**
   - Documentar fluxos principais
   - Identificar telas críticas
   - Mapear modelos de dados

3. **Definir Prioridades**
   - Escolher fluxo para vertical slice
   - Validar com cliente/stakeholder

### Curto Prazo (Próximas 2 Semanas)

1. **Iniciar Frontend React**
   ```bash
   npx create-react-app pdpw-frontend --template typescript
   # ou
   npx create-next-app@latest pdpw-frontend --typescript
   ```

2. **Implementar Primeiro Fluxo**
   - Backend API
   - Frontend componentes
   - Integração

3. **Configurar CI/CD Básico**
   - GitHub Actions
   - Build automático
   - Testes automáticos

---

## ?? Recursos e Referências

### Documentação Oficial

- [.NET 8 Documentation](https://learn.microsoft.com/dotnet/)
- [React Documentation](https://react.dev/)
- [TypeScript Handbook](https://www.typescriptlang.org/docs/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)

### Guias de Migração

- [Migrating from .NET Framework to .NET 8](https://learn.microsoft.com/dotnet/core/porting/)
- [WebForms to Blazor/React](https://learn.microsoft.com/aspnet/core/migration/)

### Clean Architecture

- [Microsoft Clean Architecture](https://github.com/dotnet-architecture/eShopOnWeb)
- [Jason Taylor Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

---

## ?? Considerações de Segurança

### Backend

- [ ] Autenticação JWT
- [ ] Autorização baseada em roles
- [ ] Validação de entrada
- [ ] Proteção contra SQL Injection (EF Core)
- [ ] Rate limiting
- [ ] CORS configurado corretamente
- [ ] HTTPS obrigatório

### Frontend

- [ ] XSS protection
- [ ] CSRF tokens
- [ ] Sanitização de inputs
- [ ] Validação no cliente
- [ ] Secrets não expostos

---

## ?? Contatos e Suporte

| Papel | Nome | Contato |
|-------|------|---------|
| Desenvolvedor | Willian Charantola Bulhoes | [GitHub](https://github.com/wbulhoes) |
| Cliente | ONS | [A definir] |
| Stakeholder | [A definir] | [A definir] |

---

## ?? Notas Importantes

### Decisões Arquiteturais

1. **Clean Architecture**: Escolhida para garantir manutenibilidade e testabilidade
2. **Entity Framework Core**: Para facilitar migrações e manutenção
3. **React + TypeScript**: Stack moderna e demandada pelo mercado
4. **Windows Container**: Compatibilidade com sistemas legados do ONS

### Riscos Identificados

| Risco | Impacto | Mitigação |
|-------|---------|-----------|
| Complexidade do legado | Alto | Análise detalhada prévia |
| Integração com sistemas externos | Médio | Mapeamento de APIs |
| Curva de aprendizado React | Baixo | Documentação e treinamento |
| Performance | Médio | Testes de carga |

---

**Última atualização:** 17/12/2025  
**Versão:** 1.0  
**Status:** ?? Pronto para Iniciar

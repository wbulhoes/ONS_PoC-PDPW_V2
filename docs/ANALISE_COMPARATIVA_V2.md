# ?? ANÁLISE COMPARATIVA - POC PDPW

**Data**: 19/12/2024  
**Repositório Referência**: https://github.com/RafaelSuzanoACT/POCMigracaoPDPw  
**Repositório Atual**: https://github.com/wbulhoes/ONS_PoC-PDPW

---

## ?? VISÃO GERAL

### Repositório do Rafael Suzano (Referência)

**Características Principais**:
- ? Migração de .NET Framework 4.8/VB.NET para .NET 8/C#
- ? Estrutura modular com Clean Architecture
- ? Frontend React + TypeScript
- ? Pasta `legado/` com código VB.NET original
- ? Testes unitários (xUnit para backend, Jest para frontend)
- ? Docker Compose configurado
- ? Documentação estruturada (AGENTS.md, STRUCTURE.md, etc.)
- ? Linguagem Ubíqua do domínio PDP bem definida

**Estrutura de Pastas**:
```
POCMigracaoPDPw/
??? .cursor/                    # Configurações Cursor AI
??? .github/                    # GitHub Actions e Copilot Instructions
??? docs/                       # Documentação
??? frontend/                   # React + TypeScript
?   ??? src/
?   ?   ??? components/
?   ?   ??? pages/
?   ?   ??? services/
?   ??? tests/
??? legado/                     # Código VB.NET original (referência)
??? src/
?   ??? Web.Api/               # Controllers ASP.NET Core
?   ??? Application/           # Services
?   ??? Domain/                # Entidades e interfaces
?   ??? Infrastructure/        # Repositórios e EF Core
??? tests/
?   ??? UnitTests/             # Testes xUnit
??? AGENTS.md                  # Documentação para IA
??? CONTRIBUTING.md            # Guia de contribuição
??? QUICKSTART.md              # Guia rápido
??? STRUCTURE.md               # Estrutura do projeto
??? docker-compose.yml
```

### Nossa POC Atual

**Características**:
- ? .NET 8 com Clean Architecture
- ? 5 APIs completas (39 endpoints)
- ? Entity Framework Core
- ? Seed data funcional
- ? Swagger documentado
- ? Documentação técnica
- ? Frontend React ainda não iniciado
- ? Testes unitários não implementados
- ? Docker não configurado
- ? Pasta legado não incluída

**Estrutura Atual**:
```
ONS_PoC-PDPW/
??? docs/                      # Documentação
??? frontend/                  # React (básico, não desenvolvido)
??? pdpw_act/                  # Código VB.NET original + Backup
??? scripts/                   # Scripts PowerShell
??? src/
?   ??? PDPW.API/             # Controllers
?   ??? PDPW.Application/     # Services + DTOs
?   ??? PDPW.Domain/          # Entidades
?   ??? PDPW.Infrastructure/  # Repositórios + EF Core
??? SETUP.md
??? README.md
```

---

## ?? PONTOS FORTES DE CADA ABORDAGEM

### ? Referência (Rafael Suzano)

1. **Documentação Estruturada**:
   - AGENTS.md (para IA)
   - STRUCTURE.md (arquitetura)
   - CONTRIBUTING.md (guia de contribuição)
   - QUICKSTART.md (início rápido)

2. **Linguagem Ubíqua**:
   - Termos do domínio bem definidos
   - Nomenclatura consistente
   - Vocabulário do setor elétrico

3. **Infraestrutura**:
   - Docker Compose completo
   - GitHub Actions (CI/CD)
   - Ambiente de desenvolvimento padronizado

4. **Testes**:
   - Testes unitários backend (xUnit)
   - Testes frontend (Jest)
   - Cobertura de código

5. **Frontend**:
   - React + TypeScript
   - Componentes estruturados
   - Services para API calls

6. **Código Legado**:
   - Pasta `legado/` organizada
   - Referência para migração
   - VB.NET original preservado

### ? Nossa POC (Willian)

1. **APIs Funcionais**:
   - 5 APIs completas (39 endpoints)
   - Validações robustas
   - Padrões consistentes

2. **Clean Architecture**:
   - Separação de camadas bem definida
   - Repository Pattern
   - Service Layer

3. **Seed Data**:
   - Dados realistas
   - Relacionamentos corretos
   - Pronto para testes

4. **Documentação Técnica**:
   - Relatórios detalhados
   - Análise do backup
   - Scripts de migração

5. **Swagger**:
   - Documentação automática
   - Testável via UI
   - Schemas bem definidos

6. **Scripts PowerShell**:
   - Automação de restauração
   - Análise de backup
   - Extração seletiva

---

## ?? MELHORIAS IDENTIFICADAS

### ?? CRÍTICAS (Implementar em V2)

1. **Testes Unitários**:
   ```
   PROBLEMA: Sem testes automatizados
   SOLUÇÃO: Criar projeto tests/UnitTests com xUnit
   PRIORIDADE: Alta
   IMPACTO: Qualidade e confiabilidade
   ```

2. **Docker Compose**:
   ```
   PROBLEMA: Sem configuração Docker
   SOLUÇÃO: docker-compose.yml com backend + frontend + SQL Server
   PRIORIDADE: Alta
   IMPACTO: Facilita setup e deploy
   ```

3. **Frontend React**:
   ```
   PROBLEMA: Frontend não desenvolvido
   SOLUÇÃO: Implementar React + TypeScript com estrutura similar à referência
   PRIORIDADE: Alta
   IMPACTO: Usuário final não tem interface
   ```

### ?? IMPORTANTES (Implementar em V2)

4. **Documentação Estruturada**:
   ```
   PROBLEMA: Documentação dispersa
   SOLUÇÃO: AGENTS.md, STRUCTURE.md, CONTRIBUTING.md, QUICKSTART.md
   PRIORIDADE: Média
   IMPACTO: Facilita contribuição e manutenção
   ```

5. **Linguagem Ubíqua**:
   ```
   PROBLEMA: Alguns nomes genéricos (ex: "Usina" vs "UsinaGeradora")
   SOLUÇÃO: Padronizar termos do domínio PDP
   PRIORIDADE: Média
   IMPACTO: Comunicação com stakeholders
   ```

6. **GitHub Actions**:
   ```
   PROBLEMA: Sem CI/CD
   SOLUÇÃO: .github/workflows/ com build, test, deploy
   PRIORIDADE: Média
   IMPACTO: Automação e qualidade
   ```

### ?? DESEJÁVEIS (Futuro)

7. **Copilot Instructions**:
   ```
   PROBLEMA: Sem instruções para IA
   SOLUÇÃO: .github/copilot-instructions.md
   PRIORIDADE: Baixa
   IMPACTO: Melhora assistência IA
   ```

8. **Organização da Pasta Legado**:
   ```
   PROBLEMA: pdpw_act/ mistura código e backup
   SOLUÇÃO: Separar em legado/ e backups/
   PRIORIDADE: Baixa
   IMPACTO: Organização
   ```

---

## ?? PLANO DE AÇÃO V2

### Fase 1: INFRAESTRUTURA (Prioridade Alta)

#### 1.1 Docker Compose
```yaml
# docker-compose.yml
version: '3.8'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Pdpw@2024!
    ports:
      - "1433:1433"
    volumes:
      - sqldata:/var/opt/mssql

  backend:
    build:
      context: .
      dockerfile: src/PDPW.API/Dockerfile
    ports:
      - "5000:80"
    depends_on:
      - sqlserver
    environment:
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=PDPW_PoC;...

  frontend:
    build:
      context: ./frontend
      dockerfile: Dockerfile
    ports:
      - "3000:80"
    depends_on:
      - backend

volumes:
  sqldata:
```

#### 1.2 Testes Unitários
```csharp
// tests/PDPW.UnitTests/Services/UsinaServiceTests.cs
public class UsinaServiceTests
{
    [Fact]
    public async Task GetByIdAsync_DeveRetornarUsina_QuandoExiste()
    {
        // Arrange
        var mockRepo = new Mock<IUsinaRepository>();
        var mockMapper = new Mock<IMapper>();
        // ...

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UHE-ITAIPU", result.Codigo);
    }
}
```

### Fase 2: DOCUMENTAÇÃO (Prioridade Média)

#### 2.1 AGENTS.md
```markdown
# Documentação para Agentes IA

## Linguagem Ubíqua do Domínio PDP

### Entidades Principais
- **ProgramacaoEnergetica**: Planejamento de geração
- **UsinaGeradora**: Instalação de geração de energia
- **AgenteSetorEletrico**: Empresa do setor elétrico
- **SemanaPMO**: Semana operativa do PMO
- ...

### Termos do Domínio
- **PMO**: Programa Mensal de Operação
- **DESSEM**: Modelo de despacho hidrotérmico
- **ONS**: Operador Nacional do Sistema
- ...
```

#### 2.2 STRUCTURE.md
```markdown
# Estrutura do Projeto

## Camadas da Aplicação

### API Layer (PDPW.API)
- Controllers
- Middlewares
- Filters
- Swagger

### Application Layer (PDPW.Application)
- Services
- DTOs
- Validators
- Mappings

### Domain Layer (PDPW.Domain)
- Entities
- Interfaces
- Value Objects

### Infrastructure Layer (PDPW.Infrastructure)
- Repositories
- DbContext
- Migrations
- Seed Data
```

### Fase 3: FRONTEND (Prioridade Alta)

#### 3.1 Estrutura React
```typescript
// frontend/src/services/usinaService.ts
export class UsinaService {
  async getAll(): Promise<UsinaDto[]> {
    const response = await api.get('/api/usinas');
    return response.data;
  }

  async getById(id: number): Promise<UsinaDto> {
    const response = await api.get(`/api/usinas/${id}`);
    return response.data;
  }
}

// frontend/src/components/Usina/UsinaList.tsx
export const UsinaList: React.FC = () => {
  const [usinas, setUsinas] = useState<UsinaDto[]>([]);

  useEffect(() => {
    const service = new UsinaService();
    service.getAll().then(setUsinas);
  }, []);

  return (
    <div className="usina-list">
      {usinas.map(usina => (
        <UsinaCard key={usina.id} usina={usina} />
      ))}
    </div>
  );
};
```

### Fase 4: CI/CD (Prioridade Média)

#### 4.1 GitHub Actions
```yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main, develop ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

---

## ?? ESTRUTURA V2 PROPOSTA

```
ONS_PoC-PDPW_V2/
??? .cursor/
?   ??? rules.txt                      # Regras Cursor AI
??? .github/
?   ??? copilot-instructions.md        # Instruções GitHub Copilot
?   ??? workflows/
?       ??? ci.yml                     # CI/CD Pipeline
?       ??? deploy.yml                 # Deploy automatizado
??? docs/
?   ??? architecture/                  # Diagramas arquitetura
?   ??? api/                          # Documentação APIs
?   ??? domain/                       # Modelos de domínio
?   ??? migration/                    # Guias de migração
??? frontend/
?   ??? public/
?   ??? src/
?   ?   ??? components/               # Componentes React
?   ?   ??? pages/                    # Páginas
?   ?   ??? services/                 # API Services
?   ?   ??? hooks/                    # Custom Hooks
?   ?   ??? contexts/                 # React Contexts
?   ?   ??? types/                    # TypeScript types
?   ??? tests/                        # Jest tests
?   ??? Dockerfile
?   ??? package.json
??? legado/
?   ??? pdpw_vb/                      # Código VB.NET original
?   ??? documentacao/                 # Docs do legado
??? backups/
?   ??? Backup_PDP_TST.bak           # Backups do cliente
??? scripts/
?   ??? migration/                    # Scripts de migração
?   ??? deployment/                   # Scripts de deploy
?   ??? analysis/                     # Scripts de análise
??? src/
?   ??? PDPW.API/
?   ?   ??? Controllers/
?   ?   ??? Middlewares/
?   ?   ??? Filters/
?   ?   ??? Extensions/
?   ?   ??? Dockerfile
?   ?   ??? Program.cs
?   ??? PDPW.Application/
?   ?   ??? Services/
?   ?   ??? DTOs/
?   ?   ??? Validators/
?   ?   ??? Mappings/
?   ?   ??? Interfaces/
?   ??? PDPW.Domain/
?   ?   ??? Entities/
?   ?   ??? Interfaces/
?   ?   ??? ValueObjects/
?   ?   ??? Specifications/
?   ??? PDPW.Infrastructure/
?       ??? Data/
?       ?   ??? Configurations/
?       ?   ??? Migrations/
?       ?   ??? Seed/
?       ??? Repositories/
?       ??? Services/
??? tests/
?   ??? PDPW.UnitTests/              # xUnit tests
?   ?   ??? Services/
?   ?   ??? Repositories/
?   ?   ??? Controllers/
?   ??? PDPW.IntegrationTests/       # Integration tests
?   ??? PDPW.E2ETests/               # End-to-end tests
??? .editorconfig
??? .gitignore
??? AGENTS.md                        # Documentação para IA
??? CONTRIBUTING.md                  # Guia de contribuição
??? QUICKSTART.md                    # Início rápido
??? STRUCTURE.md                     # Estrutura do projeto
??? README.md
??? docker-compose.yml
??? ONS_PoC-PDPW.sln
```

---

## ?? ESTRATÉGIA DE MIGRAÇÃO PARA V2

### Abordagem Recomendada: CÓPIA INCREMENTAL

```powershell
# 1. Criar pasta V2
New-Item -Path "C:\temp\_ONS_PoC-PDPW_V2" -ItemType Directory

# 2. Copiar estrutura atual
Copy-Item -Path "C:\temp\_ONS_PoC-PDPW\*" `
          -Destination "C:\temp\_ONS_PoC-PDPW_V2" `
          -Recurse -Exclude @(".git", "bin", "obj", "node_modules")

# 3. Inicializar novo repositório
cd C:\temp\_ONS_PoC-PDPW_V2
git init
git checkout -b develop

# 4. Aplicar melhorias incrementalmente
# - Fase 1: Docker
# - Fase 2: Testes
# - Fase 3: Frontend
# - Fase 4: CI/CD
```

### Checklist de Migração

- [ ] Criar pasta V2
- [ ] Copiar código existente
- [ ] Reorganizar estrutura de pastas
- [ ] Adicionar Docker Compose
- [ ] Criar projeto de testes unitários
- [ ] Implementar primeiros testes
- [ ] Estruturar frontend React
- [ ] Criar documentação (AGENTS.md, STRUCTURE.md, etc.)
- [ ] Configurar GitHub Actions
- [ ] Adicionar Copilot Instructions
- [ ] Reorganizar pasta legado
- [ ] Mover backup para pasta separada
- [ ] Atualizar README.md
- [ ] Testar ambiente Docker
- [ ] Build e validação final

---

## ?? COMPARAÇÃO FINAL

| Aspecto | V1 (Atual) | V2 (Proposta) | Referência (Rafael) |
|---------|------------|---------------|---------------------|
| **APIs Backend** | ? 5 APIs (39 endpoints) | ? 5 APIs + melhorias | ? Estrutura completa |
| **Testes** | ? Nenhum | ? xUnit + Jest | ? xUnit + Jest |
| **Docker** | ? Não configurado | ? Docker Compose | ? Docker Compose |
| **Frontend** | ? Básico | ? React + TypeScript | ? React + TypeScript |
| **CI/CD** | ? Nenhum | ? GitHub Actions | ? GitHub Actions |
| **Documentação** | ? Técnica | ? Estruturada | ? Estruturada |
| **Legado** | ?? Misturado | ? Organizado | ? Pasta dedicada |

**Legenda**:
- ? = Implementado
- ?? = Parcial
- ? = Não implementado
- ? = Referência de qualidade

---

## ?? RECOMENDAÇÃO FINAL

### CRIAR V2 COM MELHORIAS INCREMENTAIS

**Justificativa**:
1. ? Preserva todo trabalho atual (V1 intacto)
2. ? Permite comparação lado a lado
3. ? Facilita rollback se necessário
4. ? Incorpora melhores práticas da referência
5. ? Mantém APIs funcionais + adiciona testes, Docker, frontend

**Próximos Passos**:
1. ? Criar pasta V2
2. ? Copiar estrutura atual
3. ? Aplicar melhorias por fase
4. ? Testar cada fase antes de avançar
5. ? Documentar mudanças

**Tempo Estimado**:
- Fase 1 (Docker): 2-3h
- Fase 2 (Testes): 4-6h
- Fase 3 (Frontend): 8-12h
- Fase 4 (CI/CD): 2-3h
- **Total**: ~16-24h

---

**Analista**: GitHub Copilot  
**Data**: 19/12/2024  
**Status**: ? Análise Completa - Aguardando Decisão

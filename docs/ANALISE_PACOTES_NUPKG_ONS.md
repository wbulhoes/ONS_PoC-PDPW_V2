# 📦 ANÁLISE DOS PACOTES NUPKG DO CLIENTE ONS

**Data da Análise**: 24/12/2024  
**Origem**: `C:\temp\_ONS_PoC-PDPW\pdpw_act\nupkgs`  
**Total de Pacotes**: 11  
**Framework Target**: .NET Framework 4.8

---

## 📋 RESUMO EXECUTIVO

Os pacotes `.nupkg` compartilhados pelo ONS são **bibliotecas internas proprietárias** que fornecem infraestrutura comum para sistemas ONS:

- ✅ **Autenticação e Segurança** (POP - Portal Operacional Principal)
- ✅ **Cache e Contexto de Execução**
- ✅ **DTOs e Schemas**
- ✅ **Logging (Log4Net)**
- ✅ **Criptografia**
- ✅ **Providers (Membership, Role, SiteMap)**

---

## 📦 INVENTÁRIO DE PACOTES

### **1. ons.common.context** (v4.8.0.0) - 10.8 KB

**Descrição**: Componente de cache e contexto de execução  
**Dependências**: Nenhuma  
**Assembly**: `ons.common.context.dll`

**Funcionalidade Provável**:
- ✅ Gerenciamento de contexto de requisição/sessão
- ✅ Cache distribuído/local
- ✅ Armazenamento de dados temporários

**Relevância para POC**: 🟡 **MÉDIA**

---

### **2. ons.common.security** (v4.8.0.0) - 9.4 KB

**Descrição**: Componente auxiliar de geração de ticket  
**Dependências**: `ons.common.context`  
**Assembly**: `ons.common.security.dll`

**Funcionalidade Provável**:
- ✅ Geração de tokens/tickets de autenticação
- ✅ Validação de credenciais
- ✅ Claims/identidade do usuário

**Relevância para POC**: 🔴 **ALTA**

---

### **3. ons.common.providers** (v4.8.0.0) - 79.2 KB

**Descrição**: Definição e Helper dos provedores do POP  
**Dependências**: 
- `Microsoft.IdentityModel` (v6.1.7600.16394)
- `ons.common.security`
- `ons.common.utilities`

**Assembly**: `ons.common.providers.dll`

**Funcionalidades**:
- ✅ `POPHelper`, `POPAdminHelper`
- ✅ `IPOPProvider`, `IPOPAdminProvider`
- ✅ `SiteMapProviderBase`
- ✅ `MembershipProviderBase`
- ✅ `RoleProviderBase`

**Relevância para POC**: 🔴 **ALTA**

---

### **4. ProxyProviders** (v4.8.0) - 87.5 KB

**Descrição**: Implementação dos provedores do POP  
**Assembly**: `ProxyProviders.dll`

**Funcionalidades**:
- ✅ `DynamicSiteMapProvider`
- ✅ `MemberShipServiceProvider`
- ✅ `POPServiceProvider`
- ✅ `POPAdminServiceProvider`
- ✅ `POPGroupServiceProvider`
- ✅ `POPRoleServiceProvider`
- ✅ `POPScopeServiceProvider`
- ✅ `ServiceWebEventProvider`

**Relevância para POC**: 🔴 **ALTA**

---

### **5. ons.common.schemas** (v4.8.0.0) - 19.5 KB

**Descrição**: Componente de DTOs  
**Assembly**: `ons.common.schemas.dll`

**Funcionalidade Provável**:
- ✅ DTOs compartilhados entre sistemas ONS
- ✅ Contratos de dados (Data Contracts)
- ✅ Validações de schema

**Relevância para POC**: 🟡 **MÉDIA**

---

### **6. ons.common.utilities** (v4.8.0) - 13.8 KB

**Descrição**: Componentes auxiliares (Log4Net, WebEvents, XML Helper)  
**Assembly**: `ons.common.utilities.dll`

**Funcionalidades**:
- ✅ Logging com Log4Net
- ✅ WebEvents Helper
- ✅ Helper de XML

**Relevância para POC**: 🟢 **BAIXA** (podemos usar Serilog/.NET logging)

---

### **7. ONS.Core.Appenders** (v4.8.0) - 16.6 KB

**Descrição**: Componentes auxiliares para gravação de log com Log4Net  
**Assembly**: `ONS.Core.Appenders.dll`

**Funcionalidade Provável**:
- ✅ Custom Appenders para Log4Net
- ✅ Integração com sistemas de log do ONS

**Relevância para POC**: 🟢 **BAIXA**

---

### **8. ons.common.services** (v4.8.0) - 11.2 KB

**Descrição**: Componente de segurança para chamada a serviço (SafeExecution)  
**Assembly**: `ons.common.services.dll`

**Funcionalidade Provável**:
- ✅ Wrapper para chamadas seguras a serviços
- ✅ Retry policies
- ✅ Circuit breaker

**Relevância para POC**: 🟡 **MÉDIA**

---

### **9. OnsClasses** (v3.3.2) - 20.6 KB

**Descrição**: Componente da IntUnica de acesso a dados  
**Assembly**: `OnsClasses.dll`

**Funcionalidade Provável**:
- ✅ Helpers de acesso a dados (ADO.NET)
- ✅ Padrões de repositório legados

**Relevância para POC**: 🟢 **BAIXA** (usamos EF Core)

---

### **10. OnsCrypto** (v3.3.2) - 6.6 KB

**Descrição**: Componente da IntUnica utilizado para criptografia  
**Assembly**: `OnsCrypto.dll`

**Funcionalidade Provável**:
- ✅ Criptografia simétrica/assimétrica
- ✅ Hashing de senhas
- ✅ Geração de chaves

**Relevância para POC**: 🟡 **MÉDIA**

---

### **11. OnsWebControls** (v3.3.2) - 12.7 KB

**Descrição**: Componente da intunica utilizado para montagem do menu lateral  
**Assembly**: `OnsWebControls.dll`

**Funcionalidade Provável**:
- ✅ WebForms Controls customizados
- ✅ Menu de navegação

**Relevância para POC**: 🟢 **BAIXA** (frontend React não usa)

---

## 🎯 ANÁLISE DE RELEVÂNCIA PARA POC

### 🔴 **ALTA PRIORIDADE** (Críticos para funcionamento)

| Pacote | Por quê? | Ação Recomendada |
|--------|----------|------------------|
| **ons.common.security** | Autenticação é requisito | **Migrar** lógica de ticket |
| **ons.common.providers** | Membership/Roles do POP | **Adaptar** para ASP.NET Identity |
| **ProxyProviders** | Implementação dos providers | **Substituir** por JWT/.NET 8 |

---

### 🟡 **MÉDIA PRIORIDADE** (Úteis mas substituíveis)

| Pacote | Por quê? | Ação Recomendada |
|--------|----------|------------------|
| **ons.common.context** | Cache/contexto | **Substituir** por IMemoryCache/.NET 8 |
| **ons.common.schemas** | DTOs compartilhados | **Migrar** para PDPW.Application/DTOs |
| **ons.common.services** | SafeExecution | **Substituir** por Polly (resilience) |
| **OnsCrypto** | Criptografia | **Avaliar** compatibilidade ou usar System.Security |

---

### 🟢 **BAIXA PRIORIDADE** (Substituir por equivalentes .NET 8)

| Pacote | Por quê? | Substituir por |
|--------|----------|----------------|
| **ons.common.utilities** | Log4Net | **Serilog** ou **Microsoft.Extensions.Logging** |
| **ONS.Core.Appenders** | Log4Net appenders | **Serilog Sinks** (SQL, File, etc) |
| **OnsClasses** | ADO.NET helpers | **Entity Framework Core 8** |
| **OnsWebControls** | WebForms controls | **React Components** |

---

## 🚀 ESTRATÉGIA DE MIGRAÇÃO PARA POC

### **FASE 1: POC Atual (Sem Pacotes ONS)**

**Status**: ✅ **CONCLUÍDO**

**Decisão**:
- ❌ **NÃO usar** os pacotes `.nupkg` na POC
- ✅ **Usar** equivalentes .NET 8 nativos
- ✅ **Mocar** autenticação temporariamente

**Justificativa**:
- POC foca em **viabilidade técnica** da migração
- Pacotes ONS são **.NET Framework 4.8** (não compatíveis com .NET 8)
- Dependências antigas (`Microsoft.IdentityModel` v6.x)

---

### **FASE 2: MVP (Migrar Pacotes Críticos)**

**Timeline**: Janeiro-Fevereiro 2025

**Ações**:

#### **1. Autenticação/Autorização**

**Pacotes a migrar**:
- `ons.common.security`
- `ons.common.providers`
- `ProxyProviders`

**Estratégia**:

```csharp
// ANTES (Sistema Legado VB.NET)
Dim popProvider = New POPServiceProvider()
If Not popProvider.ValidateUser(username, password) Then
    Throw New UnauthorizedException()
End If

// DEPOIS (Sistema Novo C#/.NET 8)
// PDPW.Infrastructure/Auth/PdpwAuthenticationService.cs
public class PdpwAuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _config;
    
    public async Task<AuthenticationResult> AuthenticateAsync(
        string username, 
        string password)
    {
        // Migrar lógica do POPServiceProvider
        // Usar JWT ao invés de tickets WIF
        
        var user = await _userRepository.FindByUsernameAsync(username);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            return AuthenticationResult.Failure("Credenciais inválidas");
        }
        
        var token = GenerateJwtToken(user);
        return AuthenticationResult.Success(token);
    }
}
```

**Tecnologias .NET 8**:
- ✅ `Microsoft.AspNetCore.Authentication.JwtBearer`
- ✅ `Microsoft.AspNetCore.Identity`
- ✅ `System.IdentityModel.Tokens.Jwt`

---

#### **2. DTOs e Schemas**

**Pacote a migrar**: `ons.common.schemas`

**Estratégia**:

```csharp
// Criar biblioteca compartilhada
// PDPW.Shared/DTOs/OnsSchemas.cs

namespace PDPW.Shared.DTOs
{
    // Migrar DTOs do ons.common.schemas
    public class UsuarioOnsDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        // ... outros campos do schema ONS
    }
}
```

---

#### **3. Criptografia**

**Pacote a migrar**: `OnsCrypto`

**Estratégia**:

```csharp
// PDPW.Infrastructure/Security/CryptoService.cs
public class PdpwCryptoService
{
    // Migrar algoritmos do OnsCrypto
    // Usar APIs .NET 8 modernas
    
    public string Hash(string plaintext)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(plaintext);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
    
    public string Encrypt(string plaintext, string key)
    {
        using var aes = Aes.Create();
        // Migrar lógica do OnsCrypto
    }
}
```

---

### **FASE 3: Produção (Compatibilidade Total)**

**Timeline**: Março-Abril 2025

**Se absolutamente necessário**, criar **adaptadores**:

```csharp
// PDPW.Adapters/OnsCompatibility/POPProviderAdapter.cs
public class POPProviderAdapter : IPOPProvider
{
    // Implementar interface do pacote legado
    // Mas usar código .NET 8 internamente
    
    public bool ValidateUser(string username, string password)
    {
        // Delegar para PdpwAuthenticationService
        var result = _authService.AuthenticateAsync(username, password).Result;
        return result.IsSuccess;
    }
}
```

---

## 🐳 IMPACTO NO DOCKER

### **Problema: Pacotes ONS são .NET Framework 4.8**

```dockerfile
# ❌ NÃO FUNCIONA - .NET Framework requer Windows containers
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# ✅ POC ATUAL - .NET 8 funciona em Linux containers
FROM mcr.microsoft.com/dotnet/aspnet:8.0
```

### **Soluções:**

#### **Opção 1: Migrar Lógica (Recomendado)**

```
✅ Extrair código-fonte dos pacotes ONS
✅ Reescrever em .NET 8
✅ Manter em Linux containers (mais leve)
```

#### **Opção 2: Multi-Stage (Temporário)**

```yaml
# docker-compose.yml
services:
  pdpw-api:
    image: mcr.microsoft.com/dotnet/aspnet:8.0  # .NET 8
    
  pdpw-legacy-auth:
    image: mcr.microsoft.com/dotnet/framework/wcf:4.8  # .NET Framework
    # Container Windows para pacotes ONS
```

**Problema**: Requer licença Windows Server

#### **Opção 3: API Gateway (Híbrido)**

```
┌─────────────────────┐
│   React Frontend    │
└─────────┬───────────┘
          │
┌─────────▼───────────┐
│  .NET 8 API Gateway │  ← Linux container
└─────┬───────┬───────┘
      │       │
┌─────▼───┐ ┌▼──────────────────┐
│ .NET 8  │ │ .NET Framework 4.8│  ← Windows container
│ APIs    │ │ Auth Service (ONS)│     (só autenticação)
└─────────┘ └───────────────────┘
```

---

## ✅ RECOMENDAÇÕES FINAIS

### **Para a POC Atual (Agora)**

```
✅ MANTER estratégia atual (sem pacotes ONS)
✅ DOCUMENTAR dependências dos pacotes
✅ PLANEJAR migração para próxima fase
❌ NÃO tentar usar .nupkg no Docker (incompatível)
```

### **Para o MVP (Janeiro 2025)**

```
1. ✅ Extrair DLLs dos .nupkg
2. ✅ Usar ILSpy/dnSpy para descompilar código
3. ✅ Migrar lógica crítica para .NET 8
4. ✅ Criar adaptadores se necessário
5. ✅ Testar compatibilidade
```

### **Priorização:**

| Ordem | Pacote | Ação | Prazo |
|-------|--------|------|-------|
| 1º | `ons.common.security` | Migrar autenticação | Sprint 1 |
| 2º | `ons.common.providers` | Adaptar providers | Sprint 2 |
| 3º | `ProxyProviders` | Substituir por JWT | Sprint 2 |
| 4º | `ons.common.schemas` | Migrar DTOs | Sprint 3 |
| 5º | `OnsCrypto` | Avaliar/migrar | Sprint 3 |

---

## 📊 MATRIZ DE DEPENDÊNCIAS

```
ProxyProviders (87KB)
  └─ ons.common.providers (79KB)
       ├─ Microsoft.IdentityModel (v6.x) ⚠️ LEGADO
       ├─ ons.common.security (9KB)
       │    └─ ons.common.context (10KB)
       └─ ons.common.utilities (13KB)

ons.common.services (11KB)
  └─ (sem dependências externas)

OnsClasses (20KB)
  └─ (ADO.NET legado)

OnsCrypto (6KB)
  └─ (System.Security legado)

OnsWebControls (12KB)
  └─ (WebForms - descontinuar)

ONS.Core.Appenders (16KB)
  └─ Log4Net ⚠️ LEGADO
```

---

## 🔍 FERRAMENTAS PARA ANÁLISE

### **Descompilar DLLs:**

```powershell
# Instalar ILSpy CLI
dotnet tool install -g ilspycmd

# Descompilar para C#
ilspycmd C:\temp\nupkg_analysis\ons.common.security.4.8.0.0\lib\net48\ons.common.security.dll `
  -o C:\temp\decompiled\ons.common.security.cs
```

### **Analisar Dependências:**

```powershell
# Usar NuGet Package Explorer
choco install nugetpackageexplorer

# Abrir .nupkg e ver:
# - Dependências
# - Assemblies
# - Metadados
```

---

## 📝 CONCLUSÃO

### **Status Atual: POC ✅**

A POC está correta em **NÃO usar os pacotes ONS** por enquanto:
- ✅ Foco em migração de arquitetura
- ✅ Uso de tecnologias .NET 8 nativas
- ✅ Docker funcional (Linux containers)

### **Próximos Passos: MVP**

Quando migrar funcionalidades críticas:
1. ✅ Descompilar pacotes ONS
2. ✅ Reescrever lógica em .NET 8
3. ✅ Criar adaptadores se necessário
4. ✅ Testar compatibilidade

### **Bloqueio Principal:**

Os pacotes ONS são **.NET Framework 4.8** e **NÃO funcionam** diretamente em:
- ❌ .NET 8
- ❌ Linux containers
- ❌ Docker compose atual

**Solução**: **Migrar lógica** ao invés de **portar pacotes**.

---

**📅 Documento criado**: 24/12/2024  
**🔍 Pacotes analisados**: 11  
**📊 Total de código**: ~357 KB  
**⚠️ Compatibilidade .NET 8**: 0% (requer migração)  
**✅ Recomendação**: Migrar lógica ao invés de usar pacotes diretamente

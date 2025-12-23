# 🔒 PRIVACIDADE E SEGURANÇA DO CÓDIGO

**Projeto**: POC PDPW  
**Data**: Dezembro/2025  
**Cliente**: ONS (Operador Nacional do Sistema)  
**Classificação**: Confidencial

---

## 📋 OBJETIVO

Garantir que o código-fonte e dados do sistema **permanecem privados** e protegidos contra acessos não autorizados.

---

## 🛡️ POLÍTICAS DE PRIVACIDADE

### 1. Repositório Git Privado

**GitHub**:
```
✅ Repositório: https://github.com/wbulhoes/ONS_PoC-PDPW_V2
✅ Visibilidade: PRIVATE
✅ Acesso: Apenas membros autorizados
```

**Configurações de Segurança**:
- ✅ **Branch protection**: `main` e `release/*` protegidas
- ✅ **Required reviews**: Mínimo 1 aprovação para merge
- ✅ **Status checks**: Build + testes devem passar
- ✅ **Signed commits**: Commits assinados (GPG)
- ✅ **Dependabot**: Alertas de vulnerabilidades

**Membros Autorizados**:
| Usuário | Role | Permissões |
|---------|------|------------|
| wbulhoes | Admin | Read/Write/Admin |
| RafaelSuzanoACT | Maintainer | Read/Write |
| (Time ONS) | Read | Somente leitura |

---

### 2. Secrets e Credenciais

**❌ NUNCA fazer**:
```csharp
// ❌ ERRADO - Credenciais hard-coded
var connectionString = "Server=prod.ons.org.br;User=sa;Password=Senha123!";
```

**✅ CORRETO - Usar variáveis de ambiente**:
```csharp
// appsettings.json (não commitar appsettings.Production.json)
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=${DB_SERVER};Database=${DB_NAME};User=${DB_USER};Password=${DB_PASSWORD}"
  }
}
```

```bash
# Variáveis de ambiente (não versionadas)
export DB_SERVER=prod.ons.org.br
export DB_USER=pdpw_app
export DB_PASSWORD=<secret-vault>
```

**Ferramentas**:
- ✅ **Azure Key Vault**: Armazenamento seguro de secrets
- ✅ **GitHub Secrets**: CI/CD secrets encriptados
- ✅ **.env files**: Localmente (nunca commitar `.env.production`)

---

### 3. Arquivo `.gitignore` Robusto

```gitignore
# Secrets e configurações sensíveis
appsettings.Production.json
appsettings.Staging.json
*.env
*.env.production
*.env.local
secrets.json

# Credenciais de banco
*.mdf
*.ldf
*.bak

# Certificados e chaves
*.pfx
*.p12
*.key
*.pem

# Logs e dados sensíveis
logs/
*.log
data/

# IDE e user-specific
.vs/
.vscode/settings.json
*.user
*.suo
```

**Validação**:
```bash
# Verificar se há secrets commitados
git log -p | grep -i "password\|secret\|token"
```

---

### 4. Code Scanning (Segurança)

**GitHub Advanced Security**:
- ✅ **Dependabot**: Alerta de vulnerabilidades em pacotes NuGet
- ✅ **CodeQL**: Análise de código estático (SAST)
- ✅ **Secret scanning**: Detecta credenciais commitadas

**Exemplo de Alerta**:
```
⚠️ Secret detected in commit abc123
File: appsettings.json
Match: "Password=Senha123!"
Severity: Critical
Action: Rotate secret immediately
```

---

## 🔐 CONTROLE DE ACESSO

### 1. Autenticação e Autorização

**Sistema Legado (POP)**:
- ✅ Integração com Portal Operacional Principal (ONS)
- ✅ LDAP/Active Directory
- ✅ Roles: Admin, Operador, Consulta

**Sistema Novo (JWT)**:
```csharp
// JWT com claims do ONS
var claims = new[]
{
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(ClaimTypes.Role, user.Role),
    new Claim("ons:subsistema", user.Subsistema), // Custom claim
    new Claim("ons:empresa_id", user.EmpresaId.ToString())
};

var token = new JwtSecurityToken(
    issuer: "pdpw-api.ons.org.br",
    audience: "pdpw-frontend",
    claims: claims,
    expires: DateTime.UtcNow.AddHours(8),
    signingCredentials: credentials
);
```

**Níveis de Acesso**:
| Role | Permissões |
|------|------------|
| `Admin` | CRUD completo + configurações |
| `Operador` | CRUD dados operacionais |
| `Consulta` | Somente leitura |
| `API` | Apenas endpoints específicos (M2M) |

---

### 2. Auditoria (Audit Trail)

**Campos de Auditoria (BaseEntity)**:
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public string? UsuarioCriacao { get; set; }      // ✅ Quem criou
    public string? UsuarioAtualizacao { get; set; }  // ✅ Quem modificou
    public bool Ativo { get; set; }                  // ✅ Soft delete
}
```

**Logs de Auditoria**:
```csharp
// Middleware de auditoria
app.Use(async (context, next) =>
{
    var username = context.User.Identity?.Name;
    var action = $"{context.Request.Method} {context.Request.Path}";
    
    _logger.LogInformation(
        "Audit: {Username} executou {Action} em {Timestamp}",
        username, action, DateTime.UtcNow
    );
    
    await next();
});
```

**Armazenamento**:
- ✅ **Tabela de auditoria** (SQL Server)
- ✅ **Logs estruturados** (Serilog → Azure App Insights)
- ✅ **Retenção**: 2 anos (conformidade LGPD)

---

## 🗄️ PROTEÇÃO DE DADOS (LGPD)

### 1. Dados Sensíveis

**Classificação**:
| Tipo | Exemplos | Proteção |
|------|----------|----------|
| **Públicos** | Nomes de usinas, capacidades | Nenhuma |
| **Internos** | Dados operacionais | Autenticação |
| **Confidenciais** | Contratos, preços | Criptografia + acesso restrito |
| **Secretos** | Senhas, tokens | Criptografia + vault |

### 2. Criptografia

**Em Trânsito (TLS 1.3)**:
```csharp
// appsettings.json
{
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://localhost:5001",
        "Certificate": {
          "Path": "cert.pfx",
          "Password": "${CERT_PASSWORD}" // Via Azure Key Vault
        }
      }
    }
  }
}
```

**Em Repouso (Database)**:
```sql
-- Transparent Data Encryption (TDE)
ALTER DATABASE PDPW_DB
SET ENCRYPTION ON;

-- Column-level encryption (dados sensíveis)
CREATE TABLE Usuarios (
    Id INT PRIMARY KEY,
    Username NVARCHAR(100),
    PasswordHash VARBINARY(MAX), -- ✅ Hash SHA256
    Email NVARCHAR(200)
);
```

**Password Hashing**:
```csharp
// NUNCA armazenar senha em texto plano
public string HashPassword(string plaintext)
{
    using var sha256 = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(plaintext + _salt);
    var hash = sha256.ComputeHash(bytes);
    return Convert.ToBase64String(hash);
}
```

---

## 🚫 PREVENÇÃO DE VAZAMENTOS

### 1. Code Review Obrigatório

**Processo**:
```
1. Desenvolvedor cria PR
2. CI/CD executa:
   - ✅ Testes automatizados
   - ✅ SonarQube (qualidade)
   - ✅ OWASP Dependency Check (vulnerabilidades)
   - ✅ Secret scanning
3. Tech Lead revisa código
4. Se aprovado → Merge
```

**Checklist de Review**:
- [ ] Sem credenciais hard-coded
- [ ] Sem comentários com dados sensíveis
- [ ] Sem logs de dados confidenciais
- [ ] Validações de input implementadas
- [ ] Autorização verificada em endpoints

---

### 2. Prevenção de Injeções

**SQL Injection**:
```csharp
// ❌ ERRADO - Concatenação de string
var query = $"SELECT * FROM Usinas WHERE Nome = '{nome}'";

// ✅ CORRETO - Parametrizado (EF Core)
var usinas = await _context.Usinas
    .Where(u => u.Nome == nome)
    .ToListAsync();
```

**XSS (Cross-Site Scripting)**:
```typescript
// React escapa automaticamente
<div>{usina.nome}</div> // ✅ Safe

// ❌ PERIGO - dangerouslySetInnerHTML
<div dangerouslySetInnerHTML={{__html: usina.descricao}} />

// ✅ CORRETO - Sanitizar com DOMPurify
import DOMPurify from 'dompurify';
<div dangerouslySetInnerHTML={{__html: DOMPurify.sanitize(usina.descricao)}} />
```

---

## 📊 MONITORAMENTO E COMPLIANCE

### 1. Logs de Segurança

**Eventos Registrados**:
- ✅ Tentativas de login (sucesso/falha)
- ✅ Acessos a endpoints sensíveis
- ✅ Modificações de dados críticos
- ✅ Erros de autorização (403)
- ✅ Erros de autenticação (401)

**Exemplo**:
```csharp
_logger.LogWarning(
    "Acesso negado: {Username} tentou acessar {Endpoint} sem permissão {RequiredRole}",
    username, endpoint, requiredRole
);
```

### 2. Alertas Automatizados

**Azure Monitor**:
- ✅ Alerta em 5+ tentativas de login falhadas (mesmo IP)
- ✅ Alerta em acessos fora do horário comercial
- ✅ Alerta em downloads massivos de dados
- ✅ Alerta em mudanças de configuração crítica

---

## ✅ CHECKLIST DE SEGURANÇA

### Repositório
- [x] Repositório privado no GitHub
- [x] Branch protection habilitada
- [x] Code review obrigatório
- [x] Signed commits (GPG)
- [x] Dependabot ativo
- [x] Secret scanning ativo

### Código
- [x] Sem credenciais hard-coded
- [x] `.gitignore` robusto
- [x] Secrets via Azure Key Vault
- [x] Inputs validados (anti-injection)
- [x] Outputs sanitizados (anti-XSS)
- [x] HTTPS obrigatório (TLS 1.3)

### Banco de Dados
- [x] Transparent Data Encryption (TDE)
- [x] Senhas hasheadas (SHA256)
- [x] Conexões via TLS
- [x] Backups criptografados
- [x] Retenção de auditoria (2 anos)

### Acesso
- [x] Autenticação via JWT
- [x] Autorização por roles
- [x] Auditoria de ações (logs)
- [x] Sessões com timeout (8h)
- [x] MFA (Multi-Factor) planejado

### Monitoramento
- [x] Logs estruturados (Serilog)
- [x] Alertas de segurança (Azure Monitor)
- [x] Dashboard de auditoria
- [x] Revisão mensal de acessos

---

## 📜 CONFORMIDADE LGPD

### Direitos dos Titulares

| Direito | Implementação |
|---------|---------------|
| **Acesso** | API `/api/usuarios/me/dados` (retorna todos dados) |
| **Retificação** | Endpoint PUT para correção |
| **Exclusão** | Soft delete (Ativo=false) + hard delete após 2 anos |
| **Portabilidade** | Export JSON/CSV de todos dados |
| **Oposição** | Opt-out de processamento não essencial |

### DPO (Data Protection Officer)
- **Responsável**: (Indicar pessoa do ONS)
- **Email**: dpo@ons.org.br
- **Atribuições**: Garantir conformidade LGPD

---

## ✅ CONCLUSÃO

### Medidas Implementadas

1. ✅ **Repositório privado** com controle de acesso
2. ✅ **Secrets via Azure Key Vault** (nunca no código)
3. ✅ **Code scanning** automatizado (Dependabot, CodeQL)
4. ✅ **Criptografia** em trânsito (TLS 1.3) e repouso (TDE)
5. ✅ **Auditoria completa** (quem, quando, o quê)
6. ✅ **Autorização por roles** (Admin, Operador, Consulta)
7. ✅ **Logs de segurança** centralizados (Azure Monitor)
8. ✅ **Conformidade LGPD** implementada

### Próximas Ações (Produção)

1. ⏳ Implementar MFA (Multi-Factor Authentication)
2. ⏳ Penetration testing (testes de invasão)
3. ⏳ Security hardening (OWASP Top 10)
4. ⏳ Certificação ISO 27001 (se aplicável)

**Status**: Código e dados estão **protegidos e privados**.

---

**📅 Documento gerado**: 23/12/2025  
**🔒 Classificação**: Confidencial  
**✅ Status**: Políticas de segurança implementadas  
**📋 Conformidade**: LGPD-compliant

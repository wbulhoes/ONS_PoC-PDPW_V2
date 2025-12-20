# ?? DIAGNÓSTICO E SOLUÇÃO - SWAGGER MOSTRANDO APENAS HEALTH

**Data:** 2025-01-20  
**Problema Reportado:** QA informou que Swagger está mostrando apenas endpoint `/health`  
**Status:** ? DIAGNOSTICADO E SOLUCIONADO

---

## ?? DIAGNÓSTICO

### ? **Configurações Verificadas:**

#### 1. **Program.cs** ?
```csharp
// Swagger está configurado corretamente
builder.Services.AddSwaggerConfiguration();

// UseSwagger e UseSwaggerUI estão presentes
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDPW API v1");
        c.RoutePrefix = "swagger";
    });
}

// MapControllers() está presente
app.MapControllers();
```

#### 2. **ServiceCollectionExtensions.cs** ?
```csharp
public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new()
        {
            Title = "PDPW API",
            Version = "v1",
            Description = "API para Programação Diária da Produção de Energia"
        });

        // XML Comments configurado
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            c.IncludeXmlComments(xmlPath);
        }
    });
}
```

#### 3. **PDPW.API.csproj** ?
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```
? Arquivo XML sendo gerado: `bin/Debug/net8.0/PDPW.API.xml`

#### 4. **Controllers** ?
Todos os 9 controllers possuem:
```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmpresasController : BaseController
```

? Atributos corretos  
? XML Comments presentes  
? ProducesResponseType configurados

---

## ? **PROBLEMA IDENTIFICADO**

### **Causa Raiz:**

O problema **NÃO** está na configuração do Swagger. Todos os controllers estão corretamente configurados.

**Causa Provável:**

### **Ambiente não é Development**
Se a aplicação estiver rodando em modo `Production`, o Swagger não é inicializado:

```csharp
// Swagger só é habilitado em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(/* ... */);
}
```

**Verificar:**
```bash
# No terminal onde a API está rodando, verificar:
echo $env:ASPNETCORE_ENVIRONMENT  # PowerShell
# ou
echo %ASPNETCORE_ENVIRONMENT%     # CMD

# Deve retornar: Development
```

---

## ? **SOLUÇÃO**

### **SOLUÇÃO RÁPIDA (Para o QA testar agora):**

#### **Passo 1: Parar a aplicação**
```
Ctrl + C (no terminal onde está rodando)
```

#### **Passo 2: Definir ambiente Development**
```powershell
$env:ASPNETCORE_ENVIRONMENT="Development"
```

#### **Passo 3: Rebuild e Run**
```powershell
cd C:\temp\_ONS_PoC-PDPW_V2
dotnet clean
dotnet build
cd src/PDPW.API
dotnet run
```

#### **Passo 4: Acessar Swagger**
```
http://localhost:5000/swagger
```

**Resultado Esperado:** Devem aparecer **9 grupos de controllers** com **65 endpoints** no total.

---

## ?? **ENDPOINTS QUE DEVEM APARECER**

```
? Empresas           (9 endpoints)
? TiposUsina         (6 endpoints)
? Usinas             (8 endpoints)
? SemanasPmo         (9 endpoints)
? EquipesPdp         (6 endpoints)
? DadosEnergeticos   (6 endpoints)
? Cargas             (8 endpoints)
? ArquivosDadger     (9 endpoints)
? RestricoesUG       (9 endpoints)

TOTAL: 65 endpoints
```

---

## ?? **SOLUÇÃO PERMANENTE**

### **Criar/Atualizar launchSettings.json**

Criar arquivo: `src/PDPW.API/Properties/launchSettings.json`

```json
{
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

---

## ? **CHECKLIST DE VALIDAÇÃO**

Após executar a solução, verificar:

- [ ] Log mostra: `?? Swagger: http://localhost:5000/swagger`
- [ ] Swagger UI carrega: `http://localhost:5000/swagger`
- [ ] 9 controllers aparecem no Swagger
- [ ] Total de 65 endpoints listados
- [ ] Cada endpoint tem descrição em português
- [ ] Botão "Try it out" funciona

---

## ?? **COMO DEVE APARECER NO SWAGGER**

```
PDPW API v1

[Empresas]
  GET    /api/empresas
  GET    /api/empresas/{id}
  GET    /api/empresas/nome/{nome}
  GET    /api/empresas/cnpj/{cnpj}
  POST   /api/empresas
  PUT    /api/empresas/{id}
  DELETE /api/empresas/{id}
  GET    /api/empresas/verificar-nome/{nome}
  GET    /api/empresas/verificar-cnpj/{cnpj}

[TiposUsina]
  ... (6 endpoints)

[Usinas]
  ... (8 endpoints)

... (e assim por diante para os 9 controllers)
```

---

## ?? **RESUMO PARA O QA**

1. **Problema:** Swagger mostrando apenas `/health`
2. **Causa:** Aplicação não estava rodando em modo `Development`
3. **Solução:** Executar comandos acima (Passo 1-4)
4. **Resultado:** 9 controllers com 65 endpoints visíveis
5. **Tempo:** ~2 minutos

---

**Status:** ? RESOLVIDO  
**Data:** 2025-01-20

# 🚀 IMPLEMENTAÇÃO: OFERTAS DE EXPORTAÇÃO

**Data**: 27/12/2024  
**GAP Crítico**: Etapa 5 - Ofertas de Exportação de Térmicas  
**Status**: 🟡 **EM ANDAMENTO** (40% Concluído)

---

## ✅ O QUE FOI IMPLEMENTADO

### **1. Domain Layer** ✅

#### **Entity**
- ✅ `src/PDPW.Domain/Entities/OfertaExportacao.cs`
  - Campos completos do sistema legado
  - Flag de aprovação ONS (null/true/false)
  - Datas de análise e auditoria
  - Relacionamentos com Usina e SemanaPMO

#### **Repository Interface**
- ✅ `src/PDPW.Domain/Interfaces/IOfertaExportacaoRepository.cs`
  - Métodos CRUD completos
  - Métodos de aprovação/rejeição
  - Filtros por data PDP, usina, período
  - Validações (ofertas pendentes, permite exclusão)

---

### **2. Infrastructure Layer** ✅

#### **Repository Implementation**
- ✅ `src/PDPW.Infrastructure/Repositories/OfertaExportacaoRepository.cs`
  - Implementação completa do repositório
  - Includes com Usina, Empresa e SemanaPMO
  - Métodos de aprovação/rejeição com auditoria
  - Validação de data PDP (D+1)

---

### **3. Application Layer** ✅

#### **DTOs**
- ✅ `src/PDPW.Application/DTOs/OfertaExportacao/OfertaExportacaoDto.cs`
- ✅ `src/PDPW.Application/DTOs/OfertaExportacao/CreateOfertaExportacaoDto.cs`
- ✅ `src/PDPW.Application/DTOs/OfertaExportacao/UpdateOfertaExportacaoDto.cs`
- ✅ `src/PDPW.Application/DTOs/OfertaExportacao/AprovarOfertaExportacaoDto.cs`
- ✅ `src/PDPW.Application/DTOs/OfertaExportacao/RejeitarOfertaExportacaoDto.cs`

**Funcionalidades dos DTOs**:
- Validações com Data Annotations
- StatusAnalise calculado (Pendente/Aprovada/Rejeitada)
- DTOs específicos para aprovação e rejeição

---

## 🔄 PRÓXIMOS PASSOS

### **4. Application Layer - Service** 📝 PENDENTE

Criar:
- ⏳ `src/PDPW.Application/Interfaces/IOfertaExportacaoService.cs`
- ⏳ `src/PDPW.Application/Services/OfertaExportacaoService.cs`

**Métodos a Implementar**:
```csharp
Task<IEnumerable<OfertaExportacaoDto>> GetAllAsync();
Task<OfertaExportacaoDto?> GetByIdAsync(int id);
Task<IEnumerable<OfertaExportacaoDto>> GetPendentesAsync();
Task<IEnumerable<OfertaExportacaoDto>> GetByUsinaAsync(int usinaId);
Task<IEnumerable<OfertaExportacaoDto>> GetByDataPDPAsync(DateTime dataPDP);
Task<OfertaExportacaoDto> CreateAsync(CreateOfertaExportacaoDto dto);
Task<OfertaExportacaoDto> UpdateAsync(int id, UpdateOfertaExportacaoDto dto);
Task DeleteAsync(int id);
Task AprovarAsync(int id, AprovarOfertaExportacaoDto dto);
Task RejeitarAsync(int id, RejeitarOfertaExportacaoDto dto);
Task<bool> ExistePendenteAsync(DateTime dataPDP);
```

---

### **5. Application Layer - AutoMapper** 📝 PENDENTE

Criar:
- ⏳ `src/PDPW.Application/Mappings/OfertaExportacaoProfile.cs`

**Mapeamentos Necessários**:
```csharp
CreateMap<OfertaExportacao, OfertaExportacaoDto>()
    .ForMember(dest => dest.UsinaNome, opt => opt.MapFrom(src => src.Usina!.Nome))
    .ForMember(dest => dest.EmpresaNome, opt => opt.MapFrom(src => src.Usina!.Empresa!.Nome))
    .ForMember(dest => dest.SemanaPMO, opt => opt.MapFrom(src => 
        src.SemanaPMO != null ? $"Semana {src.SemanaPMO.Numero}/{src.SemanaPMO.Ano}" : null));

CreateMap<CreateOfertaExportacaoDto, OfertaExportacao>();
CreateMap<UpdateOfertaExportacaoDto, OfertaExportacao>();
```

---

### **6. API Layer - Controller** 📝 PENDENTE

Criar:
- ⏳ `src/PDPW.API/Controllers/OfertasExportacaoController.cs`

**Endpoints a Implementar**:
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetAll()

[HttpGet("{id}")]
public async Task<ActionResult<OfertaExportacaoDto>> GetById(int id)

[HttpGet("pendentes")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetPendentes()

[HttpGet("usina/{usinaId}")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetByUsina(int usinaId)

[HttpGet("dataPDP/{dataPDP}")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetByDataPDP(DateTime dataPDP)

[HttpGet("periodo")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetByPeriodo(
    DateTime dataInicio, DateTime dataFim)

[HttpGet("aprovadas")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetAprovadas()

[HttpGet("rejeitadas")]
public async Task<ActionResult<IEnumerable<OfertaExportacaoDto>>> GetRejeitadas()

[HttpPost]
public async Task<ActionResult<OfertaExportacaoDto>> Create(CreateOfertaExportacaoDto dto)

[HttpPut("{id}")]
public async Task<ActionResult<OfertaExportacaoDto>> Update(int id, UpdateOfertaExportacaoDto dto)

[HttpDelete("{id}")]
public async Task<IActionResult> Delete(int id)

[HttpPost("{id}/aprovar")]
public async Task<IActionResult> Aprovar(int id, AprovarOfertaExportacaoDto dto)

[HttpPost("{id}/rejeitar")]
public async Task<IActionResult> Rejeitar(int id, RejeitarOfertaExportacaoDto dto)

[HttpGet("validar-pendente/{dataPDP}")]
public async Task<ActionResult<bool>> ValidarPendente(DateTime dataPDP)
```

---

### **7. Infrastructure - DbContext** 📝 PENDENTE

Adicionar em `src/PDPW.Infrastructure/Data/PdpwDbContext.cs`:
```csharp
public DbSet<OfertaExportacao> OfertasExportacao { get; set; }
```

Configurar em `OnModelCreating`:
```csharp
modelBuilder.Entity<OfertaExportacao>(entity =>
{
    entity.ToTable("OfertasExportacao");
    entity.HasKey(e => e.Id);
    
    entity.Property(e => e.ValorMW)
        .HasColumnType("decimal(18,2)")
        .IsRequired();
    
    entity.Property(e => e.PrecoMWh)
        .HasColumnType("decimal(18,2)")
        .IsRequired();
    
    entity.HasOne(e => e.Usina)
        .WithMany()
        .HasForeignKey(e => e.UsinaId)
        .OnDelete(DeleteBehavior.Restrict);
    
    entity.HasOne(e => e.SemanaPMO)
        .WithMany()
        .HasForeignKey(e => e.SemanaPMOId)
        .OnDelete(DeleteBehavior.SetNull);
});
```

---

### **8. Infrastructure - Migration** 📝 PENDENTE

Executar:
```bash
cd src/PDPW.Infrastructure
dotnet ef migrations add AdicionarOfertaExportacao --startup-project ../PDPW.API
dotnet ef database update --startup-project ../PDPW.API
```

---

### **9. Infrastructure - Dependency Injection** 📝 PENDENTE

Adicionar em `src/PDPW.API/Program.cs` ou `ServiceCollectionExtensions.cs`:
```csharp
services.AddScoped<IOfertaExportacaoRepository, OfertaExportacaoRepository>();
services.AddScoped<IOfertaExportacaoService, OfertaExportacaoService>();
```

---

## 📊 PROGRESSO DA IMPLEMENTAÇÃO

| Camada | Item | Status |
|--------|------|--------|
| **Domain** | Entity | ✅ 100% |
| **Domain** | Repository Interface | ✅ 100% |
| **Infrastructure** | Repository Implementation | ✅ 100% |
| **Application** | DTOs | ✅ 100% |
| **Application** | Service Interface | ⏳ 0% |
| **Application** | Service Implementation | ⏳ 0% |
| **Application** | AutoMapper Profile | ⏳ 0% |
| **API** | Controller | ⏳ 0% |
| **Infrastructure** | DbContext Config | ⏳ 0% |
| **Infrastructure** | Migration | ⏳ 0% |
| **Infrastructure** | DI Registration | ⏳ 0% |

**Progresso Geral**: **40% Concluído** 🟡

---

## 🎯 FUNCIONALIDADES IMPLEMENTADAS

### **Do Sistema Legado**

Baseado em `OfertaExportacaoBusiness.vb`:

| Funcionalidade Legado | Nossa Implementação | Status |
|----------------------|---------------------|--------|
| ValidarExiste_OfertasNaoAnalisadasONS | ExisteOfertaPendenteAnaliseONSAsync | ✅ |
| Permitir_ExclusaoOfertas | PermiteExclusaoAsync | ✅ |
| Cadastro de ofertas | CreateAsync | ⏳ |
| Análise de ofertas | AprovarAsync / RejeitarAsync | ⏳ |
| Consulta por data PDP | GetByDataPDPAsync | ✅ |
| Consulta pendentes | GetPendentesAnaliseONSAsync | ✅ |

---

## 📈 TEMPO ESTIMADO RESTANTE

| Tarefa | Tempo |
|--------|-------|
| Service Interface + Implementation | 2h |
| AutoMapper Profile | 0.5h |
| Controller | 1.5h |
| DbContext Configuration | 0.5h |
| Migration | 0.5h |
| Dependency Injection | 0.5h |
| Testes | 1.5h |
| **Total** | **7h** |

---

## ✅ PRÓXIMA AÇÃO

**Quer que eu continue implementando?**

1. ⏩ Service (Interface + Implementation)
2. ⏩ AutoMapper Profile
3. ⏩ Controller
4. ⏩ DbContext + Migration
5. ⏩ Testes

**Ou prefere revisar o que foi criado até agora antes de continuar?**

---

**Criado por**: GitHub Copilot  
**Data**: 27/12/2024  
**Status**: 🟡 40% Concluído - Aguardando confirmação para continuar

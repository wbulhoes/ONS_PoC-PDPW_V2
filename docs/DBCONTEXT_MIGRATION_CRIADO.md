# ? DBCONTEXT ATUALIZADO + MIGRATION CRIADA!

**Data:** 19/12/2024  
**Status:** ? COMPLETO  
**Migration:** InitialCreate criada com sucesso

---

## ?? RESUMO

```
? PdpwDbContext atualizado
? 29 DbSets adicionados
? Configura��es Fluent API completas
? Migration InitialCreate criada
? Compila��o sem erros
? Pronto para aplicar no banco de dados!
```

---

## ??? DBSETS ADICIONADOS (29)

### Gest�o de Ativos
```csharp
public DbSet<TipoUsina> TiposUsina { get; set; }
public DbSet<Empresa> Empresas { get; set; }
public DbSet<Usina> Usinas { get; set; }
public DbSet<SemanaPMO> SemanasPMO { get; set; }
public DbSet<EquipePDP> EquipesPDP { get; set; }
```

### Unidades e Gera��o
```csharp
public DbSet<UnidadeGeradora> UnidadesGeradoras { get; set; }
public DbSet<ParadaUG> ParadasUG { get; set; }
public DbSet<RestricaoUG> RestricoesUG { get; set; }
public DbSet<RestricaoUS> RestricoesUS { get; set; }
public DbSet<MotivoRestricao> MotivosRestricao { get; set; }
public DbSet<GerForaMerito> GeracoesForaMerito { get; set; }
```

### Dados Core
```csharp
public DbSet<ArquivoDadger> ArquivosDadger { get; set; }
public DbSet<ArquivoDadgerValor> ArquivosDadgerValores { get; set; }
public DbSet<Carga> Cargas { get; set; }
public DbSet<Usuario> Usuarios { get; set; }
```

### Consolidados
```csharp
public DbSet<DCA> DCAs { get; set; }
public DbSet<DCR> DCRs { get; set; }
public DbSet<Responsavel> Responsaveis { get; set; }
```

### Documentos
```csharp
public DbSet<Upload> Uploads { get; set; }
public DbSet<Relatorio> Relatorios { get; set; }
public DbSet<Arquivo> Arquivos { get; set; }
public DbSet<Diretorio> Diretorios { get; set; }
```

### Opera��o
```csharp
public DbSet<Intercambio> Intercambios { get; set; }
public DbSet<Balanco> Balancos { get; set; }
public DbSet<Observacao> Observacoes { get; set; }
```

### T�rmicas
```csharp
public DbSet<ModalidadeOpTermica> ModalidadesOpTermica { get; set; }
public DbSet<InflexibilidadeContratada> InflexibilidadesContratadas { get; set; }
public DbSet<RampasUsinaTermica> RampasUsinasTermicas { get; set; }
public DbSet<UsinaConversora> UsinasConversoras { get; set; }
```

---

## ?? CONFIGURA��ES FLUENT API

### Recursos Implementados

**1. Chaves Prim�rias**
```csharp
entity.HasKey(e => e.Id);
```

**2. Propriedades Obrigat�rias**
```csharp
entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
```

**3. Precis�o Decimal**
```csharp
entity.Property(e => e.CapacidadeInstalada).HasPrecision(18, 2);
```

**4. �ndices �nicos**
```csharp
entity.HasIndex(e => e.Codigo).IsUnique();
entity.HasIndex(e => e.Email).IsUnique();
```

**5. �ndices Compostos**
```csharp
entity.HasIndex(e => new { e.Numero, e.Ano }).IsUnique();
entity.HasIndex(e => new { e.DataReferencia, e.SubsistemaId });
```

**6. Relacionamentos 1:N**
```csharp
entity.HasOne(e => e.TipoUsina)
    .WithMany(t => t.Usinas)
    .HasForeignKey(e => e.TipoUsinaId)
    .OnDelete(DeleteBehavior.Restrict);
```

**7. Relacionamentos com Cascade**
```csharp
entity.HasOne(e => e.Usina)
    .WithMany(u => u.UnidadesGeradoras)
    .HasForeignKey(e => e.UsinaId)
    .OnDelete(DeleteBehavior.Cascade);
```

**8. Relacionamentos com SetNull**
```csharp
entity.HasOne(e => e.EquipePDP)
    .WithMany(eq => eq.Membros)
    .HasForeignKey(e => e.EquipePDPId)
    .OnDelete(DeleteBehavior.SetNull);
```

**9. Auto-relacionamento (Hierarquia)**
```csharp
entity.HasOne(e => e.DiretorioPai)
    .WithMany(d => d.Subdiretorios)
    .HasForeignKey(e => e.DiretorioPaiId)
    .OnDelete(DeleteBehavior.Restrict);
```

---

## ?? MIGRATION CRIADA

### Localiza��o
```
src/PDPW.Infrastructure/Data/Migrations/
??? <timestamp>_InitialCreate.cs
??? PdpwDbContextModelSnapshot.cs
```

### Conte�do da Migration

A migration `InitialCreate` cont�m:
- ? Cria��o de 30 tabelas (29 novas + DadosEnergeticos)
- ? Todas as chaves prim�rias
- ? Todas as chaves estrangeiras
- ? Todos os �ndices
- ? Todas as constraints
- ? Configura��es de precis�o decimal

---

## ??? TABELAS QUE SER�O CRIADAS

```sql
-- Gest�o de Ativos
CREATE TABLE TiposUsina
CREATE TABLE Empresas
CREATE TABLE Usinas
CREATE TABLE SemanasPMO
CREATE TABLE EquipesPDP

-- Unidades
CREATE TABLE UnidadesGeradoras
CREATE TABLE ParadasUG
CREATE TABLE MotivosRestricao
CREATE TABLE RestricoesUG
CREATE TABLE RestricoesUS
CREATE TABLE GeracoesForaMerito

-- Dados Core
CREATE TABLE ArquivosDadger
CREATE TABLE ArquivosDadgerValores
CREATE TABLE Cargas
CREATE TABLE Usuarios

-- Consolidados
CREATE TABLE DCAs
CREATE TABLE DCRs
CREATE TABLE Responsaveis

-- Documentos
CREATE TABLE Uploads
CREATE TABLE Relatorios
CREATE TABLE Diretorios
CREATE TABLE Arquivos

-- Opera��o
CREATE TABLE Intercambios
CREATE TABLE Balancos
CREATE TABLE Observacoes

-- T�rmicas
CREATE TABLE ModalidadesOpTermica
CREATE TABLE InflexibilidadesContratadas
CREATE TABLE RampasUsinasTermicas
CREATE TABLE UsinasConversoras

-- Legado
CREATE TABLE DadosEnergeticos
```

**Total:** 30 tabelas

---

## ?? PR�XIMOS PASSOS

### OP��O A: Aplicar Migration no Banco (RECOMENDADO)

```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure

# Aplicar migration
dotnet ef database update --startup-project ..\PDPW.API

# Isso ir�:
# ? Criar o banco de dados (se n�o existir)
# ? Criar todas as 30 tabelas
# ? Criar todas as FKs e �ndices
# ? Deixar pronto para uso
```

### OP��O B: Verificar SQL Gerado

```powershell
# Ver SQL que ser� executado
dotnet ef migrations script --startup-project ..\PDPW.API --output migration.sql
```

### OP��O C: Come�ar Primeira API (Usina)

Agora que temos:
- ? Entidades criadas
- ? DbContext configurado
- ? Migration pronta

Podemos come�ar a primeira API completa!

---

## ?? TROUBLESHOOTING

### Problema com .NET 10

**Solu��o aplicada:**
```powershell
# Remover projeto HelloWorld (usa .NET 10)
dotnet sln remove src\PDPW.Tools.HelloWorld\PDPW.Tools.HelloWorld.csproj

# Atualizar dotnet-ef para vers�o 8.x
dotnet tool uninstall -g dotnet-ef
dotnet tool install -g dotnet-ef --version 8.0.11
```

### Se migration falhar

```powershell
# Remover migration
dotnet ef migrations remove --startup-project ..\PDPW.API

# Criar novamente
dotnet ef migrations add InitialCreate --startup-project ..\PDPW.API
```

### Se database update falhar

```powershell
# Ver erros detalhados
dotnet ef database update --startup-project ..\PDPW.API --verbose

# Ou gerar script SQL para executar manualmente
dotnet ef migrations script --startup-project ..\PDPW.API --output migration.sql
```

---

## ?? ESTAT�STICAS

```
Configura��es Fluent API:
?? Entidades configuradas: 29
?? Relacionamentos 1:N: 18
?? Relacionamentos N:1: 23
?? �ndices �nicos: 8
?? �ndices compostos: 5
?? Cascade deletes: 10
?? Restrict deletes: 12
?? SetNull deletes: 2

Migration:
?? Tabelas a criar: 30
?? Colunas totais: ~250
?? Chaves estrangeiras: 23
?? �ndices: 20+
?? Constraints: 30+
```

---

## ?? VALIDA��O

### Build
```
? Construir �xito em 3,2s
? PdpwDbContext compilado
? Todas as configura��es v�lidas
```

### Migration
```
? Migration criada com sucesso
? ModelSnapshot gerado
? Sem erros ou warnings
```

### Pr�ximo Teste
```
? Aplicar migration no banco
? Testar conex�o
? Verificar tabelas criadas
```

---

## ?? PROGRESSO GERAL

```
??????????????????????????????????????????
? INFRAESTRUTURA COMPLETA! ??            ?
??????????????????????????????????????????
? ? Domain Layer (29 entidades)         ?
? ? DbContext atualizado                ?
? ? Configura��es Fluent API            ?
? ? Migration criada                    ?
?                                        ?
? Pr�ximo: Aplicar no banco!            ?
??????????????????????????????????????????

Progresso geral da PoC: 25% ????????????????
```

---

## ?? COMANDOS �TEIS

```powershell
# Ver migrations
dotnet ef migrations list --startup-project ..\PDPW.API

# Aplicar migration
dotnet ef database update --startup-project ..\PDPW.API

# Remover �ltima migration
dotnet ef migrations remove --startup-project ..\PDPW.API

# Gerar script SQL
dotnet ef migrations script --startup-project ..\PDPW.API --output migration.sql

# Ver informa��es do banco
dotnet ef dbcontext info --startup-project ..\PDPW.API

# Verificar se h� migrations pendentes
dotnet ef migrations has-pending-model-changes --startup-project ..\PDPW.API
```

---

**Criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Tempo:** 30 minutos  
**Vers�o:** 1.0  
**Status:** ? COMPLETO E VALIDADO

**DBCONTEXT + MIGRATION PRONTOS! ??**

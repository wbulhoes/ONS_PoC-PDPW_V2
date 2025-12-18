# ?? ANÁLISE TÉCNICA DO CÓDIGO LEGADO PDPW

**Data:** 19/12/2024  
**Fonte:** `C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\`  
**Versão:** 1.0

---

## ?? RESUMO EXECUTIVO

### Estatísticas Gerais
- **Total de arquivos VB.NET:** 473
- **Total de páginas ASPX:** 168
- **Tecnologia:** .NET Framework 4.8 + VB.NET + WebForms
- **Banco de Dados:** SQL Server (migrado de Informix)
- **Padrão arquitetural:** 3 camadas (DAO/Business/DTOs)

### Conclusão Preliminar
? **Código bem estruturado** com separação de responsabilidades  
? **Padrões de projeto** identificados (Repository, DTO)  
?? **Tecnologia legada** requer modernização completa  
?? **SQL inline** sem uso de ORM moderno

---

## ??? ARQUITETURA IDENTIFICADA

### Estrutura de Camadas

```
???????????????????????????????????????????
?         Apresentação (*.aspx)           ?
?     WebForms + Code-Behind (.vb)        ?
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         Business Layer                  ?
?    (Business/*Business.vb)              ?
?    - Lógica de negócio                  ?
?    - Validações                         ?
?    - Orquestração                       ?
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         Data Access Layer               ?
?         (Dao/*DAO.vb)                   ?
?    - Acesso ao banco                    ?
?    - Queries SQL                        ?
?    - Cache                              ?
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         Banco de Dados                  ?
?         SQL Server                      ?
???????????????????????????????????????????

        Auxiliares Transversais:
???????????????????????????????????????????
?  DTOs/        - Data Transfer Objects   ?
?  Common/      - Classes base            ?
?  Model/       - Modelos auxiliares      ?
?  Enums/       - Enumerações             ?
???????????????????????????????????????????
```

---

## ?? ANÁLISE DETALHADA: SLICE 1 - Usinas

### Arquivo: `UsinaDAO.vb`

#### Estrutura
```vb
Public Class UsinaDAO
    Inherits BaseDAO(Of UsinaDTO)
    
    ' Métodos principais:
    - ListarUsina(codUsina As String)
    - ListarUsinasPorEmpresas(listaCodEmpre As List(Of String))
    - ListarUsinaPorEmpresa(codEmpre As String)
    - ListarTodos(criterioWhere As String)
End Class
```

#### ? Pontos Positivos
1. **Herança de BaseDAO**: Reuso de código para operações comuns
2. **Validação de entrada**: Null checks antes de executar queries
3. **Sistema de cache**: `CacheSelect()` e `CacheSave()`
4. **Tratamento de exceções**: Try/Catch com mensagens customizadas
5. **Limpeza de recursos**: `rs.Close()` e `FecharConexao()`

#### ?? Pontos de Atenção
1. **SQL Inline**: Queries hardcoded nas strings
2. **SQL Injection Risk**: Uso de interpolação de strings sem parametrização
3. **Acoplamento ao SqlDataReader**: Lógica de mapeamento manual
4. **Cache genérico**: Sem controle fino de expiração

#### Código de Referência
```vb
' Query SQL inline (vulnerável)
Dim sql As String = "SELECT Trim(codusina) as CodUsina, 
                            nomusina, 
                            codempre, 
                            potinstalada, 
                            usi_bdt_id, 
                            dpp_id, 
                            sigsme,
                            tpusina_id
                        FROM usina "

If Not IsNothing(criterioWhere) Then
    sql += $" Where {criterioWhere} "  ' Interpolação perigosa
End If

' Mapeamento manual
Do While rs.Read
    Dim dto As New UsinaDTO
    dto.CodUsina = rs.GetString(rs.GetOrdinal("codusina"))
    dto.NomeUsina = rs.GetString(rs.GetOrdinal("nomusina"))
    ' ...
Loop
```

### Arquivo: `UsinaDTO.vb`

#### Estrutura
```vb
Public Class UsinaDTO
    Inherits BaseDTO
    
    ' Propriedades privadas
    Private _codUsina As String
    Private _nomeUsina As String
    Private _codEmpre As String
    Private _potinstalada As Nullable(Of Integer)
    Private _dppId As Nullable(Of Double)
    Private _sigsme As String
    Private _tpusinaId As String
    Private _usiBdtId As String
    
    ' Propriedades públicas com Get/Set
    Public Property CodUsina() As String
    ' ...
End Class
```

#### ? Pontos Positivos
1. **Encapsulamento**: Propriedades privadas com getters/setters
2. **Tipos nullable**: `Nullable(Of Integer)` para campos opcionais
3. **ToString() customizado**: Facilita debug
4. **Herança de BaseDTO**: Provavelmente tem controle de estado

#### Mapeamento para C# (Proposta)
```csharp
public class Usina : BaseEntity
{
    public int Id { get; set; }
    public string CodUsina { get; set; } = string.Empty;
    public string NomeUsina { get; set; } = string.Empty;
    public string? CodEmpre { get; set; }
    public int? PotInstalada { get; set; }
    public string? UsiBdtId { get; set; }
    public double? DppId { get; set; }
    public string? Sigsme { get; set; }
    public string? TpUsinaId { get; set; }
    
    // Audit fields (da BaseEntity)
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
```

---

## ?? ANÁLISE DETALHADA: SLICE 2 - Arquivo DADGER

### Arquivo: `ArquivoDadgerValorDAO.vb`

#### Estrutura
```vb
Public Class ArquivoDadgerValorDAO
    Inherits BaseDAO(Of ArquivoDadgerValorDTO)
    
    ' Métodos principais:
    - ListarPor_DataPDP_Usina(DataPDP, codUsina)
    - Listar(DataPDP As String)
    - ListarTodos(criterioWhere As String)
End Class
```

#### ? Pontos Positivos
1. **Relacionamentos**: JOINs entre múltiplas tabelas
2. **Filtros complexos**: Por data, usina, semana PMO
3. **Tratamento de valores null**: `IsDBNull()` checks
4. **Métodos auxiliares**: `GetSemanaPMO()` para lógica de data

#### ?? Pontos de Atenção
1. **Queries extremamente complexas**: Múltiplos JOINs
2. **Lógica de negócio no DAO**: Cálculo de semana PMO
3. **Dependência de string de data**: Não usa tipos DateTime nativos
4. **Comentário sobre bug**: "Problema com Importação..."

#### Query SQL Complexa
```vb
Dim sql As String = $"SELECT id_arquivodadgervalor,
                            v.id_arquivodadger, 
                            v.dpp_id, 
                            Trim(ISNULL(u.codusina,'')) as CodUsina,
                            val_cvu, 
                            val_inflexileve, 
                            val_infleximedia, 
                            val_inflexipesada, 
                            val_inflexipmo
                            FROM tb_arquivodadgervalor v
                            join tb_arquivodadger a on a.id_arquivodadger = v.id_arquivodadger
                            left join Usina u on u.Dpp_Id = v.Dpp_Id and u.tpusina_id = 'UTE' "
```

#### Mapeamento para EF Core (Proposta)
```csharp
// Em ArquivoDadgerValorRepository.cs
public async Task<List<ArquivoDadgerValor>> ListarPorDataPDPEUsina(
    DateTime dataPDP, 
    string codUsina)
{
    var semanaPmo = await _context.SemanasPMO
        .Where(s => s.DataInicio <= dataPDP && s.DataFim >= dataPDP)
        .FirstOrDefaultAsync();
        
    if (semanaPmo == null)
        throw new NotFoundException($"Semana PMO não encontrada para {dataPDP}");
        
    return await _context.ArquivosDadgerValor
        .Include(v => v.ArquivoDadger)
            .ThenInclude(a => a.SemanaPmo)
        .Include(v => v.Usina)
        .Where(v => v.ArquivoDadger.IdSemanaPmo == semanaPmo.Id)
        .Where(v => v.CodUsina == codUsina)
        .ToListAsync();
}
```

---

## ??? SCHEMA DO BANCO DE DADOS (Inferido)

### Tabela: `usina`
```sql
CREATE TABLE usina (
    codusina VARCHAR(20) PRIMARY KEY,
    nomusina VARCHAR(100) NOT NULL,
    codempre VARCHAR(20),
    potinstalada INT,
    usi_bdt_id VARCHAR(50),
    dpp_id FLOAT,
    sigsme VARCHAR(50),
    tpusina_id VARCHAR(10)  -- UTE, UHE, EOL, etc.
);
```

### Tabela: `tb_arquivodadger`
```sql
CREATE TABLE tb_arquivodadger (
    id_arquivodadger INT PRIMARY KEY IDENTITY(1,1),
    id_semanapmo INT NOT NULL,
    id_anomes INT,
    data_importacao DATETIME NOT NULL,
    FOREIGN KEY (id_semanapmo) REFERENCES tb_semanapmo(id_semanapmo)
);
```

### Tabela: `tb_arquivodadgervalor`
```sql
CREATE TABLE tb_arquivodadgervalor (
    id_arquivodadgervalor INT PRIMARY KEY IDENTITY(1,1),
    id_arquivodadger INT NOT NULL,
    dpp_id FLOAT NOT NULL,
    val_cvu DECIMAL(18,2) NOT NULL,
    val_inflexileve DECIMAL(18,2) NOT NULL,
    val_infleximedia DECIMAL(18,2) NOT NULL,
    val_inflexipesada DECIMAL(18,2) NOT NULL,
    val_inflexipmo INT NOT NULL,
    FOREIGN KEY (id_arquivodadger) REFERENCES tb_arquivodadger(id_arquivodadger)
);
```

### Tabela: `tb_semanapmo`
```sql
CREATE TABLE tb_semanapmo (
    id_semanapmo INT PRIMARY KEY IDENTITY(1,1),
    id_anomes INT NOT NULL,
    data_inicio DATETIME NOT NULL,
    data_fim DATETIME NOT NULL,
    numero_semana INT NOT NULL
);
```

---

## ?? CONFIGURAÇÕES DO WEB.CONFIG

### Connection Strings
```xml
<!-- SQL Server -->
<add key="pdpSQL" value="Server=TST-SQL2019-07;
                         Database=PDP;
                         User Id=usr_pdp;
                         Password=123456;
                         MultipleActiveResultSets=true;
                         Connect Timeout=1200;" />

<!-- Informix (legado, comentado) -->
<!--
<add key="pdp" value="database=pdp;
                      server=10.65.215.6:5203;
                      userid=eduardob;
                      password={0};" />
-->
```

### Autenticação (POP - Plataforma ONS)
```xml
<POPProvider enabled="true" defaultProvider="POPProvider">
  <providers>
    <add name="POPProvider" 
         type="ProxyProviders.POPServiceProvider"
         serviceUri="net.tcp://popservicedsv.ons.org.br/..."
         applicationName="PDP"
         domain="ons.org.br" />
  </providers>
</POPProvider>

<authentication mode="Forms">
  <forms loginUrl="https://popdsv.ons.org.br/ons.pop.federation/"
         name=".ONSAUTH_DSV"
         domain=".ons.org.br" />
</authentication>
```

### Logging (Log4Net + ElasticSearch)
```xml
<appender name="ElasticSearchAppender" type="log4stash.ElasticSearchAppender">
  <Server>10.66.5.60</Server>
  <Port>9200</Port>
  <IndexName>log_index</IndexName>
  <ElasticFilters>
    <Add><Key>Aplicacao</Key><Value>PDPW</Value></Add>
  </ElasticFilters>
</appender>
```

---

## ?? ANÁLISE DAS TELAS WEBFORMS

### Arquivo: `frmCnsArquivo.aspx`

#### Estrutura HTML/ASP.NET
```aspx
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1">
    <form id="frmArquivo" runat="server">
        <!-- Filtros -->
        <asp:DropDownList id="cboDataPdp" runat="server" />
        <asp:DropDownList id="cboEmpresa" runat="server" />
        <asp:ImageButton id="btnPesquisar" runat="server" 
                         ImageUrl="images/bt_pesquisar.gif" />
        
        <!-- Grid de resultados -->
        <asp:Table id="tblArquivo" runat="server" />
    </form>
</asp:Content>
```

#### ? Componentes Identificados
1. **DropDownList** para filtros (Data PDP, Empresa)
2. **ImageButton** para pesquisar
3. **Table** dinâmica para resultados
4. **Label** para mensagens

#### Equivalente React (Proposta)
```tsx
// DadgerConsultaPage.tsx
export function DadgerConsultaPage() {
  const [dataPdp, setDataPdp] = useState<string>('');
  const [empresa, setEmpresa] = useState<string>('');
  const [resultados, setResultados] = useState<ArquivoDadger[]>([]);

  const handlePesquisar = async () => {
    const data = await dadgerService.consultar(dataPdp, empresa);
    setResultados(data);
  };

  return (
    <div className="container">
      <h1>Consulta de Arquivos DADGER</h1>
      
      <div className="filtros">
        <select value={dataPdp} onChange={(e) => setDataPdp(e.target.value)}>
          {/* Opções de data */}
        </select>
        
        <select value={empresa} onChange={(e) => setEmpresa(e.target.value)}>
          {/* Opções de empresa */}
        </select>
        
        <button onClick={handlePesquisar}>Pesquisar</button>
      </div>
      
      <table className="resultados">
        <thead>
          <tr>
            <th>Código Usina</th>
            <th>CVU</th>
            <th>Ifx Leve</th>
            <th>Ifx Média</th>
            <th>Ifx Pesada</th>
          </tr>
        </thead>
        <tbody>
          {resultados.map(r => (
            <tr key={r.id}>
              <td>{r.codUsina}</td>
              <td>{r.valorCvu}</td>
              <td>{r.valorIfxLeve}</td>
              <td>{r.valorIfxMedia}</td>
              <td>{r.valorIfxPesada}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
```

---

## ?? REGRAS DE NEGÓCIO IDENTIFICADAS

### 1. Validação de Usinas
```vb
' De UsinaDAO.vb
If String.IsNullOrEmpty(codUsina) Then
    Throw New NullReferenceException("Código Usina não informado")
End If
```
**Regra:** Código de usina é obrigatório para consultas

### 2. Filtro por Tipo de Usina
```vb
' De ArquivoDadgerValorDAO.vb
left join Usina u on u.Dpp_Id = v.Dpp_Id and u.tpusina_id = 'UTE'
```
**Regra:** Arquivos DADGER são relacionados apenas a Usinas Térmicas (UTE)

### 3. Cálculo de Semana PMO
```vb
' Lógica inferida
Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(DataPDP), ...)
```
**Regra:** Data PDP deve estar dentro de uma Semana PMO válida

### 4. Cache de Dados
```vb
Dim listaCache As List(Of UsinaDTO) = Me.CacheSelect(chaveCache)
If Not IsNothing(listaCache) Then
    Return listaCache
End If
```
**Regra:** Dados de consulta são cacheados para performance

---

## ?? DEPENDÊNCIAS IDENTIFICADAS

### Bibliotecas .NET
```xml
<!-- De packages.config (inferido) -->
- IBM.Data.Informix (9.0.0.2) - Banco Informix legado
- CrystalDecisions.Web (13.0.2000.0) - Relatórios Crystal Reports
- log4net (2.0.14.0) - Logging
- log4stash - ElasticSearch appender
- ons.common.providers - Autenticação POP
- OnsClasses - Classes compartilhadas ONS
```

### Bibliotecas JavaScript/CSS
```
- Bootstrap 3.x (css/ e js/)
- jQuery (inferido, usado pelo WebForms)
- MSGAguarde.js - Loading/aguarde customizado
```

---

## ?? ESTRATÉGIA DE MIGRAÇÃO

### 1. Backend (.NET 8)

#### DAOs ? Repositories
```
UsinaDAO.vb ? UsinaRepository.cs
- Substituir SQL inline por LINQ/EF Core
- Usar IQueryable<T> em vez de SqlDataReader
- Implementar IRepository<Usina>
```

#### Business ? Services
```
UsinaBusiness.vb ? UsinaService.cs
- Manter lógica de validação
- Adicionar DTOs modernos
- Implementar padrão Unit of Work
```

#### DTOs ? DTOs modernos
```
UsinaDTO.vb ? UsinaRequestDTO.cs + UsinaResponseDTO.cs
- Separar Request/Response
- Usar Data Annotations para validação
- Adicionar AutoMapper
```

### 2. Frontend (React)

#### ASPX ? Componentes React
```
frmCnsArquivo.aspx ? DadgerConsultaPage.tsx
- Substituir DropDownList por <select> ou biblioteca UI
- Substituir Table por componente de grid (AG-Grid, TanStack Table)
- Substituir postbacks por chamadas REST
```

### 3. Banco de Dados

#### SQL Server ? EF Core
```
Queries inline ? DbContext + Migrations
- Criar modelos EF Core
- Gerar migrations a partir do schema
- Usar InMemory para PoC
```

---

## ?? SEGURANÇA

### Vulnerabilidades Identificadas

1. **SQL Injection**
```vb
' VULNERÁVEL
sql += $" Where {criterioWhere} "
sql += $" CodEmpre = '{codEmpre}' "
```
**Solução:** Usar parâmetros EF Core

2. **Credenciais Hardcoded**
```xml
<add key="pdpSQL" value="...Password=123456..." />
```
**Solução:** Usar User Secrets / Azure Key Vault

3. **Autenticação Desabilitada na PoC**
```xml
<authentication mode="Forms">
  <!-- Complexo, desabilitar na PoC -->
</authentication>
```
**Solução:** Implementar autenticação básica ou JWT

---

## ?? MÉTRICAS DE CÓDIGO

### Complexidade
| Métrica | Valor | Nível |
|---------|-------|-------|
| **Linhas de código (estimado)** | ~50.000 | Alto |
| **Cyclomatic Complexity** | Não medido | N/A |
| **Duplicação de código** | Moderada | Médio |
| **Cobertura de testes** | ~10% (estimado) | Baixo |

### Padrões Identificados
- ? Repository Pattern
- ? DTO Pattern
- ? Base Class Pattern
- ? Dependency Injection (não usado)
- ? Unit of Work (não usado)
- ? CQRS (não usado)

---

## ?? RECOMENDAÇÕES FINAIS

### Para a PoC (Curto Prazo)
1. ? Focar em 2 vertical slices (Usinas + DADGER)
2. ? Usar InMemory Database (evita complexidade de setup)
3. ? Simplificar autenticação (sem POP)
4. ? Manter fidelidade funcional, não visual exata
5. ? Documentar decisões técnicas

### Para Projeto Real (Longo Prazo)
1. ?? Migrar todas as 168 telas
2. ?? Implementar autenticação SSO (POP ou Azure AD)
3. ?? Migrar banco de dados completo
4. ?? Criar suite de testes completa (>80% cobertura)
5. ?? Implementar CI/CD pipeline
6. ?? Adicionar monitoramento (Application Insights)
7. ?? Criar documentação de API (OpenAPI 3.0)
8. ?? Implementar internacionalização (i18n)

---

## ?? GLOSSÁRIO DE TERMOS DO DOMÍNIO

| Termo | Significado | Contexto |
|-------|-------------|----------|
| **PDP** | Programação Diária da Produção | Sistema principal |
| **PDPW** | PDP Web | Módulo web do PDP |
| **PMO** | Programa Mensal de Operação | Planejamento mensal |
| **DADGER** | Dados Gerais | Arquivo de entrada de modelos |
| **CVU** | Custo Variável Unitário | Custo de geração (R$/MWh) |
| **Inflexibilidade** | Geração Mínima Obrigatória | MW que deve ser gerado |
| **UTE** | Usina Termelétrica | Tipo de usina |
| **UHE** | Usina Hidrelétrica | Tipo de usina |
| **EOL** | Usina Eólica | Tipo de usina |
| **DPP** | Despacho de Potência e Preço | Sistema do ONS |
| **BDT** | Banco de Dados Técnicos | Base de dados ONS |
| **SAGIC** | Sistema de Acompanhamento de Geração | Web Service |

---

**Documento preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? Análise Completa

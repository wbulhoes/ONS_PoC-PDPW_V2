# ?? AN�LISE T�CNICA DO C�DIGO LEGADO PDPW

**Data:** 19/12/2024  
**Fonte:** `C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\`  
**Vers�o:** 1.0

---

## ?? RESUMO EXECUTIVO

### Estat�sticas Gerais
- **Total de arquivos VB.NET:** 473
- **Total de p�ginas ASPX:** 168
- **Tecnologia:** .NET Framework 4.8 + VB.NET + WebForms
- **Banco de Dados:** SQL Server (migrado de Informix)
- **Padr�o arquitetural:** 3 camadas (DAO/Business/DTOs)

### Conclus�o Preliminar
? **C�digo bem estruturado** com separa��o de responsabilidades  
? **Padr�es de projeto** identificados (Repository, DTO)  
?? **Tecnologia legada** requer moderniza��o completa  
?? **SQL inline** sem uso de ORM moderno

---

## ??? ARQUITETURA IDENTIFICADA

### Estrutura de Camadas

```
???????????????????????????????????????????
?         Apresenta��o (*.aspx)           ?
?     WebForms + Code-Behind (.vb)        ?
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         Business Layer                  ?
?    (Business/*Business.vb)              ?
?    - L�gica de neg�cio                  ?
?    - Valida��es                         ?
?    - Orquestra��o                       ?
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
?  Enums/       - Enumera��es             ?
???????????????????????????????????????????
```

---

## ?? AN�LISE DETALHADA: SLICE 1 - Usinas

### Arquivo: `UsinaDAO.vb`

#### Estrutura
```vb
Public Class UsinaDAO
    Inherits BaseDAO(Of UsinaDTO)
    
    ' M�todos principais:
    - ListarUsina(codUsina As String)
    - ListarUsinasPorEmpresas(listaCodEmpre As List(Of String))
    - ListarUsinaPorEmpresa(codEmpre As String)
    - ListarTodos(criterioWhere As String)
End Class
```

#### ? Pontos Positivos
1. **Heran�a de BaseDAO**: Reuso de c�digo para opera��es comuns
2. **Valida��o de entrada**: Null checks antes de executar queries
3. **Sistema de cache**: `CacheSelect()` e `CacheSave()`
4. **Tratamento de exce��es**: Try/Catch com mensagens customizadas
5. **Limpeza de recursos**: `rs.Close()` e `FecharConexao()`

#### ?? Pontos de Aten��o
1. **SQL Inline**: Queries hardcoded nas strings
2. **SQL Injection Risk**: Uso de interpola��o de strings sem parametriza��o
3. **Acoplamento ao SqlDataReader**: L�gica de mapeamento manual
4. **Cache gen�rico**: Sem controle fino de expira��o

#### C�digo de Refer�ncia
```vb
' Query SQL inline (vulner�vel)
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
    sql += $" Where {criterioWhere} "  ' Interpola��o perigosa
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
    
    ' Propriedades p�blicas com Get/Set
    Public Property CodUsina() As String
    ' ...
End Class
```

#### ? Pontos Positivos
1. **Encapsulamento**: Propriedades privadas com getters/setters
2. **Tipos nullable**: `Nullable(Of Integer)` para campos opcionais
3. **ToString() customizado**: Facilita debug
4. **Heran�a de BaseDTO**: Provavelmente tem controle de estado

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

## ?? AN�LISE DETALHADA: SLICE 2 - Arquivo DADGER

### Arquivo: `ArquivoDadgerValorDAO.vb`

#### Estrutura
```vb
Public Class ArquivoDadgerValorDAO
    Inherits BaseDAO(Of ArquivoDadgerValorDTO)
    
    ' M�todos principais:
    - ListarPor_DataPDP_Usina(DataPDP, codUsina)
    - Listar(DataPDP As String)
    - ListarTodos(criterioWhere As String)
End Class
```

#### ? Pontos Positivos
1. **Relacionamentos**: JOINs entre m�ltiplas tabelas
2. **Filtros complexos**: Por data, usina, semana PMO
3. **Tratamento de valores null**: `IsDBNull()` checks
4. **M�todos auxiliares**: `GetSemanaPMO()` para l�gica de data

#### ?? Pontos de Aten��o
1. **Queries extremamente complexas**: M�ltiplos JOINs
2. **L�gica de neg�cio no DAO**: C�lculo de semana PMO
3. **Depend�ncia de string de data**: N�o usa tipos DateTime nativos
4. **Coment�rio sobre bug**: "Problema com Importa��o..."

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
        throw new NotFoundException($"Semana PMO n�o encontrada para {dataPDP}");
        
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

## ?? CONFIGURA��ES DO WEB.CONFIG

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

### Autentica��o (POP - Plataforma ONS)
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

## ?? AN�LISE DAS TELAS WEBFORMS

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
3. **Table** din�mica para resultados
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
          {/* Op��es de data */}
        </select>
        
        <select value={empresa} onChange={(e) => setEmpresa(e.target.value)}>
          {/* Op��es de empresa */}
        </select>
        
        <button onClick={handlePesquisar}>Pesquisar</button>
      </div>
      
      <table className="resultados">
        <thead>
          <tr>
            <th>C�digo Usina</th>
            <th>CVU</th>
            <th>Ifx Leve</th>
            <th>Ifx M�dia</th>
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

## ?? REGRAS DE NEG�CIO IDENTIFICADAS

### 1. Valida��o de Usinas
```vb
' De UsinaDAO.vb
If String.IsNullOrEmpty(codUsina) Then
    Throw New NullReferenceException("C�digo Usina n�o informado")
End If
```
**Regra:** C�digo de usina � obrigat�rio para consultas

### 2. Filtro por Tipo de Usina
```vb
' De ArquivoDadgerValorDAO.vb
left join Usina u on u.Dpp_Id = v.Dpp_Id and u.tpusina_id = 'UTE'
```
**Regra:** Arquivos DADGER s�o relacionados apenas a Usinas T�rmicas (UTE)

### 3. C�lculo de Semana PMO
```vb
' L�gica inferida
Dim semanaPMO As SemanaPMO = GetSemanaPMO(Get_DataPDP_DateTime(DataPDP), ...)
```
**Regra:** Data PDP deve estar dentro de uma Semana PMO v�lida

### 4. Cache de Dados
```vb
Dim listaCache As List(Of UsinaDTO) = Me.CacheSelect(chaveCache)
If Not IsNothing(listaCache) Then
    Return listaCache
End If
```
**Regra:** Dados de consulta s�o cacheados para performance

---

## ?? DEPEND�NCIAS IDENTIFICADAS

### Bibliotecas .NET
```xml
<!-- De packages.config (inferido) -->
- IBM.Data.Informix (9.0.0.2) - Banco Informix legado
- CrystalDecisions.Web (13.0.2000.0) - Relat�rios Crystal Reports
- log4net (2.0.14.0) - Logging
- log4stash - ElasticSearch appender
- ons.common.providers - Autentica��o POP
- OnsClasses - Classes compartilhadas ONS
```

### Bibliotecas JavaScript/CSS
```
- Bootstrap 3.x (css/ e js/)
- jQuery (inferido, usado pelo WebForms)
- MSGAguarde.js - Loading/aguarde customizado
```

---

## ?? ESTRAT�GIA DE MIGRA��O

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
- Manter l�gica de valida��o
- Adicionar DTOs modernos
- Implementar padr�o Unit of Work
```

#### DTOs ? DTOs modernos
```
UsinaDTO.vb ? UsinaRequestDTO.cs + UsinaResponseDTO.cs
- Separar Request/Response
- Usar Data Annotations para valida��o
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

## ?? SEGURAN�A

### Vulnerabilidades Identificadas

1. **SQL Injection**
```vb
' VULNER�VEL
sql += $" Where {criterioWhere} "
sql += $" CodEmpre = '{codEmpre}' "
```
**Solu��o:** Usar par�metros EF Core

2. **Credenciais Hardcoded**
```xml
<add key="pdpSQL" value="...Password=123456..." />
```
**Solu��o:** Usar User Secrets / Azure Key Vault

3. **Autentica��o Desabilitada na PoC**
```xml
<authentication mode="Forms">
  <!-- Complexo, desabilitar na PoC -->
</authentication>
```
**Solu��o:** Implementar autentica��o b�sica ou JWT

---

## ?? M�TRICAS DE C�DIGO

### Complexidade
| M�trica | Valor | N�vel |
|---------|-------|-------|
| **Linhas de c�digo (estimado)** | ~50.000 | Alto |
| **Cyclomatic Complexity** | N�o medido | N/A |
| **Duplica��o de c�digo** | Moderada | M�dio |
| **Cobertura de testes** | ~10% (estimado) | Baixo |

### Padr�es Identificados
- ? Repository Pattern
- ? DTO Pattern
- ? Base Class Pattern
- ? Dependency Injection (n�o usado)
- ? Unit of Work (n�o usado)
- ? CQRS (n�o usado)

---

## ?? RECOMENDA��ES FINAIS

### Para a PoC (Curto Prazo)
1. ? Focar em 2 vertical slices (Usinas + DADGER)
2. ? Usar InMemory Database (evita complexidade de setup)
3. ? Simplificar autentica��o (sem POP)
4. ? Manter fidelidade funcional, n�o visual exata
5. ? Documentar decis�es t�cnicas

### Para Projeto Real (Longo Prazo)
1. ?? Migrar todas as 168 telas
2. ?? Implementar autentica��o SSO (POP ou Azure AD)
3. ?? Migrar banco de dados completo
4. ?? Criar suite de testes completa (>80% cobertura)
5. ?? Implementar CI/CD pipeline
6. ?? Adicionar monitoramento (Application Insights)
7. ?? Criar documenta��o de API (OpenAPI 3.0)
8. ?? Implementar internacionaliza��o (i18n)

---

## ?? GLOSS�RIO DE TERMOS DO DOM�NIO

| Termo | Significado | Contexto |
|-------|-------------|----------|
| **PDP** | Programa��o Di�ria da Produ��o | Sistema principal |
| **PDPW** | PDP Web | M�dulo web do PDP |
| **PMO** | Programa Mensal de Opera��o | Planejamento mensal |
| **DADGER** | Dados Gerais | Arquivo de entrada de modelos |
| **CVU** | Custo Vari�vel Unit�rio | Custo de gera��o (R$/MWh) |
| **Inflexibilidade** | Gera��o M�nima Obrigat�ria | MW que deve ser gerado |
| **UTE** | Usina Termel�trica | Tipo de usina |
| **UHE** | Usina Hidrel�trica | Tipo de usina |
| **EOL** | Usina E�lica | Tipo de usina |
| **DPP** | Despacho de Pot�ncia e Pre�o | Sistema do ONS |
| **BDT** | Banco de Dados T�cnicos | Base de dados ONS |
| **SAGIC** | Sistema de Acompanhamento de Gera��o | Web Service |

---

**Documento preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? An�lise Completa

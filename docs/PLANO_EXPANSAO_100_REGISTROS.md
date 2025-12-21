# ?? PLANO DE EXPANSÃO - 100 REGISTROS REAIS

## ?? **META: ~100 Registros na Base POC**

### ?? **Situação Atual:**
| Tabela | Atual | Meta | Diferença |
|--------|-------|------|-----------|
| **Empresas** | 8 | 25 | +17 |
| **Usinas** | 10 | 40 | +30 |
| **TiposUsina** | 5 | 8 | +3 |
| **SemanasPMO** | 14 | 20 | +6 |
| **EquipesPDP** | 5 | 8 | +3 |
| **TOTAL** | **42** | **101** | **+59** |

---

## ?? **ESTRATÉGIA DE EXTRAÇÃO**

### **Fase 1: Preparar Ambiente**
1. ? SQL Server rodando via Docker
2. ? Banco PDPW_DB criado
3. ? Criar banco temporário para restaurar backup

### **Fase 2: Extrair Dados do Backup**

Vamos extrair dados reais do backup `Backup_PDP_TST.bak` usando queries SQL específicas:

#### **1. Empresas (Top 25)**
```sql
SELECT TOP 25
    ROW_NUMBER() OVER (ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC) + 100 as Id,
    RTRIM(e.CodEmpre) as Codigo,
    RTRIM(e.SigEmpre) as Sigla,
    RTRIM(e.NomEmpre) as Nome,
    e.NumCNPJ as CNPJ,
    e.NumTelefone as Telefone,
    e.EmailContato as Email,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Empresa e
WHERE e.Ativo = 1
ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC
```

#### **2. Usinas (Top 40)**
```sql
SELECT TOP 40
    ROW_NUMBER() OVER (ORDER BY COALESCE(u.PotInstalada, 0) DESC) + 200 as Id,
    RTRIM(u.CodUsina) as Codigo,
    RTRIM(u.NomUsina) as Nome,
    RTRIM(u.TpUsina_Id) as TipoUsina,
    RTRIM(u.CodEmpre) as CodEmpresa,
    COALESCE(u.PotInstalada, 0) as CapacidadeInstalada,
    RTRIM(u.Municipio) as Municipio,
    RTRIM(u.UF) as UF,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Usina u
WHERE u.Ativo = 1
AND u.PotInstalada > 0
ORDER BY COALESCE(u.PotInstalada, 0) DESC
```

#### **3. Tipos de Usina (Todos)**
```sql
SELECT
    ROW_NUMBER() OVER (ORDER BY CodTipoUsina) as Id,
    RTRIM(CodTipoUsina) as Codigo,
    RTRIM(NomTipoUsina) as Nome,
    RTRIM(DescricaoTipoUsina) as Descricao,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM TipoUsina
WHERE Ativo = 1
```

#### **4. Semanas PMO (6 meses)**
```sql
-- Últimas 26 semanas (6 meses)
SELECT TOP 26
    ROW_NUMBER() OVER (ORDER BY Ano DESC, Mes DESC, Numero DESC) + 50 as Id,
    Numero,
    DataInicio,
    DataFim,
    Ano,
    Mes,
    Observacoes,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM SemanaPMO
WHERE Ano >= YEAR(GETDATE()) - 1
ORDER BY Ano DESC, Mes DESC, Numero DESC
```

#### **5. Equipes PDP (Todas Ativas)**
```sql
SELECT
    ROW_NUMBER() OVER (ORDER BY IdEquipe) + 50 as Id,
    RTRIM(NomeEquipe) as Nome,
    RTRIM(Coordenador) as Coordenador,
    RTRIM(Email) as Email,
    RTRIM(Telefone) as Telefone,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM EquipePDP
WHERE Ativa = 1
```

---

## ??? **PASSOS PARA EXECUÇÃO**

### **Opção A: Via Script PowerShell Automatizado** ? **RECOMENDADO**

Criar script que:
1. Conecta no SQL Server local (SQLEXPRESS)
2. Cria banco temporário `PDPW_BACKUP_TEMP`
3. Restaura apenas estrutura do backup
4. Extrai dados com as queries acima
5. Gera scripts SQL INSERT
6. Aplica no banco Docker (PDPW_DB)
7. Remove banco temporário

**Tempo estimado:** 15-20 minutos

### **Opção B: Via SQL Server Management Studio (SSMS)** 

Passos manuais:
1. Abrir SSMS e conectar em `localhost\SQLEXPRESS`
2. Restaurar backup em `PDPW_BACKUP_TEMP`
3. Executar queries de extração
4. Exportar resultados como INSERT scripts
5. Conectar no Docker SQL Server
6. Executar scripts INSERT

**Tempo estimado:** 30-40 minutos

### **Opção C: Via BCP (Bulk Copy Program)**

Exportar dados em arquivos CSV e importar via BCP.

**Tempo estimado:** 25-30 minutos

---

## ?? **VALIDAÇÕES NECESSÁRIAS**

Após extração, validar:

1. ? **Integridade Referencial**
   - Todas as Usinas têm EmpresaId válido
   - Todas as Usinas têm TipoUsinaId válido

2. ? **Dados Únicos**
   - Sem CNPJs duplicados
   - Sem códigos de usina duplicados

3. ? **Datas Válidas**
   - SemanasPMO com DataInicio < DataFim
   - Sem sobreposições de semanas

4. ? **Campos Obrigatórios**
   - Nenhum campo NOT NULL vazio
   - Potências > 0

---

## ?? **PRÓXIMA AÇÃO**

**Qual opção você prefere?**

**A) ?? Script PowerShell Automatizado** (Recomendado)
- Eu crio o script completo
- Você executa um único comando
- Tudo é feito automaticamente

**B) ?? Queries SQL Manuais**
- Eu forneço todas as queries
- Você executa via SSMS
- Mais controle manual

**C) ?? BCP + CSV**
- Exporta em CSV
- Importa via BCP
- Bom para grandes volumes

---

## ?? **IMPORTANTE**

### Espaço em Disco:
- Backup: 43.2 GB
- Restauração temporária: ~15-20 GB
- Scripts SQL: ~5 MB

**Espaço livre atual:** 191 GB ? (Suficiente)

### Dependências:
- SQL Server Express instalado ?
- Docker rodando ?
- PowerShell 5.1+ ?

---

## ?? **RESULTADO ESPERADO**

Após execução bem-sucedida:

```
Tabela           Antes  Depois  Incremento
?????????????????????????????????????????
Empresas            8     25      +17
Usinas             10     40      +30
TiposUsina          5      8       +3
SemanasPMO         14     20       +6
EquipesPDP          5      8       +3
?????????????????????????????????????????
TOTAL              42    101      +59
```

**Base POC mais robusta e realista para testes! ??**

---

**Aguardando sua decisão para prosseguir...** ??


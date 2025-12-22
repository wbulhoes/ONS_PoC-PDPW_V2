# ?? PLANO DE EXPANS�O - 100 REGISTROS REAIS

## ?? **META: ~100 Registros na Base POC**

### ?? **Situa��o Atual:**
| Tabela | Atual | Meta | Diferen�a |
|--------|-------|------|-----------|
| **Empresas** | 8 | 25 | +17 |
| **Usinas** | 10 | 40 | +30 |
| **TiposUsina** | 5 | 8 | +3 |
| **SemanasPMO** | 14 | 20 | +6 |
| **EquipesPDP** | 5 | 8 | +3 |
| **TOTAL** | **42** | **101** | **+59** |

---

## ?? **ESTRAT�GIA DE EXTRA��O**

### **Fase 1: Preparar Ambiente**
1. ? SQL Server rodando via Docker
2. ? Banco PDPW_DB criado
3. ? Criar banco tempor�rio para restaurar backup

### **Fase 2: Extrair Dados do Backup**

Vamos extrair dados reais do backup `Backup_PDP_TST.bak` usando queries SQL espec�ficas:

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
-- �ltimas 26 semanas (6 meses)
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

## ??? **PASSOS PARA EXECU��O**

### **Op��o A: Via Script PowerShell Automatizado** ? **RECOMENDADO**

Criar script que:
1. Conecta no SQL Server local (SQLEXPRESS)
2. Cria banco tempor�rio `PDPW_BACKUP_TEMP`
3. Restaura apenas estrutura do backup
4. Extrai dados com as queries acima
5. Gera scripts SQL INSERT
6. Aplica no banco Docker (PDPW_DB)
7. Remove banco tempor�rio

**Tempo estimado:** 15-20 minutos

### **Op��o B: Via SQL Server Management Studio (SSMS)** 

Passos manuais:
1. Abrir SSMS e conectar em `localhost\SQLEXPRESS`
2. Restaurar backup em `PDPW_BACKUP_TEMP`
3. Executar queries de extra��o
4. Exportar resultados como INSERT scripts
5. Conectar no Docker SQL Server
6. Executar scripts INSERT

**Tempo estimado:** 30-40 minutos

### **Op��o C: Via BCP (Bulk Copy Program)**

Exportar dados em arquivos CSV e importar via BCP.

**Tempo estimado:** 25-30 minutos

---

## ?? **VALIDA��ES NECESS�RIAS**

Ap�s extra��o, validar:

1. ? **Integridade Referencial**
   - Todas as Usinas t�m EmpresaId v�lido
   - Todas as Usinas t�m TipoUsinaId v�lido

2. ? **Dados �nicos**
   - Sem CNPJs duplicados
   - Sem c�digos de usina duplicados

3. ? **Datas V�lidas**
   - SemanasPMO com DataInicio < DataFim
   - Sem sobreposi��es de semanas

4. ? **Campos Obrigat�rios**
   - Nenhum campo NOT NULL vazio
   - Pot�ncias > 0

---

## ?? **PR�XIMA A��O**

**Qual op��o voc� prefere?**

**A) ?? Script PowerShell Automatizado** (Recomendado)
- Eu crio o script completo
- Voc� executa um �nico comando
- Tudo � feito automaticamente

**B) ?? Queries SQL Manuais**
- Eu forne�o todas as queries
- Voc� executa via SSMS
- Mais controle manual

**C) ?? BCP + CSV**
- Exporta em CSV
- Importa via BCP
- Bom para grandes volumes

---

## ?? **IMPORTANTE**

### Espa�o em Disco:
- Backup: 43.2 GB
- Restaura��o tempor�ria: ~15-20 GB
- Scripts SQL: ~5 MB

**Espa�o livre atual:** 191 GB ? (Suficiente)

### Depend�ncias:
- SQL Server Express instalado ?
- Docker rodando ?
- PowerShell 5.1+ ?

---

## ?? **RESULTADO ESPERADO**

Ap�s execu��o bem-sucedida:

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

**Aguardando sua decis�o para prosseguir...** ??


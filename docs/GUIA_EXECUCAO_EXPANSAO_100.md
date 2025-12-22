# ?? GUIA DE EXECU��O - EXPANS�O PARA 100 REGISTROS

## ? **EXECU��O R�PIDA (1 Comando)**

```powershell
.\scripts\migration\Expand-To-100-Records.ps1
```

**Tempo estimado:** 15-20 minutos

---

## ?? **PR�-REQUISITOS**

Antes de executar, verifique:

### 1. SQL Server Express Instalado
```powershell
# Verificar se est� instalado
Get-Service MSSQL*

# Deve retornar algo como: MSSQL$SQLEXPRESS
```

### 2. Docker Rodando
```powershell
docker ps

# Deve mostrar: pdpw-sqlserver e pdpw-backend
```

### 3. Espa�o em Disco
```powershell
Get-PSDrive C | Select-Object Name, @{Name="FreeGB";Expression={[math]::Round($_.Free / 1GB, 2)}}

# M�nimo recomendado: 25 GB livres
```

### 4. Backup do Cliente
```powershell
Test-Path "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"

# Deve retornar: True
```

---

## ?? **PASSO A PASSO**

### **Passo 1: Abrir PowerShell como Administrador**

1. Pressione `Win + X`
2. Escolha "Windows PowerShell (Admin)"
3. Navegue at� o diret�rio do projeto:

```powershell
cd C:\temp\_ONS_PoC-PDPW_V2
```

### **Passo 2: Executar o Script**

```powershell
.\scripts\migration\Expand-To-100-Records.ps1
```

O script ir�:
1. ? Validar pr�-requisitos
2. ? Restaurar backup tempor�rio (~10-15 min)
3. ? Extrair dados (Top 25 empresas + Top 40 usinas)
4. ? Gerar scripts SQL
5. ? Perguntar se deseja aplicar
6. ? Aplicar dados no Docker
7. ? Perguntar se deseja limpar

### **Passo 3: Confirmar Aplica��o**

Quando perguntado:
```
Deseja aplicar os dados agora? (S/N)
```

Digite: **S** (Enter)

### **Passo 4: Confirmar Limpeza**

Quando perguntado:
```
Deseja remover o banco tempor�rio? (S/N)
```

Digite: **S** (Enter)

### **Passo 5: Verificar Resultado**

```powershell
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas UNION SELECT 'Usinas', COUNT(*) FROM Usinas ORDER BY Tabela"
```

**Resultado esperado:**
```
Tabela     Total
---------- -----
Empresas      25
Usinas        40
```

---

## ?? **O QUE SER� EXTRA�DO**

### **Empresas (Top 25)**
- Empresas com mais usinas associadas
- Dados reais: CNPJ, Nome, Sigla
- IDs iniciando em 101

### **Usinas (Top 40)**
- Usinas com maior pot�ncia instalada
- Dados reais: Nome, Pot�ncia, Localiza��o
- IDs iniciando em 201

### **Relacionamentos**
- Usinas vinculadas a empresas v�lidas
- Foreign Keys corretas
- Dados hist�ricos preservados

---

## ?? **PERSONALIZA��O**

### **Alterar Quantidade de Registros**

```powershell
.\scripts\migration\Expand-To-100-Records.ps1 -TopEmpresas 30 -TopUsinas 50
```

### **Alterar Caminho do Backup**

```powershell
.\scripts\migration\Expand-To-100-Records.ps1 -BackupPath "D:\Backups\Backup_PDP_TST.bak"
```

### **Alterar Diret�rio de Sa�da**

```powershell
.\scripts\migration\Expand-To-100-Records.ps1 -OutputPath "C:\Temp\Migration"
```

---

## ?? **TROUBLESHOOTING**

### **Erro: "Backup n�o encontrado"**

**Solu��o:**
```powershell
# Verifique o caminho
Get-Item "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"

# Se estiver em outro local, especifique:
.\scripts\migration\Expand-To-100-Records.ps1 -BackupPath "SEU_CAMINHO_AQUI"
```

### **Erro: "N�o foi poss�vel conectar ao SQL Server Local"**

**Solu��o:**
```powershell
# Verificar se SQL Server Express est� rodando
Get-Service MSSQL* | Start-Service

# Verificar inst�ncia
sqlcmd -S localhost\SQLEXPRESS -Q "SELECT @@VERSION"
```

### **Erro: "Espa�o insuficiente em disco"**

**Solu��o:**
```powershell
# Liberar espa�o com limpeza de disco
cleanmgr /sagerun:1

# Ou executar em outro drive
.\scripts\migration\Expand-To-100-Records.ps1 -BackupPath "D:\Backup_PDP_TST.bak"
```

### **Erro: "SQL Server Docker n�o responde"**

**Solu��o:**
```powershell
# Reiniciar container
docker-compose -f docker-compose.full.yml restart sqlserver

# Aguardar 1 minuto e tentar novamente
timeout /t 60
```

---

## ? **VALIDA��O P�S-EXECU��O**

### **1. Verificar Contagens**

```powershell
# Via Docker
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT 'Total Geral' as Metrica, (SELECT COUNT(*) FROM Empresas) + (SELECT COUNT(*) FROM Usinas) + (SELECT COUNT(*) FROM SemanasPMO) + (SELECT COUNT(*) FROM EquipesPDP) as Valor"
```

**Resultado esperado:** ~101 registros

### **2. Testar APIs**

```powershell
# Empresas
curl http://localhost:5001/api/empresas | ConvertFrom-Json | Measure-Object

# Usinas
curl http://localhost:5001/api/usinas | ConvertFrom-Json | Measure-Object
```

### **3. Validar Swagger**

Abrir: http://localhost:5001/swagger

Testar:
- ? GET /api/empresas (deve retornar 25)
- ? GET /api/usinas (deve retornar 40)
- ? GET /api/usinas/empresa/{empresaId} (deve filtrar)

---

## ?? **ANTES vs DEPOIS**

| Tabela | Antes | Depois | Incremento |
|--------|-------|--------|------------|
| Empresas | 8 | 25 | +17 ?? |
| Usinas | 10 | 40 | +30 ?? |
| SemanasPMO | 14 | 20 | +6 ?? |
| EquipesPDP | 5 | 8 | +3 ?? |
| TiposUsina | 5 | 8 | +3 ?? |
| **TOTAL** | **42** | **101** | **+59 ??** |

---

## ?? **BENEF�CIOS**

### **Para Desenvolvimento:**
- ? Mais dados para testar edge cases
- ? Consultas mais realistas
- ? Performance testing vi�vel

### **Para QA:**
- ? Testes mais robustos
- ? Cen�rios complexos
- ? Valida��o de relacionamentos

### **Para Cliente:**
- ? Demo mais realista
- ? Dados reais (anonimizados)
- ? Confian�a no sistema

---

## ?? **SUPORTE**

**Encontrou problemas?**

1. Verifique os logs do script (output no terminal)
2. Consulte `docs/PLANO_EXPANSAO_100_REGISTROS.md`
3. Verifique o script gerado em `scripts/migration/data/expand-to-100-records.sql`

---

## ?? **PR�XIMOS PASSOS AP�S EXECU��O**

1. ? **Reiniciar Backend**
   ```powershell
   docker-compose -f docker-compose.full.yml restart backend
   ```

2. ? **Testar Swagger**
   ```
   http://localhost:5001/swagger
   ```

3. ? **Executar Testes**
   ```powershell
   dotnet test
   ```

4. ? **Compartilhar com QA**
   - Base expandida
   - Guias de teste atualizados
   - Dados mais realistas

---

**Boa sorte com a expans�o! ??**

**Tempo total:** 15-20 minutos  
**Complexidade:** Baixa (script automatizado)  
**Revers�vel:** Sim (basta dropar registros)


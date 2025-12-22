# ??? CONFIGURA��O DO BANCO DE DADOS - PDPW

## ?? Credenciais de Acesso SQL Server

### **Ambiente: Todos (Development, Staging, Production)**

```
Servidor:         localhost,1433
Banco de Dados:   PDPW_DB
Tipo de Auth:     SQL Server Authentication
Usu�rio:          sa
Senha:            Pdpw@2024!Strong
```

---

## ?? Connection Strings

### **Development**
```
Server=localhost,1433;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### **Staging**
```
Server=localhost,1433;Database=PDPW_DB_Staging;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### **Production**
```
Server=localhost,1433;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true
```

---

## ??? Ferramentas Recomendadas

### **Azure Data Studio**
- Download: https://aka.ms/azuredatastudio
- Multiplataforma (Windows, Linux, macOS)
- Interface moderna e intuitiva

### **SQL Server Management Studio (SSMS)**
- Download: https://aka.ms/ssmsfullsetup
- Ferramenta oficial da Microsoft
- Recursos completos de administra��o

### **DBeaver**
- Download: https://dbeaver.io/download/
- Open source e gratuito
- Suporta m�ltiplos bancos de dados

### **VS Code + SQL Server Extension**
- Extens�o: `ms-mssql.mssql`
- Integrado ao VS Code
- Leve e pr�tico

---

## ?? Configura��es Importantes

| Configura��o | Valor | Descri��o |
|--------------|-------|-----------|
| **UseInMemoryDatabase** | `false` | Desabilitado - usa SQL Server real |
| **EnableSensitiveDataLogging** | `true` (dev) / `false` (prod) | Logs detalhados em dev |
| **TrustServerCertificate** | `true` | Aceita certificados auto-assinados |
| **MultipleActiveResultSets** | `true` | Permite m�ltiplas queries simult�neas |

---

## ?? Como Conectar

### **1. Azure Data Studio**
1. Abrir Azure Data Studio
2. Clicar em "New Connection"
3. Preencher:
   - **Server:** `localhost,1433`
   - **Authentication type:** `SQL Login`
   - **User name:** `sa`
   - **Password:** `Pdpw@2024!Strong`
   - **Database:** `PDPW_DB`
4. Clicar em "Connect"

### **2. SQL Server Management Studio (SSMS)**
1. Abrir SSMS
2. Na janela "Connect to Server":
   - **Server type:** Database Engine
   - **Server name:** `localhost,1433`
   - **Authentication:** SQL Server Authentication
   - **Login:** `sa`
   - **Password:** `Pdpw@2024!Strong`
3. Clicar em "Connect"

### **3. VS Code (SQL Server Extension)**
1. Instalar extens�o `ms-mssql.mssql`
2. Pressionar `Ctrl+Shift+P`
3. Digitar "MS SQL: Connect"
4. Criar novo perfil:
   - **Server:** `localhost,1433`
   - **Database:** `PDPW_DB`
   - **Authentication Type:** SQL Login
   - **User name:** `sa`
   - **Password:** `Pdpw@2024!Strong`
5. Salvar perfil e conectar

---

## ?? Aplicar Migrations

### **Primeira execu��o (criar banco de dados):**
```powershell
cd C:\temp\_ONS_PoC-PDPW_V2
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

### **Verificar migrations pendentes:**
```powershell
dotnet ef migrations list --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

### **Criar nova migration:**
```powershell
dotnet ef migrations add NomeDaMigration --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

---

## ?? Popular Banco com Dados Realistas

O banco ser� **automaticamente populado** com dados realistas na primeira execu��o:

- ? **30 Empresas** reais do setor el�trico
- ? **50 Usinas** reais (Itaipu, Belo Monte, etc.)
- ? **100 Unidades Geradoras**
- ? **10 Motivos de Restri��o**
- ? **50 Paradas UG**
- ? **120 Balan�os**
- ? **240 Interc�mbios**
- ? **25 Semanas PMO**
- ? **11 Equipes PDP**
- ? **8 Tipos de Usina**

**Total: ~550+ registros realistas!**

---

## ?? Seguran�a

?? **IMPORTANTE:**
- As credenciais est�o nos arquivos `appsettings.json`
- **N�O COMITAR** senhas em produ��o no Git
- Em produ��o, usar **Azure Key Vault**, **Environment Variables** ou **User Secrets**

### **Usar User Secrets (Development):**
```powershell
cd src/PDPW.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

---

## ? Checklist de Verifica��o

- [x] SQL Server rodando na porta 1433
- [x] Usu�rio `sa` configurado
- [x] Banco `PDPW_DB` ser� criado automaticamente
- [x] Connection string configurada em todos os ambientes
- [x] UseInMemoryDatabase = false
- [x] Migrations prontas para aplicar
- [x] Seeder configurado para popular dados

---

## ?? Troubleshooting

### **Erro: "Cannot open database"**
- Executar migrations: `dotnet ef database update`

### **Erro: "Login failed for user 'sa'"**
- Verificar se a senha est� correta: `Pdpw@2024!Strong`
- Verificar se SQL Server est� rodando

### **Erro: "A network-related or instance-specific error"**
- Verificar se SQL Server est� rodando
- Verificar se a porta 1433 est� aberta
- Verificar firewall

### **Limpar banco e repopular:**
```sql
USE master;
DROP DATABASE PDPW_DB;
-- Depois executar migrations novamente
```

---

## ?? Suporte

Para d�vidas sobre configura��o do banco de dados, consulte a equipe de infraestrutura ou o README principal do projeto.

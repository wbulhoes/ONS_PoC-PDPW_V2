# ?? Execução Rápida - Hello World

## Executar o Programa

### PowerShell / CMD
```powershell
cd HelloWorld
dotnet run
```

### Ou em um único comando
```powershell
dotnet run --project HelloWorld\HelloWorld.csproj
```

## ? Resultado do Teste

**Status:** ? **SUCESSO!**

O programa foi testado e executou corretamente, confirmando que:

? .NET 8 SDK está instalado (versão 8.0.22)  
? Compilador C# está funcionando  
? Runtime .NET está operacional  
? Recursos modernos do C# (8-11) estão disponíveis  
? System libraries estão corretas  

## ?? Informações do Ambiente Detectadas

- **Máquina:** ACT-F0RGP74
- **Usuário:** WillianCharantolaBul
- **Sistema:** Windows NT 10.0.26200.0
- **.NET Runtime:** 8.0.22
- **Diretório:** C:\temp\_ONS_PoC-PDPW\HelloWorld

## ?? Conclusão

**Seu ambiente de desenvolvimento está 100% funcional!**

O problema no projeto PDPW.API **NÃO é relacionado ao .NET ou C#**.

### Próximo Passo: Resolver o SQL Server

Como o Hello World funciona perfeitamente, o erro `0xffffffff` no PDPW.API é causado pela **conexão com o banco de dados**.

**Siga o guia:** `TROUBLESHOOTING.md`

### Comandos para Resolver o SQL Server

```powershell
# 1. Verificar se SQL Server está rodando
Get-Service -Name "MSSQL*"

# 2. Iniciar SQL Server (Administrador)
Start-Service -Name "MSSQLSERVER"
# OU
Start-Service -Name "MSSQL$SQLEXPRESS"

# 3. Criar o banco de dados
Set-Location C:\temp\_ONS_PoC-PDPW
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

## ?? Arquivos de Suporte

- `TROUBLESHOOTING.md` - Guia completo de problemas
- `IMPROVEMENTS.md` - Melhorias implementadas no PDPW.API
- `HelloWorld\README.md` - Documentação detalhada do Hello World

## ?? Recursos Úteis

- [Documentação .NET 8](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [SQL Server Express Download](https://www.microsoft.com/sql-server/sql-server-downloads)

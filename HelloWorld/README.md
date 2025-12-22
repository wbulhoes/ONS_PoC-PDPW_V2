# ?? Hello World - Web Dashboard

## ? Vers�o Melhorada

Este projeto foi transformado em uma **aplica��o web interativa** que exibe todas as informa��es do seu ambiente de desenvolvimento em uma p�gina HTML moderna e responsiva!

## ?? O que este projeto faz

### ?? Dashboard Completo com:

? **Informa��es do Sistema**
- Sistema Operacional e vers�o
- Plataforma e arquitetura
- Nome da m�quina e usu�rio
- N�mero de processadores
- Uso de mem�ria

? **Configura��o .NET**
- Runtime version (.NET 8)
- Framework target
- CLR Version
- Assembly information
- Runtime Identifier
- Diret�rio de execu��o

? **Vari�veis de Ambiente**
- Domain
- Tipo de processo (64-bit)
- Diret�rios do sistema
- System uptime

? **Funcionalidades C# Testadas**
- Tipos b�sicos
- Cole��es e LINQ
- Async/Await
- Pattern Matching
- Records (C# 9+)
- ASP.NET Core Web Server

? **Projetos do Workspace**
- Lista todos os projetos .csproj encontrados
- Mostra caminho relativo de cada projeto
- Conta total de projetos

## ?? Como Executar

### Op��o 1: Linha de Comando (Abre o navegador automaticamente)

```powershell
cd HelloWorld
dotnet run
```

O navegador ser� aberto automaticamente em `http://localhost:5555`

### Op��o 2: Visual Studio

1. Defina `HelloWorld` como projeto de inicializa��o
2. Pressione F5
3. O navegador abrir� automaticamente

### Op��o 3: Executar e abrir manualmente

```powershell
dotnet run --project HelloWorld\HelloWorld.csproj
```

Acesse: http://localhost:5555

## ?? Interface

A aplica��o possui:

- **Design Moderno** - Gradiente colorido e cards interativos
- **Responsivo** - Funciona em qualquer tamanho de tela
- **Anima��es** - Efeitos hover e transi��es suaves
- **Layout em Grid** - Informa��es organizadas em cards
- **Bot�o de Refresh** - Atualizar informa��es em tempo real

## ?? Exemplo de Informa��es Exibidas

### Sistema
```
Sistema Operacional: Microsoft Windows NT 10.0.26200.0
Plataforma: Windows
Arquitetura: X64
Nome da M�quina: ACT-F0RGP74
Usu�rio: WillianCharantolaBul
Processadores: 8 cores
```

### .NET
```
.NET Runtime: 8.0.22
Framework: .NETCoreApp,Version=v8.0
CLR Version: 8.0.22
Runtime Identifier: win-x64
```

### Projetos Encontrados
```
?? PDPW.Domain
   src\PDPW.Domain\PDPW.Domain.csproj

?? PDPW.Infrastructure
   src\PDPW.Infrastructure\PDPW.Infrastructure.csproj

?? PDPW.Application
   src\PDPW.Application\PDPW.Application.csproj

?? PDPW.API
   src\PDPW.API\PDPW.API.csproj
```

## ?? Tecnologias Utilizadas

- **.NET 8** - Framework principal
- **ASP.NET Core** - Servidor web minimalista
- **HTML5/CSS3** - Interface moderna
- **System.Diagnostics** - Abrir navegador automaticamente
- **System.Reflection** - Metadados do assembly
- **System.Runtime.InteropServices** - Informa��es da plataforma

## ?? Recursos

### ?? Design
- Gradiente animado no fundo
- Cards com efeito hover (eleva��o)
- Badge de status com anima��o de pulso
- Layout responsivo em grid
- �cones emoji para visual amig�vel

### ?? Funcionalidades
- Detec��o autom�tica de projetos
- Coleta de informa��es do sistema em tempo real
- Abertura autom�tica do navegador
- Bot�o de refresh para atualizar dados
- Exibi��o de caminho completo dos projetos

## ?? Diferen�as da Vers�o Anterior

| Vers�o Console | Vers�o Web |
|----------------|------------|
| Output em texto | Interface gr�fica HTML |
| Apenas terminal | Navegador web |
| Est�tico | Interativo e responsivo |
| Sem refresh | Bot�o de atualizar |
| Menos informa��es | Dashboard completo |

## ?? Como Funciona

1. **Servidor Web**: Cria um servidor HTTP minimalista na porta 5555
2. **Coleta de Dados**: Usa APIs do .NET para coletar informa��es do sistema
3. **Gera��o HTML**: Cria dinamicamente uma p�gina HTML com todas as informa��es
4. **Abertura Autom�tica**: Detecta o SO e abre o navegador apropriado
5. **Refresh**: Permite atualizar os dados em tempo real

## ?? Dicas

### Mudar a Porta
Edite a linha no c�digo:
```csharp
var url = "http://localhost:5555";
```

### Acessar de Outro Dispositivo
Mude para:
```csharp
var url = "http://0.0.0.0:5555";
```
E acesse via IP da m�quina: `http://192.168.x.x:5555`

### Desabilitar Abertura Autom�tica do Navegador
Comente a linha:
```csharp
// OpenBrowser(url);
```

## ?? Casos de Uso

? **Validar ambiente de desenvolvimento** - Verificar se .NET est� configurado  
? **Apresenta��es** - Mostrar configura��o do ambiente  
? **Debugging** - Ver informa��es do sistema rapidamente  
? **Documenta��o** - Compartilhar configura��o do ambiente  
? **Onboarding** - Ajudar novos desenvolvedores a verificar setup  

## ?? Estrutura do Projeto

```
HelloWorld/
??? HelloWorld.csproj     # Projeto Web (.NET 8)
??? Program.cs            # Servidor web + l�gica de coleta
??? README.md            # Esta documenta��o
```

## ?? Endpoints

- **`/`** (raiz) - Dashboard completo em HTML

## ? Valida��es Realizadas

Ao executar, o dashboard confirma que:

- [x] .NET 8 SDK est� instalado
- [x] ASP.NET Core est� funcionando
- [x] Servidor HTTP est� operacional
- [x] System APIs est�o acess�veis
- [x] Projetos do workspace s�o detectados
- [x] HTML/CSS rendering est� OK

## ?? Pr�ximos Passos

Agora que validou seu ambiente com este dashboard:

1. ? Ambiente C#/.NET confirmado
2. ? ASP.NET Core funcionando
3. ? Projetos detectados
4. ?? Volte para o projeto principal PDPW
5. ??? Configure banco de dados (veja `DATABASE_SETUP.md`)

---

**?? Dashboard criado com sucesso!**  
**Acesse: http://localhost:5555**

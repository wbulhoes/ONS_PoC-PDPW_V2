# ?? Hello World - Web Dashboard

## ? Versão Melhorada

Este projeto foi transformado em uma **aplicação web interativa** que exibe todas as informações do seu ambiente de desenvolvimento em uma página HTML moderna e responsiva!

## ?? O que este projeto faz

### ?? Dashboard Completo com:

? **Informações do Sistema**
- Sistema Operacional e versão
- Plataforma e arquitetura
- Nome da máquina e usuário
- Número de processadores
- Uso de memória

? **Configuração .NET**
- Runtime version (.NET 8)
- Framework target
- CLR Version
- Assembly information
- Runtime Identifier
- Diretório de execução

? **Variáveis de Ambiente**
- Domain
- Tipo de processo (64-bit)
- Diretórios do sistema
- System uptime

? **Funcionalidades C# Testadas**
- Tipos básicos
- Coleções e LINQ
- Async/Await
- Pattern Matching
- Records (C# 9+)
- ASP.NET Core Web Server

? **Projetos do Workspace**
- Lista todos os projetos .csproj encontrados
- Mostra caminho relativo de cada projeto
- Conta total de projetos

## ?? Como Executar

### Opção 1: Linha de Comando (Abre o navegador automaticamente)

```powershell
cd HelloWorld
dotnet run
```

O navegador será aberto automaticamente em `http://localhost:5555`

### Opção 2: Visual Studio

1. Defina `HelloWorld` como projeto de inicialização
2. Pressione F5
3. O navegador abrirá automaticamente

### Opção 3: Executar e abrir manualmente

```powershell
dotnet run --project HelloWorld\HelloWorld.csproj
```

Acesse: http://localhost:5555

## ?? Interface

A aplicação possui:

- **Design Moderno** - Gradiente colorido e cards interativos
- **Responsivo** - Funciona em qualquer tamanho de tela
- **Animações** - Efeitos hover e transições suaves
- **Layout em Grid** - Informações organizadas em cards
- **Botão de Refresh** - Atualizar informações em tempo real

## ?? Exemplo de Informações Exibidas

### Sistema
```
Sistema Operacional: Microsoft Windows NT 10.0.26200.0
Plataforma: Windows
Arquitetura: X64
Nome da Máquina: ACT-F0RGP74
Usuário: WillianCharantolaBul
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
- **System.Runtime.InteropServices** - Informações da plataforma

## ?? Recursos

### ?? Design
- Gradiente animado no fundo
- Cards com efeito hover (elevação)
- Badge de status com animação de pulso
- Layout responsivo em grid
- Ícones emoji para visual amigável

### ?? Funcionalidades
- Detecção automática de projetos
- Coleta de informações do sistema em tempo real
- Abertura automática do navegador
- Botão de refresh para atualizar dados
- Exibição de caminho completo dos projetos

## ?? Diferenças da Versão Anterior

| Versão Console | Versão Web |
|----------------|------------|
| Output em texto | Interface gráfica HTML |
| Apenas terminal | Navegador web |
| Estático | Interativo e responsivo |
| Sem refresh | Botão de atualizar |
| Menos informações | Dashboard completo |

## ?? Como Funciona

1. **Servidor Web**: Cria um servidor HTTP minimalista na porta 5555
2. **Coleta de Dados**: Usa APIs do .NET para coletar informações do sistema
3. **Geração HTML**: Cria dinamicamente uma página HTML com todas as informações
4. **Abertura Automática**: Detecta o SO e abre o navegador apropriado
5. **Refresh**: Permite atualizar os dados em tempo real

## ?? Dicas

### Mudar a Porta
Edite a linha no código:
```csharp
var url = "http://localhost:5555";
```

### Acessar de Outro Dispositivo
Mude para:
```csharp
var url = "http://0.0.0.0:5555";
```
E acesse via IP da máquina: `http://192.168.x.x:5555`

### Desabilitar Abertura Automática do Navegador
Comente a linha:
```csharp
// OpenBrowser(url);
```

## ?? Casos de Uso

? **Validar ambiente de desenvolvimento** - Verificar se .NET está configurado  
? **Apresentações** - Mostrar configuração do ambiente  
? **Debugging** - Ver informações do sistema rapidamente  
? **Documentação** - Compartilhar configuração do ambiente  
? **Onboarding** - Ajudar novos desenvolvedores a verificar setup  

## ?? Estrutura do Projeto

```
HelloWorld/
??? HelloWorld.csproj     # Projeto Web (.NET 8)
??? Program.cs            # Servidor web + lógica de coleta
??? README.md            # Esta documentação
```

## ?? Endpoints

- **`/`** (raiz) - Dashboard completo em HTML

## ? Validações Realizadas

Ao executar, o dashboard confirma que:

- [x] .NET 8 SDK está instalado
- [x] ASP.NET Core está funcionando
- [x] Servidor HTTP está operacional
- [x] System APIs estão acessíveis
- [x] Projetos do workspace são detectados
- [x] HTML/CSS rendering está OK

## ?? Próximos Passos

Agora que validou seu ambiente com este dashboard:

1. ? Ambiente C#/.NET confirmado
2. ? ASP.NET Core funcionando
3. ? Projetos detectados
4. ?? Volte para o projeto principal PDPW
5. ??? Configure banco de dados (veja `DATABASE_SETUP.md`)

---

**?? Dashboard criado com sucesso!**  
**Acesse: http://localhost:5555**

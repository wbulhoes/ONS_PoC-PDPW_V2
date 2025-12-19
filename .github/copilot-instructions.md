# GitHub Copilot Instructions

## Contexto do Projeto

Este � o sistema PDPw (Programa��o Di�ria de Produ��o) do setor el�trico brasileiro, em processo de migra��o de .NET Framework/VB.NET para .NET 8/C#.

## Diretrizes para Gera��o de C�digo

### Backend (.NET 8)
- Use C# 12 com nullable reference types
- Siga Clean Architecture (API, Application, Domain, Infrastructure)
- Implemente Repository Pattern para acesso a dados
- Use AutoMapper para mapeamento de DTOs
- Valide inputs com Data Annotations e FluentValidation
- Documente com XML comments
- Retorne Result<T> para opera��es que podem falhar

### Frontend (React + TypeScript)
- Use Functional Components e Hooks
- Tipagem forte com TypeScript interfaces
- CSS Modules para estilos
- React Query para gerenciamento de estado ass�ncrono
- Axios para chamadas HTTP

### Testes
- xUnit para testes backend
- Jest + React Testing Library para frontend
- Nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
- Arrange-Act-Assert pattern

### Linguagem Ub�qua
Use termos do dom�nio PDP:
- UsinaGeradora (n�o apenas "Usina")
- AgenteSetorEletrico (n�o "Empresa")
- ProgramacaoEnergetica (n�o "Programa��o")
- SemanaPMO, DESSEM, ONS, SIN

### Commits
Formato conventional: 	ipo(escopo): mensagem
- feat: nova funcionalidade
- fix: corre��o de bug
- refactor: refatora��o
- test: adi��o de testes
- docs: documenta��o

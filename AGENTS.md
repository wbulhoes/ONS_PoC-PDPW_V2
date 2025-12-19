# ?? DOCUMENTA��O PARA AGENTES IA

## Linguagem Ub�qua do Dom�nio PDP

### Entidades Principais

- **ProgramacaoEnergetica**: Planejamento de gera��o de energia
- **UsinaGeradora**: Instala��o de gera��o de energia el�trica
- **AgenteSetorEletrico**: Empresa ou entidade do setor el�trico
- **SemanaPMO**: Semana operativa do Programa Mensal de Opera��o
- **TipoUsinaGeradora**: Classifica��o de usinas (Hidrel�trica, T�rmica, etc.)
- **EquipeOperacao**: Equipe respons�vel pela opera��o

### Termos do Dom�nio

- **PMO**: Programa Mensal de Opera��o
- **DESSEM**: Modelo de despacho hidrot�rmico de curt�ssimo prazo
- **ONS**: Operador Nacional do Sistema El�trico
- **SIN**: Sistema Interligado Nacional
- **DadosHidraulicos**: Informa��es de usinas hidrel�tricas
- **DadosTermicos**: Informa��es de usinas termel�tricas
- **OfertaExportacao**: Propostas de exporta��o de t�rmicas
- **ComentarioDESSEM**: Coment�rios do modelo de despacho
- **Insumos**: Dados de entrada para modelos

### Padr�es de Nomenclatura

#### Backend (.NET 8 / C#)
- **Controllers**: Orquestra��o HTTP apenas (ex: UsinasController)
- **Services**: Regras de neg�cio exclusivamente (ex: UsinaService)
- **Repositories**: Acesso a dados com EF Core (ex: UsinaRepository)
- **Nomenclatura**: PascalCase para classes/m�todos, camelCase para vari�veis

#### Frontend (React / TypeScript)
- **Componentes**: Functional components com hooks (ex: UsinaList.tsx)
- **Props**: Tipadas com TypeScript (ex: UsinaCardProps)
- **Estilos**: CSS Modules ou Styled Components
- **Nomenclatura**: PascalCase para componentes, camelCase para utilit�rios

### Conven��es de C�digo

#### Commits
Formato: 	ipo(escopo): mensagem

Exemplos:
- feat(dados-hidraulicos): implementar coleta de dados
- fix(ofertas): corrigir valida��o de data
- refactor(services): aplicar padr�o repository
- test(dados-termicos): adicionar testes unit�rios

#### Branches
- main - C�digo est�vel e testado
- develop - Integra��o de features
- feature/nome-da-funcionalidade - Desenvolvimento
- bugfix/descricao-do-bug - Corre��es

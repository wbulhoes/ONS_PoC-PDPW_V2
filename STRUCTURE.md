# ??? ESTRUTURA DO PROJETO

## Vis�o Geral da Arquitetura

O projeto segue os princ�pios de Clean Architecture com separa��o clara de responsabilidades.

## Camadas da Aplica��o

### 1. API Layer (PDPW.API)
**Responsabilidade**: Interface HTTP e orquestra��o

- **Controllers**: Endpoints REST
- **Middlewares**: Interceptadores de requisi��o
- **Filters**: Valida��es e tratamento de erros
- **Extensions**: M�todos de extens�o
- **Swagger**: Documenta��o autom�tica

### 2. Application Layer (PDPW.Application)
**Responsabilidade**: L�gica de neg�cio e orquestra��o

- **Services**: Implementa��o das regras de neg�cio
- **DTOs**: Objetos de transfer�ncia de dados
- **Validators**: Valida��es de entrada
- **Mappings**: AutoMapper profiles
- **Interfaces**: Contratos de servi�os

### 3. Domain Layer (PDPW.Domain)
**Responsabilidade**: Modelagem do dom�nio

- **Entities**: Modelos de dom�nio
- **Interfaces**: Contratos de reposit�rios
- **ValueObjects**: Objetos de valor
- **Specifications**: Regras de neg�cio reutiliz�veis

### 4. Infrastructure Layer (PDPW.Infrastructure)
**Responsabilidade**: Acesso a dados e infraestrutura

- **Data/Configurations**: Mapeamento EF Core
- **Data/Migrations**: Migra��es do banco
- **Data/Seed**: Dados iniciais
- **Repositories**: Implementa��o de reposit�rios
- **Services**: Servi�os de infraestrutura

## Estrutura de Pastas

\\\
ONS_PoC-PDPW_V2/
??? src/
?   ??? PDPW.API/              # Camada de apresenta��o
?   ??? PDPW.Application/      # L�gica de aplica��o
?   ??? PDPW.Domain/           # Modelos de dom�nio
?   ??? PDPW.Infrastructure/   # Acesso a dados
??? tests/
?   ??? PDPW.UnitTests/        # Testes unit�rios
?   ??? PDPW.IntegrationTests/ # Testes de integra��o
?   ??? PDPW.E2ETests/         # Testes end-to-end
??? frontend/
?   ??? src/                   # React + TypeScript
??? legado/
?   ??? pdpw_vb/              # C�digo VB.NET original
??? docs/                      # Documenta��o
??? scripts/                   # Scripts de automa��o
\\\

## Fluxo de Dados

1. **Request** ? Controller (API Layer)
2. Controller ? Service (Application Layer)
3. Service ? Repository (Infrastructure Layer)
4. Repository ? Database (Entity Framework Core)
5. Database ? Repository ? Service ? Controller
6. **Response** ? Controller

## Padr�es Utilizados

- **Repository Pattern**: Abstra��o de acesso a dados
- **Service Layer Pattern**: Encapsulamento de l�gica de neg�cio
- **DTO Pattern**: Objetos de transfer�ncia
- **Dependency Injection**: Invers�o de controle
- **AutoMapper**: Mapeamento autom�tico
- **Unit of Work**: Transa��es coordenadas

# PDPw - Programa��o Di�ria de Produ��o (Migra��o .NET 8 + React)

**Vers�o**: 2.0  
**Status**: ?? Em Desenvolvimento  
**Cliente**: ONS (Operador Nacional do Sistema El�trico)

---

## ?? Sobre o Projeto

Migra��o incremental do sistema PDPw de um legado .NET Framework 4.8/VB.NET com WebForms para uma arquitetura moderna usando:

- **Back-end**: .NET 8 com C# e ASP.NET Core Web API
- **Front-end**: React com TypeScript
- **Banco de Dados**: SQL Server (Entity Framework Core)
- **Infraestrutura**: Docker e Docker Compose
- **Testes**: xUnit (backend) + Jest (frontend)

---

## ?? In�cio R�pido

### Via Docker (Recomendado)
\\\ash
docker-compose up -d
# Backend: http://localhost:5000/swagger
# Frontend: http://localhost:3000
\\\

### Via Local
Consulte [QUICKSTART.md](QUICKSTART.md)

---

## ?? Progresso

### Backend APIs
- ? Usinas (8 endpoints)
- ? TiposUsina (6 endpoints)
- ? Empresas (8 endpoints)
- ? SemanasPMO (9 endpoints)
- ? EquipesPDP (8 endpoints)
- ? 24 APIs restantes

**Total**: 5/29 APIs (17.2%) | 39/154 endpoints (25.3%)

### Frontend
- ? Em desenvolvimento

### Testes
- ? A implementar

---

## ??? Arquitetura

Consulte [STRUCTURE.md](STRUCTURE.md) para detalhes da arquitetura.

---

## ?? Documenta��o

- [AGENTS.md](AGENTS.md) - Documenta��o para IA
- [STRUCTURE.md](STRUCTURE.md) - Estrutura do projeto
- [CONTRIBUTING.md](CONTRIBUTING.md) - Guia de contribui��o
- [QUICKSTART.md](QUICKSTART.md) - In�cio r�pido
- [docs/](docs/) - Documenta��o adicional

---

## ?? Contribuindo

Consulte [CONTRIBUTING.md](CONTRIBUTING.md)

---

## ?? Licen�a

Propriedade intelectual do ONS (Operador Nacional do Sistema El�trico Brasileiro).

---

**Desenvolvido com ?? por Willian + GitHub Copilot**

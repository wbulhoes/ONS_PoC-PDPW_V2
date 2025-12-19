# ?? GUIA DE CONTRIBUI��O

## Como Contribuir

### 1. Fork do Reposit�rio
\\\ash
git clone https://github.com/wbulhoes/ONS_PoC-PDPW.git
cd ONS_PoC-PDPW
\\\

### 2. Criar Branch
\\\ash
git checkout develop
git checkout -b feature/minha-feature
\\\

### 3. Fazer Altera��es
- Escreva c�digo limpo e bem documentado
- Siga os padr�es do projeto
- Adicione testes para novas funcionalidades

### 4. Commit
\\\ash
git add .
git commit -m "feat(escopo): descri��o da feature"
\\\

### 5. Push e Pull Request
\\\ash
git push origin feature/minha-feature
\\\

Abra um Pull Request no GitHub com descri��o detalhada.

## Padr�es de C�digo

### Backend (.NET 8)
- Use C# 12 features quando apropriado
- Siga conven��es de nomenclatura .NET
- Documente m�todos p�blicos com XML comments
- Mantenha m�todos pequenos e focados

### Frontend (React)
- Use Functional Components e Hooks
- Tipagem forte com TypeScript
- Props interface para cada componente
- CSS Modules para estilos

### Testes
- Cobertura m�nima de 80%
- Testes unit�rios para l�gica de neg�cio
- Testes de integra��o para APIs
- Nomenclatura: NomeDoMetodo_Cenario_ResultadoEsperado

## Checklist de PR

- [ ] C�digo compila sem erros
- [ ] Testes passam
- [ ] Documenta��o atualizada
- [ ] Commits seguem padr�o conventional
- [ ] Branch est� atualizada com develop
- [ ] Code review solicitado

## Reportar Bugs

Use as Issues do GitHub com:
- Descri��o clara do problema
- Passos para reproduzir
- Comportamento esperado vs atual
- Screenshots se aplic�vel
- Vers�o do ambiente

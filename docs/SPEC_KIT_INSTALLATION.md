# ?? Guia de Instalação do Spec Kit

## ?? O Que é Spec Kit?

**Spec Kit** é uma ferramenta da GitHub para criar e gerenciar especificações técnicas estruturadas de projetos de software.

---

## ? Alternativa: Especificação Manual (JÁ CRIADA)

**Boa notícia!** Já criei uma especificação técnica completa para você em:

?? **`docs/TECHNICAL_SPEC.md`**

Esta especificação contém:
- ? Visão geral do projeto
- ? Arquitetura detalhada
- ? Endpoints da API
- ? Modelo de dados
- ? Configurações
- ? Roadmap
- ? E muito mais!

**Você pode usar esta especificação sem precisar instalar o Spec Kit!**

---

## ??? Instalar Spec Kit (Opcional)

Se ainda quiser instalar o Spec Kit oficial, siga estes passos:

### Pré-requisitos

1. **Python 3.11+**
2. **UV** (gerenciador de pacotes Python moderno)

---

### ?? Passo 1: Instalar Python

#### Opção A: Via Microsoft Store (Mais Fácil)

```powershell
# Abre a Microsoft Store e busca por "Python 3.12"
start ms-windows-store://search/?query=Python
```

#### Opção B: Via Instalador Oficial

1. Acesse: https://www.python.org/downloads/
2. Baixe Python 3.12 ou superior
3. **IMPORTANTE**: Marque "Add Python to PATH" durante a instalação

#### Opção C: Via winget

```powershell
# Requer Windows Package Manager
winget install Python.Python.3.12
```

### Verificar Instalação

```powershell
# Fechar e reabrir o terminal, depois:
python --version
# Deve mostrar: Python 3.12.x

pip --version
# Deve mostrar versão do pip
```

---

### ?? Passo 2: Instalar UV

```powershell
# Opção A: Via pip
pip install uv

# Opção B: Via instalador PowerShell
irm https://astral.sh/uv/install.ps1 | iex

# Opção C: Via Cargo (se tiver Rust)
cargo install uv
```

### Verificar Instalação do UV

```powershell
uv --version
# Deve mostrar a versão do UV
```

---

### ?? Passo 3: Executar Spec Kit

```powershell
# Navegar para o diretório do projeto
cd C:\temp\_ONS_PoC-PDPW

# Executar Spec Kit
uvx --from git+https://github.com/github/spec-kit.git specify init ONS_PoC-PDPW

# Ou se preferir nome diferente
uvx --from git+https://github.com/github/spec-kit.git specify init PDPW
```

### O Que Vai Acontecer

O Spec Kit irá criar:
- ?? Pasta `.spec/` com estrutura de especificação
- ?? Arquivo `spec.yaml` com configurações
- ?? Templates de especificação

---

## ?? Problemas Comuns

### Erro: "Python was not found"

**Solução:**
1. Instalar Python (ver Passo 1)
2. Reiniciar terminal
3. Verificar com `python --version`

### Erro: "uv: command not found"

**Solução:**
1. Instalar UV (ver Passo 2)
2. Reiniciar terminal
3. Verificar com `uv --version`

### Erro: "Permission denied"

**Solução:**
```powershell
# Executar PowerShell como Administrador
# Depois executar os comandos de instalação
```

### Erro: "SSL Certificate verification failed"

**Solução:**
```powershell
# Configurar certificados
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org uv
```

---

## ?? Alternativas ao Spec Kit

Se você não quiser instalar o Spec Kit, pode usar:

### 1. **Especificação Manual** (JÁ CRIADA) ? Recomendado

? **Já disponível em**: `docs/TECHNICAL_SPEC.md`

**Vantagens:**
- Zero instalação necessária
- Totalmente customizável
- Formato Markdown simples
- Integrado ao projeto

### 2. **GitHub Wiki**

```powershell
# Acessar Wiki do repositório
start https://github.com/wbulhoes/ONS_PoC-PDPW/wiki
```

**Vantagens:**
- Hospedado no GitHub
- Fácil edição online
- Versionamento automático

### 3. **README.md Estendido**

Usar o próprio README.md com seções detalhadas.

### 4. **Confluence/SharePoint**

Para ambientes corporativos.

---

## ?? Comparação

| Ferramenta | Instalação | Colaboração | Estrutura | Recomendação |
|------------|-----------|-------------|-----------|--------------|
| **Spec Kit** | Complexa | ??? | ????? | Se já usa Python |
| **Manual (MD)** | Nenhuma | ???? | ???? | ? **Melhor para você** |
| **GitHub Wiki** | Nenhuma | ????? | ??? | Colaboração online |
| **README.md** | Nenhuma | ???? | ?? | Projetos pequenos |

---

## ? Recomendação Final

Para o seu projeto PDPW, **recomendo usar a especificação manual** que já criei:

?? **`docs/TECHNICAL_SPEC.md`**

**Por quê?**
1. ? **Já está pronto** - Zero configuração necessária
2. ? **Totalmente customizado** para seu projeto
3. ? **Formato Markdown** - Fácil de editar e versionar
4. ? **Integrado ao Git** - Versionamento automático
5. ? **Sem dependências** - Não precisa Python/UV

---

## ?? Como Usar a Especificação Manual

### Visualizar

```powershell
# Abrir no VS Code
code docs\TECHNICAL_SPEC.md

# Abrir no navegador (via GitHub)
start https://github.com/wbulhoes/ONS_PoC-PDPW/blob/develop/docs/TECHNICAL_SPEC.md
```

### Editar

1. Abra `docs/TECHNICAL_SPEC.md`
2. Edite em Markdown
3. Commit no Git

### Manter Atualizado

```powershell
# Adicionar ao Git
git add docs/TECHNICAL_SPEC.md
git commit -m "docs: atualizar especificação técnica"
git push origin develop
```

---

## ?? Recursos Adicionais

### Documentação Spec Kit
- https://github.com/github/spec-kit

### Markdown Guide
- https://www.markdownguide.org/

### GitHub Flavored Markdown
- https://github.github.com/gfm/

---

## ? Conclusão

**Você já tem tudo que precisa!**

A especificação técnica em `docs/TECHNICAL_SPEC.md` fornece:
- Documentação completa
- Estrutura profissional
- Fácil manutenção
- Zero dependências

**Não é necessário instalar Spec Kit!** ??

---

**Dúvidas?** Veja a especificação criada ou me pergunte!

# ?? Guia de Instala��o do Spec Kit

## ?? O Que � Spec Kit?

**Spec Kit** � uma ferramenta da GitHub para criar e gerenciar especifica��es t�cnicas estruturadas de projetos de software.

---

## ? Alternativa: Especifica��o Manual (J� CRIADA)

**Boa not�cia!** J� criei uma especifica��o t�cnica completa para voc� em:

?? **`docs/TECHNICAL_SPEC.md`**

Esta especifica��o cont�m:
- ? Vis�o geral do projeto
- ? Arquitetura detalhada
- ? Endpoints da API
- ? Modelo de dados
- ? Configura��es
- ? Roadmap
- ? E muito mais!

**Voc� pode usar esta especifica��o sem precisar instalar o Spec Kit!**

---

## ??? Instalar Spec Kit (Opcional)

Se ainda quiser instalar o Spec Kit oficial, siga estes passos:

### Pr�-requisitos

1. **Python 3.11+**
2. **UV** (gerenciador de pacotes Python moderno)

---

### ?? Passo 1: Instalar Python

#### Op��o A: Via Microsoft Store (Mais F�cil)

```powershell
# Abre a Microsoft Store e busca por "Python 3.12"
start ms-windows-store://search/?query=Python
```

#### Op��o B: Via Instalador Oficial

1. Acesse: https://www.python.org/downloads/
2. Baixe Python 3.12 ou superior
3. **IMPORTANTE**: Marque "Add Python to PATH" durante a instala��o

#### Op��o C: Via winget

```powershell
# Requer Windows Package Manager
winget install Python.Python.3.12
```

### Verificar Instala��o

```powershell
# Fechar e reabrir o terminal, depois:
python --version
# Deve mostrar: Python 3.12.x

pip --version
# Deve mostrar vers�o do pip
```

---

### ?? Passo 2: Instalar UV

```powershell
# Op��o A: Via pip
pip install uv

# Op��o B: Via instalador PowerShell
irm https://astral.sh/uv/install.ps1 | iex

# Op��o C: Via Cargo (se tiver Rust)
cargo install uv
```

### Verificar Instala��o do UV

```powershell
uv --version
# Deve mostrar a vers�o do UV
```

---

### ?? Passo 3: Executar Spec Kit

```powershell
# Navegar para o diret�rio do projeto
cd C:\temp\_ONS_PoC-PDPW

# Executar Spec Kit
uvx --from git+https://github.com/github/spec-kit.git specify init ONS_PoC-PDPW

# Ou se preferir nome diferente
uvx --from git+https://github.com/github/spec-kit.git specify init PDPW
```

### O Que Vai Acontecer

O Spec Kit ir� criar:
- ?? Pasta `.spec/` com estrutura de especifica��o
- ?? Arquivo `spec.yaml` com configura��es
- ?? Templates de especifica��o

---

## ?? Problemas Comuns

### Erro: "Python was not found"

**Solu��o:**
1. Instalar Python (ver Passo 1)
2. Reiniciar terminal
3. Verificar com `python --version`

### Erro: "uv: command not found"

**Solu��o:**
1. Instalar UV (ver Passo 2)
2. Reiniciar terminal
3. Verificar com `uv --version`

### Erro: "Permission denied"

**Solu��o:**
```powershell
# Executar PowerShell como Administrador
# Depois executar os comandos de instala��o
```

### Erro: "SSL Certificate verification failed"

**Solu��o:**
```powershell
# Configurar certificados
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org uv
```

---

## ?? Alternativas ao Spec Kit

Se voc� n�o quiser instalar o Spec Kit, pode usar:

### 1. **Especifica��o Manual** (J� CRIADA) ? Recomendado

? **J� dispon�vel em**: `docs/TECHNICAL_SPEC.md`

**Vantagens:**
- Zero instala��o necess�ria
- Totalmente customiz�vel
- Formato Markdown simples
- Integrado ao projeto

### 2. **GitHub Wiki**

```powershell
# Acessar Wiki do reposit�rio
start https://github.com/wbulhoes/ONS_PoC-PDPW/wiki
```

**Vantagens:**
- Hospedado no GitHub
- F�cil edi��o online
- Versionamento autom�tico

### 3. **README.md Estendido**

Usar o pr�prio README.md com se��es detalhadas.

### 4. **Confluence/SharePoint**

Para ambientes corporativos.

---

## ?? Compara��o

| Ferramenta | Instala��o | Colabora��o | Estrutura | Recomenda��o |
|------------|-----------|-------------|-----------|--------------|
| **Spec Kit** | Complexa | ??? | ????? | Se j� usa Python |
| **Manual (MD)** | Nenhuma | ???? | ???? | ? **Melhor para voc�** |
| **GitHub Wiki** | Nenhuma | ????? | ??? | Colabora��o online |
| **README.md** | Nenhuma | ???? | ?? | Projetos pequenos |

---

## ? Recomenda��o Final

Para o seu projeto PDPW, **recomendo usar a especifica��o manual** que j� criei:

?? **`docs/TECHNICAL_SPEC.md`**

**Por qu�?**
1. ? **J� est� pronto** - Zero configura��o necess�ria
2. ? **Totalmente customizado** para seu projeto
3. ? **Formato Markdown** - F�cil de editar e versionar
4. ? **Integrado ao Git** - Versionamento autom�tico
5. ? **Sem depend�ncias** - N�o precisa Python/UV

---

## ?? Como Usar a Especifica��o Manual

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
git commit -m "docs: atualizar especifica��o t�cnica"
git push origin develop
```

---

## ?? Recursos Adicionais

### Documenta��o Spec Kit
- https://github.com/github/spec-kit

### Markdown Guide
- https://www.markdownguide.org/

### GitHub Flavored Markdown
- https://github.github.com/gfm/

---

## ? Conclus�o

**Voc� j� tem tudo que precisa!**

A especifica��o t�cnica em `docs/TECHNICAL_SPEC.md` fornece:
- Documenta��o completa
- Estrutura profissional
- F�cil manuten��o
- Zero depend�ncias

**N�o � necess�rio instalar Spec Kit!** ??

---

**D�vidas?** Veja a especifica��o criada ou me pergunte!

# Scripts de Desenvolvimento - PDPW PoC

## 🚀 Como usar

### Opção 1: PowerShell (Recomendado - mais funcionalidades)

```powershell
# Abrir ambiente completo (VS + Terminal Frontend)
.\Start-Development.ps1

# Opções avançadas:

# Pular verificação de npm install
.\Start-Development.ps1 -SkipNpmInstall

# Abrir apenas VS (sem terminal frontend)
.\Start-Development.ps1 -NoFrontend
```

**Pré-requisito:** Permitir scripts PowerShell:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Opção 2: Batch (Simples - Windows Cmd)

```cmd
# Duplo clique em Start-Development.bat
# Ou execute:
Start-Development.bat
```

---

## 📋 O que cada script faz

### `Start-Development.ps1` (PowerShell)
✅ Abre Visual Studio Community com `PDPW.sln`  
✅ Abre Windows Terminal (ou PowerShell) na pasta `frontend`  
✅ Verifica se `node_modules` existe  
✅ Exibe instruções claras de próximos passos  
✅ Suporta parâmetros avançados  

### `Start-Development.bat` (Batch)
✅ Abre Visual Studio Community com `PDPW.sln`  
✅ Abre cmd/Windows Terminal na pasta `frontend`  
✅ Simples e direto (sem dependências PowerShell)  

---

## 🎯 Fluxo típico de desenvolvimento

### Primeira execução:
```powershell
.\Start-Development.ps1
```

No terminal frontend que abre:
```powershell
npm install
npm run dev
```

### Próximas execuções:
```powershell
.\Start-Development.ps1 -SkipNpmInstall
```

---

## ✅ Resultado esperado

Após executar um dos scripts:

1. **Visual Studio Community** abre com a solução `PDPW.sln`
2. **Terminal** abre na pasta `frontend`
3. Você vê as instruções:
   ```
   1️⃣  No Visual Studio: defina 'PDPW.API' como projeto de inicialização e pressione F5
   2️⃣  No terminal frontend: execute 'npm run dev'
   3️⃣  Acesse:
      - Backend API: http://localhost:5000/swagger
      - Frontend: http://localhost:5173
   ```

---

## 🐛 Troubleshooting

### Visual Studio não abre
- Verifique se `devenv.exe` está no PATH
- Instale Visual Studio Community: https://visualstudio.microsoft.com/pt-br/vs/community/

### Terminal frontend não abre
- Tente usar `Start-Development.bat` (mais compatível)
- Ou execute manualmente:
  ```powershell
  cd frontend
  npm run dev
  ```

### Erro de permissão PowerShell
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

---

## 📝 Notas

- Os scripts abrem em paralelo (VS + Terminal simultâneos)
- Nenhum comando é executado automaticamente (segurança)
- Você mantém controle total do que rodar

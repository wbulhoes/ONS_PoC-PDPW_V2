# 🚀 Guia Rápido - Frontend PDPw

## Início Rápido (5 minutos)

### 1️⃣ Pré-requisitos
```bash
# Verificar versões
node --version  # >= 18.0.0
npm --version   # >= 9.0.0
```

### 2️⃣ Instalação
```bash
# Clonar repositório
git clone https://github.com/wbulhoes/ONS_PoC-PDPW_V2.git
cd ONS_PoC-PDPW_V2/frontend

# Instalar dependências
npm install

# Configurar variáveis de ambiente
cp .env.example .env
```

### 3️⃣ Executar
```bash
# Iniciar frontend (porta 5173)
npm run dev

# Em outro terminal, iniciar backend (porta 5001)
cd ../src/PDPW.API
dotnet run
```

### 4️⃣ Acessar
- **Frontend:** http://localhost:5173
- **Backend API:** http://localhost:5001/swagger

---

## 🎯 Estrutura de Pastas

```
frontend/src/
├── pages/              # Páginas principais (9 etapas)
├── components/         # Componentes reutilizáveis
├── services/           # APIs e comunicação backend
├── types/              # TypeScript types/interfaces
├── App.tsx             # Componente raiz
└── main.tsx            # Entry point
```

---

## 📋 Etapas Implementadas

| # | Etapa | Status | Rota |
|---|-------|--------|------|
| 0 | Dashboard | ✅ | `/` |
| 1 | Dados Energéticos | ✅ | `/dados-energeticos` |
| 2 | Programação Elétrica | ✅ | `/programacao-eletrica` |
| 3 | Previsão Eólica | ✅ | `/previsao-eolica` |
| 4 | Geração de Arquivos | ✅ | `/geracao-arquivos` |
| 5 | Finalização | 🚧 | `/finalizacao` |
| 6 | Insumos Agentes | 🚧 | `/insumos-agentes` |
| 7 | Ofertas Térmicas | 🚧 | `/ofertas-exportacao` |
| 8 | Ofertas RV | 🚧 | `/ofertas-rv` |
| 9 | Energia Vertida | 🚧 | `/energia-vertida` |

✅ Completo | 🚧 Em desenvolvimento

---

## 🔌 APIs Disponíveis

### Dados Energéticos
```typescript
import { dadosEnergeticosService } from './services';

// Listar todos
const dados = await dadosEnergeticosService.obterTodos();

// Criar novo
await dadosEnergeticosService.criar({
  dataReferencia: '2025-01-15',
  codigoUsina: 'ITB001',
  producaoMWh: 14000,
  capacidadeDisponivel: 14000,
  status: 'PLANEJADO'
});
```

### Programação Elétrica
```typescript
import { cargasService, intercambiosService } from './services';

// Cargas por semana
const cargas = await cargasService.obterPorSemana(semanaPmoId);

// Intercâmbios entre subsistemas
const inter = await intercambiosService.obterPorSubsistemas('SE', 'S');
```

### Previsão Eólica
```typescript
import { previsoesEolicasService } from './services';

// Criar previsão
await previsoesEolicasService.criar({
  parqueEolicoId: 1,
  semanaPmoId: 108,
  dataPrevisao: '2025-01-15',
  potenciaPrevistoMW: 850,
  fatorCapacidade: 42.5
});
```

### Arquivos DADGER
```typescript
import { arquivosDadgerService } from './services';

// Gerar arquivo
await arquivosDadgerService.gerar(semanaPmoId);

// Aprovar arquivo
await arquivosDadgerService.aprovar(arquivoId);

// Download
const blob = await arquivosDadgerService.download(arquivoId);
```

---

## 🎨 Componentes Padrão

### Criar Nova Página

```typescript
// src/pages/MinhaEtapa.tsx
import React, { useState, useEffect } from 'react';
import { meuService } from '../services';
import { MeuDto } from '../types';
import styles from './MinhaEtapa.module.css';

const MinhaEtapa: React.FC = () => {
  const [dados, setDados] = useState<MeuDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      setLoading(true);
      const resultado = await meuService.obterTodos();
      setDados(resultado);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return <div className={styles.loading}>Carregando...</div>;

  return (
    <div className={styles.container}>
      <h2>Minha Etapa</h2>
      {/* Seu conteúdo aqui */}
    </div>
  );
};

export default MinhaEtapa;
```

### Adicionar Rota

```typescript
// App.tsx
import MinhaEtapa from './pages/MinhaEtapa';

// Dentro de <Routes>
<Route path="/minha-etapa" element={<MinhaEtapa />} />
```

---

## 🛠️ Comandos Úteis

```bash
# Desenvolvimento
npm run dev              # Iniciar dev server
npm run build            # Build produção
npm run preview          # Preview do build
npm run type-check       # Verificar tipos TypeScript
npm run lint             # Verificar código

# Limpeza
npm run clean            # Limpar dist e node_modules
rm -rf node_modules && npm install  # Reinstalar dependências
```

---

## 🐛 Troubleshooting

### Erro: "Module not found"
```bash
# Limpar cache e reinstalar
rm -rf node_modules package-lock.json
npm install
```

### Erro: "CORS"
- Verificar se backend está rodando (porta 5001)
- Verificar CORS configurado no backend (`Program.cs`)
- Conferir variável `VITE_API_URL` no `.env`

### Erro: "Port 5173 already in use"
```bash
# Matar processo na porta 5173
npx kill-port 5173

# Ou usar outra porta
npm run dev -- --port 3000
```

### Backend não responde
```bash
# Verificar se backend está rodando
curl http://localhost:5001/health

# Iniciar backend
cd ../src/PDPW.API
dotnet run
```

---

## 📊 Dados de Teste

O backend já possui 857 registros de teste:
- 10 Usinas (Itaipu, Belo Monte, Tucuruí, etc.)
- 100 Unidades Geradoras
- 108 Semanas PMO (2024-2026)
- 240 Intercâmbios
- 120 Cargas e Balanços

Para resetar dados:
```bash
cd ../src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
```

---

## 🔐 Autenticação (Próxima Fase)

Estrutura preparada para JWT:
```typescript
// apiClient.ts já tem interceptor configurado
const token = localStorage.getItem('token');
if (token) {
  config.headers.Authorization = `Bearer ${token}`;
}
```

---

## 📱 Testar Responsividade

```bash
# Chrome DevTools
F12 → Toggle Device Toolbar (Ctrl+Shift+M)

# Tamanhos comuns
- Mobile: 375x667 (iPhone SE)
- Tablet: 768x1024 (iPad)
- Desktop: 1920x1080
```

---

## 🚀 Deploy (Próxima Fase)

### Build para Produção
```bash
npm run build
# Gera pasta dist/ com arquivos otimizados
```

### Servir Build
```bash
npm run preview
# Ou usar servidor de produção (nginx, Apache)
```

---

## 📞 Suporte

- **Issues:** Use o GitHub Issues
- **Documentação:** Consulte `frontend/README.md`
- **Backend:** Veja `README_BACKEND.md`

---

**PDPw v2.0** - React + TypeScript + .NET 8
Desenvolvido para ONS - Operador Nacional do Sistema Elétrico

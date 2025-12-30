# Análise do Frontend — POC de Migração e Integração com Backend

Resumo do panorama das páginas migradas no frontend e status da integração com o backend (.NET API). Destaque para as páginas já configuradas e funcionando com chamadas reais à API (usando `apiClient` / `api` / `axios` apontando para `VITE_API_BASE_URL`).

Observação: este arquivo foi gerado automaticamente a partir dos arquivos em `frontend/src`.

---

## Panorama geral
- Local das páginas: `frontend/src/pages` e subpastas.
- Cliente HTTP central: `frontend/src/services/apiClient.ts` e `frontend/src/services/api.ts` (axios). Ambos usam `VITE_API_BASE_URL` para apontar à API (.NET).
- Padrão de integração: services em `frontend/src/services` fazem chamadas ao backend; páginas consomem esses services.
- Existem services com implementação real (`apiClient` / `axios`) e também alguns services/mocks locais usados para POC.

---

## Páginas com integração configurada e funcionando (evidências: chamam services que usam `apiClient`/`api`/`axios`)
Essas páginas já estão integradas ao backend na POC — podem ser testadas quando a API estiver rodando e a variável `VITE_API_BASE_URL` configurada.

- `EnergiaVertidaManagement`
  - Path: `frontend/src/pages/EnergiaVertida/EnergiaVertidaManagement.tsx`
  - Service: `frontend/src/services/energiaVertidaService.ts` (usa `axios` com `VITE_API_BASE_URL`)
  - Endpoints consumidos: `/energia-vertida` (GET, POST, PUT, DELETE)

- Páginas de Empresas / Administração (Company)
  - Paths: `frontend/src/pages/Administration/Company.tsx`, `frontend/src/pages/Admin/Company/CompanyManagement.tsx`, `.../CompanyRegistry.tsx`
  - Service: `frontend/src/services/empresaService.ts` e `frontend/src/services/companyService.ts` (algumas variações) — usam `apiClient` → endpoint `/empresas`

- Cargas (dados de carga)
  - Services: `frontend/src/services/cargaService.ts` (usa `apiClient` → `/cargas`)
  - Páginas relacionadas: `frontend/src/pages/Collection/Load/*` (ex.: `EstimatedLoad.tsx`, `Consumption.tsx`) — muitas consomem hooks/services que usam `apiClient`.

- Dados Energéticos
  - Components/pages: `frontend/src/components/DadosEnergeticosForm.tsx`, `frontend/src/components/DadosEnergeticosLista.tsx` e possivelmente `frontend/src/pages/*` que usam `energeticService`.
  - Service: `frontend/src/services/energeticService.ts` (usa `apiClient`)

- Ofertas de Exportação
  - Service: `frontend/src/services/ofertaExportacaoService.ts` (usa `apiClient` → `/ofertas-exportacao`)
  - Páginas relacionadas (migradas na POC): `frontend/src/pages/Collection/Thermal/ExportOffer.tsx`, `ExportOfferAnalysis.tsx` (algumas implementações usam mocks — ver nota abaixo)

- Ofertas de Resposta Voluntária
  - Service: `frontend/src/services/*` (há implementação `ofertaResposta` / `exportOfferService` / `exportOffer` helper) — `ofertaExportacaoService.ts` e variantes indicam endpoints `/ofertas-exportacao` e paths similares. Páginas em `frontend/src/pages` consomem esses serviços.

- Previsões Eólicas (integração presente nos services)
  - Page: `frontend/src/pages/Collection/Electrical/PrevisaoEolica.tsx`
  - Service: `frontend/src/services/previsaoEolicaService.ts` (usa `apiClient` e expõe métodos CRUD para `/previsoes-eolicas`)
  - Observação: a página usa `getPrevisao(...)` em algumas implementações — validar método concreto (há pequenas variações entre mock e service real). Em geral a infraestrutura para integração já existe.

- Intercâmbios, Balanços, Arquivos DADGER, Semanas PMO, Usinas, Unidades Geradoras, Tipos de Usina, Restrição UG, Paradas UG, MotivosRestricao
  - Services: `intercambioService`, `balancoService`, `arquivosDadgerService` (ou equivalentes) e `apiClient` indicam endpoints correspondentes em backend (controladores .NET já implementados).
  - Páginas: muitas em `frontend/src/pages/Collection/*` e `frontend/src/pages/*` consomem esses services. Verificar arquivos concretos para cada área.

---

## Páginas migradas mas com integração parcial / mocks (precisam validação ou adaptação)
- `ProgramacaoEnergetica` (`frontend/src/pages/Collection/Electrical/ProgramacaoEnergetica.tsx`)
  - Usa `programacaoService` que, hoje, é um serviço POC com dados simulados (mock) — não realiza chamadas à API real.
  - Status: migrada, UI funcional, mas integrar ao endpoint real caso backend possua rota equivalente.

- `exportOfferService` (mock) vs `ofertaExportacaoService` (real)
  - Existem implementações mock em `frontend/src/services/exportOfferService.ts` e implementações reais `ofertaExportacaoService.ts`. Algumas páginas podem ainda estar apontando para a versão mock. Necessita padronização para `apiClient`.

- Alguns formulários e páginas novas (indicadas nos scripts como "NOVA") podem ter sido implementadas apenas com mocks e precisam apontar para os endpoints adequados:
  - `FinalizacaoProgramacao.tsx`
  - `OfertasExportacao.tsx` (verificar se consome `ofertaExportacaoService` real)
  - `OfertasRespostaVoluntaria.tsx`
  - `EnergiaVertida.tsx` (já existe `EnergiaVertidaManagement` que integra)

---

## Páginas migradas mas que precisam de verificação manual (lista curta para checagem rápida)
- `RROQuery` (`frontend/src/pages/Query/Other/RROQuery.tsx`) — chama `rroService.getOffers(...)` (verificar se `rroService` usa `apiClient` ou mock).
- `Collection` pages: Availability, Balance, Energy, Consumption, EstimatedGeneration, EstimatedInterchange — checar cada página para garantir que chamam os services que usam `apiClient`.

---

## Testes e evidências automatizadas
- Existem testes unitários/integração de serviços em `frontend/tests/services/*` (ex.: `companyService.test.ts`, `previsaoEolicaService.test.ts`, `insumosService.test.ts`, `ir1Service.test.ts`). Esses testes mostram que os services foram contemplados e têm contratos esperados.
- Serviços centrais com `apiClient` (fetch wrapper) e `api.ts` (axios) implementam tratamento de erros e uso de `VITE_API_BASE_URL`.

---

## Checklist rápido para validar integração localmente
1. Backend: executar API (.NET 8)
   - Ex.: `cd src/PDPW.API && dotnet run` → por padrão expõe `/swagger` e endpoints descritos nas controllers.
2. Frontend: configurar `VITE_API_BASE_URL` no `.env` (ex.: `VITE_API_BASE_URL=http://localhost:5001/api`) e iniciar:
   - `cd frontend && npm install && npm run dev`
3. Testar páginas integradas listadas na seção "funcionando" (Energia Vertida, Empresas, Cargas, Dados Energéticos, OfertasExportacao, PrevisoesEolicas, etc.)
4. Rodar testes de services (Vitest): `cd frontend && npm test` — olhar por falhas que indiquem endpoints inconsistentes
5. Se alguma página usar mock (ex.: `programacaoService`), substituir pela chamada real para o endpoint correspondente ou adaptar rota/backend.

---

## Recomendações (próximos passos)
- Padronizar services para usar `apiClient` (ou `api.ts`) em toda a base — remover duplicidades (mocks vs real) onde possível.
- Atualizar páginas que ainda usam mocks para apontarem aos endpoints reais do backend, ou fornecer um adapter que escolha mock em ambiente de desenvolvimento.
- Executar testes de contrato entre frontend e backend (API contract tests) para detectar diferenças de payload/rotas.
- Documentar em `FRONTEND_COMPLETO_9_ETAPAS.md` ou similar as páginas que já estão 100% integradas.

---

Arquivo gerado automaticamente a partir do código em `frontend/src` — revise e ajuste se quiser incluir uma lista completa de todas as páginas (posso gerar essa lista se desejar).

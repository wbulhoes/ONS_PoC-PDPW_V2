# APIs, Controllers e Endpoints

Resumo das controllers implementadas na API PDPW e seus endpoints (rotas, verbos HTTP e descrição curta).

> Observação: rotas base usam o padrão `api/[controller]` salvo quando indicado explicitamente.

---

## ArquivosDadgerController
Base: `api/arquivosdadger`
- `GET /` - Listar todos os arquivos DADGER
- `GET /{id}` - Obter arquivo por ID
- `GET /semana/{semanaPMOId}` - Arquivos por Semana PMO
- `GET /processados?processado={bool}` - Filtrar por processado
- `GET /periodo?dataInicio={}&dataFim={}` - Arquivos em período
- `GET /nome/{nomeArquivo}` - Obter por nome de arquivo
- `POST /` - Criar novo arquivo DADGER
- `PUT /{id}` - Atualizar arquivo
- `PATCH /{id}/processar` - Marcar como processado
- `DELETE /{id}` - Remover (soft delete)
- `GET /status/{status}` - Filtrar por status (Aberto, EmAnalise, Aprovado)
- `GET /pendentes-aprovacao` - Arquivos pendentes de aprovação
- `POST /{id}/finalizar` - Finalizar programação
- `POST /{id}/aprovar` - Aprovar programação
- `POST /{id}/reabrir` - Reabrir programação

---

## BalancosController
Base: `api/balancos`
- `GET /` - Listar todos os balanços energéticos
- `GET /{id}` - Obter balanço por ID
- `GET /subsistema/{subsistemaId}` - Balanços por subsistema
- `GET /data/{data}` - Balanços por data
- `GET /periodo?dataInicio={}&dataFim={}` - Balanços em período
- `GET /subsistema/{subsistemaId}/data/{data}` - Balanço por subsistema e data
- `POST /` - Criar balanço
- `PUT /{id}` - Atualizar balanço
- `DELETE /{id}` - Remover balanço

---

## CargasController
Base: `api/cargas`
- `GET /` - Listar todas as cargas
- `GET /{id}` - Obter carga por ID
- `GET /periodo?dataInicio={}&dataFim={}` - Cargas por período
- `GET /subsistema/{subsistemaId}` - Cargas por subsistema
- `GET /data/{dataReferencia}` - Cargas por data de referência
- `POST /` - Criar carga
- `PUT /{id}` - Atualizar carga
- `DELETE /{id}` - Remover carga

---

## DadosEnergeticosController
Base: `api/dadosenergeticos` (controller: `DadosEnergeticosController`)
- `GET /` - Obter todos os dados energéticos
- `GET /{id}` - Obter por ID
- `GET /periodo?dataInicio={}&dataFim={}` - Dados por período
- `POST /` - Criar dado energético
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## DashboardController
Base: `api/dashboard`
- `GET /resumo` - Resumo geral do sistema
- `GET /metricas/{categoria}` - Métricas por categoria
- `GET /alertas?prioridade={}` - Alertas do sistema

---

## EmpresasController
Base: `api/empresas`
- `GET /` - Listar empresas
- `GET /{id}` - Obter por ID
- `GET /nome/{nome}` - Obter por nome
- `GET /cnpj/{cnpj}` - Obter por CNPJ
- `POST /` - Criar empresa
- `PUT /{id}` - Atualizar empresa
- `DELETE /{id}` - Remover empresa
- `GET /verificar-nome/{nome}?empresaId={}` - Verificar existência de nome
- `GET /verificar-cnpj/{cnpj}?empresaId={}` - Verificar existência de CNPJ
- `GET /buscar?termo={}` - Buscar por termo (nome ou CNPJ)

---

## EquipesPdpController
Base: `api/equipespdp`
- `GET /` - Listar equipes PDP
- `GET /{id}` - Obter equipe por ID
- `GET /nome/{nome}` - Obter por nome
- `GET /{id}/membros` - Obter equipe com membros
- `POST /` - Criar equipe
- `PUT /{id}` - Atualizar equipe
- `DELETE /{id}` - Remover equipe
- `GET /verificar-nome?nome={}&equipePdpId={}` - Verificar nome

---

## IntercambiosController
Base: `api/intercambios`
- `GET /` - Listar intercâmbios
- `GET /{id}` - Obter por ID
- `GET /origem/{subsistemaOrigem}` - Origem
- `GET /destino/{subsistemaDestino}` - Destino
- `GET /subsistema?origem={}&destino={}` - Filtrar por subsistemas
- `GET /data/{data}` - Por data
- `GET /periodo?dataInicio={}&dataFim={}` - Por período
- `GET /origem/{origem}/destino/{destino}/data/{data}` - Intercâmbio específico por data
- `POST /` - Criar intercâmbio
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## MotivosRestricaoController
Base: `api/motivosrestricao`
- `GET /` - Listar motivos
- `GET /{id}` - Obter por ID
- `GET /categoria/{categoria}` - Filtrar por categoria
- `GET /ativos` - Motivos ativos
- `POST /` - Criar motivo
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## OfertasExportacaoController
Base: `api/ofertas-exportacao`
- `GET /` - Listar ofertas de exportação
- `GET /{id}` - Obter por ID
- `GET /pendentes` - Pendentes de análise
- `GET /aprovadas` - Aprovadas
- `GET /rejeitadas` - Rejeitadas
- `GET /usina/{usinaId}` - Por usina
- `GET /data-pdp/{dataPDP}` - Por data PDP
- `GET /periodo?dataInicio={}&dataFim={}` - Por período
- `POST /` - Criar oferta
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover
- `POST /{id}/aprovar` - Aprovar oferta
- `POST /{id}/rejeitar` - Rejeitar oferta
- `GET /validar-pendente/{dataPDP}` - Verifica pendência para data PDP
- `GET /permite-exclusao/{dataPDP}` - Verifica permissão de exclusão

---

## OfertasRespostaVoluntariaController
Base: `api/ofertas-resposta-voluntaria`
- `GET /` - Listar ofertas de resposta voluntária
- `GET /{id}` - Obter por ID
- `GET /pendentes` - Pendentes
- `GET /aprovadas` - Aprovadas
- `GET /rejeitadas` - Rejeitadas
- `GET /empresa/{empresaId}` - Por empresa
- `GET /data-pdp/{dataPDP}` - Por data PDP
- `GET /tipo-programa/{tipoPrograma}` - Por tipo de programa
- `POST /` - Criar oferta
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover
- `POST /{id}/aprovar` - Aprovar
- `POST /{id}/rejeitar` - Rejeitar

---

## ParadasUGController
Base: `api/paradasug`
- `GET /` - Listar paradas UG
- `GET /{id}` - Obter por ID
- `GET /unidade/{unidadeGeradoraId}` - Por unidade geradora
- `GET /programadas` - Paradas programadas
- `GET /nao-programadas` - Paradas não programadas (emergenciais)
- `GET /ativas?dataReferencia={}` - Paradas ativas em data
- `GET /periodo?dataInicio={}&dataFim={}` - Por período
- `POST /` - Criar parada
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## PrevisoesEolicasController
Base: `api/previsoes-eolicas`
- `GET /` - Listar previsões eólicas
- `GET /{id}` - Obter por ID
- `GET /usina/{usinaId}` - Previsões por usina
- `GET /usina/{usinaId}/ultimas?quantidade={}` - Últimas previsões
- `GET /periodo?dataInicio={}&dataFim={}` - Por período
- `GET /modelo/{modelo}` - Filtrar por modelo
- `GET /usina/{usinaId}/estatisticas?dataInicio={}&dataFim={}` - Estatísticas de acurácia
- `POST /` - Criar previsão
- `PUT /{id}` - Atualizar previsão
- `PATCH /{id}/geracao-real` - Atualizar geração real verificada
- `DELETE /{id}` - Remover previsão

---

## RestricoesUGController
Base: `api/restricoesug`
- `GET /` - Listar restrições UG
- `GET /{id}` - Obter por ID
- `GET /unidade/{unidadeGeradoraId}` - Por unidade geradora
- `GET /ativas?dataReferencia={}` - Ativas em data
- `GET /periodo?dataInicio={}&dataFim={}` - Por período
- `GET /motivo/{motivoRestricaoId}` - Por motivo
- `POST /` - Criar restrição
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## SemanasPmoController
Base: `api/semanaspmo`
- `GET /` - Listar semanas PMO
- `GET /{id}` - Obter por ID
- `GET /numero/{numero}/ano/{ano}` - Obter por número e ano
- `GET /ano/{ano}` - Semanas de um ano
- `GET /data/{data}` - Semana que contém a data
- `POST /` - Criar semana PMO
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover
- `GET /verificar-numero/{numero}/ano/{ano}?semanaPmoId={}` - Verificar existência
- `GET /atual` - Semana PMO atual (contendo hoje)
- `GET /proximas?quantidade={}` - Próximas N semanas

---

## TiposUsinaController
Base: `api/tiposusina`
- `GET /` - Listar tipos de usina
- `GET /buscar?termo={}` - Buscar por termo
- `GET /{id}` - Obter por ID
- `GET /nome/{nome}` - Obter por nome
- `POST /` - Criar tipo de usina
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover
- `GET /verificar-nome/{nome}?tipoUsinaId={}` - Verificar nome

---

## UnidadesGeradorasController
Base: `api/unidadesgeradoras`
- `GET /` - Listar unidades geradoras
- `GET /{id}` - Obter por ID
- `GET /codigo/{codigo}` - Obter por código
- `GET /usina/{usinaId}` - Unidades por usina
- `GET /status/{status}` - Filtrar por status
- `GET /ativas` - Unidades ativas
- `POST /` - Criar unidade
- `PUT /{id}` - Atualizar
- `DELETE /{id}` - Remover

---

## UsinasController
Base: `api/usinas`
- `GET /` - Listar usinas geradoras
- `GET /{id}` (`GetUsinaById`) - Obter por ID
- `GET /codigo/{codigo}` - Obter por código
- `GET /tipo/{tipoUsinaId}` - Obter por tipo de usina
- `GET /empresa/{empresaId}` - Obter por empresa
- `POST /` - Criar usina
- `PUT /{id}` - Atualizar usina
- `DELETE /{id}` - Remover usina
- `GET /verificar-codigo/{codigo}?usinaId={}` - Verificar código

---

## UsuariosController
Base: `api/usuarios`
- `GET /` - Listar usuários ativos
- `GET /{id}` - Obter usuário por ID
- `GET /perfil/{perfil}` - Buscar por perfil
- `GET /equipe/{equipePdpId}` - Buscar por equipe PDP
- `GET /email/{email}` - Buscar por email
- `POST /` - Criar usuário
- `PUT /{id}` - Atualizar usuário
- `DELETE /{id}` - Remover usuário (soft delete)

---

Arquivo gerado automaticamente com base na leitura das controllers em `src/PDPW.API/Controllers`.

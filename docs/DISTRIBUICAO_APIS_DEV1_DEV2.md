# ?? DISTRIBUIÇÃO DE APIs - DEV 1 e DEV 2

**Data:** 19/12/2024  
**Total de APIs:** 29  
**DEV 1 (Backend Senior):** 14 APIs  
**DEV 2 (Backend Pleno):** 15 APIs  
**Critério:** Complexidade balanceada

---

## ?? RESUMO DA DISTRIBUIÇÃO

```
??????????????????????????????????????????????
? DEV 1 (Backend Senior)                     ?
? 14 APIs - Mais complexas                   ?
??????????????????????????????????????????????
? Gestão de Ativos:        5 APIs            ?
? Core System:             4 APIs            ?
? Consolidados:            3 APIs            ?
? Documentos:              2 APIs            ?
??????????????????????????????????????????????

??????????????????????????????????????????????
? DEV 2 (Backend Pleno)                      ?
? 15 APIs - Balanceadas                      ?
??????????????????????????????????????????????
? Dados e Arquivos:        6 APIs            ?
? Restrições e Operação:   5 APIs            ?
? Configurações:           4 APIs            ?
??????????????????????????????????????????????
```

---

## ????? DEV 1 (BACKEND SENIOR) - 14 APIs

### ?? CATEGORIA 1: GESTÃO DE ATIVOS (5 APIs)

#### 1. API USINA ?? COMPLEXA
**Branch:** `feature/gestao-ativos`  
**Prioridade:** ?? CRÍTICA (dia 1)  
**Tempo estimado:** 3h

**Entidade:**
```csharp
public class Usina : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int TipoUsinaId { get; set; }
    public TipoUsina? TipoUsina { get; set; }
    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
    public decimal CapacidadeInstalada { get; set; }
    public string? Localizacao { get; set; }
    public DateTime DataOperacao { get; set; }
    
    // Relacionamentos
    public ICollection<UnidadeGeradora>? UnidadesGeradoras { get; set; }
    public ICollection<RestricaoUS>? Restricoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/usinas
- GET /api/usinas/{id}
- GET /api/usinas/codigo/{codigo}
- POST /api/usinas
- PUT /api/usinas/{id}
- DELETE /api/usinas/{id}

**Seed Data:** 10 usinas (3 hidro, 3 térmicas, 2 eólicas, 2 solares)

---

#### 2. API EMPRESA ?? MÉDIA
**Prioridade:** ?? ALTA (dia 1)  
**Tempo estimado:** 2h

**Entidade:**
```csharp
public class Empresa : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? CNPJ { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    
    // Relacionamentos
    public ICollection<Usina>? Usinas { get; set; }
}
```

**Endpoints:** 5
- GET /api/empresas
- GET /api/empresas/{id}
- POST /api/empresas
- PUT /api/empresas/{id}
- DELETE /api/empresas/{id}

**Seed Data:** 8 empresas exemplo

---

#### 3. API TIPO USINA ?? SIMPLES
**Prioridade:** ?? MÉDIA (dia 1)  
**Tempo estimado:** 1.5h

**Entidade:**
```csharp
public class TipoUsina : BaseEntity
{
    public string Nome { get; set; } = string.Empty; // Hidrelétrica, Térmica, Eólica, Solar
    public string? Descricao { get; set; }
    public string? FonteEnergia { get; set; } // Água, Gás, Vento, Solar
    
    // Relacionamentos
    public ICollection<Usina>? Usinas { get; set; }
}
```

**Endpoints:** 4
- GET /api/tiposusina
- GET /api/tiposusina/{id}
- POST /api/tiposusina
- PUT /api/tiposusina/{id}

**Seed Data:** 5 tipos (Hidrelétrica, Térmica, Eólica, Solar, Nuclear)

---

#### 4. API SEMANA PMO ?? MÉDIA
**Prioridade:** ?? ALTA (dia 2)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class SemanaPMO : BaseEntity
{
    public int Numero { get; set; } // Número da semana PMO
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int Ano { get; set; }
    public string? Observacoes { get; set; }
    
    // Relacionamentos
    public ICollection<ArquivoDadger>? ArquivosDadger { get; set; }
}
```

**Endpoints:** 6
- GET /api/semanaspmo
- GET /api/semanaspmo/{id}
- GET /api/semanaspmo/ano/{ano}
- GET /api/semanaspmo/atual
- POST /api/semanaspmo
- PUT /api/semanaspmo/{id}

**Seed Data:** 20 semanas (ano atual + próximo)

---

#### 5. API EQUIPE PDP ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 2)  
**Tempo estimado:** 2h

**Entidade:**
```csharp
public class EquipePDP : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Coordenador { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    
    // Relacionamentos
    public ICollection<Usuario>? Membros { get; set; }
}
```

**Endpoints:** 5
- GET /api/equipespdp
- GET /api/equipespdp/{id}
- POST /api/equipespdp
- PUT /api/equipespdp/{id}
- DELETE /api/equipespdp/{id}

**Seed Data:** 5 equipes exemplo

---

### ?? CATEGORIA 2: DADOS CORE (4 APIs)

#### 6. API ARQUIVO DADGER ?? MUITO COMPLEXA
**Prioridade:** ?? CRÍTICA (dia 2)  
**Tempo estimado:** 4h

**Entidade:**
```csharp
public class ArquivoDadger : BaseEntity
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public DateTime DataImportacao { get; set; }
    public int SemanaPMOId { get; set; }
    public SemanaPMO? SemanaPMO { get; set; }
    public string? Observacoes { get; set; }
    public bool Processado { get; set; }
    public DateTime? DataProcessamento { get; set; }
    
    // Relacionamentos
    public ICollection<ArquivoDadgerValor>? Valores { get; set; }
}
```

**Endpoints:** 8
- GET /api/arquivosdadger
- GET /api/arquivosdadger/{id}
- GET /api/arquivosdadger/semana/{semanaPmoId}
- POST /api/arquivosdadger
- PUT /api/arquivosdadger/{id}
- DELETE /api/arquivosdadger/{id}
- POST /api/arquivosdadger/{id}/processar
- GET /api/arquivosdadger/{id}/valores

**Seed Data:** 10 arquivos exemplo

---

#### 7. API ARQUIVO DADGER VALOR ?? MUITO COMPLEXA
**Prioridade:** ?? CRÍTICA (dia 3)  
**Tempo estimado:** 4h

**Entidade:**
```csharp
public class ArquivoDadgerValor : BaseEntity
{
    public int ArquivoDadgerId { get; set; }
    public ArquivoDadger? ArquivoDadger { get; set; }
    public string Chave { get; set; } = string.Empty; // Ex: "CADGER.PROD.UTE.001"
    public string? Valor { get; set; }
    public string? Tipo { get; set; } // String, Number, Date
    public int? Linha { get; set; } // Linha no arquivo
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 7
- GET /api/arquivosdadgervalores
- GET /api/arquivosdadgervalores/{id}
- GET /api/arquivosdadgervalores/arquivo/{arquivoId}
- GET /api/arquivosdadgervalores/arquivo/{arquivoId}/chave/{chave}
- POST /api/arquivosdadgervalores
- PUT /api/arquivosdadgervalores/{id}
- DELETE /api/arquivosdadgervalores/{id}

**Seed Data:** 50 valores exemplo (5 por arquivo)

---

#### 8. API CARGA ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 3)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class Carga : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public string SubsistemaId { get; set; } = string.Empty; // SE, S, NE, N
    public decimal CargaMWmed { get; set; }
    public decimal CargaVerificada { get; set; }
    public decimal PrevisaoCarga { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/cargas
- GET /api/cargas/{id}
- GET /api/cargas/subsistema/{subsistemaId}
- GET /api/cargas/periodo
- POST /api/cargas
- PUT /api/cargas/{id}

**Seed Data:** 30 registros (últimos 30 dias)

---

#### 9. API USUARIO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 4)  
**Tempo estimado:** 2h

**Entidade:**
```csharp
public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public int? EquipePDPId { get; set; }
    public EquipePDP? EquipePDP { get; set; }
    public string? Perfil { get; set; } // Admin, Operador, Consultor
}
```

**Endpoints:** 6
- GET /api/usuarios
- GET /api/usuarios/{id}
- GET /api/usuarios/equipe/{equipeId}
- POST /api/usuarios
- PUT /api/usuarios/{id}
- DELETE /api/usuarios/{id}

**Seed Data:** 15 usuários exemplo

---

### ?? CATEGORIA 3: CONSOLIDADOS (3 APIs)

#### 10. API DCA (Dados Consolidados Anterior) ?? COMPLEXA
**Prioridade:** ?? ALTA (dia 4)  
**Tempo estimado:** 3.5h

**Entidade:**
```csharp
public class DCA : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public int SemanaPMOId { get; set; }
    public SemanaPMO? SemanaPMO { get; set; }
    public string? DadosConsolidados { get; set; } // JSON
    public bool Aprovado { get; set; }
    public DateTime? DataAprovacao { get; set; }
    public string? UsuarioAprovacao { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 7
- GET /api/dca
- GET /api/dca/{id}
- GET /api/dca/semana/{semanaPmoId}
- POST /api/dca
- PUT /api/dca/{id}
- POST /api/dca/{id}/aprovar
- GET /api/dca/pendentes

**Seed Data:** 10 registros

---

#### 11. API DCR (Dados Consolidados Revisão) ?? COMPLEXA
**Prioridade:** ?? ALTA (dia 4)  
**Tempo estimado:** 3.5h

**Entidade:**
```csharp
public class DCR : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public int SemanaPMOId { get; set; }
    public SemanaPMO? SemanaPMO { get; set; }
    public int? DCAId { get; set; }
    public DCA? DCA { get; set; }
    public string? DadosRevisados { get; set; } // JSON
    public string? MotivoRevisao { get; set; }
    public bool Aprovado { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 7
- GET /api/dcr
- GET /api/dcr/{id}
- GET /api/dcr/semana/{semanaPmoId}
- GET /api/dcr/dca/{dcaId}
- POST /api/dcr
- PUT /api/dcr/{id}
- POST /api/dcr/{id}/aprovar

**Seed Data:** 8 registros

---

#### 12. API RESPONSAVEL ?? SIMPLES
**Prioridade:** ?? MÉDIA (dia 4)  
**Tempo estimado:** 1.5h

**Entidade:**
```csharp
public class Responsavel : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public string? Area { get; set; }
}
```

**Endpoints:** 5
- GET /api/responsaveis
- GET /api/responsaveis/{id}
- POST /api/responsaveis
- PUT /api/responsaveis/{id}
- DELETE /api/responsaveis/{id}

**Seed Data:** 10 responsáveis

---

### ?? CATEGORIA 4: DOCUMENTOS (2 APIs)

#### 13. API UPLOAD ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 5)  
**Tempo estimado:** 3h

**Entidade:**
```csharp
public class Upload : BaseEntity
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public long TamanhoBytes { get; set; }
    public string? TipoArquivo { get; set; }
    public DateTime DataUpload { get; set; }
    public string? UsuarioUpload { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/uploads
- GET /api/uploads/{id}
- POST /api/uploads
- GET /api/uploads/{id}/download
- DELETE /api/uploads/{id}
- GET /api/uploads/usuario/{usuarioId}

**Seed Data:** 5 uploads exemplo

---

#### 14. API RELATORIO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 5)  
**Tempo estimado:** 3h

**Entidade:**
```csharp
public class Relatorio : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string TipoRelatorio { get; set; } = string.Empty; // Excel, PDF, CSV
    public DateTime DataGeracao { get; set; }
    public string? CaminhoArquivo { get; set; }
    public string? Parametros { get; set; } // JSON
    public string? UsuarioGeracao { get; set; }
}
```

**Endpoints:** 6
- GET /api/relatorios
- GET /api/relatorios/{id}
- POST /api/relatorios/gerar
- GET /api/relatorios/{id}/download
- DELETE /api/relatorios/{id}
- GET /api/relatorios/tipos

**Seed Data:** 5 relatórios exemplo

---

## ?? RESUMO DEV 1

```
????????????????????????????????????????????????????
? DEV 1 (BACKEND SENIOR) - 14 APIs                 ?
????????????????????????????????????????????????????
? DIA 1 (3 APIs):                                  ?
?  1. Usina (3h)           ?? COMPLEXA             ?
?  2. Empresa (2h)         ?? MÉDIA                ?
?  3. TipoUsina (1.5h)     ?? SIMPLES              ?
?                                                  ?
? DIA 2 (3 APIs):                                  ?
?  4. SemanaPMO (2.5h)     ?? MÉDIA                ?
?  5. EquipePDP (2h)       ?? MÉDIA                ?
?  6. ArquivoDadger (4h)   ?? MUITO COMPLEXA       ?
?                                                  ?
? DIA 3 (2 APIs):                                  ?
?  7. ArquivoDadgerValor (4h) ?? MUITO COMPLEXA    ?
?  8. Carga (2.5h)         ?? MÉDIA                ?
?                                                  ?
? DIA 4 (4 APIs):                                  ?
?  9. Usuario (2h)         ?? MÉDIA                ?
? 10. DCA (3.5h)           ?? COMPLEXA             ?
? 11. DCR (3.5h)           ?? COMPLEXA             ?
? 12. Responsavel (1.5h)   ?? SIMPLES              ?
?                                                  ?
? DIA 5 (2 APIs):                                  ?
? 13. Upload (3h)          ?? MÉDIA                ?
? 14. Relatorio (3h)       ?? MÉDIA                ?
????????????????????????????????????????????????????
? TOTAL: 14 APIs | ~80 endpoints                   ?
????????????????????????????????????????????????????
```

---

## ????? DEV 2 (BACKEND PLENO) - 15 APIs

### ?? CATEGORIA 1: UNIDADES E GERAÇÃO (6 APIs)

#### 15. API UNIDADE GERADORA ?? COMPLEXA
**Branch:** `feature/arquivos-dados`  
**Prioridade:** ?? CRÍTICA (dia 1)  
**Tempo estimado:** 3h

**Entidade:**
```csharp
public class UnidadeGeradora : BaseEntity
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public decimal PotenciaNominal { get; set; }
    public decimal PotenciaMinima { get; set; }
    public DateTime DataComissionamento { get; set; }
    public string? Status { get; set; }
    
    // Relacionamentos
    public ICollection<ParadaUG>? Paradas { get; set; }
    public ICollection<RestricaoUG>? Restricoes { get; set; }
}
```

**Endpoints:** 7
- GET /api/unidadesgeradoras
- GET /api/unidadesgeradoras/{id}
- GET /api/unidadesgeradoras/usina/{usinaId}
- GET /api/unidadesgeradoras/codigo/{codigo}
- POST /api/unidadesgeradoras
- PUT /api/unidadesgeradoras/{id}
- DELETE /api/unidadesgeradoras/{id}

**Seed Data:** 15 UGs (3 por usina)

---

#### 16. API PARADA UG ?? MÉDIA
**Prioridade:** ?? ALTA (dia 1)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class ParadaUG : BaseEntity
{
    public int UnidadeGeradoraId { get; set; }
    public UnidadeGeradora? UnidadeGeradora { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string MotivoParada { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Programada { get; set; }
}
```

**Endpoints:** 6
- GET /api/paradasug
- GET /api/paradasug/{id}
- GET /api/paradasug/unidade/{unidadeId}
- GET /api/paradasug/periodo
- POST /api/paradasug
- PUT /api/paradasug/{id}

**Seed Data:** 10 paradas

---

#### 17. API RESTRIÇÃO UG ?? MÉDIA
**Prioridade:** ?? ALTA (dia 2)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class RestricaoUG : BaseEntity
{
    public int UnidadeGeradoraId { get; set; }
    public UnidadeGeradora? UnidadeGeradora { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int MotivoRestricaoId { get; set; }
    public MotivoRestricao? MotivoRestricao { get; set; }
    public decimal PotenciaRestrita { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/restricoesug
- GET /api/restricoesug/{id}
- GET /api/restricoesug/unidade/{unidadeId}
- GET /api/restricoesug/periodo
- POST /api/restricoesug
- PUT /api/restricoesug/{id}

**Seed Data:** 8 restrições

---

#### 18. API RESTRIÇÃO US ?? MÉDIA
**Prioridade:** ?? ALTA (dia 2)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class RestricaoUS : BaseEntity
{
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int MotivoRestricaoId { get; set; }
    public MotivoRestricao? MotivoRestricao { get; set; }
    public decimal CapacidadeRestrita { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/restricoesus
- GET /api/restricoesus/{id}
- GET /api/restricoesus/usina/{usinaId}
- GET /api/restricoesus/periodo
- POST /api/restricoesus
- PUT /api/restricoesus/{id}

**Seed Data:** 6 restrições

---

#### 19. API MOTIVO RESTRIÇÃO ?? SIMPLES
**Prioridade:** ?? MÉDIA (dia 2)  
**Tempo estimado:** 1.5h

**Entidade:**
```csharp
public class MotivoRestricao : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; } // Técnico, Ambiental, Operacional
}
```

**Endpoints:** 4
- GET /api/motivosrestricao
- GET /api/motivosrestricao/{id}
- POST /api/motivosrestricao
- PUT /api/motivosrestricao/{id}

**Seed Data:** 8 motivos

---

#### 20. API GERAÇÃO FORA MÉRITO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 3)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class GerForaMerito : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public decimal GeracaoMWmed { get; set; }
    public string? Motivo { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/geracaoforamerito
- GET /api/geracaoforamerito/{id}
- GET /api/geracaoforamerito/usina/{usinaId}
- GET /api/geracaoforamerito/periodo
- POST /api/geracaoforamerito
- PUT /api/geracaoforamerito/{id}

**Seed Data:** 10 registros

---

### ?? CATEGORIA 2: OPERAÇÃO (5 APIs)

#### 21. API INTERCÂMBIO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 2-3)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class Intercambio : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public string SubsistemaOrigem { get; set; } = string.Empty;
    public string SubsistemaDestino { get; set; } = string.Empty;
    public decimal EnergiaIntercambiada { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/intercambios
- GET /api/intercambios/{id}
- GET /api/intercambios/subsistema/{subsistemaId}
- GET /api/intercambios/periodo
- POST /api/intercambios
- PUT /api/intercambios/{id}

**Seed Data:** 20 registros

---

#### 22. API BALANÇO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 3)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class Balanco : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public string SubsistemaId { get; set; } = string.Empty;
    public decimal Geracao { get; set; }
    public decimal Carga { get; set; }
    public decimal Intercambio { get; set; }
    public decimal Perdas { get; set; }
    public decimal Deficit { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/balancos
- GET /api/balancos/{id}
- GET /api/balancos/subsistema/{subsistemaId}
- GET /api/balancos/periodo
- POST /api/balancos
- PUT /api/balancos/{id}

**Seed Data:** 15 registros

---

#### 23. API OBSERVAÇÃO ?? SIMPLES
**Prioridade:** ?? MÉDIA (dia 4)  
**Tempo estimado:** 1.5h

**Entidade:**
```csharp
public class Observacao : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public string? Categoria { get; set; }
    public string? UsuarioAutor { get; set; }
}
```

**Endpoints:** 5
- GET /api/observacoes
- GET /api/observacoes/{id}
- GET /api/observacoes/periodo
- POST /api/observacoes
- PUT /api/observacoes/{id}

**Seed Data:** 10 observações

---

#### 24. API DIRETÓRIO ?? SIMPLES
**Prioridade:** ?? MÉDIA (dia 4)  
**Tempo estimado:** 2h

**Entidade:**
```csharp
public class Diretorio : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public int? DiretorioPaiId { get; set; }
    public Diretorio? DiretorioPai { get; set; }
    public string? Descricao { get; set; }
    
    // Relacionamentos
    public ICollection<Arquivo>? Arquivos { get; set; }
    public ICollection<Diretorio>? Subdiretorios { get; set; }
}
```

**Endpoints:** 6
- GET /api/diretorios
- GET /api/diretorios/{id}
- GET /api/diretorios/{id}/arquivos
- POST /api/diretorios
- PUT /api/diretorios/{id}
- DELETE /api/diretorios/{id}

**Seed Data:** 8 diretórios

---

#### 25. API ARQUIVO ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 4-5)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class Arquivo : BaseEntity
{
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoCompleto { get; set; } = string.Empty;
    public int DiretorioId { get; set; }
    public Diretorio? Diretorio { get; set; }
    public long TamanhoBytes { get; set; }
    public string? TipoArquivo { get; set; }
    public DateTime DataCriacao { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/arquivos
- GET /api/arquivos/{id}
- GET /api/arquivos/diretorio/{diretorioId}
- POST /api/arquivos
- PUT /api/arquivos/{id}
- DELETE /api/arquivos/{id}

**Seed Data:** 15 arquivos

---

### ?? CATEGORIA 3: CONFIGURAÇÕES TÉRMICAS (4 APIs)

#### 26. API MODALIDADE OP TÉRMICA ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 5)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class ModalidadeOpTermica : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal CustoOperacional { get; set; }
    public decimal PotenciaMinima { get; set; }
    public decimal PotenciaMaxima { get; set; }
}
```

**Endpoints:** 5
- GET /api/modalidadesoptermica
- GET /api/modalidadesoptermica/{id}
- POST /api/modalidadesoptermica
- PUT /api/modalidadesoptermica/{id}
- DELETE /api/modalidadesoptermica/{id}

**Seed Data:** 6 modalidades

---

#### 27. API INFLEXIBILIDADE CONTRATADA ?? MÉDIA
**Prioridade:** ?? MÉDIA (dia 5)  
**Tempo estimado:** 2.5h

**Entidade:**
```csharp
public class InflexibilidadeContratada : BaseEntity
{
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal GeracaoMinima { get; set; }
    public string? Contrato { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 6
- GET /api/inflexibilidadescontratadas
- GET /api/inflexibilidadescontratadas/{id}
- GET /api/inflexibilidadescontratadas/usina/{usinaId}
- GET /api/inflexibilidadescontratadas/periodo
- POST /api/inflexibilidadescontratadas
- PUT /api/inflexibilidadescontratadas/{id}

**Seed Data:** 8 contratos

---

#### 28. API RAMPAS USINA TÉRMICA ?? MÉDIA
**Prioridade:** ?? BAIXA (dia 6)  
**Tempo estimado:** 2h

**Entidade:**
```csharp
public class RampasUsinaTermica : BaseEntity
{
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public decimal RampaSubida { get; set; } // MW/min
    public decimal RampaDescida { get; set; } // MW/min
    public decimal TempoPartida { get; set; } // minutos
    public decimal TempoParada { get; set; } // minutos
}
```

**Endpoints:** 5
- GET /api/rampasusinatermica
- GET /api/rampasusinatermica/{id}
- GET /api/rampasusinatermica/usina/{usinaId}
- POST /api/rampasusinatermica
- PUT /api/rampasusinatermica/{id}

**Seed Data:** 5 configurações

---

#### 29. API USINA CONVERSORA ?? SIMPLES
**Prioridade:** ?? BAIXA (dia 6)  
**Tempo estimado:** 1.5h

**Entidade:**
```csharp
public class UsinaConversora : BaseEntity
{
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public decimal CapacidadeConversao { get; set; }
    public string TipoConversao { get; set; } = string.Empty; // AC-DC, DC-AC
    public decimal Eficiencia { get; set; }
    public string? Observacoes { get; set; }
}
```

**Endpoints:** 5
- GET /api/usinasconversoras
- GET /api/usinasconversoras/{id}
- GET /api/usinasconversoras/usina/{usinaId}
- POST /api/usinasconversoras
- PUT /api/usinasconversoras/{id}

**Seed Data:** 3 usinas

---

## ?? RESUMO DEV 2

```
????????????????????????????????????????????????????
? DEV 2 (BACKEND PLENO) - 15 APIs                  ?
????????????????????????????????????????????????????
? DIA 1 (2.5 APIs):                                ?
? 15. UnidadeGeradora (3h)    ?? COMPLEXA          ?
? 16. ParadaUG (2.5h)         ?? MÉDIA             ?
? 17. RestricaoUG (início)    ?? MÉDIA             ?
?                                                  ?
? DIA 2 (3.5 APIs):                                ?
? 17. RestricaoUG (conclusão) ?? MÉDIA             ?
? 18. RestricaoUS (2.5h)      ?? MÉDIA             ?
? 19. MotivoRestricao (1.5h)  ?? SIMPLES           ?
? 21. Intercambio (início)    ?? MÉDIA             ?
?                                                  ?
? DIA 3 (3 APIs):                                  ?
? 21. Intercambio (conclusão) ?? MÉDIA             ?
? 22. Balanco (2.5h)          ?? MÉDIA             ?
? 20. GerForaMerito (2.5h)    ?? MÉDIA             ?
?                                                  ?
? DIA 4 (3 APIs):                                  ?
? 23. Observacao (1.5h)       ?? SIMPLES           ?
? 24. Diretorio (2h)          ?? SIMPLES           ?
? 25. Arquivo (2.5h)          ?? MÉDIA             ?
?                                                  ?
? DIA 5 (3 APIs):                                  ?
? 26. ModalidadeOpTermica (2.5h) ?? MÉDIA          ?
? 27. InflexibilidadeContratada (2.5h) ?? MÉDIA    ?
? 28. RampasUsinaTermica (início)                  ?
?                                                  ?
? DIA 6 (2 APIs):                                  ?
? 28. RampasUsinaTermica (conclusão) ?? MÉDIA      ?
? 29. UsinaConversora (1.5h)  ?? SIMPLES           ?
????????????????????????????????????????????????????
? TOTAL: 15 APIs | ~85 endpoints                   ?
????????????????????????????????????????????????????
```

---

## ?? COMPARAÇÃO FINAL

```
???????????????????????????????????????????????
?             DEV 1    vs    DEV 2            ?
???????????????????????????????????????????????
? APIs:          14           15              ?
? Endpoints:    ~80          ~85              ?
?                                             ?
? Complexas:      6            1              ?
? Médias:         6           11              ?
? Simples:        2            3              ?
?                                             ?
? Horas:        ~40h         ~40h             ?
???????????????????????????????????????????????

BALANCEAMENTO: ? Equilibrado
- DEV 1: Mais APIs complexas (Senior)
- DEV 2: Mais APIs médias (Pleno)
- Ambos: Carga de trabalho similar
```

---

## ?? CHECKPOINTS DE PROGRESSO

### Checkpoint DIA 2 (20/12 - Sexta)
**Meta:** 11 APIs acumuladas (38%)
- DEV 1: 6 APIs
- DEV 2: 5 APIs

### Checkpoint DIA 3 (21/12 - Sábado)
**Meta:** 16 APIs acumuladas (55%)
- DEV 1: 8 APIs
- DEV 2: 8 APIs

### Checkpoint DIA 4 (22/12 - Domingo)
**Meta:** 22 APIs acumuladas (76%)
- DEV 1: 12 APIs
- DEV 2: 10 APIs

### Checkpoint DIA 5 (23/12 - Segunda)
**Meta:** 27 APIs acumuladas (93%)
- DEV 1: 14 APIs
- DEV 2: 13 APIs

### Final DIA 6 (24/12 - Terça)
**Meta:** 29 APIs completas (100%) ?
- DEV 1: 14 APIs
- DEV 2: 15 APIs

---

**Documentação criada por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? COMPLETO E BALANCEADO

**Próximo:** Criar arquivos base e começar primeira API (Usina)

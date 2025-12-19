IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Balancos] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [SubsistemaId] nvarchar(10) NOT NULL,
    [Geracao] decimal(18,2) NOT NULL,
    [Carga] decimal(18,2) NOT NULL,
    [Intercambio] decimal(18,2) NOT NULL,
    [Perdas] decimal(18,2) NOT NULL,
    [Deficit] decimal(18,2) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Balancos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Cargas] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [SubsistemaId] nvarchar(10) NOT NULL,
    [CargaMWmed] decimal(18,2) NOT NULL,
    [CargaVerificada] decimal(18,2) NOT NULL,
    [PrevisaoCarga] decimal(18,2) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Cargas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DadosEnergeticos] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [CodigoUsina] nvarchar(50) NULL,
    [ProducaoMWh] decimal(18,2) NOT NULL,
    [CapacidadeDisponivel] decimal(18,2) NOT NULL,
    [Status] nvarchar(50) NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_DadosEnergeticos] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Diretorios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Caminho] nvarchar(1000) NOT NULL,
    [DiretorioPaiId] int NULL,
    [Descricao] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Diretorios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Diretorios_Diretorios_DiretorioPaiId] FOREIGN KEY ([DiretorioPaiId]) REFERENCES [Diretorios] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Empresas] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [CNPJ] nvarchar(18) NULL,
    [Telefone] nvarchar(20) NULL,
    [Email] nvarchar(200) NULL,
    [Endereco] nvarchar(500) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Empresas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [EquipesPDP] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NULL,
    [Coordenador] nvarchar(max) NULL,
    [Email] nvarchar(200) NULL,
    [Telefone] nvarchar(20) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_EquipesPDP] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Intercambios] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [SubsistemaOrigem] nvarchar(10) NOT NULL,
    [SubsistemaDestino] nvarchar(10) NOT NULL,
    [EnergiaIntercambiada] decimal(18,2) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Intercambios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ModalidadesOpTermica] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NULL,
    [CustoOperacional] decimal(18,2) NOT NULL,
    [PotenciaMinima] decimal(18,2) NOT NULL,
    [PotenciaMaxima] decimal(18,2) NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_ModalidadesOpTermica] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [MotivosRestricao] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NULL,
    [Categoria] nvarchar(100) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_MotivosRestricao] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Observacoes] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [Titulo] nvarchar(200) NOT NULL,
    [Conteudo] nvarchar(max) NOT NULL,
    [Categoria] nvarchar(max) NULL,
    [UsuarioAutor] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Observacoes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Relatorios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Descricao] nvarchar(max) NULL,
    [TipoRelatorio] nvarchar(50) NOT NULL,
    [DataGeracao] datetime2 NOT NULL,
    [CaminhoArquivo] nvarchar(max) NULL,
    [Parametros] nvarchar(max) NULL,
    [UsuarioGeracao] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Relatorios] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Responsaveis] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Cargo] nvarchar(max) NULL,
    [Email] nvarchar(200) NULL,
    [Telefone] nvarchar(max) NULL,
    [Area] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Responsaveis] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [SemanasPMO] (
    [Id] int NOT NULL IDENTITY,
    [Numero] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NOT NULL,
    [Ano] int NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_SemanasPMO] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TiposUsina] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(100) NOT NULL,
    [Descricao] nvarchar(500) NULL,
    [FonteEnergia] nvarchar(100) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_TiposUsina] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Uploads] (
    [Id] int NOT NULL IDENTITY,
    [NomeArquivo] nvarchar(500) NOT NULL,
    [CaminhoArquivo] nvarchar(1000) NOT NULL,
    [TamanhoBytes] bigint NOT NULL,
    [TipoArquivo] nvarchar(max) NULL,
    [DataUpload] datetime2 NOT NULL,
    [UsuarioUpload] nvarchar(max) NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Uploads] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Arquivos] (
    [Id] int NOT NULL IDENTITY,
    [NomeArquivo] nvarchar(500) NOT NULL,
    [CaminhoCompleto] nvarchar(1000) NOT NULL,
    [DiretorioId] int NOT NULL,
    [TamanhoBytes] bigint NOT NULL,
    [TipoArquivo] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Arquivos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Arquivos_Diretorios_DiretorioId] FOREIGN KEY ([DiretorioId]) REFERENCES [Diretorios] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [Nome] nvarchar(200) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Telefone] nvarchar(max) NULL,
    [EquipePDPId] int NULL,
    [Perfil] nvarchar(50) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Usuarios_EquipesPDP_EquipePDPId] FOREIGN KEY ([EquipePDPId]) REFERENCES [EquipesPDP] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [ArquivosDadger] (
    [Id] int NOT NULL IDENTITY,
    [NomeArquivo] nvarchar(500) NOT NULL,
    [CaminhoArquivo] nvarchar(1000) NOT NULL,
    [DataImportacao] datetime2 NOT NULL,
    [SemanaPMOId] int NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [Processado] bit NOT NULL,
    [DataProcessamento] datetime2 NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_ArquivosDadger] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ArquivosDadger_SemanasPMO_SemanaPMOId] FOREIGN KEY ([SemanaPMOId]) REFERENCES [SemanasPMO] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [DCAs] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [SemanaPMOId] int NOT NULL,
    [DadosConsolidados] nvarchar(max) NULL,
    [Aprovado] bit NOT NULL,
    [DataAprovacao] datetime2 NULL,
    [UsuarioAprovacao] nvarchar(max) NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_DCAs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DCAs_SemanasPMO_SemanaPMOId] FOREIGN KEY ([SemanaPMOId]) REFERENCES [SemanasPMO] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Usinas] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] nvarchar(50) NOT NULL,
    [Nome] nvarchar(200) NOT NULL,
    [TipoUsinaId] int NOT NULL,
    [EmpresaId] int NOT NULL,
    [CapacidadeInstalada] decimal(18,2) NOT NULL,
    [Localizacao] nvarchar(500) NULL,
    [DataOperacao] datetime2 NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_Usinas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Usinas_Empresas_EmpresaId] FOREIGN KEY ([EmpresaId]) REFERENCES [Empresas] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Usinas_TiposUsina_TipoUsinaId] FOREIGN KEY ([TipoUsinaId]) REFERENCES [TiposUsina] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [ArquivosDadgerValores] (
    [Id] int NOT NULL IDENTITY,
    [ArquivoDadgerId] int NOT NULL,
    [Chave] nvarchar(200) NOT NULL,
    [Valor] nvarchar(max) NULL,
    [Tipo] nvarchar(50) NULL,
    [Linha] int NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_ArquivosDadgerValores] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ArquivosDadgerValores_ArquivosDadger_ArquivoDadgerId] FOREIGN KEY ([ArquivoDadgerId]) REFERENCES [ArquivosDadger] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DCRs] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [SemanaPMOId] int NOT NULL,
    [DCAId] int NULL,
    [DadosRevisados] nvarchar(max) NULL,
    [MotivoRevisao] nvarchar(max) NULL,
    [Aprovado] bit NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_DCRs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DCRs_DCAs_DCAId] FOREIGN KEY ([DCAId]) REFERENCES [DCAs] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DCRs_SemanasPMO_SemanaPMOId] FOREIGN KEY ([SemanaPMOId]) REFERENCES [SemanasPMO] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [GeracoesForaMerito] (
    [Id] int NOT NULL IDENTITY,
    [DataReferencia] datetime2 NOT NULL,
    [UsinaId] int NOT NULL,
    [GeracaoMWmed] decimal(18,2) NOT NULL,
    [Motivo] nvarchar(max) NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_GeracoesForaMerito] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GeracoesForaMerito_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [InflexibilidadesContratadas] (
    [Id] int NOT NULL IDENTITY,
    [UsinaId] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NOT NULL,
    [GeracaoMinima] decimal(18,2) NOT NULL,
    [Contrato] nvarchar(max) NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_InflexibilidadesContratadas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InflexibilidadesContratadas_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RampasUsinasTermicas] (
    [Id] int NOT NULL IDENTITY,
    [UsinaId] int NOT NULL,
    [RampaSubida] decimal(18,4) NOT NULL,
    [RampaDescida] decimal(18,4) NOT NULL,
    [TempoPartida] decimal(18,2) NOT NULL,
    [TempoParada] decimal(18,2) NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_RampasUsinasTermicas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RampasUsinasTermicas_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RestricoesUS] (
    [Id] int NOT NULL IDENTITY,
    [UsinaId] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NULL,
    [MotivoRestricaoId] int NOT NULL,
    [CapacidadeRestrita] decimal(18,2) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_RestricoesUS] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RestricoesUS_MotivosRestricao_MotivoRestricaoId] FOREIGN KEY ([MotivoRestricaoId]) REFERENCES [MotivosRestricao] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RestricoesUS_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UnidadesGeradoras] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] nvarchar(50) NOT NULL,
    [Nome] nvarchar(200) NOT NULL,
    [UsinaId] int NOT NULL,
    [PotenciaNominal] decimal(18,2) NOT NULL,
    [PotenciaMinima] decimal(18,2) NOT NULL,
    [DataComissionamento] datetime2 NOT NULL,
    [Status] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_UnidadesGeradoras] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UnidadesGeradoras_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UsinasConversoras] (
    [Id] int NOT NULL IDENTITY,
    [UsinaId] int NOT NULL,
    [CapacidadeConversao] decimal(18,2) NOT NULL,
    [TipoConversao] nvarchar(50) NOT NULL,
    [Eficiencia] decimal(5,4) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_UsinasConversoras] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UsinasConversoras_Usinas_UsinaId] FOREIGN KEY ([UsinaId]) REFERENCES [Usinas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ParadasUG] (
    [Id] int NOT NULL IDENTITY,
    [UnidadeGeradoraId] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NULL,
    [MotivoParada] nvarchar(500) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [Programada] bit NOT NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_ParadasUG] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ParadasUG_UnidadesGeradoras_UnidadeGeradoraId] FOREIGN KEY ([UnidadeGeradoraId]) REFERENCES [UnidadesGeradoras] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [RestricoesUG] (
    [Id] int NOT NULL IDENTITY,
    [UnidadeGeradoraId] int NOT NULL,
    [DataInicio] datetime2 NOT NULL,
    [DataFim] datetime2 NULL,
    [MotivoRestricaoId] int NOT NULL,
    [PotenciaRestrita] decimal(18,2) NOT NULL,
    [Observacoes] nvarchar(max) NULL,
    [DataCriacao] datetime2 NOT NULL,
    [DataAtualizacao] datetime2 NULL,
    [Ativo] bit NOT NULL,
    CONSTRAINT [PK_RestricoesUG] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RestricoesUG_MotivosRestricao_MotivoRestricaoId] FOREIGN KEY ([MotivoRestricaoId]) REFERENCES [MotivosRestricao] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RestricoesUG_UnidadesGeradoras_UnidadeGeradoraId] FOREIGN KEY ([UnidadeGeradoraId]) REFERENCES [UnidadesGeradoras] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Arquivos_DiretorioId] ON [Arquivos] ([DiretorioId]);
GO

CREATE INDEX [IX_ArquivosDadger_SemanaPMOId] ON [ArquivosDadger] ([SemanaPMOId]);
GO

CREATE INDEX [IX_ArquivosDadgerValores_ArquivoDadgerId] ON [ArquivosDadgerValores] ([ArquivoDadgerId]);
GO

CREATE INDEX [IX_ArquivosDadgerValores_Chave] ON [ArquivosDadgerValores] ([Chave]);
GO

CREATE INDEX [IX_Balancos_DataReferencia_SubsistemaId] ON [Balancos] ([DataReferencia], [SubsistemaId]);
GO

CREATE INDEX [IX_Cargas_DataReferencia_SubsistemaId] ON [Cargas] ([DataReferencia], [SubsistemaId]);
GO

CREATE INDEX [IX_DadosEnergeticos_DataReferencia] ON [DadosEnergeticos] ([DataReferencia]);
GO

CREATE INDEX [IX_DCAs_SemanaPMOId] ON [DCAs] ([SemanaPMOId]);
GO

CREATE INDEX [IX_DCRs_DCAId] ON [DCRs] ([DCAId]);
GO

CREATE INDEX [IX_DCRs_SemanaPMOId] ON [DCRs] ([SemanaPMOId]);
GO

CREATE INDEX [IX_Diretorios_DiretorioPaiId] ON [Diretorios] ([DiretorioPaiId]);
GO

CREATE UNIQUE INDEX [IX_Empresas_CNPJ] ON [Empresas] ([CNPJ]) WHERE [CNPJ] IS NOT NULL;
GO

CREATE INDEX [IX_Empresas_Nome] ON [Empresas] ([Nome]);
GO

CREATE INDEX [IX_GeracoesForaMerito_UsinaId] ON [GeracoesForaMerito] ([UsinaId]);
GO

CREATE INDEX [IX_InflexibilidadesContratadas_UsinaId] ON [InflexibilidadesContratadas] ([UsinaId]);
GO

CREATE INDEX [IX_Intercambios_DataReferencia_SubsistemaOrigem_SubsistemaDestino] ON [Intercambios] ([DataReferencia], [SubsistemaOrigem], [SubsistemaDestino]);
GO

CREATE INDEX [IX_ParadasUG_UnidadeGeradoraId] ON [ParadasUG] ([UnidadeGeradoraId]);
GO

CREATE INDEX [IX_RampasUsinasTermicas_UsinaId] ON [RampasUsinasTermicas] ([UsinaId]);
GO

CREATE INDEX [IX_RestricoesUG_MotivoRestricaoId] ON [RestricoesUG] ([MotivoRestricaoId]);
GO

CREATE INDEX [IX_RestricoesUG_UnidadeGeradoraId] ON [RestricoesUG] ([UnidadeGeradoraId]);
GO

CREATE INDEX [IX_RestricoesUS_MotivoRestricaoId] ON [RestricoesUS] ([MotivoRestricaoId]);
GO

CREATE INDEX [IX_RestricoesUS_UsinaId] ON [RestricoesUS] ([UsinaId]);
GO

CREATE UNIQUE INDEX [IX_SemanasPMO_Numero_Ano] ON [SemanasPMO] ([Numero], [Ano]);
GO

CREATE INDEX [IX_TiposUsina_Nome] ON [TiposUsina] ([Nome]);
GO

CREATE UNIQUE INDEX [IX_UnidadesGeradoras_Codigo] ON [UnidadesGeradoras] ([Codigo]);
GO

CREATE INDEX [IX_UnidadesGeradoras_UsinaId] ON [UnidadesGeradoras] ([UsinaId]);
GO

CREATE UNIQUE INDEX [IX_Usinas_Codigo] ON [Usinas] ([Codigo]);
GO

CREATE INDEX [IX_Usinas_EmpresaId] ON [Usinas] ([EmpresaId]);
GO

CREATE INDEX [IX_Usinas_Nome] ON [Usinas] ([Nome]);
GO

CREATE INDEX [IX_Usinas_TipoUsinaId] ON [Usinas] ([TipoUsinaId]);
GO

CREATE INDEX [IX_UsinasConversoras_UsinaId] ON [UsinasConversoras] ([UsinaId]);
GO

CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
GO

CREATE INDEX [IX_Usuarios_EquipePDPId] ON [Usuarios] ([EquipePDPId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251219122515_InitialCreate', N'8.0.0');
GO

COMMIT;
GO


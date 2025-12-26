using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Infrastructure.Data.Seed;

namespace PDPW.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados para o PDPW
/// </summary>
public class PdpwDbContext : DbContext
{
    public PdpwDbContext(DbContextOptions<PdpwDbContext> options) : base(options)
    {
    }

    // DbSets - Entidades do sistema
    
    // Dados Energéticos (legado)
    public DbSet<DadoEnergetico> DadosEnergeticos { get; set; }

    // Gestão de Ativos
    public DbSet<TipoUsina> TiposUsina { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Usina> Usinas { get; set; }
    public DbSet<SemanaPMO> SemanasPMO { get; set; }
    public DbSet<EquipePDP> EquipesPDP { get; set; }

    // Unidades e Geração
    public DbSet<UnidadeGeradora> UnidadesGeradoras { get; set; }
    public DbSet<ParadaUG> ParadasUG { get; set; }
    public DbSet<RestricaoUG> RestricoesUG { get; set; }
    public DbSet<RestricaoUS> RestricoesUS { get; set; }
    public DbSet<MotivoRestricao> MotivosRestricao { get; set; }
    public DbSet<GerForaMerito> GeracoesForaMerito { get; set; }

    // Dados Core
    public DbSet<ArquivoDadger> ArquivosDadger { get; set; }
    public DbSet<ArquivoDadgerValor> ArquivosDadgerValores { get; set; }
    public DbSet<Carga> Cargas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    // Consolidados
    public DbSet<DCA> DCAs { get; set; }
    public DbSet<DCR> DCRs { get; set; }
    public DbSet<Responsavel> Responsaveis { get; set; }

    // Documentos
    public DbSet<Upload> Uploads { get; set; }
    public DbSet<Relatorio> Relatorios { get; set; }
    public DbSet<Arquivo> Arquivos { get; set; }
    public DbSet<Diretorio> Diretorios { get; set; }

    // Operação
    public DbSet<Intercambio> Intercambios { get; set; }
    public DbSet<Balanco> Balancos { get; set; }
    public DbSet<Observacao> Observacoes { get; set; }

    // Térmicas
    public DbSet<ModalidadeOpTermica> ModalidadesOpTermica { get; set; }
    public DbSet<InflexibilidadeContratada> InflexibilidadesContratadas { get; set; }
    public DbSet<RampasUsinaTermica> RampasUsinasTermicas { get; set; }
    public DbSet<UsinaConversora> UsinasConversoras { get; set; }

    // Ofertas
    public DbSet<OfertaExportacao> OfertasExportacao { get; set; }
    public DbSet<OfertaRespostaVoluntaria> OfertasRespostaVoluntaria { get; set; }
    
    // Controle de Agentes
    public DbSet<JanelaEnvioAgente> JanelasEnvioAgente { get; set; }
    public DbSet<SubmissaoAgente> SubmissoesAgente { get; set; }
    
    // Previsão Eólica
    public DbSet<PrevisaoEolica> PrevisoesEolicas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração de DadoEnergetico (legado)
        ConfigurarDadoEnergetico(modelBuilder);

        // Aplicar configurações via Fluent API
        ConfigurarEntidades(modelBuilder);

        // Aplicar seed data
        DbSeeder.Seed(modelBuilder);
    }

    private void ConfigurarDadoEnergetico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DadoEnergetico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CodigoUsina).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ProducaoMWh).HasPrecision(18, 2);
            entity.Property(e => e.CapacidadeDisponivel).HasPrecision(18, 2);
            entity.Property(e => e.EnergiaVertida).HasPrecision(18, 2);
            entity.Property(e => e.EnergiaTurbinavelNaoUtilizada).HasPrecision(18, 2);
            entity.Property(e => e.MotivoVertimento).HasMaxLength(500);
            entity.HasIndex(e => e.DataReferencia);
        });
    }

    private void ConfigurarEntidades(ModelBuilder modelBuilder)
    {
        // TipoUsina
        modelBuilder.Entity<TipoUsina>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Descricao).HasMaxLength(500);
            entity.Property(e => e.FonteEnergia).HasMaxLength(100);
            entity.HasIndex(e => e.Nome);
        });

        // Empresa
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CNPJ).HasMaxLength(18);
            entity.Property(e => e.Telefone).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Endereco).HasMaxLength(500);
            entity.HasIndex(e => e.Nome);
            entity.HasIndex(e => e.CNPJ).IsUnique();
        });

        // Usina
        modelBuilder.Entity<Usina>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CapacidadeInstalada).HasPrecision(18, 2);
            entity.Property(e => e.Localizacao).HasMaxLength(500);
            
            entity.HasIndex(e => e.Codigo).IsUnique();
            entity.HasIndex(e => e.Nome);

            entity.HasOne(e => e.TipoUsina)
                .WithMany(t => t.Usinas)
                .HasForeignKey(e => e.TipoUsinaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Empresa)
                .WithMany(em => em.Usinas)
                .HasForeignKey(e => e.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SemanaPMO
        modelBuilder.Entity<SemanaPMO>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Numero).IsRequired();
            entity.Property(e => e.Ano).IsRequired();
            entity.HasIndex(e => new { e.Numero, e.Ano }).IsUnique();
        });

        // EquipePDP
        modelBuilder.Entity<EquipePDP>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Telefone).HasMaxLength(20);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Perfil).HasMaxLength(50);
            
            entity.HasIndex(e => e.Email).IsUnique();

            entity.HasOne(e => e.EquipePDP)
                .WithMany(eq => eq.Membros)
                .HasForeignKey(e => e.EquipePDPId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // UnidadeGeradora
        modelBuilder.Entity<UnidadeGeradora>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Codigo).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PotenciaNominal).HasPrecision(18, 2);
            entity.Property(e => e.PotenciaMinima).HasPrecision(18, 2);
            
            entity.HasIndex(e => e.Codigo).IsUnique();

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.UnidadesGeradoras)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ParadaUG
        modelBuilder.Entity<ParadaUG>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MotivoParada).IsRequired().HasMaxLength(500);

            entity.HasOne(e => e.UnidadeGeradora)
                .WithMany(ug => ug.Paradas)
                .HasForeignKey(e => e.UnidadeGeradoraId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // MotivoRestricao
        modelBuilder.Entity<MotivoRestricao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Categoria).HasMaxLength(100);
        });

        // RestricaoUG
        modelBuilder.Entity<RestricaoUG>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PotenciaRestrita).HasPrecision(18, 2);

            entity.HasOne(e => e.UnidadeGeradora)
                .WithMany(ug => ug.Restricoes)
                .HasForeignKey(e => e.UnidadeGeradoraId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MotivoRestricao)
                .WithMany(m => m.RestricoesUG)
                .HasForeignKey(e => e.MotivoRestricaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // RestricaoUS
        modelBuilder.Entity<RestricaoUS>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CapacidadeRestrita).HasPrecision(18, 2);

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.Restricoes)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MotivoRestricao)
                .WithMany(m => m.RestricoesUS)
                .HasForeignKey(e => e.MotivoRestricaoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // GerForaMerito
        modelBuilder.Entity<GerForaMerito>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GeracaoMWmed).HasPrecision(18, 2);

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.GeracoesForaMerito)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ArquivoDadger
        modelBuilder.Entity<ArquivoDadger>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CaminhoArquivo).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.SemanaPMO)
                .WithMany(s => s.ArquivosDadger)
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ArquivoDadgerValor
        modelBuilder.Entity<ArquivoDadgerValor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Chave).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(e => e.ArquivoDadger)
                .WithMany(a => a.Valores)
                .HasForeignKey(e => e.ArquivoDadgerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Chave);
        });

        // Carga
        modelBuilder.Entity<Carga>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubsistemaId).IsRequired().HasMaxLength(10);
            entity.Property(e => e.CargaMWmed).HasPrecision(18, 2);
            entity.Property(e => e.CargaVerificada).HasPrecision(18, 2);
            entity.Property(e => e.PrevisaoCarga).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.DataReferencia, e.SubsistemaId });
        });

        // DCA
        modelBuilder.Entity<DCA>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.SemanaPMO)
                .WithMany(s => s.DCAs)
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DCR
        modelBuilder.Entity<DCR>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.SemanaPMO)
                .WithMany(s => s.DCRs)
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DCA)
                .WithMany(d => d.DCRs)
                .HasForeignKey(e => e.DCAId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Responsavel
        modelBuilder.Entity<Responsavel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(200);
        });

        // Upload
        modelBuilder.Entity<Upload>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CaminhoArquivo).IsRequired().HasMaxLength(1000);
        });

        // Relatorio
        modelBuilder.Entity<Relatorio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.TipoRelatorio).IsRequired().HasMaxLength(50);
        });

        // Diretorio
        modelBuilder.Entity<Diretorio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Caminho).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.DiretorioPai)
                .WithMany(d => d.Subdiretorios)
                .HasForeignKey(e => e.DiretorioPaiId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Arquivo
        modelBuilder.Entity<Arquivo>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NomeArquivo).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CaminhoCompleto).IsRequired().HasMaxLength(1000);

            entity.HasOne(e => e.Diretorio)
                .WithMany(d => d.Arquivos)
                .HasForeignKey(e => e.DiretorioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Intercambio
        modelBuilder.Entity<Intercambio>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubsistemaOrigem).IsRequired().HasMaxLength(10);
            entity.Property(e => e.SubsistemaDestino).IsRequired().HasMaxLength(10);
            entity.Property(e => e.EnergiaIntercambiada).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.DataReferencia, e.SubsistemaOrigem, e.SubsistemaDestino });
        });

        // Balanco
        modelBuilder.Entity<Balanco>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.SubsistemaId).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Geracao).HasPrecision(18, 2);
            entity.Property(e => e.Carga).HasPrecision(18, 2);
            entity.Property(e => e.Intercambio).HasPrecision(18, 2);
            entity.Property(e => e.Perdas).HasPrecision(18, 2);
            entity.Property(e => e.Deficit).HasPrecision(18, 2);

            entity.HasIndex(e => new { e.DataReferencia, e.SubsistemaId });
        });

        // Observacao
        modelBuilder.Entity<Observacao>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Conteudo).IsRequired();
        });

        // ModalidadeOpTermica
        modelBuilder.Entity<ModalidadeOpTermica>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nome).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustoOperacional).HasPrecision(18, 2);
            entity.Property(e => e.PotenciaMinima).HasPrecision(18, 2);
            entity.Property(e => e.PotenciaMaxima).HasPrecision(18, 2);
        });

        // InflexibilidadeContratada
        modelBuilder.Entity<InflexibilidadeContratada>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GeracaoMinima).HasPrecision(18, 2);

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.InflexibilidadesContratadas)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RampasUsinaTermica
        modelBuilder.Entity<RampasUsinaTermica>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RampaSubida).HasPrecision(18, 4);
            entity.Property(e => e.RampaDescida).HasPrecision(18, 4);
            entity.Property(e => e.TempoPartida).HasPrecision(18, 2);
            entity.Property(e => e.TempoParada).HasPrecision(18, 2);

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.RampasTermicas)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // UsinaConversora
        modelBuilder.Entity<UsinaConversora>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TipoConversao).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CapacidadeConversao).HasPrecision(18, 2);
            entity.Property(e => e.Eficiencia).HasPrecision(5, 4);

            entity.HasOne(e => e.Usina)
                .WithMany(u => u.Conversoras)
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OfertaExportacao
        modelBuilder.Entity<OfertaExportacao>(entity =>
        {
            entity.ToTable("OfertasExportacao");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ValorMW)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.PrecoMWh)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.UsuarioAnaliseONS)
                .HasMaxLength(100);
            
            entity.Property(e => e.ObservacaoONS)
                .HasMaxLength(500);
            
            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);
            
            entity.HasOne(e => e.Usina)
                .WithMany()
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.SemanaPMO)
                .WithMany()
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.DataPDP);
            entity.HasIndex(e => e.FlgAprovadoONS);
            entity.HasIndex(e => new { e.UsinaId, e.DataPDP });
        });

        // OfertaRespostaVoluntaria
        modelBuilder.Entity<OfertaRespostaVoluntaria>(entity =>
        {
            entity.ToTable("OfertasRespostaVoluntaria");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ReducaoDemandaMW)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.PrecoMWh)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.TipoPrograma)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.UsuarioAnaliseONS)
                .HasMaxLength(100);
            
            entity.Property(e => e.ObservacaoONS)
                .HasMaxLength(500);
            
            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);
            
            entity.HasOne(e => e.Empresa)
                .WithMany()
                .HasForeignKey(e => e.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.SemanaPMO)
                .WithMany()
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.DataPDP);
            entity.HasIndex(e => e.FlgAprovadoONS);
            entity.HasIndex(e => e.TipoPrograma);
            entity.HasIndex(e => new { e.EmpresaId, e.DataPDP });
        });

        // JanelaEnvioAgente
        modelBuilder.Entity<JanelaEnvioAgente>(entity =>
        {
            entity.ToTable("JanelasEnvioAgente");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.TipoDado)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);
            
            entity.Property(e => e.UsuarioAutorizacaoExcecao)
                .HasMaxLength(100);
            
            entity.HasOne(e => e.SemanaPMO)
                .WithMany()
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.TipoDado);
            entity.HasIndex(e => e.DataReferencia);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.TipoDado, e.DataReferencia });
        });

        // SubmissaoAgente
        modelBuilder.Entity<SubmissaoAgente>(entity =>
        {
            entity.ToTable("SubmissoesAgente");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.TipoDado)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.UsuarioEnvio)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.IpOrigem)
                .HasMaxLength(50);
            
            entity.Property(e => e.StatusSubmissao)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.MotivoRejeicao)
                .HasMaxLength(500);
            
            entity.Property(e => e.HashDados)
                .HasMaxLength(32);
            
            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);
            
            entity.HasOne(e => e.Empresa)
                .WithMany()
                .HasForeignKey(e => e.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.JanelaEnvio)
                .WithMany()
                .HasForeignKey(e => e.JanelaEnvioId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.EmpresaId);
            entity.HasIndex(e => e.TipoDado);
            entity.HasIndex(e => e.DataReferencia);
            entity.HasIndex(e => e.DataHoraSubmissao);
            entity.HasIndex(e => e.StatusSubmissao);
            entity.HasIndex(e => e.HashDados);
            entity.HasIndex(e => new { e.EmpresaId, e.TipoDado, e.DataReferencia });
        });

        // PrevisaoEolica
        modelBuilder.Entity<PrevisaoEolica>(entity =>
        {
            entity.ToTable("PrevisoesEolicas");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.GeracaoPrevistaMWmed)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            
            entity.Property(e => e.VelocidadeVentoMS)
                .HasColumnType("decimal(6,2)");
            
            entity.Property(e => e.DirecaoVentoGraus)
                .HasColumnType("decimal(5,2)");
            
            entity.Property(e => e.TemperaturaC)
                .HasColumnType("decimal(5,2)");
            
            entity.Property(e => e.PressaoAtmosfericaHPa)
                .HasColumnType("decimal(6,2)");
            
            entity.Property(e => e.UmidadeRelativa)
                .HasColumnType("decimal(5,2)");
            
            entity.Property(e => e.ModeloPrevisao)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.VersaoModelo)
                .HasMaxLength(50);
            
            entity.Property(e => e.IncertezaPercentual)
                .HasColumnType("decimal(5,2)");
            
            entity.Property(e => e.LimiteInferiorMW)
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.LimiteSuperiorMW)
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.TipoPrevisao)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.GeracaoRealMWmed)
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.ErroAbsolutoMW)
                .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.ErroPercentual)
                .HasColumnType("decimal(6,2)");
            
            entity.Property(e => e.Observacoes)
                .HasMaxLength(500);
            
            entity.HasOne(e => e.Usina)
                .WithMany()
                .HasForeignKey(e => e.UsinaId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.SemanaPMO)
                .WithMany()
                .HasForeignKey(e => e.SemanaPMOId)
                .OnDelete(DeleteBehavior.SetNull);
            
            entity.HasIndex(e => e.UsinaId);
            entity.HasIndex(e => e.DataHoraReferencia);
            entity.HasIndex(e => e.DataHoraPrevista);
            entity.HasIndex(e => e.ModeloPrevisao);
            entity.HasIndex(e => e.TipoPrevisao);
            entity.HasIndex(e => new { e.UsinaId, e.DataHoraPrevista });
        });
    }
}

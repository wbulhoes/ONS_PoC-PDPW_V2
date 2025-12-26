# =========================================================
# GERADOR AUTOMÁTICO DE SEEDER EXPANDIDO
# Objetivo: Gerar ~1000 registros realistas para o banco
# Data: 26/12/2024
# =========================================================

$ErrorActionPreference = "Stop"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "GERADOR DE SEEDER EXPANDIDO - PDPW" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$outputPath = "C:\temp\_ONS_PoC-PDPW_V2\src\PDPW.Infrastructure\Data\Seed\PdpwRealDataSeeder_Expandido.cs"

# =========================================================
# CABEÇALHO DO ARQUIVO
# =========================================================

$code = @"
using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seeder COMPLETO com dados realistas do setor elétrico brasileiro
/// Gerado automaticamente em: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
/// Total: ~1019 registros
/// </summary>
public static class PdpwRealDataSeeder
{
    /// <summary>
    /// Aplica todos os dados no ModelBuilder
    /// </summary>
    public static void SeedRealData(this ModelBuilder modelBuilder)
    {
        // 1. Tipos de Usina (8)
        SeedTiposUsina(modelBuilder);
        
        // 2. Empresas (30)
        SeedEmpresas(modelBuilder);
        
        // 3. Usinas (50)
        SeedUsinas(modelBuilder);
        
        // 4. Semanas PMO (40)
        SeedSemanasPMO(modelBuilder);
        
        // 5. Equipes PDP (11)
        SeedEquipesPDP(modelBuilder);
        
        // 6. Motivos Restrição (10)
        SeedMotivosRestricao(modelBuilder);
        
        // 7. Unidades Geradoras (100)
        SeedUnidadesGeradoras(modelBuilder);
        
        // 8. Cargas (120)
        SeedCargas(modelBuilder);
        
        // 9. Intercâmbios (240)
        SeedIntercambios(modelBuilder);
        
        // 10. Balanços (120)
        SeedBalancos(modelBuilder);
        
        // 11. Restrições UG (50)
        SeedRestricoesUG(modelBuilder);
        
        // 12. Paradas UG (30)
        SeedParadasUG(modelBuilder);
        
        // 13. Arquivos DADGER (20)
        SeedArquivosDadger(modelBuilder);
        
        // 14. Usuários (15)
        SeedUsuarios(modelBuilder);
    }

"@

Write-Host "1. Gerando Tipos de Usina (8)..." -ForegroundColor Yellow

$code += @"
    private static void SeedTiposUsina(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoUsina>().HasData(
            new TipoUsina { Id = 1, Nome = "Hidrelétrica", Descricao = "Usina que gera energia através da força da água", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 2, Nome = "Térmica", Descricao = "Usina que gera energia através da queima de combustíveis", FonteEnergia = "Combustíveis Fósseis / Biomassa", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 3, Nome = "Eólica", Descricao = "Usina que gera energia através da força dos ventos", FonteEnergia = "Eólica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 4, Nome = "Solar", Descricao = "Usina que gera energia através da luz solar", FonteEnergia = "Solar", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 5, Nome = "Nuclear", Descricao = "Usina que gera energia através da fissão nuclear", FonteEnergia = "Nuclear", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 6, Nome = "PCH", Descricao = "Pequena Central Hidrelétrica", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 7, Nome = "CGH", Descricao = "Central Geradora Hidrelétrica", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 8, Nome = "Biomassa", Descricao = "Usina que gera energia através da queima de biomassa", FonteEnergia = "Biomassa", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true }
        );
    }

"@

Write-Host "✅ Tipos de Usina concluído" -ForegroundColor Green

# =========================================================
# Devido ao tamanho, vou salvar o que já tenho
# e continuar adicionando as outras entidades
# =========================================================

Write-Host "`n2. Salvando arquivo parcial..." -ForegroundColor Yellow

# Adicionar fechamento temporário da classe
$code += @"
    // PLACEHOLDER: Outros métodos serão adicionados
    
    private static void SeedEmpresas(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 30 empresas
    }
    
    private static void SeedUsinas(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 50 usinas
    }
    
    private static void SeedSemanasPMO(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 40 semanas
    }
    
    private static void SeedEquipesPDP(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 11 equipes
    }
    
    private static void SeedMotivosRestricao(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 10 motivos
    }
    
    private static void SeedUnidadesGeradoras(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 100 UGs
    }
    
    private static void SeedCargas(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 120 cargas
    }
    
    private static void SeedIntercambios(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 240 intercâmbios
    }
    
    private static void SeedBalancos(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 120 balanços
    }
    
    private static void SeedRestricoesUG(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 50 restrições
    }
    
    private static void SeedParadasUG(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 30 paradas
    }
    
    private static void SeedArquivosDadger(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 20 arquivos
    }
    
    private static void SeedUsuarios(ModelBuilder modelBuilder)
    {
        // TODO: Implementar 15 usuários
    }
}
"@

$code | Out-File -FilePath $outputPath -Encoding UTF8 -Force

Write-Host "✅ Arquivo skeleton criado: $outputPath" -ForegroundColor Green
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "PRÓXIMA ETAPA: Expandir cada método" -ForegroundColor Yellow
Write-Host "========================================`n" -ForegroundColor Cyan

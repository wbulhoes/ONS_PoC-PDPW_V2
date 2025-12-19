namespace PDPW.Domain.Entities;

/// <summary>
/// Equipe responsável pela PDP (Programação Diária da Produção) do ONS.
/// Grupo de profissionais que realiza o planejamento e acompanhamento da operação diária do SIN.
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: EquipeProgramacaoDiaria (mantido como EquipePDP por ser sigla amplamente conhecida)
/// A PDP é o processo de planejamento operativo de curtíssimo prazo (D-1) do Sistema Interligado Nacional.
/// Cada equipe é responsável por subsistemas ou áreas específicas da operação.
/// </remarks>
public class EquipePDP : BaseEntity
{
    /// <summary>
    /// Nome identificador da equipe de programação diária
    /// </summary>
    /// <example>Equipe Sudeste, Equipe Geração Térmica, Equipe Hidráulica Norte</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição das responsabilidades e escopo de atuação da equipe
    /// </summary>
    /// <example>Responsável pela programação de usinas hidrelétricas do subsistema Sudeste/Centro-Oeste</example>
    public string? Descricao { get; set; }

    /// <summary>
    /// Nome do coordenador responsável pela equipe
    /// </summary>
    public string? Coordenador { get; set; }

    /// <summary>
    /// Email institucional da equipe para comunicações oficiais
    /// </summary>
    /// <example>equipe.pdp.se@ons.org.br</example>
    public string? Email { get; set; }

    /// <summary>
    /// Telefone de contato da equipe (ramal ou direto)
    /// </summary>
    /// <example>(21) 3444-5000 ramal 1234</example>
    public string? Telefone { get; set; }

    // Relacionamentos
    /// <summary>
    /// Usuários membros desta equipe de programação
    /// </summary>
    public ICollection<Usuario>? Membros { get; set; }
}

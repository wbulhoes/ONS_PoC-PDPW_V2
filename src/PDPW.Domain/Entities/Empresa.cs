namespace PDPW.Domain.Entities;

/// <summary>
/// Empresa responsável por usinas geradoras
/// </summary>
public class Empresa : BaseEntity
{
    /// <summary>
    /// Nome da empresa
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// CNPJ da empresa
    /// </summary>
    public string? CNPJ { get; set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Email de contato
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Endereço completo
    /// </summary>
    public string? Endereco { get; set; }

    // Relacionamentos
    public ICollection<Usina>? Usinas { get; set; }
}

namespace PDPW.Domain.Entities;

/// <summary>
/// Agente do Setor Elétrico Brasileiro autorizado pela ANEEL.
/// Pessoa jurídica que atua na geração, transmissão, distribuição ou comercialização de energia.
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: AgenteSetorEletrico (mantido como Empresa por compatibilidade)
/// Tipos de agentes: Gerador, Transmissor, Distribuidor, Comercializador
/// </remarks>
public class Empresa : BaseEntity
{
    /// <summary>
    /// Nome ou razão social do agente do setor elétrico
    /// </summary>
    /// <example>Eletrobras, CEMIG, CPFL Energia</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// CNPJ (Cadastro Nacional de Pessoa Jurídica) do agente
    /// </summary>
    /// <example>00.123.456/0001-78</example>
    public string? CNPJ { get; set; }

    /// <summary>
    /// Telefone de contato do agente
    /// </summary>
    /// <example>(21) 2211-4000</example>
    public string? Telefone { get; set; }

    /// <summary>
    /// Email de contato do agente
    /// </summary>
    /// <example>contato@agente.com.br</example>
    public string? Email { get; set; }

    /// <summary>
    /// Endereço completo da sede do agente
    /// </summary>
    public string? Endereco { get; set; }

    // Relacionamentos
    /// <summary>
    /// Usinas geradoras sob responsabilidade deste agente
    /// </summary>
    public ICollection<Usina>? Usinas { get; set; }
}

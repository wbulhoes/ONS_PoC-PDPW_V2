namespace PDPW.Application.DTOs.Empresa;

/// <summary>
/// DTO de leitura de Empresa
/// </summary>
public class EmpresaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? CNPJ { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Endereco { get; set; }
    public int QuantidadeUsinas { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}

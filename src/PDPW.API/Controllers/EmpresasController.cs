using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Empresa;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Empresas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EmpresasController : BaseController
{
    private readonly IEmpresaService _service;
    private readonly ILogger<EmpresasController> _logger;

    public EmpresasController(
        IEmpresaService service,
        ILogger<EmpresasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as empresas
    /// </summary>
    /// <returns>Lista de empresas</returns>
    /// <response code="200">Lista de empresas retornada com sucesso</response>
    [HttpGet(Name = nameof(GetAllEmpresas))]
    [ProducesResponseType(typeof(List<EmpresaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllEmpresas()
    {
        _logger.LogInformation("GET api/empresas - Buscando todas as empresas");
        
        var empresas = await _service.GetAllAsync();
        return Ok(empresas);
    }

    /// <summary>
    /// Obtém uma empresa por ID
    /// </summary>
    /// <param name="id">ID da empresa</param>
    /// <returns>Empresa encontrada</returns>
    /// <response code="200">Empresa encontrada</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("{id:int}", Name = "GetEmpresaById")]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmpresaById(int id)
    {
        _logger.LogInformation("GET api/empresas/{Id} - Buscando empresa por ID", id);
        
        var empresa = await _service.GetByIdAsync(id);
        return HandleResult(empresa);
    }

    /// <summary>
    /// Obtém uma empresa por nome
    /// </summary>
    /// <param name="nome">Nome da empresa</param>
    /// <returns>Empresa encontrada</returns>
    /// <response code="200">Empresa encontrada</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("nome/{nome}", Name = nameof(GetEmpresaByNome))]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmpresaByNome(string nome)
    {
        _logger.LogInformation("GET api/empresas/nome/{Nome} - Buscando empresa por nome", nome);
        
        var empresa = await _service.GetByNomeAsync(nome);
        return HandleResult(empresa);
    }

    /// <summary>
    /// Obtém uma empresa por CNPJ
    /// </summary>
    /// <param name="cnpj">CNPJ da empresa</param>
    /// <returns>Empresa encontrada</returns>
    /// <response code="200">Empresa encontrada</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpGet("cnpj/{cnpj}", Name = nameof(GetEmpresaByCnpj))]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmpresaByCnpj(string cnpj)
    {
        _logger.LogInformation("GET api/empresas/cnpj/{Cnpj} - Buscando empresa por CNPJ", cnpj);
        // Normaliza o CNPJ para apenas números
        var cnpjLimpo = new string(cnpj.Where(char.IsDigit).ToArray());
        var empresa = await _service.GetByCnpjAsync(cnpjLimpo);
        return HandleResult(empresa);
    }

    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    /// <param name="dto">Dados da empresa</param>
    /// <returns>Empresa criada</returns>
    /// <response code="201">Empresa criada com sucesso</response>
    /// <response code="400">Dados inválidos, nome duplicado ou CNPJ duplicado</response>
    [HttpPost(Name = nameof(CreateEmpresa))]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEmpresa([FromBody] CreateEmpresaDto dto)
    {
        _logger.LogInformation("POST api/empresas - Criando nova empresa: {Nome}", dto.Nome);

        try
        {
            var empresa = await _service.CreateAsync(dto);
            return CreatedAtRoute(
                nameof(GetEmpresaById),
                new { id = empresa.Id },
                empresa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar empresa");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    /// <param name="id">ID da empresa</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Empresa atualizada</returns>
    /// <response code="200">Empresa atualizada com sucesso</response>
    /// <response code="400">Dados inválidos, nome duplicado ou CNPJ duplicado</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpPut("{id:int}", Name = nameof(UpdateEmpresa))]
    [ProducesResponseType(typeof(EmpresaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateEmpresa(int id, [FromBody] UpdateEmpresaDto dto)
    {
        _logger.LogInformation("PUT api/empresas/{Id} - Atualizando empresa", id);

        try
        {
            var empresa = await _service.UpdateAsync(id, dto);
            return HandleResult(empresa);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar empresa");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma empresa (soft delete)
    /// </summary>
    /// <param name="id">ID da empresa</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Empresa removida com sucesso</response>
    /// <response code="400">Não é possível remover empresa com usinas vinculadas</response>
    /// <response code="404">Empresa não encontrada</response>
    [HttpDelete("{id:int}", Name = nameof(DeleteEmpresa))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmpresa(int id)
    {
        _logger.LogInformation("DELETE api/empresas/{Id} - Removendo empresa", id);

        try
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = $"Empresa com ID {id} não encontrada" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao tentar remover empresa com usinas vinculadas");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o nome informado
    /// </summary>
    /// <param name="nome">Nome a verificar</param>
    /// <param name="empresaId">ID da empresa a excluir da verificação (opcional)</param>
    /// <returns>Indica se o nome já existe</returns>
    /// <response code="200">Resultado da verificação</response>
    [HttpGet("verificar-nome/{nome}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarNomeExiste(string nome, [FromQuery] int? empresaId = null)
    {
        _logger.LogInformation("GET api/empresas/verificar-nome/{Nome} - Verificando existência de nome", nome);
        
        var existe = await _service.ExisteNomeAsync(nome, empresaId);
        return Ok(new { existe });
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    /// <param name="cnpj">CNPJ a verificar</param>
    /// <param name="empresaId">ID da empresa a excluir da verificação (opcional)</param>
    /// <returns>Indica se o CNPJ já existe</returns>
    /// <response code="200">Resultado da verificação</response>
    [HttpGet("verificar-cnpj/{cnpj}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarCnpjExiste(string cnpj, [FromQuery] int? empresaId = null)
    {
        _logger.LogInformation("GET api/empresas/verificar-cnpj/{Cnpj} - Verificando existência de CNPJ", cnpj);
        
        var existe = await _service.ExisteCnpjAsync(cnpj, empresaId);
        return Ok(new { existe });
    }

    /// <summary>
    /// Busca empresas por termo (nome ou CNPJ)
    /// </summary>
    /// <param name="termo">Termo de busca</param>
    /// <returns>Lista de empresas que correspondem ao termo</returns>
    /// <response code="200">Lista de empresas retornada com sucesso</response>
    [HttpGet("buscar", Name = nameof(BuscarEmpresas))]
    [ProducesResponseType(typeof(List<EmpresaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> BuscarEmpresas([FromQuery] string termo)
    {
        _logger.LogInformation("GET api/empresas/buscar?termo={Termo} - Buscando empresas", termo);
        
        var empresas = await _service.GetAllAsync();
        var filtradas = empresas.Where(e => 
            e.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) ||
            (e.CNPJ != null && e.CNPJ.Contains(termo, StringComparison.OrdinalIgnoreCase))
        ).ToList();
        
        return Ok(filtradas);
    }
}

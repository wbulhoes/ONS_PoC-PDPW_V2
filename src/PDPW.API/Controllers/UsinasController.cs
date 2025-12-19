using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Usinas Geradoras
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: UsinasGeradorasController (mantido como UsinasController por compatibilidade)
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsinasController : BaseController
{
    private readonly IUsinaService _service;
    private readonly ILogger<UsinasController> _logger;

    public UsinasController(IUsinaService service, ILogger<UsinasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as usinas geradoras
    /// </summary>
    /// <returns>Lista de usinas</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsinaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Dados da usina</returns>
    /// <response code="200">Usina encontrada</response>
    /// <response code="404">Usina não encontrada</response>
    [HttpGet("{id:int}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém usina por código
    /// </summary>
    /// <param name="codigo">Código único da usina</param>
    /// <returns>Dados da usina</returns>
    /// <response code="200">Usina encontrada</response>
    /// <response code="404">Usina não encontrada</response>
    [HttpGet("codigo/{codigo}")]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var result = await _service.GetByCodigoAsync(codigo);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    /// <param name="tipoUsinaId">ID do tipo de usina</param>
    /// <returns>Lista de usinas do tipo especificado</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("tipo/{tipoUsinaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<UsinaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTipo(int tipoUsinaId)
    {
        var result = await _service.GetByTipoAsync(tipoUsinaId);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    /// <param name="empresaId">ID da empresa</param>
    /// <returns>Lista de usinas da empresa especificada</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("empresa/{empresaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<UsinaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmpresa(int empresaId)
    {
        var result = await _service.GetByEmpresaAsync(empresaId);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Cria nova usina
    /// </summary>
    /// <param name="createDto">Dados da usina</param>
    /// <returns>Usina criada</returns>
    /// <response code="201">Usina criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="409">Código já existe</response>
    [HttpPost]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUsinaDto createDto)
    {
        var result = await _service.CreateAsync(createDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Usina {UsinaId} criada com sucesso: {UsinaNome}", 
                result.Value!.Id, result.Value.Nome);
        }
        
        return result.ToCreatedAtActionResult(this, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <param name="updateDto">Dados atualizados</param>
    /// <returns>Usina atualizada</returns>
    /// <response code="200">Usina atualizada com sucesso</response>
    /// <response code="404">Usina não encontrada</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="409">Código já existe</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUsinaDto updateDto)
    {
        var result = await _service.UpdateAsync(id, updateDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Usina {UsinaId} atualizada com sucesso", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Remove usina
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Usina removida com sucesso</response>
    /// <response code="404">Usina não encontrada</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Usina {UsinaId} removida com sucesso", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    /// <param name="codigo">Código a verificar</param>
    /// <param name="usinaId">ID da usina a excluir da verificação (opcional)</param>
    /// <returns>True se código existe, False caso contrário</returns>
    /// <response code="200">Verificação realizada</response>
    [HttpGet("verificar-codigo/{codigo}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarCodigo(string codigo, [FromQuery] int? usinaId = null)
    {
        var existe = await _service.CodigoExisteAsync(codigo, usinaId);
        return Ok(new { existe });
    }
}

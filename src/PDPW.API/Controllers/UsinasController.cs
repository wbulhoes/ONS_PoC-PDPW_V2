using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Usinas
/// </summary>
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
    /// Obtém todas as usinas
    /// </summary>
    /// <returns>Lista de usinas</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsinaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var usinas = await _service.GetAllAsync();
            return HandleCollectionResult(usinas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as usinas");
            return HandleError(ex);
        }
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
        try
        {
            var usina = await _service.GetByIdAsync(id);
            return HandleResult(usina);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usina {UsinaId}", id);
            return HandleError(ex);
        }
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
        try
        {
            var usina = await _service.GetByCodigoAsync(codigo);
            return HandleResult(usina);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usina por código {Codigo}", codigo);
            return HandleError(ex);
        }
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
        try
        {
            var usinas = await _service.GetByTipoAsync(tipoUsinaId);
            return HandleCollectionResult(usinas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usinas por tipo {TipoUsinaId}", tipoUsinaId);
            return HandleError(ex);
        }
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
        try
        {
            var usinas = await _service.GetByEmpresaAsync(empresaId);
            return HandleCollectionResult(usinas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usinas por empresa {EmpresaId}", empresaId);
            return HandleError(ex);
        }
    }

    /// <summary>
    /// Cria nova usina
    /// </summary>
    /// <param name="createDto">Dados da usina</param>
    /// <returns>Usina criada</returns>
    /// <response code="201">Usina criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUsinaDto createDto)
    {
        try
        {
            var usina = await _service.CreateAsync(createDto);
            _logger.LogInformation("Usina {UsinaId} criada com sucesso: {UsinaNome}", usina.Id, usina.Nome);
            return HandleCreated(nameof(GetById), new { id = usina.Id }, usina);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar usina");
            return HandleValidationError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usina");
            return HandleError(ex);
        }
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
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(UsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUsinaDto updateDto)
    {
        try
        {
            var usina = await _service.UpdateAsync(id, updateDto);
            _logger.LogInformation("Usina {UsinaId} atualizada com sucesso", id);
            return Ok(usina);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Usina {UsinaId} não encontrada para atualização", id);
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar usina {UsinaId}", id);
            return HandleValidationError(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usina {UsinaId}", id);
            return HandleError(ex);
        }
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
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Usina com ID {id} não encontrada" });
            }

            _logger.LogInformation("Usina {UsinaId} removida com sucesso", id);
            return HandleNoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover usina {UsinaId}", id);
            return HandleError(ex);
        }
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
        try
        {
            var existe = await _service.CodigoExisteAsync(codigo, usinaId);
            return Ok(new { existe });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar código {Codigo}", codigo);
            return HandleError(ex);
        }
    }
}

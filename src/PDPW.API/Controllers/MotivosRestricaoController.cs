using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.MotivoRestricao;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Motivos de Restrição
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class MotivosRestricaoController : ControllerBase
{
    private readonly IMotivoRestricaoService _service;
    private readonly ILogger<MotivosRestricaoController> _logger;

    public MotivosRestricaoController(
        IMotivoRestricaoService service,
        ILogger<MotivosRestricaoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os motivos de restrição
    /// </summary>
    /// <returns>Lista de motivos de restrição</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotivoRestricaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MotivoRestricaoDto>>> GetAll()
    {
        try
        {
            var motivos = await _service.GetAllAsync();
            return Ok(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os motivos de restrição");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca um motivo de restrição por ID
    /// </summary>
    /// <param name="id">ID do motivo de restrição</param>
    /// <returns>Motivo de restrição encontrado</returns>
    /// <response code="200">Motivo de restrição encontrado</response>
    /// <response code="404">Motivo de restrição não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotivoRestricaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotivoRestricaoDto>> GetById(int id)
    {
        try
        {
            var motivo = await _service.GetByIdAsync(id);
            if (motivo == null)
            {
                return NotFound($"Motivo de restrição com ID {id} não encontrado");
            }

            return Ok(motivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivo de restrição com ID {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista motivos de restrição por categoria
    /// </summary>
    /// <param name="categoria">Categoria do motivo (ex: PROGRAMADA, EMERGENCIAL, MANUTENÇÃO)</param>
    /// <returns>Lista de motivos da categoria especificada</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("categoria/{categoria}")]
    [ProducesResponseType(typeof(IEnumerable<MotivoRestricaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MotivoRestricaoDto>>> GetByCategoria(string categoria)
    {
        try
        {
            var motivos = await _service.GetByCategoriaAsync(categoria);
            return Ok(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivos de restrição da categoria {Categoria}", categoria);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista apenas motivos de restrição ativos
    /// </summary>
    /// <returns>Lista de motivos de restrição ativos</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<MotivoRestricaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MotivoRestricaoDto>>> GetAtivos()
    {
        try
        {
            var motivos = await _service.GetAtivasAsync();
            return Ok(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivos de restrição ativos");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria um novo motivo de restrição
    /// </summary>
    /// <param name="dto">Dados do novo motivo de restrição</param>
    /// <returns>Motivo de restrição criado</returns>
    /// <response code="201">Motivo de restrição criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(MotivoRestricaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotivoRestricaoDto>> Create([FromBody] CreateMotivoRestricaoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var motivo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = motivo.Id }, motivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar motivo de restrição");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza um motivo de restrição existente
    /// </summary>
    /// <param name="id">ID do motivo de restrição</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Motivo de restrição atualizado</returns>
    /// <response code="200">Motivo de restrição atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Motivo de restrição não encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MotivoRestricaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotivoRestricaoDto>> Update(int id, [FromBody] UpdateMotivoRestricaoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var motivo = await _service.UpdateAsync(id, dto);
            return Ok(motivo);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Motivo de restrição {Id} não encontrado para atualização", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar motivo de restrição {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove um motivo de restrição (soft delete)
    /// </summary>
    /// <param name="id">ID do motivo de restrição</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Motivo de restrição removido com sucesso</response>
    /// <response code="400">Não é possível remover (há restrições associadas)</response>
    /// <response code="404">Motivo de restrição não encontrado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Motivo de restrição com ID {id} não encontrado");
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Tentativa de remover motivo de restrição {Id} com restrições associadas", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover motivo de restrição {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}

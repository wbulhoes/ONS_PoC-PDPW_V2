using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.EquipePdp;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Equipes PDP
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Tags("Gestão de Equipes")]
public class EquipesPdpController : ControllerBase
{
    private readonly IEquipePdpService _service;
    private readonly ILogger<EquipesPdpController> _logger;

    public EquipesPdpController(IEquipePdpService service, ILogger<EquipesPdpController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as equipes PDP
    /// </summary>
    /// <returns>Lista de equipes PDP</returns>
    /// <response code="200">Lista de equipes PDP retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EquipePdpDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EquipePdpDto>>> GetAll()
    {
        try
        {
            var equipes = await _service.GetAllAsync();
            return Ok(equipes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter todas as equipes PDP");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Obtém uma equipe PDP por ID
    /// </summary>
    /// <param name="id">ID da equipe PDP</param>
    /// <returns>Equipe PDP encontrada</returns>
    /// <response code="200">Equipe PDP encontrada</response>
    /// <response code="404">Equipe PDP não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EquipePdpDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipePdpDto>> GetById(int id)
    {
        try
        {
            var equipe = await _service.GetByIdAsync(id);
            if (equipe == null)
            {
                return NotFound($"Equipe PDP com ID {id} não encontrada");
            }
            return Ok(equipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter equipe PDP por ID: {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Obtém uma equipe PDP por nome
    /// </summary>
    /// <param name="nome">Nome da equipe PDP</param>
    /// <returns>Equipe PDP encontrada</returns>
    /// <response code="200">Equipe PDP encontrada</response>
    /// <response code="404">Equipe PDP não encontrada</response>
    [HttpGet("nome/{nome}")]
    [ProducesResponseType(typeof(EquipePdpDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipePdpDto>> GetByNome(string nome)
    {
        try
        {
            var equipe = await _service.GetByNomeAsync(nome);
            if (equipe == null)
            {
                return NotFound($"Equipe PDP '{nome}' não encontrada");
            }
            return Ok(equipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter equipe PDP por nome: {Nome}", nome);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Obtém uma equipe PDP com seus membros
    /// </summary>
    /// <param name="id">ID da equipe PDP</param>
    /// <returns>Equipe PDP com membros</returns>
    /// <response code="200">Equipe PDP encontrada com membros</response>
    /// <response code="404">Equipe PDP não encontrada</response>
    [HttpGet("{id}/membros")]
    [ProducesResponseType(typeof(EquipePdpDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipePdpDto>> GetComMembros(int id)
    {
        try
        {
            var equipe = await _service.GetComMembrosAsync(id);
            if (equipe == null)
            {
                return NotFound($"Equipe PDP com ID {id} não encontrada");
            }
            return Ok(equipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter equipe PDP com membros: {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria uma nova equipe PDP
    /// </summary>
    /// <param name="dto">Dados da equipe PDP</param>
    /// <returns>Equipe PDP criada</returns>
    /// <response code="201">Equipe PDP criada com sucesso</response>
    /// <response code="400">Dados inválidos ou nome duplicado</response>
    [HttpPost]
    [ProducesResponseType(typeof(EquipePdpDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EquipePdpDto>> Create([FromBody] CreateEquipePdpDto dto)
    {
        try
        {
            var equipe = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = equipe.Id }, equipe);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar equipe PDP");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza uma equipe PDP existente
    /// </summary>
    /// <param name="id">ID da equipe PDP</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Equipe PDP atualizada</returns>
    /// <response code="200">Equipe PDP atualizada com sucesso</response>
    /// <response code="400">Dados inválidos ou nome duplicado</response>
    /// <response code="404">Equipe PDP não encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EquipePdpDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipePdpDto>> Update(int id, [FromBody] UpdateEquipePdpDto dto)
    {
        try
        {
            var equipe = await _service.UpdateAsync(id, dto);
            if (equipe == null)
            {
                return NotFound($"Equipe PDP com ID {id} não encontrada");
            }
            return Ok(equipe);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar equipe PDP: {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove uma equipe PDP (soft delete)
    /// </summary>
    /// <param name="id">ID da equipe PDP</param>
    /// <returns>Confirmação da remoção</returns>
    /// <response code="204">Equipe PDP removida com sucesso</response>
    /// <response code="400">Não é possível remover equipe com membros vinculados</response>
    /// <response code="404">Equipe PDP não encontrada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound($"Equipe PDP com ID {id} não encontrada");
            }
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover equipe PDP: {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Verifica se já existe uma equipe PDP com o nome informado
    /// </summary>
    /// <param name="nome">Nome da equipe</param>
    /// <param name="equipePdpId">ID da equipe a excluir da verificação (opcional)</param>
    /// <returns>Indica se já existe</returns>
    /// <response code="200">Resultado da verificação</response>
    [HttpGet("verificar-nome")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> VerificarNome(
        [FromQuery] string nome, 
        [FromQuery] int? equipePdpId = null)
    {
        try
        {
            var existe = await _service.ExisteNomeAsync(nome, equipePdpId);
            return Ok(new { existe });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao verificar nome da equipe PDP");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}

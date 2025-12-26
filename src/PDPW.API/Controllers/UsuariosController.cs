using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Usuario;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller de Usuários
/// </summary>
[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _service;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService service, ILogger<UsuariosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os usuários ativos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
    {
        var usuarios = await _service.GetAllAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetById(int id)
    {
        var usuario = await _service.GetByIdAsync(id);
        
        if (usuario == null)
        {
            return NotFound(new { message = $"Usuário com ID {id} não encontrado" });
        }

        return Ok(usuario);
    }

    /// <summary>
    /// Busca usuários por perfil
    /// </summary>
    [HttpGet("perfil/{perfil}")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetByPerfil(string perfil)
    {
        var usuarios = await _service.GetByPerfilAsync(perfil);
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca usuários por equipe PDP
    /// </summary>
    [HttpGet("equipe/{equipePdpId}")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetByEquipe(int equipePdpId)
    {
        var usuarios = await _service.GetByEquipeAsync(equipePdpId);
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca usuário por email
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UsuarioDto>> GetByEmail(string email)
    {
        var usuario = await _service.GetByEmailAsync(email);
        
        if (usuario == null)
        {
            return NotFound(new { message = $"Usuário com email {email} não encontrado" });
        }

        return Ok(usuario);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Create([FromBody] CreateUsuarioDto dto)
    {
        try
        {
            var usuario = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UsuarioDto>> Update(int id, [FromBody] UpdateUsuarioDto dto)
    {
        try
        {
            var usuario = await _service.UpdateAsync(id, dto);
            return Ok(usuario);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove um usuário (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}

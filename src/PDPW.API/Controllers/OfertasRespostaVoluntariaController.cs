using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.OfertaRespostaVoluntaria;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

namespace PDPW.API.Controllers;

[ApiController]
[Route("api/ofertas-resposta-voluntaria")]
[Produces("application/json")]
public class OfertasRespostaVoluntariaController : BaseController
{
    private readonly IOfertaRespostaVoluntariaService _service;
    private readonly ILogger<OfertasRespostaVoluntariaController> _logger;

    public OfertasRespostaVoluntariaController(
        IOfertaRespostaVoluntariaService service, 
        ILogger<OfertasRespostaVoluntariaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    [HttpGet("{id:int}", Name = nameof(GetOfertaRespostaById))]
    [ProducesResponseType(typeof(OfertaRespostaVoluntariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOfertaRespostaById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    [HttpGet("pendentes")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendentes()
    {
        var result = await _service.GetPendentesAsync();
        return result.ToActionResult(this);
    }

    [HttpGet("aprovadas")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAprovadas()
    {
        var result = await _service.GetAprovadasAsync();
        return result.ToActionResult(this);
    }

    [HttpGet("rejeitadas")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRejeitadas()
    {
        var result = await _service.GetRejeitadasAsync();
        return result.ToActionResult(this);
    }

    [HttpGet("empresa/{empresaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmpresa(int empresaId)
    {
        var result = await _service.GetByEmpresaAsync(empresaId);
        return result.ToActionResult(this);
    }

    [HttpGet("data-pdp/{dataPDP:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDataPDP(DateTime dataPDP)
    {
        var result = await _service.GetByDataPDPAsync(dataPDP);
        return result.ToActionResult(this);
    }

    [HttpGet("tipo-programa/{tipoPrograma}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaRespostaVoluntariaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTipoPrograma(string tipoPrograma)
    {
        var result = await _service.GetByTipoProgramaAsync(tipoPrograma);
        return result.ToActionResult(this);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OfertaRespostaVoluntariaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOfertaRespostaVoluntariaDto createDto)
    {
        var result = await _service.CreateAsync(createDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Resposta Voluntária {Id} criada - Empresa {EmpresaId}", 
                result.Value!.Id, result.Value.EmpresaId);
        }
        
        return result.ToCreatedAtActionResult(this, nameof(GetOfertaRespostaById), new { id = result.Value?.Id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OfertaRespostaVoluntariaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOfertaRespostaVoluntariaDto updateDto)
    {
        var result = await _service.UpdateAsync(id, updateDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Resposta Voluntária {Id} atualizada", id);
        }
        
        return result.ToActionResult(this);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Resposta Voluntária {Id} removida", id);
        }
        
        return result.ToActionResult(this);
    }

    [HttpPost("{id:int}/aprovar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Aprovar(int id, [FromBody] AprovarOfertaRespostaVoluntariaDto aprovarDto)
    {
        var result = await _service.AprovarAsync(id, aprovarDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Resposta Voluntária {Id} APROVADA pelo ONS - Usuário: {Usuario}", 
                id, aprovarDto.UsuarioONS);
        }
        
        return result.ToActionResult(this);
    }

    [HttpPost("{id:int}/rejeitar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Rejeitar(int id, [FromBody] RejeitarOfertaRespostaVoluntariaDto rejeitarDto)
    {
        var result = await _service.RejeitarAsync(id, rejeitarDto);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning("Oferta de Resposta Voluntária {Id} REJEITADA pelo ONS - Usuário: {Usuario}", 
                id, rejeitarDto.UsuarioONS);
        }
        
        return result.ToActionResult(this);
    }
}

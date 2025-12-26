using AutoMapper;
using Moq;
using PDPW.Application.DTOs.OfertaRespostaVoluntaria;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using Xunit;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para OfertaRespostaVoluntariaService
/// </summary>
public class OfertaRespostaVoluntariaServiceTests
{
    private readonly Mock<IOfertaRespostaVoluntariaRepository> _mockRepository;
    private readonly Mock<IEmpresaRepository> _mockEmpresaRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OfertaRespostaVoluntariaService _service;

    public OfertaRespostaVoluntariaServiceTests()
    {
        _mockRepository = new Mock<IOfertaRespostaVoluntariaRepository>();
        _mockEmpresaRepository = new Mock<IEmpresaRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new OfertaRespostaVoluntariaService(
            _mockRepository.Object,
            _mockEmpresaRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasOfertas_QuandoChamado()
    {
        // Arrange
        var ofertas = new List<OfertaRespostaVoluntaria>
        {
            new() { Id = 1, EmpresaId = 1, ReducaoDemandaMW = 50 },
            new() { Id = 2, EmpresaId = 2, ReducaoDemandaMW = 100 }
        };

        var dtos = new List<OfertaRespostaVoluntariaDto>
        {
            new() { Id = 1, EmpresaId = 1, ReducaoDemandaMW = 50 },
            new() { Id = 2, EmpresaId = 2, ReducaoDemandaMW = 100 }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(ofertas);
        _mockMapper.Setup(m => m.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas)).Returns(dtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarOferta_QuandoOfertaExiste()
    {
        // Arrange
        var oferta = new OfertaRespostaVoluntaria { Id = 1, EmpresaId = 1 };
        var dto = new OfertaRespostaVoluntariaDto { Id = 1, EmpresaId = 1 };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockMapper.Setup(m => m.Map<OfertaRespostaVoluntariaDto>(oferta)).Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_DeveCriarOferta_QuandoDadosValidos()
    {
        // Arrange
        var createDto = new CreateOfertaRespostaVoluntariaDto
        {
            EmpresaId = 1,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(2),
            ReducaoDemandaMW = 50,
            PrecoMWh = 100,
            HoraInicial = new TimeSpan(8, 0, 0),
            HoraFinal = new TimeSpan(18, 0, 0),
            TipoPrograma = "Interruptível"
        };

        var empresa = new Empresa { Id = 1, Nome = "Empresa Teste" };
        var oferta = new OfertaRespostaVoluntaria { Id = 1, EmpresaId = 1 };
        var dto = new OfertaRespostaVoluntariaDto { Id = 1, EmpresaId = 1 };

        _mockEmpresaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _mockMapper.Setup(m => m.Map<OfertaRespostaVoluntaria>(createDto)).Returns(oferta);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<OfertaRespostaVoluntaria>())).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockMapper.Setup(m => m.Map<OfertaRespostaVoluntariaDto>(oferta)).Returns(dto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task CreateAsync_DeveRetornarFalha_QuandoEmpresaNaoExiste()
    {
        // Arrange
        var createDto = new CreateOfertaRespostaVoluntariaDto
        {
            EmpresaId = 999,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(2),
            ReducaoDemandaMW = 50,
            PrecoMWh = 100,
            HoraInicial = new TimeSpan(8, 0, 0),
            HoraFinal = new TimeSpan(18, 0, 0),
            TipoPrograma = "Interruptível"
        };

        _mockEmpresaRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Empresa?)null);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Empresa", result.Error);
    }

    [Fact]
    public async Task AprovarAsync_DeveAprovarOferta_QuandoOfertaPendente()
    {
        // Arrange
        var oferta = new OfertaRespostaVoluntaria
        {
            Id = 1,
            EmpresaId = 1,
            FlgAprovadoONS = null
        };

        var aprovarDto = new AprovarOfertaRespostaVoluntariaDto
        {
            UsuarioONS = "teste@ons.org.br",
            Observacao = "Aprovada"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);

        // Act
        var result = await _service.AprovarAsync(1, aprovarDto);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RejeitarAsync_DeveRejeitarOferta_QuandoOfertaPendente()
    {
        // Arrange
        var oferta = new OfertaRespostaVoluntaria
        {
            Id = 1,
            EmpresaId = 1,
            FlgAprovadoONS = null
        };

        var rejeitarDto = new RejeitarOfertaRespostaVoluntariaDto
        {
            UsuarioONS = "teste@ons.org.br",
            Observacao = "Rejeitada"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);

        // Act
        var result = await _service.RejeitarAsync(1, rejeitarDto);

        // Assert
        Assert.True(result.IsSuccess);
    }
}

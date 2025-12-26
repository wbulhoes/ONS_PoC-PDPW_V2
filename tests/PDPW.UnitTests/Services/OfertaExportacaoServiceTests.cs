using AutoMapper;
using Moq;
using PDPW.Application.DTOs.OfertaExportacao;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using Xunit;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para OfertaExportacaoService
/// Nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
/// </summary>
public class OfertaExportacaoServiceTests
{
    private readonly Mock<IOfertaExportacaoRepository> _mockRepository;
    private readonly Mock<IUsinaRepository> _mockUsinaRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OfertaExportacaoService _service;

    public OfertaExportacaoServiceTests()
    {
        _mockRepository = new Mock<IOfertaExportacaoRepository>();
        _mockUsinaRepository = new Mock<IUsinaRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new OfertaExportacaoService(
            _mockRepository.Object,
            _mockUsinaRepository.Object,
            _mockMapper.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasOfertas_QuandoChamado()
    {
        // Arrange
        var ofertas = new List<OfertaExportacao>
        {
            new() { Id = 1, UsinaId = 1, ValorMW = 100 },
            new() { Id = 2, UsinaId = 2, ValorMW = 200 }
        };

        var dtos = new List<OfertaExportacaoDto>
        {
            new() { Id = 1, UsinaId = 1, ValorMW = 100 },
            new() { Id = 2, UsinaId = 2, ValorMW = 200 }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(ofertas);
        _mockMapper.Setup(m => m.Map<IEnumerable<OfertaExportacaoDto>>(ofertas)).Returns(dtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(2, result.Value.Count());
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_DeveRetornarOferta_QuandoOfertaExiste()
    {
        // Arrange
        var oferta = new OfertaExportacao { Id = 1, UsinaId = 1, ValorMW = 100 };
        var dto = new OfertaExportacaoDto { Id = 1, UsinaId = 1, ValorMW = 100 };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockMapper.Setup(m => m.Map<OfertaExportacaoDto>(oferta)).Returns(dto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarFalha_QuandoOfertaNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((OfertaExportacao?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("não foi encontrada", result.Error);
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_DeveCriarOferta_QuandoDadosValidos()
    {
        // Arrange
        var createDto = new CreateOfertaExportacaoDto
        {
            UsinaId = 1,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(2),
            ValorMW = 100,
            PrecoMWh = 250,
            HoraInicial = new TimeSpan(8, 0, 0),
            HoraFinal = new TimeSpan(18, 0, 0)
        };

        var usina = new Usina { Id = 1, Nome = "Usina Teste" };
        var oferta = new OfertaExportacao { Id = 1, UsinaId = 1, ValorMW = 100 };
        var dto = new OfertaExportacaoDto { Id = 1, UsinaId = 1, ValorMW = 100 };

        _mockUsinaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);
        _mockMapper.Setup(m => m.Map<OfertaExportacao>(createDto)).Returns(oferta);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<OfertaExportacao>())).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockMapper.Setup(m => m.Map<OfertaExportacaoDto>(oferta)).Returns(dto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(1, result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_DeveRetornarFalha_QuandoUsinaNaoExiste()
    {
        // Arrange
        var createDto = new CreateOfertaExportacaoDto
        {
            UsinaId = 999,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(2),
            ValorMW = 100,
            PrecoMWh = 250,
            HoraInicial = new TimeSpan(8, 0, 0),
            HoraFinal = new TimeSpan(18, 0, 0)
        };

        _mockUsinaRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usina?)null);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Usina", result.Error);
        Assert.Contains("não encontrada", result.Error);
    }

    [Fact]
    public async Task CreateAsync_DeveRetornarFalha_QuandoHoraFinalMenorQueInicial()
    {
        // Arrange
        var createDto = new CreateOfertaExportacaoDto
        {
            UsinaId = 1,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(2),
            ValorMW = 100,
            PrecoMWh = 250,
            HoraInicial = new TimeSpan(18, 0, 0),
            HoraFinal = new TimeSpan(8, 0, 0) // Hora final menor que inicial
        };

        var usina = new Usina { Id = 1 };
        _mockUsinaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Hora final deve ser maior que hora inicial", result.Error);
    }

    [Fact]
    public async Task CreateAsync_DeveRetornarFalha_QuandoDataPDPNoPassado()
    {
        // Arrange
        var createDto = new CreateOfertaExportacaoDto
        {
            UsinaId = 1,
            DataOferta = DateTime.Now,
            DataPDP = DateTime.Now.AddDays(-1), // Data no passado
            ValorMW = 100,
            PrecoMWh = 250,
            HoraInicial = new TimeSpan(8, 0, 0),
            HoraFinal = new TimeSpan(18, 0, 0)
        };

        var usina = new Usina { Id = 1 };
        _mockUsinaRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Data do PDP não pode ser no passado", result.Error);
    }

    #endregion

    #region AprovarAsync Tests

    [Fact]
    public async Task AprovarAsync_DeveAprovarOferta_QuandoOfertaPendente()
    {
        // Arrange
        var oferta = new OfertaExportacao
        {
            Id = 1,
            UsinaId = 1,
            FlgAprovadoONS = null // Pendente
        };

        var aprovarDto = new AprovarOfertaExportacaoDto
        {
            UsuarioONS = "teste@ons.org.br",
            Observacao = "Aprovada"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.AprovarAsync(1, aprovarDto.UsuarioONS, aprovarDto.Observacao))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AprovarAsync(1, aprovarDto);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(r => r.AprovarAsync(1, aprovarDto.UsuarioONS, aprovarDto.Observacao), Times.Once);
    }

    [Fact]
    public async Task AprovarAsync_DeveRetornarFalha_QuandoOfertaJaAnalisada()
    {
        // Arrange
        var oferta = new OfertaExportacao
        {
            Id = 1,
            UsinaId = 1,
            FlgAprovadoONS = true, // Já aprovada
            DataAnaliseONS = DateTime.Now
        };

        var aprovarDto = new AprovarOfertaExportacaoDto
        {
            UsuarioONS = "teste@ons.org.br"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);

        // Act
        var result = await _service.AprovarAsync(1, aprovarDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("já foi", result.Error);
    }

    #endregion

    #region RejeitarAsync Tests

    [Fact]
    public async Task RejeitarAsync_DeveRejeitarOferta_QuandoOfertaPendente()
    {
        // Arrange
        var oferta = new OfertaExportacao
        {
            Id = 1,
            UsinaId = 1,
            FlgAprovadoONS = null // Pendente
        };

        var rejeitarDto = new RejeitarOfertaExportacaoDto
        {
            UsuarioONS = "teste@ons.org.br",
            Observacao = "Rejeitada - preço muito alto"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.RejeitarAsync(1, rejeitarDto.UsuarioONS, rejeitarDto.Observacao))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.RejeitarAsync(1, rejeitarDto);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(r => r.RejeitarAsync(1, rejeitarDto.UsuarioONS, rejeitarDto.Observacao), Times.Once);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_DeveExcluirOferta_QuandoOfertaPendente()
    {
        // Arrange
        var oferta = new OfertaExportacao
        {
            Id = 1,
            UsinaId = 1,
            DataPDP = DateTime.Now.AddDays(2),
            FlgAprovadoONS = null // Pendente
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalha_QuandoOfertaJaAnalisada()
    {
        // Arrange
        var oferta = new OfertaExportacao
        {
            Id = 1,
            UsinaId = 1,
            DataPDP = DateTime.Now.AddDays(2),
            FlgAprovadoONS = true // Já analisada
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(oferta);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("já foi analisada", result.Error);
    }

    #endregion
}

using AutoMapper;
using FluentAssertions;
using Moq;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.UnitTests.Helpers;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para UsinaService
/// </summary>
public class UsinaServiceTests
{
    private readonly Mock<IUsinaRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UsinaService _service;

    public UsinaServiceTests()
    {
        _mockRepository = new Mock<IUsinaRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new UsinaService(_mockRepository.Object, _mockMapper.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoExistemUsinas()
    {
        // Arrange
        var usinas = TestDataBuilder.Usinas.CreateList(3);
        var usinasDto = usinas.Select(u => new UsinaDto { Id = u.Id, Nome = u.Nome, Codigo = u.Codigo }).ToList();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(usinas);
        _mockMapper.Setup(m => m.Map<IEnumerable<UsinaDto>>(usinas)).Returns(usinasDto);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(3);
        result.Value.Should().BeEquivalentTo(usinasDto);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoExistemUsinas()
    {
        // Arrange
        var usinasVazio = new List<Usina>();
        var usinasDtoVazio = new List<UsinaDto>();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(usinasVazio);
        _mockMapper.Setup(m => m.Map<IEnumerable<UsinaDto>>(usinasVazio)).Returns(usinasDtoVazio);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_DeveRetornarSucesso_QuandoUsinaExiste()
    {
        // Arrange
        var usina = TestDataBuilder.Usinas.CreateValid(1, "UTE001", "Usina Teste 1");
        var usinaDto = new UsinaDto { Id = usina.Id, Nome = usina.Nome, Codigo = usina.Codigo };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);
        _mockMapper.Setup(m => m.Map<UsinaDto>(usina)).Returns(usinaDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Id.Should().Be(1);
        result.Value.Nome.Should().Be("Usina Teste 1");
        result.Value.Codigo.Should().Be("UTE001");
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNotFound_QuandoUsinaNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usina?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("não foi encontrado");
    }

    #endregion

    #region GetByCodigoAsync Tests

    [Fact]
    public async Task GetByCodigoAsync_DeveRetornarSucesso_QuandoUsinaExiste()
    {
        // Arrange
        var usina = TestDataBuilder.Usinas.CreateValid(1, "UTE001", "Usina Teste 1");
        var usinaDto = new UsinaDto { Id = usina.Id, Nome = usina.Nome, Codigo = usina.Codigo };

        _mockRepository.Setup(r => r.GetByCodigoAsync("UTE001")).ReturnsAsync(usina);
        _mockMapper.Setup(m => m.Map<UsinaDto>(usina)).Returns(usinaDto);

        // Act
        var result = await _service.GetByCodigoAsync("UTE001");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Codigo.Should().Be("UTE001");
    }

    [Fact]
    public async Task GetByCodigoAsync_DeveRetornarNotFound_QuandoUsinaNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByCodigoAsync("INEXISTENTE")).ReturnsAsync((Usina?)null);

        // Act
        var result = await _service.GetByCodigoAsync("INEXISTENTE");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("não foi encontrada");
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var createDto = new CreateUsinaDto
        {
            Codigo = "UTE999",
            Nome = "Nova Usina",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000,
            Localizacao = "São Paulo, SP",
            DataOperacao = DateTime.Now
        };

        var usina = new Usina { Id = 1, Codigo = createDto.Codigo, Nome = createDto.Nome };
        var usinaDto = new UsinaDto { Id = 1, Codigo = createDto.Codigo, Nome = createDto.Nome };

        _mockRepository.Setup(r => r.CodigoExisteAsync(createDto.Codigo, null)).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map<Usina>(createDto)).Returns(usina);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Usina>())).ReturnsAsync(usina);
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);
        _mockMapper.Setup(m => m.Map<UsinaDto>(usina)).Returns(usinaDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Codigo.Should().Be("UTE999");
    }

    [Fact]
    public async Task CreateAsync_DeveRetornarConflict_QuandoCodigoJaExiste()
    {
        // Arrange
        var createDto = new CreateUsinaDto
        {
            Codigo = "UTE001",
            Nome = "Nova Usina",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000
        };

        _mockRepository.Setup(r => r.CodigoExisteAsync(createDto.Codigo, null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Já existe uma usina com o código");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Usina>()), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var updateDto = new UpdateUsinaDto
        {
            Codigo = "UTE001",
            Nome = "Usina Atualizada",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1500,
            Localizacao = "Rio de Janeiro, RJ",
            DataOperacao = DateTime.Now
        };

        var usina = TestDataBuilder.Usinas.CreateValid(1, "UTE001", "Usina Original");
        var usinaDto = new UsinaDto { Id = 1, Nome = "Usina Atualizada", Codigo = "UTE001" };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);
        _mockRepository.Setup(r => r.CodigoExisteAsync(updateDto.Codigo, 1)).ReturnsAsync(false);
        _mockMapper.Setup(m => m.Map(updateDto, usina)).Returns(usina);
        _mockRepository.Setup(r => r.UpdateAsync(usina)).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<UsinaDto>(usina)).Returns(usinaDto);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Nome.Should().Be("Usina Atualizada");
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarNotFound_QuandoUsinaNaoExiste()
    {
        // Arrange
        var updateDto = new UpdateUsinaDto { Codigo = "UTE001", Nome = "Usina" };

        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Usina?)null);

        // Act
        var result = await _service.UpdateAsync(999, updateDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("não foi encontrad");
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usina>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_DeveRetornarConflict_QuandoCodigoJaExisteEmOutraUsina()
    {
        // Arrange
        var updateDto = new UpdateUsinaDto { Codigo = "UTE002", Nome = "Usina" };
        var usina = TestDataBuilder.Usinas.CreateValid(1, "UTE001", "Usina 1");

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usina);
        _mockRepository.Setup(r => r.CodigoExisteAsync("UTE002", 1)).ReturnsAsync(true);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Já existe outra usina com o código");
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usina>()), Times.Never);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_DeveRetornarSucesso_QuandoUsinaExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFailure_QuandoUsinaNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.ExistsAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("não foi encontrad");
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    #endregion

    #region CodigoExisteAsync Tests

    [Fact]
    public async Task CodigoExisteAsync_DeveRetornarTrue_QuandoCodigoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.CodigoExisteAsync("UTE001", null)).ReturnsAsync(true);

        // Act
        var result = await _service.CodigoExisteAsync("UTE001");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CodigoExisteAsync_DeveRetornarFalse_QuandoCodigoNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.CodigoExisteAsync("NOVO001", null)).ReturnsAsync(false);

        // Act
        var result = await _service.CodigoExisteAsync("NOVO001");

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}

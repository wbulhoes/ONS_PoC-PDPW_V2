using Moq;
using PDPW.Application.DTOs.Carga;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using Xunit;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para CargaService
/// </summary>
public class CargaServiceTests
{
    private readonly Mock<ICargaRepository> _repositoryMock;
    private readonly CargaService _service;

    public CargaServiceTests()
    {
        _repositoryMock = new Mock<ICargaRepository>();
        _service = new CargaService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaDeCargas()
    {
        // Arrange
        var cargas = new List<Carga>
        {
            new() { Id = 1, SubsistemaId = "SE", CargaMWmed = 100, CargaVerificada = 95, PrevisaoCarga = 105, Ativo = true },
            new() { Id = 2, SubsistemaId = "NE", CargaMWmed = 80, CargaVerificada = 82, PrevisaoCarga = 85, Ativo = true }
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(cargas);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdValido_DeveRetornarCarga()
    {
        // Arrange
        var carga = new Carga
        {
            Id = 1,
            SubsistemaId = "SE",
            CargaMWmed = 100,
            CargaVerificada = 95,
            PrevisaoCarga = 105,
            Ativo = true
        };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(carga);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("SE", result.SubsistemaId);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ComIdInvalido_DeveRetornarNull()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Carga?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ComDadosValidos_DeveCriarCarga()
    {
        // Arrange
        var dto = new CreateCargaDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaId = "SE",
            CargaMWmed = 100,
            CargaVerificada = 95,
            PrevisaoCarga = 105
        };

        var cargaCriada = new Carga
        {
            Id = 1,
            DataReferencia = dto.DataReferencia,
            SubsistemaId = dto.SubsistemaId,
            CargaMWmed = dto.CargaMWmed,
            CargaVerificada = dto.CargaVerificada,
            PrevisaoCarga = dto.PrevisaoCarga,
            Ativo = true
        };

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Carga>())).ReturnsAsync(cargaCriada);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("SE", result.SubsistemaId);
        Assert.Equal(100, result.CargaMWmed);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Carga>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ComIdValido_DeveAtualizarCarga()
    {
        // Arrange
        var cargaExistente = new Carga
        {
            Id = 1,
            SubsistemaId = "SE",
            CargaMWmed = 100,
            CargaVerificada = 95,
            PrevisaoCarga = 105,
            Ativo = true
        };

        var dto = new UpdateCargaDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaId = "SE",
            CargaMWmed = 110,
            CargaVerificada = 105,
            PrevisaoCarga = 115
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(cargaExistente);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Carga>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(110, result.CargaMWmed);
        Assert.Equal(105, result.CargaVerificada);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Carga>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ComIdInvalido_DeveLancarException()
    {
        // Arrange
        var dto = new UpdateCargaDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaId = "SE",
            CargaMWmed = 110,
            CargaVerificada = 105,
            PrevisaoCarga = 115
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Carga?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, dto));
        _repositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Carga>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ComIdValido_DeveRetornarTrue()
    {
        // Arrange
        var carga = new Carga { Id = 1, SubsistemaId = "SE", CargaMWmed = 100, Ativo = true };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(carga);
        _repositoryMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        Assert.True(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(1), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ComIdInvalido_DeveRetornarFalse()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Carga?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        Assert.False(result);
        _repositoryMock.Verify(r => r.GetByIdAsync(999), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetBySubsistemaAsync_DeveRetornarCargasDoSubsistema()
    {
        // Arrange
        var cargas = new List<Carga>
        {
            new() { Id = 1, SubsistemaId = "SE", CargaMWmed = 100, Ativo = true },
            new() { Id = 2, SubsistemaId = "SE", CargaMWmed = 110, Ativo = true }
        };
        _repositoryMock.Setup(r => r.GetBySubsistemaAsync("SE")).ReturnsAsync(cargas);

        // Act
        var result = await _service.GetBySubsistemaAsync("SE");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, c => Assert.Equal("SE", c.SubsistemaId));
        _repositoryMock.Verify(r => r.GetBySubsistemaAsync("SE"), Times.Once);
    }

    [Fact]
    public async Task GetByPeriodoAsync_DeveRetornarCargasNoPeriodo()
    {
        // Arrange
        var dataInicio = new DateTime(2025, 1, 1);
        var dataFim = new DateTime(2025, 1, 31);
        var cargas = new List<Carga>
        {
            new() { Id = 1, DataReferencia = new DateTime(2025, 1, 15), SubsistemaId = "SE", CargaMWmed = 100, Ativo = true }
        };
        _repositoryMock.Setup(r => r.GetByPeriodoAsync(dataInicio, dataFim)).ReturnsAsync(cargas);

        // Act
        var result = await _service.GetByPeriodoAsync(dataInicio, dataFim);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        _repositoryMock.Verify(r => r.GetByPeriodoAsync(dataInicio, dataFim), Times.Once);
    }
}

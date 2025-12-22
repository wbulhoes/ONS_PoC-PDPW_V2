using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PDPW.Application.DTOs.Intercambio;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para IntercambioService
/// </summary>
public class IntercambioServiceTests
{
    private readonly Mock<IIntercambioRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<IntercambioService>> _mockLogger;
    private readonly IntercambioService _service;

    public IntercambioServiceTests()
    {
        _mockRepository = new Mock<IIntercambioRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<IntercambioService>>();
        _service = new IntercambioService(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoExistemIntercambios()
    {
        // Arrange
        var intercambios = new List<Intercambio>
        {
            new() { Id = 1, SubsistemaOrigem = "SE", SubsistemaDestino = "S", EnergiaIntercambiada = 500, Ativo = true },
            new() { Id = 2, SubsistemaOrigem = "NE", SubsistemaDestino = "SE", EnergiaIntercambiada = 300, Ativo = true }
        };

        var intercambiosDto = intercambios.Select(i => new IntercambioDto
        {
            Id = i.Id,
            SubsistemaOrigem = i.SubsistemaOrigem,
            SubsistemaDestino = i.SubsistemaDestino,
            EnergiaIntercambiada = i.EnergiaIntercambiada
        }).ToList();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(intercambios);
        _mockMapper.Setup(m => m.Map<IEnumerable<IntercambioDto>>(intercambios)).Returns(intercambiosDto);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().SubsistemaOrigem.Should().Be("SE");
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoExistemIntercambios()
    {
        // Arrange
        var intercambiosVazio = new List<Intercambio>();
        var intercambiosDtoVazio = new List<IntercambioDto>();

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(intercambiosVazio);
        _mockMapper.Setup(m => m.Map<IEnumerable<IntercambioDto>>(intercambiosVazio)).Returns(intercambiosDtoVazio);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_DeveRetornarIntercambio_QuandoExiste()
    {
        // Arrange
        var intercambio = new Intercambio
        {
            Id = 1,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500,
            Ativo = true
        };

        var intercambioDto = new IntercambioDto
        {
            Id = 1,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(intercambio);
        _mockMapper.Setup(m => m.Map<IntercambioDto>(intercambio)).Returns(intercambioDto);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.SubsistemaOrigem.Should().Be("SE");
        result.SubsistemaDestino.Should().Be("S");
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Intercambio?)null);

        // Act
        var result = await _service.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region CreateAsync Tests

    [Fact]
    public async Task CreateAsync_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var createDto = new CreateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500,
            Observacoes = "Intercâmbio normal"
        };

        var intercambio = new Intercambio
        {
            Id = 1,
            DataReferencia = createDto.DataReferencia,
            SubsistemaOrigem = createDto.SubsistemaOrigem,
            SubsistemaDestino = createDto.SubsistemaDestino,
            EnergiaIntercambiada = createDto.EnergiaIntercambiada,
            Ativo = true
        };

        var intercambioDto = new IntercambioDto
        {
            Id = 1,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500
        };

        _mockMapper.Setup(m => m.Map<Intercambio>(createDto)).Returns(intercambio);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Intercambio>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<IntercambioDto>(intercambio)).Returns(intercambioDto);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.SubsistemaOrigem.Should().Be("SE");
        result.SubsistemaDestino.Should().Be("S");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Intercambio>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarException_QuandoSubsistemaOrigemVazio()
    {
        // Arrange
        var createDto = new CreateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarException_QuandoSubsistemaDestinoVazio()
    {
        // Arrange
        var createDto = new CreateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "",
            EnergiaIntercambiada = 500
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarException_QuandoOrigemIgualDestino()
    {
        // Arrange
        var createDto = new CreateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "SE",
            EnergiaIntercambiada = 500
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        exception.Message.Should().Contain("origem");
        exception.Message.Should().Contain("destino");
        exception.Message.Should().Contain("diferente");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var updateDto = new UpdateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "NE",
            EnergiaIntercambiada = 600,
            Observacoes = "Intercâmbio atualizado"
        };

        var intercambio = new Intercambio
        {
            Id = 1,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500,
            Ativo = true
        };

        var intercambioDto = new IntercambioDto
        {
            Id = 1,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "NE",
            EnergiaIntercambiada = 600
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(intercambio);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Intercambio>())).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<IntercambioDto>(intercambio)).Returns(intercambioDto);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.SubsistemaDestino.Should().Be("NE");
        result.EnergiaIntercambiada.Should().Be(600);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Intercambio>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarException_QuandoIntercambioNaoExiste()
    {
        // Arrange
        var updateDto = new UpdateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "S",
            EnergiaIntercambiada = 500
        };

        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Intercambio?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, updateDto));
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarException_QuandoOrigemIgualDestino()
    {
        // Arrange
        var updateDto = new UpdateIntercambioDto
        {
            DataReferencia = DateTime.Today,
            SubsistemaOrigem = "SE",
            SubsistemaDestino = "SE",
            EnergiaIntercambiada = 500
        };

        var intercambio = new Intercambio { Id = 1, SubsistemaOrigem = "SE", SubsistemaDestino = "S", Ativo = true };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(intercambio);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateAsync(1, updateDto));
        exception.Message.Should().Contain("origem");
        exception.Message.Should().Contain("destino");
        exception.Message.Should().Contain("diferente");
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_DeveRetornarTrue_QuandoIntercambioExiste()
    {
        // Arrange
        var intercambio = new Intercambio { Id = 1, SubsistemaOrigem = "SE", SubsistemaDestino = "S", Ativo = true };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(intercambio);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Intercambio>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Intercambio>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalse_QuandoIntercambioNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Intercambio?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Intercambio>()), Times.Never);
    }

    #endregion

    #region GetByPeriodoAsync Tests

    [Fact]
    public async Task GetByPeriodoAsync_DeveRetornarIntercambios_QuandoExistemNoPeriodo()
    {
        // Arrange
        var dataInicio = new DateTime(2025, 1, 1);
        var dataFim = new DateTime(2025, 1, 31);

        var intercambios = new List<Intercambio>
        {
            new() { Id = 1, DataReferencia = new DateTime(2025, 1, 15), SubsistemaOrigem = "SE", SubsistemaDestino = "S", Ativo = true }
        };

        var intercambiosDto = intercambios.Select(i => new IntercambioDto
        {
            Id = i.Id,
            DataReferencia = i.DataReferencia,
            SubsistemaOrigem = i.SubsistemaOrigem,
            SubsistemaDestino = i.SubsistemaDestino
        }).ToList();

        _mockRepository.Setup(r => r.GetByPeriodoAsync(dataInicio, dataFim)).ReturnsAsync(intercambios);
        _mockMapper.Setup(m => m.Map<IEnumerable<IntercambioDto>>(intercambios)).Returns(intercambiosDto);

        // Act
        var result = await _service.GetByPeriodoAsync(dataInicio, dataFim);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().DataReferencia.Should().BeBefore(dataFim);
        result.First().DataReferencia.Should().BeOnOrAfter(dataInicio);
    }

    #endregion
}

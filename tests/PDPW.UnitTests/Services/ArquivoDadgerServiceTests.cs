using FluentAssertions;
using Moq;
using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Application.Services;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.UnitTests.Services;

/// <summary>
/// Testes unitários para ArquivoDadgerService
/// </summary>
public class ArquivoDadgerServiceTests
{
    private readonly Mock<IArquivoDadgerRepository> _mockRepository;
    private readonly Mock<ISemanaPMORepository> _mockSemanaPMORepository;
    private readonly ArquivoDadgerService _service;

    public ArquivoDadgerServiceTests()
    {
        _mockRepository = new Mock<IArquivoDadgerRepository>();
        _mockSemanaPMORepository = new Mock<ISemanaPMORepository>();
        _service = new ArquivoDadgerService(
            _mockRepository.Object,
            _mockSemanaPMORepository.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_DeveRetornarSucesso_QuandoExistemArquivos()
    {
        // Arrange
        var arquivos = new List<ArquivoDadger>
        {
            new() { Id = 1, NomeArquivo = "dadger_2025_01.dat", SemanaPMOId = 1, Ativo = true },
            new() { Id = 2, NomeArquivo = "dadger_2025_02.dat", SemanaPMOId = 2, Ativo = true }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(arquivos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().NomeArquivo.Should().Be("dadger_2025_01.dat");
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarListaVazia_QuandoNaoExistemArquivos()
    {
        // Arrange
        var arquivosVazio = new List<ArquivoDadger>();
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(arquivosVazio);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_DeveRetornarArquivo_QuandoExiste()
    {
        // Arrange
        var arquivo = new ArquivoDadger
        {
            Id = 1,
            NomeArquivo = "dadger_2025_01.dat",
            SemanaPMOId = 1,
            Ativo = true
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(arquivo);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.NomeArquivo.Should().Be("dadger_2025_01.dat");
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ArquivoDadger?)null);

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
        var createDto = new CreateArquivoDadgerDto
        {
            NomeArquivo = "dadger_2025_01.dat",
            CaminhoArquivo = "/uploads/2025/dadger_2025_01.dat",
            DataImportacao = DateTime.Now,
            SemanaPMOId = 1,
            Observacoes = "Arquivo importado com sucesso"
        };

        var semanaPMO = new SemanaPMO { Id = 1, Numero = 1, Ano = 2025, Ativo = true };
        var arquivo = new ArquivoDadger
        {
            Id = 1,
            NomeArquivo = createDto.NomeArquivo,
            CaminhoArquivo = createDto.CaminhoArquivo,
            SemanaPMOId = createDto.SemanaPMOId,
            Ativo = true
        };

        _mockSemanaPMORepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(semanaPMO);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<ArquivoDadger>())).ReturnsAsync(arquivo);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.NomeArquivo.Should().Be("dadger_2025_01.dat");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<ArquivoDadger>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarException_QuandoNomeArquivoVazio()
    {
        // Arrange
        var createDto = new CreateArquivoDadgerDto
        {
            NomeArquivo = "",
            SemanaPMOId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<ArquivoDadger>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_DeveLancarException_QuandoSemanaPMONaoExiste()
    {
        // Arrange
        var createDto = new CreateArquivoDadgerDto
        {
            NomeArquivo = "dadger.dat",
            SemanaPMOId = 999
        };

        _mockSemanaPMORepository.Setup(r => r.ObterPorIdAsync(999)).ReturnsAsync((SemanaPMO?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(createDto));
        exception.Message.Should().Contain("Semana PMO");
        exception.Message.Should().Contain("não encontrada");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<ArquivoDadger>()), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_DeveRetornarSucesso_QuandoDadosSaoValidos()
    {
        // Arrange
        var updateDto = new UpdateArquivoDadgerDto
        {
            NomeArquivo = "dadger_atualizado.dat",
            CaminhoArquivo = "/uploads/2025/dadger_atualizado.dat",
            DataImportacao = DateTime.Now,
            SemanaPMOId = 1,
            Processado = true,
            DataProcessamento = DateTime.Now
        };

        var arquivo = new ArquivoDadger
        {
            Id = 1,
            NomeArquivo = "dadger_original.dat",
            SemanaPMOId = 1,
            Ativo = true
        };

        var semanaPMO = new SemanaPMO { Id = 1, Numero = 1, Ano = 2025, Ativo = true };

        var arquivoDto = new ArquivoDadgerDto
        {
            Id = 1,
            NomeArquivo = "dadger_atualizado.dat",
            SemanaPMOId = 1
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(arquivo);
        _mockSemanaPMORepository.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(semanaPMO);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<ArquivoDadger>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.UpdateAsync(1, updateDto);

        // Assert
        result.Should().NotBeNull();
        result.NomeArquivo.Should().Be("dadger_atualizado.dat");
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ArquivoDadger>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarException_QuandoArquivoNaoExiste()
    {
        // Arrange
        var updateDto = new UpdateArquivoDadgerDto
        {
            NomeArquivo = "dadger.dat",
            SemanaPMOId = 1
        };

        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ArquivoDadger?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(999, updateDto));
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ArquivoDadger>()), Times.Never);
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_DeveRetornarTrue_QuandoArquivoExiste()
    {
        // Arrange
        var arquivo = new ArquivoDadger { Id = 1, NomeArquivo = "dadger.dat", Ativo = true };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(arquivo);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveRetornarFalse_QuandoArquivoNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ArquivoDadger?)null);

        // Act
        var result = await _service.DeleteAsync(999);

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    #endregion

    #region MarcarComoProcessadoAsync Tests

    [Fact]
    public async Task MarcarComoProcessadoAsync_DeveRetornarSucesso_QuandoArquivoExiste()
    {
        // Arrange
        var arquivo = new ArquivoDadger
        {
            Id = 1,
            NomeArquivo = "dadger.dat",
            Processado = false,
            Ativo = true
        };

        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(arquivo);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<ArquivoDadger>())).Returns(Task.CompletedTask);

        // Act
        var result = await _service.MarcarComoProcessadoAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Processado.Should().BeTrue();
        arquivo.Processado.Should().BeTrue();
        arquivo.DataProcessamento.Should().NotBeNull();
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ArquivoDadger>()), Times.Once);
    }

    [Fact]
    public async Task MarcarComoProcessadoAsync_DeveLancarException_QuandoArquivoNaoExiste()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ArquivoDadger?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.MarcarComoProcessadoAsync(999));
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<ArquivoDadger>()), Times.Never);
    }

    #endregion

    #region GetBySemanaPMOAsync Tests

    [Fact]
    public async Task GetBySemanaPMOAsync_DeveRetornarArquivos_QuandoExistem()
    {
        // Arrange
        var arquivos = new List<ArquivoDadger>
        {
            new() { Id = 1, NomeArquivo = "dadger_01.dat", SemanaPMOId = 1, Ativo = true },
            new() { Id = 2, NomeArquivo = "dadger_02.dat", SemanaPMOId = 1, Ativo = true }
        };

        _mockRepository.Setup(r => r.GetBySemanaPMOAsync(1)).ReturnsAsync(arquivos);

        // Act
        var result = await _service.GetBySemanaPMOAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.All(a => a.SemanaPMOId == 1).Should().BeTrue();
    }

    #endregion
}

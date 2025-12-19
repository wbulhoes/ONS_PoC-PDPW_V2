using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using PDPW.Application.DTOs.Usina;
using PDPW.IntegrationTests.Fixtures;

namespace PDPW.IntegrationTests.Controllers;

/// <summary>
/// Testes de integração para UsinasController
/// </summary>
public class UsinasControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public UsinasControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_DeveRetornar200_ComListaVazia()
    {
        // Act
        var response = await _client.GetAsync("/api/usinas");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var usinas = await response.Content.ReadFromJsonAsync<List<UsinaDto>>();
        usinas.Should().NotBeNull();
        usinas.Should().BeEmpty();
    }

    [Fact]
    public async Task Create_DeveRetornar201_QuandoDadosSaoValidos()
    {
        // Arrange
        var createDto = new CreateUsinaDto
        {
            Codigo = "UTE-TEST-001",
            Nome = "Usina de Teste Integração",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000,
            Localizacao = "São Paulo, SP",
            DataOperacao = DateTime.Now.AddYears(-5)
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/usinas", createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        
        var usina = await response.Content.ReadFromJsonAsync<UsinaDto>();
        usina.Should().NotBeNull();
        usina!.Codigo.Should().Be("UTE-TEST-001");
        usina.Nome.Should().Be("Usina de Teste Integração");
    }

    [Fact]
    public async Task Create_DeveRetornar409_QuandoCodigoJaExiste()
    {
        // Arrange - Criar primeira usina
        var createDto1 = new CreateUsinaDto
        {
            Codigo = "UTE-DUP-001",
            Nome = "Usina Original",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000,
            Localizacao = "São Paulo, SP",
            DataOperacao = DateTime.Now
        };

        await _client.PostAsJsonAsync("/api/usinas", createDto1);

        // Act - Tentar criar com mesmo código
        var createDto2 = new CreateUsinaDto
        {
            Codigo = "UTE-DUP-001", // Mesmo código
            Nome = "Usina Duplicada",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1500,
            Localizacao = "Rio de Janeiro, RJ",
            DataOperacao = DateTime.Now
        };

        var response = await _client.PostAsJsonAsync("/api/usinas", createDto2);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetById_DeveRetornar404_QuandoUsinaNaoExiste()
    {
        // Act
        var response = await _client.GetAsync("/api/usinas/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_DeveRetornar200_QuandoDadosSaoValidos()
    {
        // Arrange - Criar usina
        var createDto = new CreateUsinaDto
        {
            Codigo = "UTE-UPDATE-001",
            Nome = "Usina Para Atualizar",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000,
            Localizacao = "São Paulo, SP",
            DataOperacao = DateTime.Now
        };

        var createResponse = await _client.PostAsJsonAsync("/api/usinas", createDto);
        var usinaCriada = await createResponse.Content.ReadFromJsonAsync<UsinaDto>();

        // Act - Atualizar usina
        var updateDto = new UpdateUsinaDto
        {
            Codigo = "UTE-UPDATE-001",
            Nome = "Usina Atualizada",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1500,
            Localizacao = "Rio de Janeiro, RJ",
            DataOperacao = DateTime.Now
        };

        var updateResponse = await _client.PutAsJsonAsync($"/api/usinas/{usinaCriada!.Id}", updateDto);

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var usinaAtualizada = await updateResponse.Content.ReadFromJsonAsync<UsinaDto>();
        usinaAtualizada.Should().NotBeNull();
        usinaAtualizada!.Nome.Should().Be("Usina Atualizada");
        usinaAtualizada.CapacidadeInstalada.Should().Be(1500);
    }

    [Fact]
    public async Task Delete_DeveRetornar204_QuandoUsinaExiste()
    {
        // Arrange - Criar usina
        var createDto = new CreateUsinaDto
        {
            Codigo = "UTE-DELETE-001",
            Nome = "Usina Para Deletar",
            TipoUsinaId = 1,
            EmpresaId = 1,
            CapacidadeInstalada = 1000,
            Localizacao = "São Paulo, SP",
            DataOperacao = DateTime.Now
        };

        var createResponse = await _client.PostAsJsonAsync("/api/usinas", createDto);
        var usina = await createResponse.Content.ReadFromJsonAsync<UsinaDto>();

        // Act - Deletar usina
        var deleteResponse = await _client.DeleteAsync($"/api/usinas/{usina!.Id}");

        // Assert
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar que foi deletada
        var getResponse = await _client.GetAsync($"/api/usinas/{usina.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_DeveRetornar404_QuandoUsinaNaoExiste()
    {
        // Act
        var response = await _client.DeleteAsync("/api/usinas/99999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}

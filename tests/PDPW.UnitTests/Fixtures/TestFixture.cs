using Microsoft.EntityFrameworkCore;
using PDPW.Infrastructure.Data;

namespace PDPW.UnitTests.Fixtures;

/// <summary>
/// Fixture base para testes unitários com DbContext InMemory
/// </summary>
public class TestFixture : IDisposable
{
    public PdpwDbContext DbContext { get; private set; }

    public TestFixture()
    {
        var options = new DbContextOptionsBuilder<PdpwDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        DbContext = new PdpwDbContext(options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext?.Database.EnsureDeleted();
        DbContext?.Dispose();
    }
}

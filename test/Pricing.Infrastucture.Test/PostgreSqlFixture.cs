using Pricing.Infrastructure;
using Testcontainers.PostgreSql;

namespace Pricing.Infrastucture.Test;

public class PostgreSqlFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();

    public IDbConnectionFactory ConnectionFactory;
    
    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        ConnectionFactory = new NpgsqlConnectionFactory(_postgreSqlContainer.GetConnectionString());

        await new DatabaseInitializer(ConnectionFactory).InitialyzeAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.DisposeAsync();
    }
}
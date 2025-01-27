using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Connect4.Infrastructure.UnitTests;

/// <summary>
/// Base class for repository tests.
/// Creates a new in-memory database for each test.
/// </summary>
public class RepositoryTest
{
    public DatabaseContext Context { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryTest"/> class.
    /// </summary>
    public RepositoryTest()
    {
        var connectionStringBuilder = new SqliteConnectionStringBuilder()
        {
            Cache = SqliteCacheMode.Private,
            DataSource = $"Connect4TestDb.{Guid.NewGuid()}.sqlite3",
            Mode = SqliteOpenMode.Memory
        };

        var keepAliveConnection = new SqliteConnection(connectionStringBuilder.ConnectionString);
        keepAliveConnection.Open();

        var dbOptions = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlite(keepAliveConnection)
            .EnableSensitiveDataLogging(true)
            .Options;

        Context = new DatabaseContext(dbOptions);
        Context.Database.EnsureCreated();
    }
}
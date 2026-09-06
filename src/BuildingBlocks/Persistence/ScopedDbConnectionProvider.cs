using System.Collections.Concurrent;
using System.Data.Common;
using FSH.Framework.Shared.Persistence;
using Microsoft.Data.SqlClient;
using Npgsql;
using Pgvector.Npgsql;

namespace FSH.Framework.Persistence;

/// <summary>
/// Default <see cref="IScopedDbConnectionProvider"/>: caches one connection per connection string
/// for the lifetime of the DI scope and disposes them all at scope end.
///
/// Connections are handed to EF Core unopened and un-owned, so EF keeps its usual open/close
/// behaviour around each operation and the underlying pooled connection is returned as normal.
/// What changes is only the identity of the <see cref="DbConnection"/> object: contexts in the same
/// scope now share one, which is what makes a cross-context transaction possible.
///
/// Postgres connections come from process-lifetime data sources with the pgvector plugin
/// registered, so vector-typed parameters (e.g. module embeddings) serialize. Modules without
/// vector columns are unaffected.
/// </summary>
public sealed class ScopedDbConnectionProvider : IScopedDbConnectionProvider, IAsyncDisposable, IDisposable
{
    private static readonly ConcurrentDictionary<string, NpgsqlDataSource> DataSources = new(StringComparer.Ordinal);

    private readonly Dictionary<string, DbConnection> _connections = new(StringComparer.Ordinal);
    private bool _disposed;

    public DbConnection GetConnection(string dbProvider, string connectionString)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(dbProvider);
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_connections.TryGetValue(connectionString, out var existing))
        {
            return existing;
        }

        DbConnection connection = dbProvider.ToUpperInvariant() switch
        {
            DbProviders.PostgreSQL => DataSources
                .GetOrAdd(connectionString, cs =>
                {
                    var builder = new NpgsqlDataSourceBuilder(cs);
#pragma warning disable NPG9001 // pgvector 0.3.x's supported registration path; revisit on upgrade
                    builder.AddTypeInfoResolverFactory(new VectorTypeInfoResolverFactory());
#pragma warning restore NPG9001
                    return builder.Build();
                })
                .CreateConnection(),
            DbProviders.MSSQL => new SqlConnection(connectionString),
            _ => throw new InvalidOperationException($"Database Provider {dbProvider} is not supported."),
        };

        _connections[connectionString] = connection;
        return connection;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        foreach (var connection in _connections.Values)
        {
            connection.Dispose();
        }

        _connections.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        foreach (var connection in _connections.Values)
        {
            await connection.DisposeAsync().ConfigureAwait(false);
        }

        _connections.Clear();
    }
}

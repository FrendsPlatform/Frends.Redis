namespace Frends.Redis.GetValue.Helpers;

using System;
using GetKeys.Definitions;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Azure.Identity;
using StackExchange.Redis;

internal static class ConnectionHandler
{
    internal static async Task<ConnectionMultiplexer> GetConnectionAsync(Connection connection)
    {
        return connection.ConnectionMethod switch
        {
            ConnectionMethod.SimpleConnectionString => await SimpleConnection(connection.ConnectionString),
            ConnectionMethod.MicrosoftEntraId => await MicrosoftEntraIdConnection(connection.ConnectionString),
            _ => throw new ArgumentOutOfRangeException(nameof(connection), connection.ConnectionMethod, null)
        };
    }

    private static async Task<ConnectionMultiplexer> SimpleConnection(string connectionString)
    {
        return await ConnectionMultiplexer.ConnectAsync(connectionString);
    }

    [ExcludeFromCodeCoverage(Justification = "Unable to test EntraId due to lack of testing environment")]
    private static async Task<ConnectionMultiplexer> MicrosoftEntraIdConnection(string connectionString)
    {
        var options = ConfigurationOptions.Parse(connectionString);
        await options.ConfigureForAzureWithTokenCredentialAsync(new DefaultAzureCredential());
        return await ConnectionMultiplexer.ConnectAsync(options).ConfigureAwait(false);
    }
}

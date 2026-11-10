using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Frends.Redis.DeleteKey.Definitions;
using Frends.Redis.DeleteKey.Helpers;

namespace Frends.Redis.DeleteKey;

/// <summary>
/// Main class of the Task to delete Key from Redis database.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task do delete key from Redis database.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.DeleteKey).
    /// </summary>
    /// <param name="input">Data to set.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, Error Error { string Message, object AdditionalInfo } }</returns>
    public static async Task<Result> DeleteKey(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ValidationHandler.Run(input, connection);
            cancellationToken.ThrowIfCancellationRequested();

            await using var redis = await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
            var db = redis.GetDatabase();
            var deleted = await db.KeyDeleteAsync(input.Key);
            return new Result(deleted);
        }
        catch (Exception ex)
        {
            return ex.Handle(options);
        }
    }
}

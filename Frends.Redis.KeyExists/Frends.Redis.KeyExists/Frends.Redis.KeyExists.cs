namespace Frends.Redis.KeyExists;

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Definitions;
using Helpers;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task to check if Redis key exists.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.KeyExists).
    /// </summary>
    /// <param name="input">Data to check.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Object { bool Success, Error error }.</returns>
    public static async Task<Result> KeyExists(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await using var redis = await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
            var db = redis.GetDatabase();
            var exists = await db.KeyExistsAsync(input.Key);
            return new Result(exists);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }
}

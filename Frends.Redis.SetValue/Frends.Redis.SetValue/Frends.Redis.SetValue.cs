using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Frends.Redis.SetValue.Definitions;
using Frends.Redis.SetValue.Helpers;
using StackExchange.Redis;

namespace Frends.Redis.SetValue;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.SetValue).
    /// </summary>
    /// <param name="input">Data to set.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Object { bool Success, Error error }.</returns>
    public static async Task<Result> SetValue(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken = default)
    {
        try
        {
            input.Validate();
            connection.Validate();
            await using var redis =
                await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
            var db = redis.GetDatabase();
            cancellationToken.ThrowIfCancellationRequested();
            await db.SetValue(input).ConfigureAwait(false);

            return new Result();
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }
}

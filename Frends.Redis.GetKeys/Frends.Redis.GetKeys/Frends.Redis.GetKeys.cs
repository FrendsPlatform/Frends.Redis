namespace Frends.Redis.GetKeys;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Frends.Redis.GetKeys.Definitions;
using Frends.Redis.GetKeys.Helpers;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task to get keys from Redis.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.GetKeys).
    /// </summary>
    /// <param name="input">Key selection settings.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>object { bool Success, List&lt;string&gt; Keys, object Error { string Message, object AdditionalInfo } }</returns>
    public static async Task<Result> GetKeys(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            connection.Validate();
            cancellationToken.ThrowIfCancellationRequested();
            await using var redis = await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
            List<string> keys = [];
            var server = redis.GetServer(connection.ConnectionString);

            foreach (var key in server.Keys(pattern: input.KeyPattern))
            {
                cancellationToken.ThrowIfCancellationRequested();
                keys.Add((string)key);
            }

            return new Result(keys);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }
}

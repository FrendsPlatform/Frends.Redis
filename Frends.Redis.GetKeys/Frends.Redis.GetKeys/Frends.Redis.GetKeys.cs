using Frends.Redis.GetValue.Helpers;

namespace Frends.Redis.GetKeys;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Definitions;
using Helpers;
using StackExchange.Redis;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task to get keys from Redis.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.GetKeys).
    /// </summary>
    /// <param name="options">Exception settings.</param>
    /// <param name="connection">Connection info.</param>
    /// <returns>Object { bool Success, Error error, List<string> Keys keys }.</returns>
    public static async Task<Result> GetKeys([PropertyTab] Options options, [PropertyTab] Connection connection)
    {
        try
        {
            connection.Validate();
            await using var redis = await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
            List<string> keys = [];
            var server = redis.GetServer(connection.ConnectionString);
            keys.AddRange(server.Keys().Select(key => (string)key));
            return new Result(keys);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }
}

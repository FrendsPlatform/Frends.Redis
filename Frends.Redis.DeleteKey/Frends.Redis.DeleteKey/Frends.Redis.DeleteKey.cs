namespace Frends.Redis.DeleteKey;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Definitions;
using Helpers;
using StackExchange.Redis;

/// <summary>
/// Main class of the Task to delete Key from Redis database.
/// </summary>
public static class Redis
{
    private static IConnectionMultiplexer redis;

    /// <summary>
    /// This is Task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.DeleteKey).
    /// </summary>
    /// <param name="input">Data to set.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="connection">Connection info.</param>
    /// <returns>Object { bool Success, Error error }.</returns>
    public static async Task<Result> DeleteKey([PropertyTab] Input input, [PropertyTab] Options options, [PropertyTab] Connection connection)
    {
        try
        {
            redis = await ConnectionMultiplexer.ConnectAsync(connection.ConnectionString);
            var db = redis.GetDatabase();
            var deleted = await db.KeyDeleteAsync(input.Key);
            return new Result(deleted);
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
        finally
        {
            redis?.Dispose();
        }
    }
}
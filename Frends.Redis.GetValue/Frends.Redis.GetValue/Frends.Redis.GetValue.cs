namespace Frends.Redis.GetValue;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Definitions;
using Helpers;
using StackExchange.Redis;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    private static IConnectionMultiplexer redis;

    /// <summary>
    /// This is Task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.SetValue).
    /// </summary>
    /// <param name="input">Data to set.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="connection">Connection info.</param>
    /// <returns>Object { string Value, bool Success, Error error }.</returns>
    public static async Task<Result> GetValue([PropertyTab] Input input, [PropertyTab] Options options, [PropertyTab] Connection connection)
    {
        try
        {
            redis = await ConnectionMultiplexer.ConnectAsync(connection.ConnectionString);
            var db = redis.GetDatabase();
            var value = await db.StringGetAsync(input.Key);

            if (value.HasValue) return new Result(value.ToString());
            throw new Exception("Value not found");
        }
        catch (Exception ex)
        {
            return ErrorHandler.Handle(ex, options.ThrowErrorOnFailure, options.ErrorMessageOnFailure);
        }
    }
}
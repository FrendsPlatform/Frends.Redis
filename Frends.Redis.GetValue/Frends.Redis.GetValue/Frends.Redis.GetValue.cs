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
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.GetValue).
    /// </summary>
    /// <param name="input">Data to set.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="connection">Connection info.</param>
    /// <returns>Object { string Value, bool Success, Error error }.</returns>
    public static async Task<Result> GetValue([PropertyTab] Input input, [PropertyTab] Options options,
        [PropertyTab] Connection connection)
    {
        try
        {
            if (string.IsNullOrEmpty(input.Key))
                throw new ArgumentException("Key cannot be null or empty", nameof(input.Key));
            redis = await ConnectionMultiplexer.ConnectAsync(connection.ConnectionString);
            var db = redis.GetDatabase();
            var value = await db.StringGetAsync(input.Key);
            // TODO same ValueType needed + get values same as in tests for in SetValue task
            // OR TRY THIS
            // RedisType type = await db.KeyTypeAsync(key);
            // return type switch
            // {
            //     RedisType.String => (string)await db.StringGetAsync(key),
            //     RedisType.Hash => (await db.HashGetAllAsync(key)).ToDictionary(x => x.Name.ToString(),
            //         x => x.Value.ToString()),
            //     RedisType.List => (await db.ListRangeAsync(key)).Select(x => x.ToString()).ToList(),
            //     RedisType.Set => (await db.SetMembersAsync(key)).Select(x => x.ToString()).ToList(),
            //     RedisType.None => null, // Key does not exist
            //     _ => throw new NotSupportedException($"Redis type {type} is not handled.")
            // };


            if (value.HasValue) return new Result(value.ToString());

            throw new Exception("Value not found");
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


namespace Frends.Redis.GetValue;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
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
    public static async Task<Result> GetValue(
        [PropertyTab] Input input,
        [PropertyTab] Options options,
        [PropertyTab] Connection connection)
    {
        try
        {
            if (string.IsNullOrEmpty(input.Key))
            {
                throw new ArgumentException("Key cannot be null or empty", nameof(input));
            }

            redis = await ConnectionMultiplexer.ConnectAsync(connection.ConnectionString);
            var db = redis.GetDatabase();
            RedisType type = await db.KeyTypeAsync(input.Key);

            var result = new Result { Success = true };

            switch (type)
            {
                case RedisType.String:
                    result.StringValue = await db.StringGetAsync(input.Key);
                    break;
                case RedisType.List:
                    var val = await db.ListRangeAsync(input.Key);
                    result.ListValue = val.Select(x => x.ToString()).ToList();
                    break;
                case RedisType.Set:
                    var setVal = await db.SetMembersAsync(input.Key);
                    result.ListValue = setVal.Select(x => x.ToString()).ToList();
                    break;
                case RedisType.Hash:
                    var hashVal = await db.HashGetAllAsync(input.Key);
                    result.DictionaryValue = hashVal.ToDictionary(x => x.Name.ToString(), x => x.Value.ToString());
                    break;
                case RedisType.None:
                    throw new Exception("Value not found");
                default:
                    throw new NotSupportedException($"Redis type {type} is not handled.");
            }

            return result;
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

using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Frends.Redis.GetValue.Definitions;
using Frends.Redis.GetValue.Helpers;
using StackExchange.Redis;

namespace Frends.Redis.GetValue;

/// <summary>
/// Main class of the Task.
/// </summary>
public static class Redis
{
    /// <summary>
    /// This is Task.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.Redis.GetValue).
    /// </summary>
    /// <param name="input">Data to get.</param>
    /// <param name="connection">Connection info.</param>
    /// <param name="options">Exception settings.</param>
    /// <param name="cancellationToken">A cancellation token provided by Frends Platform.</param>
    /// <returns>Object { string StringValue, List&lt;string&gt; ListValue, Dictionary&lt;string, string&gt; DictionaryValue, bool Success, Error Error }.</returns>
    public static async Task<Result> GetValue(
        [PropertyTab] Input input,
        [PropertyTab] Connection connection,
        [PropertyTab] Options options,
        CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            ValidationHandler.Run(input, connection);

            await using var redis = await ConnectionHandler.GetConnectionAsync(connection).ConfigureAwait(false);
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
            return ex.Handle(options);
        }
    }
}

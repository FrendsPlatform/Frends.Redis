using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frends.Redis.SetValue.Definitions;
using StackExchange.Redis;

namespace Frends.Redis.SetValue.Helpers;

internal static class Setter
{
    internal static async Task SetValue(this IDatabase db, Input input)
    {
        var expiry = input.ExpiryInSeconds.HasValue
            ? TimeSpan.FromSeconds(input.ExpiryInSeconds.Value)
            : (TimeSpan?)null;

        switch (input.ValueType)
        {
            case Definitions.ValueType.String:
            case Definitions.ValueType.Json:
                await db.StringSetAsync(input.Key, input.StringValue, expiry);

                break;

            case Definitions.ValueType.Hash:
                await db.SetHash(input.Key, input.HashValue, expiry);

                break;
            case Definitions.ValueType.List:
                await db.SetList(input.Key, input.ListValue, expiry);

                break;

            case Definitions.ValueType.Set:
                await db.SetSet(input.Key, input.ListValue, expiry);

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(input), input.ValueType, null);
        }
    }

    private static async Task SetHash(
        this IDatabase db,
        string key,
        Dictionary<string, string> values,
        TimeSpan? expiry)
    {
        var entries = values.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
        await db.HashSetAsync(key, entries);
        if (expiry.HasValue) await db.KeyExpireAsync(key, expiry);
    }

    private static async Task SetList(this IDatabase db, string key, List<string> values, TimeSpan? expiry)
    {
        var redisValues = values.Select(x => (RedisValue)x).ToArray();
        await db.ListRightPushAsync(key, redisValues);
        if (expiry.HasValue) await db.KeyExpireAsync(key, expiry);
    }

    private static async Task SetSet(this IDatabase db, string key, List<string> values, TimeSpan? expiry)
    {
        var redisValues = values.Select(x => (RedisValue)x).ToArray();
        await db.SetAddAsync(key, redisValues);
        if (expiry.HasValue) await db.KeyExpireAsync(key, expiry);
    }
}

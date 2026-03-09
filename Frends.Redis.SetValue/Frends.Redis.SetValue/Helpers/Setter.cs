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
        var transaction = db.CreateTransaction();
        if (input.CollectionOperation == Operation.Overwrite)
            transaction.KeyDeleteAsync(input.Key).ConfigureAwait(false);

        switch (input.ValueType)
        {
            case Definitions.ValueType.String:
                transaction.StringSetAsync(input.Key, input.StringValue);
                break;
            case Definitions.ValueType.Hash:
                var hashEntries = input.HashValue.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
                transaction.HashSetAsync(input.Key, hashEntries);
                break;
            case Definitions.ValueType.List:
                var listEntries = input.ListValue.Select(x => (RedisValue)x).ToArray();
                transaction.ListLeftPushAsync(input.Key, listEntries);
                break;
            case Definitions.ValueType.Set:
                var setValues = input.ListValue.Select(x => (RedisValue)x).ToArray();
                transaction.SetAddAsync(input.Key, setValues);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(input), input.ValueType, null);
        }

        var expiry = input.ExpiryInSeconds.HasValue
            ? TimeSpan.FromSeconds(input.ExpiryInSeconds.Value)
            : (TimeSpan?)null;
        if (expiry.HasValue) transaction.KeyExpireAsync(input.Key, expiry);
        await transaction.ExecuteAsync().ConfigureAwait(false);
    }
}

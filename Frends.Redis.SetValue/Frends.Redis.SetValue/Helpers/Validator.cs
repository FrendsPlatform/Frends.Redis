using System;
using Frends.Redis.SetValue.Definitions;
using ValueType = Frends.Redis.SetValue.Definitions.ValueType;

namespace Frends.Redis.SetValue.Helpers;

internal static class Validator
{
    internal static void Validate(this Input input)
    {
        var isValid = input.ValueType switch
        {
            ValueType.String => input.StringValue is not null,
            ValueType.Hash => input.HashValue is not null,
            ValueType.List or ValueType.Set => input.ListValue is not null,
            _ => throw new ArgumentOutOfRangeException(nameof(input), input.ValueType, null),
        };

        if (!isValid) throw new Exception("You must provide a value for the given type.");
    }

    internal static void Validate(this Connection connection)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connection));
    }
}

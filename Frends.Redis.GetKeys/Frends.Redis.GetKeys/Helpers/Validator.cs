namespace Frends.Redis.GetKeys.Helpers;

using System;
using Definitions;

internal static class Validator
{
    internal static void Validate(this Connection connection)
    {
        if (string.IsNullOrEmpty(connection.ConnectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty", nameof(connection));
        }
    }
}

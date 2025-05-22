namespace Frends.Redis.GetKeys.Definitions;

using System.Collections.Generic;

/// <summary>
/// Result class defines structure of return data from GetKeys task
/// </summary>
public class Result
{
    internal Result(List<string> keys = null, bool success = true, Error error = null)
    {
        Success = success;
        Error = error;
        Keys = keys ?? [];
    }

    /// <summary>
    /// Success flag
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Error info
    /// </summary>
    public Error Error { get; private set; }

    /// <summary>
    /// Found keys
    /// </summary>
    public List<string> Keys { get; private set; }
}

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
    public bool Success { get; set; }

    /// <summary>
    /// Error info
    /// </summary>
    /// <example>null</example>
    public Error Error { get; set; }

    /// <summary>
    /// Found keys
    /// </summary>
    /// <example>["customer:42", "customer:43"]</example>
    public List<string> Keys { get; set; }
}

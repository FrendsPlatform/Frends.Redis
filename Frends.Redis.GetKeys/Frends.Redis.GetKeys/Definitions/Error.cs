namespace Frends.Redis.GetKeys.Definitions;

/// <summary>
/// Object with error information
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    /// <example>Unable to connect to the Redis server</example>
    public string Message { get; set; }

    /// <summary>
    /// Object with additional information
    /// </summary>
    /// <example>Redis connection was refused</example>
    public object AdditionalInfo { get; set; }
}

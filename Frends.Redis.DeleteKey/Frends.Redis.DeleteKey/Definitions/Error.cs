namespace Frends.Redis.DeleteKey.Definitions;

/// <summary>
/// Object with error information
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    /// <example>Failed to delete Redis key: server unavailable</example>
    public string Message { get; set; }

    /// <summary>
    /// Object with additional information
    /// </summary>
    /// <example>RedisConnectionException: No connection is available to service this operation</example>
    public dynamic AdditionalInfo { get; set; }
}

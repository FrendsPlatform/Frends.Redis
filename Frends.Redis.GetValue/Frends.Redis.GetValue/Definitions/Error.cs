namespace Frends.Redis.GetValue.Definitions;

/// <summary>
/// Object with error information
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    /// <example>Unable to retrieve Redis key 'customer:42'.</example>
    public string Message { get; set; }

    /// <summary>
    /// Object with additional information
    /// </summary>
    /// <example>RedisConnectionException: No connection is available to service this operation.</example>
    public dynamic AdditionalInfo { get; set; }
}

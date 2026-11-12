namespace Frends.Redis.KeyExists.Definitions;

/// <summary>
/// Object with error information
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    /// <example>Task failed during execution</example>
    public string Message { get; set; }

    /// <summary>
    /// Object with additional information
    /// </summary>
    /// <example>object { Exception Exception }</example>
    public object AdditionalInfo { get; set; }
}
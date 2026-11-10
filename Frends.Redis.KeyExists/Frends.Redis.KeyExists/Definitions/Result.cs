namespace Frends.Redis.KeyExists.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Success flag
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Error info
    /// </summary>
    /// <example>object { string Message, object AdditionalInfo }</example>
    public Error Error { get; set; }
}

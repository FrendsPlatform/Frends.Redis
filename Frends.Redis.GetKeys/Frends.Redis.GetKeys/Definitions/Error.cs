namespace Frends.Redis.GetKeys.Definitions;

/// <summary>
/// Object with error information
/// </summary>
public class Error
{
    /// <summary>
    /// Error message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Object with additional information
    /// </summary>
    public object AdditionalInfo { get; set; }
}
namespace Frends.Redis.DeleteKey.Definitions;

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
    public dynamic AdditionalInfo { get; set; }
}
namespace Frends.Redis.GetValue.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    internal Result(string value = null, bool success = true, Error error = null)
    {
        Value = value;
        Success = success;
        Error = error;
    }

    /// <summary>
    /// Returned value
    /// </summary>
    /// <example>"foobar"</example>
    public string? Value { get; private set; }

    /// <summary>
    /// Success flag
    /// </summary>
    /// <example>true</example>
    public bool Success { get; private set; }

    /// <summary>
    /// Error info
    /// </summary>
    public Error Error { get; private set; }
}

namespace Frends.Redis.DeleteKey.Definitions;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    internal Result(bool success = true, Error error = null)
    {
        Success = success;
        Error = error;
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
}

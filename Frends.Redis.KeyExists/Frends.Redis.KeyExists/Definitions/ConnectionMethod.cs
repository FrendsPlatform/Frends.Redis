namespace Frends.Redis.KeyExists.Definitions;

/// <summary>
/// Method to use to connect to Redis
/// </summary>
public enum ConnectionMethod
{
    /// <summary>
    /// Using basic connection string with all required credentials
    /// </summary>
    SimpleConnectionString = 1,

    /// <summary>
    /// Using Microsoft Entra ID to get token authentication
    /// </summary>
    MicrosoftEntraId = 2,
}

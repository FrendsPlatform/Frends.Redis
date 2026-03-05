namespace Frends.Redis.GetValue.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Connection info
/// </summary>
public class Connection
{
    /// <summary>
    /// Method to use to connect to Redis
    /// </summary>
    /// <example>ConnectionMethod.SimpleConnectionString</example>
    [DefaultValue(ConnectionMethod.SimpleConnectionString)]
    public ConnectionMethod ConnectionMethod { get; set; } = ConnectionMethod.SimpleConnectionString;

    /// <summary>
    /// Connection string to Redis.
    /// </summary>
    /// <example>your-redis-name.redis.cache.windows.net:6380,ssl=true</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string ConnectionString { get; set; }
}

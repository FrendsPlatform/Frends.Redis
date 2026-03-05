namespace Frends.Redis.GetKeys.Definitions;

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
    /// <example>127.0.0.1:6379</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string ConnectionString { get; set; }
}

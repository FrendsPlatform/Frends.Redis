namespace Frends.Redis.KeyExists.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Connection info
/// </summary>
public class Connection
{
    /// <summary>
    /// Connection string to Redis.
    /// </summary>
    /// <example>127.0.0.1:6379</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string ConnectionString { get; set; }
}
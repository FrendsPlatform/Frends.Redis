namespace Frends.Redis.DeleteKey.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data to use in Redis
/// </summary>
public class Input
{
    /// <summary>
    /// Key to delete
    /// </summary>
    /// <example>Foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Key { get; set; }
}
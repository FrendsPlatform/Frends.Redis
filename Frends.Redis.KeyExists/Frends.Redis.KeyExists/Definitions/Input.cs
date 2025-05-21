namespace Frends.Redis.KeyExists.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data to use in Redis
/// </summary>
public class Input
{
    /// <summary>
    /// Key to set
    /// </summary>
    /// <example>Foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Key { get; set; }
}
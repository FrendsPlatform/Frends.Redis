namespace Frends.Redis.GetValue.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data to use in Redis
/// </summary>
public class Input
{
    /// <summary>
    /// Key to get
    /// </summary>
    /// <example>Foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Key { get; set; }
}
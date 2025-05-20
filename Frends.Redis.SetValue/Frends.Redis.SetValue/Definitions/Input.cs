namespace Frends.Redis.SetValue.Definitions;

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

    /// <summary>
    /// Value to set
    /// </summary>
    /// <example>Bar</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Value { get; set; }

    /// <summary>
    /// Time to live in seconds. Leave empty for infinite.
    /// </summary>
    /// <example>3600</example>
    [DisplayFormat(DataFormatString = "Text")]
    public int? ExpiryInSeconds { get; set; }
}
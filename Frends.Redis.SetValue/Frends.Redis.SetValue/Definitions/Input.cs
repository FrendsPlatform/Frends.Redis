using System.Collections.Generic;
using System.ComponentModel;

namespace Frends.Redis.SetValue.Definitions;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Data to use in Redis
/// </summary>
public class Input
{
    /// <summary>
    /// Type of the value we want to save in Redis
    /// </summary>
    /// <example>ValueType.String</example>
    [DefaultValue(ValueType.String)]
    public ValueType ValueType { get; set; } = ValueType.String;

    /// <summary>
    /// Key to set
    /// </summary>
    /// <example>Foo</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Key { get; set; }

    /// <summary>
    /// String value to set
    /// </summary>
    /// <example>Bar</example>
    [DisplayFormat(DataFormatString = "Text")]
    [UIHint(nameof(ValueType), "", ValueType.String, ValueType.Json)]
    public string StringValue { get; set; }

    /// <summary>
    /// List value to set
    /// </summary>
    /// <example>["Foo", "Bar"]</example>
    [UIHint(nameof(ValueType), "", ValueType.List, ValueType.Set)]
    public List<string> ListValue { get; set; }

    /// <summary>
    /// Dictionary value to set
    /// </summary>
    /// <example>{ { "Foo", "Bar" }, { "Moo", "Baz" } }</example>
    [UIHint(nameof(ValueType), "", ValueType.Hash)]
    public Dictionary<string, string> HashValue { get; set; }

    /// <summary>
    /// Time to live in seconds. Leave empty for infinite.
    /// </summary>
    /// <example>3600</example>
    [DisplayFormat(DataFormatString = "Text")]
    public int? ExpiryInSeconds { get; set; }
}

namespace Frends.Redis.GetKeys.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Key selection settings.
/// </summary>
public class Input
{
    /// <summary>
    /// Pattern used to match Redis keys.
    /// Use default "*" to match all keys, or specify a pattern like "customer:*" to match keys starting with "customer:".
    /// </summary>
    /// <example>customer:*</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("*")]
    public string KeyPattern { get; set; } = "*";
}

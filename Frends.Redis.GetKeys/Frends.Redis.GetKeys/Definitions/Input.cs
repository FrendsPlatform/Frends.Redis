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
    /// </summary>
    /// <example>customer:*</example>
    [DisplayFormat(DataFormatString = "Text")]
    [DefaultValue("*")]
    public string KeyPattern { get; set; } = "*";
}

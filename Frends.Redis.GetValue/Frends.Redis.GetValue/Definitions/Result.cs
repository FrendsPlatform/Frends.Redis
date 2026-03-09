namespace Frends.Redis.GetValue.Definitions;

using System.Collections.Generic;

/// <summary>
/// Result class usually contains properties of the return object.
/// </summary>
public class Result
{
    /// <summary>
    /// Returned string value
    /// </summary>
    /// <example>"foobar"</example>
    public string StringValue { get; set; }

    /// <summary>
    /// Returned list value
    /// </summary>
    /// <example>["Foo", "Bar"]</example>
    public List<string> ListValue { get; set; }

    /// <summary>
    /// Returned dictionary value
    /// </summary>
    /// <example>{ { "Foo", "Bar" }, { "Moo", "Baz" } }</example>
    public Dictionary<string, string> DictionaryValue { get; set; }

    /// <summary>
    /// Success flag
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Error info
    /// </summary>
    public Error Error { get; set; }
}

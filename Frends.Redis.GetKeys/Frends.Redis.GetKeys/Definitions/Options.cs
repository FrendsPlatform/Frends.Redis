namespace Frends.Redis.GetKeys.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options class with settings for specific behaviour of task
/// </summary>
public class Options
{
    /// <summary>
    /// True: Throw an exception.
    /// False: Error will be added to the Result.Error.AdditionalInformation list instead of stopping the Task.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Message what will be used when error occurs
    /// </summary>
    /// <example>Task failed during execution</example>
    [DefaultValue("Error getting Keys from Redis")]
    public string ErrorMessageOnFailure { get; set; } = "Error getting Keys from Redis";
}
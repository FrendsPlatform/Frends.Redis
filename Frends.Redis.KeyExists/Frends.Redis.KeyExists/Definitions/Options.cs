﻿namespace Frends.Redis.KeyExists.Definitions;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Options class usually contains parameters that are required.
/// </summary>
public class Options
{
    /// <summary>
    /// True: Throw an exception.
    /// False: Error will be added to the Result.Error.AdditionalInfo list instead of stopping the Task.
    /// </summary>
    /// <example>true</example>
    [DefaultValue(true)]
    public bool ThrowErrorOnFailure { get; set; } = true;

    /// <summary>
    /// Message what will be used when an error occurs
    /// </summary>
    /// <example>Task failed during execution</example>
    [DefaultValue("Error checking key from Redis.")]
    public string ErrorMessageOnFailure { get; set; } = "Error checking key from Redis.";
}
namespace Frends.Redis.SetValue.Definitions;

/// <summary>
/// Operation that will be executed in case of collection-like type
/// </summary>
public enum Operation
{
#pragma warning disable SA1602 // enum self-explanatory
    Append = 1,
    Overwrite = 2,
#pragma warning disable SA1602 // enum self-explanatory
}

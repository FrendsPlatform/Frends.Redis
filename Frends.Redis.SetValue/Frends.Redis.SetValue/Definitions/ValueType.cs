namespace Frends.Redis.SetValue.Definitions;

/// <summary>
/// Type of value we want to save in Redis
/// </summary>
public enum ValueType
{
#pragma warning disable SA1602 // enum self-explanatory
    String = 1,
    Hash = 2,
    Json = 3,
    List = 4,
    Set = 5,
#pragma warning restore SA1602
}

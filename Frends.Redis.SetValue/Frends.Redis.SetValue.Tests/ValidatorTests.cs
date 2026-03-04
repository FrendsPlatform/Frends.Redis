using System;
using Frends.Redis.SetValue.Definitions;
using Frends.Redis.SetValue.Helpers;
using NUnit.Framework;
using ValueType = Frends.Redis.SetValue.Definitions.ValueType;

namespace Frends.Redis.SetValue.Tests;

public class ValidatorTests
{
    [TestCase(ValueType.String)]
    [TestCase(ValueType.Hash)]
    [TestCase(ValueType.Json)]
    [TestCase(ValueType.Set)]
    [TestCase(ValueType.List)]
    public void ValidateInputThrowsProperly(ValueType valueType)
    {
        // Arrange
        Input input = new()
        {
            ValueType = ValueType.String,
            StringValue = null,
            ListValue = null,
            HashValue = null,
        };

        var ex = Assert.Throws<Exception>(() => input.Validate());
        Assert.That(ex.Message, Does.Contain("You must provide a value for the given type."));
    }
}

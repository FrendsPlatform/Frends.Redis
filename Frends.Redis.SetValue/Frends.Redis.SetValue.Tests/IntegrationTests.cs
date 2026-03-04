using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Frends.Redis.SetValue.Definitions;
using NUnit.Framework;
using StackExchange.Redis;
using Testcontainers.Redis;
using ValueType = Frends.Redis.SetValue.Definitions.ValueType;

namespace Frends.Redis.SetValue.Tests;

[TestFixture]
public class IntegrationTests
{
    private RedisContainer redisContainer;
    private IConnectionMultiplexer redis;
    private Connection connection;
    private Input input;
    private Options options;
    private string connectionString;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .WithPortBinding(6379, true)
            .Build();

        await redisContainer.StartAsync();
        connectionString = redisContainer.GetConnectionString();

        redis = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions
        {
            EndPoints =
            {
                connectionString,
            },
            AllowAdmin = true,
        });
    }

    [SetUp]
    public void Setup()
    {
        connection = new Connection
        {
            ConnectionString = connectionString,
        };

        input = new Input
        {
            Key = "test-key",
            StringValue = "test-value",
        };

        options = new Options
        {
            ThrowErrorOnFailure = true,
        };

        var server = redis.GetServer(connectionString);
        server.FlushAllDatabases();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        if (redis != null)
        {
            await redis.CloseAsync();
            redis.Dispose();
        }

        if (redisContainer != null)
        {
            await redisContainer.DisposeAsync();
        }
    }

    [Test]
    public async Task SetStringValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        var savedValue = await db.StringGetAsync(input.Key);
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(input.StringValue, Is.EqualTo(savedValue.ToString()));
    }

    [Test]
    public async Task SetHashValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        var testVal = new Dictionary<string, string>
        {
            {
                "Foo", "Bar"
            },
            {
                "Moo", "Baz"
            },
        };
        var entries = testVal.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
        input.HashValue = testVal;
        input.ValueType = ValueType.Hash;

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        var savedValue = await db.HashGetAllAsync(input.Key);
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(entries, Is.EquivalentTo(savedValue));
    }

    [Test]
    public async Task SetListValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        var testVal = new List<string>
        {
            "foo",
            "foo",
        };
        var entries = testVal.Select(x => (RedisValue)x).ToArray();
        input.ListValue = testVal;
        input.ValueType = ValueType.List;

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        var savedValue = await db.ListRangeAsync(input.Key);
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(entries, Is.EquivalentTo(savedValue));
    }

    [Test]
    public async Task SetSetValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        var testVal = new List<string>
        {
            "foo",
            "foo",
        };
        var entries = testVal.Distinct().Select(x => (RedisValue)x).ToArray();

        input.ListValue = testVal;
        input.ValueType = ValueType.Set;

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        var savedValue = await db.SetMembersAsync(input.Key);
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(entries, Is.EquivalentTo(savedValue));
    }

    [Test]
    public async Task ValueExpiresAfterProvidedTime()
    {
        // Arrange
        var db = redis.GetDatabase();
        input.ExpiryInSeconds = 2;

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        await Task.Delay(TimeSpan.FromSeconds(3));
        var savedValue = await db.StringGetAsync(input.Key);

        Assert.That(result.Success, Is.True);
        Assert.IsFalse(savedValue.HasValue);
    }

    [Test]
    public async Task InvalidConnectionStringThrowsException()
    {
        // Arrange
        var db = redis.GetDatabase();
        connection.ConnectionString = "invalid-connection-string";
        var savedValue = await db.StringGetAsync(input.Key);

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await Redis.SetValue(input, connection, options));
        Assert.IsFalse(savedValue.HasValue);
    }

    [Test]
    public void CustomErrorMessageIsUsed()
    {
        // Arrange
        const string message = "Custom error message";
        connection.ConnectionString = "invalid-connection-string";
        options.ErrorMessageOnFailure = message;

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(async () => await Redis.SetValue(input, connection, options));
        Assert.That(ex.Message, Does.Contain(message));
    }

    [Test]
    public void ErrorIsThrownOnFailure()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";
        options.ThrowErrorOnFailure = true;

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await Redis.SetValue(input, connection, options));
    }

    [Test]
    public async Task ResultWithErrorIsReturnedOnFailure()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";
        options.ThrowErrorOnFailure = false;

        // Act
        var result = await Redis.SetValue(input, connection, options);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
    }
}

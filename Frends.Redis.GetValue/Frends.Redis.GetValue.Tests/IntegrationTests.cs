namespace Frends.Redis.GetValue.Tests;

using System;
using System.Threading.Tasks;
using Definitions;
using System.Collections.Generic;
using System.Linq;
using StackExchange.Redis;
using Testcontainers.Redis;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    private const string Key = "test-key";
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
            EndPoints = { connectionString },
            AllowAdmin = true,
        });
    }

    [SetUp]
    public void Setup()
    {
        connection = new Connection { ConnectionString = connectionString, };

        input = new Input { Key = Key, };

        options = new Options { ThrowErrorOnFailure = true, };

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
    public async Task GetStringValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        const string value = "test-value";

        await db.StringSetAsync(Key, value);

        // Act
        var result = await Redis.GetValue(input, options, connection);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.ListValue, Is.Null);
        Assert.That(result.DictionaryValue, Is.Null);
        Assert.That(result.StringValue, Is.EqualTo(value));
    }

    [Test]
    public async Task GetDictionaryValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        var value = new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } };
        var entries = value.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
        await db.HashSetAsync(Key, entries);

        // Act
        var result = await Redis.GetValue(input, options, connection);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.StringValue, Is.Null);
        Assert.That(result.ListValue, Is.Null);
        Assert.That(result.DictionaryValue, Is.EquivalentTo(value));
    }

    [Test]
    public async Task GetListValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        var value = new List<string> { "key1", "key2" };
        var entries = value.Select(x => (RedisValue)x).ToArray();
        await db.ListRightPushAsync(Key, entries);

        // Act
        var result = await Redis.GetValue(input, options, connection);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.StringValue, Is.Null);
        Assert.That(result.DictionaryValue, Is.Null);
        Assert.That(result.ListValue, Is.EquivalentTo(value));
    }

    [Test]
    public void InvalidInputThrowsException()
    {
        // Arrange
        input.Key = null;

        // Act
        AsyncTestDelegate action = async () => await Redis.GetValue(input, options, connection);

        // Assert
        Assert.ThrowsAsync<Exception>(action);
    }

    [Test]
    public void InvalidConnectionStringThrowsException()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";

        // Act
        AsyncTestDelegate action = async () => await Redis.GetValue(input, options, connection);

        // Assert
        Assert.ThrowsAsync<Exception>(action);
    }

    [Test]
    public void CustomErrorMessageIsUsed()
    {
        // Arrange
        const string message = "Custom error message";
        connection.ConnectionString = "invalid-connection-string";
        options.ErrorMessageOnFailure = message;

        // Act
        AsyncTestDelegate action = async () => await Redis.GetValue(input, options, connection);

        // Assert
        var ex = Assert.ThrowsAsync<Exception>(action);
        Assert.That(ex.Message, Does.Contain(message));
    }

    [Test]
    public void ErrorIsThrownOnFailure()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";
        options.ThrowErrorOnFailure = true;

        // Act
        AsyncTestDelegate action = async () => await Redis.GetValue(input, options, connection);

        // Assert
        Assert.ThrowsAsync<Exception>(action);
    }

    [Test]
    public async Task ResultWithErrorIsReturnedOnFailure()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";
        options.ThrowErrorOnFailure = false;

        // Act
        var result = await Redis.GetValue(input, options, connection);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
    }
}

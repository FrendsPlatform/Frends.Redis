namespace Frends.Redis.GetValue.Tests;

using System;
using System.Threading.Tasks;
using Definitions;
using StackExchange.Redis;
using Testcontainers.Redis;
using NUnit.Framework;

[TestFixture]
public class IntegrationTests
{
    private const string Value = "test-value";
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
        connection = new Connection
        {
            ConnectionString = connectionString,
        };

        input = new Input
        {
            Key = Key,
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
    public async Task GetValueIsSuccessful()
    {
        // Arrange
        var db = redis.GetDatabase();
        await db.StringSetAsync(Key, Value);

        // Act
        var result = await Redis.GetValue(input, options, connection);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.Value, Is.EqualTo(Value));
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
    public void DefaultErrorMessageIsUsed()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";

        // Act
        AsyncTestDelegate action = async () => await Redis.GetValue(input, options, connection);

        // Assert
        var ex = Assert.ThrowsAsync<Exception>(action);
        Assert.That(ex.Message, Does.Contain("Default error message"));
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
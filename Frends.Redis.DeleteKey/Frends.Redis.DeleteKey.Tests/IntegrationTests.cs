namespace Frends.Redis.DeleteKey.Tests;

using System;
using System.Threading.Tasks;
using Definitions;
using StackExchange.Redis;
using Testcontainers.Redis;
using NUnit.Framework;

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
            Key = "test-key",
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
    public async Task DeleteKeyReturnsTrueWhenKeyExisted()
    {
        // Arrange
        var db = redis.GetDatabase();
        var testKey = input.Key;
        const string testValue = "test-value";
        await db.StringSetAsync(testKey, testValue);

        // Act
        var result = await Redis.DeleteKey(input, options, connection);

        // Assert
        var savedValue = await db.StringGetAsync(input.Key);
        Assert.IsTrue(result.Success);
        Assert.IsNull(result.Error);
        Assert.IsFalse(savedValue.HasValue);
    }

    [Test]
    public async Task DeleteKeyReturnsFalseWhenKeyDidNotExist()
    {
        // Act
        var result = await Redis.DeleteKey(input, options, connection);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNull(result.Error);
    }

    [Test]
    public async Task InvalidConnectionStringThrowsException()
    {
        // Arrange
        var db = redis.GetDatabase();
        connection.ConnectionString = "invalid-connection-string";

        // Act
        AsyncTestDelegate action = async () => await Redis.DeleteKey(input, options, connection);

        // Assert
        var savedValue = await db.StringGetAsync(input.Key);
        Assert.ThrowsAsync<Exception>(action);
        Assert.IsFalse(savedValue.HasValue);
    }

    [Test]
    public void DefaultErrorMessageIsUsed()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";

        // Act
        AsyncTestDelegate action = async () => await Redis.DeleteKey(input, options, connection);

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
        AsyncTestDelegate action = async () => await Redis.DeleteKey(input, options, connection);

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
        AsyncTestDelegate action = async () => await Redis.DeleteKey(input, options, connection);

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
        var result = await Redis.DeleteKey(input, options, connection);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
    }
}
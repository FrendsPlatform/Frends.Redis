namespace Frends.Redis.GetKeys.Tests;

using System;
using System.Threading;
using System.Threading.Tasks;
using Frends.Redis.GetKeys.Definitions;
using NUnit.Framework;
using StackExchange.Redis;
using Testcontainers.Redis;

[TestFixture]
public class IntegrationTests
{
    private RedisContainer redisContainer;
    private IConnectionMultiplexer redis;
    private Input input;
    private Connection connection;
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
        input = new Input();
        connection = new Connection
        {
            ConnectionString = connectionString,
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
    public async Task GetKeysIsSuccessfulWhenNoKeyExists()
    {
        // Act
        var result = await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.Keys, Is.Empty);
    }

    [Test]
    public async Task GetKeysIsSuccessfulWhenKeysExist()
    {
        // Arrange
        var db = redis.GetDatabase();
        await db.StringSetAsync("key1", "value1");
        await db.StringSetAsync("key2", "value2");
        var expectedKeys = new[] { "key1", "key2" };

        // Act
        var result = await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.IsNull(result.Error);
        Assert.That(result.Keys, Is.EquivalentTo(expectedKeys));
    }

    [Test]
    public async Task GetKeysOnlyReturnsMatchingKeys()
    {
        // Arrange
        var db = redis.GetDatabase();
        await db.StringSetAsync("customer:42", "value1");
        await db.StringSetAsync("order:43", "value2");
        input.KeyPattern = "customer:*";

        // Act
        var result = await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.That(result.Keys, Is.EquivalentTo(new[] { "customer:42" }));
    }

    [Test]
    public void InvalidConnectionStringThrowsException()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";

        // Act
        AsyncTestDelegate action = async () => await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<RedisConnectionException>(action);
    }

    [Test]
    public void CustomErrorMessageIsUsed()
    {
        // Arrange
        const string message = "Custom error message";
        connection.ConnectionString = "invalid-connection-string";
        options.ErrorMessageOnFailure = message;

        // Act
        AsyncTestDelegate action = async () => await Redis.GetKeys(input, connection, options, CancellationToken.None);

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
        AsyncTestDelegate action = async () => await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.ThrowsAsync<RedisConnectionException>(action);
    }

    [Test]
    public async Task ResultWithErrorIsReturnedOnFailure()
    {
        // Arrange
        connection.ConnectionString = "invalid-connection-string";
        options.ThrowErrorOnFailure = false;

        // Act
        var result = await Redis.GetKeys(input, connection, options, CancellationToken.None);

        // Assert
        Assert.IsFalse(result.Success);
        Assert.IsNotNull(result.Error);
    }
}

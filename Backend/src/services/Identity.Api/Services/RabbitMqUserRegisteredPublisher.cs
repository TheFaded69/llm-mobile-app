using System.Text;
using System.Text.Json;
using Identity.Api.Models;
using Identity.Application.Services;
using Identity.Contracts.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Identity.Api.Services;

public class RabbitMqUserRegisteredPublisher : IUserRegisteredPublisher
{
    private readonly RabbitMqOptions _options;

    public RabbitMqUserRegisteredPublisher(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
    }

    public async Task PublishAsync(Guid userId, string email, string username, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using var connection = await factory.CreateConnectionAsync(cancellationToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(_options.Exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);

        var payload = new UserRegisteredEvent
        {
            UserId = userId,
            Email = email,
            Username = username
        };

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload));
        await channel.BasicPublishAsync(
            exchange: _options.Exchange,
            routingKey: _options.UserRegisteredRoutingKey,
            mandatory: false,
            body: body,
            cancellationToken: cancellationToken);
    }
}

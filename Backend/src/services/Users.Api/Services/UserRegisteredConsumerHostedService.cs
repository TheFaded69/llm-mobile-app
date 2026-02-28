using System.Text;
using System.Text.Json;
using Identity.Contracts.Events;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Users.Api.Models;
using Users.Domain.Enums;
using Users.Infrastructure.Repositories;

namespace Users.Api.Services;

public class UserRegisteredConsumerHostedService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly RabbitMqOptions _options;

    public UserRegisteredConsumerHostedService(IServiceProvider serviceProvider, IOptions<RabbitMqOptions> options)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(_options.Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync(_options.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(_options.Queue, _options.Exchange, _options.UserRegisteredRoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var evt = JsonSerializer.Deserialize<UserRegisteredEvent>(json);
            if (evt == null)
                return;

            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var existing = await repository.GetByEmailAsync(evt.Email, stoppingToken);
            if (existing == null)
            {
                await repository.AddUserAsync(new Users.Domain.Models.User
                {
                    Id = evt.UserId,
                    Email = evt.Email,
                    UserName = evt.Username,
                    Role = UserRole.User,
                    UserType = UserType.None
                }, stoppingToken);
            }

            await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
        };

        await channel.BasicConsumeAsync(_options.Queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}

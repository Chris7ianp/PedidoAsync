using Microsoft.Extensions.Configuration;
using PedidoAsync.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PedidoAsync.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublicarAsync<T>(T mensagem)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMq:UserName"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest"
        };

        await using var connection = await factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "pedidos.exchange",
            type: ExchangeType.Direct,
            durable: true);

        await channel.QueueDeclareAsync(
            queue: "pedidos.criados",
            durable: true,
            exclusive: false,
            autoDelete: false);

        await channel.QueueBindAsync(
            queue: "pedidos.criados",
            exchange: "pedidos.exchange",
            routingKey: "pedido.criado");

        var json = JsonSerializer.Serialize(mensagem);

        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "pedidos.exchange",
            routingKey: "pedido.criado",
            body: body);
    }
}
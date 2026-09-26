using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Events;

namespace PedidoAsync.Worker.Consumers;

public class PedidoCriadoConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PedidoCriadoConsumer> _logger;

    private IConnection? _connection;
    private IChannel? _channel;

    public PedidoCriadoConsumer(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<PedidoCriadoConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:HostName"] ?? "localhost",
            UserName = _configuration["RabbitMq:UserName"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest"
        };

        _connection = await factory.CreateConnectionAsync();

        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: "pedidos.exchange",
            type: ExchangeType.Direct,
            durable: true);

        await _channel.QueueDeclareAsync(
            queue: "pedidos.criados",
            durable: true,
            exclusive: false,
            autoDelete: false);

        await _channel.QueueBindAsync(
            queue: "pedidos.criados",
            exchange: "pedidos.exchange",
            routingKey: "pedido.criado");

        await _channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += ProcessarMensagemAsync;

        await _channel.BasicConsumeAsync(
            queue: "pedidos.criados",
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation(
            "Worker conectado ao RabbitMQ e aguardando mensagens...");

        try
        {
            await Task.Delay(
                Timeout.Infinite,
                stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation(
                "Worker está sendo encerrado.");
        }
    }

    private async Task ProcessarMensagemAsync(
        object sender,
        BasicDeliverEventArgs args)
    {
        if (_channel is null)
        {
            return;
        }

        try
        {
            var json = Encoding.UTF8.GetString(
                args.Body.ToArray());

            var evento = JsonSerializer.Deserialize<PedidoCriadoEvent>(
                json);

            if (evento is null)
            {
                throw new InvalidOperationException(
                    "Não foi possível desserializar o evento.");
            }

            _logger.LogInformation(
                "Processando pedido {PedidoId}",
                evento.PedidoId);

            using var scope = _scopeFactory.CreateScope();

            var repository = scope.ServiceProvider
                .GetRequiredService<IPedidoRepository>();

            var pedido = await repository.ObterPorIdAsync(
                evento.PedidoId);

            if (pedido is null)
            {
                _logger.LogWarning(
                    "Pedido {PedidoId} não encontrado no MongoDB.",
                    evento.PedidoId);

                await _channel.BasicAckAsync(
                    args.DeliveryTag,
                    multiple: false);

                return;
            }

            pedido.MarcarComoProcessado();

            await repository.AtualizarAsync(pedido);

            await _channel.BasicAckAsync(
                args.DeliveryTag,
                multiple: false);

            _logger.LogInformation(
                "Pedido {PedidoId} processado com sucesso.",
                evento.PedidoId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar mensagem do RabbitMQ.");

            await _channel.BasicNackAsync(
                args.DeliveryTag,
                multiple: false,
                requeue: true);
        }
    }

    public override async Task StopAsync(
        CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync();
        }

        if (_connection is not null)
        {
            await _connection.CloseAsync();
        }

        await base.StopAsync(cancellationToken);
    }
}
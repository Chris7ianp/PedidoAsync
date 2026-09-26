using PedidoAsync.Infrastructure.Data;
using PedidoAsync.Infrastructure.Extensions;
using PedidoAsync.Worker;
using PedidoAsync.Worker.Consumers;

MongoDbConfiguration.Configure();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<PedidoCriadoConsumer>();

var host = builder.Build();

host.Run();
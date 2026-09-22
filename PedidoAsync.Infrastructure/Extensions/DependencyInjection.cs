using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Infrastructure.Data;
using PedidoAsync.Infrastructure.Repositories;

namespace PedidoAsync.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var mongoSettings = new MongoDbSettings
        {
            ConnectionString = configuration["MongoDbSettings:ConnectionString"]
                ?? throw new InvalidOperationException(
                    "MongoDbSettings:ConnectionString não foi configurado."),

            DatabaseName = configuration["MongoDbSettings:DatabaseName"]
                ?? throw new InvalidOperationException(
                    "MongoDbSettings:DatabaseName não foi configurado."),

            CollectionName = configuration["MongoDbSettings:CollectionName"]
                ?? throw new InvalidOperationException(
                    "MongoDbSettings:CollectionName não foi configurado.")
        };

        services.AddSingleton(mongoSettings);

        services.AddSingleton<MongoDbContext>();

        services.AddScoped<IPedidoRepository, PedidoRepository>();

        return services;
    }
}
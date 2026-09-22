using MongoDB.Driver;
using PedidoAsync.Domain.Entities;

namespace PedidoAsync.Infrastructure.Data;

public class MongoDbContext
{
    public IMongoCollection<Pedido> Pedidos { get; }

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);

        var database = client.GetDatabase(settings.DatabaseName);

        Pedidos = database.GetCollection<Pedido>(settings.CollectionName);
    }
}
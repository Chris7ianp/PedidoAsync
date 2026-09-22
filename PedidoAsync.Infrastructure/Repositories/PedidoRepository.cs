using MongoDB.Driver;
using PedidoAsync.Application;
using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Entities;
using PedidoAsync.Infrastructure.Data;

namespace PedidoAsync.Infrastructure.Repositories;

public class PedidoRepository : IPedidoRepository
{
    private readonly IMongoCollection<Pedido> _pedidos;

    public PedidoRepository(MongoDbContext context)
    {
        _pedidos = context.Pedidos;
    }

    public async Task AdicionarAsync(Pedido pedido)
    {
        await _pedidos.InsertOneAsync(pedido);
    }

    public async Task<Pedido?> ObterPorIdAsync(Guid id)
    {
        return await _pedidos
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Pedido>> ObterTodosAsync()
    {
        return await _pedidos
            .Find(_ => true)
            .ToListAsync();
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        await _pedidos.ReplaceOneAsync(
            p => p.Id == pedido.Id,
            pedido);
    }
}
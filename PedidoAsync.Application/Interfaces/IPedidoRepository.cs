using PedidoAsync.Domain.Entities;

namespace PedidoAsync.Application
{
    public interface IPedidoRepository
    {
        Task AdicionarAsync(Pedido pedido);
        Task<Pedido?> ObterPorIdAsync(Guid id);
        Task<List<Pedido>> ObterTodosAsync();
        Task AtualizarAsync(PedidoDto pedido);
    }
}

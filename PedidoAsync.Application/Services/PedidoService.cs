using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Entities;

namespace PedidoAsync.Application
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public async Task<PedidoDto> CriarAsync(string cliente, decimal valor)
        {
            var pedido = new Pedido(cliente, valor);

            await _pedidoRepository.AdicionarAsync(pedido);

            return new PedidoDto
            {
                Id = pedido.Id,
                Cliente = cliente,
                Valor = pedido.Valor,
                Status = pedido.Status.ToString(),
                DataCriacao = pedido.DataCriacao,
                DataProcessamento = pedido.DataProcessamento
            };
        }

    }
}

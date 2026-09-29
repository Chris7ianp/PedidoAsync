using PedidoAsync.Application.Interfaces;
using PedidoAsync.Domain.Entities;
using PedidoAsync.Domain.Events;

namespace PedidoAsync.Application.Services
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        private readonly IMessagePublisher _messagePublisher;

        public PedidoService(
            IPedidoRepository pedidoRepository,
            IMessagePublisher messagePublisher)
        {
            _pedidoRepository = pedidoRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task<PedidoDto> CriarAsync(int numeroPedido, string cliente, string email, decimal valor, DateTime dataPedido)
        {
            var pedido = new Pedido(cliente, valor, numeroPedido, email, dataPedido);

            await _pedidoRepository.AdicionarAsync(pedido);

            var evento = new PedidoCriadoEvent
            {
                PedidoId = pedido.Id,
                Cliente = pedido.Cliente,
                Valor = pedido.Valor,
                DataCriacao = pedido.DataCriacao
            };

            await _messagePublisher.PublicarAsync(evento);

            return MapearParaDto(pedido);
        }

        public async Task<List<PedidoDto>> ObterTodosAsync()
        {
            var pedidos = await _pedidoRepository.ObterTodosAsync();

            return pedidos
                .Select(MapearParaDto)
                .ToList();
        }

        public async Task<PedidoDto?> ObterPorIdAsync(Guid id)
        {
            var pedido = await _pedidoRepository.ObterPorIdAsync(id);

            if (pedido is null)
            {
                return null;
            }

            return MapearParaDto(pedido);
        }

        private static PedidoDto MapearParaDto(Pedido pedido)
        {
            return new PedidoDto
            {
                Id = pedido.Id,
                NumeroPedido = pedido.NumeroPedido,
                Cliente = pedido.Cliente,
                Email = pedido.Email,
                Valor = pedido.Valor,
                DataPedido = pedido.DataPedido,
                DataCriacao = pedido.DataCriacao,
                DataProcessamento = pedido.DataProcessamento,
                Status = pedido.Status.ToString()
            };
        }

    }
}

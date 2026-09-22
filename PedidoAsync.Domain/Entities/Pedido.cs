using PedidoAsync.Domain.Enums;

namespace PedidoAsync.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public string Cliente { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public StatusPedido Status { get; set; }

        public Pedido(string cliente, decimal valor) 
        {
            Id = Guid.NewGuid();
            Cliente = cliente;
            Valor = valor;
            DataCriacao = DateTime.UtcNow;
            Status = StatusPedido.Pendente;
        }

        public void MarcarComoProcessado()
        {
            Status = StatusPedido.Processando;
            DataProcessamento = DateTime.UtcNow;
        }
    }
}

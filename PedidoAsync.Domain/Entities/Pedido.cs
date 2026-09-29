using PedidoAsync.Domain.Enums;

namespace PedidoAsync.Domain.Entities
{
    public class Pedido
    {
        public Guid Id { get; private set; }
        public int NumeroPedido { get; set; }
        public string Cliente { get; set; }
        public string Email { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public StatusPedido Status { get; set; }

        private Pedido() 
        {
            
        }

        public Pedido(string cliente, decimal valor, int pedido, string email, DateTime dataPedido) 
        {
            Id = Guid.NewGuid();
            Cliente = cliente;
            Valor = valor;
            NumeroPedido = pedido;
            Email = email;
            DataPedido = dataPedido;
            DataCriacao = DateTime.UtcNow;
            Status = StatusPedido.Pendente;
            
        }

        public void MarcarComoProcessado()
        {
            Status = StatusPedido.Processado;
            DataProcessamento = DateTime.UtcNow;
        }
    }
}
